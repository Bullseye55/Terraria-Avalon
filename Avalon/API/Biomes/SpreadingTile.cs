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

        bool CanSpreadOn(Point pos)
        {
            return Main.tile[pos.X, pos.Y].active() && Main.tile[pos.X, pos.Y].type != ToSpread && !TileDef.breaksByCut[Main.tile[pos.X, pos.Y].type];
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
