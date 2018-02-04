using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neural.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural.Test.Utilities
{
    [TestClass]
    public class TiffConverterTest
    {
        [TestMethod]
        public void TiffConverterTest_Convert()
        {
            try
            {
                var byteRay = TiffConverter.Split(@"D:\Imagery\o13901_ne.tif");
              
             
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
