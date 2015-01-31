using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.NPCs.Baits
{
	/// <summary>
	/// The Lava Mite.
	/// </summary>
	public sealed class LavaMite : ModNPC
	{
        const int FRAME_DELAY = 3;

        int frame = 0;
        int frameCD = FRAME_DELAY;

		/// <summary>
		/// Checks whether the spawning mechanism should spawn an <see cref="NPC" /> or not.
		/// </summary>
		/// <param name="x">The X position of the spawn place (in tiles).</param>
		/// <param name="y">The X position of the spawn place (in tiles).</param>
		/// <param name="type">The type of the <see cref="NPC" /> that might spawn.</param>
		/// <param name="spawnedOn">The <see cref="Player" /> the <see cref="NPC" /> might spawn on.</param>
		/// <returns>true if the <see cref="NPC" /> should spawn; otherwise, false.</returns>
		public override bool CanSpawn(int x, int y, int type, Player spawnedOn)
		{
			return Biome.Biomes["Hell"].Check(spawnedOn) && Main.rand.Next(13) == 0;
		}

        /// <summary>
        ///
        /// </summary>
        /// <param name="frameSize"></param>
        public override void SelectFrame(int frameSize)
        {
            if (--frameCD <= 0)
            {
                frame = 1 - frame;

                frameCD = FRAME_DELAY;
            }

            npc.frame = new Rectangle(0, npc.height * frame, npc.width, npc.height);

            base.SelectFrame(frameSize);
        }
    }
}
