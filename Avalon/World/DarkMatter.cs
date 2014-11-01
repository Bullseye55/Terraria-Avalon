using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.World
{
    sealed class DarkMatter : Biome
    {
        internal DarkMatter()
            : base("Avalon:Dark Matter", new List<int>()
            {
                TileDef.byName["Avalon:Dark Matter Ooze"]
            }, AvalonMod.EmptyIntList, 150 /* temp value */)
        {
            biomeMusic = "Avalon:Resources/Music/Dark Matter (temp).ogg";
            biomeMusicPriority = MusicPriority.Med;
        }
    }
}
