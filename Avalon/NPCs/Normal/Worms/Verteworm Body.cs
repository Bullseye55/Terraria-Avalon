using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.NPCs.Normal.Worms
{
    /// <summary>
    /// The Verteworm's body.
    /// </summary>
    public sealed class VertewormBody : ModNPC
    {
        /// <summary>
        /// 
        /// </summary>
        public override void AI()
        {
            //Code help on, massivley, by Yoraiz0r. Give thanks to him if you see this.

            int toFollowSlot = 1;

            bool shouldSuicide = false;

            if (npc.ai[toFollowSlot] < 0 || npc.ai[toFollowSlot] > Main.maxNPCs || !Main.npc[(int)npc.ai[toFollowSlot]].active)
                shouldSuicide = true;

            #region Suicide on realLife Depletion
            if (shouldSuicide)
            {
                npc.life = 0;
                npc.active = false;
                npc.HitEffect(0, 9999); 
                return;
            }
            #endregion

            #region Living AI
            NPC toFollow = Main.npc[(int)npc.ai[toFollowSlot]];
            npc.rotation = npc.DirectionTo(toFollow.Centre).ToRotation() + MathHelper.PiOver2;
            float distancing = 18f * npc.scale;
            Vector2 offsetting = npc.DirectionFrom(toFollow.Centre) * distancing;

            npc.velocity = Vector2.Zero;
            npc.Centre = toFollow.Centre + offsetting;
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        public override void NPCLoot()
        {
            if (Main.rand.Next(4) == 0)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, (int)npc.width, (int)npc.height, 1330, 1, false, 0);
        }
    }
}
