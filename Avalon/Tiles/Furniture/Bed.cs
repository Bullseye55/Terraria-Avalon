using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Avalon.Tiles.Furniture
{
    /// <summary>
    ///
    /// </summary>
    public class Bed : ModTileType
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool RightClick(int x, int y)
        {
            return Main.localPlayer.CheckAndSetSpawn(x, y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public override bool MouseOver(int x, int y, Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(Main.itemTexture[ItemDef.byName[TileDef.byType[Main.tile[x, y].type]].type], Main.mouse + new Vector2(10, 10), null, Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            return true;
        }
    }
}
