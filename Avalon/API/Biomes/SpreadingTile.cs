using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.API.Biomes
{
    /// <summary>
    /// The category of a spreadable tile.
    /// </summary>
    [Flags]
    public enum TileCategory
    {
        /// <summary>
        /// The tile is a soil tile (like dirt and mud).
        /// </summary>
        Dirt    = 0x01,
        /// <summary>
        /// The tile is a grass tile (like grass, corrupt grass, jungle grass, ...).
        /// </summary>
        Grass   = 0x02,
        /// <summary>
        /// The tile is a stone tile (like stone, ebonstone, hallowstone, ...)
        /// </summary>
        Stone   = 0x04,
        /// <summary>
        /// The tile is a dungeon tile (like blue, green and purple dungeon bricks).
        /// </summary>
        Dungeon = 0x08,
        /// <summary>
        /// The tile is a regular (non-dungeon) brick (like red brick, hellstone brick, obsidian brick, ...)
        /// </summary>
        Brick   = 0x10
    }

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
        /// Gets the category of the tile to spread.
        /// </summary>
        public TileCategory Category
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a function used to get the tile type that should spread on the specified point.
        /// </summary>
        protected Func<Point, int > GetToSpread
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets a function used to determine whether the tile can spread on the tile at the specified point.
        /// </summary>
        protected Func<Point, bool> SpreadOn
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SpreadingTile" /> class.
        /// </summary>
        protected SpreadingTile(TileCategory category)
            : base()
        {
            Category = category;

            GetToSpread = p => ToSpread;
            SpreadOn = _CanSpreadOn;
        }

        static bool BelongsToCategory(TileCategory category, Point pt)
        {
            int type = Main.tile[pt.X, pt.Y].type;

            return ((category & TileCategory.Grass  ) != 0 && TileDef.grass      [type])
                || ((category & TileCategory.Stone  ) != 0 && TileDef.stone      [type])
                || ((category & TileCategory.Dungeon) != 0 && TileDef.tileDungeon[type])
                || ((category & TileCategory.Brick  ) != 0 && TileDef.brick      [type])
                || ((category & TileCategory.Dirt   ) != 0
                    && !TileDef.grass[type] && !TileDef.stone[type] && !TileDef.tileDungeon[type] && !TileDef.brick[type]
                    );
        }
        bool _CanSpreadOn(Point pt)
        {
            int type = Main.tile[pt.X, pt.Y].type;
            return Main.tile[pt.X, pt.Y].active() && type != TileToSpread(pt)
                && !TileDef.breaksByCut[type] &&  TileDef.solid    [type] && !TileDef.door    [type] && !TileDef.alchemyFlower[type]
                && !TileDef.chair      [type] && !TileDef.noAttach [type] && !TileDef.platform[type] && !TileDef.rope         [type]
                && !TileDef.table      [type] && !TileDef.tileFlame[type]
                && BelongsToCategory(Category, pt);
        }

        /// <summary>
        /// Gets the type of the tile to spread on the tile at the specified point.
        /// </summary>
        /// <param name="pt">The coordinates of the tile to check.</param>
        /// <returns>The type of the tile to spread at the given point.</returns>
        public int TileToSpread(Point pt)
        {
            return GetToSpread == null ? ToSpread : GetToSpread(pt);
        }
        /// <summary>
        /// Gets whether the tile can spread on the tile at the specified point.
        /// </summary>
        /// <param name="pt">The coordinates of the tile to check.</param>
        /// <returns>true if the tile can spread on the given tile, false otherwise.</returns>
        public bool CanSpreadOn(Point pt)
        {
            return SpreadOn == null ? _CanSpreadOn(pt) : SpreadOn(pt);
        }

        void Spread(Point pt)
        {
            int type = Main.tile[pt.X, pt.Y].type;
            //WorldGen.PlaceTile(pt.X, pt.Y, GetToSpread == null ? ToSpread : GetToSpread(pt), true, true, style: PlaceStyle);
            Main.tile[pt.X, pt.Y].type = (ushort)(GetToSpread == null ? ToSpread : GetToSpread(pt));
            WorldGen.TileFrame(pt.X, pt.Y, true);
            OnSpread(pt, type);
        }

        /// <summary>
        /// Updates the <see cref="ModTile" />.
        /// </summary>
        public override void Update()
        {
            // hook not called .__.

            base.Update();

            Point pt;

            // up
            if      (CanSpreadOn(pt = new Point(position.X    , position.Y - 1)) && Main.rand.Next(2) == 0)
                Spread(pt);

            // down
            else if (CanSpreadOn(pt = new Point(position.X    , position.Y + 1)) && Main.rand.Next(2) == 0)
                Spread(pt);

            // left
            else if (CanSpreadOn(pt = new Point(position.X - 1, position.Y    )) && Main.rand.Next(2) == 0)
                Spread(pt);

            // right
            else if (CanSpreadOn(pt = new Point(position.X + 1, position.Y    )))
                Spread(pt);
        }

        /// <summary>
        /// Called when the <see cref="SpreadingTile" /> spreads on a tile.
        /// </summary>
        /// <param name="pt">The position of the tile where the <see cref="SpreadingTile" /> spread on.</param>
        /// <param name="oldType">The type of the tile before it was changed.</param>
        protected virtual void OnSpread(Point pt, int oldType) { }
    }
}
