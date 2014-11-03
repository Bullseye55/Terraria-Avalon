using System;
using System.Collections.Generic;
using System.Linq;
using TAPI;
using Terraria;

namespace Avalon.ModClasses
{
    /// <summary>
    /// The global <see cref="ModNPC" /> class of the Avalon mod.
    /// </summary>
    [GlobalMod]
    public sealed class MNPC : ModNPC
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pool"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public override List<int> EditSpawnPool(int x, int y, List<int> pool, Player p)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                // I'll check for the attributes later... P:
                if (AvalonMod.DarkMatter.Check(p))
                {

                }
                if (AvalonMod.Wraiths.IsActive)
                {
                    
                }
            }

            return pool;
        }
    }
}
