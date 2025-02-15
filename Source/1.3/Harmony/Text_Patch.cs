﻿using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;
using Verse.Sound;
using HarmonyLib;

namespace aRandomKiwi.RimThemes
{
        [HarmonyPatch(typeof(Text), "get_CurFontStyle")]
        class CurFontStyle_Patch
        {
            [HarmonyPrefix]
            static bool Prefix( ref GUIStyle __result)
            {
                try
                {
                    GUIStyle guistyle;
                    guistyle = Themes.getDBGUIStyle(Text.Font);
                    if (Settings.curTheme == "Vanilla" || guistyle == null)
                    {
                        switch (Text.Font)
                        {
                            case GameFont.Tiny:
                                guistyle = Text.fontStyles[0];
                                break;
                            case GameFont.Small:
                                guistyle = Text.fontStyles[1];
                                break;
                            case GameFont.Medium:
                                guistyle = Text.fontStyles[2];
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    guistyle.alignment = Text.Anchor;
                    guistyle.wordWrap = Text.WordWrap;
                    __result = guistyle;

                    return false;
                }
                catch(Exception e)
                {
                    Themes.LogException("Patch Text.get_CurFontStyle failed : ", e);
                    return true;
                }
            }
        }


    [HarmonyPatch(typeof(Text), "get_CurTextFieldStyle")]
    class CurTextFieldStyle_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(ref GUIStyle __result)
        {
            try
            {
                GUIStyle guistyle;
                guistyle = Themes.getDBGUIStyleTextField(Text.Font);
                if (Settings.curTheme == "Vanilla" || guistyle == null)
                {
                    switch (Text.Font)
                    {
                        case GameFont.Tiny:
                            guistyle = Text.textFieldStyles[0];
                            break;
                        case GameFont.Small:
                            guistyle = Text.textFieldStyles[1];
                            break;
                        case GameFont.Medium:
                            guistyle = Text.textFieldStyles[2];
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                __result = guistyle;

                return false;
            }
            catch(Exception e)
            {
                Themes.LogException("Text.get_CurTextFieldStyle patch failed : ", e);
                return true;
            }
        }
    }


    [HarmonyPatch(typeof(Text), "get_CurTextAreaStyle")]
    class CurTextAreaStyle_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(ref GUIStyle __result)
        {
            try
            {
                GUIStyle guistyle;
                guistyle = Themes.getDBGUIStyleTextArea(Text.Font);
                if (Settings.curTheme == "Vanilla" || guistyle == null)
                {
                    switch (Text.Font)
                    {
                        case GameFont.Tiny:
                            guistyle = Text.textFieldStyles[0];
                            break;
                        case GameFont.Small:
                            guistyle = Text.textFieldStyles[1];
                            break;
                        case GameFont.Medium:
                            guistyle = Text.textFieldStyles[2];
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                __result = guistyle;

                return false;
            }
            catch (Exception e)
            {
                Themes.LogException("Text.get_CurTextAreaStyle patch failed : ", e);
                return true;
            }
        }
    }


    [HarmonyPatch(typeof(Text), "get_CurTextAreaReadOnlyStyle")]
    class CurTextAreaReadOnlyStyle_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(ref GUIStyle __result)
        {
            try
            {
                GUIStyle guistyle;
                guistyle = Themes.getDBGUIStyleTextAreaReadOnly(Text.Font);
                if (Settings.curTheme == "Vanilla" || guistyle == null)
                {
                    switch (Text.Font)
                    {
                        case GameFont.Tiny:
                            guistyle = Text.textFieldStyles[0];
                            break;
                        case GameFont.Small:
                            guistyle = Text.textFieldStyles[1];
                            break;
                        case GameFont.Medium:
                            guistyle = Text.textFieldStyles[2];
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                __result = guistyle;

                return false;
            }
            catch(Exception e)
            {
                Themes.LogException("Text.get_CurTextAreaReadOnlyStyle patch failed : ", e);
                return true;
            }
        }
    }



    [HarmonyPatch(typeof(Text), "get_SpaceBetweenLines")]
    class SpaceBetweenLines_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(ref float __result, float[] ___spaceBetweenLines)
        {
            try
            {
                float ret;
                if (Settings.curTheme == "Vanilla" || Themes.getDBGUIStyle(Text.Font) == null)
                    ret = ___spaceBetweenLines[(int)Text.Font];
                else
                    ret = Themes.getDBGUIStyleSpaceBetweenLine(Text.Font);
                __result = ret;

                return false;
            }
            catch(Exception e)
            {
                Themes.LogException("Text.get_SpaceBetweenLines patch failed : ", e);
                return true;
            }
        }
    }


    [HarmonyPatch(typeof(Text), "get_LineHeight")]
    class LineHeight_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(ref float __result, float[] ___lineHeights)
        {
            try
            {
                float ret;
                if (Settings.curTheme == "Vanilla" || Themes.getDBGUIStyle(Text.Font) == null)
                    ret = ___lineHeights[(int)Text.Font];
                else
                    ret = Themes.getDBGUIStyleLineHeight(Text.Font);
                __result = ret;

                return false;
            }
            catch(Exception e)
            {
                Themes.LogException("Text.get_LineHeight patch failed : ", e);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(Text), "set_Font")]
    class setFont_Patch
    {
        [HarmonyPrefix]
        static bool Prefix()
        {
            if (Utils.textFontSetterLock)
            {
                return false;
            }
            return true;
        }
    }

}
