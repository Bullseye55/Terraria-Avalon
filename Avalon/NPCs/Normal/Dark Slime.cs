using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.NPCs.Normal
{
    /// <summary>
    /// The Dark Slime spawning procedure, used by Dark Slimes 1 and 2.
    /// </summary>
    public sealed class DarkSlime : ModNPC
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CanSpawn(int x, int y, int type, Player p)
        {
            return AvalonMod.DarkMatter.Check(p) && Main.hardMode && Main.rand.Next(7) == 1;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void NPCLoot()
        {
            const int amt = -1;//, dmg = -1, owner = -1;
            Vector2 spawnAt = Vector2.Zero;
            string projName = String.Empty;
            //const float kb = -1f, maxRange = 32f;

            for (int i = 0; i < amt; i++)
            {
                double rot = Main.rand.NextDouble() * MathHelper.TwoPi;
                float speed = 0f;

                Vector2 velocity = new Vector2((float)Math.Cos(rot) * speed, (float)Math.Sin(rot) * speed);

                Dust d = Main.dust[Dust.NewDust(spawnAt, velocity, 58)];
                //d.timeLeft = (int)(maxRange / velocity.Length()); // in ticks, x is in px, v is in px/tick
            }
        }
    }
}
