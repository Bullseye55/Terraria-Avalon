using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.Tiles.Furniture
{
    /// <summary>
    ///
    /// </summary>
    public class OpenDoor : ModTileType
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public override bool MouseOver(int x, int y, Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            string tileName = TileDef.byType[Main.tile[x, y].type];
            sb.Draw(Main.itemTexture[ItemDef.byName[tileName].type], Main.mouse + new Vector2(10, 10), null, Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool RightClick(int x, int y)
        {
            Point p = TileDef.FindTopLeftPoint(x, y);
            int i = p.X, j = p.Y;
            int direction = Main.tile[i, j].frameX == 36 ? 0 : 1;

            if (Collision.EmptyTile(i + direction, j, true) && Collision.EmptyTile(i + direction, j + 1, true) && Collision.EmptyTile(i + direction, j + 2, true))
            {
                string tileName = TileDef.byType[Main.tile[i, j].type] + " closed";
                Main.PlaySound(9, x * 16, j * 16, 1);

                if (Wiring.running)
                {
                    Wiring.SkipWire(i + direction, j);
                    Wiring.SkipWire(i + direction, j + 1);
                    Wiring.SkipWire(i + direction, j + 2);
                }

                //Open to the right
                if (Main.tile[i, j].frameX == 36)
                {
                    Main.tile[i + 1, j].active(false);
                    Main.tile[i + 1, j + 1].active(false);
                    Main.tile[i + 1, j + 2].active(false);
                    Main.tile[i, j].active(false);
                    Main.tile[i, j + 1].active(false);
                    Main.tile[i, j + 2].active(false);
                    WorldGen.PlaceTile(i, j, TileDef.byName[tileName], true);
                }
                //Open to the left
                else
                {
                    Main.tile[i, j].active(false);
                    Main.tile[i, j + 1].active(false);
                    Main.tile[i, j + 2].active(false);
                    Main.tile[i + 1, j].active(false);
                    Main.tile[i + 1, j + 1].active(false);
                    Main.tile[i + 1, j + 2].active(false);
                    WorldGen.PlaceTile(i + 1, j, TileDef.byName[tileName], true);
                }
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="wireType"></param>
        public override void HitWire(int x, int y, int wireType)
        {
            RightClick(x, y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="solidTiles"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        //This is a terrible place to put this, but I can't find a better hook- will ask about it later
        public override bool PreDrawType(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, bool solidTiles, int x, int y)
        {
            Point p = TileDef.FindTopLeftPoint(x, y);
            int i = p.X, j = p.Y;
            int direction = Main.tile[i, j].frameX == 36 ? 0 : 1;
            if (!Main.tile[i + direction, j - 1].active() || !Main.tile[i + direction, j + 3].active())
            {
                WorldGen.KillTile(i, j);
            }
            return true;
        }
    }
}
