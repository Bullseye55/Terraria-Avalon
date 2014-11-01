using System;
using System.Collections.Generic;
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
            return Biome.Biomes["Avalon:Dark Matter"].Check(p) && Main.hardMode && Main.rand.Next(7) == 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void NPCLoot()
        {
            
        }
    }
}
