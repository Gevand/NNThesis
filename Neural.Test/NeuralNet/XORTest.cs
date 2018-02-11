using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural.Test.NeuralNet
{
    [TestClass]
    public class XORTest
    {
        [TestMethod]
        public void XORTest_Run()
        {
            Neural.NeuralNet.XOR.Run();
        }
    }
}
