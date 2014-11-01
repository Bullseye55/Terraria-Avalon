using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        /// Gets whether the <see cref="OggVorbis" /> instance is disposed or not.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return Effect.IsDisposed;
            }
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
            Effect = CreateEffect(new MemoryStream(data));
        }

        internal static byte[] GetByteFromResource(string res, ModBase @base)
        {
            Mod mod = Mods.mods.FirstOrDefault(m => m != null && m.Loaded && m.modBase != null && m.modBase.GetType().Assembly == Assembly.GetCallingAssembly());
            return (@base ?? (mod == null ? AvalonMod.Instance : mod.modBase)).includes[res];
        }

        static void   SwapEndianness(byte[] data)
        {
            byte temp;
            int len = (int)Math.Floor(data.Length / 2d);

            for (int i = 0; i < len; i++)
            {
                temp = data[i];
                data[i] = data[data.Length - i];
            }

            temp = data[0];
            data[0] = data[3];
            data[3] = data[0];
        }
        static uint   SwapEndianness(uint   data)
        {
            return
                ((data & 0x000000FF) << 24) |
                ((data & 0x0000FF00) << 8) |
                ((data & 0x00FF0000) >> 8) |
                ((data & 0xFF000000) >> 24)
            ;
        }
        static  int   SwapEndianness( int   data)
        {
            return BitConverter.IsLittleEndian ? data : (data >> 24 | data << 24 | (data & 65280) << 8 | (data & 16711680) >> 8);
        }
        static ushort SwapEndianness(ushort data)
        {
            return (ushort)(
                ((data & 0x00FF) << 8) |
                ((data & 0xFF00) >> 8)
            );
        }
        static  short SwapEndianness( short data)
        {
            return unchecked((short)SwapEndianness((ushort)data));
        }

        static MemoryStream CreateWave(AudioChannels channels, int rate, float[] data)
        {
            MemoryStream ms = new MemoryStream();

            BinaryWriter w = new BinaryWriter(ms, Encoding.UTF8);

            unchecked // MEEP MOOP
            {
                // --- RIFF header ---
                w.Write(new[] { 'R', 'I', 'F', 'F' });
                w.Write(data.Length * sizeof(ushort) + 36); // chunck size (dword)
                w.Write(new[] { 'W', 'A', 'V', 'E' });

                // --- format data ---
                w.Write(new[] { 'f', 'm', 't', ' ' });
                // 16-bit PCM (big endian) (dword)
                w.Write(16);
                // format type (PCM=1) (big endian) (word)
                w.Write((ushort)       1);
                w.Write((ushort)channels);
                // sample rate (big endian) (dword)
                w.Write((uint  )rate);
                // byte rate   (big endian) (dword)
                w.Write((uint  )(rate * (int)channels * sizeof(ushort)));
                // block align (big endian) ( word)
                w.Write((ushort)(       (int)channels * sizeof(ushort)));
                w.Write((ushort)16);

                // --- (actual) wave data ---
                w.Write(new[] { 'd', 'a', 't', 'a' });

                w.Write(data.Length * sizeof(ushort)); // data length

                for (int i = 0; i < data.Length; i++)
                    w.Write((short)(data[i] < 0f ? data[i] * 32768f : data[i] * 32767f));
            }

            ms.Position = 0L;
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

                using (MemoryStream ms = CreateWave((AudioChannels)r.Channels, r.SampleRate, data))
                {
                    return SoundEffect.FromStream(ms);
                }
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
