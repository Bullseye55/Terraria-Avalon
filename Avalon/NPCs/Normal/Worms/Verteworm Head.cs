using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.NPCs.Normal.Worms
{
    /// <summary>
    /// The Veteworm's head NPC.
    /// </summary>
    public sealed class VertewormHead : ModNPC
    {
        bool hasTail = false;

        /// <summary>
        /// 
        /// </summary>
        public override void AI()
        {
            if (!hasTail)
            {
                NPC prev = npc;

                for (int i = 0; i < 14; i++)
                {
                    NPC segment = Main.npc[NPC.NewNPC((int)npc.Centre.X, (int)npc.Centre.Y , NPCDef.byName["Avalon:Verteworm " + ((i >= 0 && i < 13) ? "Body" : "Tail")].type, npc.whoAmI)];

                    segment.realLife = npc.whoAmI;
                    segment.ai[2] = npc.whoAmI;
                    segment.ai[1] = prev.whoAmI;
                    prev.ai[0] = segment.whoAmI;

                    NetMessage.SendData(23, -1, -1, String.Empty, segment.whoAmI, 0f, 0f, 0f, 0);

                    prev = segment;
                }
                hasTail = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void NPCLoot()
        {
            Gore.NewGore(npc.position, npc.velocity, GoreDef.gores["Avalon:Verteworm Head Gore"], 1f);
        }
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
            return p.zoneBlood && Main.rand.Next(13) == 0;
        }
    }
}
