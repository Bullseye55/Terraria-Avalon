using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TAPI;
using Terraria;

namespace Avalon.Tests.ProjectileTesting
{
    class NetBulletProjectile : ModProjectile
    {
        public override void AI()
        {
            int timer = 0;
            if (timer <= 20)
                timer++;
            else
            {
                Projectile.NewProjectile(this.projectile.position, this.projectile.velocity, "Avalon:NetProjectile", 0, 0f);
                this.projectile.Kill();
            }
        }
    }
}
