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
    public sealed class PollutedGrass : WastelandsTile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PollutedGrass" /> class.
        /// </summary>
        public PollutedGrass()
            : base(TileCategory.Dirt | TileCategory.Grass | TileCategory.Stone, TileDef.byName["Avalon:Polluted Grass"])
        {
            GetToSpread = pt => TileDef.grass[Main.tile[pt.X, pt.Y].type] ? TileDef.byName["Avalon:Polluted Grass"] : TileDef.byName["Avalon:Polluted Dirt"];
        }
    }
}
