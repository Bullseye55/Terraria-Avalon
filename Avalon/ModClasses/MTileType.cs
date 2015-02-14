using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using TAPI;

namespace Avalon.ModClasses
{
    /// <summary>
    /// The global <see cref="ModTileType" /> class of the Avalon mod.
    /// </summary>
    [GlobalMod]
    public sealed class MTileType : ModTileType
    {
        static Player GetMostNearby(Vector2 pos)
        {
            if (Main.netMode == 0)
                return Main.localPlayer;

            int   closest      = -1;
            float curClosestSq = Single.PositiveInfinity;

            for (int i = 0; i < PoroCYon.MCT.World.NumPlayers; i++)
            {
                float dist = Vector2.DistanceSquared(pos, Main.player[i].Centre);

                if (dist < curClosestSq)
                {
                    curClosestSq = dist;
                    closest      = i;
                }
            }

            return closest == -1 ? Main.localPlayer /* you'll never know! */ : Main.player[closest];
        }

        static List<int> slingshotTypes = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fail"></param>
        /// <param name="effectsOnly"></param>
        /// <param name="noItem"></param>
        public override void Kill(int x, int y, bool fail, bool effectsOnly, bool noItem)
        {
            return;

            if (slingshotTypes == null)
                slingshotTypes = new List<int>()
                {
                    ItemDef.byName["Avalon:Slingshot"              ].type,
                    ItemDef.byName["Avalon:Ebonwood Slingshot"     ].type,
                    ItemDef.byName["Avalon:Pearlwood Slingshot"    ].type,
                    ItemDef.byName["Avalon:Shadewood Slingshot"    ].type,
                    ItemDef.byName["Avalon:Rich Mahogany Slingshot"].type
                };

            base.Kill(x, y, fail, effectsOnly, noItem);

            Vector2 tp = new Vector2(x, y) * 16f;

            if (!noItem && GetMostNearby(tp).inventory.Any(i => slingshotTypes.Any(t => t == i.type)))
                Item.NewItem(tp, Vector2.Zero, ItemID.Seed, Main.rand.Next(1, 6), noGrabDelay: true);
        }

        public override bool PreKill(int x, int y, bool fail, bool effectsOnly, bool noItem)
        {
            if (WorldGen.destroyObject)
                return true;
            //If the we are destroying the tile and it's one of our custom chests
            if (!fail && Main.tile[x, y].type == 21 && Main.tile[x, y].frameX >= 1728)
            {
                int chestId = Main.tile[x, y].frameX % 1728 / 36 + 48;

                int k = 0;
                k += (int)(Main.tile[x, y].frameX / 18);
                int num = y + (int)(Main.tile[x, y].frameY / 18 * -1);
                while (k > 1)
                {
                    k -= 2;
                }
                k *= -1;
                k += x;

                if (Main.chest[Chest.FindChest(k, num)].item.ToList().Where(i => i.type > 0).Count() == 0)
                    Item.NewItem(new Vector2(x, y) * 16, Vector2.Zero, Chest.itemSpawn[chestId]);

                WorldGen.destroyObject = true;

                Chest.DestroyChest(k, num);
                WorldGen.KillTile(k, num);
                WorldGen.KillTile(k + 1, num);
                WorldGen.KillTile(k, num + 1);
                WorldGen.KillTile(k + 1, num + 1);

                WorldGen.destroyObject = false;

                return false;
            }
            return true;
        }
    }
}
