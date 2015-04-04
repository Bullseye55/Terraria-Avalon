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
        const float MAX_TARGET_DIST = 1638400f;

        int curTar = -1, dragItem = -1;

        /// <summary>
        ///
        /// </summary>
        public override void AI()
        {
            base.AI();

            if (curTar != -1)
            {
                NPC tar = Main.npc[curTar];

                if (!tar.active || tar.life <= 0 || projectile.DistanceSQ(tar.Centre) > MAX_TARGET_DIST)
                    curTar = -1;
            }

            if (curTar == -1 && dragItem == -1)
            {
                projectile.velocity *= 0.85f;

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
                    dragItem = Item.NewItem(tar.position, tar.Size, tar.catchItem);
                    tar.active = false;
                    curTar = -1;
                }
            }

            if (dragItem != -1)
            {
                Item d = Main.item[dragItem];

                if (!d.active || d.IsBlank())
                {
                    dragItem = -1;
                    return;
                }

                Player o = Main.player[projectile.owner];

                d.Centre = projectile.Centre;

                projectile.velocity = Vector2.Normalize(o.Centre - projectile.Centre) * 5f;

                if (projectile.Hitbox.Intersects(o.Hitbox))
                    dragItem = -1;
            }
        }
    }
}
