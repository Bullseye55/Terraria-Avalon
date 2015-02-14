using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;

namespace Avalon.Tiles.Furniture
{
    public class Door : ModTileType
    {
        public override bool CanPlace(int x, int y)
        {
            int adjOpenTiles = 0, originalY = y;
            while (!WorldGen.SolidTile(x, y) && adjOpenTiles < 3)
            {
                adjOpenTiles++;
                y--;
            }
            y = originalY + 1;
            if (adjOpenTiles < 3)
            {
                while (!WorldGen.SolidTile(x, y) && adjOpenTiles < 3)
                {
                    adjOpenTiles++;
                    y++;
                }
            }
            return adjOpenTiles == 3;
        }

        public override bool RightClick(int x, int y)
        {
            int j = y + (Main.tile[x, y].frameY / 18 * -1);
            //Check direction and place here.
            return false;
        }

        public override bool MouseOver(int x, int y, Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            int type = Main.tile[x, y].type;
            int[] doorTypes = new int[] { TileDef.byName["Avalon:Heartstone Door closed"]};
            if (type == doorTypes[0])
            {
                sb.Draw(Main.itemTexture[ItemDef.byName["Avalon:Heartstone Door"].type], Main.mouse, null, Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            }
            return true;
        }
    }
}
