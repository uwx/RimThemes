﻿using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Video;
using Verse;
using Verse.Sound;
using HarmonyLib;

namespace aRandomKiwi.RimThemes
{
    [HarmonyPatch(typeof(Game), "FinalizeInit")]
    internal static class Background_FinalizeInitPatch
    {
        internal static void Postfix()
        {
            //QualitySettings.vSyncCount = 0;
            Utils.CurrentMainAnimatedBgPlaying = false;
            Utils.CurrentMainAnimatedBgSourceSet = false;
        }
    }

    [HarmonyPatch(typeof(UI_BackgroundMain), "BackgroundOnGUI")]
    class UI_BackgroundMain_Patch
    {
        private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);
        public static Vector2 MainBackgroundSize = new Vector2(2000f, 1190f);

        [HarmonyPrefix]
        static bool Prefix()
        {
            try
            {
                //If custom loader disabled and still loading before loading step of theme textures then we leave the usual bg(NO blackscreen)
                // Or if the current theme does not exist(steam HS, mod theme deleted)
                if ((Settings.disableCustomLoader && !LoaderGM.themeTexAlreadyLoaded) || Settings.disableWallpaper || !Themes.currentThemeExist)
                    return true;

                //Obtaining the applicable theme background
                var curTheme = Themes.getCustomBackgroundApplyableTheme();


                if (Settings.curTheme == Themes.VanillaThemeID && Settings.disableRandomBg)
                    return true;

                //If the animated background is available and loaded, we start reading it!
                var animatedBgOK = Current.ProgramState == ProgramState.Entry && Themes.DBAnimatedBackground.ContainsKey(curTheme) && !Settings.disableVideoBg;
                var animatedBgOKException = false;
                if (animatedBgOK)
                {
                    Rect pos;
                    if (Screen.width <= Screen.height * (MainBackgroundSize.x / (double)MainBackgroundSize.y))
                    {
                        var height = Screen.height;
                        var width = Screen.height * (MainBackgroundSize.x / MainBackgroundSize.y);
                        pos = new Rect(Screen.width / 2 - width / 2f, 0.0f, width, height);
                    }
                    else
                    {
                        var width = Screen.width;
                        var height = Screen.width * (MainBackgroundSize.y / MainBackgroundSize.x);
                        pos = new Rect(0.0f, Screen.height / 2 - height / 2f, width, height);
                    }

                    if (!Utils.CurrentMainAnimatedBgPlaying)
                    {
                        //Utils.CurrentMainAnimatedBg == null &&
                        if (Utils.CurrentMainAnimatedBg == null)
                        {

                            Utils.CurrentMainAnimatedBg = Current.Root_Entry.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
                            Utils.CurrentMainAnimatedBg.enabled = true;
                        }

                        
                        if (!Utils.CurrentMainAnimatedBgSourceSet)
                        {
                            Utils.CurrentMainAnimatedBg.renderMode = VideoRenderMode.RenderTexture;
                            Themes.LogMsg("Start animated background animation");
                            Utils.CurrentMainAnimatedBg.isLooping = true;
                            Utils.CurrentMainAnimatedBg.audioOutputMode = VideoAudioOutputMode.None;
                            Utils.CurrentMainAnimatedBg.targetCameraAlpha = 1.0F;
                            Utils.CurrentMainAnimatedBg.frame = 0;
                            Utils.CurrentMainAnimatedBg.playOnAwake = true;


                            Utils.CurrentMainAnimatedBg.url = Themes.DBAnimatedBackground[curTheme];
                            Utils.CurrentMainAnimatedBgSourceSet = true;
                            Utils.CurrentMainAnimatedBg.errorReceived += delegate (VideoPlayer source, string message)
                            {
                                Themes.LogMsg("VideoPlayer_Error : " + message + " ");
                            };
                            Utils.CurrentMainAnimatedBg.time = 0;
                            Utils.CurrentMainAnimatedBg.Prepare();
                        }

                        if (Utils.CurrentMainAnimatedBg.isPrepared)
                        {
                            Themes.LogMsg("Animated background animation of duration " + Utils.CurrentMainAnimatedBg.length);
                            Utils.CurrentMainAnimatedBg.Play();
                            Utils.CurrentMainAnimatedBgPlaying = true;
                        }

                        //To wait show the picture background
                        animatedBgOKException = true;
                    }
                    else
                    {
                        GUI.DrawTexture(pos, Utils.CurrentMainAnimatedBg.texture, ScaleMode.ScaleToFit);
                    }
                }

                if(!animatedBgOK || animatedBgOKException)
                {
                    var flag = !(UI.screenWidth > UI.screenHeight * (BGPlanetSize.x / BGPlanetSize.y));

                    Rect position;
                    if (flag)
                    {
                        var height = (float)UI.screenHeight;
                        var num = (float)UI.screenHeight * (BGPlanetSize.x / BGPlanetSize.y);
                        position = new Rect(UI.screenWidth / 2 - num / 2f, 0f, num, height);
                    }
                    else
                    {
                        var width = (float)UI.screenWidth;
                        var num2 = (float)UI.screenWidth * (BGPlanetSize.y / BGPlanetSize.x);
                        position = new Rect(0f, UI.screenHeight / 2 - num2 / 2f, width, num2);
                    }

                    Texture bg = Themes.getThemeTex("UI_BackgroundMain", "BGPlanet",curTheme);
                    if (!ReferenceEquals(bg, null))
                        GUI.DrawTexture(position, bg, ScaleMode.ScaleToFit);
                }

                return false;
            }
            catch (Exception e)
            {
                Themes.LogError("UI.BackgroundMain patch failed : "+e.Message);
                return true;
            }
        }
    }
   
}
