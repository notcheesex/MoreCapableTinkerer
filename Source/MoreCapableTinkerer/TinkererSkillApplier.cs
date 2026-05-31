using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace MoreCapableTinkerer
{
    [StaticConstructorOnStartup]
    internal static class TinkererSkillLifecycleStartupHook
    {
        static TinkererSkillLifecycleStartupHook()
        {
            TinkererSkillApplier.EnsureLifecycleHooksInstalledSafely();
        }
    }

    internal static class TinkererSkillApplier
    {
        private const string HarmonyId = "cheesex.MoreCapableTinkerer.skillLifecycle";
        private const string TargetPackageId = "vanillaquestsexpanded.dronefactory";
        private const string CraftingDronePawnKindDefName = "VQE_CraftingDroneKindDef";
        private const string ConstructionSkillDefName = "Construction";
        private const string HarmonyTypeName = "HarmonyLib.Harmony";
        private const string HarmonyMethodTypeName = "HarmonyLib.HarmonyMethod";
        private const string SkillsTickIntervalMethodName = "SkillsTickInterval";
        private const string PawnSpawnSetupMethodName = "SpawnSetup";

        private static readonly HashSet<string> warnedMissingWorkTypeDefNames = new HashSet<string>();
        private static readonly HashSet<int> appliedPawnThingIds = new HashSet<int>();
        private static readonly HashSet<Pawn_SkillTracker> registeredTinkererSkillTrackers = new HashSet<Pawn_SkillTracker>(new ReferenceEqualityComparer<Pawn_SkillTracker>());
        private static bool warnedTargetInactive;
        private static bool warnedMissingPawnKind;
        private static bool warnedMissingConstructionSkill;
        private static bool warnedPawnFinderFailure;
        private static bool warnedHarmonyUnavailable;
        private static bool warnedLifecycleHookFailure;
        private static bool tickGuardPatchInstalled;
        private static bool spawnHookPatchInstalled;

        internal static void ResetPerGameTracking()
        {
            appliedPawnThingIds.Clear();
            registeredTinkererSkillTrackers.Clear();
        }

        internal static void ApplyCurrentSettingsSafely()
        {
            try
            {
                if (!EnsureLifecycleHooksInstalledSafely())
                {
                    return;
                }

                MoreCapableTinkererSettings settings = MoreCapableTinkererMod.GetSettingsForApplication();
                if (settings == null)
                {
                    Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Could not retrieve mod settings; skipping tinkerer skill apply.");
                    return;
                }

                Apply(settings);
            }
            catch (Exception ex)
            {
                Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Tinkerer skill apply failed safely. " + ex.GetType().Name + ": " + ex.Message);
            }
        }

        internal static void TryApplyToSpawnedPawn(Pawn pawn)
        {
            try
            {
                if (!EnsureLifecycleHooksInstalledSafely())
                {
                    return;
                }

                if (pawn == null || Current.Game == null || !IsTargetPackageActive(false) || !IsTargetTinkererPawn(pawn))
                {
                    return;
                }

                RegisterExistingTinkererSkillTracker(pawn);

                MoreCapableTinkererSettings settings = MoreCapableTinkererMod.GetSettingsForApplication();
                if (settings == null)
                {
                    Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Could not retrieve mod settings; skipping spawned tinkerer skill apply.");
                    return;
                }

                settings.ClampAllSkillLevels();
                Dictionary<SkillDef, int> relevantSkillLevels = GetRelevantSkillLevels(settings);
                if (relevantSkillLevels.Count == 0)
                {
                    return;
                }

                ApplySkillsToPawn(pawn, relevantSkillLevels, false);
            }
            catch (Exception ex)
            {
                Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Spawned tinkerer skill apply failed safely. " + ex.GetType().Name + ": " + ex.Message);
            }
        }

        private static void Apply(MoreCapableTinkererSettings settings)
        {
            if (settings == null || Current.Game == null)
            {
                return;
            }

            if (!IsTargetPackageActive(true))
            {
                return;
            }

            PawnKindDef targetPawnKind = DefDatabase<PawnKindDef>.GetNamedSilentFail(CraftingDronePawnKindDefName);
            if (targetPawnKind == null)
            {
                if (!warnedMissingPawnKind)
                {
                    Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Missing PawnKindDef '" + CraftingDronePawnKindDefName + "'; skipping tinkerer skill apply.");
                    warnedMissingPawnKind = true;
                }

                return;
            }

            settings.ClampAllSkillLevels();
            Dictionary<SkillDef, int> relevantSkillLevels = GetRelevantSkillLevels(settings);

            foreach (Pawn pawn in GetTinkererPawns(targetPawnKind))
            {
                RegisterExistingTinkererSkillTracker(pawn);
                if (relevantSkillLevels.Count != 0)
                {
                    ApplySkillsToPawn(pawn, relevantSkillLevels, true);
                }
            }
        }

        private static Dictionary<SkillDef, int> GetRelevantSkillLevels(MoreCapableTinkererSettings settings)
        {
            Dictionary<SkillDef, int> skillLevels = new Dictionary<SkillDef, int>();

            foreach (string workTypeDefName in TinkererWorkSettingsApplier.GetEnabledWorkTypeDefNames(settings))
            {
                WorkTypeDef workTypeDef = DefDatabase<WorkTypeDef>.GetNamedSilentFail(workTypeDefName);
                if (workTypeDef == null)
                {
                    WarnMissingWorkTypeOnce(workTypeDefName);
                    continue;
                }

                if (workTypeDef.relevantSkills == null)
                {
                    continue;
                }

                int workTypeSkillLevel = settings.GetSkillLevelForWorkTypeDefName(workTypeDefName);
                for (int i = 0; i < workTypeDef.relevantSkills.Count; i++)
                {
                    SkillDef skillDef = workTypeDef.relevantSkills[i];
                    if (skillDef != null)
                    {
                        AddSkillLevel(skillLevels, skillDef, workTypeSkillLevel);
                    }
                }
            }

            if (settings.enableRepair)
            {
                AddExplicitConstructionSkillLevel(skillLevels, settings.GetRepairSkillLevel());
            }

            if (settings.enableFixBrokenDownBuilding)
            {
                AddExplicitConstructionSkillLevel(skillLevels, settings.GetFixBrokenDownBuildingSkillLevel());
            }

            return skillLevels;
        }

        private static IEnumerable<Pawn> GetTinkererPawns(PawnKindDef targetPawnKind)
        {
            IEnumerable<Pawn> pawns;
            try
            {
                pawns = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
            }
            catch (Exception ex)
            {
                if (!warnedPawnFinderFailure)
                {
                    Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Could not enumerate alive map/caravan/transport pawns for skill apply. " + ex.GetType().Name + ": " + ex.Message);
                    warnedPawnFinderFailure = true;
                }

                yield break;
            }

            if (pawns == null)
            {
                yield break;
            }

            HashSet<Pawn> seenPawns = new HashSet<Pawn>();
            foreach (Pawn pawn in pawns)
            {
                if (pawn == null || pawn.kindDef != targetPawnKind || !seenPawns.Add(pawn))
                {
                    continue;
                }

                yield return pawn;
            }
        }

        private static void ApplySkillsToPawn(Pawn pawn, Dictionary<SkillDef, int> skillLevels, bool force)
        {
            if (pawn == null || skillLevels == null || skillLevels.Count == 0)
            {
                return;
            }

            int pawnTrackingKey = GetPawnTrackingKey(pawn);
            if (!force && appliedPawnThingIds.Contains(pawnTrackingKey))
            {
                return;
            }

            if (!TryEnsureSkillTracker(pawn))
            {
                return;
            }

            RegisterTinkererSkillTracker(pawn);

            foreach (KeyValuePair<SkillDef, int> skillLevel in skillLevels)
            {
                TryApplySkillLevel(pawn, skillLevel.Key, skillLevel.Value);
            }

            appliedPawnThingIds.Add(pawnTrackingKey);
        }

        private static void AddExplicitConstructionSkillLevel(Dictionary<SkillDef, int> skillLevels, int targetLevel)
        {
            SkillDef constructionSkillDef = DefDatabase<SkillDef>.GetNamedSilentFail(ConstructionSkillDefName);
            if (constructionSkillDef != null)
            {
                AddSkillLevel(skillLevels, constructionSkillDef, targetLevel);
            }
            else if (!warnedMissingConstructionSkill)
            {
                Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Missing SkillDef '" + ConstructionSkillDefName + "'; repair and broken-down-building skill support will be skipped.");
                warnedMissingConstructionSkill = true;
            }
        }

        private static void AddSkillLevel(Dictionary<SkillDef, int> skillLevels, SkillDef skillDef, int targetLevel)
        {
            targetLevel = MoreCapableTinkererSettings.ClampSkillLevel(targetLevel);

            int existingLevel;
            if (!skillLevels.TryGetValue(skillDef, out existingLevel) || targetLevel > existingLevel)
            {
                skillLevels[skillDef] = targetLevel;
            }
        }

        private static bool TryEnsureSkillTracker(Pawn pawn)
        {
            if (pawn.skills != null)
            {
                return true;
            }

            try
            {
                pawn.skills = new Pawn_SkillTracker(pawn);
                return pawn.skills != null;
            }
            catch (Exception ex)
            {
                Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Could not create a skill tracker for tinkerer pawn '" + pawn + "'; skipping skill apply. " + ex.GetType().Name + ": " + ex.Message);
                return false;
            }
        }

        private static void TryApplySkillLevel(Pawn pawn, SkillDef skillDef, int targetLevel)
        {
            if (skillDef == null)
            {
                return;
            }

            try
            {
                SkillRecord skillRecord = pawn.skills.GetSkill(skillDef);
                if (skillRecord == null)
                {
                    Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Tinkerer pawn '" + pawn + "' has no skill record for '" + skillDef.defName + "'; skipping that skill.");
                    return;
                }

                if (skillRecord.Level < targetLevel)
                {
                    skillRecord.Level = targetLevel;
                }
            }
            catch (Exception ex)
            {
                Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Could not set tinkerer pawn '" + pawn + "' skill '" + skillDef.defName + "'; skipping that skill. " + ex.GetType().Name + ": " + ex.Message);
            }
        }

        private static void WarnMissingWorkTypeOnce(string workTypeDefName)
        {
            if (warnedMissingWorkTypeDefNames.Add(workTypeDefName))
            {
                Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Missing WorkTypeDef '" + workTypeDefName + "'; its relevant skills will be skipped.");
            }
        }

        private static bool IsTargetPackageActive(bool warnIfInactive)
        {
            if (ModLister.GetActiveModWithIdentifier(TargetPackageId, true) != null)
            {
                return true;
            }

            if (warnIfInactive && !warnedTargetInactive)
            {
                Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Target package '" + TargetPackageId + "' is not active; skipping tinkerer skill apply.");
                warnedTargetInactive = true;
            }

            return false;
        }

        private static bool IsTargetTinkererPawn(Pawn pawn)
        {
            return pawn != null && pawn.kindDef != null && pawn.kindDef.defName == CraftingDronePawnKindDefName;
        }

        private static int GetPawnTrackingKey(Pawn pawn)
        {
            if (pawn == null)
            {
                return 0;
            }

            return pawn.thingIDNumber;
        }

        private static void RegisterExistingTinkererSkillTracker(Pawn pawn)
        {
            if (pawn == null || pawn.skills == null)
            {
                return;
            }

            RegisterTinkererSkillTracker(pawn);
        }

        private static void RegisterTinkererSkillTracker(Pawn pawn)
        {
            if (!IsTargetTinkererPawn(pawn) || pawn.skills == null)
            {
                return;
            }

            registeredTinkererSkillTrackers.Add(pawn.skills);
        }

        internal static bool EnsureLifecycleHooksInstalledSafely()
        {
            if (tickGuardPatchInstalled && spawnHookPatchInstalled)
            {
                return true;
            }

            try
            {
                Type harmonyType = GetLoadedType(HarmonyTypeName);
                Type harmonyMethodType = GetLoadedType(HarmonyMethodTypeName);
                if (harmonyType == null || harmonyMethodType == null)
                {
                    if (!warnedHarmonyUnavailable)
                    {
                        Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Harmony is not loaded; skipping tinkerer skill apply because the drone skill tick guard cannot be installed.");
                        warnedHarmonyUnavailable = true;
                    }

                    return false;
                }

                object harmony = Activator.CreateInstance(harmonyType, HarmonyId);
                MethodInfo skillsTickPrefix = typeof(TinkererSkillApplier).GetMethod("TinkererSkillsTickIntervalPrefix", BindingFlags.Static | BindingFlags.NonPublic);
                MethodInfo spawnSetupPostfix = typeof(TinkererSkillApplier).GetMethod("TinkererPawnSpawnSetupPostfix", BindingFlags.Static | BindingFlags.NonPublic);

                if (!tickGuardPatchInstalled)
                {
                    MethodInfo original = typeof(Pawn_SkillTracker).GetMethod(SkillsTickIntervalMethodName, BindingFlags.Instance | BindingFlags.Public);
                    tickGuardPatchInstalled = TryPatchWithHarmony(harmony, harmonyType, harmonyMethodType, original, skillsTickPrefix, null);
                }

                if (!spawnHookPatchInstalled)
                {
                    MethodInfo original = typeof(Pawn).GetMethod(PawnSpawnSetupMethodName, BindingFlags.Instance | BindingFlags.Public);
                    spawnHookPatchInstalled = TryPatchWithHarmony(harmony, harmonyType, harmonyMethodType, original, null, spawnSetupPostfix);
                }

                return tickGuardPatchInstalled && spawnHookPatchInstalled;
            }
            catch (Exception ex)
            {
                if (!warnedLifecycleHookFailure)
                {
                    Log.Warning(TinkererWorkSettingsApplier.LogPrefix + ": Could not install tinkerer skill lifecycle hooks; skipping skill apply to avoid unsafe drone skill ticking. " + ex.GetType().Name + ": " + ex.Message);
                    warnedLifecycleHookFailure = true;
                }

                return false;
            }
        }

        private static bool TryPatchWithHarmony(object harmony, Type harmonyType, Type harmonyMethodType, MethodInfo original, MethodInfo prefix, MethodInfo postfix)
        {
            if (harmony == null || harmonyType == null || harmonyMethodType == null || original == null)
            {
                return false;
            }

            MethodInfo patchMethod = harmonyType.GetMethod(
                "Patch",
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new Type[] { typeof(MethodBase), harmonyMethodType, harmonyMethodType, harmonyMethodType, harmonyMethodType },
                null);

            ConstructorInfo harmonyMethodConstructor = harmonyMethodType.GetConstructor(new Type[] { typeof(MethodInfo) });
            if (patchMethod == null || harmonyMethodConstructor == null)
            {
                return false;
            }

            object prefixMethod = prefix == null ? null : harmonyMethodConstructor.Invoke(new object[] { prefix });
            object postfixMethod = postfix == null ? null : harmonyMethodConstructor.Invoke(new object[] { postfix });
            patchMethod.Invoke(harmony, new object[] { original, prefixMethod, postfixMethod, null, null });
            return true;
        }

        private static bool TinkererSkillsTickIntervalPrefix(Pawn_SkillTracker __instance)
        {
            return __instance == null || !registeredTinkererSkillTrackers.Contains(__instance);
        }

        private static void TinkererPawnSpawnSetupPostfix(Pawn __instance)
        {
            TryApplyToSpawnedPawn(__instance);
        }

        private sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
        {
            public bool Equals(T x, T y)
            {
                return ReferenceEquals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj == null ? 0 : RuntimeHelpers.GetHashCode(obj);
            }
        }

        private static Type GetLoadedType(string fullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Type type = assemblies[i].GetType(fullName, false);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }
    }

    public sealed class TinkererSkillGameComponent : GameComponent
    {
        public TinkererSkillGameComponent(Game game)
        {
        }

        public override void StartedNewGame()
        {
            TinkererSkillApplier.ResetPerGameTracking();
            TinkererSkillApplier.ApplyCurrentSettingsSafely();
        }

        public override void LoadedGame()
        {
            TinkererSkillApplier.ResetPerGameTracking();
            TinkererSkillApplier.ApplyCurrentSettingsSafely();
        }
    }
}
