using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using PoroCYon.MCT.Net;

namespace PoroCYon.MCT
{
    /// <summary>
    /// The invasion types
    /// </summary>
    public enum InvasionType : int
    {
        /// <summary>
        /// No invasion.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Goblin Army
        /// </summary>
        GoblinArmy = 1,
        /// <summary>
        /// The Frost Legion
        /// </summary>
        FrostLegion = 2,
        /// <summary>
        /// The Pirates
        /// </summary>
        Pirates = 3
    }

    /// <summary>
    /// Provides easy event invocation
    /// </summary>
    public static class World
    {
        /// <summary>
        /// Gets the current invasion type
        /// </summary>
        public static InvasionType CurrentInvasion
        {
            get
            {
                return (InvasionType)Main.invasionType;
            }
        }

        /// <summary>
        /// Gets the maximum amount of players in this session.
        /// </summary>
        /// <remarks>Use this instead of Main.player.Length. It will speed up stuff by a lot.</remarks>
        public static int NumPlayers
        {
            get
            {
                return Main.netMode == 0 ? 1 : Main.numPlayers;
            }
        }

        /// <summary>
        /// Gets Main.time as a <see cref="System.DateTime"/>
        /// </summary>
        public static DateTime TimeAsDateTime
        {
            get
            {
                double time = Main.time;

                if (!Main.dayTime)
                    time += 54000d;
                time = time / 86400d * 24d;

                double timeOffset = 7.5; // day/night offset
                time -= timeOffset - 12d;

                if (time < 0d)
                    time += 24d;

                DateTime now = DateTime.Now;

                int
                    hour   = (int)time % 24,
                    minute = (int)( (time - hour) * 60d) % 60,
                    second = (int)(((time - hour) * 60d - minute) * 60d) % 60,
                    day    = Math.Min(Main.moonPhase * 4 + 1, DateTime.DaysInMonth(now.Year, now.Month));

                return new DateTime(now.Year, now.Month, day, hour, minute, second);
            }
        }
        /// <summary>
        /// Gets Main.time as a <see cref="System.TimeSpan"/>
        /// </summary>
        public static TimeSpan TimeAsTimeSpan
        {
            get
            {
                return TimeAsDateTime.TimeOfDay;
            }
        }
        /// <summary>
        /// Gets Main.time as a string
        /// </summary>
        public static string TimeAsString
        {
            get
            {
                string amOrPm = "AM";
                double time = Main.time;

                if (!Main.dayTime)
                    time += 54000d;
                time = time / 86400d * 24d;

                double timeOffset = 7.5; // day/night offset
                time -= timeOffset - 12d;

                if (time < 0.0)
                    time += 24.0;

                if (time >= 12.0)
                    amOrPm = "PM";

                int
                    hour = (int)time,
                    minute = (int)((time - (double)hour) * 60d);

                string minText = minute.ToString();

                if (minute < 10.0)
                    minText = "0" + minText;

                if (hour > 12)
                    hour -= 12;
                if (hour == 0)
                    hour = 12;

                return hour + ":" + minText + " " + amOrPm;
            }
        }

        /// <summary>
        /// Spawns a custom Dust
        /// </summary>
        /// <param name="position">The position of the particle</param>
        /// <param name="size">The size of the particle</param>
        /// <param name="texture">The sprite of the particle</param>
        /// <param name="maskType">The masking type of the particle (mimic AI type)</param>
        /// <param name="velocity">The velocity of the particle; default is {0;0}</param>
        /// <param name="c">The colour (incl. alpha) of the particle; default is {255;255;255;0}</param>
        /// <param name="scale">The scale of the particle</param>
        /// <param name="noGravity">Wether the particle is bound to the laws of gravity or not; default is false (laws do apply)</param>
        /// <param name="noLight">Weter the particle can emit light or not; default is false (can emit light)</param>
        /// <returns>The ID of the newly spawned Dust instance</returns>
        public static int SpawnCustomDust(Vector2 position, Vector2 size, Texture2D texture, int maskType,
            Vector2 velocity = default(Vector2), Color? c = null, float scale = 1f, bool noGravity = false, bool noLight = false)
        {
            Color co = c ?? new Color(255, 255, 255, 0);
            int i;

            Main.dust[i = Dust.NewDust(position, (int)size.X, (int)size.Y, maskType, velocity.X, velocity.Y, co.A, co, scale)].OverrideTexture = texture;
            Main.dust[i].noGravity = noGravity;
            Main.dust[i].noLight = noLight;

            return i;
        }
        /// <summary>
        /// Spawns a custom Dust
        /// </summary>
        /// <param name="position">The position of the particle</param>
        /// <param name="size">The size of the particle</param>
        /// <param name="texture">The sprite of the particle</param>
        /// <param name="onUpdate">The behaviour of the particle</param>
        /// <param name="velocity">The velocity of the particle; default is {0;0}</param>
        /// <param name="c">The colour (incl. alpha) of the particle; default is {255;255;255;0}</param>
        /// <param name="scale">The scale of the particle</param>
        /// <param name="noGravity">Wether the particle is bound to the laws of gravity or not; default is false (laws do apply)</param>
        /// <param name="noLight">Weter the particle can emit light or not; default is false (can emit light)</param>
        /// <returns>The ID of the newly spawned Dust instance</returns>
        public static int SpawnCustomDust(Vector2 position, Vector2 size, Texture2D texture, Action<Dust> onUpdate,
            Vector2 velocity = default(Vector2), Color? c = null, float scale = 1f, bool noGravity = false, bool noLight = false)
        {
            Color co = c ?? new Color(255, 255, 255, 0);
            int i;

            Main.dust[i = Dust.NewDust(position, (int)size.X, (int)size.Y, 0, velocity.X, velocity.Y, co.A, co, scale)].OverrideTexture = texture;
            Main.dust[i].OverrideUpdate += onUpdate;
            Main.dust[i].noGravity = noGravity;
            Main.dust[i].noLight = noLight;

            return i;
        }
    }
}
