using System;
using System.Collections.Generic;
using System.Text;

namespace SdlDotNet.Widgets
{
    [FlagsAttribute]
    public enum DragDropEvents
    {
        /// <summary>
        /// The drop target does not accept the data.
        /// </summary>
        None,
        /// <summary>
        /// The data from the drag source is copied to the drop target.
        /// </summary>
        Copy,
        /// <summary>
        /// The data from the drag source is moved to the drop target.
        /// </summary>
        Move,
        /// <summary>
        /// The data from the drag source is linked to the drop target.
        /// </summary>
        Link,
        /// <summary>
        /// The target can be scrolled while dragging to locate a drop position that is not currently visible in the target.
        /// </summary>
        Scroll,
        /// <summary>
        /// The combination of the Copy, Move, and Scroll effects.
        /// </summary>
        All
    }
}
