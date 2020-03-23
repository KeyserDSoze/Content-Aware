using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ContentAware.SeamRarving
{
    public class RavingPixel : IPixel
    {
        public int Energy { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
        public void XMinusMinus(int deep = 1)
            => this.X--;
    }
    public class RavingContent : IContentAwarable
    {
        public IContentAwarable Me => this as IContentAwarable;
        public int Width { get; }
        public int Height { get; }
        public int Deep { get; set; }
        public int RedimensionSize { get; set; }
        public int StaticSize { get; set; }
        public ReduceType ReduceType { get; }
        public ICollection<IPixel> Map { get; set; }
        public IList<IPixel>[,] Memoization { get; set; }
        public RavingContent(Bitmap bitmap, ReduceType reduceType)
        {
            this.ReduceType = reduceType;
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            this.Deep = 1;
            this.Map = Me.ToMap<RavingPixel>(bitmap);
        }
        public Bitmap Reduce(int times)
        {
            Me.EnergyCalculationOnMap();
            var grouping = this.ReduceType == ReduceType.ByWidth ? this.Map.GroupBy(x => x.X) : this.Map.GroupBy(x => x.Y);
            foreach (var element in grouping.OrderBy(x => x.Key).ThenBy(x => x.Sum(y => y.Energy)).Take(times))
                Me.ReduceMap(element.ToList());
            return Me.ToBitmap(times);
        }
    }
}
