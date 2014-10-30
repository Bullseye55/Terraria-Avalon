using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.NPCs.Normal.Worms
{
    /// <summary>
    /// The Dark Matter Spearworm's head.
    /// </summary>
    public sealed class DMSpearwormHead : ModNPC
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
                    // Leaves four segments for a tail, for the unique look.
                    NPC segment = Main.npc[NPC.NewNPC((int)npc.Centre.X, (int)npc.Centre.Y, NPCDef.byName["Avalon:Dark Matter Spearworm " + ((i >= 0 && i < 10) ? "Body" : "Tail")].type, npc.whoAmI)];

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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CanSpawn(int x, int y, int type, Player p)
        {
            return /*Main.player[Main.myPlayer].zoneBlood && Main.hardMode && Main.rand.Next(7) == 1*/ false;
        }
    }
}
