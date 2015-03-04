using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.Items.Accessories
{
    /// <summary>
    /// The Shiver Heart.
    /// </summary>
	public sealed class ShiverHeart : ModItem
    {
        /// <summary>
        /// When the <see cref="Player" /> has the <see cref="Item" /> equipped.
        /// </summary>
        /// <param name="p"></param>
        public override void Effects(Player p) 
        {
            if (p.wet && !p.lavaWet)
                p.lifeRegen = -32;

            if (p.lavaWet && !p.wet)
            {
                p.lifeRegen += 3;
                p.lavaImmune = true;
            }
        }
	}
}
