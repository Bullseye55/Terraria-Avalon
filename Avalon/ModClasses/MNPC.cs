using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using Avalon.API.NPCs;
using Avalon.API.World;

namespace Avalon.ModClasses
{
    /// <summary>
    /// The global <see cref="ModNPC" /> class of the Avalon mod.
    /// </summary>
    [GlobalMod]
    public sealed class MNPC : ModNPC
    {
        /// <summary>
        /// Called before the <see cref="NPC" />'s loot is dropped.
        /// </summary>
        /// <returns></returns>
        public override bool PreNPCLoot()
        {
            if (VanillaDrop.Drops.ContainsKey(npc.type))
            {
                VanillaDrop[] dropArr = VanillaDrop.Drops[npc.type];

                for (int i = 0; i < dropArr.Length; i++)
                    if (Main.rand.NextDouble() < dropArr[i].Chance)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, dropArr[i].Type, dropArr[i].Amount(), false, -1, true);
            }

            return base.PreNPCLoot();
        }

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
                if (!NPCDef.byType.ContainsKey(pool[i]))
                    continue;

                NPC n = NPCDef.byType[pool[i]];

                bool keepAlive = n.townNPC || n.boss; // other conditions?

                if (!keepAlive)
                {
                    if (AvalonMod.DarkMatter.Check(p))
                        for (int j = 0; j < n.modEntities.Count; i++)
                        {
                            ModNPC mn = n.modEntities[j];

                            object[] arr = mn.GetType().GetCustomAttributes(typeof(DarkMatterNpcAttribute), true);

                            if (arr == null || arr.Length == 0)
                                continue;

                            keepAlive = true;
                        }
                    // other stuff here (using else-ifs!)
                    else
                        keepAlive = true;
                }

                if (!keepAlive)
                    pool.RemoveAt(i--);
            }

            return pool;
        }
    }
}
