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
    /// The Quad Sunfury.
    /// </summary>
    [ChainTexture("Quad Whip Chain", ReplaceFlailChain = true)]
    public sealed class QuadSunfury : ModItem
    {
        /// <summary>
        /// All possible modes of the <see cref="QuadSunfury" />.
        /// </summary>
        public enum Mode : byte
        {
            /// <summary>
            /// Four Sunfuries are launched in a sequiental order.
            /// </summary>
            Sequiental,
            /// <summary>
            /// Four Sunfuries are launched in four different directions.
            /// </summary>
            FourDirections,
            /// <summary>
            /// One huge Sunfury is launched.
            /// </summary>
            OneBig,
            /// <summary>
            /// Four Sunfuries are launched in a spread towards the mouse.
            /// </summary>
            Spread
        }

        const int CD_MAX = 15;

        int seq_timer = 0;
        int cd = 0;

        /// <summary>
        /// Gets or sets the <see cref="Mode" /> of the <see cref="QuadSunfury" />.
        /// </summary>
        public Mode UseMode
        {
            get;
            set;
        }

        /// <summary>
        /// Called when the <see cref="Player" /> is using the <see cref="Item" />.
        /// </summary>
        /// <param name="p">The <see cref="Player" /> that is currently using the <see cref="Item" />.</param>
        public override void UseStyle(Player p)
        {
            base.UseStyle(p);

            if (UseMode == Mode.Sequiental && seq_timer < 46 && ++seq_timer % 15 == 1)
            {
                float
                    vX = Main.mouseX + Main.screenPosition.X - p.Centre.X,
                    vY = Main.mouseY + Main.screenPosition.Y - p.Centre.Y,
                    dist = 12f / (float)Math.Sqrt(vX * vX + vY * vY);

				ExtendedSpawning.NewProj(p.Centre, new Vector2(vX * dist, vY * dist), ProjDef.byName["Avalon:Quad Ball"].type, item.damage, item.knockBack, p.whoAmI);
            }
        }

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
            seq_timer = 0;

            switch (UseMode)
            {
                case Mode.FourDirections:
					ExtendedSpawning.NewProj(pos,  vel, type, dmg, kb, p.whoAmI);
					ExtendedSpawning.NewProj(pos, -vel, type, dmg, kb, p.whoAmI);
                    Projectile.NewProjectile(pos.X, pos.Y, -vel.X,  vel.Y, type, dmg, kb, p.whoAmI);
                    Projectile.NewProjectile(pos.X, pos.Y,  vel.X, -vel.Y, type, dmg, kb, p.whoAmI);
                    break;
                case Mode.OneBig:
					ExtendedSpawning.NewProj(pos, vel, ProjDef.byName["Avalon:Mega Quad Ball"].type, dmg, kb, p.whoAmI);
                    break;
                case Mode.Sequiental:
                    // see UseStyle
                    break;
                case Mode.Spread:
                    byte spread = 24;

                    for (int i = 0; i < 4; i++)
						ExtendedSpawning.NewProj(pos, vel + new Vector2(Main.rand.Next(-spread, spread + 1), Main.rand.Next(-spread, spread + 1)) * 0.1f, type, dmg, kb, p.whoAmI);
                    break;
            }

            return false;
        }
        /// <summary>
        /// Called when the <see cref="Player" /> is holding the <see cref="Item" />.
        /// </summary>
        /// <param name="p">The <see cref="Player" /> that is currently using the <see cref="Item" />.</param>
        public override void HoldStyle(Player p)
        {
            base.HoldStyle(p);

            if (item.tooltips.Count <= 1 || String.IsNullOrEmpty(item.tooltip2))
                item.tooltip2 = "Mode: " + UseMode;

            if (cd < CD_MAX)
                cd++;

            if (cd >= CD_MAX && Main.mouseRight && Main.mouseRightRelease && !p.mouseInterface)
            {
                cd = 0;

                if (++UseMode > Mode.Spread)
                    UseMode = Mode.Sequiental;

                Main.NewText("Mode: " + UseMode);
                item.tooltip2 = "Mode: " + UseMode;
            }
        }

        /// <summary>
        /// Saves data about the <see cref="Item" /> to a <see cref="BinBuffer" />.
        /// </summary>
        /// <param name="bb">The <see cref="BinBuffer" /> to write data to.</param>
        public override void Save(BinBuffer bb)
        {
            base.Save(bb);

            bb.Write((byte)UseMode);
        }
        /// <summary>
        /// Loads data about the <see cref="Item" /> from a <see cref="BinBuffer" />.
        /// </summary>
        /// <param name="bb">The <see cref="BinBuffer" /> to read data from.</param>
        public override void Load(BinBuffer bb)
        {
            base.Load(bb);

            UseMode = (Mode)bb.ReadByte();
            item.tooltip2 = "Mode: " + UseMode;
        }
    }
}
