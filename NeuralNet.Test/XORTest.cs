using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeuralNet.Test
{
    [TestClass]
    public class XORTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Neural.NeuralNet.XOR.Run();
        }
    }
}
