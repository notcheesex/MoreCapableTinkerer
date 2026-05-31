using System;
using UnityEngine;
using Verse;

namespace MoreCapableTinkerer
{
    public sealed class MoreCapableTinkererMod : Mod
    {
        private const string SettingsCategoryKey = "MoreCapableTinkerer_SettingsCategory";
        private const string SettingsHelpKey = "MoreCapableTinkerer_SettingsHelp";
        private const string SkillLevelLabelKey = "MoreCapableTinkerer_SkillLevelLabel";
        private const string SkillLevelTooltipKey = "MoreCapableTinkerer_SkillLevelTooltip";
        private const string ResetToDefaultsLabelKey = "MoreCapableTinkerer_ResetToDefaultsLabel";
        private const string ResetToDefaultsTooltipKey = "MoreCapableTinkerer_ResetToDefaultsTooltip";
        private const string SettingLabelKeyPrefix = "MoreCapableTinkerer_Enable";
        private const string SettingLabelKeySuffix = "Label";
        private const string SettingTooltipKeySuffix = "Tooltip";
        private const float ScrollBarWidth = 16f;
        private const float RowHeight = 32f;
        private const float RowGap = 4f;
        private const float HeaderHeight = 24f;
        private const float ButtonHeight = 32f;
        private const float ResetButtonWidth = 180f;
        private const float NumericBoxWidth = 42f;
        private const float SliderMinWidth = 120f;
        private const float SliderMaxWidth = 220f;
        private const float ControlGap = 8f;
        private const float SmallGap = 6f;

        private static readonly WorkEntryDefinition[] WorkEntries =
        {
            new WorkEntryDefinition("Crafting", s => s.enableCrafting, (s, value) => s.enableCrafting = value, s => s.craftingSkillLevel, (s, value) => s.craftingSkillLevel = value),
            new WorkEntryDefinition("Smithing", s => s.enableSmithing, (s, value) => s.enableSmithing = value, s => s.smithingSkillLevel, (s, value) => s.smithingSkillLevel = value),
            new WorkEntryDefinition("Tailoring", s => s.enableTailoring, (s, value) => s.enableTailoring = value, s => s.tailoringSkillLevel, (s, value) => s.tailoringSkillLevel = value),
            new WorkEntryDefinition("Repair", s => s.enableRepair, (s, value) => s.enableRepair = value, s => s.repairSkillLevel, (s, value) => s.repairSkillLevel = value),
            new WorkEntryDefinition("FixBrokenDownBuilding", s => s.enableFixBrokenDownBuilding, (s, value) => s.enableFixBrokenDownBuilding = value, s => s.fixBrokenDownBuildingSkillLevel, (s, value) => s.fixBrokenDownBuildingSkillLevel = value),
            new WorkEntryDefinition("Firefighter", s => s.enableFirefighter, (s, value) => s.enableFirefighter = value, s => s.firefighterSkillLevel, (s, value) => s.firefighterSkillLevel = value),
            new WorkEntryDefinition("Patient", s => s.enablePatient, (s, value) => s.enablePatient = value, s => s.patientSkillLevel, (s, value) => s.patientSkillLevel = value),
            new WorkEntryDefinition("Doctor", s => s.enableDoctor, (s, value) => s.enableDoctor = value, s => s.doctorSkillLevel, (s, value) => s.doctorSkillLevel = value),
            new WorkEntryDefinition("PatientBedRest", s => s.enablePatientBedRest, (s, value) => s.enablePatientBedRest = value, s => s.patientBedRestSkillLevel, (s, value) => s.patientBedRestSkillLevel = value),
            new WorkEntryDefinition("BasicWorker", s => s.enableBasicWorker, (s, value) => s.enableBasicWorker = value, s => s.basicWorkerSkillLevel, (s, value) => s.basicWorkerSkillLevel = value),
            new WorkEntryDefinition("Warden", s => s.enableWarden, (s, value) => s.enableWarden = value, s => s.wardenSkillLevel, (s, value) => s.wardenSkillLevel = value),
            new WorkEntryDefinition("Handling", s => s.enableHandling, (s, value) => s.enableHandling = value, s => s.handlingSkillLevel, (s, value) => s.handlingSkillLevel = value),
            new WorkEntryDefinition("Cooking", s => s.enableCooking, (s, value) => s.enableCooking = value, s => s.cookingSkillLevel, (s, value) => s.cookingSkillLevel = value),
            new WorkEntryDefinition("Hunting", s => s.enableHunting, (s, value) => s.enableHunting = value, s => s.huntingSkillLevel, (s, value) => s.huntingSkillLevel = value),
            new WorkEntryDefinition("Construction", s => s.enableConstruction, (s, value) => s.enableConstruction = value, s => s.constructionSkillLevel, (s, value) => s.constructionSkillLevel = value),
            new WorkEntryDefinition("Growing", s => s.enableGrowing, (s, value) => s.enableGrowing = value, s => s.growingSkillLevel, (s, value) => s.growingSkillLevel = value),
            new WorkEntryDefinition("Mining", s => s.enableMining, (s, value) => s.enableMining = value, s => s.miningSkillLevel, (s, value) => s.miningSkillLevel = value),
            new WorkEntryDefinition("PlantCutting", s => s.enablePlantCutting, (s, value) => s.enablePlantCutting = value, s => s.plantCuttingSkillLevel, (s, value) => s.plantCuttingSkillLevel = value),
            new WorkEntryDefinition("Art", s => s.enableArt, (s, value) => s.enableArt = value, s => s.artSkillLevel, (s, value) => s.artSkillLevel = value),
            new WorkEntryDefinition("Hauling", s => s.enableHauling, (s, value) => s.enableHauling = value, s => s.haulingSkillLevel, (s, value) => s.haulingSkillLevel = value),
            new WorkEntryDefinition("Cleaning", s => s.enableCleaning, (s, value) => s.enableCleaning = value, s => s.cleaningSkillLevel, (s, value) => s.cleaningSkillLevel = value),
            new WorkEntryDefinition("Research", s => s.enableResearch, (s, value) => s.enableResearch = value, s => s.researchSkillLevel, (s, value) => s.researchSkillLevel = value),
            new WorkEntryDefinition("Childcare", s => s.enableChildcare, (s, value) => s.enableChildcare = value, s => s.childcareSkillLevel, (s, value) => s.childcareSkillLevel = value),
            new WorkEntryDefinition("DarkStudy", s => s.enableDarkStudy, (s, value) => s.enableDarkStudy = value, s => s.darkStudySkillLevel, (s, value) => s.darkStudySkillLevel = value),
            new WorkEntryDefinition("Fishing", s => s.enableFishing, (s, value) => s.enableFishing = value, s => s.fishingSkillLevel, (s, value) => s.fishingSkillLevel = value)
        };

        private static MoreCapableTinkererSettings settings;
        private static Vector2 settingsScrollPosition;
        private static bool skillTextBuffersInitialized;

        public MoreCapableTinkererMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<MoreCapableTinkererSettings>();
            TinkererWorkSettingsApplier.ScheduleApplyAfterDefsLoaded();
        }

        public override string SettingsCategory()
        {
            return Translate(SettingsCategoryKey);
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            MoreCapableTinkererSettings currentSettings = Settings;
            if (currentSettings == null)
            {
                return;
            }

            currentSettings.ClampAllSkillLevels();
            EnsureSkillTextBuffers(currentSettings);

            float viewWidth = inRect.width - ScrollBarWidth;
            float viewHeight = Mathf.Max(inRect.height, CalculateScrollContentHeight(viewWidth));
            ClampScrollPosition(inRect.height, viewHeight);

            Rect viewRect = new Rect(0f, 0f, viewWidth, viewHeight);
            Widgets.BeginScrollView(inRect, ref settingsScrollPosition, viewRect);

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(viewRect);

            listing.Label(Translate(SettingsHelpKey));
            listing.Gap(SmallGap);
            DrawResetButton(listing, currentSettings);
            listing.Gap(SmallGap);
            DrawSkillHeader(listing);
            listing.Gap(RowGap);

            for (int i = 0; i < WorkEntries.Length; i++)
            {
                DrawWorkEntryRow(listing, WorkEntries[i], currentSettings);
            }

            listing.End();
            Widgets.EndScrollView();
        }

        public override void WriteSettings()
        {
            MoreCapableTinkererSettings currentSettings = Settings;
            if (currentSettings != null)
            {
                currentSettings.ClampAllSkillLevels();
            }

            base.WriteSettings();
            TinkererWorkSettingsApplier.ApplyCurrentSettingsSafely();
            TinkererSkillApplier.ApplyCurrentSettingsSafely();
        }

        internal static MoreCapableTinkererSettings GetSettingsForApplication()
        {
            if (settings != null)
            {
                return settings;
            }

            MoreCapableTinkererMod mod = LoadedModManager.GetMod<MoreCapableTinkererMod>();
            if (mod == null)
            {
                return null;
            }

            settings = mod.GetSettings<MoreCapableTinkererSettings>();
            return settings;
        }

        private static MoreCapableTinkererSettings Settings
        {
            get
            {
                return GetSettingsForApplication();
            }
        }

        private static string Translate(string key)
        {
            return key.Translate().ToString();
        }

        private static float CalculateScrollContentHeight(float viewWidth)
        {
            float height = Text.CalcHeight(Translate(SettingsHelpKey), viewWidth);
            height += SmallGap;
            height += ButtonHeight;
            height += SmallGap;
            height += HeaderHeight;
            height += RowGap;
            height += WorkEntries.Length * (RowHeight + RowGap);
            return height;
        }

        private static void ClampScrollPosition(float windowHeight, float viewHeight)
        {
            float maxScrollY = Mathf.Max(0f, viewHeight - windowHeight);
            if (settingsScrollPosition.y > maxScrollY)
            {
                settingsScrollPosition.y = maxScrollY;
            }

            if (settingsScrollPosition.y < 0f)
            {
                settingsScrollPosition.y = 0f;
            }
        }

        private static void DrawResetButton(Listing_Standard listing, MoreCapableTinkererSettings currentSettings)
        {
            Rect rowRect = listing.GetRect(ButtonHeight);
            Rect buttonRect = new Rect(rowRect.x, rowRect.y, ResetButtonWidth, rowRect.height);
            if (Widgets.ButtonText(buttonRect, Translate(ResetToDefaultsLabelKey)))
            {
                currentSettings.ResetToDefaults();
                ResetSkillTextBuffers(currentSettings);
            }

            TooltipHandler.TipRegion(buttonRect, Translate(ResetToDefaultsTooltipKey));
        }

        private static void DrawSkillHeader(Listing_Standard listing)
        {
            Rect headerRect = listing.GetRect(HeaderHeight);
            Rect checkboxRect;
            Rect sliderRect;
            Rect numericRect;
            GetWorkEntryRects(headerRect, out checkboxRect, out sliderRect, out numericRect);

            Rect skillHeaderRect = new Rect(sliderRect.x, headerRect.y, numericRect.xMax - sliderRect.x, headerRect.height);
            Widgets.Label(skillHeaderRect, Translate(SkillLevelLabelKey));
            TooltipHandler.TipRegion(skillHeaderRect, Translate(SkillLevelTooltipKey));
        }

        private static void DrawWorkEntryRow(Listing_Standard listing, WorkEntryDefinition entry, MoreCapableTinkererSettings currentSettings)
        {
            Rect rowRect = listing.GetRect(RowHeight);
            Rect checkboxRect;
            Rect sliderRect;
            Rect numericRect;
            GetWorkEntryRects(rowRect, out checkboxRect, out sliderRect, out numericRect);

            string settingLabel = Translate(SettingLabelKeyPrefix + entry.KeyName + SettingLabelKeySuffix);
            string settingTooltip = Translate(SettingLabelKeyPrefix + entry.KeyName + SettingTooltipKeySuffix);
            string skillTooltip = Translate(SkillLevelTooltipKey);
            string rowTooltip = settingTooltip + "\n\n" + skillTooltip;

            bool enabled = entry.GetEnabled(currentSettings);
            Widgets.CheckboxLabeled(checkboxRect, settingLabel, ref enabled);
            entry.SetEnabled(currentSettings, enabled);

            int skillLevel = MoreCapableTinkererSettings.ClampSkillLevel(entry.GetSkillLevel(currentSettings));
            entry.SetSkillLevel(currentSettings, skillLevel);

            float sliderValue = Widgets.HorizontalSlider(
                sliderRect,
                skillLevel,
                MoreCapableTinkererSettings.MinSkillLevel,
                MoreCapableTinkererSettings.MaxSkillLevel,
                true,
                skillLevel.ToString(),
                MoreCapableTinkererSettings.MinSkillLevel.ToString(),
                MoreCapableTinkererSettings.MaxSkillLevel.ToString(),
                1f);

            int sliderSkillLevel = MoreCapableTinkererSettings.ClampSkillLevel(Mathf.RoundToInt(sliderValue));
            if (sliderSkillLevel != skillLevel)
            {
                skillLevel = sliderSkillLevel;
                entry.SetSkillLevel(currentSettings, skillLevel);
                entry.SkillTextBuffer = skillLevel.ToString();
            }

            int numericSkillLevel = skillLevel;
            Widgets.TextFieldNumeric(
                numericRect,
                ref numericSkillLevel,
                ref entry.SkillTextBuffer,
                MoreCapableTinkererSettings.MinSkillLevel,
                MoreCapableTinkererSettings.MaxSkillLevel);

            int bufferedSkillLevel;
            if (int.TryParse(entry.SkillTextBuffer, out bufferedSkillLevel))
            {
                numericSkillLevel = MoreCapableTinkererSettings.ClampSkillLevel(bufferedSkillLevel);
                if (numericSkillLevel != bufferedSkillLevel)
                {
                    entry.SkillTextBuffer = numericSkillLevel.ToString();
                }
            }
            else
            {
                numericSkillLevel = MoreCapableTinkererSettings.ClampSkillLevel(numericSkillLevel);
            }

            if (numericSkillLevel != skillLevel)
            {
                entry.SetSkillLevel(currentSettings, numericSkillLevel);
            }

            TooltipHandler.TipRegion(rowRect, rowTooltip);
            listing.Gap(RowGap);
        }

        private static void EnsureSkillTextBuffers(MoreCapableTinkererSettings currentSettings)
        {
            if (skillTextBuffersInitialized)
            {
                return;
            }

            ResetSkillTextBuffers(currentSettings);
            skillTextBuffersInitialized = true;
        }

        private static void ResetSkillTextBuffers(MoreCapableTinkererSettings currentSettings)
        {
            for (int i = 0; i < WorkEntries.Length; i++)
            {
                WorkEntryDefinition entry = WorkEntries[i];
                entry.SkillTextBuffer = MoreCapableTinkererSettings.ClampSkillLevel(entry.GetSkillLevel(currentSettings)).ToString();
            }
        }

        private static void GetWorkEntryRects(Rect rowRect, out Rect checkboxRect, out Rect sliderRect, out Rect numericRect)
        {
            float sliderWidth = Mathf.Min(SliderMaxWidth, Mathf.Max(SliderMinWidth, rowRect.width * 0.32f));
            numericRect = new Rect(rowRect.xMax - NumericBoxWidth, rowRect.y + 4f, NumericBoxWidth, 24f);
            sliderRect = new Rect(numericRect.x - ControlGap - sliderWidth, rowRect.y + 4f, sliderWidth, 24f);
            checkboxRect = new Rect(rowRect.x, rowRect.y, Mathf.Max(0f, sliderRect.x - rowRect.x - ControlGap), rowRect.height);
        }

        private sealed class WorkEntryDefinition
        {
            private readonly Func<MoreCapableTinkererSettings, bool> getEnabled;
            private readonly Action<MoreCapableTinkererSettings, bool> setEnabled;
            private readonly Func<MoreCapableTinkererSettings, int> getSkillLevel;
            private readonly Action<MoreCapableTinkererSettings, int> setSkillLevel;

            internal WorkEntryDefinition(
                string keyName,
                Func<MoreCapableTinkererSettings, bool> getEnabled,
                Action<MoreCapableTinkererSettings, bool> setEnabled,
                Func<MoreCapableTinkererSettings, int> getSkillLevel,
                Action<MoreCapableTinkererSettings, int> setSkillLevel)
            {
                KeyName = keyName;
                this.getEnabled = getEnabled;
                this.setEnabled = setEnabled;
                this.getSkillLevel = getSkillLevel;
                this.setSkillLevel = setSkillLevel;
            }

            internal string KeyName { get; private set; }

            internal string SkillTextBuffer;

            internal bool GetEnabled(MoreCapableTinkererSettings currentSettings)
            {
                return getEnabled(currentSettings);
            }

            internal void SetEnabled(MoreCapableTinkererSettings currentSettings, bool value)
            {
                setEnabled(currentSettings, value);
            }

            internal int GetSkillLevel(MoreCapableTinkererSettings currentSettings)
            {
                return getSkillLevel(currentSettings);
            }

            internal void SetSkillLevel(MoreCapableTinkererSettings currentSettings, int value)
            {
                setSkillLevel(currentSettings, MoreCapableTinkererSettings.ClampSkillLevel(value));
            }
        }
    }
}
