using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.Items.Armour.DarkMatter
{
    /// <summary>
    /// The Dark Matter Chest.
    /// </summary>
	public sealed class DarkMatterChest : ModItem
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public override void ArmorSetBonus(Player p)
        {
            Main.dust[Dust.NewDust(p.position, p.width, p.height, 27, 0, 0, 200, Color.Purple, 1.0f)].noGravity = true;

            p.setBonus = "The Dark Matter has spread";
            p.invis = true;

               for (int i = 0; i < Main.gore.Length; i++)
                   if (Vector2.DistanceSquared(Main.gore[i].position, p.Centre) < 100 * 100 && Main.gore[i].active == true)
                   {
                       int x = (int)(Main.gore[i].position.X / 16f), y = (int)(Main.gore[i].position.Y / 16f);
                       Item.NewItem((int)Main.gore[i].position.X, (int)Main.gore[i].position.Y, (int)Main.gore[i].scale, (int)Main.gore[i].scale, ItemDef.byName["Avalon:Dark Matter Ooze"].type, 1, false, 0);
                       Main.gore[i].active = false;

                   }
        }
	}
}
