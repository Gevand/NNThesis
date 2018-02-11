using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Data.Image;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.SGD;
using Encog.Util.DownSample;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural.NeuralNet
{
    public class VegetationNN
    {
        public static double[] Vegetation = { 0.0, 1.0 };

        public static double[] Nothing = { 1.0, 0.0 };



        public static void Run()
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 30000));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 100));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 2));



            network.Structure.FinalizeStructure();
            network.Reset();

            Encog.ML.Data.Image.ImageMLDataSet trainingSet = new Encog.ML.Data.Image.ImageMLDataSet(new RGBDownsample(), false, 1, -1);

            string[] fileEntries = Directory.GetFiles(@"D:\Imagery\_Vege");
            foreach (var file in fileEntries)
            {
                if (!file.Contains("_real")) continue;

                ImageMLData data = new ImageMLData(new System.Drawing.Bitmap(file));
                BasicMLData ideal = new BasicMLData(Vegetation);
                trainingSet.Add(data, ideal);
            }
            fileEntries = Directory.GetFiles(@"D:\Imagery\_Nothing");
            foreach (var file in fileEntries)
            {
                if (!file.Contains("_real")) continue;

                ImageMLData data = new ImageMLData(new System.Drawing.Bitmap(file));
                BasicMLData ideal = new BasicMLData(Nothing);
                trainingSet.Add(data, ideal);
            }
            trainingSet.Downsample(100, 100);

            IMLTrain train = new Backpropagation(network, trainingSet, .01, .9) { };

            int epoch = 1;
            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error: " + train.Error);
                epoch++;
            } while (train.Error > 0.01 || epoch > 100);
            train.FinishTraining();

            Encog.ML.Data.Image.ImageMLDataSet testingSet = new Encog.ML.Data.Image.ImageMLDataSet(new RGBDownsample(), false, 1, -1);
            fileEntries = Directory.GetFiles(@"D:\Imagery\_VegeTest");
            foreach (var file in fileEntries)
            {
                if (!file.Contains("_real")) continue;

                ImageMLData data = new ImageMLData(new System.Drawing.Bitmap(file));
                BasicMLData ideal = new BasicMLData(Vegetation);
                testingSet.Add(data, ideal);
            }
            fileEntries = Directory.GetFiles(@"D:\Imagery\_NothingTest");
            foreach (var file in fileEntries)
            {
                if (!file.Contains("_real")) continue;

                ImageMLData data = new ImageMLData(new System.Drawing.Bitmap(file));
                BasicMLData ideal = new BasicMLData(Nothing);
                testingSet.Add(data, ideal);
            }
            testingSet.Downsample(100, 100);

            Console.WriteLine(@"Neural Network Results:");
            foreach (IMLDataPair pair in testingSet)
            {
                IMLData output = network.Compute(pair.Input);
                Console.WriteLine(pair.Input[0] + @"," + pair.Input[1] + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            }

            EncogFramework.Instance.Shutdown();
        }
    }
}
