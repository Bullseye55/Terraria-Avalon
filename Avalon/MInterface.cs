using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;

namespace Avalon
{
    public class MInterface : ModInterface
    {
        public override bool PreItemSlotRightClick(TAPI.UIKit.ItemSlot slot, ref bool release)
        {
            if (release)
            {
                if (slot.MyItem.type == ItemDef.byName["Flesh Crate"].type)
                {
                    if (Main.hardMode && Main.rand.Next(0, 250) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Corruption Key Mold"].type, 1);
                    }
                    else if (Main.rand.Next(0, 30) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Worm Food"].type, 1);
                    }
                    else if (Main.rand.Next(0, 15) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Demonite Ore"].type, Main.rand.Next(20, 36));
                    }
                    else if (NPC.downedBoss2 && Main.rand.Next(0, 13) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Shadow Scale"].type, Main.rand.Next(10, 16));
                    }
                    else if (NPC.downedBoss2 && Main.rand.Next(0, 10) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Ebonstone Block"].type, Main.rand.Next(20, 41));
                    }
                    else if (Main.hardMode && Main.rand.Next(0, 9) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Soul of Night"].type, Main.rand.Next(3, 7));
                    }
                    else if (Main.rand.Next(0, 6) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Deathweed"].type, Main.rand.Next(3, 7));
                    }
                    else if (Main.rand.Next(0, 9) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Rotten Eye"].type, Main.rand.Next(1, 3));
                    }
                    else
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Rotten Chunk"].type, Main.rand.Next(2, 6));
                    }
                    if (slot.MyItem.stack > 1)
                    {
                        slot.MyItem.stack--;
                    }
                    else
                    {
                        slot.MyItem = ItemDef.byType[0];
                    }
                    return false;
                }
                else if (slot.MyItem.type == ItemDef.byName["Corrupt Crate"].type)
                {
                    if (Main.hardMode && Main.rand.Next(0, 250) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Crimson Key Mold"].type, 1);
                    }
                    else if (Main.rand.Next(0, 30) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Bloody Spine"].type, 1);
                    }
                    else if (Main.rand.Next(0, 15) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Crimtane Ore"].type, Main.rand.Next(20, 36));
                    }
                    else if (NPC.downedBoss2 && Main.rand.Next(0, 13) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Tissue Sample"].type, Main.rand.Next(10, 16));
                    }
                    else if (NPC.downedBoss2 && Main.rand.Next(0, 10) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Crimstone Block"].type, Main.rand.Next(20, 41));
                    }
                    else if (Main.hardMode && Main.rand.Next(0, 9) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Soul of Night"].type, Main.rand.Next(3, 7));
                    }
                    else if (Main.rand.Next(0, 6) == 0)
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Bloodberry"].type, Main.rand.Next(3, 8));
                    }
                    else
                    {
                        Item.NewItem(Main.player[Main.myPlayer].position, Vector2.Zero, ItemDef.byName["Vanilla:Vertebrae"].type, Main.rand.Next(2, 6));
                    }
                    if (slot.MyItem.stack > 1)
                    {
                        slot.MyItem.stack--;
                    }
                    else
                    {
                        slot.MyItem = ItemDef.byType[0];
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
