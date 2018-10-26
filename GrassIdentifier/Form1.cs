using Encog.ML.Data;
using Encog.ML.Data.Image;
using Encog.Neural.Networks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrassIdentifier
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public const string NETWORK_PATH = @"Resources\network_10.eg";
        public const int SIZE = 10;
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.bmp; *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var bitMap = new Bitmap(dlg.FileName);
                    NeuralForm n = new NeuralForm();
                    n.BitMap = bitMap;
                    n.Show();

                    //
                    var network = new BasicNetwork();
                    FileInfo networkFile = new FileInfo(NETWORK_PATH);
                    if (System.IO.File.Exists(NETWORK_PATH))
                        network = (BasicNetwork)(Encog.Persist.EncogDirectoryPersistence.LoadObject(networkFile));


                    int hCounter = 0; int Wcounter = 0;
                    int hMax = bitMap.Height / SIZE;
                    int wMax = bitMap.Width / SIZE;
                    var outputBitmap = new Bitmap(bitMap.Width, bitMap.Height);
                    while (hMax > hCounter)
                    {
                        Encog.ML.Data.Image.ImageMLDataSet testingSet = new Encog.ML.Data.Image.ImageMLDataSet(new Encog.Util.DownSample.RGBDownsample(), false, 1, -1);
                        var _25x25bitmap = new Bitmap(SIZE, SIZE);

                        for (int i = 0; i < SIZE; i++)
                        {

                            for (int j = 0; j < SIZE; j++)
                            {
                                var px = bitMap.GetPixel(Wcounter * SIZE + j, hCounter * SIZE + i);
                                _25x25bitmap.SetPixel(i, j, px);
                            }
                        }

                        ImageMLData data = new ImageMLData(_25x25bitmap);
                        testingSet.Add(data, null);
                        testingSet.Downsample(SIZE, SIZE);
                        bool isVege = false;

                        foreach (IMLDataPair pair in testingSet)
                        {
                            IMLData output = network.Compute(pair.Input);
<<<<<<< HEAD
                            if (output[1] > .1)
=======
                            if (output[1] > .05)
>>>>>>> 5191c7f6afe7e264015c1350a6e8a5437c0d2537
                                isVege = true;
                        }
                        for (int i = 0; i < SIZE; i++)
                        {

                            for (int j = 0; j < SIZE; j++)
                            {

                                var px = _25x25bitmap.GetPixel(i, j);
                                if (!isVege)
                                    outputBitmap.SetPixel( Wcounter * SIZE + j, hCounter * SIZE + i, px);
                                else
                                    outputBitmap.SetPixel( Wcounter * SIZE + j, hCounter * SIZE + i, Color.FromArgb(px.R , 0, px.B));
                            }
                        }

                        Wcounter++;
                        if (wMax <= Wcounter)
                        {
                            Wcounter = 0;
                            hCounter++;
                        }

                    }


                    NeuralForm n2 = new NeuralForm();
                    n2.BitMap = outputBitmap;
                    n2.Show();


                }
            }
        }
    }
}
