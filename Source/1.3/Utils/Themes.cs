using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.IO;
using System.Xml;
using System.Collections;
using LibAPNG;
using System.Reflection;
using System.Runtime.CompilerServices;
using RuntimeAudioClipLoader;
using Verse.Sound;
using UnityEngine.Networking;
// ReSharper disable InconsistentNaming

namespace aRandomKiwi.RimThemes
{
    [StaticConstructorOnStartup]
    static class Themes
    {
        static Themes()
        {
            const BindingFlags NPS = (BindingFlags.NonPublic | BindingFlags.Static);

            /************************************************************************* Textures **********************************************************************/
            //Init of the list of variables of interest of textures
            fieldsOfInterestTex["Widgets"] = new List<FOI>()
            {
                new FOI("DefaultBarBgTex",NPS),new FOI("BarFullTexHor",NPS),new FOI("CheckboxOnTex"), new FOI("CheckboxOffTex"), new FOI("CheckboxPartialTex"), new FOI("RadioButOnTex"),
                new FOI("RadioButOffTex",NPS),new FOI("FillArrowTexRight",NPS),new FOI("FillArrowTexLeft",NPS),new FOI("ShadowAtlas",NPS),
                new FOI("ButtonBGAtlas"), new FOI("ButtonBGAtlasMouseover",NPS), new FOI("ButtonBGAtlasClick"), new FOI("FloatRangeSliderTex",NPS), new FOI("LightHighlight"),
                new FOI("AltTexture",NPS), new FOI("ButtonSubtleAtlas"), new FOI("ButtonBarTex",NPS)
            };
            fieldsOfInterestTex["ColonistBarColonistDrawer"] = new List<FOI>()
            {
                new FOI("MoodBGTex",NPS),new FOI("DeadColonistTex",NPS),new FOI("Icon_FormingCaravan",NPS),new FOI("Icon_MentalStateNonAggro",NPS),new FOI("Icon_MentalStateAggro",NPS),
                new FOI("Icon_MedicalRest",NPS),new FOI("Icon_Sleeping",NPS),
                new FOI("Icon_Fleeing",NPS),new FOI("Icon_Attacking",NPS),new FOI("Icon_Idle",NPS),new FOI("Icon_Burning",NPS),new FOI("Icon_Inspired",NPS)
            };
            fieldsOfInterestTex["GenUI"] = new List<FOI>(){new FOI("UnderShadowTex",NPS),new FOI("UIFlash",NPS)};
            fieldsOfInterestTex["LogEntry"] = new List<FOI>(){new FOI("Blood"),new FOI("BloodTarget"),new FOI("Downed"),new FOI("DownedTarget"),new FOI("Skull"),new FOI("SkullTarget")};

            fieldsOfInterestTex["TrainingCardUtility"] = new List<FOI>() { new FOI("LearnedTrainingTex", NPS), new FOI("LearnedNotTrainingTex", NPS) };
            fieldsOfInterestTex["CompTransporter"] = new List<FOI>() {
                new FOI("CancelLoadCommandTex"), new FOI("LoadCommandTex", NPS), new FOI("SelectPreviousInGroupCommandTex", NPS),
                new FOI("SelectAllInGroupCommandTex", NPS),new FOI("SelectNextInGroupCommandTex", NPS)
            };
            fieldsOfInterestTex["Thought"] = new List<FOI>() { new FOI("DefaultGoodIcon", NPS), new FOI("DefaultBadIcon", NPS) };
            fieldsOfInterestTex["PawnColumnWorker_Bond"] = new List<FOI>() { new FOI("BondIcon", NPS), new FOI("BondBrokenIcon", NPS)};
            fieldsOfInterestTex["HostilityResponseModeUtility"] = new List<FOI>() { new FOI("IgnoreIcon", NPS), new FOI("AttackIcon", NPS), new FOI("FleeIcon", NPS) };
            fieldsOfInterestTex["GenderUtility"] = new List<FOI>() { new FOI("GenderlessIcon", NPS), new FOI("MaleIcon", NPS), new FOI("FemaleIcon", NPS) };
            fieldsOfInterestTex["ContentSourceUtility"] = new List<FOI>() { new FOI("ContentSourceIcon_ModsFolder", NPS), new FOI("ContentSourceIcon_SteamWorkshop", NPS), new FOI("ContentSourceIcon_OfficialModsFolder", NPS) };
            fieldsOfInterestTex["MainMenuDrawer"] = new List<FOI>() { new FOI("TexTitle", NPS), new FOI("TexLudeonLogo", NPS) };
            fieldsOfInterestTex["ActiveTip"] = new List<FOI>() { new FOI("TooltipBGAtlas") };
            //fieldsOfInterestTex["UI_BackgroundMain"] = new List<FOI>() { new FOI("BGPlanet",NPS) };
            fieldsOfInterestTex["TabRecord"] = new List<FOI>() { new FOI("TabAtlas",NPS) };
            fieldsOfInterestTex["Command"] = new List<FOI>(){ new FOI("BGTex") };
            fieldsOfInterestTex["CustomCursor"] = new List<FOI>() { new FOI("CursorTex",NPS) };
            fieldsOfInterestTex["InspectPaneUtility"] = new List<FOI>() { new FOI("InspectTabButtonFillTex", NPS) };
            

            fieldsOfInterestTex["TexButton"] = new List<FOI>() {
                new FOI("DeleteX"),new FOI("CloseXBig"),new FOI("CloseXSmall"),new FOI("NextBig"),new FOI("ReorderUp"),new FOI("ReorderDown"),new FOI("Plus"),new FOI("Minus"),
                new FOI("Suspend"),new FOI("SelectOverlappingNext"),new FOI("Info"),new FOI("Rename"),new FOI("Banish"),new FOI("OpenStatsReport"),
                new FOI("Copy"),new FOI("Paste"),new FOI("Drop"),new FOI("Ingest"),new FOI("DragHash"),new FOI("ToggleLog"),new FOI("OpenDebugActionsMenu"),
                new FOI("OpenInspector"),new FOI("OpenInspectSettings"),new FOI("ToggleGodMode"),new FOI("TogglePauseOnError"),
                new FOI("ToggleTweak"),new FOI("Add"),
                new FOI("NewItem"), new FOI("Reveal"),new FOI("Collapse"),new FOI("Empty"),new FOI("Save"),new FOI("NewFile"),new FOI("RenameDev"),new FOI("Reload"),
                new FOI("Play"),new FOI("Stop"),new FOI("RangeMatch"),new FOI("InspectModeToggle"),new FOI("CenterOnPointsTex"),new FOI("CurveResetTex"),new FOI("QuickZoomHor1Tex"),
                new FOI("QuickZoomHor100Tex"),
                new FOI("QuickZoomHor20kTex"),new FOI("QuickZoomVer1Tex"),new FOI("QuickZoomVer100Tex"),new FOI("QuickZoomVer20kTex"),new FOI("IconBlog"),new FOI("IconForums"),
                new FOI("IconTwitter"),new FOI("IconBook"),new FOI("IconSoundtrack"),new FOI("ShowLearningHelper"),new FOI("ShowZones"),
                new FOI("ShowBeauty"),new FOI("ShowRoomStats"),new FOI("ShowColonistBar"),new FOI("ShowRoofOverlay"),new FOI("AutoHomeArea"),
                new FOI("AutoRebuild"),new FOI("CategorizedResourceReadout"),new FOI("LockNorthUp"),new FOI("UsePlanetDayNightSystem"),new FOI("ShowExpandingIcons"),
                new FOI("ShowWorldFeatures")
            };
            fieldsOfInterestTex["TexCommand"] = new List<FOI>()
            {
                new FOI("DesirePower"),new FOI("Draft"),new FOI("ReleaseAnimals"),new FOI("HoldOpen"),new FOI("GatherSpotActive"),new FOI("Install"),new FOI("SquadAttack"),
                new FOI("AttackMelee"),new FOI("Attack"),new FOI("FireAtWill"),new FOI("ToggleVent"),new FOI("PauseCaravan"),new FOI("ForbidOff"),new FOI("ForbidOn"),
                new FOI("RearmTrap"),new FOI("CannotShoot"),new FOI("ClearPrioritizedWork"),new FOI("RemoveRoutePlannerWaypoint")
            };

            fieldsOfInterestTex["WidgetsWork"] = new List<FOI>()
            {
                new FOI("WorkBoxBGTex_Awful"),new FOI("WorkBoxBGTex_Bad"),new FOI("WorkBoxBGTex_Mid"),new FOI("WorkBoxBGTex_Excellent"),new FOI("WorkBoxCheckTex"),
                new FOI("PassionWorkboxMinorIcon"),new FOI("PassionWorkboxMajorIcon"),new FOI("WorkBoxOverlay_Warning")
            };
            fieldsOfInterestTex["TexUI"] = new List<FOI>() {
                new FOI("TitleBGTex"),new FOI("HighlightTex"),new FOI("HighlightSelectedTex"),new FOI("ArrowTexRight"),new FOI("ArrowTexLeft"),new FOI("WinExpandWidget"),new FOI("ArrowTex")
                ,new FOI("RotLeftTex"),new FOI("RotRightTex"),new FOI("HighlightTex"),new FOI("FastFillTex"),new FOI("TextBGBlack"),new FOI("GrayTextBG"),new FOI("FloatMenuOptionBG")
            };
            fieldsOfInterestTex["PawnColumnWorker_Predator"] = new List<FOI>() { new FOI("Icon", NPS) };
            fieldsOfInterestTex["CaravanThingsTabUtility"] = new List<FOI>(){new FOI("AbandonButtonTex"),new FOI("AbandonSpecificCountButtonTex"),new FOI("SpecificTabButtonTex")};
            fieldsOfInterestTex["Settlement"] = new List<FOI>(){ new FOI("ShowSellableItemsCommand"),new FOI("FormCaravanCommand"),new FOI("AttackCommand") };
            fieldsOfInterestTex["HediffComp_TendDuration"] = new List<FOI>(){ new FOI("TendedIcon_Need_General",NPS),new FOI("TendedIcon_Well_General",NPS),new FOI("TendedIcon_Well_Injury",NPS)};
            fieldsOfInterestTex["PawnColumnWorker"] = new List<FOI>() { new FOI("SortingIcon",NPS),new FOI("SortingDescendingIcon",NPS)};
            fieldsOfInterestTex["SettleUtility"] = new List<FOI>() { new FOI("SettleCommandTex") };
            fieldsOfInterestTex["PawnColumnWorker_Pregnant"] = new List<FOI>() { new FOI("Icon",NPS) };
            fieldsOfInterestTex["SkillUI"] = new List<FOI>() { new FOI("PassionMinorIcon", NPS), new FOI("PassionMajorIcon", NPS), new FOI("SkillBarFillTex", NPS) };
            fieldsOfInterestTex["Need"] = new List<FOI>() { new FOI("BarInstantMarkerTex", NPS), new FOI("NeedUnitDividerTex", NPS) };
            fieldsOfInterestTex["CompassWidget"] = new List<FOI>() { new FOI("CompassTex", NPS) };
            fieldsOfInterestTex["TransferableUIUtility"] = new List<FOI>() { new FOI("TradeArrow", NPS), new FOI("DividerTex", NPS), new FOI("PregnantIcon", NPS), new FOI("BondIcon", NPS) };
            fieldsOfInterestTex["Dialog_ManageDrugPolicies"] = new List<FOI>() { new FOI("IconForAddiction", NPS), new FOI("IconForJoy", NPS), new FOI("IconScheduled", NPS) };
            fieldsOfInterestTex["FormCaravanComp"] = new List<FOI>() { new FOI("FormCaravanCommand") };
            fieldsOfInterestTex["SelectionDrawerUtility"] = new List<FOI>() { new FOI("SelectedTexGUI") };
            fieldsOfInterestTex["CaravanVisitUtility"] = new List<FOI>() { new FOI("TradeCommandTex",NPS) };
            fieldsOfInterestTex["HediffComp_Immunizable"] = new List<FOI>() { new FOI("IconImmune",NPS) };
            fieldsOfInterestTex["SettlementAbandonUtility"] = new List<FOI>() { new FOI("AbandonCommandTex",NPS) };
            fieldsOfInterestTex["Dialog_Trade"] = new List<FOI>() { new FOI("ShowSellableItemsIcon", NPS), new FOI("GiftModeIcon", NPS), new FOI("TradeModeIcon", NPS) };
            fieldsOfInterestTex["CaravanMergeUtility"] = new List<FOI>() { new FOI("MergeCommandTex", NPS) };
            fieldsOfInterestTex["Designator_Dropdown"] = new List<FOI>() { new FOI("PlusTex") };
            fieldsOfInterestTex["Frame"] = new List<FOI>() { new FOI("CornerTex",NPS), new FOI("TileTex", NPS) };
            fieldsOfInterestTex["FactionGiftUtility"] = new List<FOI>() { new FOI("OfferGiftsCommandTex", NPS)};
            fieldsOfInterestTex["MapParent"] = new List<FOI>() { new FOI("ShowMapCommand", NPS) };
            fieldsOfInterestTex["Command_SetPlantToGrow"] = new List<FOI>() { new FOI("SetPlantToGrowTex", NPS) };
            fieldsOfInterestTex["WorldRoutePlanner"] = new List<FOI>() { new FOI("ButtonTex", NPS), new FOI("MouseAttachment", NPS) };
            fieldsOfInterestTex["CompLaunchable"] = new List<FOI>() { new FOI("TargeterMouseAttachment"), new FOI("LaunchCommandTex") };
            fieldsOfInterestTex["HealthCardUtility"] = new List<FOI>() { new FOI("BleedingIcon", NPS) };
            fieldsOfInterestTex["TransferableOneWayWidget"] = new List<FOI>() { new FOI("CanGrazeIcon", NPS) };


            //Definition of relative NS
            //fieldsOfInterestTexNS["UI_BackgroundMain"] = "RimWorld";
            fieldsOfInterestTexNS["TexCommand"] = "RimWorld";
            fieldsOfInterestTexNS["WidgetsWork"] = "RimWorld";
            fieldsOfInterestTexNS["ColonistBarColonistDrawer"] = "RimWorld";
            fieldsOfInterestTexNS["MainMenuDrawer"] = "RimWorld";
            fieldsOfInterestTexNS["HostilityResponseModeUtility"] = "RimWorld";
            fieldsOfInterestTexNS["PawnColumnWorker_Bond"] = "RimWorld";
            fieldsOfInterestTexNS["PawnColumnWorker_Predator"] = "RimWorld";
            fieldsOfInterestTexNS["Thought"] = "RimWorld";
            fieldsOfInterestTexNS["CompTransporter"] = "RimWorld";
            fieldsOfInterestTexNS["TrainingCardUtility"] = "RimWorld";
            fieldsOfInterestTexNS["PawnColumnWorker_Predator"] = "RimWorld";
            fieldsOfInterestTexNS["CaravanThingsTabUtility"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["Settlement"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["PawnColumnWorker"] = "RimWorld";
            fieldsOfInterestTexNS["SettleUtility"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["PawnColumnWorker_Pregnant"] = "RimWorld";
            fieldsOfInterestTexNS["SkillUI"] = "RimWorld";
            fieldsOfInterestTexNS["Need"] = "RimWorld";
            fieldsOfInterestTexNS["CompassWidget"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["TransferableUIUtility"] = "RimWorld";
            fieldsOfInterestTexNS["Dialog_ManageDrugPolicies"] = "RimWorld";
            fieldsOfInterestTexNS["FormCaravanComp"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["SelectionDrawerUtility"] = "RimWorld";
            fieldsOfInterestTexNS["CaravanVisitUtility"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["SettlementAbandonUtility"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["Dialog_Trade"] = "RimWorld";
            fieldsOfInterestTexNS["CaravanMergeUtility"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["Designator_Dropdown"] = "RimWorld";
            fieldsOfInterestTexNS["Frame"] = "RimWorld";
            fieldsOfInterestTexNS["FactionGiftUtility"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["MapParent"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["WorldRoutePlanner"] = "RimWorld.Planet";
            fieldsOfInterestTexNS["CompLaunchable"] = "RimWorld";
            fieldsOfInterestTexNS["HealthCardUtility"] = "RimWorld";
            fieldsOfInterestTexNS["TransferableOneWayWidget"] = "RimWorld";
            fieldsOfInterestTexNS["InspectPaneUtility"] = "RimWorld";

            /************************************************************************* Colors **********************************************************************/
            fieldsOfInterestColor["SocialCardUtility"] = new List<FOI>()
            {
                new FOI("RelationLabelColor",NPS), new FOI("PawnLabelColor",NPS),new FOI("HighlightColor",NPS)
            };
            //Init of the list of color variables of interest
            fieldsOfInterestColor["Widgets"] = new List<FOI>() {
                new FOI("WindowBGBorderColor",NPS), new FOI("WindowBGFillColor"), new FOI("MenuSectionBGFillColor"), new FOI("MenuSectionBGBorderColor",NPS), new FOI("TutorWindowBGFillColor",NPS),
                new FOI("TutorWindowBGBorderColor",NPS),
                new FOI("OptionUnselectedBGFillColor",NPS),new FOI("OptionUnselectedBGBorderColor",NPS),new FOI("OptionSelectedBGFillColor",NPS),new FOI("OptionSelectedBGBorderColor",NPS),
                new FOI("NormalOptionColor"),new FOI("MouseoverOptionColor"),new FOI("SeparatorLineColor"), new FOI("SeparatorLabelColor")
            };
            fieldsOfInterestColor["GenUI"] = new List<FOI>() {
                new FOI("MouseoverColor"),new FOI("SubtleMouseoverColor")
            };
            fieldsOfInterestColor["FloatMenuOption"] = new List<FOI>()
            {
                new FOI("ColorBGActiveMouseover",NPS), new FOI("ColorBGActive",NPS), new FOI("ColorBGDisabled",NPS),new FOI("ColorTextActive",NPS),new FOI("ColorTextDisabled",NPS)
            };

            fieldsOfInterestColor["HealthCardUtility"] = new List<FOI>()
            {
                new FOI("HighlightColor",NPS), new FOI("StaticHighlightColor",NPS), new FOI("MediumPainColor",NPS),new FOI("SeverePainColor",NPS)
            };
            fieldsOfInterestColor["Dialog_SaveFileList"] = new List<FOI>() { new FOI("AutosaveTextColor", NPS) };
            fieldsOfInterestColor["Dialog_FileList"] = new List<FOI>() { new FOI("DefaultFileTextColor", NPS) };
            fieldsOfInterestColor["SaveFileInfo"] = new List<FOI>() { new FOI("UnimportantTextColor") };
            fieldsOfInterestColor["PawnNameColorUtility"] = new List<FOI>()
            {
                new FOI("ColorBaseNeutral",NPS), new FOI("ColorBaseHostile",NPS), new FOI("ColorBasePrisoner",NPS),new FOI("ColorColony",NPS),new FOI("ColorWildMan",NPS)
            };
            fieldsOfInterestColor["ITab_Pawn_Gear"] = new List<FOI>() { new FOI("ThingLabelColor"), new FOI("HighlightColor") };
            fieldsOfInterestColor["TransferableOneWayWidget"] = new List<FOI>() { new FOI("ItemMassColor") };

            fieldsOfInterestColor["HealthUtility"] = new List<FOI>() { new FOI("ImpairedColor"), new FOI("SlightlyImpairedColor"),
                new FOI("RedColor"), new FOI("GoodConditionColor") };

            fieldsOfInterestColor["NeedsCardUtility"] = new List<FOI>() { new FOI("MoodColor", NPS), new FOI("MoodColorNegative", NPS), new FOI("NoEffectColor", NPS),
            };

            fieldsOfInterestColor["Hediff_Injury"] = new List<FOI>() { new FOI("PermanentInjuryColor", NPS)
            };
            fieldsOfInterestColor["ColoredText"] = new List<FOI>()
            {
                new FOI("NameColor"), new FOI("CurrencyColor"),new FOI("DateTimeColor"),new FOI("FactionColor_Ally"),
                new FOI("FactionColor_Hostile"), new FOI("ThreatColor"), new FOI("FactionColor_Neutral"),
                new FOI("WarningColor"), new FOI("ColonistCountColor")
            };

            fieldsOfInterestColor["ColorLibrary"] = new List<FOI>()
            {
                new FOI("RedReadable"),new FOI("Red"),new FOI("Brown"),new FOI("Pink"),new FOI("Blue"),new FOI("Green")
                ,new FOI("Purple"),new FOI("Black"),new FOI("Violet"),new FOI("Teal"),new FOI("Grey"),new FOI("Magenta")
                ,new FOI("Orange"),new FOI("Yellow"),new FOI("Leather"),new FOI("LightPurple"),new FOI("LimeGreen"),new FOI("SkyBlue")
                ,new FOI("LightGreen"),new FOI("LightBlue"),new FOI("DarkOrange"),new FOI("Sand"),new FOI("PastelGreen"),new FOI("Mint")
                ,new FOI("LightOrange"),new FOI("BrightPink"),new FOI("DeepPurple"),new FOI("DarkBrown"),new FOI("Taupe"),new FOI("PeaGreen")
                ,new FOI("PukeGreen"),new FOI("BlueGreen"),new FOI("Khaki"),new FOI("Burgundy"),new FOI("DarkTeal"),new FOI("BrickRed")
                ,new FOI("RoyalPurple"),new FOI("Plum"),new FOI("Gold"),new FOI("BabyBlue"),new FOI("YellowGreen"),new FOI("BrightPurple")
                ,new FOI("DarkRed"),new FOI("PaleBlue"),new FOI("GrassGreen"),new FOI("Navy"),new FOI("Aquamarine"),new FOI("BurntOrange")
                ,new FOI("NeonGreen"),new FOI("BrightBlue"),new FOI("Rose"),new FOI("LightPink"),new FOI("Mustard"),new FOI("Indigo")
                ,new FOI("Lime"),new FOI("DarkPink"),new FOI("OliveGreen"),new FOI("Peach"),new FOI("PaleGreen"),new FOI("LightBrown")
                ,new FOI("HotPink"),new FOI("Lilac"),new FOI("NavyBlue"),new FOI("RoyalBlue"),new FOI("Beige"),new FOI("Salmon")
                ,new FOI("Olive"),new FOI("Maroon"),new FOI("BrightGreen"),new FOI("Mauve"),new FOI("ForestGreen"),new FOI("Aqua")
                ,new FOI("Cyan"),new FOI("Tan"),new FOI("DarkBlue"),new FOI("Lavender"),new FOI("Lavender"),new FOI("DarkGreen")
                ,new FOI("LogError")

            };



            fieldsOfInterestColorNS["Dialog_SaveFileList"] = "RimWorld";
            fieldsOfInterestColorNS["Dialog_FileList"] = "RimWorld";
            fieldsOfInterestColorNS["HealthCardUtility"] = "RimWorld";
            fieldsOfInterestColorNS["ITab_Pawn_Gear"] = "RimWorld";
            fieldsOfInterestColorNS["TransferableOneWayWidget"] = "RimWorld";
            fieldsOfInterestColorNS["SocialCardUtility"] = "RimWorld";
            fieldsOfInterestColorNS["HealthUtility"] = "Verse";
            fieldsOfInterestColorNS["NeedsCardUtility"] = "RimWorld";
            fieldsOfInterestColorNS["Hediff_Injury"] = "Verse";
            fieldsOfInterestColorNS["ColoredText"] = "Verse";
            fieldsOfInterestColorNS["ColorLibrary"] = "Verse";

        }

        public static void startInit()
        {
            try
            {
                LoaderGM.curStep = LoaderSteps.loadingTheme;

                //Search for theme in mod folder
                string themesDir = Utils.currentMod.RootDir + Path.DirectorySeparatorChar + "Themes" + Path.DirectorySeparatorChar;
                DBModInfo["-1"] = new Dictionary<string, string>();
                DBModInfo["-1"]["name"] = "";

                loadThemesInFolder("-1", themesDir, true);
            }
            catch(Exception e)
            {
                Themes.LogException("Cannot load core : ", e);
                return;
            }

            //Loading themes at the root of the Rimworld folder
            try
            {
                //Search for theme in mod folder
                string themesDir = Utils.RWBaseFolderPath + Path.DirectorySeparatorChar + "RimThemes" + Path.DirectorySeparatorChar;
                if (Directory.Exists(themesDir))
                {
                    DBModInfo["-2"] = new Dictionary<string, string>();
                    DBModInfo["-2"]["name"] = "";

                    loadThemesInFolder("-2", themesDir, true);
                }
            }
            catch (Exception e)
            {
                Themes.LogException("Cannot load default themes : ", e);
            }

            //Theme search in other mods

            try
            {
                List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
                for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
                {
                    ModContentPack cmod = runningModsListForReading[i];
                    var curMod = cmod.RootDir + Path.DirectorySeparatorChar + "RimThemes";
                    //Theme folder found
                    if (Directory.Exists(curMod))
                    {
                        try
                        {
                            //Mod info info
                            DBModInfo[cmod.PackageId] = new Dictionary<string, string>();
                            DBModInfo[cmod.PackageId]["name"] = cmod.Name;


                            loadThemesInFolder(cmod.PackageId, curMod, false);
                        }
                        catch (Exception e)
                        {
                            Themes.LogException("Cannot load " + cmod.Name + "'s themes : ", e);
                        }
                    }
                }

                //ActiveTheme = new ActiveTheme(Settings.curTheme); // load vanilla ActiveTheme

                //Custom cursor application if applicable
                CustomCursor.Deactivate();
                CustomCursor.Activate();

                //Check if the currently selected theme exists (may not exist due to offline steam or removed mod theme)
                if (!DBAvailableThemes.Contains(Settings.curTheme))
                    currentThemeExist = false;

                //We define that everything is ok, the patches can do their job
                initialized = true;
            }
            catch (Exception e)
            {
                Themes.LogException(" Unhandled error at startInit : ", e);
            }
        }


        /*
         * Find and load themes in the specified folder
         */
        private static void loadThemesInFolder(string modID,string themesDir, bool prefix)
        {
            LogMsg("Searching themes in " + themesDir+"...");

            string fullPathRoot;
            string[] folders = System.IO.Directory.GetDirectories(themesDir);
            LogMsg("Found " + folders.Length + " themes");
            try
            {
                foreach (var dir in folders)
                {
                    var theme = dir.Remove(0, dir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    string themeID = modID + "§" + theme;

                    fullPathRoot = themesDir + Path.DirectorySeparatorChar + theme + Path.DirectorySeparatorChar;

                    LoaderGM.curTheme = theme;

                    LogMsg("Loading theme : " + theme);


                    RDBTex[themeID] = new Dictionary<string, Dictionary<string, byte[]>>();


                    //Loading theme icon
                    string iconFP = fullPathRoot + "Misc" + Path.DirectorySeparatorChar + "Icon.png";
                    if (File.Exists(iconFP))
                    {
                        RDBTexThemeIcon[themeID] = rawBytesLoad(iconFP);
                    }
                    else
                    {
                        //Define the default theme icon
                        DBTexThemeIcon[themeID] = Loader.defaultIconTex;
                    }

                    //Particle loading if applicable
                    string ParticleFP = fullPathRoot + "Misc" + Path.DirectorySeparatorChar + "Particle.png";
                    if (File.Exists(ParticleFP))
                        RDBTexParticle[themeID] = rawBytesLoad(ParticleFP);
                    else
                        RDBTexParticle[themeID] = null;

                    //Upholstery loading if applicable
                    string tapestryFP = fullPathRoot + "Misc" + Path.DirectorySeparatorChar + "Tapestry";
                    if (Utils.texFileExist(tapestryFP))
                        RDBTexTapestry[themeID] = Utils.readAllBytesTexFile(tapestryFP);
                    else
                        RDBTexTapestry[themeID] = null;

                    //Loading loading screen loader texture
                    string loaderBarFP = fullPathRoot + "Loader" + Path.DirectorySeparatorChar + "LoaderBar.png";
                    if (File.Exists(loaderBarFP))
                        RDBTexLoaderBar[themeID] = rawBytesLoad(loaderBarFP);
                    else
                        RDBTexLoaderBar[themeID] = null;


                    //Loading texture background from the loader bar
                    string loaderBarTextFP = fullPathRoot + "Loader" + Path.DirectorySeparatorChar + "TextBar.png";
                    if (File.Exists(loaderBarTextFP))
                        RDBTexLoaderText[themeID] = rawBytesLoad(loaderBarTextFP);
                    else
                        RDBTexLoaderText[themeID] = null;

                    //Loading custom textures
                    string texPath = fullPathRoot + Path.DirectorySeparatorChar + "Textures";
                    string imgExt = "";
                    //Theme folder found
                    try
                    {
                        if (Directory.Exists(texPath))
                        {
                            string[] texFiles = System.IO.Directory.GetFiles(texPath);
                            foreach (var tex in texFiles)
                            {
                                imgExt = Path.GetExtension(tex);
                                //If not expected image format we squeeze the current file
                                if (imgExt == null || !Utils.isValidImgExt(imgExt.ToLower()))
                                    continue;
                                //Filename deduction without extension
                                string curTexName = Path.GetFileNameWithoutExtension(tex);

                                //Get class name and field name
                                if (curTexName.IndexOf('.') == -1)
                                    LogMsg("Bad tex name for " + curTexName + " cannot get className and fieldName");
                                else
                                {
                                    string[] tmp = getClassNameAndFileName(curTexName);

                                    if (tmp.Length != 2)
                                    {
                                        LogMsg("Bad tex name for " + curTexName);
                                    }
                                    else
                                    {
                                        if (!RDBTex[themeID].ContainsKey(tmp[0]) || RDBTex[themeID][tmp[0]] == null)
                                            RDBTex[themeID][tmp[0]] = new Dictionary<string, byte[]>();

                                        //Attempt to get the property dynamically to verify sound validity
                                        try
                                        {
                                            RDBTex[themeID][tmp[0]][tmp[1]] = rawBytesLoad(tex);
                                        }
                                        catch (Exception e)
                                        {
                                            Themes.LogException("Invalid custom texture " + curTexName + " : ", e);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Themes.LogException("Fatal error in custom textures processing : ", e);
                    }

                    //If available loading the animated background
                    string animatedPath = dir + Path.DirectorySeparatorChar + "Textures" + Path.DirectorySeparatorChar + "UI_BackgroundMain.BGPlanet.webm";
                    if (File.Exists(animatedPath))
                    {
                        try
                        {
                            DBAnimatedBackground[themeID] = animatedPath;
                            Themes.LogMsg("Found and loading animated background "+ animatedPath);
                        }
                        catch(Exception e)
                        {
                            Themes.LogError("Error while trying to load animated background : "+e.Message);
                        }
                    }


                    //Loading the meta storage file
                    string metaPath = dir + Path.DirectorySeparatorChar + "meta.xml";

                    //Loading metas from the current theme
                    processMetaXML(themeID,metaPath);

                    //Applying the default choices of theme categories
                    if (!DBText[themeID].ContainsKey("category") || !Dialog_ThemesList.categoriesID.Contains<string>(DBText[themeID]["category"]) )
                    {
                        DBText[themeID]["category"] = "other";
                    }

                    //Loading custom sounds
                    string soundPath = dir + Path.DirectorySeparatorChar + "Sounds";
                    //Theme folder found
                    try
                    {
                        if (Directory.Exists(soundPath))
                        {
                            Type ctype = Type.GetType(typeof(SoundDefOf).AssemblyQualifiedName);
                            DBSound[themeID] = new Dictionary<string, AudioGrain_ClipTheme>();

                            string[] soundFiles = System.IO.Directory.GetFiles(soundPath);
                            foreach (var sound in soundFiles)
                            {
                                //Filename deduction without extension
                                string curSoundName = Path.GetFileNameWithoutExtension(sound);

                                //Attempt to get the property dynamically to verify sound validity
                                try
                                {
                                    FieldInfo fi = ctype.GetField(curSoundName);
                                    SoundDef entry = (SoundDef)fi.GetValue(null);

                                    //bool doStream = ShouldStreamAudioClipFromPath(sound);
                                    DBSound[themeID][curSoundName] = new AudioGrain_ClipTheme();
                                    DBSound[themeID][curSoundName].themeClipPath = sound;
                                    //DBSound[themeID][curSoundName] = (AudioClip)((object)Manager.Load(sound, doStream, true, true));
                                    LogMsg("Loading custom sound " + curSoundName + " OK");
                                }
                                catch (Exception e)
                                {
                                    Themes.LogException("Invalid custom sound " + curSoundName + " : ", e);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Themes.LogException("Fatal error in custom sound processing : ", e);
                    }


                    //Constitution of the list of custom music to load
                    string songPath = dir + Path.DirectorySeparatorChar + "Songs";
                    string ext;
                    //Theme folder found
                    try
                    {
                        if (Directory.Exists(songPath))
                        {
                            DBSong[themeID] = new Dictionary<string, AudioClip>();

                            string[] songFiles = System.IO.Directory.GetFiles(songPath);
                            DBSongsToLoad[themeID] = new Dictionary<string, string>();

                            foreach (string cfsong in songFiles)
                            {
                                //string cfsong = songPath + Path.DirectorySeparatorChar + "EntrySong.ogg";
                                ext = Path.GetExtension(cfsong);
                                if (File.Exists(cfsong) &&  ext != null && ext.ToLower() == ".ogg" )
                                {
                                    //Filename deduction without extension
                                    string curSongName = Path.GetFileNameWithoutExtension(cfsong);
                                    //Addition to the list of songs to load
                                    DBSongsToLoad[themeID][curSongName] = cfsong;
                                    LogMsg("Append " + curSongName + " song to preload songs list");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Themes.LogException("Fatal error in custom song pre-processing : ", e);
                    }

                    //Addition of the theme to the list of available themes
                    DBAvailableThemes.Add(themeID);
                }

                //Check if the font pack is available, if necessary save for loading in the main thread
                string fontBundlePath = themesDir + Path.DirectorySeparatorChar + "fontsPackage";
                //Theme folder found
                try
                {
                    if (File.Exists(fontBundlePath))
                    {
                        DBfontsBundleToLoad.Add(fontBundlePath);
                        Themes.LogMsg("Found a font bundle");
                    }
                }
                catch (Exception e)
                {
                    Themes.LogException("Fatal error in custom fonts package pre-processing : ", e);
                }
            }
            catch(Exception e)
            {
                Themes.LogException("Fatal error in loadThemesInFolder : ", e);
            }
        }

        public static void processMetaXML(string themeID, string metaPath)
        {
            DBDynColor[themeID] = new Dictionary<Color, Color>();
            DBColor[themeID] = new Dictionary<string, Dictionary<string, Color>>();
            DBText[themeID] = new Dictionary<string, string>();
            if (!DBVal.ContainsKey(themeID))
            {
                DBVal[themeID] = new Dictionary<string, int>();
                DBVal[themeID]["loaderfps"] = 20;
            }

            //Check existence of meta file
            if (File.Exists(metaPath))
            {
                if(RDBTex.ContainsKey(themeID))
                    RDBTex[themeID]["buttonbarcolor"] = null;

                DBText[themeID]["description"] = "";

                DBGUIStyle[themeID] = null;

                //Buffer dictionary used to store customFontX sequentially before pushing into the fontsToLoad list
                Dictionary<string, string> fontToLoadEntry = new Dictionary<string, string>();

                DBColor[themeID][MiscKey] = new Dictionary<string, Color>();
                try
                {
                    using (var reader = XmlReader.Create(@metaPath))
                    {
                        reader.MoveToContent();
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                var tag = reader.Name;
                                var value = reader.ReadString();

                                //Log.Message(tag+" "+value);
                                //Tab mark containing a color ('.') (ClassName and fieldName concatenated)
                                if (tag.IndexOf('.') != -1)
                                {
                                    string[] tmp = getClassNameAndFileName(tag);
                                    if (tmp.Length != 2)
                                        LogMsg("Bad color tag " + tag);
                                    else
                                    {
                                        if (!DBColor[themeID].ContainsKey(tmp[0]) || DBColor[themeID][tmp[0]] == null)
                                            DBColor[themeID][tmp[0]] = new Dictionary<string, Color>();
                                        try
                                        {
                                            DBColor[themeID][tmp[0]][tmp[1]] = ParseHelper.FromString<Color>(value);
                                        }
                                        catch (Exception e)
                                        {
                                            Themes.LogException("Cannot parse field " + tag + " : ", e);
                                        }
                                    }
                                }
                                else
                                {
                                    tag = tag.ToLower().Trim();
                                    value = value.Trim();
                                    //return only when you have START tag  
                                    try
                                    {
                                        switch (tag)
                                        {
                                            case "textcolorwhite":
                                            case "textcolorcyan":
                                            case "textcolorgray":
                                            case "textcolorred":
                                            case "textcoloryellow":
                                            case "textcolorblue":
                                            case "textcolorgreen":
                                            case "textcolormangenta":
                                                try
                                                {
                                                    Color c = ParseHelper.FromString<Color>(value);
                                                    switch (tag)
                                                    {
                                                        case "textcolorwhite":
                                                            DBTextColorWhite[themeID] = c;
                                                            break;
                                                        case "textcolorcyan":
                                                            DBTextColorCyan[themeID] = c;
                                                            break;
                                                        case "textcolorgray":
                                                            DBTextColorGray[themeID] = c;
                                                            break;
                                                        case "textcolorred":
                                                            DBTextColorRed[themeID] = c;
                                                            break;
                                                        case "textcoloryellow":
                                                            DBTextColorYellow[themeID] = c;
                                                            break;
                                                        case "textcolorblue":
                                                            DBTextColorBlue[themeID] = c;
                                                            break;
                                                        case "textcolorgreen":
                                                            DBTextColorGreen[themeID] = c;
                                                            break;
                                                        case "textcolormagenta":
                                                            DBTextColorMagenta[themeID] = c;
                                                            break;
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    LogMsg("Cannot parse textcolor white field " + tag + " : " + e.Message);
                                                }
                                                break;
                                            case "texturecolorwhite":
                                            case "texturecolorcyan":
                                            case "texturecolorgray":
                                            case "texturecolorred":
                                            case "texturecoloryellow":
                                            case "texturecolorblue":
                                            case "texturecolorgreen":
                                            case "texturecolormangenta":
                                                try
                                                {
                                                    Color c = ParseHelper.FromString<Color>(value);
                                                    switch (tag)
                                                    {
                                                        case "texturecolorwhite":
                                                            DBTexColorWhite[themeID] = c;
                                                            break;
                                                        case "texturecolorcyan":
                                                            DBTexColorCyan[themeID] = c;
                                                            break;
                                                        case "texturecolorgray":
                                                            DBTexColorGray[themeID] = c;
                                                            break;
                                                        case "texturecolorred":
                                                            DBTexColorRed[themeID] = c;
                                                            break;
                                                        case "texturecoloryellow":
                                                            DBTexColorYellow[themeID] = c;
                                                            break;
                                                        case "texturecolorblue":
                                                            DBTexColorBlue[themeID] = c;
                                                            break;
                                                        case "texturecolorgreen":
                                                            DBTexColorGreen[themeID] = c;
                                                            break;
                                                        case "textureolormagenta":
                                                            DBTexColorMagenta[themeID] = c;
                                                            break;
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    LogMsg("Cannot parse texureColor white field " + tag + " : " + e.Message);
                                                }
                                                break;
                                            case "textcolorfactionneutral":
                                                try
                                                {
                                                    Color c = ParseHelper.FromString<Color>(value);
                                                    DBTextColorFactionsNeutral[themeID] = c;
                                                }
                                                catch (Exception e)
                                                {
                                                    LogMsg("Cannot parse textcolor faction neutral : " + e.Message);
                                                }
                                                break;
                                            //Old tag for adding a font not dependent on a language and sizes according to the 3 sizes of RimWorld
                                            case "font":
                                                //var d = new Dictionary<string, string>();
                                                fontToLoadEntry["font"] = value;
                                                fontToLoadEntry["themeID"] = themeID;
                                                //fontsToLoad.Add(d);
                                                break;
                                            case "windowanim":
                                                string v = value.ToLower();
                                                if (v == "clip")
                                                    DBWindowAnim[themeID] = WindowAnim.Clip;
                                                else if (v == "slideleft")
                                                    DBWindowAnim[themeID] = WindowAnim.SlideLeft;
                                                else if (v == "slideright")
                                                    DBWindowAnim[themeID] = WindowAnim.SlideRight;
                                                else if (v == "slidetop")
                                                    DBWindowAnim[themeID] = WindowAnim.SlideTop;
                                                else if (v == "slidebottom")
                                                    DBWindowAnim[themeID] = WindowAnim.SlideBottom;
                                                else
                                                    DBWindowAnim[themeID] = WindowAnim.None;
                                                break;
                                            case "particlemode":
                                            case "buttonnotex":
                                            case "buttonnobg":
                                            case "buttonusecolor":
                                            case "dialogstacking":
                                            case "disablemainrimthemeslogo":
                                            case "disablemainthemeselector":
                                            case "category":
                                            case "tapestryscalemode":
                                                DBText[themeID][tag] = value.ToLower();
                                                break;
                                            case "buttonfillcolor":
                                            case "buttonhovercolor":
                                            case "buttonclickcolor":
                                            case "tapestrybordercolor":
                                                DBColor[themeID][MiscKey][tag] = ParseHelper.FromString<Color>(value);
                                                break;
                                            case "dyncolor":
                                                string[] parts = value.Split('=');
                                                if (parts.Length == 2)
                                                {
                                                    Themes.LogMsg("Found dynColor " + parts[0] + " => " + parts[1]);
                                                    Color c1 = ParseHelper.FromString<Color>(parts[0]);
                                                    Color c2 = ParseHelper.FromString<Color>(parts[1]);
                                                    DBDynColor[themeID][c1] = c2;
                                                }
                                                break;
                                            case "disabletransparenttext":
                                            case "menuspecialmode":
                                            case "loaderfps":
                                                int num = 0;
                                                bool res = int.TryParse(value, out num);
                                                if (res)
                                                    DBVal[themeID][tag] = num;
                                                else
                                                    Themes.LogError("DBVal : value '" + value + "' of tag '" + tag + "' is not a valid number");
                                                break;
                                            default:
                                                DBText[themeID][tag] = value;

                                                //Attempt to parse a potential customFontX
                                                Fonts.tryParseCustomFontTag(fontToLoadEntry, themeID, tag, value);
                                                break;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Themes.LogException("Cannot parse tag " + tag + " : ", e);
                                    }
                                }
                            }
                        }
                    }
                    //IF customFontX buffering dictionary not empty then pushing into fontsToLoad
                    if (fontToLoadEntry.Count != 0)
                        fontsToLoad.Add(fontToLoadEntry);
                }
                catch (Exception e)
                {
                    Themes.LogException("Cannot parse meta.xml ", e);
                }
            }
        }


        public static Texture2D customTexLoad(string fullPath, int w=64, int h=64)
        {
            var texture = new Texture2D(w, h, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(fullPath));
            return texture;
        }

        public static byte[] rawBytesLoad(string fullPath)
        {
            return File.ReadAllBytes(fullPath);
        }

        public static void changeThemeNow(string modID, string themeName, bool manualChange)
        {
            string newTheme = modID + "§" + themeName;
            changeThemeNow(newTheme, manualChange);
        }

        public static void changeThemeNow(string newTheme,bool manualChange=false)
        {
            try
            {
                //Themes selected from a list made by RT, it is supposed to exist (if it is a manual change)
                if (manualChange)
                    Themes.currentThemeExist = true;
                string theme = Settings.curTheme;
                if (!vanillaThemeSaved)
                {
                    LogMsg("Saving vanilla theme...");
                    saveVanillaRsc<Texture2D>(DBTex);
                    saveVanillaRsc<Color>(DBColor);
                    DBDynColor[VanillaThemeID] = new Dictionary<Color, Color>();
                    DBText[VanillaThemeID] = new Dictionary<string, string>();
                    DBText[VanillaThemeID]["description"] = "Want some old school interface ?";
                    LogMsg("Saving vanilla theme ending");
                    vanillaThemeSaved = true;

                    Utils.applyWindowFillColorOpacityOverride(newTheme);

                    //Application theme at startup, if theme vanilla does not need to be loaded it is already the case
                    if (theme == VanillaThemeID)
                    {
                        ActiveTheme = new ActiveTheme(theme);
                        return;
                    }
                }

                //Vanilla base theme restore only if current theme not vanilla
                if (theme != VanillaThemeID)
                {
                    changeTheme<Texture2D>(DBTex, VanillaThemeID);
                    changeTheme<Color>(DBColor, VanillaThemeID);

                    //Application theme to other mods too
                    changeThemeOtherMods(VanillaThemeID);
                }

                stopCurrentAnimatedBackground();

                //Vanilla crush with the new theme
                changeTheme<Texture2D>(DBTex, newTheme);
                changeTheme<Color>(DBColor, newTheme);

                //Apply override:
                Utils.applyWindowFillColorOpacityOverride(newTheme);

                //Apply theme to other mods too
                changeThemeOtherMods(newTheme);

                //Reset cache label sizes if applicable
                Utils.resetCachedLabelWidthCache();

                //Save theme change
                Settings.curTheme = newTheme;
                ActiveTheme = new ActiveTheme(Settings.curTheme);
                Utils.modSettings.Write();

                changeSoundTheme();
                changeSongTheme();

                //Mouse cursor refresh if applicable
                CustomCursor.Deactivate();
                CustomCursor.Activate();

                //We require a reloading of the BG
                Utils.needRefresh = true;
            }
            catch(Exception e)
            {
                Themes.LogError("Error while trying to change theme : "+e.Message);
            }
        }
        
        /*
         * Change theme song
         */
        public static void changeSongTheme()
        {
            var theme = Settings.curTheme;

            //If custom sounds disabled then forcing the Vanilla theme sound
            if (Settings.disableCustomSongs)
                theme = VanillaThemeID;

            //If vanilla beans never saved, we proceed immediately
            if (!vanillaSongGrainsSaved)
            {
                try
                {
                    if (!DBSong.ContainsKey(VanillaThemeID))
                        DBSong[VanillaThemeID] = new Dictionary<string, AudioClip>();

                    DBSong[VanillaThemeID]["EntrySong"] = SongDefOf.EntrySong.clip;
                }
                catch (Exception e)
                {
                    Themes.LogError("Cannot save vanilla song "+ e.Message);
                }
                vanillaSongGrainsSaved = true;
            }
            else
            {
                //We restore the vanilla beans
                SongDefOf.EntrySong.clip = DBSong[VanillaThemeID]["EntrySong"];
            }


            if (!DBSong.TryGetValue(theme, out var songs) || songs == null)
                theme = VanillaThemeID;

            try
            {
                //New grain definition
                SongDefOf.EntrySong.clip = songs["EntrySong"];
                var ctype = typeof(Verse.Current);

                var fi = ctype.GetField("rootEntryInt", BindingFlags.NonPublic | BindingFlags.Static);
                var re = (Root_Entry)fi.GetValue(null);
                //Root_Entry re = (Root_Entry) Traverse.Create("Current").Field("rootEntryInt").GetValue();
                /*if (re == null)
                    Log.Message("re == null");*/
                var musicManagerEntry = (MusicManagerEntry)Traverse.Create(re).Field("musicManagerEntry").GetValue();
                /*if (musicManagerEntry == null)
                    Log.Message("Music manager == null");*/
                var audio = (AudioSource)Traverse.Create(musicManagerEntry).Field("audioSource").GetValue();
                /*if (audio == null)
                    Log.Message("audio == null");*/
                if (audio != null)
                {
                    audio.Stop();
                    audio.clip = songs["EntrySong"];
                    audio.Play();
                }
            }
            catch (Exception e)
            {
                Themes.LogException("changeSoundTheme : ", e);
            }
        }



        /*
         * Change of the sound theme (dynamic alteration of SoundDefOf)
         */
        public static void changeSoundTheme()
        {
            string theme = Settings.curTheme;

            //If custom sounds disabled then forcing the Vanilla theme sound
            if (Settings.disableCustomSounds)
                theme = VanillaThemeID;

            //If vanilla grains never saved, we proceed immediately
            if (!vanillaGrainsSaved)
            {
                saveVanillaGrains();
                vanillaGrainsSaved = true;
            }
            else
            {
                //We restore the vanilla grains
                restoreVanilaGrains();
            }

            if (!DBSound.ContainsKey(theme))
                return;

            Type ctype = typeof(SoundDefOf);

            foreach (var (propName, value) in DBSound[theme])
            {
                try
                {
                    FieldInfo fi = ctype.GetField(propName);
                    SoundDef entry = (SoundDef)fi.GetValue(null);

                    //New grain definition
                    entry.subSounds[0].grains[0] = value;
                    //Resolving new grains
                    entry.subSounds[0].ResolveReferences();
                }
                catch(Exception e)
                {
                    Themes.LogException("changeSoundTheme : ", e);
                }
            }
        }

        /*
         * Saving the resolved grains of Vanilla
         */
        public static void saveVanillaGrains()
        {
            Type ctype = typeof(SoundDefOf);
            FieldInfo[] fields = ctype.GetFields();

            foreach(var field in fields)
            {
                try
                {
                    SoundDef entry = (SoundDef)field.GetValue(null);
                    if (entry.subSounds.Count != 0)
                    {
                        List<ResolvedGrain> resolvedGrains = (List<ResolvedGrain>)Traverse.Create(entry.subSounds[0]).Field("resolvedGrains").GetValue();
                        DBVanillaResolvedGrains[field.Name] = new List<ResolvedGrain>();
                        //Grain removal (references)
                        foreach (var grain in resolvedGrains)
                        {
                            DBVanillaResolvedGrains[field.Name].Add(grain);
                        }

                    }
                    else
                    {
                        DBVanillaResolvedGrains[field.Name] = null;
                    }
                }
                catch(Exception e)
                {
                    if(field != null)
                        Themes.LogError("Cannot save vanilla grain "+field.Name+" : "+e.Message);
                }
            }
        }

        public static void restoreVanilaGrains()
        {
            Type ctype = typeof(SoundDefOf);
            FieldInfo[] fields = ctype.GetFields();
            foreach (var field in fields)
            {
                SoundDef entry = (SoundDef)field.GetValue(null);
                if (DBVanillaResolvedGrains[field.Name] != null)
                {
                    List<ResolvedGrain> resolvedGrains = (List<ResolvedGrain>)Traverse.Create(entry.subSounds[0]).Field("resolvedGrains").GetValue();
                    resolvedGrains.Clear();
                    foreach (var grain in DBVanillaResolvedGrains[field.Name])
                    {
                        resolvedGrains.Add(grain);
                    }
                }
            }
        }



        /*
         * Change of textures with respect to the current theme
         */
        public static void changeTheme<T>(Dictionary<string, Dictionary<string, Dictionary<string, T>>> db, string forcedTheme=null)
        {
            string theme;
            if (forcedTheme != null)
                theme = forcedTheme;
            else
                theme = Settings.curTheme;

            bool typeTex = true;
            Dictionary<string, List<FOI>> curFOI = fieldsOfInterestTex;

            if (!db.ContainsKey(theme))
                return;

            if (typeof(T) != typeof(Texture2D)){
                typeTex = false;
                curFOI = fieldsOfInterestColor;
            }

            //Browse all the fields Of interest present for the current theme in order to substitute the fields in the RW classes
            //For each class of interest
            foreach (var (key, value) in curFOI)
            {
                try
                {
                    //If the theme has overloads for the current classname
                    if (db[theme].ContainsKey(key))
                    {
                        //Namespace deduction
                        string ns = "Verse";
                        if(typeTex)
                        {
                            if (fieldsOfInterestTexNS.TryGetValue(key, out var ns1))
                                ns = ns1;
                        }
                        else
                        {
                            if (fieldsOfInterestColorNS.TryGetValue(key, out var ns1))
                                ns = ns1;
                        }


                        //Obtaining the type
                        Type classType = typeof(FloatMenuOption).Assembly.GetType(ns+"." + key);

                        //For each of the texture variables of interest we save the texture reference
                        foreach (var field in value)
                        {
                            try
                            {
                                //If the theme has an overload for the current fieldName
                                if (db[theme][key].ContainsKey(field.field) && db[theme][key][field.field] != null)
                                {
                                    //LogMsg("Overwrite " + ns + "." + fields.Key+"."+field.field);
                                    //Traverse.CreateWithType(ns + "." + fields.Key).Field(field.field).SetValue(db[theme][fields.Key][field.field]);
                                    //classType.GetField(field.field, (BindingFlags)field.bf).SetValue(null, db[theme][fields.Key][field.field]);

                                    //We overwrite the data in memory
                                    if (typeTex)
                                    {
                                        ((Texture2D)classType.GetField(field.field, field.bf).GetValue(null)).LoadImage(((Texture2D)(object)db[theme][key][field.field]).EncodeToPNG());
                                    }
                                    else
                                    {
                                        classType.GetField(field.field, field.bf).SetValue(null, db[theme][key][field.field]);
                                    }
                                }
                                else
                                {
                                    //LogMsg("Not found " + field.field);
                                }
                            }
                            catch (Exception e)
                            {
                                LogMsg("changeTheme ("+typeof(T)+") : Cannot set field " + key + "." + field.field+" : "+e.Message);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Themes.LogError("changeTheme (" + typeof(T) + ") : Cannot get class " + key+" : "+e.Message);
                }
            }
        }

        public static void changeThemeOtherMods(string theme)
        {
            //Applying patches for crappy mods that overrides graphic elements
            const BindingFlags NPS = (BindingFlags.NonPublic | BindingFlags.Static);

            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i != ass.Length; i++)
            {
                try
                {
                    if (ass[i].GetName().Name == "RecipeIcons")
                    {
                        Type classType = ass[i].GetType("RecipeIcons.RecipeTooltip");
                        classType.GetField("ColorBGActive", NPS).SetValue(null, DBColor[theme]["FloatMenuOption"]["ColorBGActive"]);
                        //classType.GetField("ColorBGActiveMouseover", NPS).SetValue(null, DBColor[theme]["FloatMenuOption"]["ColorBGActiveMouseover"]);
                        //classType.GetField("ColorBGDisabled", NPS).SetValue(null, DBColor[theme]["FloatMenuOption"]["ColorBGDisabled"]);
                        classType.GetField("ColorTextActive", NPS).SetValue(null, DBColor[theme]["FloatMenuOption"]["ColorTextActive"]);
                        //classType.GetField("ColorTextDisabled", NPS).SetValue(null, DBColor[theme]["FloatMenuOption"]["ColorTextDisabled"]);
                    }
                }
                catch (Exception e)
                {
                    Themes.LogException("Cannot apply theme on custom assembly : ", e);
                }
            }
        }

        /*
         * Saving Vanilla textures from RimWorld
         */
        public static void saveVanillaRsc<T>(Dictionary<string, Dictionary<string, Dictionary<string, T>>> db)
        {
            const string theme = VanillaThemeID;
            
            db[theme] = new Dictionary<string, Dictionary<string, T>>();
            var curFOI = fieldsOfInterestTex;
            var typeTex = true;

            if (typeof(T) != typeof(Texture2D))
            {
                typeTex = false;
                curFOI = fieldsOfInterestColor;
            }
            //Browse all the fields Of interest present for the current theme in order to substitute the fields in the RW classes
            //For each class of interest
            foreach (var (key, value) in curFOI)
            {
                try
                {
                    db[theme][key] = new Dictionary<string, T>();
                    //Namespace deduction
                    var ns = "Verse";
                    if (typeTex)
                    {
                        if (fieldsOfInterestTexNS.TryGetValue(key, out var ns1))
                            ns = ns1;
                    }
                    else
                    {
                        if (fieldsOfInterestColorNS.TryGetValue(key, out var ns1))
                            ns = ns1;
                    }

                    //Obtaining the type
                    var classType = typeof(FloatMenuOption).Assembly.GetType(ns + "." + key);

                    //For each of the texture variables of interest we save the texture reference
                    foreach (var field in value)
                    {
                        try
                        {
                            //LogMsg("Saving vanilla field " + ns + "." + fields.Key + "." + field.field);
                            //We duplicate the textures and the data in memory
                            if (typeTex)
                            {
                                var savedTex = (Dictionary<string, Texture2D>) (object) db[theme][key];
                                var curTex = (Texture2D) classType.GetField(field.field, field.bf).GetValue(null);
                                savedTex[field.field] = new Texture2D(curTex.width, curTex.height, curTex.format, curTex.mipmapCount, false);
                                Graphics.CopyTexture(curTex, savedTex[field.field]);
                            }
                            else
                            {
                                var savedColor = (Dictionary<string, Color>) (object) db[theme][key];
                                savedColor[field.field] = (Color)classType.GetField(field.field, field.bf).GetValue(null);
                            }
                        }
                        catch
                        {
                            try
                            {
                                var savedTex = (Dictionary<string, Texture2D>) (object) db[theme][key];
                                var curTex = (Texture2D) classType.GetField(field.field, field.bf).GetValue(null);
                                curTex.filterMode = FilterMode.Point;
                                
                                var rt = RenderTexture.GetTemporary(curTex.width, curTex.height);
                                RenderTexture.active = rt;
                                try
                                {
                                    Graphics.Blit(curTex, rt);

                                    var img2 = new Texture2D(curTex.width, curTex.height, TextureFormat.RGBA32, false);
                                    img2.ReadPixels(new Rect(0, 0, curTex.width, curTex.height), 0, 0, false);
                                    img2.Apply(false);

                                    Graphics.CopyTexture(img2, savedTex[field.field]);
                                }
                                finally
                                {
                                    RenderTexture.active = null;
                                    rt.Release();
                                }
                            }
                            catch (Exception e)
                            {
                                Themes.LogException("saveVanillaRsc (" + typeof(T) + ")  : Cannot save field " + key + "." + field.field + " : ", e);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Themes.LogError("saveVanillaRsc (" + typeof(T) + ")  : Cannot get class " + key+" : "+e.Message);
                }
            }
        }


        public static Texture2D getThemeIcon(string dtheme = "")
        {
            if (dtheme.Length == 0)
                // ReSharper disable once Unity.NoNullCoalescing
                return ActiveTheme.DBTexThemeIcon ?? Loader.defaultIconTex;

            if (DBTexThemeIcon.TryGetValue(dtheme, out var result))
                return result;

            return Loader.defaultIconTex;

        }

        public static Texture2D getThemeParticle(string dtheme = "")
        {
            if (dtheme.Length == 0)
                return ActiveTheme.DBTexParticle;
            
            if (DBTexParticle.TryGetValue(dtheme, out var result))
                return result;

            return null;
        }

        /*
         * Obtaining a texture for the specified class and fieldName, if not available then obtaining the Vanilla version
         */
        public static Texture2D getThemeTex(string className,string fieldName,string dtheme = "")
        {
            if (dtheme.Length == 0)
                return ActiveTheme.DBTex != null && ActiveTheme.DBTex.TryGetValue((className, fieldName), out var result)
                    ? result
                    : null;

            if (DBTex.TryGetValue(dtheme, out var p1) && p1.TryGetValue(className, out var p2) && p2.TryGetValue(fieldName, out var p3) && !ReferenceEquals(p3, null))
                return p3;
            
            //If absent, the Vanilla version is returned
            if (DBTex.TryGetValue(VanillaThemeID, out var v1) && v1.TryGetValue(className, out var v2) && v2.TryGetValue(fieldName, out var v3) && !ReferenceEquals(v3, null))
                return v3;

            return null;
        }

        public static string getText(string text, string dtheme = "")
        {
            if (dtheme.Length == 0)
                return ActiveTheme.DBText != null && ActiveTheme.DBText.TryGetValue(text, out var result)
                    ? result
                    : null;

            if (DBText != null && DBText.TryGetValue(dtheme, out var p1) && p1 != null && p1.TryGetValue(text, out var p2))
                return p2;
    
            return null;
        }

        public static int getVal(string text, string dtheme = "")
        {
            if (dtheme.Length == 0)
                return ActiveTheme.DBVal != null && ActiveTheme.DBVal.TryGetValue(text, out var result)
                    ? result
                    : -9999;

            if (DBVal.TryGetValue(dtheme, out var p1) && p1 != null && p1.TryGetValue(text, out var p2))
                return p2;

            return -9999;
        }

        public static Color getColorMisc(string text, string dtheme = "")
        {
            if (dtheme.Length == 0)
                return ActiveTheme.DBColor != null && ActiveTheme.DBColor.TryGetValue((text, MiscKey), out var result)
                    ? result
                    : Color.black;

            if (DBColor.TryGetValue(dtheme, out var p1) && p1 != null && p1.TryGetValue(MiscKey, out var p2) &&
                p2.TryGetValue(text, out var p3)) return p3;

            return Color.black;
        }

        private static GUIStyle cacheGetDBGUIStyleTiny = null;
        private static GUIStyle cacheGetDBGUIStyleSmall = null;
        private static GUIStyle cacheGetDBGUIStyleMedium = null;
        private static string cacheGetDBGUIStyleTinyTheme = "";
        private static string cacheGetDBGUIStyleSmallTheme = "";
        private static string cacheGetDBGUIStyleMediumTheme = "";

        public static GUIStyle getDBGUIStyle(GameFont gf)
        {
            var theme = Settings.curTheme;

            if (forcedFontTheme.Length != 0)
                theme = forcedFontTheme;

            if (Settings.disableCustomFonts)
                return null;

            //Extract from cache
            switch (gf)
            {
                case GameFont.Tiny:
                    if (cacheGetDBGUIStyleTiny != null && ReferenceEquals(cacheGetDBGUIStyleTinyTheme, theme))
                        return cacheGetDBGUIStyleTiny;
                    break;
                case GameFont.Small:
                    if (cacheGetDBGUIStyleSmall != null && ReferenceEquals(cacheGetDBGUIStyleSmallTheme, theme))
                        return cacheGetDBGUIStyleSmall;
                    break;
                case GameFont.Medium:
                    if (cacheGetDBGUIStyleMedium != null && ReferenceEquals(cacheGetDBGUIStyleMediumTheme, theme))
                        return cacheGetDBGUIStyleMedium;
                    break;
            }

            if (!DBGUIStyle.TryGetValue(theme, out var p1) || p1 == null)
                return null;

            //Put in cache
            switch (gf)
            {
                case GameFont.Tiny:
                    cacheGetDBGUIStyleTinyTheme = theme;
                    cacheGetDBGUIStyleTiny = p1[gf];
                    break;
                case GameFont.Small:
                    cacheGetDBGUIStyleSmallTheme = theme;
                    cacheGetDBGUIStyleSmall = p1[gf];
                    break;
                case GameFont.Medium:
                    cacheGetDBGUIStyleMediumTheme = theme;
                    cacheGetDBGUIStyleMedium = p1[gf];
                    break;
            }

            return p1[gf];
        }

        public static GUIStyle getDBGUIStyleTextField(GameFont gf)
        {
            if (Settings.disableCustomFonts)
                return null;

            if (forcedFontTheme.Length == 0)
                return ActiveTheme.DBGUIStyleTextField?[gf];

            if (!DBGUIStyleTextField.TryGetValue(forcedFontTheme, out var result) || result == null)
                return null;
            return result[gf];
        }

        public static GUIStyle getDBGUIStyleTextArea(GameFont gf)
        {
            if (Settings.disableCustomFonts)
                return null;
            
            if (forcedFontTheme.Length == 0)
                return ActiveTheme.DBGUIStyleTextArea?[gf];

            if (!DBGUIStyleTextArea.TryGetValue(forcedFontTheme, out var result) || result == null)
                return null;
            return result[gf];
        }

        public static GUIStyle getDBGUIStyleTextAreaReadOnly(GameFont gf)
        {
            if (Settings.disableCustomFonts)
                return null;
            
            if (forcedFontTheme.Length == 0)
                return ActiveTheme.DBGUIStyleTextAreaReadOnly?[gf];

            if (!DBGUIStyleTextAreaReadOnly.TryGetValue(forcedFontTheme, out var result) || result == null)
                return null;
            return result[gf];
        }

        public static float getDBGUIStyleLineHeight(GameFont gf)
        {
            if (Settings.disableCustomFonts)
                return 0;
            
            if (forcedFontTheme.Length == 0)
                return ActiveTheme.DBGUIStyleLineHeight?[gf] ?? 0;

            if (!DBGUIStyleLineHeight.TryGetValue(forcedFontTheme, out var result) || result == null)
                return 0;
            return result[gf];
        }

        public static float getDBGUIStyleSpaceBetweenLine(GameFont gf)
        {
            if (Settings.disableCustomFonts)
                return 0;
            
            if (forcedFontTheme.Length == 0)
                return ActiveTheme.DBGUIStyleSpaceBetweenLine?[gf] ?? 0;

            if (!DBGUIStyleSpaceBetweenLine.TryGetValue(forcedFontTheme, out var result) || result == null)
                return 0;
            return result[gf];
        }


        /*
         * Method allowing to obtain the current theme loader and to load it if necessary
         */
        public static Texture2D[] getThemeLoader()
        {
            string[] parts = Settings.curTheme.Split('§');

            if (!Themes.DBVal.ContainsKey(Themes.VanillaThemeID))
            {
                Themes.DBVal[Themes.VanillaThemeID] = new Dictionary<string, int>();
                Themes.DBVal[Themes.VanillaThemeID]["loaderfps"] = 20;
            }

            //If NO LOADER desired we return a NULL texture
            if (DBNoLoader.ContainsKey(Settings.curTheme) && DBNoLoader[Settings.curTheme])
                return null;
            //If NO LOADER because not present
            if (DBLoaderNotFound.ContainsKey(Settings.curTheme) && DBLoaderNotFound[Settings.curTheme])
                return getDefaultLoader();

            if (parts[0] == "-1")
            {
                //Loader not loaded we try to load it
                if (!DBLoader.ContainsKey(Settings.curTheme))
                {
                    //If NOLOADER detected then we mark the theme as wishing to deactivate the loader
                    if (!DBNoLoader.ContainsKey(Settings.curTheme))
                    {
                        string pathNL = Utils.currentMod.RootDir + Path.DirectorySeparatorChar + "Themes"
                            + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + "NOLOADER";
                        if (File.Exists(pathNL))
                        {
                            DBNoLoader[Settings.curTheme] = true;
                            return null;
                        }
                        else
                            DBNoLoader[Settings.curTheme] = false;
                    }

                    //Loading meta.xml if applicable (dependency with loaderFPS)
                    string pathMeta = Utils.currentMod.RootDir + Path.DirectorySeparatorChar + "Themes"
                        + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "meta.xml";

                    if (File.Exists(pathMeta))
                    {
                        try
                        {
                            Themes.processMetaXML(Settings.curTheme, pathMeta);
                        }
                        catch (Exception e)
                        {
                            Themes.LogException("getThemeLoader processing meta.xml : ", e);
                        }
                    }

                    string path = Utils.currentMod.RootDir + Path.DirectorySeparatorChar + "Themes" 
                        + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar +"Loader.png";

                    if (!File.Exists(path))
                    {
                        DBLoaderNotFound[Settings.curTheme] = true;
                        return getDefaultLoader();
                    }
                    else
                        DBLoaderNotFound[Settings.curTheme] = false;

                    //Custom loader loading
                    if (loadCustomThemeLoader(path))
                    {
                        //Loading ok we return the custom loader
                        return DBLoader[Settings.curTheme];
                    }

                    //error we return the loader by default
                    return getDefaultLoader();

                }

                //No loader, we grab the default one
                if (DBLoader[Settings.curTheme] == null)
                    return getDefaultLoader();

                return DBLoader[Settings.curTheme];
            }

            if (parts[0] == "-2")
            {
                //Loader not loaded we try to load it
                if (!DBLoader.ContainsKey(Settings.curTheme))
                {
                    string basePath = Utils.RWBaseFolderPath;

                    //If NOLOADER detected then we mark the theme as wishing to deactivate the loader
                    if (!DBNoLoader.ContainsKey(Settings.curTheme))
                    {
                        string pathNL = basePath + Path.DirectorySeparatorChar + "RimThemes"
                                        + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + "NOLOADER";
                        if (File.Exists(pathNL))
                        {
                            DBNoLoader[Settings.curTheme] = true;
                            return null;
                        }
                        else
                            DBNoLoader[Settings.curTheme] = false;
                    }

                    //Loading meta.xml if applicable(dependency with loaderFPS)
                    string pathMeta = basePath + Path.DirectorySeparatorChar + "RimThemes"
                                      + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "meta.xml";

                    if (File.Exists(pathMeta))
                    {
                        try
                        {
                            Themes.processMetaXML(Settings.curTheme, pathMeta);
                        }
                        catch (Exception e)
                        {
                            Themes.LogException("getThemeLoader processing meta.xml : ", e);
                        }
                    }

                    string path = basePath + Path.DirectorySeparatorChar + "RimThemes"
                                  + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + "Loader.png";
                    if (!File.Exists(path))
                    {
                        DBLoaderNotFound[Settings.curTheme] = true;
                        return getDefaultLoader();
                    }
                    else
                        DBLoaderNotFound[Settings.curTheme] = false;

                    //Custom loader loading
                    if (loadCustomThemeLoader(path))
                    {
                        //Chargement ok on retourne le custom loader
                        return DBLoader[Settings.curTheme];
                    }
                    else
                    {
                        //error we return the loader by default
                        return getDefaultLoader();
                    }

                }
                else
                {
                    //No loader, we grab the default one
                    if (DBLoader[Settings.curTheme] == null)
                        return getDefaultLoader();
                    else
                        return DBLoader[Settings.curTheme];
                }
            }

            //Theme located in an external theme
            //If already loaded we return it
            if (DBLoader.ContainsKey(Settings.curTheme))
            {
                //No loader, we grab the default one
                if (DBLoader[Settings.curTheme] == null)
                    return getDefaultLoader();
                else
                    return DBLoader[Settings.curTheme];
            }
            else
            {
                //External theme not loaded we try to load it
                List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
                for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
                {
                    ModContentPack cmod = runningModsListForReading[i];
                    if (cmod.PackageId == parts[0])
                    {
                        //If NOLOADER detected then we mark the theme as wishing to deactivate the loader
                        if (!DBNoLoader.ContainsKey(Settings.curTheme))
                        {
                            string loaderPathNL = cmod.RootDir + Path.DirectorySeparatorChar + "RimThemes" + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + "NOLOADER";
                            if (File.Exists(loaderPathNL))
                            {
                                DBNoLoader[Settings.curTheme] = true;
                                return null;
                            }
                            else
                            {
                                DBNoLoader[Settings.curTheme] = false;
                            }
                        }

                        //Loading meta.xml if applicable (dependency with loaderFPS)
                        string pathMeta = cmod.RootDir + Path.DirectorySeparatorChar + "RimThemes"
                                          + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "meta.xml";

                        if (File.Exists(pathMeta))
                        {
                            try
                            {
                                Themes.processMetaXML(Settings.curTheme, pathMeta);
                            }
                            catch (Exception e)
                            {
                                Themes.LogException("getThemeLoader processing meta.xml : ", e);
                            }
                        }

                        //Check if existence Loader.png for the mod
                        string loaderPath = cmod.RootDir + Path.DirectorySeparatorChar + "RimThemes" + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + "Loader.png";
                        if (File.Exists(loaderPath))
                        {
                            if (loadCustomThemeLoader(loaderPath))
                            {
                                DBLoaderNotFound[Settings.curTheme] = false;
                                return DBLoader[Settings.curTheme];
                            }
                            else
                            {
                                DBLoaderNotFound[Settings.curTheme] = true;
                                return getDefaultLoader();
                            }
                        }
                    }
                }

                //No loader found for the current theme, we return the default one
                return getDefaultLoader();
            }
        }

        /*
         * Method allowing to load a theme loader in APNG format
         */
        private static bool loadCustomThemeLoader(string customLoaderPath)
        {
            bool ret = true;
            try
            {
                APNG png = new APNG(File.ReadAllBytes(customLoaderPath));
                DBLoader[Settings.curTheme] = new Texture2D[png.Frames.Length];
                LibAPNG.Frame[] frames = png.Frames;

                for (var i = 0; i != frames.Length; i++)
                {
                    var curTex = new Texture2D(196, 196, TextureFormat.ARGB32, false);
                    curTex.LoadImage(frames[i].GetStream().ToArray());
                    DBLoader[Settings.curTheme][i] = curTex;
                }
            }
            catch (Exception e)
            {
                ret = false;
                //We add at least a null texture to use it as a sign that the texture has already been tried to be loaded
                if (DBLoader[Settings.curTheme] == null)
                    DBLoader[Settings.curTheme] = new Texture2D[1];

                for (var i = 0; i != DBLoader[Settings.curTheme].Length; i++)
                {
                    DBLoader[Settings.curTheme][i] = null;
                }
                Themes.LogError("Cannot load custom loader for current theme : "+e.Message);
            }

            return ret;
        }

        //Return loader by default, if not loaded it is loaded on the way
        private static Texture2D[] getDefaultLoader()
        {
            if (Loader.tex[0] == null)
                Loader.initTextures();

            return Loader.tex;
        }


        /*
         * Obtaining the applicable menu alignment according to the parameters
         */
        public static string getMenuAlignment()
        {
            string ret;
            //Application determined by the current theme
            if (Settings.menuAlignment == 3) {
                ret = Themes.getText("menualignment");
            }
            else
            {
                if (Settings.menuAlignment == 0)
                    ret = "left";
                else if (Settings.menuAlignment == 1)
                    ret= "middle";
                else
                    ret = "right";
            }

            return ret;
        }

        /*
         * Obtaining the dialog stacking applicable according to the theme (by default yes)
         */
        public static bool dialogStacking()
        {
            bool ret;
            //Application determined by the current theme
            if (Settings.dialogStacking == 1)
            {
                string iret = Themes.getText("dialogstacking");
                if (iret == null)
                    ret = true;
                else if (iret == "true")
                    ret = true;
                else
                    ret = false;
            }
            else
            {
                if (Settings.dialogStacking == 2)
                    ret = true;
                else
                    ret = false;
            }

            return ret;
        }

        /*
        * Obtaining the window shadow mode applicable according to the theme (by default yes)
        */
        public static bool windowsShadow()
        {
            bool ret;
            //Application determined by the current theme
            if (Settings.windowsShadowMode == 3)
            {
                string iret = Themes.getText("windowsshadow");
                if (iret == null)
                    ret = true;
                else if (iret == "true")
                    ret = true;
                else
                    ret = false;
            }
            else
            {
                if (Settings.windowsShadowMode == 2)
                    ret = false;
                else
                    ret = true;
            }

            return ret;
        }

        /*
        * Obtaining the applicable extension icons display mode according to the theme (by default yes)
        */
        public static bool expansionsIcons()
        {
            bool ret;
            //Application determined by the current theme
            if (Settings.expansionsIconsMode == 3)
            {
                string iret = Themes.getText("expansionsicons");
                if (iret == null)
                    ret = true;
                else if (iret == "true")
                    ret = true;
                else
                    ret = false;
            }
            else
            {
                if (Settings.expansionsIconsMode == 2)
                    ret = false;
                else
                    ret = true;
            }

            return ret;
        }


        /*
         * Obtaining the window opening animation to perform
         */
        public static WindowAnim getWindowAnimation()
        {
            string theme = Settings.curTheme;
            WindowAnim settings = (WindowAnim) Settings.windowAnimation;

            //Application determined by the current theme
            if (settings == WindowAnim.Theme)
            {
                //anim defined in the theme
                if (ActiveTheme.DBWindowAnim != null)
                {
                    return ActiveTheme.DBWindowAnim.Value;
                }
                else
                {
                    return WindowAnim.None;
                }
            }
            else
                return (WindowAnim) Settings.windowAnimation;
        }


        /*
         * Obtaining BG buttons
         */
        public static bool getButtonNoBG()
        {
            return Settings.disableButtonBG || Themes.getText("buttonnobg") == "true" || Utils.tempDisableButtonsBackground;
        }

        /*
         * Log Message by adding the RimTheme prefix
         */
        public static void LogMsg(string msg,bool forceShow=false)
        {
            if(Settings.verboseMode || forceShow)
                Log.Message("[RimThemes] " + msg);
        }

        /*
         * Log Error message adding the RimTheme prefix
         */
        public static void LogError(string msg)
        {
            Log.Message("[RimThemesError] " + msg);
        }

        public static void LogException(string msg, Exception exception)
        {
            Log.Message("[RimThemesError] " + msg + "\n" + exception);
        }


        /*
         * Particle drawing on button texture
         */
        public static void drawParticle(Rect rect)
        {
            Texture2D tex = getThemeParticle();

            if (Settings.disableParticle || tex == null || rect.width < 80f)
                return;

            var partMode = getText("particlemode");
            bool mono;

            if (partMode == "mono")
                mono = true;
            else
                mono = false;

            GUI.BeginGroup(rect);
            Rect drawRect = new Rect(9f, (rect.height / 2) - 8f, 16, 16);
            Widgets.DrawTextureFitted(drawRect, tex, 1f);

            //If dual mode drawing of the right part
            if (!mono)
            {
                drawRect = new Rect( rect.width - 25f, (rect.height / 2) - 8f, 16, 16);
                Widgets.DrawTextureFitted(drawRect, tex, 1f);
            }
            GUI.EndGroup();
        }


        public static string[] getClassNameAndFileName(string rscName)
        {
            string[] ret=null;
            int lastIndex = rscName.LastIndexOf('.');
            if (lastIndex + 1 < rscName.Length)
            {
                ret = new string[2];
                ret[0] = rscName.Substring(0, lastIndex);
                ret[1] = rscName.Substring(lastIndex + 1);
            }
            return ret;
        }


        /*
         * Routine used to define a new random background
         */
        public static void setNewRandomBg(bool force = false)
        {
            //If user wishes to keep his current wallpaper, we force a thing to do if the call is not forced (in the case of a theme no longer available)
            if (!force && Settings.keepCurrentBg)
                return;

            string ret = null;
            if (Themes.DBAvailableThemes.Count <= 0)
            {
                Settings.curRandomBg = VanillaThemeID;
                return;
            }

            do
            {
                ret = Themes.DBAvailableThemes.RandomElement();
            }
            while (Themes.DBAvailableThemes.Count >= 2 && ret == Settings.curRandomBg);

            //Recording of the randomly selected theme
            Settings.curRandomBg = ret;
        }


        /*
         * Obtaining the video / image wallpaper of the applicable theme according to the parameters
         */
        public static string getCustomBackgroundApplyableTheme()
        {
            //If random change of backgrouund disabled OR if it is enabled AND the textures have still not been loaded we define the current theme as the normal theme 
            if (Settings.disableRandomBg || !LoaderGM.themeTexAlreadyLoaded || LoaderGM.curStep <= LoaderSteps.FinishUp)
            {
                return Settings.curTheme;
            }
            else
            {
                //Check if the theme is still valid, if not we throw it 
                if (!Themes.DBAvailableThemes.Contains(Settings.curRandomBg))
                {
                    setNewRandomBg(true);
                }
                return Settings.curRandomBg;
            }
        }


        public static void stopCurrentAnimatedBackground()
        {
            //Potential shutdown animated backgrounds
            if (Utils.CurrentMainAnimatedBg != null)
            {
                Utils.CurrentMainAnimatedBg.Stop();
                //Utils.CurrentMainAnimatedBg = null;
                Utils.CurrentMainAnimatedBgPlaying = false;
                Utils.CurrentMainAnimatedBgSourceSet = false;
            }
        }

        public static ActiveTheme ActiveTheme = new();

        //Storage of fix tables in case of field / class name change to avoid breaking themes
        public static readonly Dictionary<string, string> DBFix = new();

        public static readonly List<string> DBAvailableThemes = new();
        //Storage of effects data
        public static readonly Dictionary<string, float> DBEffect = new();

        public static readonly List<Dictionary<string,string>> fontsToLoad = new();

        public static readonly Dictionary<string, Dictionary<GameFont, GUIStyle>> DBGUIStyle = new();
        public static readonly Dictionary<string, Dictionary<GameFont, GUIStyle>> DBGUIStyleTextField = new();
        public static readonly Dictionary<string, Dictionary<GameFont, GUIStyle>> DBGUIStyleTextArea = new();
        public static readonly Dictionary<string, Dictionary<GameFont, GUIStyle>> DBGUIStyleTextAreaReadOnly = new();

        public static readonly Dictionary<string, Dictionary<GameFont, float>> DBGUIStyleLineHeight = new();
        public static readonly Dictionary<string, Dictionary<GameFont, float>> DBGUIStyleSpaceBetweenLine = new();

        //Byte [] dictionaries before conversion to textures
        public static readonly Dictionary<string, byte[]> RDBTexThemeIcon = new();
        public static readonly Dictionary<string, byte[]> RDBTexParticle = new();
        public static readonly Dictionary<string, byte[]> RDBTexTapestry = new();
        public static readonly Dictionary<string, byte[][]> RDBLoader = new();
        public static readonly Dictionary<string, byte[]> RDBBGLoader = new();
        public static readonly Dictionary<string, byte[]> RDBTexLoaderBar = new();
        public static readonly Dictionary<string, byte[]> RDBTexLoaderText = new();
        public static readonly Dictionary<string, Dictionary<string, Dictionary<string, byte[]>>> RDBTex = new();

        //Texture dictionaries
        public static readonly Dictionary<string, Texture2D> DBTexThemeIcon = new();
        public static readonly Dictionary<string, Texture2D> DBTexParticle = new();
        public static readonly Dictionary<string, Texture2D> DBTexTapestry = new();
        public static readonly Dictionary<string, Texture2D[]> DBLoader = new();
        public static readonly Dictionary<string, bool> DBNoLoader = new();
        public static readonly Dictionary<string, bool> DBLoaderNotFound = new();
        public static readonly Dictionary<string, Texture2D> DBBGLoader = new();
        public static readonly Dictionary<string, Texture2D> DBTexLoaderBar = new();
        public static readonly Dictionary<string, Texture2D> DBTexLoaderText = new();
        public static readonly Dictionary<string, Dictionary<string, Dictionary<string, Texture2D>>> DBTex = new();

        //Animated background
        public static readonly Dictionary<string, string> DBAnimatedBackground = new();

        //Text color by theme
        public static readonly Dictionary<string, Color> DBTextColorWhite = new();
        public static readonly Dictionary<string, Color> DBTextColorYellow = new();
        public static readonly Dictionary<string, Color> DBTextColorGreen = new();
        public static readonly Dictionary<string, Color> DBTextColorRed = new();
        public static readonly Dictionary<string, Color> DBTextColorCyan = new();
        public static readonly Dictionary<string, Color> DBTextColorBlue = new();
        public static readonly Dictionary<string, Color> DBTextColorGray = new();
        public static readonly Dictionary<string, Color> DBTextColorMagenta = new();


        //Texture color by theme
        public static readonly Dictionary<string, Color> DBTexColorWhite = new();
        public static readonly Dictionary<string, Color> DBTexColorYellow = new();
        public static readonly Dictionary<string, Color> DBTexColorGreen = new();
        public static readonly Dictionary<string, Color> DBTexColorRed = new();
        public static readonly Dictionary<string, Color> DBTexColorCyan = new();
        public static readonly Dictionary<string, Color> DBTexColorBlue = new();
        public static readonly Dictionary<string, Color> DBTexColorGray = new();
        public static readonly Dictionary<string, Color> DBTexColorMagenta = new();

        //Neutral color of factions
        public static readonly Dictionary<string, Color> DBTextColorFactionsNeutral = new();

        public static readonly Dictionary<string, WindowAnim> DBWindowAnim = new();
        public static readonly Dictionary<string, Dictionary<string, Dictionary<string,Color>>> DBColor = new();
        public static readonly Dictionary<string, Dictionary<string, AudioGrain_ClipTheme>> DBSound = new();
        public static readonly Dictionary<string, Dictionary<string, AudioClip>> DBSong = new();
        public static readonly Dictionary<string, Dictionary<string, string>> DBText = new();
        public static readonly Dictionary<string, Dictionary<string, int>> DBVal = new();
        public static readonly Dictionary<string, Dictionary<string, string>> DBModInfo = new();
        public static readonly Dictionary<string, Dictionary<Color, Color>> DBDynColor = new();

        //List of music files to load (EntrySong)
        public static readonly Dictionary<string, Dictionary<string, string>> DBSongsToLoad = new();
        //List of font packs to load
        public static readonly List<string> DBfontsBundleToLoad = new();

        //Storage of Namespaces, if any, different from Verse for className (Default "Verse")
        public static readonly Dictionary<string,string> fieldsOfInterestTexNS = new();
        public static readonly Dictionary<string,string> fieldsOfInterestColorNS = new();
        //Storage of className + fields of interest for mod themes
        public static readonly Dictionary<string, List<FOI>> fieldsOfInterestTex = new();
        public static readonly Dictionary<string, List<FOI>> fieldsOfInterestColor = new();


        public static Dictionary<string, Font> DBTinyFontByTheme = new();
        public static Dictionary<string, int> DBTinyFontSizeByTheme = new();

        public const string VanillaThemeID = "-1§Vanilla";
        public const string MiscKey = "_Misc_";
        public static Dictionary<string, List<ResolvedGrain>> DBVanillaResolvedGrains = new();
        public static List<AssetBundle> fontsPackage = new();
        public static string forcedFontTheme = "";
        public static Texture2D TexLoaderBar;
        public static Texture2D TexLoaderText;

        //Define if everything is ok for the themes
        public static bool initialized = false;
        public static bool cursorFirstSet = false;
        public static bool vanillaGrainsSaved = false;
        public static bool vanillaSongGrainsSaved = false;
        public static bool vanillaThemeSaved = false;
        public static bool currentThemeExist = true;
    }
}
