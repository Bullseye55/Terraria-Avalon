using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;
using PoroCYon.MCT.Net;
using Avalon.API.Items.MysticalTomes;

namespace Avalon
{
    /// <summary>
    /// All Avalon NetMessages.
    /// </summary>
    public enum NetMessages
    {
        /// <summary>
        /// Start the Wraith Invasion
        /// </summary>
        StartWraithInvasion,
        /// <summary>
        /// Something not really obvious
        /// </summary>
        SetMusicBox,
        /// <summary>
        /// Request tiles when teleporting with the <!--<see cref="Items.Other.ShadowMirror" />-->.
        /// </summary>
        RequestTiles,
        /// <summary>
        /// Request the content of the extra accessory slots and the tome slot.
        /// </summary>
        RequestCustomSlots,
        /// <summary>
        /// Send the content of the extra accessory slots and the tome slot. Usually sent as a response to <see cref="RequestCustomSlots" />.
        /// </summary>
        SendCustomSlots,
        /// <summary>
        /// Activates a <see cref="SkillManager" />.
        /// </summary>
        ActivateSkill
    }

    namespace ModClasses
    {
        /// <summary>
        /// Networking stuff.
        /// </summary>
        public sealed class MNet : ModNet
        {
            /// <summary>
            /// When the mod receives data from a peer connection.
            /// </summary>
            /// <param name="bb">The content of the message.</param>
            /// <param name="msg">The message type.</param>
            /// <param name="buffer">The <see cref="MessageBuffer" /> that received the message.</param>
            public override void NetReceive(BinBuffer bb, int msg, MessageBuffer buffer)
            {
                base.NetReceive(bb, msg, buffer);

                // commonly used vars
                int id;

                switch ((NetMessages)msg)
                {
                    case NetMessages.StartWraithInvasion:
                        // todo
                        break;
                    case NetMessages.SetMusicBox:
                        // todo
                        break;
                    case NetMessages.RequestTiles:
                        NetMessage.SendTileSquare(bb.ReadInt(), bb.ReadInt(), bb.ReadInt(), bb.ReadInt());
                        break;
                    case NetMessages.RequestCustomSlots:
                        BinBuffer itemB = new BinBuffer();

                        itemB.Write(Main.myPlayer);
                        itemB.WriteX(MWorld.localAccessories);
                        itemB.Write(MWorld.localTome);

                        itemB.Pos = 0;

                        NetHelper.SendModData(AvalonMod.Instance, NetMessages.SendCustomSlots, bb.ReadInt(), -1, itemB.ReadBytes());
                        break;
                    case NetMessages.SendCustomSlots:
                        id = bb.ReadInt();

                        for (int i = 0; i < AvalonMod.ExtraSlots; i++)
                            MWorld.accessories[id][i] = bb.ReadItem();

                        MWorld.tomes[id] = bb.ReadItem();
                        MWorld.managers[id] = SkillManager.FromItem(MWorld.tomes[id]);
                        break;
                    case NetMessages.ActivateSkill:
                        id = bb.ReadInt();

                        if (MWorld.managers[id] == null)
                            MWorld.managers[id] = SkillManager.FromItem(MWorld.tomes[id]);

                        if (MWorld.managers[id] != null)
                            MWorld.managers[id].Activate(Main.player[id]); // id is both client id and player id.
                        break;
                }
            }

            /// <summary>
            /// Called when a <see cref="Player" /> joins the sever
            /// </summary>
            /// <param name="index">The whoAmI of the <see cref="Player" /> who is joining</param>
            public override void PlayerConnected(int index)
            {
                base.PlayerConnected(index);

                NetHelper.SendModData(modBase, NetMessages.RequestCustomSlots, index, -1, Main.myPlayer);
            }
        }
    }
}
