using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.API.Biomes
{
    /// <summary>
    /// A <see cref="Tile" /> that spreads.
    /// </summary>
    public abstract class SpreadingTile : ModTile
    {
        int? toSpread = null;

        /// <summary>
        /// Gets the spread ratio.
        /// </summary>
        public int SpreadRatio
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets the type of the tile to spread.
        /// </summary>
        public int ToSpread
        {
            get
            {
                return toSpread ?? ((Tile)entity).type;
            }
            protected set
            {
                toSpread = value < 0 ? null : (int?)value;
            }
        }
        /// <summary>
        /// Gets the place style of the tile to spread.
        /// </summary>
        public int PlaceStyle
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets whether the tile to spread is grass or not.
        /// </summary>
        public bool IsGrass
        {
            get;
            protected set;
        }
        /// <summary>
        /// Gets whether the tile to spread is stone or not.
        /// </summary>
        public bool IsStone
        {
            get;
            protected set;
        }

        bool CanSpreadOn(Point pos)
        {
            int type = Main.tile[pos.X, pos.Y].type;
            return Main.tile[pos.X, pos.Y].active() && type != ToSpread
                && !TileDef.breaksByCut[type] && TileDef.solid[type] && !TileDef.door[type] && !TileDef.alchemyFlower[type] && !TileDef.brick[type]
                && !TileDef.chair[type] && !TileDef.noAttach[type] && !TileDef.platform[type] && !TileDef.rope[type] && !TileDef.table[type]
                && !(TileDef.stone[type] && IsStone) && !TileDef.tileDungeon[type] && !TileDef.tileFlame[type] && !(TileDef.grass[type] && IsGrass);
        }

        /// <summary>
        /// Updates the <see cref="ModTile" />.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // up
            if      (CanSpreadOn(new Point(position.X    , position.Y - 1)) && Main.rand.Next(2) == 0)
                WorldGen.PlaceTile(position.X    , position.Y - 1, ToSpread, true, true, style: PlaceStyle);
            // down
            else if (CanSpreadOn(new Point(position.X    , position.Y + 1)) && Main.rand.Next(2) == 0)
                WorldGen.PlaceTile(position.X    , position.Y + 1, ToSpread, true, true, style: PlaceStyle);
            // left
            else if (CanSpreadOn(new Point(position.X - 1, position.Y    )) && Main.rand.Next(2) == 0)
                WorldGen.PlaceTile(position.X - 1, position.Y    , ToSpread, true, true, style: PlaceStyle);
            // right
            else if (CanSpreadOn(new Point(position.X + 1, position.Y    )) && Main.rand.Next(2) == 0)
                WorldGen.PlaceTile(position.X + 1, position.Y    , ToSpread, true, true, style: PlaceStyle);
        }
    }
}
