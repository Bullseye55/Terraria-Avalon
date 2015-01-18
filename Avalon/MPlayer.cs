using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;

namespace Avalon
{
    public override void OnFishSelected(Item fishingRod, Item bait, int liquidType, int poolCount, int worldLayer, int questFish, ref int caughtType)
    {
        if (Main.player[Main.myPlayer].zone["Corruption"] && Main.rand.Next(0, (int)(Main.player[Main.myPlayer].crateChance * 100)) == 0)
        {
            caughtType = ItemDef.byName["Corrupt Crate"].type;
        }
        else if (Main.player[Main.myPlayer].zone["Crimson"] && Main.rand.Next(0, (int)(Main.player[Main.myPlayer].crateChance * 100)) == 0)
        {
            caughtType = ItemDef.byName["Flesh Crate"].type;
        }
    }
}
