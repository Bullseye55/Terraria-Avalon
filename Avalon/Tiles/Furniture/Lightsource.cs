using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TAPI;
using Microsoft.Xna.Framework;

namespace Avalon.Tiles.Furniture
{
    public class Lightsource : ModTileType
    {
        private bool lightOn;

        /*
        public override void PlaceTile(int x, int y)
        {
            lightOn = true;
        }
        */

        public override void Initialize()
        {
            lightOn = true;
        }

        public override void Save(BinBuffer bb)
        {
            bb.Write(lightOn);
        }

        public override void Load(BinBuffer bb)
        {
            lightOn = bb.ReadBool();
        }
        
        //This needs cleaning up a bit
        public override void HitWire(int x, int y, int wireType)
        {
            //Upper-left corner of tile
            int i = Main.tile[x, y].frameX / 18, j = y + (int)(Main.tile[x, y].frameY / 18 * -1);

            if (!lightOn)
            {
                if (Main.tile[x, y].type == TileDef.byName["Avalon:Heartstone Candle"] || Main.tile[x, y].type == TileDef.byName["Avalon:Heartstone Lantern"] || Main.tile[x, y].type == TileDef.byName["Avalon:Heartstone Lamp"])
                {
                    i -= 1;
                }
                else if (Main.tile[x, y].type == TileDef.byName["Avalon:Heartstone Chandelier"])
                {
                    i -= 3;
                }
            }

            while (i > 1)
            {
                i -= 2;
            }
            i *= -1;
            i += x;

            if (Main.tile[i, j].type == TileDef.byName["Avalon:Heartstone Candelabra"])
            {
                if (Main.tile[i, j].frameX == 0)
                {
                    SetFrameX(i, j, 36);
                    SetFrameX(i + 1, j, 54);
                    SetFrameX(i, j + 1, 36);
                    SetFrameX(i + 1, j + 1, 54);
                    lightOn = false;
                }
                else
                {
                    SetFrameX(i, j, 0);
                    SetFrameX(i, j + 1, 0);
                    SetFrameX(i + 1, j, 18);
                    SetFrameX(i + 1, j + 1, 18);
                    lightOn = true;
                }
            }
            else if (Main.tile[i, j].type == TileDef.byName["Avalon:Heartstone Candle"] || Main.tile[i, j].type == TileDef.byName["Avalon:Heartstone Lantern"] || Main.tile[i, j].type == TileDef.byName["Avalon:Heartstone Lamp"])
            {
                if (Main.tile[i, j].frameX == 0)
                {
                    SetFrameX(i, j, 18);
                    SetFrameX(i, j + 1, 18);
                    if (Main.tile[i, j].type == TileDef.byName["Avalon:Heartstone Lamp"])
                        SetFrameX(i, j + 2, 18);
                    lightOn = false;
                }
                else
                {
                    SetFrameX(i, j, 0);
                    SetFrameX(i, j + 1, 0);
                    if (Main.tile[i, j].type == TileDef.byName["Avalon:Heartstone Lamp"])
                        SetFrameX(i, j + 2, 0);
                    lightOn = true;
                }
            }
            else if (Main.tile[i, j].type == TileDef.byName["Avalon:Heartstone Chandelier"])
            {
                if (Main.tile[i, j].frameX == 0)
                {
                    SetFrameX(i, j, 54);
                    SetFrameX(i + 1, j, 72);
                    SetFrameX(i + 2, j, 90);
                    SetFrameX(i, j + 1, 54);
                    SetFrameX(i + 1, j + 1, 72);
                    SetFrameX(i + 2, j + 1, 90);
                    SetFrameX(i, j + 2, 54);
                    SetFrameX(i + 1, j + 2, 72);
                    SetFrameX(i + 2, j + 2, 90);
                    lightOn = false;
                }
                else
                {
                    SetFrameX(i, j, 0);
                    SetFrameX(i + 1, j, 18);
                    SetFrameX(i + 2, j, 36);
                    SetFrameX(i, j + 1, 0);
                    SetFrameX(i + 1, j + 1, 18);
                    SetFrameX(i + 2, j + 1, 36);
                    SetFrameX(i, j + 2, 0);
                    SetFrameX(i + 1, j + 2, 18);
                    SetFrameX(i + 2, j + 2, 36);
                    lightOn = true;
                }
            }
        }

        private void SetFrameX(int i, int j, short frameNum)
        {
            Wiring.SkipWire(i, j);
            Main.tile[i, j].frameX = frameNum;
        }

        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            if (lightOn)
            {
                r = 1f;
                g = .4f;
                b = .7f;
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }
        }
    }
}
