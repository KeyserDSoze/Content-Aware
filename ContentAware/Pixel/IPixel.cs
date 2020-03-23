using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ContentAware
{
    public interface IPixel
    {
        int Energy { get; set; }
        int X { get; set; }
        int Y { get; set; }
        Color Color { get; set; }
        void XMinusMinus(int deep);
        public void CalculateEnergy(ICollection<IPixel> map, int redimensionSize, int staticSize)
        {
            bool leftRight = this.X > 0 && this.X < redimensionSize - 1;
            Color leftValue = leftRight ? map.FirstOrDefault(x => x.X == this.X - 1 && x.Y == this.Y).Color : Color.Empty;
            Color rightValue = leftRight ? map.FirstOrDefault(x => x.X == this.X + 1 && x.Y == this.Y).Color : Color.Empty;
            bool topBottom = this.Y > 0 && this.Y < staticSize - 1;
            Color topValue = topBottom ? map.FirstOrDefault(x => x.X == this.X && x.Y == this.Y - 1).Color : Color.Black;
            Color bottomValue = topBottom ? map.FirstOrDefault(x => x.X == this.X && x.Y == this.Y + 1).Color : Color.Black;
            this.Energy = GetSingleEnergy(leftValue, rightValue) + GetSingleEnergy(topValue, bottomValue);
        }
        public int GetSingleEnergy(Color a, Color b)
            => Math.Abs(((int)a.R - (int)b.R) + ((int)a.G - (int)b.G) + ((int)a.B - (int)b.B) + ((int)a.A - (int)b.A));
    }
}
