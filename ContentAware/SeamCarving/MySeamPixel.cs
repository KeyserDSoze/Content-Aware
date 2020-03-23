using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ContentAware.SeamCarving
{
    public class MySeamPixel : IPixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
        public int Energy { get; set; }
        public void XMinusMinus(int deep)
            => this.X--;
    }
}
