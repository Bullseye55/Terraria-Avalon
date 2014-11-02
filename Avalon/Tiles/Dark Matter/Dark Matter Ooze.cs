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
    public sealed class DarkMatterOoze : DarkMatterTile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DarkMatterOoze" /> class.
        /// </summary>
        public DarkMatterOoze()
            : base(TileCategory.Dirt | TileCategory.Stone, TileDef.byName["Avalon:Dark Matter Ooze"])
        {

        }

        /// <summary>
        /// Initializes the <see cref="Tile" />.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            SpreadRatio = 120; // temp, obviously
        }
    }
}
