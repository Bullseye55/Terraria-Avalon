using System;
using System.Collections.Generic;
using System.Linq;
using TAPI;
using Terraria;

namespace Avalon.Tests.ProjectileTesting
{ 
    class NetProjectile : ModProjectile
    {
        public override void AI()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (this.projectile.Hitbox.Intersects(NPCDef.byType[i].Hitbox))
                {
                    NPC.CatchNPC(i);
                    break;
                }
            }
        }
    }
}
 