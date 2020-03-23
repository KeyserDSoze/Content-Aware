using ContentAware;
using ContentAware.EnergizerPoint;
using ContentAware.SeamCarving;
using ContentAware.SeamRarving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Bitmap bitmap = new Bitmap(@"C:\Users\aless\Downloads\141104497-d2a191db-e894-4f72-93a8-92590d692313.jpg");
            //IContentAwarable contentAwarable = new MySeamCarving(bitmap, 10, ReduceType.ByWidth);
            //IContentAwarable contentAwarable = new RavingContent(bitmap, ReduceType.ByWidth);
            IContentAwarable contentAwarable = new Energizer(bitmap, ReduceType.ByWidth);
            bitmap = contentAwarable.Reduce(100);
            bitmap.Save(@"C:\Users\aless\Downloads\monna1.jpg");
        }
    }
}
