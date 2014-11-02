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
using Avalon.API.Audio;
using Avalon.API.Events;
using Avalon.API.Items.MysticalTomes;
using Avalon.API.World;
using Avalon.ModClasses;
using Avalon.NPCs;
using Avalon.UI.Menus;
using Avalon.World;

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

        static Texture2D sunBak;

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
        /// Gets the texture of the sun in the <see cref="World.DarkMatter" /> biome.
        /// </summary>
        public static Texture2D DarkMatterSun
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the texture of the background in the <see cref="World.DarkMatter" /> biome.
        /// </summary>
        public static Texture2D DarkMatterBackground
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

            sunBak = Main.sunTexture;
            DarkMatterSun = textures["Resources/Dark Matter/Sun"];

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

            TileDef.tileMerge[TileDef.byName["Avalon:Dark Matter Soil"]][TileDef.byName["Avalon:Dark Matter Ooze"]] = true;
            TileDef.tileMerge[TileDef.byName["Avalon:Dark Matter Soil"]][TileDef.byName["Avalon:Dark Matter Brick"]] = true;
            TileDef.tileMerge[TileDef.byName["Avalon:Dark Matter Brick"]][TileDef.byName["Avalon:Dark Matter Ooze"]] = true;

            TileDef.tileMerge[TileDef.byName["Avalon:Dark Matter Ooze"]][TileDef.byName["Avalon:Dark Matter Soil"]] = true;
            TileDef.tileMerge[TileDef.byName["Avalon:Dark Matter Brick"]][TileDef.byName["Avalon:Dark Matter Soil"]] = true;
            TileDef.tileMerge[TileDef.byName["Avalon:Dark Matter Ooze"]][TileDef.byName["Avalon:Dark Matter Brick"]] = true;

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

            VorbisPlayer.Music.overFade.Clear();

            // insert all audio/graphical/UI-related stuff AFTER this check!
            if (Main.dedServ)
            {
                base.OnUnload();

                VorbisPlayer.cache    .Clear();
                VorbisPlayer.instCache.Clear();

                VorbisPlayer.Music.music = null;

                VorbisPlayer.Music.origFade.Clear();

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

            foreach (KeyValuePair<string, float> kvp in VorbisPlayer.Music.origFade)
                if (WavebankDef.fade.ContainsKey(kvp.Key))
                    WavebankDef.fade[kvp.Key] = kvp.Value;

            VorbisPlayer.Music.origFade.Clear();

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

            // temp, if not obvious enough
            if (Main.gameMenu && WavebankDef.current == "Vanilla:Music_6"
                    && Menu.currentPage == "Main Menu" && Menu.currentPage != "Crash Page")
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
        /// Called before the game is drawn.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch" /> used to draw the game.</param>
        public override void PreGameDraw(SpriteBatch sb)
        {
            base.PreGameDraw(sb);

            if (Main.dedServ || Main.gameMenu)
                return;

            Main.sunTexture = DarkMatter.Check(Main.localPlayer) ? DarkMatterSun : sunBak;
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
            Biome.Biomes["Overworld"].Validation = (p, x, y) =>
                Biome.Biomes["Overworld"].TileValid(x, y, p.whoAmI) && !DarkMatter.Check(p) && !DarkMatter.TileValid(x, y, p.whoAmI);
            #endregion

            #region custom ones
            (DarkMatter = new DarkMatter()).AddToGame();
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
