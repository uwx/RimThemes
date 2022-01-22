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
    [HarmonyPatch(typeof(FactionRelationKindUtility), "GetColor")]
    class FactionRelationKindUtility_GetColor_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(FactionRelationKind kind, ref Color __result)
        {
            try
            {
                string theme = Settings.curTheme;
                if (kind != FactionRelationKind.Neutral || Themes.ActiveTheme.DBTextColorFactionsNeutral == null)
                    return true;

                __result = Themes.ActiveTheme.DBTextColorFactionsNeutral.Value;
                return false;
            }
            catch(Exception e)
            {
                Themes.LogException("Patch FactionRelationKindUtility.GetColor failed : ", e);
                return true;
            }
        }
    }
}