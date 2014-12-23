using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Avalon.API.Biomes;
using Avalon.ModClasses;

namespace Avalon.Tiles.Wastelands
{
    /// <summary>
    /// The base class of a tile that belongs to the Wastelands biome.
    /// </summary>
    public abstract class WastelandsTile : SpreadingTile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WastelandsTile" /> class.
        /// </summary>
        /// <param name="category">The category of the tile.</param>
        /// <param name="type">The type of the tile.</param>
        protected WastelandsTile(TileCategory category, int type = -1)
            : base(category)
        {
            ToSpread = type;

            SpreadOn += p => Biome.Biomes["Jungle"].typesIncrease.Contains(Main.tile[p.X, p.Y].type);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // don't spread on other WL tiles
            SpreadOn += pt => !AvalonMod.Wastelands.typesIncrease.Contains(Main.tile[pt.X, pt.Y].type);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            base.Update();

            SpreadRatio = MWorld.WastelandsSpreadRatio;
        }
    }
}
