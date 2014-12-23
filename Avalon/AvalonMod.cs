using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using TAPI;
using PoroCYon.MCT;
using Avalon.API.Audio;
using Avalon.API.Events;
using Avalon.API.Items.MysticalTomes;
using Avalon.API.NPCs;
using Avalon.API.World;
using Avalon.ModClasses;
using Avalon.UI.Menus;
using Avalon.World;
using System.Reflection;

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

        readonly static Color DM_BG_COLOUR = new Color(26, 0, 52);
        readonly static FieldInfo Lighting_skyColour = typeof(Lighting).GetField("skyColor", BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static);

        internal static List<BossSpawn> spawns = new List<BossSpawn>();

        internal readonly static List<int> EmptyIntList = new List<int>(); // only alloc once
        internal static Texture2D EmptyTexture;

        internal static Texture2D sunBak;
        internal static Texture2D[] bgBak = new Texture2D[Main.maxBackgrounds];
        internal static Point bg0SzBak;

        Option
			tomeSkillHotkey   ,
			shadowMirrorHotkey;

        float dmness;
        bool oldDm;

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

            if (EmptyTexture == null)
                (EmptyTexture = new Texture2D(TAPI.API.main.GraphicsDevice, 1, 1)).SetData(new Color[1] { Color.Transparent });

            sunBak = Main.sunTexture;
            DarkMatterSun = textures["Resources/Dark Matter/Sun"];
            for (int i = 0; i < Main.maxBackgrounds; i++)
            {
                try
                {
                    Main.LoadBackground(i);
                    bgBak[i] = Main.backgroundTexture[i];
                }
                catch { TConsole.Print("Could not load bg " + i + "."); }
            }

            bg0SzBak = new Point(Main.backgroundWidth[0], Main.backgroundHeight[0]);

            DarkMatterBackground = textures["Resources/Dark Matter/Background"];

            VorbisPlayer.LoadTrack("Resources/Music/Dark Matter (Overworld).ogg", this);

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

            DarkMatter = null;
            IsInSuperHardmode = false;

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

            // this is set in World\DarkMatter.cs:Avalon.World.DarkMatter::.ctor()
            //if (!Main.gameMenu && DarkMatter.Check(Main.localPlayer))
            //    current = "Avalon:Resources/Music/Dark Matter (Overworld).ogg";

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
        /// Called before the screen is cleared.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch" /> used to draw things.</param>
        public override void PreScreenClear(SpriteBatch sb)
        {
            const float BG_COLOUR_CHANGE_SPEED = 0.05f;

            base.PreScreenClear(sb);

            if (Main.gameMenu)
                return;

            if (DarkMatter.Check(Main.localPlayer))
            {
                if (dmness < 1f)
                    dmness += BG_COLOUR_CHANGE_SPEED;
            }
            else if (dmness > 0f)
                dmness -= BG_COLOUR_CHANGE_SPEED;

            if (dmness <= 0f)
                return;

            if (dmness == 1f)
            {
                // faster than calling lerp
                Lighting_skyColour.SetValue(null, Main.dayTime ? 0.5f : 0.3f);
                Lighting.brightness = Main.dayTime ? 0.3f : 0.1f;

                Main.tileColor = (Main.bgColor = DM_BG_COLOUR) * 3;
            }
            else
            {
                Lighting_skyColour.SetValue(null, MathHelper.Lerp((float)Lighting_skyColour.GetValue(null), Main.dayTime ? 0.5f : 0.3f, dmness));

                Main.tileColor = (Main.bgColor = Color.Lerp(Main.bgColor, DM_BG_COLOUR, dmness)) * 3;
            }
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

            bool dm = DarkMatter.Check(Main.localPlayer);

            Main.sunTexture = dm ? DarkMatterSun : sunBak;

            if (dm)
            {
                Main.backgroundTexture[0] = DarkMatterBackground;

                for (int i = 1; i < Main.backgroundTexture.Length; i++)
                    Main.backgroundTexture[i] = EmptyTexture;

                Main.backgroundWidth [0] = DarkMatterBackground.Width ;
                Main.backgroundHeight[0] = DarkMatterBackground.Height;
            }
            else if (oldDm)
            {
                for (int i = 0; i < Main.backgroundTexture.Length; i++)
                    Main.backgroundTexture[i] = bgBak[i];

                Main.backgroundWidth [0] = bg0SzBak.X;
                Main.backgroundHeight[0] = bg0SzBak.Y;
            }

            oldDm = dm;
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
