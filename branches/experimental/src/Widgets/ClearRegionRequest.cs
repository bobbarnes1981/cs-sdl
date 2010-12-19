using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SdlDotNet.Widgets
{
    class ClearRegionRequest
    {
        public Rectangle Region {
            get;
            set;
        }

        public Widget WidgetToSkip {
            get;
            set;
        }

        public ClearRegionRequest(Rectangle region, Widget widgetToSkip) {
            this.Region = region;
            this.WidgetToSkip = widgetToSkip;
        }
    }
}
