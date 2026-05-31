using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using Verse;
using Verse.AI;

namespace MoreCapableTinkerer
{
    [StaticConstructorOnStartup]
    internal static class TinkererWorkSettingsStartupHook
    {
        static TinkererWorkSettingsStartupHook()
        {
            TinkererWorkSettingsApplier.ScheduleApplyAfterDefsLoaded();
        }
    }

    internal static class TinkererWorkSettingsApplier
    {
        internal const string LogPrefix = "More Capable Tinkerer";

        private const string TargetPackageId = "vanillaquestsexpanded.dronefactory";
        private const string DroneThinkTreeDefName = "VQE_Drone";
        private const string CraftingDronePawnKindDefName = "VQE_CraftingDroneKindDef";
        private const string ConditionalPawnKindListClassName = "VanillaQuestsExpandedDroneFactory.ThinkNode_ConditionalPawnKindList";
        private const string JobGiverDoWorkClassName = "VanillaQuestsExpandedDroneFactory.JobGiver_DoWork";
        private const string SubNodesFieldName = "subNodes";
        private const string PawnKindsFieldName = "pawnKinds";
        private const string WorkTypesFieldName = "workTypes";
        private const string WorkGiversFieldName = "workgivers";

        private const string CraftingWorkTypeDefName = "Crafting";
        private const string SmithingWorkTypeDefName = "Smithing";
        private const string TailoringWorkTypeDefName = "Tailoring";
        private const string FirefighterWorkTypeDefName = "Firefighter";
        private const string PatientWorkTypeDefName = "Patient";
        private const string DoctorWorkTypeDefName = "Doctor";
        private const string PatientBedRestWorkTypeDefName = "PatientBedRest";
        private const string BasicWorkerWorkTypeDefName = "BasicWorker";
        private const string WardenWorkTypeDefName = "Warden";
        private const string HandlingWorkTypeDefName = "Handling";
        private const string CookingWorkTypeDefName = "Cooking";
        private const string HuntingWorkTypeDefName = "Hunting";
        private const string ConstructionWorkTypeDefName = "Construction";
        private const string GrowingWorkTypeDefName = "Growing";
        private const string MiningWorkTypeDefName = "Mining";
        private const string PlantCuttingWorkTypeDefName = "PlantCutting";
        private const string ArtWorkTypeDefName = "Art";
        private const string HaulingWorkTypeDefName = "Hauling";
        private const string CleaningWorkTypeDefName = "Cleaning";
        private const string ResearchWorkTypeDefName = "Research";
        private const string ChildcareWorkTypeDefName = "Childcare";
        private const string DarkStudyWorkTypeDefName = "DarkStudy";
        private const string FishingWorkTypeDefName = "Fishing";
        private const string RepairWorkGiverDefName = "Repair";
        private const string FixBrokenDownBuildingWorkGiverDefName = "FixBrokenDownBuilding";

        private static bool applyQueued;

        internal static void ScheduleApplyAfterDefsLoaded()
        {
            if (applyQueued)
            {
                return;
            }

            applyQueued = true;
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                applyQueued = false;
                ApplyCurrentSettingsSafely();
                TinkererSkillApplier.ApplyCurrentSettingsSafely();
            });
        }

        internal static void ApplyCurrentSettingsSafely()
        {
            try
            {
                MoreCapableTinkererSettings settings = MoreCapableTinkererMod.GetSettingsForApplication();
                if (settings == null)
                {
                    Log.Warning(LogPrefix + ": Could not retrieve mod settings; skipping runtime work settings apply.");
                    return;
                }

                Apply(settings);
            }
            catch (Exception ex)
            {
                Log.Warning(LogPrefix + ": Runtime work settings apply failed safely. " + ex.GetType().Name + ": " + ex.Message);
            }
        }

        private static void Apply(MoreCapableTinkererSettings settings)
        {
            if (settings == null)
            {
                Log.Warning(LogPrefix + ": Cannot apply runtime work settings because settings were null.");
                return;
            }

            if (ModLister.GetActiveModWithIdentifier(TargetPackageId, true) == null)
            {
                Log.Warning(LogPrefix + ": Target package '" + TargetPackageId + "' is not active; skipping runtime work settings apply.");
                return;
            }

            if (GetLoadedType(ConditionalPawnKindListClassName) == null)
            {
                Log.Warning(LogPrefix + ": Missing target class '" + ConditionalPawnKindListClassName + "'; skipping runtime work settings apply.");
                return;
            }

            if (GetLoadedType(JobGiverDoWorkClassName) == null)
            {
                Log.Warning(LogPrefix + ": Missing target class '" + JobGiverDoWorkClassName + "'; skipping runtime work settings apply.");
                return;
            }

            ThinkTreeDef droneThinkTree = DefDatabase<ThinkTreeDef>.GetNamedSilentFail(DroneThinkTreeDefName);
            if (droneThinkTree == null)
            {
                Log.Warning(LogPrefix + ": Missing ThinkTreeDef '" + DroneThinkTreeDefName + "'; skipping runtime work settings apply.");
                return;
            }

            if (droneThinkTree.thinkRoot == null)
            {
                Log.Warning(LogPrefix + ": ThinkTreeDef '" + DroneThinkTreeDefName + "' has no thinkRoot; skipping runtime work settings apply.");
                return;
            }

            object targetJobGiver = FindTargetJobGiver(droneThinkTree.thinkRoot);
            if (targetJobGiver == null)
            {
                Log.Warning(LogPrefix + ": Could not find the target JobGiver_DoWork node for '" + CraftingDronePawnKindDefName + "'; skipping runtime work settings apply.");
                return;
            }

            FieldInfo workTypesField = GetInstanceField(targetJobGiver.GetType(), WorkTypesFieldName);
            if (workTypesField == null)
            {
                Log.Warning(LogPrefix + ": Target node is missing field '" + WorkTypesFieldName + "'; skipping runtime work settings apply.");
                return;
            }

            FieldInfo workGiversField = GetInstanceField(targetJobGiver.GetType(), WorkGiversFieldName);
            if (workGiversField == null)
            {
                Log.Warning(LogPrefix + ": Target node is missing field '" + WorkGiversFieldName + "'; skipping runtime work settings apply.");
                return;
            }

            if (!workTypesField.FieldType.IsAssignableFrom(typeof(List<WorkTypeDef>)))
            {
                Log.Warning(LogPrefix + ": Target field '" + WorkTypesFieldName + "' has unsupported type '" + workTypesField.FieldType.FullName + "'; skipping runtime work settings apply.");
                return;
            }

            if (!workGiversField.FieldType.IsAssignableFrom(typeof(List<WorkGiverDef>)))
            {
                Log.Warning(LogPrefix + ": Target field '" + WorkGiversFieldName + "' has unsupported type '" + workGiversField.FieldType.FullName + "'; skipping runtime work settings apply.");
                return;
            }

            List<WorkTypeDef> workTypes;
            List<WorkGiverDef> workGivers;
            if (!TryBuildConfiguredLists(settings, out workTypes, out workGivers))
            {
                return;
            }

            workTypesField.SetValue(targetJobGiver, workTypes);
            workGiversField.SetValue(targetJobGiver, workGivers);
        }

        private static bool TryBuildConfiguredLists(MoreCapableTinkererSettings settings, out List<WorkTypeDef> workTypes, out List<WorkGiverDef> workGivers)
        {
            workTypes = new List<WorkTypeDef>();
            workGivers = new List<WorkGiverDef>();

            foreach (string workTypeDefName in GetEnabledWorkTypeDefNames(settings))
            {
                if (!TryAddWorkType(workTypes, workTypeDefName))
                {
                    return false;
                }
            }

            if (settings.enableRepair && !TryAddWorkGiver(workGivers, RepairWorkGiverDefName))
            {
                return false;
            }

            if (settings.enableFixBrokenDownBuilding && !TryAddWorkGiver(workGivers, FixBrokenDownBuildingWorkGiverDefName))
            {
                return false;
            }

            return true;
        }

        internal static IEnumerable<string> GetEnabledWorkTypeDefNames(MoreCapableTinkererSettings settings)
        {
            if (settings == null)
            {
                yield break;
            }

            if (settings.enableCrafting)
            {
                yield return CraftingWorkTypeDefName;
            }

            if (settings.enableSmithing)
            {
                yield return SmithingWorkTypeDefName;
            }

            if (settings.enableTailoring)
            {
                yield return TailoringWorkTypeDefName;
            }

            if (settings.enableFirefighter)
            {
                yield return FirefighterWorkTypeDefName;
            }

            if (settings.enablePatient)
            {
                yield return PatientWorkTypeDefName;
            }

            if (settings.enableDoctor)
            {
                yield return DoctorWorkTypeDefName;
            }

            if (settings.enablePatientBedRest)
            {
                yield return PatientBedRestWorkTypeDefName;
            }

            if (settings.enableBasicWorker)
            {
                yield return BasicWorkerWorkTypeDefName;
            }

            if (settings.enableWarden)
            {
                yield return WardenWorkTypeDefName;
            }

            if (settings.enableHandling)
            {
                yield return HandlingWorkTypeDefName;
            }

            if (settings.enableCooking)
            {
                yield return CookingWorkTypeDefName;
            }

            if (settings.enableHunting)
            {
                yield return HuntingWorkTypeDefName;
            }

            if (settings.enableConstruction)
            {
                yield return ConstructionWorkTypeDefName;
            }

            if (settings.enableGrowing)
            {
                yield return GrowingWorkTypeDefName;
            }

            if (settings.enableMining)
            {
                yield return MiningWorkTypeDefName;
            }

            if (settings.enablePlantCutting)
            {
                yield return PlantCuttingWorkTypeDefName;
            }

            if (settings.enableArt)
            {
                yield return ArtWorkTypeDefName;
            }

            if (settings.enableHauling)
            {
                yield return HaulingWorkTypeDefName;
            }

            if (settings.enableCleaning)
            {
                yield return CleaningWorkTypeDefName;
            }

            if (settings.enableResearch)
            {
                yield return ResearchWorkTypeDefName;
            }

            if (settings.enableChildcare)
            {
                yield return ChildcareWorkTypeDefName;
            }

            if (settings.enableDarkStudy)
            {
                yield return DarkStudyWorkTypeDefName;
            }

            if (settings.enableFishing)
            {
                yield return FishingWorkTypeDefName;
            }
        }

        internal static bool HasEnabledExplicitConstructionWorkGiver(MoreCapableTinkererSettings settings)
        {
            return settings != null && (settings.enableRepair || settings.enableFixBrokenDownBuilding);
        }

        private static bool TryAddWorkType(List<WorkTypeDef> workTypes, string defName)
        {
            WorkTypeDef workType = DefDatabase<WorkTypeDef>.GetNamedSilentFail(defName);
            if (workType == null)
            {
                Log.Warning(LogPrefix + ": Missing WorkTypeDef '" + defName + "'; skipping runtime work settings apply.");
                return false;
            }

            workTypes.Add(workType);
            return true;
        }

        private static bool TryAddWorkGiver(List<WorkGiverDef> workGivers, string defName)
        {
            WorkGiverDef workGiver = DefDatabase<WorkGiverDef>.GetNamedSilentFail(defName);
            if (workGiver == null)
            {
                Log.Warning(LogPrefix + ": Missing WorkGiverDef '" + defName + "'; skipping runtime work settings apply.");
                return false;
            }

            workGivers.Add(workGiver);
            return true;
        }

        private static object FindTargetJobGiver(object node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.GetType().FullName == ConditionalPawnKindListClassName && HasTargetPawnKind(node))
            {
                foreach (object subNode in GetSubNodes(node))
                {
                    if (subNode != null && subNode.GetType().FullName == JobGiverDoWorkClassName)
                    {
                        return subNode;
                    }
                }

                Log.Warning(LogPrefix + ": Found '" + ConditionalPawnKindListClassName + "' for '" + CraftingDronePawnKindDefName + "' but not its direct '" + JobGiverDoWorkClassName + "' child.");
                return null;
            }

            foreach (object subNode in GetSubNodes(node))
            {
                object found = FindTargetJobGiver(subNode);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private static bool HasTargetPawnKind(object node)
        {
            FieldInfo pawnKindsField = GetInstanceField(node.GetType(), PawnKindsFieldName);
            if (pawnKindsField == null)
            {
                Log.Warning(LogPrefix + ": Target class '" + ConditionalPawnKindListClassName + "' is missing field '" + PawnKindsFieldName + "'; skipping this node.");
                return false;
            }

            object value = pawnKindsField.GetValue(node);
            IEnumerable pawnKinds = value as IEnumerable;
            if (pawnKinds == null || value is string)
            {
                return false;
            }

            foreach (object pawnKind in pawnKinds)
            {
                Def pawnKindDef = pawnKind as Def;
                if (pawnKindDef != null && pawnKindDef.defName == CraftingDronePawnKindDefName)
                {
                    return true;
                }

                if (pawnKind != null && pawnKind.ToString() == CraftingDronePawnKindDefName)
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<object> GetSubNodes(object node)
        {
            FieldInfo subNodesField = GetInstanceField(node.GetType(), SubNodesFieldName);
            if (subNodesField == null)
            {
                Log.Warning(LogPrefix + ": Node class '" + node.GetType().FullName + "' is missing field '" + SubNodesFieldName + "'; skipping its children.");
                yield break;
            }

            object value = subNodesField.GetValue(node);
            IEnumerable subNodes = value as IEnumerable;
            if (subNodes == null || value is string)
            {
                yield break;
            }

            foreach (object subNode in subNodes)
            {
                yield return subNode;
            }
        }

        private static FieldInfo GetInstanceField(Type type, string fieldName)
        {
            Type currentType = type;
            while (currentType != null)
            {
                FieldInfo field = currentType.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                if (field != null)
                {
                    return field;
                }

                currentType = currentType.BaseType;
            }

            return null;
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
}
