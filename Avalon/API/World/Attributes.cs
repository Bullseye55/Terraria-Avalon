using System;
using System.Collections.Generic;
using System.Linq;

namespace Avalon.API.World
{
    /// <summary>
    /// Specifies whether the ModNPC is an NPC that appears (read: doesn't despawn) when a Wraith invasion is active.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class WraithInvasionNpcAttribute : Attribute { }
    /// <summary>
    /// Specifies whether the ModNPC is an NPC that spawns in the Dark Matter biome.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DarkMatterNpcAttribute     : Attribute { }
}
