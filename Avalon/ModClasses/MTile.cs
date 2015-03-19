using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.ModClasses
{
    /// <summary>
    /// The global <see cref="ModTile" /> class of the Avalon mod.
    /// </summary>
    [GlobalMod]
    public sealed class MTile : ModTile
    {
        static bool CanPlacePot(Point p)
        {
            int
                x = p.X,
                y = p.Y;

            return !Main.tile[x, y].active() && !Main.tile[x + 1, y].active() && !Main.tile[x, y + 1].active() && !Main.tile[x + 1, y + 1].active();
        }
        static void    PlacePot(Point stonep, int fx, int fy)
        {
            int
                tx = stonep.X    ,
                ty = stonep.Y - 2;

            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 2; x++)
                {
                    Main.tile[tx + x, ty + y].active(true);
                    Main.tile[tx + x, ty + y].type   = 28; // pot
                    Main.tile[tx + x, ty + y].frameX = (short)fx;
                    Main.tile[tx + x, ty + y].frameY = (short)fy;
                }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            base.Update();

            Tile t = Main.tile[position.X, position.Y];

            if (CanPlacePot(position))
            {
                int fx = -1, fy = -1;

                if (t.type ==  25) // ebonstone
                {
                    fx = WorldGen.genRand.Next(3) + 17;
                    fy = WorldGen.genRand.Next(3) +  1;
                }
                if (t.type == 203) // crimstone
                {
                    fx = WorldGen.genRand.Next(3) + 23;
                    fy = WorldGen.genRand.Next(3) +  1;
                }

                if (fx != -1 && fy != -1 && WorldGen.genRand.Next(20000) == 0)
                    PlacePot(position, fx, fy);
            }
        }
    }
}
