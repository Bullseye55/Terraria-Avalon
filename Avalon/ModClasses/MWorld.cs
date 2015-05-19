using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;
using Avalon.API.Items.MysticalTomes;
using Avalon.UI;
using Avalon.World;

namespace Avalon.ModClasses
{
    /// <summary>
    /// The <see cref="ModWorld" /> class of the Avalon mod.
    /// </summary>
    public sealed class MWorld : ModWorld
    {
        const int spawnSpaceX = 3, spawnSpaceY = 3;

#pragma warning disable 414
		static bool needsToLoadRecipes = true, scanned = false;	// no idea
#pragma warning restore 414

		internal static bool oldNight = false;

        TimeSpan oldTimeOfDay;

        /// <summary>
        /// Wether UltraOblivion has already been killed or not.
        /// </summary>
        public static bool UltraOblivionDowned = false;
        /// <summary>
        /// Wether Berserker Ore is already generated in the world.
        /// </summary>
        public static bool SpawnedBerserkerOre = false;

        /// <summary>
        /// How many times Cataryst has been killed.
        /// </summary>
        public static int CatarystDownedCount = 0;
        /// <summary>
        /// How many times the Armageddon Slime has been defeated.
        /// </summary>
        public static int ArmageddonCount = 0;
        /// <summary>
        /// How many times something something something.
        /// </summary>
        public static int EverIceCount = 0;
        /// <summary>
        /// How many times a hallowed altar was broken.
        /// </summary>
        public static int HallowAltarsBroken = 0;
        /// <summary>
        /// How many times a wraith in the Wraith Invasion was killed.
        /// </summary>
        public static int WraithsDowned = 0;
        /// <summary>
        /// The spread ratio of the wastelands.
        /// </summary>
        /// <remarks>(s^-1)/Tile</remarks>
        public static int WastelandsSpreadRatio = -1;

#pragma warning disable 414
        static int grassCounter = 0, jungleEx = 0, gCount = 0;
#pragma warning restore 414
		static int jungleX = -1, jungleY = -1, lOceanY = -1, rOceanY = -1, hellY;

        /// <summary>
        /// Gets the position of the Jungle (in tile coordinates).
        /// </summary>
        public static Point JunglePosition
        {
            get
            {
                if (jungleX == -1 || jungleY == -1)
                    CorrectJunglePos();

                return new Point(jungleX, jungleY);
            }
        }
        /// <summary>
        /// Gets the position of the left Ocean (in tile coordinates).
        /// </summary>
        public static Point LeftOceanPosition
        {
            get
            {
                if (lOceanY == -1)
                    CorrectOceanPos();

                return new Point(250, lOceanY);
            }
        }
        /// <summary>
        /// Gets the position of the right Ocean (in tile coordinates).
        /// </summary>
        public static Point RightOceanPosition
        {
            get
            {
                if (rOceanY == -1)
                    CorrectOceanPos();

                return new Point(Main.maxTilesX - 250, rOceanY);
            }
        }
        /// <summary>
        /// Gets the position of the Underworld (in tile coordinates).
        /// </summary>
        public static Point UnderworldPosition
        {
            get
            {
                if (hellY == -1)
                    CorrectHellPos();

                return new Point(Main.maxTilesX / 2, hellY);
            }
        }

        /// <summary>
        /// Gets the rectangle for the Tropics biome.
        /// </summary>
        public static Rectangle TropicsRect
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the layer containing the extra accessory slots.
        /// </summary>
        public static AccessorySlotLayer AccessoryLayer
        {
            get;
            internal set;
        }
        /// <summary>
        /// Gets the layer containing the Mystical Tome slot.
        /// </summary>
        public static TomeSlotLayer      TomeSlotLayer
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the amount of Dark Matter tiles that have spreaded this (Terrarian) day.
        /// </summary>
        public static int DarkMatterSpreaded
        {
            get;
            internal set;
        }

        internal static SkillManager[/* player ID */] managers;
        internal static SkillManager localManager
        {
            get
            {
                if (managers[Main.myPlayer] == null)
                    managers[Main.myPlayer] = SkillManager.FromItem(tomes[Main.myPlayer]);

                return managers[Main.myPlayer];
            }
            set
            {
                managers[Main.myPlayer] = value;
            }
        }

        internal static Item[/* player ID */][/* item index */] accessories;
        internal static Item                 [/* item index */] localAccessories
        {
            get
            {
                return accessories[Main.myPlayer];
            }
            set
            {
                accessories[Main.myPlayer] = value;
            }
        }

        internal static Item[/* player id*/] tomes;
        internal static Item localTome
        {
            get
            {
                return tomes[Main.myPlayer];
            }
            set
            {
                tomes[Main.myPlayer] = value;
                localManager = SkillManager.FromItem(value);
            }
        }

        static MWorld()
        {
            // values must have default values, Player.DeepClone calls MPlayer.Save that accesses the MWorlds fields, which are null when Main.gameMenu is true.
            Init();
        }

        /// <summary>
        /// Corrects a Y position so the tile at the given position is inactive.
        /// </summary>
        /// <param name="x">The X coördinate.</param>
        /// <param name="y">The Y coördinate.</param>
        public static void CorrectY(int x, ref int y)
        {
            if (Main.tile[x, y].active())
                do y--; while (Main.tile[x, y].active());
            else
                while (!Main.tile[x, y].active())
                    y++;

            y -= 3; // make sure it won't be IN the ground
        }

        static void CorrectJunglePos()
        {
            if (jungleX == -1 || jungleY == -1)
            {
                jungleX = Main.maxTilesX - Main.dungeonX;
                jungleY = Main.dungeonY;

                CorrectY(jungleX, ref jungleY);
            }
        }
        static void CorrectOceanPos ()
        {
            if (lOceanY == -1)
            {
                lOceanY = (int)Main.worldSurface - 25;

                CorrectY(250, ref lOceanY);
            }

            if (rOceanY == -1)
            {
                rOceanY = (int)Main.worldSurface - 25;

                CorrectY(Main.maxTilesX - 250, ref rOceanY);
            }
        }
        static void CorrectHellPos  ()
        {
            if (hellY == -1)
            {
                hellY = Main.maxTilesY - 150;

                CorrectY(Main.maxTilesX / 2, ref hellY);
            }
        }

        /// <summary>
        /// Called when the world is opened on this client or server
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            Init();
        }

        internal static void Init()
        {
            CatarystDownedCount = EverIceCount = ArmageddonCount = HallowAltarsBroken = 0;
            AvalonMod.IsInSuperHardmode = UltraOblivionDowned = SpawnedBerserkerOre = false;

            int plrs = Main.netMode == 0 ? 1 : Main.numPlayers;

            Array.Resize(ref managers   , plrs);
            Array.Resize(ref accessories, plrs);
            Array.Resize(ref tomes      , plrs);

            for (int i = 1; i < accessories.Length; i++)
            {
                accessories[i] = new Item[AvalonMod.ExtraSlots];

                for (int j = 0; j < accessories[i].Length; j++)
                    accessories[i][j] = new Item();
            }
            for (int i = 1; i < tomes.Length; i++)
                tomes[i] = new Item();

            // insert all graphical/UI-related stuff AFTER this check!
            if (Main.dedServ)
                return;
        }

        /// <summary>
        /// Called after the world is updated
        /// </summary>
        public override void PostUpdate()
        {
            base.PostUpdate();

            if ((!Main.dayTime && PoroCYon.MCT.World.TimeAsTimeSpan < oldTimeOfDay) || WastelandsSpreadRatio <= -1)
                WastelandsSpreadRatio = Main.rand.Next(6000, 14000);

            oldTimeOfDay = PoroCYon.MCT.World.TimeAsTimeSpan;

            //if (Main.dayTime && Main.time == 0d && DarkMatterSpreaded == 0)
            //{
            //    // spread somehow (need more info on this - put somewhere randomly a DM tile, or add a tile somewhere to an existing DM biome?)
            //}
        }

        /// <summary>
        /// Writes binary data to a world save file. Called when the world is closed.
        /// </summary>
        /// <param name="bb">The buffer containing the binary data.</param>
        public override void Save(BinBuffer bb)
        {
			// kinda hacky: use this as an OnQuit hook
			if (Main.gameMenu) // if ingame, it's a backup save (while playing), not when quitting
			{
				Array.Resize(ref managers   , 1);
				Array.Resize(ref accessories, 1);
				Array.Resize(ref tomes      , 1);
			}

            if (!Main.dedServ)
                Main.sunTexture = AvalonMod.sunBak;

            for (int i = 0; i < Main.backgroundTexture.Length; i++)
                Main.backgroundTexture[i] = AvalonMod.bgBak[i];

            Main.backgroundWidth [0] = AvalonMod.bg0SzBak.X;
            Main.backgroundHeight[0] = AvalonMod.bg0SzBak.Y;

            base.Save(bb);

            bb.WriteX(AvalonMod.IsInSuperHardmode, scanned);

            bb.WriteX(WraithsDowned);

            CorrectJunglePos();
            CorrectOceanPos ();
            CorrectHellPos  ();

            bb.WriteX(jungleX, jungleY, lOceanY, rOceanY, hellY);

            bb.Write(DarkMatterSpreaded);
        }
        /// <summary>
        /// Loads binary data from a world save file. Called when the world is loaded.
        /// </summary>
        /// <param name="bb">The buffer containing the binary data.</param>
        public override void Load(BinBuffer bb)
        {
            base.Load(bb);

            accessories = new Item[Main.netMode == 0 ? 1 : Main.numPlayers][];

            for (int i = 0; i < accessories.Length; i++)
            {
                accessories[i] = new Item[AvalonMod.ExtraSlots];

                for (int j = 0; j < AvalonMod.ExtraSlots; j++)
                    accessories[i][j] = new Item();
            }

            AvalonMod.IsInSuperHardmode = bb.ReadBool();
            scanned = bb.ReadBool();

            WraithsDowned = bb.ReadInt();

            jungleX = bb.ReadInt();
            jungleY = bb.ReadInt();

            lOceanY = bb.ReadInt();
            rOceanY = bb.ReadInt();

            hellY = bb.ReadInt();

            CorrectJunglePos();
            CorrectOceanPos ();
            CorrectHellPos  ();

            DarkMatterSpreaded = bb.ReadInt();
        }

        /// <summary>
        /// Called after the world is generated
        /// </summary>
        public override void WorldGenPostInit()
        {
            base.WorldGenPostInit();


        }
        /// <summary>
        /// Used to modify the worldgen task list (insert additional tasks).
        /// </summary>
        /// <param name="list">The task list to modify</param>
        public override void WorldGenModifyTaskList(List<WorldGenTask> list)
        {
            base.WorldGenModifyTaskList(list);

            // ...?
        }
        /// <summary>
        /// Used to modify the hardmode task list (when the WoF is defeated).
        /// </summary>
        /// <param name="list">The task list to modify</param>
        public override void WorldGenModifyHardmodeTaskList(List<WorldGenTask> list)
        {
            base.WorldGenModifyHardmodeTaskList(list);

            list.Add(new DynamicTask("Avalon:Heartstone", Gen.GenerateHeartstone));
        }

        /// <summary>
        /// Gets wether a boss is active or not.
        /// </summary>
        /// <returns>true if a boss is active, false otherwise.</returns>
        public static bool CheckForBosses()
        {
            return Main.npc.Count(n => n.boss) > 0; // LINQ++. again.
        }

        /// <summary>
        /// Creates a 3x3 circle of tiles.
        /// </summary>
        /// <param name="x">The centre of the circle's X coordinate.</param>
        /// <param name="y">The centre of the circle's Y coordinate.</param>
        /// <param name="type">The type of the tiles.</param>
        public static void Make3x3Circle(int x, int y, ushort type)
        {
            for (int xoff = x - 1; xoff <= x + 1; xoff++)
                for (int yoff = y - 1; yoff <= y + 1; yoff++)
                {
                    Main.tile[xoff, yoff].active(true);
                    Main.tile[xoff, yoff].type = type;

                    WorldGen.SquareTileFrame(xoff, yoff);
                }

            Main.tile[x - 2, y].active(true);
            Main.tile[x + 2, y].active(true);
            Main.tile[x, y - 2].active(true);
            Main.tile[x, y + 2].active(true);

            Main.tile[x - 2, y].type
                = Main.tile[x + 2, y].type
                = Main.tile[x, y - 2].type
                = Main.tile[x, y + 2].type
                    = type;

            WorldGen.SquareTileFrame(x - 2, y);
            WorldGen.SquareTileFrame(x + 2, y);
            WorldGen.SquareTileFrame(x, y - 2);
            WorldGen.SquareTileFrame(x, y + 2);
        }

        internal static void SmashHallowAltar(int x, int y)
        {

        }
    }
}
