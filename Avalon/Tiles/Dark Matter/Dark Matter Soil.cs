using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using Avalon.API.Biomes;

namespace Avalon.Tiles.DarkMatter
{
    /// <summary>
    /// The Dark Matter Soil tile.
    /// </summary>
    public sealed class DarkMatterSoil : SpreadingTile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DarkMatterSoil" /> class.
        /// </summary>
        public DarkMatterSoil()
            : base(TileCategory.Grass)
        {

        }

        /// <summary>
        /// Initializes the <see cref="Tile" />.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            ToSpread = TileDef.byName["Avalon:Dark Matter Soil"];
            SpreadOn += pt => AvalonMod.DarkMatter.CountNum() < 350;
            PlaceStyle = 0;
            SpreadRatio = 120; // temp, obviously
        }

        ///// <summary>
        ///// Updates the tile.
        ///// </summary>
        //public override void Update()
        //{
        //    base.Update();

        //    if (AvalonMod.DarkMatter.CountNum() >= 350)
        //        World.DarkMatter.Reinforce();
        //}
    }
}
