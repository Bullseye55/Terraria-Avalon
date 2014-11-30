using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using Avalon.API.Biomes;

namespace Avalon.Tiles.DarkMatter
{
    /// <summary>
    /// The Dark Matter Ooze tile.
    /// </summary>
    public sealed class DarkMatterBrick : DarkMatterTile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DarkMatterOoze" /> class.
        /// </summary>
        public DarkMatterBrick()
            : base(TileCategory.Dirt | TileCategory.Stone | TileCategory.Grass, TileDef.byName["Avalon:Dark Matter Ooze"])
        {
            GetToSpread = pt => TileDef.grass[Main.tile[pt.X, pt.Y].type] ? TileDef.byName["Avalon:Dark Matter Soil"] : TileDef.byName["Avalon:Dark Matter Ooze"];
        }

        /// <summary>
        /// Initializes the <see cref="Tile" />.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            SpreadRatio = 1200; // temp, obviously
        }
    }
}
