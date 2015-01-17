using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.Buffs
{
    /// <summary>
    /// The Freeze buff.
    /// </summary>
    public class Freeze : ModBuff
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">The <see cref="NPC" /> that has the buff.</param>
        /// <param name="index">The index of the buff.</param>
        public override void Effects(NPC n, int index)
        {
            n.velocity = Vector2.Zero;

            n.color = new Color(0.27f, 0.51f, 0.76f, 100);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">The <see cref="NPC" /> that has the buff.</param>
        /// <param name="index">The index of the buff.</param>
        public override void End    (NPC n, int index)
        {
            n.color = default(Color);
        }
    }
}
