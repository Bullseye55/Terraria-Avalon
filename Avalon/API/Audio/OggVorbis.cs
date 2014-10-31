using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using NVorbis;
using TAPI;

namespace Avalon.API.Audio
{
    /// <summary>
    /// OGG Vorbis audio.
    /// </summary>
    public class OggVorbis : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="SoundEffect" /> that is contained in the OGG Vorbis data.
        /// </summary>
        public SoundEffect Effect
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OggVorbis" /> class.
        /// </summary>
        /// <param name="resource">The name of the resource that contains the OGG Vorbis data.</param>
        /// <param name="base">The <see cref="ModBase" /> that holds the OGG Vorbis data. Default is the calling mod's <see cref="ModBase" />.</param>
        public OggVorbis(string resource, ModBase @base = null)
            : this(GetByteFromResource(resource, @base))
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="OggVorbis" /> class.
        /// </summary>
        /// <param name="data">The OGG Vorbis data of the audio.</param>
        public OggVorbis(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Effect = CreateEffect(ms, false);
            }
        }

        static byte[] GetByteFromResource(string res, ModBase @base)
        {
            Mod mod = Mods.mods.FirstOrDefault(m => m.modBase.GetType().Assembly == Assembly.GetCallingAssembly());
            return (@base ?? (mod == null ? AvalonMod.Instance : mod.modBase)).includes[res];
        }

        static Stream CreateWave(AudioChannels channels, int rate, float[] data)
        {
            MemoryStream ms = new MemoryStream();

            using (BinaryWriter w = new BinaryWriter(ms))
            {
                w.Write(new char[4] { 'R', 'I', 'F', 'F' });
                w.Write(36 + data.Length);
                w.Write(new char[4] { 'W', 'A', 'V', 'E' });

                w.Write(new char[4] { 'f', 'm', 't', ' ' });
                w.Write(16);
                w.Write((short)1);
                w.Write((short)((int)channels & 0xFFFF));
                w.Write(rate);
                w.Write((rate * ((16 * (int)channels) / 8)));
                w.Write((short)(((16 * (int)channels) / 8)) & 0xFFFF);
                w.Write((short)16);

                w.Write(new char[4] { 'd', 'a', 't', 'a' });
                w.Write(data.Length);

                for (long i = 0L; i < data.LongLength; i++)
                    w.Write(data[i]);
            }

            return ms;
        }

        /// <summary>
        /// Creates a <see cref="SoundEffect" /> from an OGG Vorbis <see cref="Stream" />.
        /// </summary>
        /// <param name="vorbisData">The <see cref="Stream" /> that contains the OGG Vorbis data.</param>
        /// <param name="disposeStream">Whether to dispose <paramref name="vorbisData" /> after the track has been loaded.</param>
        /// <returns>The <see cref="SoundEffect" /> created from <paramref name="vorbisData" />.</returns>
        public static SoundEffect CreateEffect(Stream vorbisData, bool disposeStream = true)
        {
            using (VorbisReader r = new VorbisReader(vorbisData, disposeStream))
            {
                float[] data = new float[r.TotalSamples];

                r.ReadSamples(data, 0, (int)r.TotalSamples);

                return SoundEffect.FromStream(CreateWave((AudioChannels)r.Channels, r.SampleRate, data));
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Effect.Dispose();
        }
    }
}
