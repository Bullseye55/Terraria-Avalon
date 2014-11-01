using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.Items.Potions
{
    /// <summary>
    /// The Brain in a Bottle.
    /// </summary>
    public class BrainInABottle : ModItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
		public override bool? UseItem(Player p)
		{
            p.AddBuff(BuffDef.byName["Avalon:Brain Follower"], 300, true);
            //NPC.NewNPC((int)p.position.X + p.width / 2, (int)p.position.Y + p.height / 2, NPCDef.byName["Avalon:Dark Matter Spearworm Head"].type, 1);
            return null;
		}
	}
}
