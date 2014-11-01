using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.Content;
using Avalon.API.Events;
using Avalon.API.Items.MysticalTomes;
using Avalon.API.World;
using Avalon.NPCs;
using Avalon.UI.Menus;
using Avalon.World;
using Avalon.API.Audio;

namespace Avalon
{
    /// <summary>
    /// The entry point of the Avalon mod.
    /// </summary>
    /// <remarks>Like 'Program' but for a mod</remarks>
    public sealed class AvalonMod : ModBase
    {
        /// <summary>
        /// Gets the singleton instance of the mod's <see cref="ModBase" />.
        /// </summary>
        public static AvalonMod Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// The amount of extra accessory slots.
        /// </summary>
        public const int ExtraSlots = 3;

		Option
			tomeSkillHotkey   ,
			shadowMirrorHotkey;

		bool addedWings = false;

		internal static List<BossSpawn> spawns = new List<BossSpawn>();
        internal readonly static List<int> EmptyIntList = new List<int>(); // only alloc once

        /// <summary>
        /// Gets or sets the Mystical Tomes skill hotkey.
        /// </summary>
        public static Keys TomeSkillHotkey
        {
			get
			{
				return (Keys)Instance.tomeSkillHotkey.Value;
			}
			set
			{
				Instance.tomeSkillHotkey.Value = value;
			}
		}
		/// <summary>
		/// Gets or sets the <!--<see cref="ShadowMirror" />--> hotkey.
		/// </summary>
		public static Keys ShadowMirrorHotkey
		{
			get
			{
				return (Keys)Instance.shadowMirrorHotkey.Value;
			}
			set
			{
				Instance.shadowMirrorHotkey.Value = value;
			}
		}

		/// <summary>
		/// Gets the Wraiths <see cref="Invasion" /> instance.
		/// </summary>
		public static Invasion Wraiths
        {
            get;
            internal set;
        }
        /// <summary>
        /// Gets whether the game is in superhardmode or not.
        /// </summary>
        public static bool IsInSuperHardmode
        {
            get;
            internal set;
        }
        /// <summary>
        /// Gets the Dark Matter <see cref="Biome" /> instance.
        /// </summary>
        public static Biome DarkMatter
        {
            get;
            private set;
        }

		/// <summary>
		/// Gets the ID of the Golden Wings texture.
		/// </summary>
		public static int GoldenWings
		{
			get;
			private set;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="AvalonMod" /> class.
		/// </summary>
		/// <remarks>Called by the mod loader.</remarks>
		public AvalonMod()
            : base()
        {
            Instance = this;
        }

        /// <summary>
        /// Called when the mod is loaded.
        /// </summary>
        public override void OnLoad()
        {
            Invasion .LoadVanilla();
            DateEvent.LoadVanilla();

            LoadBiomes   ();
            LoadInvasions();
            LoadSpawns   ();

			base.OnLoad();

			tomeSkillHotkey    = options[0];
			shadowMirrorHotkey = options[1];

			MWorld.managers    = new SkillManager[1]  ;
			MWorld.tomes       = new Item        [1]  ;
			MWorld.accessories = new Item        [1][];

			MWorld.tomes   [0] = new Item();

			for (int i = 0; i < MWorld.accessories.Length; i++)
			{
				MWorld.accessories[i] = new Item[ExtraSlots];

				for (int j = 0; j < MWorld.accessories[i].Length; j++)
					MWorld.accessories[i][j] = new Item();
			}

			// insert all audio/graphical/UI-related stuff AFTER this check!
			if (Main.dedServ)
				return;

			Texture2D gWings = textures["Resources/Wings/Golden Wings"];
			foreach (Texture2D t in Main.wingsTexture.Values)
				if (gWings == t)
				{
					addedWings = true;
					break;
				}

			if (!addedWings)
			{
				GoldenWings = Main.dedServ ? Main.wingsTexture.Count : ObjectLoader.AddWingsToGame(gWings);

				addedWings = true;
            }

            VorbisPlayer.LoadTrack("Resources/Music/Dark Matter (temp).ogg", this);

            StarterSetSelectionHandler.Init();
		}
        /// <summary>
        /// Called when all mods are loaded.
        /// </summary>
        [CallPriority(Single.PositiveInfinity)]
        public override void OnAllModsLoaded()
        {
            base.OnAllModsLoaded();

            VanillaDrop.InitDrops();

            // insert all audio/graphical/UI-related stuff AFTER this check!
            if (Main.dedServ)
                return;
        }
        /// <summary>
        /// Called when the mod is unloaded.
        /// </summary>
        public override void OnUnload()
        {
            Instance = null;

            Wraiths    = null;
            DarkMatter = null;
            IsInSuperHardmode = false;

            GoldenWings = 0;

            spawns.Clear();

            Invasion.invasions    .Clear();
            Invasion.invasionTypes.Clear();

            DateEvent.events.Clear();

            // insert all audio/graphical/UI-related stuff AFTER this check!
            if (Main.dedServ)
            {
                base.OnUnload();

                VorbisPlayer.Music.music = null;

                VorbisPlayer.cache    .Clear();
                VorbisPlayer.instCache.Clear();

                return;
            }

            VorbisPlayer.Music.StopOgg(true);

            foreach (OggVorbis track in VorbisPlayer.cache.Values)
                if (!track.IsDisposed)
                    track.Dispose();

            foreach (SoundEffectInstance inst in VorbisPlayer.instCache.Values)
            {
                if (inst.IsDisposed)
                    continue;

                if (inst.State != SoundState.Stopped)
                    inst.Stop();

                inst.Dispose();
            }

            VorbisPlayer.cache    .Clear();
            VorbisPlayer.instCache.Clear();

            base.OnUnload();
        }

        /// <summary>
        /// Chooses the music track to play.
        /// </summary>
        /// <param name="current">The music track to play.</param>
        [CallPriority(Single.NegativeInfinity)]
        public override void ChooseTrack(ref string current)
        {
            // not ran on dedServ

            base.ChooseTrack(ref current);

            if (Main.gameMenu && Menu.currentPage == "Main Menu" && Menu.currentPage != "Crash Page") // temp, if not obvious enough
                current = "Avalon:Resources/Music/Dark Matter (temp).ogg";

            VorbisPlayer.Update(ref current);
        }

        /// <summary>
        /// Called at the end of <see cref="Main" />.Update.
        /// </summary>
        public override void PostGameUpdate()
        {
            if (!Main.hasFocus)
                VorbisPlayer.Music.UpdateInactive();

            base.PostGameUpdate();
        }

        /// <summary>
        /// Called when an option is changed.
        /// </summary>
        /// <param name="option">The option that has changed.</param>
        public override void OptionChanged(Option option)
        {
            base.OptionChanged(option);

            switch (option.name)
            {
                case "TomeSkillHotkey"   :
					if (tomeSkillHotkey != option)
						tomeSkillHotkey  = option;
					break;
				case "ShadowMirrorHotkey":
					if (shadowMirrorHotkey != option)
						shadowMirrorHotkey  = option;
					break;
            }
        }

        static void LoadBiomes   ()
        {
            #region edit vanilla
            //Biome.Biomes["Dungeon"].typesIncrease.Add(TileDef.type["Avalon:Dungeon Orange Brick"]);

            //Biome.Biomes["Ocean"].typesIncrease.Add(TileDef.type["Avalon:Black Sand"]);
            ////Biome.Biomes["Ocean"].TileValid = /* set! */ (x, y, pid) =>
            ////{
            ////    return NPC.areaWater && y < Main.rockLayer && (x < 250 || x > Main.maxTilesX - 250)
            ////        && Biome.Biomes["Ocean"].typesIncrease.Contains(Main.tile[x, y].type);
            ////};

            Biome.Biomes["Overworld"].TileValid += /* add! */ (x, y, pid) =>
            {
                Player p = Main.player[pid];

                return /*!p.zone["Avalon:Tropics"] && !p.zone["Avalon:Comet"] && !p.zone["Avalon:Ice Cave"]
                    && !p.zone["Avalon:Hellcastle"] && !p.zone["Avalon:Sky Fortress"]*/ !p.zone["Avalon:Dark Matter"];
            };
            #endregion

            #region custom ones
            (DarkMatter = new DarkMatter()).AddToGame();

            //new Biome("Avalon:Comet", new List<int> { TileDef.type["Avalon:Ever Ice"] }, EmptyIntList, 50).AddToGame();

            //new Biome("Avalon:Tropics", new List<int>
            //{
            //    TileDef.type["Avalon:Black Sand"], TileDef.type["Avalon:Tropical Mud"], TileDef.type["Avalon:Tropical Grass"], TileDef.type["Avalon:Tropic Stone"]
            //}, new List<int> { }, 80).AddToGame();

            //new Biome("Avalon:Ice Cave", new List<int> { TileDef.type["Avalon:Ice Block"] }, EmptyIntList, 50).AddToGame();

            //new Biome("Avalon:Hellcastle", new List<int> { TileDef.type["Avalon:Impervious Brick"], TileDef.type["Avalon:Resistant Wood"] }, EmptyIntList, 100)
            //{
            //    TileValid = (x, y, pid) => y < Main.maxTilesX - 200 && Biome.Biomes["Avalon:Hellcastle"].typesIncrease.Contains(Main.tile[x, y].type)
            //}.AddToGame();

            //new Biome("Avalon:Sky Fortress", new List<int> { TileDef.type["Avalon:Reinforced Glass"], TileDef.type["Avalon:Hallowstone Block"] }, EmptyIntList, 100)
            //{
            //    TileValid = (x, y, pid) => y < 200 && Biome.Biomes["Avalon:Sky Fortress"].typesIncrease.Contains(Main.tile[x, y].type)
            //}.AddToGame();

            //new Biome("Avalon:Clouds", new List<int> { TileDef.type["Avalon:Cloud"] }, EmptyIntList, 35)
            //{
            //    TileValid = (x, y, pid) => y < 200 && Biome.Biomes["Avalon:Clouds"].typesIncrease.Contains(Main.tile[x, y].type)
            //}.AddToGame();
            #endregion
        }
        static void LoadInvasions()
        {
            Invasion.Load(Instance, "Wraiths", Wraiths = new WraithInvasion());
        }
        static void LoadSpawns   ()
        {
            RegisterBossSpawn(new BossSpawn()
            {
                CanSpawn = (r, p) => !Main.dayTime && Main.rand.Next(30000 * r / 5) == 0,
                Type = 4 // Eye of Ctulhu
            });
            RegisterBossSpawn(new BossSpawn()
            {
                CanSpawn = (r, p) => !Main.dayTime && Main.rand.Next(36000 * r / 5) == 0,
                Type = 134 // probably The Destroyer
            });
            RegisterBossSpawn(new BossSpawn()
            {
                CanSpawn = (r, p) => !Main.dayTime && Main.rand.Next(40000 * r / 5) == 0,
                Type = 127 // probably The Twins or Skeletron Prime
            });

            //RegisterBossSpawn(new BossSpawn()
            //{
            //    CanSpawn = (r, p) => Main.dayTime && Main.sandTiles >= 100 && Main.rand.Next(30000 * r / 5) == 0,
            //    NpcInternalName = "Avalon:Desert Beak"
            //});
            //RegisterBossSpawn(new BossSpawn()
            //{
            //    CanSpawn = (r, p) => Main.dayTime && p.zoneJungle          && Main.rand.Next(30000 * r / 5) == 0,
            //    NpcInternalName = "Avalon:King Sting"
            //});
            //RegisterBossSpawn(new BossSpawn()
            //{
            //    CanSpawn = (r, p) => Main.dayTime && p.zoneHoly            && Main.rand.Next(42000 * r / 5) == 0,
            //    NpcInternalName = "Avalon:Cataryst"
            //});

            RegisterBossSpawn(new WraithSpawn());
        }

        /// <summary>
        /// Registers a BossSpawn.
        /// </summary>
        /// <param name="bs">The BossSpawn to register.</param>
        public static void RegisterBossSpawn(BossSpawn bs)
        {
            spawns.Add(bs);
        }
    }
}
