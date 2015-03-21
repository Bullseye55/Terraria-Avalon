using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.Projectiles.FromPlayer.Pets
{
    /// <summary>
    ///
    /// </summary>
    public sealed class FishCatcher : ModProjectile
    {
        Vector2 closestWater = new Vector2(Single.NaN);

        int dragItem = -1; // ?

        /// <summary>
        ///
        /// </summary>
        public override void AI()
        {
            base.AI();

            if (Single.IsNaN(closestWater.X))
            {
                int
                    sx = (int)(projectile.position.X * 0.0625f /* / 16f */) - 40,
                    sy = (int)(projectile.position.Y * 0.0625f /* / 16f */) - 40;

                for (int y = sy; y < sy + 80; y++)
                    for (int x = sx; x < sx + 80; x++)
                        if (Main.tile[x, y].liquid > 0)
                            closestWater = new Vector2(x * 16f, y * 16f);
            }

            if (!projectile.wet)
                projectile.velocity = Vector2.Normalize(closestWater - projectile.Centre) * 4f;
            else
            {
                projectile.velocity *= 0.75f;

                Player o = Main.player[projectile.owner];
                Item i = o.inventory[o.selectedItem];

                int poleBak = i.fishingPole;
                i.fishingPole = 28;
                int l = o.FishingLevel();
                i.fishingPole = poleBak;

                Item n = null; // .........?

                if (n == null || n.IsBlank())
                    return;

                Item b = o.GetBait();

                int chance = Math.Max(b.bait / 5, 1) + o.accTackleBox;
                bool consume = (chance <= 1 || Main.rand.Next(chance) == 0) && n.rare >= 0;

                if (consume && --b.stack <= 0)
                    b.SetDefaults(0);
            }
        }
    }
}
