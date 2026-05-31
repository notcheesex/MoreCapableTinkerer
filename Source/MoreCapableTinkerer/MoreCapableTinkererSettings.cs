using Verse;

namespace MoreCapableTinkerer
{
    public sealed class MoreCapableTinkererSettings : ModSettings
    {
        public const bool DefaultEnableCrafting = true;
        public const bool DefaultEnableSmithing = true;
        public const bool DefaultEnableTailoring = true;
        public const bool DefaultEnableRepair = true;
        public const bool DefaultEnableFixBrokenDownBuilding = true;
        public const bool DefaultEnableFirefighter = false;
        public const bool DefaultEnablePatient = false;
        public const bool DefaultEnableDoctor = false;
        public const bool DefaultEnablePatientBedRest = false;
        public const bool DefaultEnableBasicWorker = false;
        public const bool DefaultEnableWarden = false;
        public const bool DefaultEnableHandling = false;
        public const bool DefaultEnableCooking = false;
        public const bool DefaultEnableHunting = false;
        public const bool DefaultEnableConstruction = false;
        public const bool DefaultEnableGrowing = false;
        public const bool DefaultEnableMining = false;
        public const bool DefaultEnablePlantCutting = false;
        public const bool DefaultEnableArt = false;
        public const bool DefaultEnableHauling = false;
        public const bool DefaultEnableCleaning = false;
        public const bool DefaultEnableResearch = false;
        public const bool DefaultEnableChildcare = false;
        public const bool DefaultEnableDarkStudy = false;
        public const bool DefaultEnableFishing = false;
        public const int DefaultSkillLevel = 8;
        public const int MinSkillLevel = 0;
        public const int MaxSkillLevel = 20;

        public bool enableCrafting = DefaultEnableCrafting;
        public bool enableSmithing = DefaultEnableSmithing;
        public bool enableTailoring = DefaultEnableTailoring;
        public bool enableRepair = DefaultEnableRepair;
        public bool enableFixBrokenDownBuilding = DefaultEnableFixBrokenDownBuilding;
        public bool enableFirefighter = DefaultEnableFirefighter;
        public bool enablePatient = DefaultEnablePatient;
        public bool enableDoctor = DefaultEnableDoctor;
        public bool enablePatientBedRest = DefaultEnablePatientBedRest;
        public bool enableBasicWorker = DefaultEnableBasicWorker;
        public bool enableWarden = DefaultEnableWarden;
        public bool enableHandling = DefaultEnableHandling;
        public bool enableCooking = DefaultEnableCooking;
        public bool enableHunting = DefaultEnableHunting;
        public bool enableConstruction = DefaultEnableConstruction;
        public bool enableGrowing = DefaultEnableGrowing;
        public bool enableMining = DefaultEnableMining;
        public bool enablePlantCutting = DefaultEnablePlantCutting;
        public bool enableArt = DefaultEnableArt;
        public bool enableHauling = DefaultEnableHauling;
        public bool enableCleaning = DefaultEnableCleaning;
        public bool enableResearch = DefaultEnableResearch;
        public bool enableChildcare = DefaultEnableChildcare;
        public bool enableDarkStudy = DefaultEnableDarkStudy;
        public bool enableFishing = DefaultEnableFishing;
        public int craftingSkillLevel = DefaultSkillLevel;
        public int smithingSkillLevel = DefaultSkillLevel;
        public int tailoringSkillLevel = DefaultSkillLevel;
        public int repairSkillLevel = DefaultSkillLevel;
        public int fixBrokenDownBuildingSkillLevel = DefaultSkillLevel;
        public int firefighterSkillLevel = DefaultSkillLevel;
        public int patientSkillLevel = DefaultSkillLevel;
        public int doctorSkillLevel = DefaultSkillLevel;
        public int patientBedRestSkillLevel = DefaultSkillLevel;
        public int basicWorkerSkillLevel = DefaultSkillLevel;
        public int wardenSkillLevel = DefaultSkillLevel;
        public int handlingSkillLevel = DefaultSkillLevel;
        public int cookingSkillLevel = DefaultSkillLevel;
        public int huntingSkillLevel = DefaultSkillLevel;
        public int constructionSkillLevel = DefaultSkillLevel;
        public int growingSkillLevel = DefaultSkillLevel;
        public int miningSkillLevel = DefaultSkillLevel;
        public int plantCuttingSkillLevel = DefaultSkillLevel;
        public int artSkillLevel = DefaultSkillLevel;
        public int haulingSkillLevel = DefaultSkillLevel;
        public int cleaningSkillLevel = DefaultSkillLevel;
        public int researchSkillLevel = DefaultSkillLevel;
        public int childcareSkillLevel = DefaultSkillLevel;
        public int darkStudySkillLevel = DefaultSkillLevel;
        public int fishingSkillLevel = DefaultSkillLevel;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref enableCrafting, "enableCrafting", DefaultEnableCrafting);
            Scribe_Values.Look(ref enableSmithing, "enableSmithing", DefaultEnableSmithing);
            Scribe_Values.Look(ref enableTailoring, "enableTailoring", DefaultEnableTailoring);
            Scribe_Values.Look(ref enableRepair, "enableRepair", DefaultEnableRepair);
            Scribe_Values.Look(ref enableFixBrokenDownBuilding, "enableFixBrokenDownBuilding", DefaultEnableFixBrokenDownBuilding);
            Scribe_Values.Look(ref enableFirefighter, "enableFirefighter", DefaultEnableFirefighter);
            Scribe_Values.Look(ref enablePatient, "enablePatient", DefaultEnablePatient);
            Scribe_Values.Look(ref enableDoctor, "enableDoctor", DefaultEnableDoctor);
            Scribe_Values.Look(ref enablePatientBedRest, "enablePatientBedRest", DefaultEnablePatientBedRest);
            Scribe_Values.Look(ref enableBasicWorker, "enableBasicWorker", DefaultEnableBasicWorker);
            Scribe_Values.Look(ref enableWarden, "enableWarden", DefaultEnableWarden);
            Scribe_Values.Look(ref enableHandling, "enableHandling", DefaultEnableHandling);
            Scribe_Values.Look(ref enableCooking, "enableCooking", DefaultEnableCooking);
            Scribe_Values.Look(ref enableHunting, "enableHunting", DefaultEnableHunting);
            Scribe_Values.Look(ref enableConstruction, "enableConstruction", DefaultEnableConstruction);
            Scribe_Values.Look(ref enableGrowing, "enableGrowing", DefaultEnableGrowing);
            Scribe_Values.Look(ref enableMining, "enableMining", DefaultEnableMining);
            Scribe_Values.Look(ref enablePlantCutting, "enablePlantCutting", DefaultEnablePlantCutting);
            Scribe_Values.Look(ref enableArt, "enableArt", DefaultEnableArt);
            Scribe_Values.Look(ref enableHauling, "enableHauling", DefaultEnableHauling);
            Scribe_Values.Look(ref enableCleaning, "enableCleaning", DefaultEnableCleaning);
            Scribe_Values.Look(ref enableResearch, "enableResearch", DefaultEnableResearch);
            Scribe_Values.Look(ref enableChildcare, "enableChildcare", DefaultEnableChildcare);
            Scribe_Values.Look(ref enableDarkStudy, "enableDarkStudy", DefaultEnableDarkStudy);
            Scribe_Values.Look(ref enableFishing, "enableFishing", DefaultEnableFishing);
            Scribe_Values.Look(ref craftingSkillLevel, "craftingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref smithingSkillLevel, "smithingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref tailoringSkillLevel, "tailoringSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref repairSkillLevel, "repairSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref fixBrokenDownBuildingSkillLevel, "fixBrokenDownBuildingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref firefighterSkillLevel, "firefighterSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref patientSkillLevel, "patientSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref doctorSkillLevel, "doctorSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref patientBedRestSkillLevel, "patientBedRestSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref basicWorkerSkillLevel, "basicWorkerSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref wardenSkillLevel, "wardenSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref handlingSkillLevel, "handlingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref cookingSkillLevel, "cookingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref huntingSkillLevel, "huntingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref constructionSkillLevel, "constructionSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref growingSkillLevel, "growingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref miningSkillLevel, "miningSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref plantCuttingSkillLevel, "plantCuttingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref artSkillLevel, "artSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref haulingSkillLevel, "haulingSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref cleaningSkillLevel, "cleaningSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref researchSkillLevel, "researchSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref childcareSkillLevel, "childcareSkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref darkStudySkillLevel, "darkStudySkillLevel", DefaultSkillLevel);
            Scribe_Values.Look(ref fishingSkillLevel, "fishingSkillLevel", DefaultSkillLevel);
            ClampAllSkillLevels();
        }

        internal void ResetToDefaults()
        {
            enableCrafting = DefaultEnableCrafting;
            enableSmithing = DefaultEnableSmithing;
            enableTailoring = DefaultEnableTailoring;
            enableRepair = DefaultEnableRepair;
            enableFixBrokenDownBuilding = DefaultEnableFixBrokenDownBuilding;
            enableFirefighter = DefaultEnableFirefighter;
            enablePatient = DefaultEnablePatient;
            enableDoctor = DefaultEnableDoctor;
            enablePatientBedRest = DefaultEnablePatientBedRest;
            enableBasicWorker = DefaultEnableBasicWorker;
            enableWarden = DefaultEnableWarden;
            enableHandling = DefaultEnableHandling;
            enableCooking = DefaultEnableCooking;
            enableHunting = DefaultEnableHunting;
            enableConstruction = DefaultEnableConstruction;
            enableGrowing = DefaultEnableGrowing;
            enableMining = DefaultEnableMining;
            enablePlantCutting = DefaultEnablePlantCutting;
            enableArt = DefaultEnableArt;
            enableHauling = DefaultEnableHauling;
            enableCleaning = DefaultEnableCleaning;
            enableResearch = DefaultEnableResearch;
            enableChildcare = DefaultEnableChildcare;
            enableDarkStudy = DefaultEnableDarkStudy;
            enableFishing = DefaultEnableFishing;
            ResetSkillLevelsToDefault();
        }

        internal void ClampAllSkillLevels()
        {
            craftingSkillLevel = ClampSkillLevel(craftingSkillLevel);
            smithingSkillLevel = ClampSkillLevel(smithingSkillLevel);
            tailoringSkillLevel = ClampSkillLevel(tailoringSkillLevel);
            repairSkillLevel = ClampSkillLevel(repairSkillLevel);
            fixBrokenDownBuildingSkillLevel = ClampSkillLevel(fixBrokenDownBuildingSkillLevel);
            firefighterSkillLevel = ClampSkillLevel(firefighterSkillLevel);
            patientSkillLevel = ClampSkillLevel(patientSkillLevel);
            doctorSkillLevel = ClampSkillLevel(doctorSkillLevel);
            patientBedRestSkillLevel = ClampSkillLevel(patientBedRestSkillLevel);
            basicWorkerSkillLevel = ClampSkillLevel(basicWorkerSkillLevel);
            wardenSkillLevel = ClampSkillLevel(wardenSkillLevel);
            handlingSkillLevel = ClampSkillLevel(handlingSkillLevel);
            cookingSkillLevel = ClampSkillLevel(cookingSkillLevel);
            huntingSkillLevel = ClampSkillLevel(huntingSkillLevel);
            constructionSkillLevel = ClampSkillLevel(constructionSkillLevel);
            growingSkillLevel = ClampSkillLevel(growingSkillLevel);
            miningSkillLevel = ClampSkillLevel(miningSkillLevel);
            plantCuttingSkillLevel = ClampSkillLevel(plantCuttingSkillLevel);
            artSkillLevel = ClampSkillLevel(artSkillLevel);
            haulingSkillLevel = ClampSkillLevel(haulingSkillLevel);
            cleaningSkillLevel = ClampSkillLevel(cleaningSkillLevel);
            researchSkillLevel = ClampSkillLevel(researchSkillLevel);
            childcareSkillLevel = ClampSkillLevel(childcareSkillLevel);
            darkStudySkillLevel = ClampSkillLevel(darkStudySkillLevel);
            fishingSkillLevel = ClampSkillLevel(fishingSkillLevel);
        }

        internal int GetSkillLevelForWorkTypeDefName(string workTypeDefName)
        {
            switch (workTypeDefName)
            {
                case "Crafting":
                    return ClampSkillLevel(craftingSkillLevel);
                case "Smithing":
                    return ClampSkillLevel(smithingSkillLevel);
                case "Tailoring":
                    return ClampSkillLevel(tailoringSkillLevel);
                case "Firefighter":
                    return ClampSkillLevel(firefighterSkillLevel);
                case "Patient":
                    return ClampSkillLevel(patientSkillLevel);
                case "Doctor":
                    return ClampSkillLevel(doctorSkillLevel);
                case "PatientBedRest":
                    return ClampSkillLevel(patientBedRestSkillLevel);
                case "BasicWorker":
                    return ClampSkillLevel(basicWorkerSkillLevel);
                case "Warden":
                    return ClampSkillLevel(wardenSkillLevel);
                case "Handling":
                    return ClampSkillLevel(handlingSkillLevel);
                case "Cooking":
                    return ClampSkillLevel(cookingSkillLevel);
                case "Hunting":
                    return ClampSkillLevel(huntingSkillLevel);
                case "Construction":
                    return ClampSkillLevel(constructionSkillLevel);
                case "Growing":
                    return ClampSkillLevel(growingSkillLevel);
                case "Mining":
                    return ClampSkillLevel(miningSkillLevel);
                case "PlantCutting":
                    return ClampSkillLevel(plantCuttingSkillLevel);
                case "Art":
                    return ClampSkillLevel(artSkillLevel);
                case "Hauling":
                    return ClampSkillLevel(haulingSkillLevel);
                case "Cleaning":
                    return ClampSkillLevel(cleaningSkillLevel);
                case "Research":
                    return ClampSkillLevel(researchSkillLevel);
                case "Childcare":
                    return ClampSkillLevel(childcareSkillLevel);
                case "DarkStudy":
                    return ClampSkillLevel(darkStudySkillLevel);
                case "Fishing":
                    return ClampSkillLevel(fishingSkillLevel);
                default:
                    return DefaultSkillLevel;
            }
        }

        internal int GetRepairSkillLevel()
        {
            return ClampSkillLevel(repairSkillLevel);
        }

        internal int GetFixBrokenDownBuildingSkillLevel()
        {
            return ClampSkillLevel(fixBrokenDownBuildingSkillLevel);
        }

        private void ResetSkillLevelsToDefault()
        {
            craftingSkillLevel = DefaultSkillLevel;
            smithingSkillLevel = DefaultSkillLevel;
            tailoringSkillLevel = DefaultSkillLevel;
            repairSkillLevel = DefaultSkillLevel;
            fixBrokenDownBuildingSkillLevel = DefaultSkillLevel;
            firefighterSkillLevel = DefaultSkillLevel;
            patientSkillLevel = DefaultSkillLevel;
            doctorSkillLevel = DefaultSkillLevel;
            patientBedRestSkillLevel = DefaultSkillLevel;
            basicWorkerSkillLevel = DefaultSkillLevel;
            wardenSkillLevel = DefaultSkillLevel;
            handlingSkillLevel = DefaultSkillLevel;
            cookingSkillLevel = DefaultSkillLevel;
            huntingSkillLevel = DefaultSkillLevel;
            constructionSkillLevel = DefaultSkillLevel;
            growingSkillLevel = DefaultSkillLevel;
            miningSkillLevel = DefaultSkillLevel;
            plantCuttingSkillLevel = DefaultSkillLevel;
            artSkillLevel = DefaultSkillLevel;
            haulingSkillLevel = DefaultSkillLevel;
            cleaningSkillLevel = DefaultSkillLevel;
            researchSkillLevel = DefaultSkillLevel;
            childcareSkillLevel = DefaultSkillLevel;
            darkStudySkillLevel = DefaultSkillLevel;
            fishingSkillLevel = DefaultSkillLevel;
        }

        internal static int ClampSkillLevel(int value)
        {
            if (value < MinSkillLevel)
            {
                return MinSkillLevel;
            }

            if (value > MaxSkillLevel)
            {
                return MaxSkillLevel;
            }

            return value;
        }
    }
}
