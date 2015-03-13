using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;

namespace Avalon.Tiles.Furniture
{
    public class Bed : ModTileType
    {
        public override bool RightClick(int x, int y)
        {
            return Main.localPlayer.CheckAndSetSpawn(x, y);
        }

        public override bool MouseOver(int x, int y, Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(Main.itemTexture[ItemDef.byName[TileDef.byType[Main.tile[x, y].type]]], Main.mouse + new Vector2(10, 10), null, Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            return true;
        }
    }
}
