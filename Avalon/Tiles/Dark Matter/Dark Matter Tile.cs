using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.API.Biomes;
using TAPI;
using Terraria;

namespace Avalon.Tiles.DarkMatter
{
    /// <summary>
    /// The base class of a tile that belongs to the Dark Matter biome.
    /// </summary>
    public abstract class DarkMatterTile : SpreadingTile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DarkMatterTile" /> class.
        /// </summary>
        /// <param name="category">The category of the tile.</param>
        /// <param name="type">The type of the tile.</param>
        protected DarkMatterTile(TileCategory category, int type = -1)
            : base(category)
        {
            ToSpread = type;
        }

        ///// <summary>
        ///// Updates the tile.
        ///// </summary>
        //public override void Update()
        //{
        //    base.Update();

        //    if (AvalonMod.DarkMatter.CountNum() >= World.DarkMatter.MINIMUM_TILES)
        //        World.DarkMatter.Reinforce();
        //}
    }
}
