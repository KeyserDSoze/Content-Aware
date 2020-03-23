using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ContentAware.SeamCarving
{
    public class MyClusterPixel : IPixel, IPixelCluster
    {
        public int Energy { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
        public IList<IPixel> Pixels { get; set; }
        public void XMinusMinus(int deep)
        {
            this.X--;
            foreach (IPixel pixel in this.Pixels)
                pixel.X -= deep;
        }
    }
}
