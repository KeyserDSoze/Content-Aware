using System;
using System.Collections.Generic;
using System.Text;

namespace ContentAware
{
    public interface IPixelCluster : IPixel
    {
        IList<IPixel> Pixels { get; set; }
    }
}
