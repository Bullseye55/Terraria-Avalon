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

        internal static Dictionary<string, OggVorbis> cache = new Dictionary<string, OggVorbis>();

        /// <summary>
        /// Handles music-related things.
        /// </summary>
        public static class Music
        {
            static OggVorbis music;
            static float bakVolume;
            static string old;

            static OggVorbis GetTrack(string name)
            {
                if (cache.ContainsKey(name))
                    return cache[name];



                return null;
            }

            internal static void Update(ref string current)
            {
                old = WavebankDef.current;

                string[] split = current.Split(':');
                string
                    completeName = current,
                    mod  = split[0],
                    name = split.Skip(1).Join(String.Empty);

                if (!name.ToUpperInvariant().EndsWith(OggExt))
                    return;

                current = String.Empty;


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
