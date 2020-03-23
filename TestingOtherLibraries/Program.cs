using ImageMagick;
using System;

namespace TestingOtherLibraries
{
    class Program
    {
        static void Main(string[] args)
        {
			using (MagickImage mImage = new MagickImage(@"C:\Users\aless\Downloads\reminder.jpg"))
			{
				mImage.UnsharpMask(0, 2, 1, 0);
				mImage.Resize(400, 8);
				mImage.Quality = 82;
				mImage.Density = new Density(72);
				mImage.Write(@"C:\Users\aless\Downloads\myFirstTest3.jpg");
			}
		}
    }
}
