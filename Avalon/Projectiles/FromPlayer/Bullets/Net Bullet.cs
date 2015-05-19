using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using TAPI;

namespace Avalon.Projectiles.FromPlayer.Bullets
{
    /// <summary>
    ///
    /// </summary>
    public sealed class NetBullet : ModProjectile
    {
        /// <summary>
        ///
        /// </summary>
        public override void PostKill()
        {
            Projectile.NewProjectile(this.projectile.position, this.projectile.velocity, "Avalon:Net", 0, 0f);
        }
    }
}
