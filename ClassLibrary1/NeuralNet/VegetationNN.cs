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
            FileInfo networkFile = new FileInfo(@"D:\Imagery\network\network.eg");

            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 25 * 25 * 3));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 50));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 2));
            network.Structure.FinalizeStructure();
            network.Reset();

            if (System.IO.File.Exists(@"D:\Imagery\network\network.eg"))
                network = (BasicNetwork)(Encog.Persist.EncogDirectoryPersistence.LoadObject(networkFile));



            Encog.ML.Data.Image.ImageMLDataSet trainingSet = new Encog.ML.Data.Image.ImageMLDataSet(new RGBDownsample(), false, 1, -1);

            Random rnd = new Random();
            //take 1000,, take 5000 from nothing and scramble them
            List<string> fileEntries = Directory.GetFiles(@"D:\Imagery\_Vege").OrderBy(x => rnd.Next()).Take(1000).ToList();
            fileEntries.AddRange(Directory.GetFiles(@"D:\Imagery\_Nothing").OrderBy(x => rnd.Next()).Take(5000).ToArray());
            fileEntries = fileEntries.OrderBy(x => rnd.Next()).Take(6000).ToList();
            foreach (var file in fileEntries)
            {
                var bitmap = new System.Drawing.Bitmap(file);
                ImageMLData data = new ImageMLData(bitmap);
                if (file.Contains("_Nothing"))
                {
                    BasicMLData ideal = new BasicMLData(Nothing);
                    trainingSet.Add(data, ideal);
                }
                else
                {
                    BasicMLData ideal = new BasicMLData(Vegetation);
                    trainingSet.Add(data, ideal);
                }
            }
            trainingSet.Downsample(25, 25);

            IMLTrain train = new Backpropagation(network, trainingSet, .001, 0.02) { };


            int epoch = 1;
            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error: " + train.Error);
                epoch++;
            } while (epoch < 50);
            train.FinishTraining();

            Encog.Persist.EncogDirectoryPersistence.SaveObject(networkFile, (BasicNetwork)network);
            Encog.ML.Data.Image.ImageMLDataSet testingSet = new Encog.ML.Data.Image.ImageMLDataSet(new RGBDownsample(), false, 1, -1);
            fileEntries = Directory.GetFiles(@"D:\Imagery\_VegeTest").ToList();
            foreach (var file in fileEntries)
            {
                ImageMLData data = new ImageMLData(new System.Drawing.Bitmap(file));
                BasicMLData ideal = new BasicMLData(Vegetation);
                testingSet.Add(data, ideal);
            }
            fileEntries = Directory.GetFiles(@"D:\Imagery\_NothingTest").ToList();
            foreach (var file in fileEntries)
            {
                ImageMLData data = new ImageMLData(new System.Drawing.Bitmap(file));
                BasicMLData ideal = new BasicMLData(Nothing);
                testingSet.Add(data, ideal);
            }
            testingSet.Downsample(25, 25);

            Console.WriteLine(@"Neural Network Results:");
            foreach (IMLDataPair pair in testingSet)
            {
                IMLData output = network.Compute(pair.Input);
                Console.WriteLine(@", actual (" + output[0] + @"," + output[1] + @"),ideal (" + pair.Ideal[0] + @"," + pair.Ideal[1] + ")");
            }

            EncogFramework.Instance.Shutdown();
        }
    }
}
