﻿using RimWorld;
using RimWorld.Planet;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse.AI;
using Verse.Profile;
using Verse.Sound;
using Verse.Steam;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Text;

namespace aRandomKiwi.RimThemes
{
    public class LoaderGM : MonoBehaviour
    {
        public virtual void Start()
        {
           
        }

        public void OnGUI()
        {
            try
            {
                //If applicable, loading the loader by default
                if (Loader.tex[0] == null)
                    Loader.initTextures();

                if (LongEventHandler.AnyEventNowOrWaiting && !Settings.disableCustomLoader && !autosave)
                {
                    GUI.depth = 0;
                    UI.ApplyUIScale();

                    //Background
                    bool flag = !(UI.screenWidth > UI.screenHeight * (BGPlanetSize.x / BGPlanetSize.y));
                    Rect position;
                    if (flag)
                    {
                        float height = UI.screenHeight;
                        float num = UI.screenHeight * (BGPlanetSize.x / BGPlanetSize.y);
                        position = new Rect(UI.screenWidth / 2 - num / 2f, 0f, num, height);
                    }
                    else
                    {
                        float width = UI.screenWidth;
                        float num2 = UI.screenWidth * (BGPlanetSize.y / BGPlanetSize.x);
                        position = new Rect(0f, UI.screenHeight / 2 - num2 / 2f, width, num2);
                    }
                    GUI.DrawTexture(position, getThemeRscLoader(LoaderRSC.BGLoader), ScaleMode.ScaleToFit);


                    //Widgets.DrawRectFast(new Rect(0, 0, UI.screenWidth, UI.screenHeight), Color.black);
                    //Rect rect1 = new Rect(100f, 100f, UI.screenWidth, UI.screenHeight);

                    int fps = Themes.getVal("loaderfps");

                    if(fps <= 0)
                    {
                        fps = 20;
                    }

                    int index = (int)(Time.time * fps);
                    Texture2D[] frames = Themes.getThemeLoader();

                    //Check if desired loader
                    if (frames != null)
                    {
                        index %= frames.Length;
                        Texture2D curTex = frames[index];

                        GUI.color = Color.white;
                        GUI.DrawTexture(new Rect((UI.screenWidth / 2) - 98, (UI.screenHeight / 2) - 98, 196, 196), curTex);
                    }
                    try
                    {
                        if (!Settings.hideLoadingText)
                            genStatusText();
                    }
                    catch (Exception e)
                    {
                        Themes.LogMsg("LoaderGM genStatusText : " + e.Message);
                    }
                }
                else
                {
                    curStep = LoaderSteps.Idle;
                    loaderLvl = 0;
                }

                //IF textures to load
                if (texThemesToLoad)
                {
                    themeTexAlreadyLoaded = true;
                    loadThemesTextures();
                    texThemesToLoad = false;
                }

                //If songs to load
                if (songsToLoad)
                {
                    loadThemesSongs();
                    songsToLoad = false;
                }

                //IF fonts to load
                if (fontsToLoad)
                {
                    Fonts.loadFonts();
                }
            }
            catch(Exception e)
            {
                Themes.LogMsg("LoaderGM OnGUI : "+e.Message);
            }
        }

        /*
         * Generation of the text to be displayed on the screen according to the current loading step
         */
        public void genStatusText()
        {
            string text = "";
            string name = "";


            switch (curStep)
            {
                case LoaderSteps.loadingTheme:
                    switch (themeLoadMode) {
                        case 0:
                            if (curTheme == "")
                            {
                                text = "RimThemes_LoaderThemesPreloading1".Translate();
                            }
                            else
                            {
                                text = "RimThemes_LoaderThemesPreloading2".Translate(curTheme);
                            }
                            break;
                        case 1:
                            text = "RimThemes_LoaderThemesLoadingTex".Translate();
                        break;
                        case 2:
                            text = "RimThemes_LoaderThemesLoadingSongs".Translate();
                        break;
                        case 3:
                            text = "RimThemes_LoaderThemesLoadingFontsPackage".Translate();
                        break;
                        case 4:
                            text = "RimThemes_LoaderThemesLoadingFonts".Translate();
                        break;
                    }
                break;
                case LoaderSteps.loadingXML:
                    if (curLoadedMod != null && curLoadedMod.Name != null && curLoadedMod.Name.Length != 0)
                        name = " - "+curLoadedMod.Name;
                    else
                        name = "";

                    text = "Loading Defs : "+nbCurModsLoaded+" / "+nbModsToLoad+name;
                    loaderLvl = 10+ (int)(nbCurModsLoaded / (float)nbModsToLoad * 10);
                    break;
                case LoaderSteps.CombineXML:
                    text = "Building Def Tree...";
                    loaderLvl = 20;
                    break;
                case LoaderSteps.Patching:
                    if (curPatching?.Name != null && curPatching.Name.Length != 0)
                        name = " - " + curPatching.Name;
                    else
                        name = "";

                    text = "Patching : "+nbPatches+" / "+nbPatchesToLoad+name;
                    loaderLvl = 25;
                    break;
                case LoaderSteps.ParsingXML:
                    text = "Parsing Defs : "+nbDefs+" / "+nbDefsToProcess;
                    loaderLvl = 30 + (int)(nbDefs / (float)nbDefsToProcess * 50);
                    break;
                case LoaderSteps.ResolvingReferences:
                    if (curDefResolving?.Name?.Length != 0)
                        name = " - " + curDefResolving.Name;
                    else
                        name = "";

                    text = "RimThemes_LoaderResolvinfDefs".Translate(nbDefResolving,nbDefToResolving,name);
                    loaderLvl = 90;
                    break;
                case LoaderSteps.FinishUp:
                    if (curConstructor?.Name?.Length != 0)
                        name = " - " + curConstructor.Name;
                    else
                        name = "";

                    text = "RimThemes_LoaderCompletingLoading".Translate(nbConstructorsCalled,nbConstructorsToCall,name);
                    loaderLvl = 100;
                    break;
                case LoaderSteps.LoadWorldMap:
                    text = "LoadingWorld".Translate()+"...";
                    nbWorldGenToRun = 0;
                    nbWorldGenRun = 0;
                    loaderLvl = 0;
                    break;
                case LoaderSteps.FillWorldMap:
                    string step = "";
                    if (curWorldGenStep != null)
                        step = " - " + curWorldGenStep;

                    text = "RimThemes_LoaderSaveFillingWorld".Translate(nbWorldGenRun,nbWorldGenToRun,step);
                    loaderLvl = 5;
                    break;
                case LoaderSteps.FinalizeWorld:
                    text = "RimThemes_LoaderSaveFinalizeWorld".Translate();
                    loaderLvl = 15;
                    break;
                case LoaderSteps.MapsInitComps:
                    text = "RimThemes_LoaderSaveMapCompCreating".Translate();
                    loaderLvl = 20;
                    break;
                case LoaderSteps.MapsLoadComps:
                    text = "RimThemes_LoaderSaveMapCompInit".Translate();
                    loaderLvl = 25;
                    break;
                case LoaderSteps.MapsLoadData:
                    text = "RimThemes_LoaderSaveMapLoad".Translate();
                    loaderLvl = 50;
                    break;
                case LoaderSteps.SetCamera:
                    text = "RimThemes_LoaderSaveInitCamera".Translate();
                    loaderLvl = 80;
                    break;
                case LoaderSteps.ResolveSaveFileCrossReferences:
                    text = "RimThemes_LoaderSaveResolve".Translate();
                    loaderLvl = 85;
                    break;
                case LoaderSteps.SpawnThings:
                    text = "SpawningAllThings".Translate()+"...";
                    loaderLvl = 90;
                    break;
                case LoaderSteps.FinalizeLoad:
                    text = "RimThemes_LoaderSaveFinalizeMapLoading".Translate();
                    loaderLvl = 100;
                    break;
                case LoaderSteps.GeneratingPlanet:
                    text = "GeneratingPlanet".Translate();
                    loaderLvl = 20;
                    break;
                case LoaderSteps.FinalizeGeneratingPlanet:
                    text = "GeneratingPlanet".Translate();
                    loaderLvl = 100;
                    break;
                case LoaderSteps.CreateWorldGeneratingWorld:
                    text = "GeneratingWorld".Translate();
                    loaderLvl = 20;
                    break;
                case LoaderSteps.CreateWorldFinalizeWorld:
                    text = "GeneratingWorld".Translate();
                    loaderLvl = 100;
                break;
                case LoaderSteps.InitSaveSaving:
                    text = "SavingLongEvent".Translate();
                    loaderLvl = 20;
                    break;
                case LoaderSteps.FinalizeSaveSaving:
                    text = "SavingLongEvent".Translate();
                    loaderLvl = 100;
                    break;
                case LoaderSteps.InitGeneratingMap:
                    text = "GeneratingMap".Translate();
                    loaderLvl = 20;
                    break;
                case LoaderSteps.InitGeneratingMapForNewEncounter:
                    text = "GeneratingMapForNewEncounter".Translate();
                    loaderLvl = 20;
                    break;
            }
            //Loader
            Rect rectLoader = new Rect(0, UI.screenHeight - 70, (int)(UI.screenWidth*((float)loaderLvl/100)), 10);
            GUI.DrawTexture(rectLoader, getThemeRscLoader(LoaderRSC.LoaderBar), ScaleMode.StretchToFill);

            //If reference to loading text not cached
            Rect rect = new Rect(0, UI.screenHeight - 60, UI.screenWidth, 50);
            Text.Anchor = TextAnchor.MiddleCenter;
            GUI.DrawTexture(rect, getThemeRscLoader(LoaderRSC.TextBar), ScaleMode.StretchToFill);
            Widgets.Label(rect, text);
            Text.Anchor = TextAnchor.UpperLeft;
        }

        /*
         * Converting preloaded theme textures from byte [] to Texture2D
         */
        private void loadThemesTextures()
        {
            //Icones de theme
            var nb = Themes.RDBTexThemeIcon.Count;
            if (nb != 0)
            {    
                foreach(var (key, _) in Themes.RDBTexThemeIcon)
                {
                    Themes.DBTexThemeIcon[key] = Themes.RDBTexThemeIcon[key] != null
                        ? Utils.LoadTexture(Themes.RDBTexThemeIcon[key])
                        : null;
                }
                Themes.RDBTexThemeIcon.Clear();
            }

            //Particles 
            nb = Themes.RDBTexParticle.Count;
            if (nb != 0)
            {
                foreach (var (key, _) in Themes.RDBTexParticle)
                {
                    Themes.DBTexParticle[key] = Themes.RDBTexParticle[key] != null
                        ? Utils.LoadTexture(Themes.RDBTexParticle[key])
                        : null;
                }
                Themes.RDBTexParticle.Clear();
            }

            //Tapestries
            nb = Themes.RDBTexTapestry.Count;
            if (nb != 0)
            {
                foreach (var (key, _) in Themes.RDBTexTapestry)
                {
                    Themes.DBTexTapestry[key] = Themes.RDBTexTapestry[key] != null
                        ? Utils.LoadTexture(Themes.RDBTexTapestry[key])
                        : null;
                }
                Themes.RDBTexTapestry.Clear();
            }

            //Loader TextBar
            nb = Themes.RDBTexLoaderText.Count;
            if (nb != 0)
            {
                foreach (var (key, _) in Themes.RDBTexLoaderText)
                {
                    Themes.DBTexLoaderText[key] = Themes.RDBTexLoaderText[key] != null
                        ? Utils.LoadTexture(Themes.RDBTexLoaderText[key])
                        : null;
                }
                Themes.RDBTexLoaderText.Clear();
            }

            //Loader texture
            nb = Themes.RDBTexLoaderBar.Count;
            if (nb != 0)
            {
                foreach (var (key, _) in Themes.RDBTexLoaderBar)
                {
                    Themes.DBTexLoaderBar[key] = Themes.RDBTexLoaderBar[key] != null
                        ? Utils.LoadTexture(Themes.RDBTexLoaderBar[key])
                        : null;
                }
                Themes.RDBTexLoaderBar.Clear();
            }

            //Loaders
            nb = Themes.RDBLoader.Count;
            if (nb != 0)
            {
                foreach (var (key, value) in Themes.RDBLoader)
                {
                    for (var i = 0; i != value.Length; i++)
                    {
                        Themes.DBLoader[key][i] = value[i] != null
                            ? Utils.LoadTexture(value[i])
                            : null;
                    }
                }
                Themes.RDBLoader.Clear();
            }

            // bacjkground loader
            nb = Themes.RDBBGLoader.Count;
            if (nb != 0)
            {
                foreach (var (key, _) in Themes.RDBBGLoader)
                {
                    Themes.DBBGLoader[key] = Themes.RDBBGLoader[key] != null
                        ? Utils.LoadTexture(Themes.RDBBGLoader[key])
                        : null;
                }
                Themes.RDBBGLoader.Clear();
            }

            // The textures of the themes themselves
            nb = Themes.RDBTex.Count;
            if (nb != 0)
            {
                foreach (var (theme, dict1) in Themes.RDBTex)
                {
                    if (!Themes.DBTex.ContainsKey(theme) || Themes.DBTex[theme] == null)
                        Themes.DBTex[theme] = new Dictionary<string, Dictionary<string, Texture2D>>();

                    foreach (var (type, dict2) in dict1)
                    {
                        if (dict2 != null)
                        {
                            if (!Themes.DBTex[theme].TryGetValue(type, out var existingDict) == false || existingDict == null)
                                Themes.DBTex[theme][type] = new Dictionary<string, Texture2D>();

                            foreach (var (field, bytes) in dict2)
                            {
                                Themes.DBTex[theme][type][field] = bytes != null
                                    ? Utils.LoadTexture(bytes)
                                    : null;
                            }
                        }
                        
                    }
                }
                Themes.RDBTex.Clear();
            }
        }

        /*
         * Request to load a music list
         */
        public void loadThemesSongs()
        {
            try
            {
                //For each theme
                foreach (var entry in Themes.DBSongsToLoad)
                {
                    //Processing of each of the associated sound files
                    foreach (var entry2 in entry.Value)
                    {
                        //Attempt to get the property dynamically to verify sound validity
                        try
                        {
                            if (Application.platform == RuntimePlatform.WindowsPlayer)
                                Themes.DBSong[entry.Key][entry2.Key] = AudioGrain_ClipTheme.winLoadAudio(entry2.Value);
                            else
                                Themes.DBSong[entry.Key][entry2.Key] = AudioGrain_ClipTheme.linuxLoadAudio(entry2.Value);

                            //DBSong[themeID]["EntrySong"].ResolveReferences();
                            //DBSound[themeID][curSoundName] = (AudioClip)((object)Manager.Load(sound, doStream, true, true));
                            Themes.LogMsg(entry.Key+" : Loading custom song EntrySong OK");
                        }
                        catch (Exception e)
                        {
                            Themes.LogMsg(entry.Key + " Invalid custom song " + entry2.Key + " : " + e.Message);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Themes.LogMsg("Fatal error when trying to load themes songs " + e.Message);
            }
        }


        /*
         * Routine for obtaining a loader resource
         */
        public static Texture2D getThemeRscLoader(LoaderRSC rsc)
        {
            if (Themes.TexLoaderBar == null)
            {
                Themes.TexLoaderBar = SolidColorMaterials.NewSolidColorTexture(new Color(0.18f, 0.27f, 0.772f, 0.77f));
                Themes.TexLoaderText = SolidColorMaterials.NewSolidColorTexture(new Color(0.27f, 0.46f, 0.5f, 0.77f));
            }

            Dictionary<string, Texture2D> db = null;
            Texture2D defaultTex = null;
            string fn = "";
            switch (rsc)
            {
                case LoaderRSC.BGLoader:
                    db = Themes.DBBGLoader;
                    defaultTex = getDefaultBGLoader();
                    fn = "BGLoader.jpg";
                    break;
                case LoaderRSC.LoaderBar:
                    db = Themes.DBTexLoaderBar;
                    defaultTex = Themes.TexLoaderBar;
                    fn = "LoaderBar.png";
                    break;
                case LoaderRSC.TextBar:
                    db = Themes.DBTexLoaderText;
                    defaultTex = Themes.TexLoaderText;
                    fn = "TextBar.png";
                    break;
            }

            string[] parts = Settings.curTheme.Split('§');
            if (parts[0] == "-1")
            {
                //Loader not loaded we try to load it
                if (!db.ContainsKey(Settings.curTheme))
                {
                    string path = Utils.currentMod.RootDir + Path.DirectorySeparatorChar
                        + "Themes" + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + fn;
                    if (!File.Exists(path))
                    {
                        //If applicable, loading the loader by default
                        return defaultTex;
                    }
                    try
                    {
                        db[Settings.curTheme] = Themes.customTexLoad(path);
                        return db[Settings.curTheme];
                    }
                    catch (Exception e)
                    {
                        db[Settings.curTheme] = defaultTex;
                        Themes.LogMsg("Cannot load custom loader rsc : " + e.Message);
                        return defaultTex;
                    }
                }
                else
                {
                    //No loader, we grab the default one
                    if (db[Settings.curTheme] == null)
                        return defaultTex;
                    else
                        return db[Settings.curTheme];
                }
            }
            else if (parts[0] == "-2")
            {
                //Loader not loaded we try to load it
                if (!db.ContainsKey(Settings.curTheme))
                {
                    string basePath = Utils.RWBaseFolderPath;

                    string path = basePath + Path.DirectorySeparatorChar
                        + "RimThemes" + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + fn;

                    if (!File.Exists(path))
                    {
                        //If applicable, loading the loader by default
                        return defaultTex;
                    }
                    try
                    {
                        db[Settings.curTheme] = Themes.customTexLoad(path);
                        return db[Settings.curTheme];
                    }
                    catch (Exception e)
                    {
                        db[Settings.curTheme] = defaultTex;
                        Themes.LogMsg("Cannot load custom loader rsc from default themes : " + e.Message);
                        return defaultTex;
                    }
                }
                else
                {
                    //No loader, we grab the default one
                    if (db[Settings.curTheme] == null)
                        return defaultTex;
                    else
                        return db[Settings.curTheme];
                }
            }
            else
            {
                //Theme located in an external theme
                //If already loaded we return it
                if (db.ContainsKey(Settings.curTheme))
                {
                    //No loader, we grab the default one
                    if (db[Settings.curTheme] == null)
                        return defaultTex;
                    else
                        return db[Settings.curTheme];
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
                            //Check if existence Loader.png for the mod
                            string loaderBGPath = cmod.RootDir + Path.DirectorySeparatorChar + "RimThemes" + Path.DirectorySeparatorChar + parts[1] + Path.DirectorySeparatorChar + "Loader" + Path.DirectorySeparatorChar + fn;
                            if (File.Exists(loaderBGPath))
                            {
                                try
                                {
                                    db[Settings.curTheme] = Themes.customTexLoad(loaderBGPath);
                                    return db[Settings.curTheme];
                                }
                                catch (Exception e)
                                {
                                    db[Settings.curTheme] = defaultTex;
                                    Themes.LogMsg("Cannot load custom loader rsc : " + e.Message);
                                    return defaultTex;
                                }
                            }
                        }
                    }

                    //No loader found for the current theme, we return the default one
                    return defaultTex;
                }
            }
        }

        //Return loader by default, if not loaded it is loaded on the way
        private static Texture2D getDefaultBGLoader()
        {
            if (Loader.tex[0] == null)
                Loader.initTextures();

            return Loader.bgTex;
        }


        public static LoaderSteps curStep=0;
        public static int loaderLvl = 0;

        //Loading Theme
        public static string curTheme = "";
        public static int themeLoadMode = 0;
        //Loading XML step
        public static int nbCurModsLoaded=0;
        public static int nbModsToLoad = 0;
        public static ModContentPack curLoadedMod;
        //Patching
        public static int nbPatchesToLoad=0;
        public static int nbPatches=0;
        public static ModContentPack curPatching;
        //Parsing
        public static int nbDefsToProcess=0;
        public static int nbDefs=0;
        //Resolving Defs
        public static Type curDefResolving;
        public static int nbDefResolving=0;
        public static int nbDefToResolving = 0;
        //FinishUp
        public static int nbConstructorsToCall = 0;
        public static int nbConstructorsCalled = 0;
        public static Type curConstructor;

        //LoadWorldMap
        public static int nbWorldGenToRun = 0;
        public static int nbWorldGenRun = 0;
        public static WorldGenStep curWorldGenStep;

        //Saving save
        public static string saveName = "";



        private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);
        public static bool songsToLoad = false;
        public static bool texThemesToLoad = false;
        public static bool fontsToLoad = false;
        public static bool autosave = false;
        public static bool themeTexAlreadyLoaded = false;

        public static bool reachedMainMenu = false;
    }
}

