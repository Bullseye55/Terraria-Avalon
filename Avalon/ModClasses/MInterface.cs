using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;
using TAPI.UIKit;

namespace Avalon
{
    /// <summary>
    ///
    /// </summary>
    public class MInterface : ModInterface
    {
        static void DecreaseStack(ItemSlot slot)
        {
            if (slot.MyItem.stack > 1)
                slot.MyItem.stack--;
            else
                slot.MyItem.SetDefaults(0);
        }

        static int SpawnAtPlayer(string itemName, int stack)
        {
            return SpawnAtPlayer(ItemDef.byName[itemName].type, stack);
        }
        static int SpawnAtPlayer(int    itemType, int stack)
        {
            return Item.NewItem(Main.localPlayer.Centre, Vector2.Zero, itemType, stack);
        }

        static void DropCorruptCrate(ItemSlot slot)
        {
            if (Main.hardMode && Main.rand.Next(0, 250) == 0)
                SpawnAtPlayer("Vanilla:Corruption Key Mold", 1);
            else if (Main.rand.Next(0, 30) == 0)
                SpawnAtPlayer("Vanilla:Worm Food", 1);
            else if (Main.rand.Next(0, 15) == 0)
                SpawnAtPlayer("Vanilla:Demonite Ore", Main.rand.Next(20, 36));
            else if (NPC.downedBoss2 && Main.rand.Next(0, 13) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Shadow Scale"].type, Main.rand.Next(10, 16));
            else if (NPC.downedBoss2 && Main.rand.Next(0, 10) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Ebonstone Block"].type, Main.rand.Next(20, 41));
            else if (Main.hardMode && Main.rand.Next(0, 9) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Soul of Night"].type, Main.rand.Next(3, 7));
            else if (Main.rand.Next(0, 6) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Deathweed"].type, Main.rand.Next(3, 7));
            // Uncomment this when the item is implemented
            //else if (Main.rand.Next(0, 9) == 0)
            //    SpawnAtPlayer(ItemDef.byName["Avalon:Rotten Eye"].type, Main.rand.Next(1, 3));
            else
                SpawnAtPlayer(ItemDef.byName["Vanilla:Rotten Chunk"].type, Main.rand.Next(2, 6));

            DecreaseStack(slot);
        }
        static void DropCrimsonCrate(ItemSlot slot)
        {
            if (Main.hardMode && Main.rand.Next(0, 250) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Crimson Key Mold"].type, 1);
            else if (Main.rand.Next(0, 30) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Bloody Spine"].type, 1);
            else if (Main.rand.Next(0, 15) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Crimtane Ore"].type, Main.rand.Next(20, 36));
            else if (NPC.downedBoss2 && Main.rand.Next(0, 13) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Tissue Sample"].type, Main.rand.Next(10, 16));
            else if (NPC.downedBoss2 && Main.rand.Next(0, 10) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Crimstone Block"].type, Main.rand.Next(20, 41));
            else if (Main.hardMode && Main.rand.Next(0, 9) == 0)
                SpawnAtPlayer(ItemDef.byName["Vanilla:Soul of Night"].type, Main.rand.Next(3, 7));
            // Uncomment this when the item is implemented
            //else if (Main.rand.Next(0, 6) == 0)
            //    SpawnAtPlayer(ItemDef.byName["Avalon:Bloodberry"].type, Main.rand.Next(3, 8));
            else
                SpawnAtPlayer(ItemDef.byName["Vanilla:Vertebrae"].type, Main.rand.Next(2, 6));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="release"></param>
        /// <returns></returns>
        public override bool PreItemSlotRightClick(ItemSlot slot, ref bool release)
        {
            if (slot.MyItem.type != ItemDef.byName["Avalon:Flesh Crate"].type && slot.MyItem.type != ItemDef.byName["Avalon:Corrupt Crate"].type)
                return true;

            if (!release)
                return false;

                 if (slot.MyItem.type == ItemDef.byName["Avalon:Corrupt Crate"].type)
                DropCorruptCrate(slot);
            else if (slot.MyItem.type == ItemDef.byName["Avalon:Flesh Crate"  ].type)
                DropCrimsonCrate(slot);

            return false;
        }
    }
}
