using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;

namespace Avalon.Tiles.Furniture
{
    public class ClosedDoor : ModTileType
    {
        public override bool CanPlace(int x, int y)
        {
            if (WorldGen.SolidTile(x, y + 1) && WorldGen.SolidTile(x, y - 3))
            {
                WorldGen.PlaceTile(x, y - 2, Main.localPlayer.heldItem.createTile);
                Main.localPlayer.heldItem.stack--;
                return true;
            }
            return WorldGen.SolidTile(x, y - 1) && WorldGen.SolidTile(x, y + 3);
        }

        public override bool RightClick(int x, int y)
        {
            string tileName = TileDef.byType[Main.tile[x, y].type].Replace(" closed", "");
            Point p = TileDef.FindTopLeftPoint(x, y);
            int j = p.Y;

            if ((!Main.tile[x + Main.localPlayer.direction, j].active() || TileDef.breaksByCut[Main.tile[x + Main.localPlayer.direction, j].type]) && (!Main.tile[x + Main.localPlayer.direction, j + 1].active() || TileDef.breaksByCut[Main.tile[x + Main.localPlayer.direction, j + 1].type]) && (!Main.tile[x + Main.localPlayer.direction, j + 2].active() || TileDef.breaksByCut[Main.tile[x + Main.localPlayer.direction, j + 2].type]))
            {
                Main.PlaySound(8, x * 16, j * 16, 1);
                //Colors for paint
                byte color1 = Main.tile[x, j].color(), color2 = Main.tile[x, j + 1].color(), color3 = Main.tile[x, j + 2].color();

                if (Wiring.running)
                {
                    Wiring.SkipWire(x, j);
                    Wiring.SkipWire(x, j + 1);
                    Wiring.SkipWire(x, j + 2);
                    Wiring.SkipWire(x + Main.localPlayer.direction, j);
                    Wiring.SkipWire(x + Main.localPlayer.direction, j + 1);
                    Wiring.SkipWire(x + Main.localPlayer.direction, j + 2);
                }

                if (TileDef.breaksByCut[Main.tile[x + Main.localPlayer.direction, j].type])
                {
                    WorldGen.KillTile(x + Main.localPlayer.direction, j);
                }
                if (TileDef.breaksByCut[Main.tile[x + Main.localPlayer.direction, j + 1].type])
                {
                    WorldGen.KillTile(x + Main.localPlayer.direction, j + 1);
                }
                if (TileDef.breaksByCut[Main.tile[x + Main.localPlayer.direction, j + 2].type])
                {
                    WorldGen.KillTile(x + Main.localPlayer.direction, j + 2);
                }

                Main.tile[x, j].type = TileDef.byName[tileName];
                Main.tile[x, j].frameX = (short)((Main.localPlayer.direction == -1)?18:36);
                Main.tile[x, j].frameY = 0;
                Main.tile[x, j].color(color1);

                Main.tile[x, j + 1].type = TileDef.byName[tileName];
                Main.tile[x, j + 1].frameX = (short)((Main.localPlayer.direction == -1) ? 18 : 36);
                Main.tile[x, j + 1].frameY = 18;
                Main.tile[x, j + 1].color(color2);

                Main.tile[x, j + 2].type = TileDef.byName[tileName];
                Main.tile[x, j + 2].frameX = (short)((Main.localPlayer.direction == -1) ? 18 : 36);
                Main.tile[x, j + 2].frameY = 36;
                Main.tile[x, j + 2].color(color3);

                Main.tile[x + Main.localPlayer.direction, j].active(true);
                Main.tile[x + Main.localPlayer.direction, j].type = TileDef.byName[tileName];
                Main.tile[x + Main.localPlayer.direction, j].frameX = (short)((Main.localPlayer.direction == -1) ? 0 : 54);
                Main.tile[x + Main.localPlayer.direction, j].frameY = 0;
                Main.tile[x + Main.localPlayer.direction, j].color(color1);

                Main.tile[x + Main.localPlayer.direction, j + 1].active(true);
                Main.tile[x + Main.localPlayer.direction, j + 1].type = TileDef.byName[tileName];
                Main.tile[x + Main.localPlayer.direction, j + 1].frameX = (short)((Main.localPlayer.direction == -1) ? 0 : 54);
                Main.tile[x + Main.localPlayer.direction, j + 1].frameY = 18;
                Main.tile[x + Main.localPlayer.direction, j + 1].color(color2);

                Main.tile[x + Main.localPlayer.direction, j + 2].active(true);
                Main.tile[x + Main.localPlayer.direction, j + 2].type = TileDef.byName[tileName];
                Main.tile[x + Main.localPlayer.direction, j + 2].frameX = (short)((Main.localPlayer.direction == -1) ? 0 : 54);
                Main.tile[x + Main.localPlayer.direction, j + 2].frameY = 36;
                Main.tile[x + Main.localPlayer.direction, j + 2].color(color3);
            }
            return false;
        }

        public override bool MouseOver(int x, int y, Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            string tileName = TileDef.byType[Main.tile[x, y].type].Replace(" closed", "");
            sb.Draw(Main.itemTexture[ItemDef.byName[tileName].type], Main.mouse + new Vector2(10, 10), null, Color.White, 0f, Vector2.Zero, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            return true;
        }

        public override void HitWire(int x, int y, int wireType)
        {
            RightClick(x, y);
        }

        //This is a terrible place to put this, but I can't find a better hook- will ask about it later
        public override bool PreDrawType(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, bool solidTiles, int x, int y)
        {
            if (!Main.tile[x, y - 1].active())
            {
                WorldGen.KillTile(x, TileDef.FindTopLeftPoint(x, y).Y);
            }
            return true;
        }
    }
}
