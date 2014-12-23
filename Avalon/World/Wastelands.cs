using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.World
{
    /// <summary>
    /// The Wastelands biome.
    /// </summary>
    public sealed class Wastelands : Biome
    {
        /// <summary>
        /// Gets the required amount of Wasteland tiles to have a Wastelands biome.
        /// </summary>
        public const int MINIMUM_TILES = 250;

        /// <summary>
        /// Creates a new instance of the <see cref="Wastelands" /> class.
        /// </summary>
        public Wastelands()
            : base("Avalon:Wastelands", new List<int>()
            {
                TileDef.byName["Avalon:Polluted Dirt" ],
                TileDef.byName["Avalon:Polluted Grass"]
            }, AvalonMod.EmptyIntList, MINIMUM_TILES)
        {

        }
    }
}
