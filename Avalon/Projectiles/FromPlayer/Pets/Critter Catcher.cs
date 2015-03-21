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
    public sealed class CritterCatcher : ModProjectile
    {
        const float MAX_TARGET_DIST = 350f;

        int curTar = -1;

        /// <summary>
        ///
        /// </summary>
        public override void AI()
        {
            base.AI();

            if (curTar != -1)
            {
                projectile.velocity *= 0.85f;

                NPC tar = Main.npc[curTar];

                if (!tar.active || tar.life <= 0 || projectile.DistanceSQ(tar.Centre) > MAX_TARGET_DIST)
                    curTar = -1;
            }

            if (curTar == -1)
            {
                float dmin = MAX_TARGET_DIST;
                int sel = -1;

                for (int i = 0; i < Main.npc.Length; i++)
                    if (Main.npc[i].active && Main.npc[i].catchItem != 0)
                    {
                        float d = projectile.DistanceSQ(Main.npc[i].Centre);

                        if (d < dmin)
                            sel = i;
                    }

                curTar = sel;
            }

            if (curTar != -1)
            {
                NPC tar = Main.npc[curTar];

                projectile.velocity = Vector2.Normalize(tar.Centre - projectile.Centre) * 7f;

                if (projectile.Hitbox.Intersects(tar.Hitbox))
                {
                    Item.NewItem(tar.position, tar.Size, tar.catchItem);
                    tar.active = false;
                    curTar = -1;
                }
            }
        }
    }
}
