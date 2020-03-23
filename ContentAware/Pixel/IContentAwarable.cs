using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ContentAware
{
    public interface IContentAwarable
    {
        IContentAwarable Me { get; }
        int Width { get; }
        int Height { get; }
        int Deep { get; set; }
        int RedimensionSize { get; set; }
        int StaticSize { get; set; }
        ReduceType ReduceType { get; }
        ICollection<IPixel> Map { get; set; }
        IList<IPixel>[,] Memoization { get; set; }
        Bitmap Reduce(int times);
        internal Bitmap ToBitmap(int times)
        {
            Bitmap bitmap = this.ReduceType == ReduceType.ByWidth ? new Bitmap(this.Width - times, this.Height) : new Bitmap(this.Width, this.Height - times);
            foreach (IPixel outerPixel in this.Map)
            {
                IList<IPixel> innerPixels = new List<IPixel>() { outerPixel };
                if (outerPixel is IPixelCluster)
                    innerPixels = (outerPixel as IPixelCluster).Pixels;
                foreach (IPixel pixel in innerPixels)
                {
                    if (ReduceType == ReduceType.ByWidth && pixel.X < bitmap.Width)
                        bitmap.SetPixel(pixel.X, pixel.Y, pixel.Color);
                    else if (ReduceType == ReduceType.ByHeight && pixel.X < bitmap.Height)
                        bitmap.SetPixel(pixel.Y, pixel.X, pixel.Color);
                }
            }
            return bitmap;
        }
        internal void ReduceMap(ICollection<IPixel> minPath)
        {
            foreach (IPixel minPixel in minPath)
            {
                this.Map.Remove(minPixel);
                foreach (IPixel pixel in this.Map.Where(x => x.X > minPixel.X && x.Y == minPixel.Y))
                    pixel.XMinusMinus(this.Deep);
            }
            this.RedimensionSize--;
        }
        internal ICollection<IPixel> ToMap<TPixel>(Bitmap bitmap)
            where TPixel : IPixel, new()
        {
            if (this.Deep < 1)
                this.Deep = 1;
            IList<IPixel> map = new List<IPixel>();
            int countI = 0;
            for (int i = 0; i < bitmap.Width; i += this.Deep)
            {
                int countJ = 0;
                for (int j = 0; j < bitmap.Height; j += this.Deep)
                {
                    IList<IPixel> pixels = new List<IPixel>();
                    for (int k = i; k < i + this.Deep && k < bitmap.Width; k++)
                        for (int h = j; h < j + this.Deep && h < bitmap.Height; h++)
                            pixels.Add(new TPixel()
                            {
                                X = this.ReduceType == ReduceType.ByWidth ? k : h,
                                Y = this.ReduceType == ReduceType.ByWidth ? h : k,
                                Color = bitmap.GetPixel(k, h)
                            });
                    if (this.Deep == 1)
                        map.Add(pixels.First());
                    else
                    {
                        TPixel pixelCluster = new TPixel();
                        pixelCluster.X = this.ReduceType == ReduceType.ByWidth ? countI : countJ;
                        pixelCluster.Y = this.ReduceType == ReduceType.ByWidth ? countJ : countI;
                        pixelCluster.Color = Color.FromArgb((int)pixels.Average(x => x.Color.R),
                                        (int)pixels.Average(x => x.Color.G),
                                        (int)pixels.Average(x => x.Color.B),
                                        (int)pixels.Average(x => x.Color.A));
                        (pixelCluster as IPixelCluster).Pixels = pixels;
                        map.Add(pixelCluster);
                    }
                    countJ++;
                }
                countI++;
            }
            this.RedimensionSize = map.OrderByDescending(x => x.X).First().X + 1;
            this.StaticSize = map.OrderByDescending(x => x.Y).First().Y + 1;
            return map;
        }
        internal void EnergyCalculationOnMap()
        {
            foreach (IPixel pixel in this.Map)
                pixel.CalculateEnergy(this.Map, this.RedimensionSize, this.StaticSize);
            this.Memoization = new List<IPixel>[RedimensionSize, StaticSize];
        }
    }
}
