using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI.UIKit;
using Avalon.API.Items.MysticalTomes;
using Avalon.ModClasses;

namespace Avalon.UI
{
    /// <summary>
    /// An item slot for a <see cref="SkillManager" />.
    /// </summary>
    public class MysticalTomeSlot : ItemSlot
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MysticalTomeSlot" /> class.
        /// </summary>
        public MysticalTomeSlot()
            : base(AvalonMod.Instance, "Equip", 0, (s, i) =>
            {
                s.MyItem.OnUnEquip(Main.localPlayer, s);
                MWorld.localManager = SkillManager.FromItem(MWorld.localTome = i);
                MWorld.localTome.OnEquip(Main.localPlayer, s);
            }, s => MWorld.localTome)
        {

        }

        /// <summary>
        /// Gets whether the <see cref="ItemSlot" /> allows the given <see cref="Item" /> or not.
        /// </summary>
        /// <param name="it">The <see cref="Item" /> to check.</param>
        /// <returns>true if the <see cref="Item" /> can be placed in the <see cref="ItemSlot" />, false otherwise.</returns>
        public override bool AllowsItem(Item it)
        {
            //TomeSkillAttribute attr = null;

			//for (int i = 0; i < it.modEntities.Count; i++)
				//if ((attr = it.modEntities[i].GetType().GetCustomAttributes(typeof(TomeSkillAttribute), true)
				//		.FirstOrDefault() /* there should be only one */ as TomeSkillAttribute) != null)
				//	break;

            return base.AllowsItem(it) && ((SkillManager.FromItem(it) != null && it.CanEquip(Main.localPlayer, this)) || it.IsBlank());
        }
    }
}
