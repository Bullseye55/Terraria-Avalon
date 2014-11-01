using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;

namespace Avalon.NPCs.Pets
{
    /// <summary>
    /// The Brain Follower.
    /// </summary>
    /// <remarks>Not the Brian follower.</remarks>
    public sealed class BrainFollower : ModNPC
    {
        int aTimer = 0;
        bool
            bA = false,
            cA = false;

        /// <summary>
        /// 
        /// </summary>
        public override void AI()
        {
            aTimer++;

            if (aTimer == 360)
                bA = true;

            if (aTimer == 615)
            {
                bA = false;
                cA = true;
            }
            if (aTimer == 870)
            {
                cA = false;
                aTimer = 0;
            }

            if (bA == true)
                npc.alpha += 3;
            if (cA == true)
                npc.alpha -= 3;

            npc.target = (int)npc.ai[0];

            bool buffOn = false;
            for (int i = 0; i < Main.player[npc.target].buffType.Length; i++)
                if (Main.player[npc.target].buffType[i] == BuffDef.byName["Avalon:Brain Follower"])
                {
                    buffOn = true;

                    if (Main.player[npc.target].buffTime[i] < 590)
                        Main.player[npc.target].buffTime[i] = 600;

                    break;
                }

            if (!buffOn)
                npc.active = false;

            if (Main.player[npc.target].position.X + 100f < npc.position.X || Main.player[npc.target].position.X - 100f > npc.position.X || Main.player[npc.target].position.Y + 100f < npc.position.Y || Main.player[npc.target].position.Y - 100f > npc.position.Y)
            {
                if (Main.player[npc.target].position.X + 100f < npc.position.X)
                {
                    if (npc.velocity.X > -10f)
                        npc.velocity.X -= 0.6f;
                }
                else if (Main.player[npc.target].position.X - 100f > npc.position.X)
                    if (npc.velocity.X < 10f)
                        npc.velocity.X += 0.6f;

                if (Main.player[npc.target].position.Y + 100f < npc.position.Y)
                {
                    if (npc.velocity.Y > -10f)
                        npc.velocity.Y -= 0.6f;
                }
                else if (Main.player[npc.target].position.Y - 100f > npc.position.Y)
                    if (npc.velocity.Y < 10f)
                        npc.velocity.Y += 0.6f;
            }
            else
            {
                npc.velocity.X *= 0.94f; npc.velocity.Y *= 0.94f;
                npc.ai[2]++;

                if (npc.ai[2] == 60)
                {
                    npc.velocity += new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));

                    npc.ai[2] = 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameSize"></param>
        public override void SelectFrame(int frameSize)
        {
            int frameNum = 1;
            if (!Main.dedServ)
                frameNum = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];

            npc.frameCounter += 1.0;
            if (npc.frameCounter >= 8.0)
            {
                npc.frame.Y = npc.frame.Y + frameNum;
                npc.frameCounter = 0.0;
            }
            if (npc.frame.Y >= frameNum * Main.npcFrameCount[npc.type])
                npc.frame.Y = 0;
        }
    }
}
