using RimWorld;
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
    [HarmonyPatch(typeof(InspectPaneUtility), "ToggleTab")]
    class InspectPaneUtility_ToggleTab_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(InspectTabBase tab, IInspectPane pane)
        {
            try
            {
                if (!Themes.dialogStacking())
                {
                    if (!(tab.GetType() == pane.OpenTabType || (tab == null && pane.OpenTabType == null)))
                    {
                        Utils.lastShowedWin.Add(new WDESC(2, -235086));
                        //Log.Message("Add -235086");
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Themes.LogException("Patch InspectPaneUtility.ToggleTab failed : ", e);
                return true;
            }
        }
    }



    [HarmonyPatch(typeof(InspectTabBase), "DoTabGUI")]
    class InspectTabBase_DoTabGUI_Patch
    {
        [HarmonyPrefix]
        static bool Prefix()
        {
            try
            {
                if (!Themes.dialogStacking())
                {
                    bool ok = true;
                    foreach(var entry in Utils.lastShowedWin)
                    {
                        if (entry.wid == -235086)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        Utils.lastShowedWin.Add(new WDESC(2, -235086));
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                Themes.LogException("Patch InspectTabBase.DoTabGUI failed : ", e);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(WindowStack), "TryRemove", typeof(Window), typeof(bool))]
    class WindowStack_TryRemove_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Window window, bool doCloseSound = true)
        {
            try
            {
                //Log.Message(">>>Dec " + window.GetType()+" "+ window.ID);
                if (!Themes.dialogStacking() && !Utils.isNSBlacklistedWindowsType(window) && ((window.layer == WindowLayer.Dialog)
                    || (window.layer == WindowLayer.GameUI && window.ID == -235086)
                    || window is MainTabWindow and not MainTabWindow_Inspect or EditWindow))
                {
                    if (Utils.lastShowedWin.Count != 0)
                    {
                        //Removing the current window from the stack
                        Utils.lastShowedWin.RemoveAll(x => x.wid == window.ID);
                        //Log.Message("Dec " + window.ID);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Themes.LogError("Patch WindowStack.TryRemove failed : "+e.Message);
                return true;
            }
        }
    }
    



    [HarmonyPatch(typeof(WindowStack), "Add")]
    class WindowsStack_Add_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Window window,int ___uniqueWindowID)
        {
            try
            {
                if (!Themes.dialogStacking() && !Utils.isNSBlacklistedWindowsType(window) && ((window.layer == WindowLayer.Dialog)
                    || window is MainTabWindow and not MainTabWindow_Inspect or EditWindow))
                {
                    Utils.lastShowedWin.Add(new WDESC(1, ___uniqueWindowID));
                    //Log.Message("Add "+___uniqueWindowID+" "+window.GetType());
                }

                return true;
            }
            catch(Exception e)
            {
                Themes.LogException("Patch WindowStack.Add failed : ", e);
                return true;
            }
        }
    }
}
