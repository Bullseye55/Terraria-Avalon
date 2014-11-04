using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.API.NPCs
{
    /// <summary>
    /// A structure used to 
    /// </summary>
    public struct VanillaDrop
    {
        static Dictionary<int, VanillaDrop[]> drops = new Dictionary<int, VanillaDrop[]>();

        /// <summary>
        /// Gets the dictionary of vanilla drops.
        /// </summary>
        public static Dictionary<int, VanillaDrop[]> Drops
        {
            get
            {
                return drops;
            }
        }

        /// <summary>
        /// The type of the <see cref="Item" /> to drop.
        /// </summary>
        public int Type;
        /// <summary>
        /// The chance (in percents) of the <see cref="Item" /> to be dropped.
        /// </summary>
        public float Chance;
        /// <summary>
        /// The amount of <see cref="Item" />s that will be dropped.
        /// </summary>
        public Func<int> Amount;

        /// <summary>
        /// Creates a new instance of the <see cref="VanillaDrop" /> structure.
        /// </summary>
        /// <param name="type"><see cref="Type" /></param>
        public VanillaDrop(int type)
            : this(type, 1f, 1)
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="VanillaDrop" /> structure.
        /// </summary>
        /// <param name="type"><see cref="Type" /></param>
        /// <param name="chance"><see cref="Chance" /></param>
        public VanillaDrop(int type, float chance)
            : this(type, chance, 1)
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="VanillaDrop" /> structure.
        /// </summary>
        /// <param name="type"><see cref="Type" /></param>
        /// <param name="chance"><see cref="Chance" /></param>
        /// <param name="amt"><see cref="Amount" /></param>
        public VanillaDrop(int type, float chance, int amt)
            : this(type, chance, () => amt)
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="VanillaDrop" /> structure.
        /// </summary>
        /// <param name="type"><see cref="Type" /></param>
        /// <param name="chance"><see cref="Chance" /></param>
        /// <param name="amt"><see cref="Amount" /></param>
        public VanillaDrop(int type, float chance, Func<int> amt)
        {
            Type   = type;
            Chance = chance;
            Amount = amt;
        }

        static int item(string s)
        {
            return ItemDef.byName[s].type;
        }
        static int npc (string s)
        {
            return NPCDef.byName[s].type;
        }

        internal static void InitDrops()
        {
            drops = new Dictionary<int, VanillaDrop[]>()
            {
                {
                    npc("Vanilla:Paladin"),
                    new[]
                    {
                        new VanillaDrop(item("Avalon:Throwing Paladin's Hammer"), 0.01f, 1)
                    }
                },
                {
                    npc("Vanilla:Goblin Archer"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Wooden Arrow"),  5f /  6f, () => Main.rand.Next(1, 6))
                    }
                },
                {
                    npc("Vanilla:Pirate Crossbower"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Wooden Arrow"), 11f / 12f, () => Main.rand.Next(1, 12))
                    }
                },
                {
                    npc("Vanilla:Skeleton Archer"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Flaming Arrow"), 0.8f, () => Main.rand.Next(1, 5))
                    }
                },
                {
                    npc("Vanilla:Snowman Gangsta"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Silver Bullet"), 0.75f, () => Main.rand.Next(1, 4))
                    }
                },
                {
                    npc("Vanilla:Tactical Skeleton"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Silver Bullet"), 0.95f, () => Main.rand.Next(1, 20))
                    }
                },
                {
                    npc("Vanilla:Pirate Captain"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Silver Bullet"),   1f, () => Main.rand.Next(5, 60)),
                        new VanillaDrop(item("Vanilla:Cannonball"   ), 0.9f, () => Main.rand.Next(1, 10))
                    }
                },
                {
                    npc("Vanilla:Elf Copter"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Silver Bullet"), 0.9f, () => Main.rand.Next(1, 10))
                    }
                },
                {
                    npc("Vanilla:Skeleton Sniper"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:High Velocity Bullet"), 1f / 17f, () => Main.rand.Next(1, 17))
                    }
                },
                {
                    npc("Vanilla:Skeleton Commando"),
                    new[]
                    {
                        new VanillaDrop(item("Vanilla:Rocket I"  ), 0.8f , () => Main.rand.Next(1, 4)),
                        new VanillaDrop(item("Vanilla:Rocket II" ), 0.75f, () => Main.rand.Next(1, 3)),
                        new VanillaDrop(item("Vanilla:Rocket III"), 0.33f, () => Main.rand.Next(1, 2)),
                        new VanillaDrop(item("Vanilla:Rocket IV" ), 0.5f , 1)
                    }
                }
            };
        }
    }
}
