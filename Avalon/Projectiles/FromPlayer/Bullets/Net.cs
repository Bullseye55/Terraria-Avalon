using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.Projectiles.FromPlayer.Bullets
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Net : ModProjectile
    {
        /// <summary>
        ///
        /// </summary>
        public override void PostKill()
        {
            for (int i = 0; i < Main.npc.Length; i++)
                if (projectile.Hitbox.Intersects(NPCDef.byType[i].Hitbox))
                {
                    NPC.CatchNPC(i);
                    break;
                }
        }
    }
}
