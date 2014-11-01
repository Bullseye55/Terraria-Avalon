using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using PoroCYon.Extensions.Collections;
using TAPI;
using Terraria;

namespace Avalon.API.Audio
{
    internal delegate void RefAction<T>(ref T t);

    /// <summary>
    /// Manages <see cref="OggVorbis" /> playback, etc.
    /// </summary>
    public static class VorbisPlayer
    {
        /// <summary>
        /// Handles music-related things.
        /// </summary>
        public static class Music
        {
            internal static Dictionary<string, float> bakFade = new Dictionary<string, float>();
            internal static SoundEffectInstance music;

            internal static void  StopOgg(bool dispose = false)
            {
                if (music != null)
                {
                    if (!music.IsDisposed)
                    {
                        if (dispose)
                        {
                            if (music.State != SoundState.Stopped)
                                music.Stop();

                            music.Dispose();
                        }
                        else if (music.State == SoundState.Playing)
                            music.Pause();
                    }

                    music = null;
                }
            }
            static void StartOgg()
            {
                if (music.State != SoundState.Playing)
                {
                    if (!music.IsLooped)
                        music.IsLooped = true;

                    music.Play();
                }
            }

            static void RestoreFade(string current)
            {
                if (!String.IsNullOrEmpty(current) && bakFade.ContainsKey(current))
                {
                    WavebankDef.fade[current] = bakFade[current];
                    bakFade.Remove(current);
                }
            }

            internal static void Update(ref string current)
            {
                string[] split = current.Split(':');
                string
                    mod  = split[0],
                    name = split.Skip(1).Join(String.Empty);

                // no OGG music to be played
                if (!name.ToUpperInvariant().EndsWith(OggExt))
                {
                    RestoreFade(current);

                    StopOgg();

                    return;
                }

                OggVorbis track = GetTrack(mod, name);

                // invalid track
                if (track == null)
                {
                    RestoreFade(current);

                    StopOgg();

                    return;
                }

                if (!String.IsNullOrEmpty(current = WavebankDef.current))
                {
                    if (!bakFade.ContainsKey(current))
                        bakFade.Add(current, WavebankDef.fade[current]);

                    WavebankDef.fade[current] = 0f;
                }

                music = GetInstance(track);

                StartOgg();

                music.Volume = Main.musicVolume;
            }
            internal static void UpdateInactive()
            {
                if (music != null && music.State == SoundState.Playing)
                    music.Pause();
            }
        }
        /// <summary>
        /// Handles sound effect-related things.
        /// </summary>
        public static class Sfx
        {
            /// <summary>
            /// Plays a <see cref="OggVorbis" /> sound effect.
            /// </summary>
            /// <param name="vorbis">The sound effect to play.</param>
            /// <returns></returns>
            public static SoundEffectInstance Play(OggVorbis vorbis)
            {
                return Main.PlaySound(vorbis.Effect);
            }
        }
        
        readonly static string OggExt = ".OGG";

        internal static Dictionary<string   , OggVorbis          > cache     = new Dictionary<string   , OggVorbis          >();
        internal static Dictionary<OggVorbis, SoundEffectInstance> instCache = new Dictionary<OggVorbis, SoundEffectInstance>();

        readonly static string Colon = ":";

        static OggVorbis GetTrack(string modName, string fileName)
        {
            string name = modName + Colon + fileName;
            if (cache.ContainsKey(name))
                return cache[name];

            Mod mod = Mods.mods.FirstOrDefault(m => m.InternalName == modName);

            if (mod == null || !mod.modBase.includes.ContainsKey(fileName))
                return null;

            OggVorbis ret = new OggVorbis(mod.modBase.includes[fileName]);

            cache.Add(name, ret);

            return ret;
        }
        static SoundEffectInstance GetInstance(OggVorbis track)
        {
            if (!instCache.ContainsKey(track))
            {
                SoundEffectInstance inst = track.Effect.CreateInstance();
                instCache.Add(track, inst);
                return inst;
            }

            return instCache[track];
        }

        /// <summary>
        /// Loads an <see cref="OggVorbis" /> and caches it.
        /// </summary>
        /// <param name="resourceName">The name of the resource that contains the OGG Vorbis tack.</param>
        /// <param name="base">The <see cref="ModBase" /> that owns the resource. Default is the <see cref="ModBase" /> of the calling mod.</param>
        /// <returns>The <see cref="OggVorbis" /> track loaded from the resource.</returns>
        public static OggVorbis LoadTrack(string resourceName, ModBase @base = null)
        {
            Mod mod = Mods.mods.FirstOrDefault(m => m != null && m.Loaded && m.modBase != null && m.modBase.GetType().Assembly == Assembly.GetCallingAssembly());
            ModBase actualBase = (@base ?? (mod == null ? AvalonMod.Instance : mod.modBase));

            if (!actualBase.includes.ContainsKey(resourceName))
                return null;

            byte[] data = actualBase.includes[resourceName];

            if (mod == null)
                mod = AvalonMod.Instance.mod;

            OggVorbis track = GetTrack(mod.InternalName, resourceName);
            GetInstance(track);
            return track;
        }

        internal static RefAction<string> Update = Music.Update;
    }
}
