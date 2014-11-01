using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.ModClasses
{
    /// <summary>
    /// 
    /// </summary>
    [GlobalMod]
    public sealed class MTileType : ModTileType
    {
        const float
            R = 52f / 255f,
            G = 0f,
            B = 91f / 255f;

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
            // hook not called .__.

            base.ModifyLight(x, y, ref r, ref g, ref b);

            if (Main.dedServ)// || !AvalonMod.DarkMatter.TileValid(x, y, Main.myPlayer))
                return;

            r = R;
            g = G;
            b = B;
        }
    }
}
