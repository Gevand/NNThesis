using Neural.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter
{
    class Program
    {
        static void Main(string[] args)
        {

            //string[] fileEntries = Directory.GetFiles(@"D:\Imagery\");
            //int counter = 0;
            //foreach (var file in fileEntries)
            //{
            //    Console.WriteLine(file.ToString());
            //    if (file.Contains(".tif"))
            //    {
            //        counter++;
            //        if (counter > 100)
            //            break;
            //        var byteRay = TiffConverter.Split(file, 10);
            //    }
            //}
            //return;
            for (int i = 0; i < 1000; i++)
            {
                Neural.NeuralNet.VegetationNN.Run();
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
