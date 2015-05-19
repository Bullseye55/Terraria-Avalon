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
       /* public override void AI()
        {
            for (int i = 0; i < Main.npc.Length; i++)
                if (projectile.Hitbox.Intersects(NPCDef.byType[i].Hitbox))
                {
                    NPC.CatchNPC(i);
                    break;
                }
        }
       */
        public override void AI()
        {
            foreach (NPC npc in Main.npc)
                if (projectile.Hitbox.Intersects(npc.Hitbox))
                {
                    if (npc.active)
                    {
                        if (Main.netMode == 1)
                        {
                            npc.active = false;
                            NetMessage.SendData(70, -1, -1, "", npc.whoAmI, (float)projectile.owner, 0.0f, 0.0f, 0);
                        }
                        else
                        {
                            if ((int)npc.catchItem > 0)
                            {
                                int num = npc.type;
                                new Item().SetDefaults((int)npc.catchItem, false);
                                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, 0, 0, (int)npc.catchItem, 1, false, 0, true);
                                npc.active = false;
                                NetMessage.SendData(23, -1, -1, "", npc.whoAmI, 0.0f, 0.0f, 0.0f, 0);
                            }
                        }
                    }

                    break;
                }
        } 
         
    }
}
