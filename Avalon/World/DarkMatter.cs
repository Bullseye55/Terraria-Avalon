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
                TileDef.byName["Avalon:Dark Matter Soil"]
            }, AvalonMod.EmptyIntList, 150 /* temp value */)
        {
            // biomeMusic = ...?
        }
    }
}
