﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            Neural.NeuralNet.VegetationNN.Run();
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
