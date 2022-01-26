using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.Video;

namespace aRandomKiwi.RimThemes
{
    [StaticConstructorOnStartup]
    static class Utils
    {
        public static bool tempDisableDynColor= false;
        public static bool tempDisableNoTransparentText = false;
        public static bool tempDisableButtonsBackground = false;
        public static bool textFontSetterLock = false;
        public static int squeezedDrawOptionListingIndex = 0;
        public static float squeezedDrawOptionListingIndexReturnVal = 0;

        public static VideoPlayer CurrentMainAnimatedBg = null;
        public static bool CurrentMainAnimatedBgPlaying = false;
        public static bool CurrentMainAnimatedBgSourceSet = false;

        public static string sponsor = "M";

        //static public bool firstOpenedConsole = false;
        //static public int vipWindowID = -1;
        static public List<WDESC> lastShowedWin = new();
        static public bool needRefresh = false;
        static public ModContentPack currentMod;
        static public Mod currentModInst;
        static public Settings modSettings;
        public const string releaseInfo = "RimThemes NX";

        public const string releaseDesc
            = "Changes :\n"
            + "-Added the name of the current main donor in the themes selection menu\n"
            + "-Improved all default themes\n"
            + "-Added new setting allowing to adjust all windows opacity level\n"
            + "-Added new setting allowing to hide the RimThemes logo in the main menu\n"
            + "-Added new settings allowing to hide the main menu expansions icons, info corner and more\n"
            + "-Added new setting allowing to hide windows shadows\n"
            + "-Added new default theme 'Rim-Life 2' and 'Mechanoid cluster'\n"
            + "-Fixed the overlapping issue with the expansions icons buttons (in the bottom left)\n"
            + "-Fixed confirm button texture issue (vanilla texture applied instead of the current theme)\n"
            + "-Few others minors improvements\n\n"
            + "For themes makers :\n"
            + "-Fixed tapestry border color tag bug (color was never applied in themes)\n"
            + "-Added support for custom APNG loader FPS with the new tag 'loaderFPS'\n"
            + "-Few others new tags (download the Theme example package for more details)\n\n"
            + "/!\\ Notice : Support for 1.0 is dropped, only RimThemes 2020R1 is compatible with Rimworld 1.0.\n"
            ;

        private static Traverse cachedLabelWidthCache = null;
        static private bool initCachedLabelWidthCache = false;

        public static void resetCachedLabelWidthCache()
        {
            if (!initCachedLabelWidthCache)
            {
                initCachedLabelWidthCache = true;
                cachedLabelWidthCache = Traverse.CreateWithType("Verse.GenUI").Field("labelWidthCache");
            }

            //Reset label width caches
            Dictionary<string, float> labelWidthCache = (Dictionary<string, float>)cachedLabelWidthCache.GetValue();
            labelWidthCache.Clear();
            //GenUI.labelWidthCache.Clear();
        }

        public static void startLoadingTheme()
        {
            LoaderGM.curStep = LoaderSteps.loadingTheme;
            //Loading all dependances from the themes THEN generating theme textures inside LoaderGM
            Themes.startInit();

            //We notify the loader to load the preloaded textures
            LoaderGM.themeLoadMode = 1;
            Thread.Sleep(250);
            LoaderGM.texThemesToLoad = true;

            //We notify the loader to load the preloaded songs
            LoaderGM.themeLoadMode = 2;
            Thread.Sleep(250);
            LoaderGM.songsToLoad = true;

            //Loading the font asset bundle
            try
            {
                LoaderGM.themeLoadMode = 3;
                Thread.Sleep(250);
                //Loading the encoded font bundle into memory
                Themes.fontsPackage.Add(AssetBundle.LoadFromMemory(FontsPackage.fonts)); //LoadFromFile(Utils.currentMod.RootDir + Path.DirectorySeparatorChar + "fontspackage");
                Themes.LogMsg("Load main fonts package OK");
            }
            catch (Exception e)
            {
                Themes.fontsPackage = null;
                Themes.LogException("Loading fonts package : ", e);
            }
            Array.Resize(ref FontsPackage.fonts, 0);
            FontsPackage.fonts = null;

            //Loading of potential font assetsbundle provided by mods
            foreach (string fbPath in Themes.DBfontsBundleToLoad)
            {
                try
                {
                    var cab = AssetBundle.LoadFromFile(fbPath);
                    if (cab == null)
                        throw new Exception("Invalid font package "+fbPath);

                    Themes.fontsPackage.Add(cab);
                    Themes.LogMsg("Load external fonts package "+fbPath+" OK");
                }
                catch (Exception e)
                {
                    Themes.LogException("Loading external fonts package : ", e);
                }
            }


            //Loading Fonts
            LoaderGM.themeLoadMode = 4;
            Thread.Sleep(250);
            LoaderGM.fontsToLoad = true;

            //If enabled we try to change the background of the main menu
            if (!Settings.disableRandomBg)
                Themes.setNewRandomBg();
        }


        /*
         * Check if an image whose path is passed as a parameter exists (without the extension, the test function exists for this image in .png and .jpg format)
         */
        static public bool texFileExist(string path)
        {
            if (File.Exists(path + ".dds"))
                return true;
            if (File.Exists(path + ".png"))
                return true;
            if (File.Exists(path + ".jpg"))
                return true;
            return false;
        }

        static public byte[] readAllBytesTexFile(string path)
        {
            if (File.Exists(path + ".dds"))
                return Encoding.UTF8.GetBytes("_DDS_" + path + ".dds");
            if (File.Exists(path + ".png"))
                return File.ReadAllBytes(path + ".png");
            if (File.Exists(path + ".jpg"))
                return File.ReadAllBytes(path + ".jpg");
            return null;
        }


        private static readonly byte[] DDSBytes = Encoding.ASCII.GetBytes("_DDS_");
        public static Texture2D LoadTexture(byte[] bytes)
        {
            if (bytes.Length >= DDSBytes.Length && bytes[0] == DDSBytes[0] && bytes[1] == DDSBytes[1] && bytes[2] == DDSBytes[2] && bytes[3] == DDSBytes[3] && bytes[4] == DDSBytes[4])
            {
                var filePath = Encoding.UTF8.GetString(bytes, 5, bytes.Length - 5);
                var tex1 = DDSLoader.LoadDDS(filePath);
                if (ReferenceEquals(tex1, null))
                {
                    Themes.LogError($"When loading DDS file '{filePath}': {DDSLoader.error}");
                    return null;
                }

                tex1.Apply();
                
                // Some textures need to be in RGBA32 with no mip chain. (maybe only the cursor?)
                // to guarantee this, we can convert the texture
                // on the GPU. technically this doesn't need to be done for every texture, but i don't want to find each
                // individual one that doesn't support whatever DDS format is used
                var tex2 = new Texture2D(tex1.width, tex1.height, TextureFormat.RGBA32, false);
                if (!Graphics.ConvertTexture(tex1, tex2))
                {
                    Themes.LogError($"Failed to convert texture '{filePath}' to RGBA32. Texture format: {tex1.format}, graphics format: {tex1.graphicsFormat}");
                }
                
                UnityEngine.Object.DestroyImmediate(tex1);
                
                return tex2;
            }
            // JPG files are loaded into RGB24 format, PNG files are loaded into ARGB32 format
            var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(bytes);
            return tex;
        }


        static public bool isNSBlacklistedWindowsType(Window win)
        {
            string type = win.GetType().FullName;
            if (type is "DubsMintMinimap.MainTabWindow_MiniMap" or "DubsMintMinimap.MainTabWindow_MiniMapSetting")
                return true;
            else
                return false;
        }

        static public string ReplaceLastOccurrence(string str, string toReplace, string replacement)
        {
            return Regex.Replace(str, $@"^(.*){toReplace}(.*?)$", $"$1{replacement}$2");
        }

        public static string RWBaseFolderPath => new DirectoryInfo(UnityData.dataPath).Parent.FullName;

        public static void applyWindowFillColorOpacityOverride(string newTheme)
        {
            if (Settings.disabledOverrideThemeWindowFillColorAlpha)
                return;

            Type classType = typeof(FloatMenuOption).Assembly.GetType("Verse.Widgets");
            if (classType == null)
                return;

            Color cColor=Color.black;

            //Change of color component filling of the current theme
            if (Themes.DBColor.ContainsKey(newTheme) && Themes.DBColor[newTheme].ContainsKey("Widgets") && Themes.DBColor[newTheme]["Widgets"].ContainsKey("WindowBGFillColor"))
            {
                cColor = Themes.DBColor[newTheme]["Widgets"]["WindowBGFillColor"];
                cColor.a = Settings.overrideThemeWindowFillColorAlphaLevel;
                Themes.DBColor[newTheme]["Widgets"]["WindowBGFillColor"] = cColor;


            }
            else
            {
                if (Themes.DBColor.ContainsKey(Themes.VanillaThemeID) && Themes.DBColor[Themes.VanillaThemeID].ContainsKey("Widgets") && Themes.DBColor[Themes.VanillaThemeID]["Widgets"].ContainsKey("WindowBGFillColor"))
                {
                    //Change alpha component of the vanilla theme
                    cColor = Themes.DBColor[Themes.VanillaThemeID]["Widgets"]["WindowBGFillColor"];
                    cColor.a = Settings.overrideThemeWindowFillColorAlphaLevel;
                    Themes.DBColor[Themes.VanillaThemeID]["Widgets"]["WindowBGFillColor"] = cColor;
                }
                else
                    return;
            }

            classType.GetField("WindowBGFillColor", (BindingFlags)(BindingFlags.Public | BindingFlags.Static)).SetValue(null, cColor);
        }
    }
}
