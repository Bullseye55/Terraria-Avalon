using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace Avalon.API.Items.DevItems
{
    /// <summary>
    /// An <see cref="Item" /> of a developer.
    /// </summary>
    public abstract class DevItem : ModItem
    {
        /// <summary>
        /// Gets the name of the developer.
        /// </summary>
        protected string Developer
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DevItem" /> class.
        /// </summary>
        /// <param name="devName">The name of the developer who owns the <see cref="Item" />.</param>
        protected DevItem(string devName)
            : base()
        {
            Developer = devName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public override void HoldStyle(Player p)
        {
            base.HoldStyle(p);

            PunishIfCheater(p);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public override void UseStyle (Player p)
        {
            base.UseStyle(p);

            PunishIfCheater(p);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool CanUse   (Player p)
        {
            PunishIfCheater(p);

            return base.CanUse(p) && IsOwner(p);
        }

        /// <summary>
        /// Gets whether <paramref name="p" /> is the actual owner of the <see cref="Item" /> or not.
        /// </summary>
        /// <param name="p">The <see cref="Player" /> to check.</param>
        /// <returns>True if <paramref name="p" /> is the owner (thus the developer); false otherwise.</returns>
        public bool IsOwner(Player p)
        {
            return
#if DEBUG
                p.name == Developer
#else
                false
#endif
                ;
        }

        /// <summary>
        /// Punishes <paramref name="p" /> if it is not the developer who owns the <see cref="Item" />.
        /// </summary>
        /// <param name="p">The <see cref="Player" /> to check.</param>
        /// <param name="fn">A method to call of <paramref name="p" /> is the developer.</param>
        protected void PunishIfCheater(Player p, Action<Player> fn = null)
        {
            if (IsOwner(p))
            {
                if (fn != null)
                    fn(p);

                return;
            }

            byte d = p.difficulty;
            p.difficulty = 0;

            p.KillMe(Math.Max(p.statLifeMax + 1, 9001d), 1, false, " did something " + Developer + " would not like...");

            item.SetDefaults(0);

            OnFoundCheater(p);

            p.difficulty = d;
        }

        /// <summary>
        /// Called when <paramref name="p" /> is not a developer.
        /// </summary>
        /// <param name="p">Not the developer.</param>
        protected virtual void OnFoundCheater(Player p) { }
    }
}
