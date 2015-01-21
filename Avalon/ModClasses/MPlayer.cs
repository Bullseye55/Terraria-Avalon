using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;
using PoroCYon.MCT.Net;
using Avalon.API.Items;
using Avalon.API.StarterSets;

namespace Avalon.ModClasses
{
    /// <summary>
    /// The <see cref="ModPlayer" /> class of the Avalon mod.
    /// </summary>
    public sealed partial class MPlayer : ModPlayer
	{
#pragma warning disable 414
#pragma warning disable 169
		float rot = 0f;
        bool LavaMerman = false;
        int skillCD = 0;
        readonly static int[] MUSIC_BOXES = new int[] { -1, 0, 1, 2, 4, 5, -1, 6, 7, 9, 8, 11, 10, 12 };
#pragma warning restore 414
#pragma warning restore 169

		int starterSet = -1;

		Item[] accessories
        {
            get
            {
                return MWorld.accessories[player.whoAmI];
            }
            set
            {
                MWorld.accessories[player.whoAmI] = value;
            }
        }

        /// <summary>
        /// Called when the Player spawns for the first time in the world.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //jungleChestId   = TileDef.type["Avalon:Jungle Chest"  ];
            //hellfireChestId = TileDef.type["Avalon:Hellfire Chest"];
        }

        /// <summary>
        /// Called when the Player is updated.
        /// </summary>
        public override void MidUpdate()
        {
            if (Main.gameMenu) // this is sometimes called when the world is still being loaded
                return;

            base.MidUpdate();

            U_SetChainTexture();
            //U_GetAchievements();
            U_SpawnBosses    ();
            U_ExtraAccs      ();
            U_LavaMerman     ();
			//U_Chests         ();
            //U_TileInteract   ();
            U_MysticalTomes  ();
            U_HideDarkMatter ();
        }
		#region MidUpdate submethods
		void U_SetChainTexture()
        {
            Item sel = player.inventory[player.selectedItem];
            if (player.itemAnimation < 0 && !player.delayUseItem) // not using it
                return;

            object attr = null;

            for (int i = 0; i < sel.modEntities.Count; i++)
            {
                object[] attrArr = sel.modEntities[i].GetType().GetCustomAttributes(typeof(ChainTextureAttribute), true);

                if (attrArr.Length > 0)
                    attr = attrArr[0]; // there should be only one
            }

            if (attr == null)
            {
                // reset
                if (Main.chainTexture != modBase.textures["Resources/Chains/Grapple Chain"])
                    Main.chainTexture  = modBase.textures["Resources/Chains/Grapple Chain"];

                if (Main.chain3Texture != modBase.textures["Resources/Chains/Blue Moon Chain"])
                    Main.chain3Texture  = modBase.textures["Resources/Chains/Blue Moon Chain"];
            }
            else
            {
                ChainTextureAttribute cta = (ChainTextureAttribute)attr;
                ModBase @base = (Mods.mods.FirstOrDefault(m => m.InternalName == cta.ModInternalName) ?? AvalonMod.Instance.mod).modBase; // LINQ++

                string path = cta.TextureName;

                if (!path.StartsWith("/"))
                    path = "Resources/Chains/" + cta.TextureName;
                else
                    path = path.Substring(1); // remove the leading '/'

                if (!@base.textures.ContainsKey(path))
                    return; // hmmm....

                Texture2D tex = @base.textures[path];

                if (cta.ReplaceFlailChain)
                    Main.chain3Texture = tex;
                else
                    Main.chainTexture = tex;
            }
        }
        void U_GetAchievements()
        {
            // to future me/reader: method call is uncommented (in OnUpdate) for the sake of performance.

            if (player.whoAmI == Main.myPlayer)
            {
                bool get = true;

                foreach (Item i in player.armor)
                    get &= i.type > 0 && i.stack > 0;
                foreach (Item i in accessories)
                    get &= i.type > 0 && i.stack > 0;

                if (get)
                {
                    // Mod.Achieve(1, "AVALON_AS+_OVERKILL", null, player.whoAmI);
                }
            }
        }
        void U_SpawnBosses    ()
        {
            if (NPC.spawnRate <= 0 || !AvalonMod.IsInSuperHardmode || MWorld.CheckForBosses() || player.townNPCs > 0 || Main.invasionType != 0)
                return;

            int rate = (NPC.spawnRate * 2) * (Main.eclipse ? 3 : (Main.bloodMoon ? 2 : 1));

            for (int i = 0; i < AvalonMod.spawns.Count; i++)
                if (AvalonMod.spawns[i].ShouldSpawn(rate, player))
                    AvalonMod.spawns[i].Spawn(player.whoAmI);
        }
        void U_ExtraAccs      ()
        {
            if (!player.active || String.IsNullOrEmpty(player.name) || player.dead || player.ghost)
                return;

            for (int i = 0; i < AvalonMod.ExtraSlots; i++)
            {
                Item acc = MWorld.accessories[player.whoAmI][i];

                if (acc == null || acc.IsBlank())
                    continue;

				ItemEffects(player, acc);

				//player.statDefense += acc.defense  ;
				//player.lifeRegen   += acc.lifeRegen;

				//if (acc.prefix != null && acc.prefix.id > 0 /*&& !acc.prefix.Equals(Prefix.None)*/ && acc.prefix.id < Defs.prefixes.Count)
				//	acc.prefix.ApplyToPlayer(player);

				//acc.Effects(player);
			}
		}
        void U_LavaMerman     ()
        {
            if (LavaMerman)
                player.fireWalk = player.lavaImmune = player.gills = player.accFlipper = player.merman = true;

            Main.armorHeadTexture[39] = LavaMerman
                ? modBase.textures["Resources/Misc/LavaMermanHead"] : modBase.textures["Resources/Misc/MermanHead"];
            Main.armorArmTexture [22] = LavaMerman
                ? modBase.textures["Resources/Misc/LavaMermanArm" ] : modBase.textures["Resources/Misc/MermanArm" ];
            Main.armorBodyTexture[22] = LavaMerman
                ? modBase.textures["Resources/Misc/LavaMermanBody"] : modBase.textures["Resources/Misc/MermanBody"];
            Main.armorLegTexture [21] = LavaMerman
                ? modBase.textures["Resources/Misc/LavaMermanLegs"] : modBase.textures["Resources/Misc/MermanLegs"];
        }
        void U_Chests         ()
		{
			// to future me/reader: method call is uncommented (in MidUpdate) for the sake of performance.

			//if (player.whoAmI == Main.myPlayer && player.chest >= 0)
			//{
			//    Chest c = Main.chest[player.chest];

			//    if (Main.tile[c.x, c.y].type == 21)
			//        switch (Main.tile[c.x, c.y].frameX)
			//        {
			//            case 2 * 18:
			//                Main.chestText = "Gold Chest";
			//                break;
			//            case 6 * 18:
			//                Main.chestText = "Shadow Chest";
			//                break;
			//        }
			//    else if (Main.tile[c.x, c.y].type == jungleChestId)
			//        Main.chestText = "Jungle Chest";
			//    else if (Main.tile[c.x, c.y].type == hellfireChestId)
			//        Main.chestText = "Hellfire Chest";
			//}
		}
		void U_TileInteract   ()
        {
			// to future me/reader: method call is uncommented (in MidUpdate) for the sake of performance (tiles are missing).

			TileHurtPlayer(player, TileDef.byName["Avalon:Magmatic Ore"], 20, CheckHurtMagma, " got burned...");
            TileHurtPlayer(player, TileDef.byName["Avalon:Dark Matter" ], 30, null, " got burned...");
            TileHurtPlayer(player, TileDef.byName["Avalon:Black Sand"  ], 20, null, " got stuck in sand...", -1);

            if (TouchesTile(player, 0, new[] { (int)TileDef.byName["Avalon:Ice Block"] }))
            {
                player.sliding = true;
                //player.baseSlideFactor = 0.1f;
            }
        }
        void U_MysticalTomes  ()
        {
            if (MWorld.localTome.IsBlank() || MWorld.localManager == null)
                return;

            MWorld.localTome.Effects(player);

            if (--skillCD <= 0)
                skillCD = 0;
			
            if (KState.Down(AvalonMod.TomeSkillHotkey) && skillCD <= 0)
            {
                MWorld.localManager.Activate(player);

                NetHelper.SendModData(modBase, NetMessages.ActivateSkill, Main.myPlayer);

                skillCD = MWorld.localManager.Cooldown;
            }
        }
        void U_HideDarkMatter ()
        {
            if (Main.dedServ || player.whoAmI != Main.myPlayer || !AvalonMod.DarkMatter.Check(player))
                return;

            int
                startX = (int)(Main.screenPosition.X / 16f) - 5,
                startY = (int)(Main.screenPosition.Y / 16f) - 5,
                lenX   = startX + Main.screenWidth  / 16 + 10,
                lenY   = startY + Main.screenHeight / 16 + 10;

            // prefetchure; Y first
            for (int y = startY; y < lenY; y++)
                for (int x = startX; x < lenX; x++)
                    if (Main.map[x, y] != null)
                        Main.map[x, y].light = 0; // 'actual' colour is #050403, not #000000...
        }
        #endregion

        /// <summary>
        /// Saves data to the player file.
        /// </summary>
        /// <param name="bb">The buffer to put data in.</param>
        public override void Save(BinBuffer bb)
        {
            base.Save(bb);

            for (int i = 0; i < accessories.Length; i++)
                bb.Write(accessories[i]);

            bb.Write(MWorld.localTome);

			bb.Write((byte)(starterSet == -1 ? 0 : starterSet));
        }
        /// <summary>
        /// Loads data from the player file.
        /// </summary>
        /// <param name="bb">The buffer to load data from.</param>
        public override void Load(BinBuffer bb)
        {
            base.Load(bb);

            for (int i = 0; i < accessories.Length; i++)
                accessories[i] = bb.ReadItem();

            MWorld.localTome = bb.ReadItem();

			starterSet = bb.ReadByte();
        }

		/// <summary>
		/// Called when the <see cref="Player" /> is created or when he/she respawns in mediumcore.
		/// </summary>
		/// <param name="mediumcoreRespawn">Whether the <see cref="Player" /> is respawning from a mediumcore death or not.</param>
		public override void OnInventoryReset(bool mediumcoreRespawn)
		{
			base.OnInventoryReset(mediumcoreRespawn);

			if (starterSet == -1)
				starterSet = StarterSet.SelectedSet;

			StarterSet set = StarterSet.Sets[starterSet];

			for (int i = 0; i < set.Items.Length; i++)
				player.inventory[i].netDefaults(set.Items[i]);

			player.armor[0].SetDefaults(set.ArmourHead);
			player.armor[1].SetDefaults(set.ArmourBody);
			player.armor[2].SetDefaults(set.ArmourLegs);

			for (int i = 0; i < MWorld.localAccessories.Length; i++)
				MWorld.localAccessories[i] = new Item();

			MWorld.localTome = new Item();

			StarterSet.SelectedSet = 0;
		}

#pragma warning disable 1591
		public override void DamageNPC   (NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
		{
			base.DamageNPC(npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);

			for (int i = 0; i < accessories.Length; i++)
				accessories[i].DamageNPC(player, npc, hitDir, ref damage, ref knockback, ref crit, ref critMult);
		}
		public override void DealtNPC    (NPC npc, int hitDir, int dmgDealt, float knockback, bool crit)
		{
			base.DealtNPC(npc, hitDir, dmgDealt, knockback, crit);

			for (int i = 0; i < accessories.Length; i++)
				accessories[i].DealtNPC(player, npc, hitDir, dmgDealt, knockback, crit);
		}
        public override void DamagePlayer(NPC npc, int hitDir, ref int damage, ref bool crit, ref float critMult)
        {
            base.DamagePlayer(npc, hitDir, ref damage, ref crit, ref critMult);

            for (int i = 0; i < accessories.Length; i++)
                accessories[i].DamagePlayer(npc, player, hitDir, ref damage, ref crit, ref critMult);
        }
        public override void DealtPlayer (NPC npc, int hitDir, int dmgDealt, bool crit)
        {
            base.DealtPlayer(npc, hitDir, dmgDealt, crit);

            for (int i = 0; i < accessories.Length; i++)
                accessories[i].DealtPlayer(npc, player, hitDir, dmgDealt, crit);
        }
        public override void DamagePVP   (Player p, int hitDir, ref int damage, ref bool crit, ref float critMult)
        {
            base.DamagePVP(p, hitDir, ref damage, ref crit, ref critMult);

            for (int i = 0; i < accessories.Length; i++)
                accessories[i].DamagePVP(player, p, hitDir, ref damage, ref crit, ref critMult);
        }
        public override void DealtPVP    (Player p, int hitDir, int dmgDealt, bool crit)
        {
            base.DealtPVP(p, hitDir, dmgDealt, crit);

            for (int i = 0; i < accessories.Length; i++)
                accessories[i].DealtPVP(player, p, hitDir, dmgDealt, crit);
        }
#pragma warning restore 1591

        /// <summary>
        ///
        /// </summary>
        /// <param name="fishingRod"></param>
        /// <param name="bait"></param>
        /// <param name="liquidType"></param>
        /// <param name="poolCount"></param>
        /// <param name="worldLayer"></param>
        /// <param name="questFish"></param>
        /// <param name="caughtType"></param>
	    public override void OnFishSelected(Item fishingRod, Item bait, int liquidType, int poolCount, int worldLayer, int questFish, ref int caughtType)
    	{
            if (Main.player[Main.myPlayer].zone["Corruption"] && Main.rand.Next(0, (int)(Main.player[Main.myPlayer].crateChance * 100)) == 0)
            {
            	caughtType = ItemDef.byName["Avalon:Corrupt Crate"].type;
            }
            else if (Main.player[Main.myPlayer].zone["Crimson"] && Main.rand.Next(0, (int)(Main.player[Main.myPlayer].crateChance * 100)) == 0)
            {
            	caughtType = ItemDef.byName["Avalon:Flesh Crate"].type;
            }
    	}

        bool CheckHurtTile ()
        {
            return player.armor.Count(i =>
                i.type == ItemDef.byName["Avalon:Tome of Luck"      ].type ||
                i.type == ItemDef.byName["Avalon:Kinetic Boots Gold"].type) > 0;
        }
        bool CheckHurtMagma()
        {
            return CheckHurtTile() && !player.lavaImmune;
        }

        /// <summary>
        /// Gets wether a player touches a tile or not.
        /// </summary>
        /// <param name="p">The player who touches or doesn't touch the tile.</param>
        /// <param name="r">The radius to check. 0 is touching, -1 is only in the tile.</param>
        /// <param name="types">The tile types to check.</param>
        /// <returns>true when the player touches it, false otherwise.</returns>
        public static bool TouchesTile(Player p, int r, IEnumerable<int> types)
        {
            int
                xMin = (int)((p.position.X - 2f)      / 16f) - r,
                xMax = (int)((p.position.X + p.width) / 16f) + r,
                yMin = (int)((p.position.Y - 2f)      / 16f) - r,
                yMax = (int)((p.position.Y + p.width) / 16f) + r;

            xMin = Math.Max(xMin, 0);
            xMax = Math.Min(xMax, Main.maxTilesX);
            yMin = Math.Max(yMin, 0);
            yMax = Math.Min(yMax, Main.maxTilesY);

            for (int x = xMin; x <= xMax; x++)
                for (int y = yMin; y <= yMax; y++)
                    if (Main.tile[x, y] != null && Main.tile[x, y].active() && types.Contains(Main.tile[x, y].type))
                        return true;

            return false;
        }
        /// <summary>
        /// Hurt a player when he/she touches a tile.
        /// </summary>
        /// <param name="p">The player who touches or doesn't touch the tile.</param>
        /// <param name="tileType">The type of the tile to check.</param>
        /// <param name="damage">The damage to inflict to the player.</param>
        /// <param name="canHurt">A function used to check when to hurt the player.</param>
        /// <param name="deathText">The text do display when the player dies from the damage.</param>
        /// <param name="r">The radius.</param>
        public static void TileHurtPlayer(Player p, int tileType, int damage, Func<bool> canHurt = null, string deathText = " got slain...", int r = 0)
        {
            TileHurtPlayer(p, new[] { tileType }, damage, canHurt, deathText, r);
        }
        /// <summary>
        /// Hurt a player when he/she touches a tile.
        /// </summary>
        /// <param name="p">The player who touches or doesn't touch the tile.</param>
        /// <param name="tileTypes">The types of the tiles to check.</param>
        /// <param name="damage">The damage to inflict to the player.</param>
        /// <param name="canHurt">A function used to check when to hurt the player.</param>
        /// <param name="deathText">The text do display when the player dies from the damage.</param>
        /// <param name="r">The radius.</param>
        public static void TileHurtPlayer(Player p, IEnumerable<int> tileTypes, int damage, Func<bool> canHurt = null, string deathText = " got slain...", int r = 0)
        {
            if (p.immune)
                return;

            bool hurt = TouchesTile(p, r, tileTypes);

            if (canHurt != null)
                hurt &= canHurt();

            if (hurt)
                p.Hurt(damage + (p.statDefense / 2), 0, false, false, deathText);
        }
    }
}
