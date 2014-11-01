using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using Avalon.API.Biomes;

namespace Avalon.Tiles.Blocks
{
    /// <summary>
    /// The Dark Matter Soil tile.
    /// </summary>
    public sealed class DarkMatterSoil : SpreadingTile
    {
        /// <summary>
        /// Initializes the <see cref="Tile" />.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            ToSpread = TileDef.byName["Avalon:Dark Matter Soil"];
            IsGrass = IsStone = false;
            PlaceStyle = 0;
            SpreadRatio = 120; // temp, obviously
        }
    }
}
