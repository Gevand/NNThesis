using System;
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
            for (int i = 0; i < 10; i++)
            {
                Neural.NeuralNet.VegetationNN.Run();
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
