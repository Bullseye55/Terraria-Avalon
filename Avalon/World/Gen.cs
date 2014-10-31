using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.World
{
    /// <summary>
    /// Performs world generation actions in the Avalon mod.
    /// </summary>
    public static class Gen
    {
        ///// <summary>
        ///// Generates Heartstone ore.
        ///// </summary>
        //public static void GenerateHeartstone()
        //{
        //    for (int i = 0; i < Main.maxTilesX * Main.maxTilesY * 0.00012; i++)
        //        WorldGen.OreRunner(
        //            WorldGen.genRand.Next(100, Main.maxTilesX - 100),
        //            WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 150),
        //            WorldGen.genRand.Next(2, 4), WorldGen.genRand.Next(1, 5), TileDef.byName["Avalon:Heartstone"]);
        //}
    }
}
