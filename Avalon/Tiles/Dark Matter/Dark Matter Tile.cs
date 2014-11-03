using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Avalon.API.Biomes;
using Avalon.ModClasses;

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

        /// <summary>
        /// Updates the tile.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (AvalonMod.DarkMatter.CountNum() >= World.DarkMatter.MINIMUM_TILES)
                World.DarkMatter.Reinforce();
        }

        /// <summary>
        /// Called when the <see cref="SpreadingTile" /> spreads on a tile.
        /// </summary>
        /// <param name="pt">The position of the tile where the <see cref="SpreadingTile" /> spread on.</param>
        /// <param name="oldType">The type of the tile before it was changed.</param>
        protected override void OnSpread(Point pt, int oldType)
        {
            base.OnSpread(pt, oldType);

            MWorld.DarkMatterSpreaded++;
        }
    }
}
