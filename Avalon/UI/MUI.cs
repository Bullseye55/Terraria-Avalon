using System;
using System.Collections.Generic;
using System.Linq;
using TAPI;

namespace Avalon.UI
{
    /// <summary>
    /// The <see cref="ModInterface" /> class of the Avalon mod.
    /// </summary>
    public sealed class MUI : ModInterface
    {
        /// <summary>
        /// Modifies the given interface layer list.
        /// </summary>
        /// <param name="list">The interface layer list to modify.</param>
        public override void ModifyInterfaceLayerList(List<InterfaceLayer> list)
        {
            base.ModifyInterfaceLayerList(list);

            InterfaceLayer.Add(list, MWorld.AccessoryLayer = new AccessorySlotLayer(), InterfaceLayer.LayerInventory, false);
            //InterfaceLayer.Add(list, MWorld.TomeSlotLayer  = new TomeSlotLayer     (),         MWorld.AccessoryLayer, false);
        }
    }
}
