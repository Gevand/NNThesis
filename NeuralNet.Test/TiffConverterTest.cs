using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neural.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.Test
{
    [TestClass]
    public class TiffConverterTest
    {
        [TestMethod]
        public void TiffConverterTest_Split()
        {
            string[] fileEntries = Directory.GetFiles(@"D:\Imagery\");
            int counter = 0;
            foreach (var file in fileEntries)
            {
                if (file.Contains(".tif"))
                {
                    counter++;
                    if (counter > 100)
                        break;
                    var byteRay = TiffConverter.Split(file, 25);
                }
            }

        }
    }
}
