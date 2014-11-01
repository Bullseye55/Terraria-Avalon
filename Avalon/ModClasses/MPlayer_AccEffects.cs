using System;
using System.Collections.Generic;
using System.Linq;
using PoroCYon.MCT.Net;
using TAPI;
using Terraria;

namespace Avalon.ModClasses
{
	partial class MPlayer
	{
		/*void _OldItemEffects(Player player, Item acc)
		{
			switch (acc.type)
			{
				#region 1.0 & 1.1
				case 238:	//Wizard Hat
					player.magicDamage += .15f;
					break;
				case 111:	//Band of Starpower
					player.statManaMax2 += 20;
					break;
				case 268:	//Diving Helmet
					player.accDivingHelm = true;
					break;
				case 15: //Copper Watch
					if (player.accWatch < 1)
						player.accWatch = 1;
					break;
				case 16: //Silver Watch
					if (player.accWatch < 2)
						player.accWatch = 2;
					break;
				case 17: //Gold Watch
					if (player.accWatch < 3)
						player.accWatch = 3;
					break;
				case 18: //Depth Meter
					if (player.accDepthMeter < 1)
						player.accDepthMeter = 1;
					break;
				case 53: //Cloud in a Bottle
					player.doubleJump = true;
					break;
				case 54: //Hermes Boots
						 //if (player.baseMaxSpeed < 6f)
						 //    player.baseMaxSpeed = 6f;
					break;
				case 128:	//Rocket Boots
					if (player.rocketBoots == 0)
						player.rocketBoots = 1;
					break;
				case 156:	//Cobalt Shield
					player.noKnockback = true;
					break;
				case 158:	//Lucky Horseshoe
					player.noFallDmg = true;
					break;
				case 159:	//Shiny Red Balloon
					player.jumpBoost = true;
					break;
				case 187:	//Flipper
					player.accFlipper = true;
					break;
				case 211:	//Feral Claws
					player.meleeSpeed += .12f;
					break;
				case 223:	//Nature's Gift
					player.manaCost -= .06f;
					break;
				case 285:	//Aglet
					player.moveSpeed += .05f;
					break;
				case 212:	//Anklet of the Wind
					player.moveSpeed += .1f;
					break;
				case 267:	//Guide Voodoo Doll
					player.killGuide = true;
					break;
				case 193:	//Obsidian Skull
					player.fireWalk = true;
					break;
				case 485:	//Moon Charm
					player.wolfAcc = true;
					break;
				case 486:	//Ruler
					player.rulerAcc = true;
					break;
				case 393:	//Compass
					player.accCompass = 1;
					break;
				case 394:	//Diving Gear
					player.accFlipper = player.accDivingHelm = true;
					break;
				case 395:	//GPS
					player.accWatch = 3;
					player.accDepthMeter = player.accCompass = 1;
					break;
				case 396:	//Obsidian Horseshoe
					player.noFallDmg = player.fireWalk = true;
					break;
				case 397:	//Obsidian Shield
					player.noKnockback = player.fireWalk = true;
					break;
				case 399:	//Cloud in a Balloon
					player.jumpBoost = player.doubleJump = true;
					break;
				case 405:	//Spectre Boots
							//if (player.baseMaxSpeed < 6f)
							//    player.baseMaxSpeed = 6f;
					if (player.rocketBoots == 0)
						player.rocketBoots = 2;
					break;
				case 407:	//Toolbelt
					if (player.blockRange < 1)
						player.blockRange = 1;
					break;
				case 489:	//Sorcerer Emblem
					player.magicDamage += .15f;
					break;
				case 490:	//Warrior Emblem
					player.meleeDamage += .15f;
					break;
				case 491:	//Ranger Emblem
					player.rangedDamage += .15f;
					break;
				case 492:	//Demon Wings
					if (player.wings == 0)
						player.wings = 1;
					break;
				case 493:	//Angel Wings
					if (player.wings == 0)
						player.wings = 2;
					break;
				case 497:	//Neptune's Shell
					player.accMerman = true;
					break;
				case 535:	//Philosopher's Stone
					player.pStone = true;
					break;
				case 536:	//Titan Glove
					player.kbGlove = true;
					break;
				case 532:	//Star Cloak
					player.starCloak = true;
					break;
				case 554:	//Cross Necklace
					player.longInvince = true;
					break;
				case 555:	//Mana Flower
					player.manaFlower = true;
					player.manaCost -= .08f;
					break;
				#endregion
				#region music box
				default:
					if (Main.myPlayer == player.whoAmI)
					{
						if (acc.type != 576 || acc.type > 603)
							break;

						if (acc.type == 576 && Main.rand.Next(18000) == 0 &&
							!WavebankDef.current.StartsWith("Vanilla:") ||
							(WavebankDef.current.StartsWith("Vanilla:") && String.IsNullOrEmpty(WavebankDef.current)))
						{
							int id = String.IsNullOrEmpty(WavebankDef.current) ? 0 : Convert.ToInt32(WavebankDef.current.Substring("Vanilla:".Length));

							int mid = id <= MUSIC_BOXES.Length - 1 ? MUSIC_BOXES[id] : -1;
							if (mid <= -1)
								break;

							acc.SetDefaults(562 + mid, false);
							BinBuffer bb = new BinBuffer(new BinBufferByte());
							bb.Write((byte)player.whoAmI);
							acc.Write(bb);

							bb.Pos = 0;
							NetHelper.SendModData(modBase, NetMessages.SetMusicBox, toSend: bb.ReadBytes());
						}
						if (acc.type >= 562 && acc.type <= 574)
							WavebankDef.musicBox = "Vanilla:" + (acc.type - 562);
					}
					break;
					#endregion
			}
		}*/

		void ItemEffects(Player player, Item acc)
		{
			MainEffects(player, acc);
			AccEffects (player, acc);

			WingEffects(player, acc);
			VanityWings(player, acc);
		}

		void MainEffects(Player player, Item acc)
		{
			player.statDefense += acc.defense;
			player.lifeRegen += acc.lifeRegen;

			if (acc.type == 268)
			{
				player.accDivingHelm = true;
			}
			if (acc.type == 238)
			{
				player.magicDamage += 0.15f;
			}
			if (acc.type == 2277)
			{
				player.magicDamage += 0.05f;
				player.meleeDamage += 0.05f;
				player.rangedDamage += 0.05f;
				player.magicCrit += 5;
				player.rangedCrit += 5;
				player.meleeCrit += 5;
				player.meleeSpeed += 0.1f;
				player.moveSpeed += 0.1f;
			}
			if (acc.type == 2279)
			{
				player.magicDamage += 0.06f;
				player.magicCrit += 6;
				player.manaCost -= 0.1f;
			}
			if (acc.type == 2275)
			{
				player.magicDamage += 0.07f;
				player.magicCrit += 7;
			}
			if (acc.type == 123 || acc.type == 124 || acc.type == 125)
			{
				player.magicDamage += 0.07f;
			}
			if (acc.type == 151 || acc.type == 152 || acc.type == 153 || acc.type == 959)
			{
				player.rangedDamage += 0.05f;
			}
			if (acc.type == 111 || acc.type == 228 || acc.type == 229 || acc.type == 230 || acc.type == 960 || acc.type == 961 || acc.type == 962)
			{
				player.statManaMax2 += 20;
			}
			if (acc.type == 228 || acc.type == 229 || acc.type == 230 || acc.type == 960 || acc.type == 961 || acc.type == 962)
			{
				player.magicCrit += 3;
			}
			if (acc.type == 100 || acc.type == 101 || acc.type == 102)
			{
				player.meleeSpeed += 0.07f;
			}
			if (acc.type == 956 || acc.type == 957 || acc.type == 958)
			{
				player.meleeSpeed += 0.07f;
			}
			if (acc.type == 791 || acc.type == 792 || acc.type == 793)
			{
				player.meleeDamage += 0.02f;
				player.rangedDamage += 0.02f;
				player.magicDamage += 0.02f;
			}
			if (acc.type == 371)
			{
				player.magicCrit += 9;
				player.statManaMax2 += 40;
			}
			if (acc.type == 372)
			{
				player.moveSpeed += 0.07f;
				player.meleeSpeed += 0.12f;
			}
			if (acc.type == 373)
			{
				player.rangedDamage += 0.1f;
				player.rangedCrit += 6;
			}
			if (acc.type == 374)
			{
				player.magicCrit += 3;
				player.meleeCrit += 3;
				player.rangedCrit += 3;
			}
			if (acc.type == 375)
			{
				player.moveSpeed += 0.1f;
			}
			if (acc.type == 376)
			{
				player.magicDamage += 0.15f;
				player.statManaMax2 += 60;
			}
			if (acc.type == 377)
			{
				player.meleeCrit += 5;
				player.meleeDamage += 0.1f;
			}
			if (acc.type == 378)
			{
				player.rangedDamage += 0.12f;
				player.rangedCrit += 7;
			}
			if (acc.type == 379)
			{
				player.rangedDamage += 0.05f;
				player.meleeDamage += 0.05f;
				player.magicDamage += 0.05f;
			}
			if (acc.type == 380)
			{
				player.magicCrit += 3;
				player.meleeCrit += 3;
				player.rangedCrit += 3;
			}
			if (acc.type >= 2367 && acc.type <= 2369)
			{
				player.fishingSkill += 5;
			}
			if (acc.type == 400)
			{
				player.magicDamage += 0.11f;
				player.magicCrit += 11;
				player.statManaMax2 += 80;
			}
			if (acc.type == 401)
			{
				player.meleeCrit += 7;
				player.meleeDamage += 0.14f;
			}
			if (acc.type == 402)
			{
				player.rangedDamage += 0.14f;
				player.rangedCrit += 8;
			}
			if (acc.type == 403)
			{
				player.rangedDamage += 0.06f;
				player.meleeDamage += 0.06f;
				player.magicDamage += 0.06f;
			}
			if (acc.type == 404)
			{
				player.magicCrit += 4;
				player.meleeCrit += 4;
				player.rangedCrit += 4;
				player.moveSpeed += 0.05f;
			}
			if (acc.type == 1205)
			{
				player.meleeDamage += 0.08f;
				player.meleeSpeed += 0.12f;
			}
			if (acc.type == 1206)
			{
				player.rangedDamage += 0.09f;
				player.rangedCrit += 9;
			}
			if (acc.type == 1207)
			{
				player.magicDamage += 0.07f;
				player.magicCrit += 7;
				player.statManaMax2 += 60;
			}
			if (acc.type == 1208)
			{
				player.meleeDamage += 0.03f;
				player.rangedDamage += 0.03f;
				player.magicDamage += 0.03f;
				player.magicCrit += 2;
				player.meleeCrit += 2;
				player.rangedCrit += 2;
			}
			if (acc.type == 1209)
			{
				player.meleeDamage += 0.02f;
				player.rangedDamage += 0.02f;
				player.magicDamage += 0.02f;
				player.magicCrit++;
				player.meleeCrit++;
				player.rangedCrit++;
			}
			if (acc.type == 1210)
			{
				player.meleeDamage += 0.07f;
				player.meleeSpeed += 0.07f;
				player.moveSpeed += 0.07f;
			}
			if (acc.type == 1211)
			{
				player.rangedCrit += 15;
				player.moveSpeed += 0.08f;
			}
			if (acc.type == 1212)
			{
				player.magicCrit += 18;
				player.statManaMax2 += 80;
			}
			if (acc.type == 1213)
			{
				player.magicCrit += 6;
				player.meleeCrit += 6;
				player.rangedCrit += 6;
			}
			if (acc.type == 1214)
			{
				player.moveSpeed += 0.11f;
			}
			if (acc.type == 1215)
			{
				player.meleeDamage += 0.08f;
				player.meleeCrit += 8;
				player.meleeSpeed += 0.08f;
			}
			if (acc.type == 1216)
			{
				player.rangedDamage += 0.16f;
				player.rangedCrit += 7;
			}
			if (acc.type == 1217)
			{
				player.magicDamage += 0.16f;
				player.magicCrit += 7;
				player.statManaMax2 += 100;
			}
			if (acc.type == 1218)
			{
				player.meleeDamage += 0.04f;
				player.rangedDamage += 0.04f;
				player.magicDamage += 0.04f;
				player.magicCrit += 3;
				player.meleeCrit += 3;
				player.rangedCrit += 3;
			}
			if (acc.type == 1219)
			{
				player.meleeDamage += 0.03f;
				player.rangedDamage += 0.03f;
				player.magicDamage += 0.03f;
				player.magicCrit += 3;
				player.meleeCrit += 3;
				player.rangedCrit += 3;
				player.moveSpeed += 0.06f;
			}
			if (acc.type == 558)
			{
				player.magicDamage += 0.12f;
				player.magicCrit += 12;
				player.statManaMax2 += 100;
			}
			if (acc.type == 559)
			{
				player.meleeCrit += 10;
				player.meleeDamage += 0.1f;
				player.meleeSpeed += 0.1f;
			}
			if (acc.type == 553)
			{
				player.rangedDamage += 0.15f;
				player.rangedCrit += 8;
			}
			if (acc.type == 551)
			{
				player.magicCrit += 7;
				player.meleeCrit += 7;
				player.rangedCrit += 7;
			}
			if (acc.type == 552)
			{
				player.rangedDamage += 0.07f;
				player.meleeDamage += 0.07f;
				player.magicDamage += 0.07f;
				player.moveSpeed += 0.08f;
			}
			if (acc.type == 1001)
			{
				player.meleeDamage += 0.16f;
				player.meleeCrit += 6;
			}
			if (acc.type == 1002)
			{
				player.rangedDamage += 0.16f;
				player.ammoCost80 = true;
			}
			if (acc.type == 1003)
			{
				player.statManaMax2 += 80;
				player.manaCost -= 0.17f;
				player.magicDamage += 0.16f;
			}
			if (acc.type == 1004)
			{
				player.meleeDamage += 0.05f;
				player.magicDamage += 0.05f;
				player.rangedDamage += 0.05f;
				player.magicCrit += 7;
				player.meleeCrit += 7;
				player.rangedCrit += 7;
			}
			if (acc.type == 1005)
			{
				player.magicCrit += 8;
				player.meleeCrit += 8;
				player.rangedCrit += 8;
				player.moveSpeed += 0.05f;
			}
			if (acc.type == 2189)
			{
				player.statManaMax2 += 60;
				player.manaCost -= 0.13f;
				player.magicDamage += 0.05f;
				player.magicCrit += 5;
			}
			if (acc.type == 1503)
			{
				player.magicDamage -= 0.4f;
			}
			if (acc.type == 1504)
			{
				player.magicDamage += 0.07f;
				player.magicCrit += 7;
			}
			if (acc.type == 1505)
			{
				player.magicDamage += 0.08f;
				player.moveSpeed += 0.08f;
			}
			if (acc.type == 1546)
			{
				player.rangedCrit += 5;
				player.arrowDamage += 0.15f;
			}
			if (acc.type == 1547)
			{
				player.rangedCrit += 5;
				player.bulletDamage += 0.15f;
			}
			if (acc.type == 1548)
			{
				player.rangedCrit += 5;
				player.rocketDamage += 0.15f;
			}
			if (acc.type == 1549)
			{
				player.rangedCrit += 13;
				player.rangedDamage += 0.13f;
				player.ammoCost80 = true;
			}
			if (acc.type == 1550)
			{
				player.rangedCrit += 7;
				player.moveSpeed += 0.12f;
			}
			if (acc.type == 1282)
			{
				player.statManaMax2 += 20;
				player.manaCost -= 0.05f;
			}
			if (acc.type == 1283)
			{
				player.statManaMax2 += 40;
				player.manaCost -= 0.07f;
			}
			if (acc.type == 1284)
			{
				player.statManaMax2 += 40;
				player.manaCost -= 0.09f;
			}
			if (acc.type == 1285)
			{
				player.statManaMax2 += 60;
				player.manaCost -= 0.11f;
			}
			if (acc.type == 1286)
			{
				player.statManaMax2 += 60;
				player.manaCost -= 0.13f;
			}
			if (acc.type == 1287)
			{
				player.statManaMax2 += 80;
				player.manaCost -= 0.15f;
			}
			if (acc.type == 1316 || acc.type == 1317 || acc.type == 1318)
			{
				player.aggro += 250;
			}
			if (acc.type == 1316)
			{
				player.meleeDamage += 0.06f;
			}
			if (acc.type == 1317)
			{
				player.meleeDamage += 0.08f;
				player.meleeCrit += 8;
			}
			if (acc.type == 1318)
			{
				player.meleeCrit += 4;
			}
			if (acc.type == 2199 || acc.type == 2202)
			{
				player.aggro += 250;
			}
			if (acc.type == 2201)
			{
				player.aggro += 400;
			}
			if (acc.type == 2199)
			{
				player.meleeDamage += 0.06f;
			}
			if (acc.type == 2200)
			{
				player.meleeDamage += 0.08f;
				player.meleeCrit += 8;
				player.meleeSpeed += 0.06f;
				player.moveSpeed += 0.06f;
			}
			if (acc.type == 2201)
			{
				player.meleeDamage += 0.05f;
				player.meleeCrit += 5;
			}
			if (acc.type == 2202)
			{
				player.meleeSpeed += 0.06f;
				player.moveSpeed += 0.06f;
			}
			if (acc.type == 684)
			{
				player.rangedDamage += 0.16f;
				player.meleeDamage += 0.16f;
			}
			if (acc.type == 685)
			{
				player.meleeCrit += 11;
				player.rangedCrit += 11;
			}
			if (acc.type == 686)
			{
				player.moveSpeed += 0.08f;
				player.meleeSpeed += 0.07f;
			}
			if (acc.type == 2361)
			{
				player.maxMinions++;
				player.minionDamage += 0.04f;
			}
			if (acc.type == 2362)
			{
				player.maxMinions++;
				player.minionDamage += 0.04f;
			}
			if (acc.type == 2363)
			{
				player.minionDamage += 0.05f;
			}
			if (acc.type >= 1158 && acc.type <= 1161)
			{
				player.maxMinions++;
			}
			if (acc.type >= 1159 && acc.type <= 1161)
			{
				player.minionDamage += 0.1f;
			}
			if (acc.type >= 2370 && acc.type <= 2371)
			{
				player.minionDamage += 0.05f;
				player.maxMinions++;
			}
			if (acc.type == 2372)
			{
				player.minionDamage += 0.06f;
				player.maxMinions++;
			}
			if (acc.type >= 1832 && acc.type <= 1834)
			{
				player.maxMinions++;
			}
			if (acc.type >= 1832 && acc.type <= 1834)
			{
				player.minionDamage += 0.11f;
			}
			if (!acc.IsBlank())
			{
				acc.Effects(player);
			}

			if (acc.prefix != null && acc.prefix.id > 0 /*&& !acc.prefix.Equals(Prefix.None)*/ && acc.prefix.id < Defs.prefixes.Count)
				acc.prefix.ApplyToPlayer(player);
		}
		void AccEffects (Player player, Item acc)
		{
			if (acc.type == 2373)
			{
				player.accFishingLine = true;
			}
			if (acc.type == 2374)
			{
				player.fishingSkill += 10;
			}
			if (acc.type == 2375)
			{
				player.accTackleBox = 1;
			}
			if (acc.type == 2423)
			{
				player.autoJump = true;
				player.jumpSpeedBoost += 2.4f;
				player.extraFall += 15;
			}
			if (acc.type == 15 && player.accWatch < 1)
			{
				player.accWatch = 1;
			}
			if (acc.type == 16 && player.accWatch < 2)
			{
				player.accWatch = 2;
			}
			if (acc.type == 17 && player.accWatch < 3)
			{
				player.accWatch = 3;
			}
			if (acc.type == 707 && player.accWatch < 1)
			{
				player.accWatch = 1;
			}
			if (acc.type == 708 && player.accWatch < 2)
			{
				player.accWatch = 2;
			}
			if (acc.type == 709 && player.accWatch < 3)
			{
				player.accWatch = 3;
			}
			if (acc.type == 18 && player.accDepthMeter < 1)
			{
				player.accDepthMeter = 1;
			}
			if (acc.type == 857)
			{
				player.doubleJump2 = true;
			}
			if (acc.type == 983)
			{
				player.doubleJump2 = true;
				player.jumpBoost = true;
			}
			if (acc.type == 987)
			{
				player.doubleJump3 = true;
			}
			if (acc.type == 1163)
			{
				player.doubleJump3 = true;
				player.jumpBoost = true;
			}
			if (acc.type == 1724)
			{
				player.doubleJump4 = true;
			}
			if (acc.type == 1863)
			{
				player.doubleJump4 = true;
				player.jumpBoost = true;
			}
			if (acc.type == 1164)
			{
				player.doubleJump = true;
				player.doubleJump2 = true;
				player.doubleJump3 = true;
				player.jumpBoost = true;
			}
			if (acc.type == 1250)
			{
				player.jumpBoost = true;
				player.doubleJump = true;
				player.noFallDmg = true;
			}
			if (acc.type == 1252)
			{
				player.doubleJump2 = true;
				player.jumpBoost = true;
				player.noFallDmg = true;
			}
			if (acc.type == 1251)
			{
				player.doubleJump3 = true;
				player.jumpBoost = true;
				player.noFallDmg = true;
			}
			if (acc.type == 1249)
			{
				player.jumpBoost = true;
				player.bee = true;
			}
			if (acc.type == 1253 && (double)player.statLife <= (double)player.statLifeMax2 * 0.25)
			{
				player.AddBuff(62, 5, true);
			}
			if (acc.type == 1290)
			{
				player.panic = true;
			}
			if ((acc.type == 1300 || acc.type == 1858) && (player.inventory[player.selectedItem].useAmmo == 14 || player.inventory[player.selectedItem].useAmmo == 311 || player.inventory[player.selectedItem].useAmmo == 323))
			{
				player.scope = true;
			}
			if (acc.type == 1858)
			{
				player.rangedCrit += 10;
				player.rangedDamage += 0.1f;
			}
			if (acc.type == 1303 && player.wet)
			{
				Lighting.AddLight((int)player.Centre.X / 16, (int)player.Centre.Y / 16, 0.9f, 0.2f, 0.6f);
			}
			if (acc.type == 1301)
			{
				player.meleeCrit += 8;
				player.rangedCrit += 8;
				player.magicCrit += 8;
				player.meleeDamage += 0.1f;
				player.rangedDamage += 0.1f;
				player.magicDamage += 0.1f;
				player.minionDamage += 0.1f;
			}
			if (acc.type == 982)
			{
				player.statManaMax2 += 20;
				player.manaRegenDelayBonus++;
				player.manaRegenBonus += 25;
			}
			if (acc.type == 1595)
			{
				player.statManaMax2 += 20;
				player.magicCuffs = Math.Max(player.magicCuffs, 1f);
			}
			if (acc.type == 2219)
			{
				player.manaMagnet = true;
			}
			if (acc.type == 2220)
			{
				player.manaMagnet = true;
				player.magicDamage += 0.15f;
			}
			if (acc.type == 2221)
			{
				player.manaMagnet = true;
				player.magicCuffs = Math.Max(player.magicCuffs, 1f);
			}
			if (player.whoAmI == Main.myPlayer && acc.type == 1923)
			{
				player.tileRangeX++;
				player.tileRangeY++;
			}
			if (acc.type == 1247)
			{
				player.starCloak = Math.Max(player.starCloak, 3);
				player.bee = true;
			}
			if (acc.type == 1248)
			{
				player.meleeCrit += 10;
				player.rangedCrit += 10;
				player.magicCrit += 10;
			}
			if (acc.type == 854)
			{
				player.discountVanilla *= 0.8f;
			}
			if (acc.type == 855)
			{
				player.coins = true;
			}
			if (acc.type == 53)
			{
				player.doubleJump = true;
			}
			if (acc.type == 54)
			{
				player.moveSpeedMax = Math.Max(player.moveSpeedMax, 6f);
			}
			if (acc.type == 1579)
			{
				player.moveSpeedMax = Math.Max(player.moveSpeedMax, 6f);
				player.coldDash = true;
			}
			if (acc.type == 128)
			{
				player.rocketBoots = Math.Max(player.rocketBoots, 1);
			}
			if (acc.type == 156)
			{
				player.knockbackResist = 0f;
			}
			if (acc.type == 158)
			{
				player.noFallDmg = true;
			}
			if (acc.type == 934)
			{
				player.carpet = true;
			}
			if (acc.type == 953)
			{
				player.spikedBoots++;
			}
			if (acc.type == 975)
			{
				player.spikedBoots++;
			}
			if (acc.type == 976)
			{
				player.spikedBoots += 2;
			}
			if (acc.type == 977)
			{
				player.dash = Math.Max(player.dash, 1);
			}
			if (acc.type == 963)
			{
				player.blackBelt = true;
			}
			if (acc.type == 984)
			{
				player.blackBelt = true;
				player.dash = Math.Max(player.dash, 1);
				player.spikedBoots = Math.Max(player.spikedBoots, 2);
			}
			if (acc.type == 1131)
			{
				player.gravControl2 = true;
			}
			if (acc.type == 1132)
			{
				player.bee = true;
			}
			if (acc.type == 1578)
			{
				player.bee = true;
				player.panic = true;
			}
			if (acc.type == 950)
			{
				player.iceSkate = true;
			}
			if (acc.type == 159)
			{
				player.jumpBoost = true;
			}
			if (acc.type == 187)
			{
				player.accFlipper = true;
			}
			if (acc.type == 211)
			{
				player.meleeSpeed += 0.12f;
			}
			if (acc.type == 223)
			{
				player.manaCost -= 0.06f;
			}
			if (acc.type == 285)
			{
				player.moveSpeed += 0.05f;
			}
			if (acc.type == 212)
			{
				player.moveSpeed += 0.1f;
			}
			if (acc.type == 267)
			{
				player.killGuide = true;
			}
			if (acc.type == 1307)
			{
				player.killClothier = true;
			}
			if (acc.type == 193)
			{
				player.fireWalk = true;
			}
			if (acc.type == 861)
			{
				player.accMerman = true;
				player.wolfAcc = true;
			}
			if (acc.type == 862)
			{
				player.starCloak = Math.Max(player.starCloak, 3);
				player.longInvince = true;
			}
			if (acc.type == 860)
			{
				player.pStoneVanilla *= 0.75f;
			}
			if (acc.type == 863)
			{
				player.waterWalk2 = true;
			}
			if (acc.type == 907)
			{
				player.waterWalk2 = true;
				player.fireWalk = true;
			}
			if (acc.type == 908)
			{
				player.waterWalk = true;
				player.fireWalk = true;
				player.lavaMax += 420;
			}
			if (acc.type == 906)
			{
				player.lavaMax += 420;
			}
			if (acc.type == 485)
			{
				player.wolfAcc = true;
			}
			if (acc.type == 486)
			{
				player.rulerAcc = true;
			}
			if (acc.type == 393 && player.accCompass < 1)
			{
				player.accCompass = 1;
			}
			if (acc.type == 394)
			{
				player.accFlipper = true;
				player.accDivingHelm = true;
			}
			if (acc.type == 395)
			{
				player.accWatch = Math.Max(player.accWatch, 3);
				player.accDepthMeter = Math.Max(player.accDepthMeter, 1);
				player.accCompass = Math.Max(player.accCompass, 1);
			}
			if (acc.type == 396)
			{
				player.noFallDmg = true;
				player.fireWalk = true;
			}
			if (acc.type == 397)
			{
                player.knockbackResist = 0f;
				player.fireWalk = true;
			}
			if (acc.type == 399)
			{
				player.jumpBoost = true;
				player.doubleJump = true;
			}
			if (acc.type == 405)
			{
				player.moveSpeedMax = Math.Max(player.moveSpeedMax, 6f);
				player.rocketBoots = Math.Max(player.rocketBoots, 2);
			}
			if (acc.type == 1860)
			{
				player.accFlipper = true;
				player.accDivingHelm = true;
				if (player.wet)
				{
					Lighting.AddLight((int)player.Centre.X / 16, (int)player.Centre.Y / 16, 0.9f, 0.2f, 0.6f);
				}
			}
			if (acc.type == 1861)
			{
				player.accFlipper = true;
				player.accDivingHelm = true;
				player.iceSkate = true;
				if (player.wet)
				{
					Lighting.AddLight((int)player.Centre.X / 16, (int)player.Centre.Y / 16, 0.2f, 0.8f, 0.9f);
				}
			}
			if (acc.type == 2214)
			{
				player.tileSpeed += 0.5f;
			}
			if (player.whoAmI == Main.myPlayer && acc.type == 2215)
			{
				player.tileRangeX += 3;
				player.tileRangeY += 2;
			}
			if (acc.type == 2216)
			{
				player.autoPaint = true;
			}
			if (acc.type == 2217)
			{
				player.wallSpeed += 0.5f;
			}
			if (acc.type == 897)
            {
                player.knockbackMeleeModVanilla *= 2f;
				player.meleeSpeed += 0.12f;
			}
			if (acc.type == 1343)
            {
                player.knockbackMeleeModVanilla *= 2f;
                player.meleeSpeed += 0.09f;
				player.meleeDamage += 0.09f;
				player.magmaStone = true;
			}
			if (acc.type == 1167)
			{
				player.minionKB += 2f;
				player.minionDamage += 0.15f;
			}
			if (acc.type == 1864)
			{
				player.minionKB += 2f;
				player.minionDamage += 0.15f;
				player.maxMinions++;
			}
			if (acc.type == 1845)
			{
				player.minionDamage += 0.1f;
				player.maxMinions++;
			}
			if (acc.type == 1321)
			{
				player.magicQuiver = true;
				player.arrowDamage += 0.1f;
			}
			if (acc.type == 1322)
			{
				player.magmaStone = true;
			}
			if (acc.type == 1323)
            {
                player.lavaRoseVanilla *= 0.625f;
            }
			if (acc.type == 938)
			{
				player.knockbackResist = 0f;

				if (player.statLife > player.statLifeMax2 * 0.25f)
				{
					if (player.whoAmI == Main.myPlayer)
					{
						player.paladinGive = true;
					}
					else if (player.miscCounter % 5 == 0)
					{
						int myPlayer = Main.myPlayer;
						if (Main.player[myPlayer].team == player.team && player.team != 0)
						{
							float num = player.position.X - Main.player[myPlayer].position.X;
							float num2 = player.position.Y - Main.player[myPlayer].position.Y;
							float num3 = (float)Math.Sqrt(num * num + num2 * num2);
							if (num3 < 800f)
							{
								Main.player[myPlayer].AddBuff(43, 10, true);
							}
						}
					}
				}
			}
			if (acc.type == 936)
			{
				player.knockbackMeleeModVanilla *= 0.75f;
				player.meleeSpeed += 0.12f;
				player.meleeDamage += 0.12f;
			}
			if (acc.type == 898)
			{
				player.moveSpeedMax = Math.Max(player.moveSpeedMax, 6.75f);
				player.rocketBoots = Math.Max(player.rocketBoots, 2);
				player.moveSpeed += 0.08f;
			}
			if (acc.type == 1862)
			{
				player.moveSpeedMax = Math.Max(player.moveSpeedMax, 6.75f);
				player.rocketBoots = Math.Max(player.rocketBoots, 3);
				player.moveSpeed += 0.08f;
				player.iceSkate = true;
			}
			if (acc.type == 1865)
			{
				player.lifeRegen += 2;
				player.statDefense += 4;
				player.meleeSpeed += 0.1f;
				player.meleeDamage += 0.1f;
				player.meleeCrit += 2;
				player.rangedDamage += 0.1f;
				player.rangedCrit += 2;
				player.magicDamage += 0.1f;
				player.magicCrit += 2;
				player.pickSpeed -= 0.15f;
				player.minionDamage += 0.1f;
				player.minionKB += 0.5f;
			}
			if (acc.type == 899 && Main.dayTime)
			{
				player.lifeRegen += 2;
				player.statDefense += 4;
				player.meleeSpeed += 0.1f;
				player.meleeDamage += 0.1f;
				player.meleeCrit += 2;
				player.rangedDamage += 0.1f;
				player.rangedCrit += 2;
				player.magicDamage += 0.1f;
				player.magicCrit += 2;
				player.pickSpeed -= 0.15f;
				player.minionDamage += 0.1f;
				player.minionKB += 0.5f;
			}
			if (acc.type == 900 && !Main.dayTime)
			{
				player.lifeRegen += 2;
				player.statDefense += 4;
				player.meleeSpeed += 0.1f;
				player.meleeDamage += 0.1f;
				player.meleeCrit += 2;
				player.rangedDamage += 0.1f;
				player.rangedCrit += 2;
				player.magicDamage += 0.1f;
				player.magicCrit += 2;
				player.pickSpeed -= 0.15f;
				player.minionDamage += 0.1f;
				player.minionKB += 0.5f;
			}
			if (acc.type == 407)
			{
				player.blockRange = Math.Max(player.blockRange, 1);
			}
			if (acc.type == 489)
			{
				player.magicDamage += 0.15f;
			}
			if (acc.type == 490)
			{
				player.meleeDamage += 0.15f;
			}
			if (acc.type == 491)
			{
				player.rangedDamage += 0.15f;
			}
			if (acc.type == 935)
			{
				player.magicDamage += 0.12f;
				player.meleeDamage += 0.12f;
				player.rangedDamage += 0.12f;
				player.minionDamage += 0.12f;
			}
			player.wingTimeMax = Math.Max(player.wingTimeMax, player.GetWingTime(acc));
			if (acc.type == 2609)
			{
				player.ignoreWater = true;
			}
			if (acc.type == 885)
			{
				player.buffImmune[30] = true;
			}
			if (acc.type == 886)
			{
				player.buffImmune[36] = true;
			}
			if (acc.type == 887)
			{
				player.buffImmune[20] = true;
			}
			if (acc.type == 888)
			{
				player.buffImmune[22] = true;
			}
			if (acc.type == 889)
			{
				player.buffImmune[32] = true;
			}
			if (acc.type == 890)
			{
				player.buffImmune[35] = true;
			}
			if (acc.type == 891)
			{
				player.buffImmune[23] = true;
			}
			if (acc.type == 892)
			{
				player.buffImmune[33] = true;
			}
			if (acc.type == 893)
			{
				player.buffImmune[31] = true;
			}
			if (acc.type == 901)
			{
				player.buffImmune[33] = true;
				player.buffImmune[36] = true;
			}
			if (acc.type == 902)
			{
				player.buffImmune[30] = true;
				player.buffImmune[20] = true;
			}
			if (acc.type == 903)
			{
				player.buffImmune[32] = true;
				player.buffImmune[31] = true;
			}
			if (acc.type == 904)
			{
				player.buffImmune[35] = true;
				player.buffImmune[23] = true;
			}
			if (acc.type == 1921)
			{
				player.buffImmune[46] = true;
				player.buffImmune[47] = true;
			}
			if (acc.type == 1612)
			{
				player.buffImmune[33] = true;
				player.buffImmune[36] = true;
				player.buffImmune[30] = true;
				player.buffImmune[20] = true;
				player.buffImmune[32] = true;
				player.buffImmune[31] = true;
				player.buffImmune[35] = true;
				player.buffImmune[23] = true;
				player.buffImmune[22] = true;
			}
			if (acc.type == 1613)
			{
				player.knockbackResist = 0f;
				player.fireWalk = true;
				player.buffImmune[33] = true;
				player.buffImmune[36] = true;
				player.buffImmune[30] = true;
				player.buffImmune[20] = true;
				player.buffImmune[32] = true;
				player.buffImmune[31] = true;
				player.buffImmune[35] = true;
				player.buffImmune[23] = true;
				player.buffImmune[22] = true;
			}
			if (acc.type == 497)
			{
				player.accMerman = true;
			}
			if (acc.type == 535)
			{
				player.pStoneVanilla *= 0.75f;
			}
			if (acc.type == 536)
            {
                player.knockbackResist = 0f;
            }
			if (acc.type == 532)
			{
                player.starCloak = Math.Max(player.starCloak, 3);
			}
			if (acc.type == 554)
			{
				player.longInvince = true;
			}
			if (acc.type == 555)
			{
				player.manaFlower = true;
				player.manaCost -= 0.08f;
			}
			if (Main.myPlayer == player.whoAmI)
			{
				if (acc.type == 576 && Main.rand.Next(18000) == 0 && WavebankDef.current != "" && WavebankDef.current.StartsWith("Vanilla:"))
				{
					int num4 = Convert.ToInt32(WavebankDef.current.Substring("Vanilla:Music_".Length));
					int num5;
					switch (num4)
					{
						case 4:
						case 5:
						case 9:
						case 11:
							num5 = num4;
							break;
						case 6:
						case 7:
						case 8:
							goto IL_3066;
						case 10:
						case 12:
							num5 = num4 - 2;
							break;
						default:
							goto IL_3066;
					}
					IL_306C:
					if (num4 == 33)
					{
						acc.SetDefaults(2742, false);
						goto IL_3105;
					}
					if (num4 == 29)
					{
						acc.SetDefaults(1610, false);
						goto IL_3105;
					}
					if (num4 >= 30 && num4 <= 32)
					{
						acc.SetDefaults(1963 + num4 - 30, false);
						goto IL_3105;
					}
					if (num4 > 13)
					{
						acc.SetDefaults(1596 + num4 - 14, false);
						goto IL_3105;
					}
					acc.SetDefaults(num5 + 562, false);
					goto IL_3105;
					IL_3066:
					num5 = num4 - 1;
					goto IL_306C;
				}
				IL_3105:
				if (acc.type >= 562 && acc.type <= 574)
				{
					Main.musicBox2 = acc.type - 562;
				}
				if (acc.type >= 1596 && acc.type <= 1609)
				{
					Main.musicBox2 = acc.type - 1596 + 13;
				}
				if (acc.type == 1610)
				{
					Main.musicBox2 = 27;
				}
				if (acc.type == 1963)
				{
					Main.musicBox2 = 28;
				}
				if (acc.type == 1964)
				{
					Main.musicBox2 = 29;
				}
				if (acc.type == 1965)
				{
					Main.musicBox2 = 30;
				}
				if (acc.type == 2742)
				{
					Main.musicBox2 = 31;
				}
				if (Main.musicBox2 != -1)
				{
					WavebankDef.HandleMusicBox(Main.musicBox2);
				}
			}
		}

		void WingEffects(Player player, Item acc)
		{
			if (acc.wingSlot > 0)
				player.wingsLogic = acc.wingSlot;
		}
		void VanityWings(Player player, Item acc)
		{
			if (acc.wingSlot > 0)
				player.wings = acc.wingSlot;
		}
	}
}
