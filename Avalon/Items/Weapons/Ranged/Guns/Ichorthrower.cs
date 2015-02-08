using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Avalon.Items.Weapons.Ranged.Guns
{
    /// <summary>
    /// The Ichorthrower.
    /// </summary>
    public class Ichorthrower : ModItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CanUse(Player p)
        {
            return p.inventory.Any(i => !i.IsBlank() && i.type == 23);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool ConsumeAmmo(Player p)
        {
            return Main.rand.Next(6) == 0;
        }

        public static readonly PlayerLayer BackpackLayer = new PlayerLayer.Action("Avalon:Ichorpack", (layer, player, sb) =>
        {
            SpriteEffects e = (player.gravDir == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically) | (player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            sb.Draw(AvalonMod.ichorPack, new Vector2((int)(player.Center.X - Main.screenPosition.X - (player.direction == 1 ? player.width : -6)), (int)(player.Center.Y - Main.screenPosition.Y - ((player.gravDir == 1) ? 23 : 9)) + Vector2.UnitY.Y * player.gfxOffY), null, Lighting.GetColor((int)(player.position.X + player.width * 0.5) / 16, (int)((player.position.Y + player.height * 0.5f) / 16.0)), 0f, Vector2.Zero, 1f, e, 0);
        });
    }
}
