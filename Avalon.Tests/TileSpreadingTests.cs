using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalon.API.Biomes;
using Avalon.Tiles.DarkMatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using TAPI;
using Terraria;

namespace Avalon.Tests
{
    [TestClass]
    public class TileSpreadingTests
    {
        enum TestedTileType : ushort
        {
            Dirt,
            Stone,
            DarkMatter,
            // insert other type mocks here

            Count
        }

        const int
            TEST_WLD_SIZE = 3;

        static bool loaded = false;
        static string debugWorldStringVisualization;

        static void FillDefs()
        {
            TileDef.Setup();

            typeof(Main).GetMethod("Initialize_TileDefStuff", BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Static).Invoke(null, null);
        }
        static void Load()
        {
            if (loaded)
                return;

            loaded = true;

            FillDefs();

            TileDef.byName.Add("Vanilla:Dirt"           , (ushort)TestedTileType.Dirt      );
            TileDef.byName.Add("Dirt"                   , (ushort)TestedTileType.Dirt      );
            TileDef.byName.Add("Avalon:Dark Matter Ooze", (ushort)TestedTileType.DarkMatter);

            // ...
        }
        static void SetupWld(TestedTileType spreading, TestedTileType testOn)
        {
            Main.tile = new Tile[TEST_WLD_SIZE, TEST_WLD_SIZE];

            for (int i = 0; i < TEST_WLD_SIZE; i++)
                for (int j = 0; j < TEST_WLD_SIZE; j++)
                {
                    Main.tile[i, j] = new Tile()
                    {
                        type = (ushort)(i == 1 && j == 1 ? spreading : testOn)
                    };

                    Main.tile[i, j].active(i != 0);
                }

            // other stuff

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < TEST_WLD_SIZE; i++)
            {
                for (int j = 0; j < TEST_WLD_SIZE; j++)
                {
                    if (Main.tile[i, j].active())
                        sb.Append(Main.tile[i, j].type);
                    else
                        sb.Append('O');
                }

                sb.Append(Environment.NewLine);
            }

            Console.WriteLine(debugWorldStringVisualization = sb.ToString());
        }
        static void TestSpreadingTile<TSpreadingTile>(Func<TSpreadingTile> ctor)
            where TSpreadingTile : SpreadingTile
        {
            TSpreadingTile tst = ctor();

            Assert.IsFalse(tst.CanSpreadOn(new Point(0, 1)));
            Assert.IsTrue (tst.CanSpreadOn(new Point(2, 1)));
        }

        [TestMethod]
        public void TestDMOnDirt ()
        {
            Load();
            SetupWld(TestedTileType.DarkMatter, TestedTileType.Dirt);

            TestSpreadingTile(() => new DarkMatterOoze());
        }
        [TestMethod]
        public void TestDMOnStone()
        {
            Load();
            SetupWld(TestedTileType.DarkMatter, TestedTileType.Stone);

            TestSpreadingTile(() => new DarkMatterOoze());
        }
    }
}
