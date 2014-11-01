using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI.UIKit;
using Avalon.ModClasses;

namespace Avalon.UI
{
    /// <summary>
    /// An item slot for an accessory.
    /// </summary>
    public sealed class ExtraAccessorySlot : ItemSlot
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ExtraAccessorySlot" /> class.
        /// </summary>
        /// <param name="index">The index of the accessory slot.</param>
        public ExtraAccessorySlot(int index)
            : base(AvalonMod.Instance, "Equip", index, (s, i) =>
            {
                MWorld.localAccessories[s.index].OnUnEquip(Main.localPlayer, s);
                MWorld.localAccessories[s.index] = i;
                MWorld.localAccessories[s.index].OnEquip  (Main.localPlayer, s);
            }, s => MWorld.localAccessories[s.index])
		{

		}

        /// <summary>
        /// Gets whether the <see cref="ItemSlot" /> allows the given <see cref="Item" /> or not.
        /// </summary>
        /// <param name="i">The <see cref="Item" /> to check.</param>
        /// <returns>true if the <see cref="Item" /> can be placed in the <see cref="ItemSlot" />, false otherwise.</returns>
        public override bool AllowsItem(Item i)
        {
            return base.AllowsItem(i) && ((i.accessory && i.CanEquip(Main.localPlayer, this)) || i.IsBlank());
        }
    }
}
