using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace aRandomKiwi.RimThemes
{
    internal static class ColorsSubstitution
    {
        // Fast replacement for Mathf.Approximately
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CloseEnough(Color left, Color right)
        {
            return Math.Abs(left.r - right.r) <= 0.00390625F
                   && Math.Abs(left.g - right.g) <= 0.00390625F
                   && Math.Abs(left.b - right.b) <= 0.00390625F;
        }

        /*
         * Obtaining substitution color from a GUI.color color for the current applicable theme
         */
        public static Color getTextSubstitutionColor(Color color)
        {
            if (Utils.tempDisableDynColor)
                return color;

            foreach (var (from, to) in Themes.ActiveTheme.DBTextColorCached)
            {
                if (CloseEnough(color, from))
                {
                    return to with { a = color.a };
                }
            }

            //DynColors
            if (Themes.ActiveTheme.DBDynColor != null && Themes.ActiveTheme.DBDynColor.TryGetValue(color, out var result1))
            {
                return result1;
            }

            return color;
        }

        /*
          * Obtaining Texture substitution color from a GUI.color color for the current applicable theme
          */
        public static Color getTextureSubstitutionColor(Color color)
        {
            if (Utils.tempDisableDynColor)
                return color;

            foreach (var (from, to) in Themes.ActiveTheme.DBTexColorCached)
            {
                if (CloseEnough(color, from))
                {
                    return to with { a = color.a };
                }
            }
            
            //DynColors
            if (Themes.ActiveTheme.DBDynColor != null && Themes.ActiveTheme.DBDynColor.TryGetValue(color, out var result1))
            {
                return result1;
            }

            return color;
        }

    }
}
