using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TAPI;
using Terraria;

namespace Avalon.Tests.ProjectileTesting
{
    class NetBullet : ModProjectile
    {
        public override void PostKill()
        {
            Projectile.NewProjectile(this.projectile.position, this.projectile.velocity, "Avalon:Net", 0, 0f);
        }
    }
}
