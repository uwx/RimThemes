using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace aRandomKiwi.RimThemes
{
    internal class ActiveTheme
    {
        //Storage of fix tables in case of field / class name change to avoid breaking themes
        public readonly string DBFix;

        //Storage of effects data
        public readonly float? DBEffect;

        public readonly Dictionary<GameFont, GUIStyle> DBGUIStyle;
        public readonly Dictionary<GameFont, GUIStyle> DBGUIStyleTextField;
        public readonly Dictionary<GameFont, GUIStyle> DBGUIStyleTextArea;
        public readonly Dictionary<GameFont, GUIStyle> DBGUIStyleTextAreaReadOnly;

        public readonly Dictionary<GameFont, float> DBGUIStyleLineHeight;
        public readonly Dictionary<GameFont, float> DBGUIStyleSpaceBetweenLine;

        //Texture dictionaries
        public readonly Texture2D DBTexThemeIcon;
        public readonly Texture2D DBTexParticle;
        public readonly Texture2D DBTexTapestry;
        public readonly Texture2D[] DBLoader;
        public readonly bool DBNoLoader;
        public readonly bool DBLoaderNotFound;
        public readonly Texture2D DBBGLoader;
        public readonly Texture2D DBTexLoaderBar;
        public readonly Texture2D DBTexLoaderText;
        public readonly Dictionary<(string className, string fieldName), Texture2D> DBTex;

        //Animated background
        public readonly string DBAnimatedBackground;

        //Text color by theme
        public readonly Color? DBTextColorWhite;
        public readonly Color? DBTextColorYellow;
        public readonly Color? DBTextColorGreen;
        public readonly Color? DBTextColorRed;
        public readonly Color? DBTextColorCyan;
        public readonly Color? DBTextColorBlue;
        public readonly Color? DBTextColorGray;
        public readonly Color? DBTextColorMagenta;


        //Texture color by theme
        public readonly Color? DBTexColorWhite;
        public readonly Color? DBTexColorYellow;
        public readonly Color? DBTexColorGreen;
        public readonly Color? DBTexColorRed;
        public readonly Color? DBTexColorCyan;
        public readonly Color? DBTexColorBlue;
        public readonly Color? DBTexColorGray;
        public readonly Color? DBTexColorMagenta;

        //Neutral color of factions
        public readonly Color? DBTextColorFactionsNeutral;

        public readonly WindowAnim? DBWindowAnim;
        public readonly Dictionary<(string key, string text), Color> DBColor;
        public readonly Dictionary<string, AudioGrain_ClipTheme> DBSound;
        public readonly Dictionary<string, AudioClip> DBSong;
        public readonly Dictionary<string, string> DBText;
        public readonly Dictionary<string, int> DBVal;
        public readonly Dictionary<string, string> DBModInfo;
        public readonly Dictionary<Color, Color> DBDynColor;

        //List of music files to load (EntrySong)
        public readonly Dictionary<string, string> DBSongsToLoad;

        public readonly Dictionary<Color, Color> CacheColor = new();
        public readonly Dictionary<Color, Color> CacheTextColor = new();

        public readonly (Color from, Color to)[] DBTextColorCached;
        public readonly (Color from, Color to)[] DBTexColorCached;

        public ActiveTheme()
        {
            // EVERYTHING IS NULL!
        }

        public ActiveTheme(string theme)
        {
            //Storage of fix tables in case of field / class name change to avoid breaking themes
            DBFix = Get(Themes.DBFix, theme);

            //Storage of effects data
            DBEffect = Get(Themes.DBEffect, theme);

            DBGUIStyle = Get(Themes.DBGUIStyle, theme);
            DBGUIStyleTextField = Get(Themes.DBGUIStyleTextField, theme);
            DBGUIStyleTextArea = Get(Themes.DBGUIStyleTextArea, theme);
            DBGUIStyleTextAreaReadOnly = Get(Themes.DBGUIStyleTextAreaReadOnly, theme);

            DBGUIStyleLineHeight = Get(Themes.DBGUIStyleLineHeight, theme);
            DBGUIStyleSpaceBetweenLine = Get(Themes.DBGUIStyleSpaceBetweenLine, theme);

            //Texture dictionaries
            DBTexThemeIcon = Get(Themes.DBTexThemeIcon, theme);
            DBTexParticle = Get(Themes.DBTexParticle, theme);
            DBTexTapestry = Get(Themes.DBTexTapestry, theme);
            DBLoader = Get(Themes.DBLoader, theme);
            DBNoLoader = Get(Themes.DBNoLoader, theme);
            DBLoaderNotFound = Get(Themes.DBLoaderNotFound, theme);
            DBBGLoader = Get(Themes.DBBGLoader, theme);
            DBTexLoaderBar = Get(Themes.DBTexLoaderBar, theme);
            DBTexLoaderText = Get(Themes.DBTexLoaderText, theme);
            DBTex = GetAsTupleDict(Themes.DBTex, theme);
            
            // awful hack to make lookups on DBTex always one operation by falling back on vanilla
            // maybe this is used only during texture loading, so it's unnecessary, because it falls back on Themes.DBTex?
            foreach (var (className, dict1) in Themes.DBTex[Themes.VanillaThemeID])
            foreach (var (fieldName, v) in dict1)
            {
                if (!DBTex.ContainsKey((className, fieldName)))
                {
                    DBTex[(className, fieldName)] = v;
                }
            }

            //Animated background
            DBAnimatedBackground = Get(Themes.DBAnimatedBackground, theme);

            //Text color by theme
            DBTextColorWhite = Get(Themes.DBTextColorWhite, theme);
            DBTextColorYellow = Get(Themes.DBTextColorYellow, theme);
            DBTextColorGreen = Get(Themes.DBTextColorGreen, theme);
            DBTextColorRed = Get(Themes.DBTextColorRed, theme);
            DBTextColorCyan = Get(Themes.DBTextColorCyan, theme);
            DBTextColorBlue = Get(Themes.DBTextColorBlue, theme);
            DBTextColorGray = Get(Themes.DBTextColorGray, theme);
            DBTextColorMagenta = Get(Themes.DBTextColorMagenta, theme);


            //Texture color by theme
            DBTexColorWhite = Get(Themes.DBTexColorWhite, theme);
            DBTexColorYellow = Get(Themes.DBTexColorYellow, theme);
            DBTexColorGreen = Get(Themes.DBTexColorGreen, theme);
            DBTexColorRed = Get(Themes.DBTexColorRed, theme);
            DBTexColorCyan = Get(Themes.DBTexColorCyan, theme);
            DBTexColorBlue = Get(Themes.DBTexColorBlue, theme);
            DBTexColorGray = Get(Themes.DBTexColorGray, theme);
            DBTexColorMagenta = Get(Themes.DBTexColorMagenta, theme);

            //Neutral color of factions
            DBTextColorFactionsNeutral = Get(Themes.DBTextColorFactionsNeutral, theme);

            DBWindowAnim = Get(Themes.DBWindowAnim, theme);
            DBColor = GetAsTupleDict(Themes.DBColor, theme);
            DBSound = Get(Themes.DBSound, theme);
            DBSong = Get(Themes.DBSong, theme);
            DBText = Get(Themes.DBText, theme);
            DBVal = Get(Themes.DBVal, theme);
            DBModInfo = Get(Themes.DBModInfo, theme);
            DBDynColor = Get(Themes.DBDynColor, theme);

            //List of music files to load (EntrySong)
            DBSongsToLoad = Get(Themes.DBSongsToLoad, theme);
            
            // ReSharper disable PossibleInvalidOperationException wtf
            var colorsCachedTemp = new (Color from, Color to)[8];
            var lastI = 0;
            if (DBTextColorWhite != null) colorsCachedTemp[lastI++] = (Color.white, DBTextColorWhite.Value);
            if (DBTextColorYellow != null) colorsCachedTemp[lastI++] = (Color.yellow, DBTextColorYellow.Value);
            if (DBTextColorGreen != null) colorsCachedTemp[lastI++] = (Color.green, DBTextColorGreen.Value);
            if (DBTextColorRed != null) colorsCachedTemp[lastI++] = (Color.red, DBTextColorRed.Value);
            if (DBTextColorCyan != null) colorsCachedTemp[lastI++] = (Color.cyan, DBTextColorCyan.Value);
            if (DBTextColorBlue != null) colorsCachedTemp[lastI++] = (Color.blue, DBTextColorBlue.Value);
            if (DBTextColorGray != null) colorsCachedTemp[lastI++] = (Color.gray, DBTextColorGray.Value);
            if (DBTextColorMagenta != null) colorsCachedTemp[lastI++] = (Color.magenta, DBTextColorMagenta.Value);
            Array.Resize(ref colorsCachedTemp, lastI);
            DBTextColorCached = colorsCachedTemp;

            colorsCachedTemp = new (Color from, Color to)[8];
            lastI = 0;
            if (DBTexColorWhite != null) colorsCachedTemp[lastI++] = (Color.white, DBTexColorWhite.Value);
            if (DBTexColorYellow != null) colorsCachedTemp[lastI++] = (Color.yellow, DBTexColorYellow.Value);
            if (DBTexColorGreen != null) colorsCachedTemp[lastI++] = (Color.green, DBTexColorGreen.Value);
            if (DBTexColorRed != null) colorsCachedTemp[lastI++] = (Color.red, DBTexColorRed.Value);
            if (DBTexColorCyan != null) colorsCachedTemp[lastI++] = (Color.cyan, DBTexColorCyan.Value);
            if (DBTexColorBlue != null) colorsCachedTemp[lastI++] = (Color.blue, DBTexColorBlue.Value);
            if (DBTexColorGray != null) colorsCachedTemp[lastI++] = (Color.gray, DBTexColorGray.Value);
            if (DBTexColorMagenta != null) colorsCachedTemp[lastI++] = (Color.magenta, DBTexColorMagenta.Value);
            Array.Resize(ref colorsCachedTemp, lastI);
            DBTexColorCached = colorsCachedTemp;
        }

        private static Dictionary<(TK1, TK2), TV> GetAsTupleDict<TK1, TK2, TV>(IReadOnlyDictionary<string, Dictionary<TK1, Dictionary<TK2, TV>>> dict, string theme)
        {
            // expensive operation, but it should only happen rarely

            var originalDict = Get(dict, theme);
            if (originalDict == null)
            {
                return null;
            }
            
            var outDict = new Dictionary<(TK1, TK2), TV>();
            foreach (var (k1, dict1) in originalDict)
            foreach (var (k2, v) in dict1)
            {
                outDict[(k1, k2)] = v;
            }

            return outDict;
        }

        private static TV Get<TV>(IReadOnlyDictionary<string, TV> dict, string theme)
        {
            return dict != null && dict.TryGetValue(theme, out var v) ? v : default;
        }
    }

    internal static class KVPExtension
    {
        public static void Deconstruct<TK, TV>(this KeyValuePair<TK, TV> pair, out TK key, out TV value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}