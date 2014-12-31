using System;
using System.Collections.Generic;
using System.IO;
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
            // keep out. you will break things.

            const float CRESC_VOLUME = 0.005f;

            internal static Dictionary<string, float>
                origFade = new Dictionary<string, float>(),
                overFade = new Dictionary<string, float>();
            internal static SoundEffectInstance music;
            internal static float musicFade = 0f;

            internal static void StopOgg(bool dispose = false)
            {
                if (music == null || music.IsDisposed)
                    return;

                if (dispose)
                {
                    if (music.State != SoundState.Stopped)
                        music.Stop();

                    music.Dispose();

                    musicFade = 0f;

                    music = null;
                }
                else if (musicFade > 0f)
                    musicFade = Math.Max(musicFade - CRESC_VOLUME, 0f);
                else if (music.State == SoundState.Playing)
                {
                    music.Pause();

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
                if (String.IsNullOrEmpty(current) || !origFade.ContainsKey(current))
                    return;

                if (overFade[current] < origFade[current])
                    WavebankDef.fade[current] = overFade[current] = Math.Min(overFade[current] + CRESC_VOLUME, origFade[current]);
                else
                {
                    origFade.Remove(current);
                    overFade.Remove(current);
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
                    if (!origFade.ContainsKey(current))
                    {
                        origFade.Add(current, WavebankDef.fade[current]);
                        overFade.Add(current, WavebankDef.fade[current]);
                    }
                    else
                    {
                        if (overFade[current] > 0f)
                            overFade[current] = Math.Max(overFade[current] - CRESC_VOLUME, -CRESC_VOLUME); // it is increased with CRESC_VOLUME after ChangeTrack in WavebankDef.UpdateMusic

                        WavebankDef.fade[current] = overFade[current];
                    }
                }

                music = GetInstance(track);

                StartOgg();

                if (musicFade < 1f)
                    musicFade = Math.Min(musicFade + CRESC_VOLUME, 1f);

                music.Volume = Main.musicVolume * musicFade;
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
            if (Main.gameMenu && Mods.Reloading)
            {
                Assembly ca = Assembly.GetCallingAssembly();

                Mod m = Mods.mods.FirstOrDefault(_m => _m.modBase.GetType().Assembly == ca);

                Main.statusText = (m == null ? ca.FullName : m.DisplayName) + Environment.NewLine
                    + "Loading music " + Path.GetFileNameWithoutExtension(resourceName);
            }

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
