using RuntimeAudioClipLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace aRandomKiwi.RimThemes
{
    public class AudioGrain_ClipTheme : AudioGrain_Clip
    {
        [NoTranslate]
        public string themeClipPath = string.Empty;

        [DebuggerHidden]
        public override IEnumerable<ResolvedGrain> GetResolvedGrains()
        {
            AudioClip clip = null;
            //Specific treatment depending on the platform
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                clip = winLoadAudio(this.themeClipPath);
            }
            else
            {
                try
                {
                    clip = linuxLoadAudio(this.themeClipPath);
                }
                catch (Exception e)
                {
                    Themes.LogMsg("Unable resolve grain : " + e.Message);
                }
            }
            if (clip != null)
            {
                yield return new ResolvedGrain_Clip(clip);
            }
            else
            {
                Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath);
            }
        }

        static public AudioClip winLoadAudio(string themeClipPath)
        {
            try
            {
                bool doStream = ShouldStreamAudioClipFromPath(themeClipPath);
                return Manager.Load(themeClipPath, doStream, true, true);
            }
            catch (Exception e)
            {
                Themes.LogMsg("winLoadAudio failed : " + e.Message);
                return null;
            }
        }

#pragma warning disable 618
        static public AudioClip linuxLoadAudio(string themeClipPath)
        {
            string url = GenFilePaths.SafeURIForUnityWWWFromPath(themeClipPath);
            using var www = new WWW(url);
            www.threadPriority = UnityEngine.ThreadPriority.High;
            while (!www.isDone)
            {
                Thread.Sleep(1);
            }
            if (www.error != null)
            {
                throw new InvalidOperationException(www.error);
            }
            var clip = www.GetAudioClip();
            if (clip != null)
            {
                clip.name = Path.GetFileNameWithoutExtension(new FileInfo(themeClipPath).Name);
            }

            return clip;
        }
#pragma warning restore 618

        private static bool ShouldStreamAudioClipFromPath(string absPath)
        {
            var fileInfo = new FileInfo(absPath);
            return fileInfo.Exists && fileInfo.Length > 307200L;
        }
    }
}