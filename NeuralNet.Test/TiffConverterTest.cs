using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neural.Utilities;
using System;
using System.Collections.Generic;
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
            var byteRay = TiffConverter.Split(@"D:\Imagery\o13901_ne.tif", 100);
        }
    }
}
