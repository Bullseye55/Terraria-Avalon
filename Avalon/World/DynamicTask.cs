using System;
using System.Collections.Generic;
using System.Linq;
using TAPI;

namespace Avalon.World
{
    using Act = Action;

    /// <summary>
    /// A <see cref="WorldGenTask" /> that executes an <see cref="System.Action" /> as <see cref="WorldGenTask.Generate" />.
    /// </summary>
    public class DynamicTask : WorldGenTask
    {
        /// <summary>
        /// Gets the <see cref="Act" /> that will be executed when <see cref="Generate" /> is called.
        /// </summary>
        public Act ToExcecute
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DynamicTask" /> class.
        /// </summary>
        /// <param name="name"><see cref="WorldGenTask.name" /></param>
        /// <param name="toExec"><see cref="ToExcecute" /></param>
        public DynamicTask(string name, Act toExec)
            : base(name)
        {

        }

        /// <summary>
        /// Executes the <see cref="WorldGenTask" />.
        /// </summary>
        public override void Generate()
        {
            throw new NotImplementedException();
        }
    }
}
