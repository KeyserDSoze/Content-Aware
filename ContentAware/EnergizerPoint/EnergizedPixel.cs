using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ContentAware.EnergizerPoint
{
    public class FinalizerPixel : IPixel
    {
        public int Energy { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
        public void XMinusMinus(int deep = 1)
            => this.X--;
    }
    public class EnergizedPixel : IPixel, IPixelCluster
    {
        public int Energy { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }
        public IList<IPixel> Pixels { get; set; }
        public void XMinusMinus(int deep = 1)
            => this.X--;
    }
    public class Energizer : IContentAwarable
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
        private Bitmap Bitmap;
        public Energizer(Bitmap bitmap, ReduceType reduceType)
        {
            this.Bitmap = bitmap;
            this.ReduceType = reduceType;
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
        }
        public Bitmap Reduce(int times)
        {
            this.Deep = this.ReduceType == ReduceType.ByWidth ? this.Width / 20 : this.Height / 20;
            this.Map = Me.ToMap<EnergizedPixel>(this.Bitmap);
            Me.EnergyCalculationOnMap();
            int averageeX = 0;
            int averageCount = 0;
            foreach (var cluster in this.Map.OrderByDescending(x => x.Energy).ThenByDescending(x => x.X).Take(5))
            {
                averageeX += (int)((cluster as IPixelCluster).Pixels.Average(x => x.X) * (100 - averageCount * 10) / 100);
                averageCount++;
            }
            averageeX /= averageCount;
            IPixel clusterWithMaxEnergy = this.Map.OrderByDescending(x => x.Energy).ThenByDescending(x => x.X).FirstOrDefault();
            int averageX = (int)(clusterWithMaxEnergy as IPixelCluster).Pixels.Average(x => x.X);
            //foreach (var t in this.Map.GroupBy(x => Math.Abs(x.X - clusterWithMaxEnergy.X)).OrderByDescending(x => x.Key))
            //{
            //    foreach (var o in t)
            //        foreach (IPixel pixel in (o as IPixelCluster).Pixels)
            //        {
            //            if (t.Key == 1)
            //                pixel.Color = Color.Blue;
            //            else if (t.Key == 2)
            //                pixel.Color = Color.Violet;
            //            else if (t.Key == 3)
            //                pixel.Color = Color.Yellow;
            //            else if (t.Key == 4)
            //                pixel.Color = Color.Brown;
            //        }
            //}
            int count = 0;
            foreach (var t in this.Map.OrderByDescending(x => x.Energy).ThenByDescending(x => x.X))
            {
                foreach (IPixel pixel in (t as IPixelCluster).Pixels)
                    pixel.Color = GetColor(count, pixel.Color);
                count++;
            }
            //List<IPixel> c = this.Map.GroupBy(x => Math.Abs(x.X - clusterWithMaxEnergy.X)).OrderByDescending(x => x.Key).Take(times / this.Deep).SelectMany(x => x).ToList();
            //foreach (IPixel pixel in c)
            //    foreach (IPixel old in (pixel as IPixelCluster).Pixels)
            //        old.Color = Color.Black;
            //Me.ReduceMap(c);
            //return Me.ToBitmap(times);
            return Me.ToBitmap(0);
        }
        private Color GetColor(int count, Color pre)
        {
            if (count <= 4)
                return Color.FromArgb(255 - (count * 50 + 55), 0, 0);
            else
                return pre;
        }
    }
}
