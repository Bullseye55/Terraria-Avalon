using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using Avalon.API.Biomes;

namespace Avalon.Tiles.Wastelands
{
    /// <summary>
    /// The Dark Matter Ooze tile.
    /// </summary>
    public sealed class PollutedDirt : WastelandsTile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PollutedDirt" /> class.
        /// </summary>
        public PollutedDirt()
            : base(TileCategory.Dirt | TileCategory.Stone | TileCategory.Grass, TileDef.byName["Avalon:Polluted Grass"])
        {
            GetToSpread = pt => TileDef.grass[Main.tile[pt.X, pt.Y].type] ? TileDef.byName["Avalon:Polluted Grass"] : TileDef.byName["Avalon:Polluted Dirt"];
        }
    }
}
