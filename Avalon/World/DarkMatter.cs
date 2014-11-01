using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.World
{
    /// <summary>
    /// The Dark Matter biome.
    /// </summary>
    public sealed class DarkMatter : Biome
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DarkMatter" /> class.
        /// </summary>
        public DarkMatter()
            : base("Avalon:Dark Matter", new List<int>()
            {
                TileDef.byName["Avalon:Dark Matter Ooze"],
                TileDef.byName["Avalon:Dark Matter Soil"]
            }, AvalonMod.EmptyIntList, 100 /* temp value */)
        {
            biomeMusic = "Avalon:Resources/Music/Dark Matter (temp).ogg";
            biomeMusicPriority = MusicPriority.Med;
        }

        /// <summary>
        /// Reinforces all Dark Matter tiles. Not implemented yet.
        /// </summary>
        [Obsolete("This method is not implemented yet.")]
        public static void Reinforce()
        {

        }
    }
}
