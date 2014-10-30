using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using Avalon.API.World;

namespace Avalon.NPCs
{
    /// <summary>
    /// Global NPC stuff
    /// </summary>
    [GlobalMod]
    public class MNPC : ModNPC
    {
        /// <summary>
        /// Updates the spawn rate for every player. Called every tick.
        /// </summary>
        /// <param name="player">The Player where the NPC(s) could spawn.</param>
        public override void UpdateSpawnRate(Player player)
        {
            base.UpdateSpawnRate(player);

            if (AvalonMod.Wraiths.IsActive)
            {
                NPC.spawnRate = 15;
                NPC.maxSpawns = 20;

                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i] == null || Main.npc[i].type <= 0 || Main.npc[i].life <= 0 || !Main.npc[i].active || String.IsNullOrEmpty(Main.npc[i].name))
                        continue;

                    if (Main.npc[i].townNPC || Main.npc[i].boss)
                        continue;

                    bool keepAlive = false;

                    object[] attr = Main.npc[i].subClass.GetType().GetCustomAttributes(typeof(WraithInvasionNPCAttribute), true);
                    for (int j = 0; j < attr.Length; j++)
                        if (attr[j] is WraithInvasionNPCAttribute)
                            keepAlive = true;

                    Main.npc[i].active &= keepAlive;
                }
            }
        }

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
    }
}
