using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.Tiles.Furniture
{
    /// <summary>
    /// 
    /// </summary>
    public class Lightsource : ModTileType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="wireType"></param>
        public override void HitWire(int x, int y, int wireType)
        {
            //Upper-left corner of tile
            Point p = TileDef.FindTopLeftPoint(x, y);
            x = p.X; 
            y = p.Y;
            int tileID = Main.tile[x, y].type;
            bool lightOn = Main.tile[x, y].frameX == 0;

            for (int i = 0; i < TileDef.width[tileID]; i++)
            {
                for (int j = 0; j < TileDef.height[tileID]; j++)
                {
                    if (Wiring.running)
                        Wiring.SkipWire(x + i, y + j);
                    Main.tile[x + i, y + j].frameX += (short)((18 * TileDef.width[tileID]) * (lightOn?1:-1));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            Point p = TileDef.FindTopLeftPoint(x, y);
            x = p.X;
            y = p.Y;
            bool lightOn = Main.tile[x, y].frameX == 0;
            r = (lightOn) ? 1 : 0;
            g = (lightOn) ? .4f : 0;
            b = (lightOn) ? .7f : 0;
        }
    }
}
