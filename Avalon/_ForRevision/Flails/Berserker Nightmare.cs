using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;
using Avalon.API;
using Avalon.API.Items;

namespace Avalon.Items.Weapons.Melee.Flails
{
    /// <summary>
    /// The Berserker Nightmare.
    /// </summary>
    [ChainTexture("Berserker Chain", ReplaceFlailChain = true)]
    public sealed class BerserkerNightmare : ModItem
    {
        /// <summary>
        /// Called before the <see cref="Player" /> shoots a <see cref="Projectile" /> with h[is|er] held <see cref="Item" />.
        /// </summary>
        /// <param name="p">The <see cref="Player" /> that is about to shoot.</param>
        /// <param name="pos">Where the <see cref="Projectile" /> will spawn.</param>
        /// <param name="vel">The velocity of the <see cref="Projectile" />.</param>
        /// <param name="type">The type of the <see cref="Projectile" />.</param>
        /// <param name="dmg">The damage of the <see cref="Projectile" />.</param>
        /// <param name="kb">The knockback of the <see cref="Projectile" />.</param>
        /// <returns>true if the <see cref="Player" /> can continue shooting, false otherwise.</returns>
        public override bool PreShoot(Player p, Vector2 pos, Vector2 vel, int type, int dmg, float kb)
        {
            int spr = 24;

            for (int i = 0; i < 2; i++)
                ExtendedSpawning.NewProj(pos, vel + new Vector2(Main.rand.Next(-spr, spr + 1), Main.rand.Next(-spr, spr + 1)) * 0.05f, type, dmg, kb, p.whoAmI);

            return false;
        }
    }
}
