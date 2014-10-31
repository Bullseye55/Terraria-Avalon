using System;
using System.Collections.Generic;
using System.Linq;
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
        readonly static string OggExt = ".OGG";

        internal static Dictionary<string   , OggVorbis          > cache     = new Dictionary<string   , OggVorbis          >();
        internal static Dictionary<OggVorbis, SoundEffectInstance> instCache = new Dictionary<OggVorbis, SoundEffectInstance>();

        /// <summary>
        /// Handles music-related things.
        /// </summary>
        public static class Music
        {
            static SoundEffectInstance music;
            static float bakVolume;
            static string old;

            static OggVorbis GetTrack(string name, string modName, string fileName)
            {
                if (cache.ContainsKey(name))
                    return cache[name];

                Mod mod = Mods.mods.FirstOrDefault(m => m.InternalName == modName);

                if (mod == null || !mod.modBase.includes.ContainsKey(fileName))
                    return null;

                return new OggVorbis(mod.modBase.includes[fileName]);
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

            internal static void Update(ref string current)
            {
                old = WavebankDef.current;

                string[] split = current.Split(':');
                string
                    mod  = split[0],
                    name = split.Skip(1).Join(String.Empty);

                if (!name.ToUpperInvariant().EndsWith(OggExt))
                {
                    if (music != null)
                    {
                        if (music.State != SoundState.Stopped)
                            music.Stop();

                        music = null;
                    }

                    return;
                }

                OggVorbis track = GetTrack(current, mod, name);

                if (track == null)
                {
                    if (music != null)
                    {
                        if (music.State != SoundState.Stopped)
                            music.Stop();

                        music = null;
                    }

                    return;
                }

                current = String.Empty;

                music = GetInstance(track);

                if (music.State != SoundState.Playing)
                {
                    if (!music.IsLooped)
                        music.IsLooped = true;

                    music.Play();
                }
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

        internal static RefAction<string> Update = Music.Update;
    }
}
