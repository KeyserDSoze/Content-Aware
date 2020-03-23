using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ContentAware.SeamCarving
{
    public class MySeamCarving : IContentAwarable
    {
        public IContentAwarable Me => this as IContentAwarable;
        public int RedimensionSize { get; set; }
        public int StaticSize { get; set; }
        public int Width { get; }
        public int Height { get; }
        public ReduceType ReduceType { get; }
        public int Deep { get; set; }
        public ICollection<IPixel> Map { get; set; }
        public IList<IPixel>[,] Memoization { get; set; }
        public MySeamCarving(Bitmap bitmap, int deep, ReduceType type)
        {
            IList<IPixel> map = new List<IPixel>();
            this.ReduceType = type;
            this.Deep = deep;
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            this.Map = this.Deep <= 1 ? Me.ToMap<MySeamPixel>(bitmap) : Me.ToMap<MyClusterPixel>(bitmap);
            Me.EnergyCalculationOnMap();
        }
        public Bitmap Reduce(int times)
        {
            for (int time = 0; time < times / this.Deep; time++)
            {
                int min = int.MaxValue;
                IList<IPixel> minPath = new List<IPixel>();
                for (int index = 0; index < this.RedimensionSize; index++)
                {
                    IList<IPixel> path = MinEnergy(index, 0);
                    int value = path.Sum(x => x.Energy);
                    if (value < min)
                    {
                        min = value;
                        minPath = path;
                    }
                }
                Me.ReduceMap(minPath);
                Me.EnergyCalculationOnMap();
            }
            return Me.ToBitmap(times);
        }
        private IList<IPixel> MinEnergy(int i, int j)
        {
            if (Memoization[i, j] != null)
                return Memoization[i, j];
            List<IPixel> pixels = new List<IPixel>() { this.Map.FirstOrDefault(x => x.X == i && x.Y == j) };
            int nextJ = j + 1;
            if (nextJ < this.StaticSize)
            {
                IList<IPixel> minPaths = new List<IPixel>();
                int minValue = int.MaxValue;
                for (int k = -1; k < 2; k++)
                {
                    int key = i + k;
                    if (key >= 0 && key < this.RedimensionSize)
                    {
                        IList<IPixel> path = MinEnergy(key, nextJ);
                        int value = path.Sum(x => x.Energy);
                        if (value < minValue)
                        {
                            minValue = value;
                            minPaths = path;
                        }
                    }
                }
                pixels.AddRange(minPaths);
            }
            return Memoization[i, j] = pixels;
        }
    }
}
