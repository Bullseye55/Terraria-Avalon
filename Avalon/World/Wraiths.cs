using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using PoroCYon.MCT;
using Avalon.API.World;

namespace Avalon.World
{
    class WraithInvasion : Invasion
    {
        public override string DisplayName
        {
            get
            {
                return "Wraiths";
            }
        }

        public override string ArrivedText
        {
            get
            {
                return "The " + DisplayName + " have arrived!";
            }
        }
        public override string DefeatedText
        {
            get
            {
                return "The " + DisplayName + " have been arrived!";
            }
        }

        internal WraithInvasion()
            : base()
        {
            StartText = d => DisplayName + " are coming from the " + d + "!";
        }
    }
    class WraithSpawn : BossSpawn
    {
        public override bool ShouldSpawn(int rate, Player p)
        {
            bool ret = Main.rand.Next(10) == 0 && Main.time == 0;

            if (!MWorld.oldNight || Main.dayTime)
                ret = false;

            MWorld.oldNight = !Main.dayTime;

            return ret;
        }
        public override void Spawn(int pid)
        {
            AvalonMod.Wraiths.Start();
        }
    }
}
