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

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.bmp, *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var bitMap = new Bitmap(dlg.FileName);
                    NeuralForm n = new NeuralForm();
                    n.BitMap = bitMap;
                    n.Show();

                    //
                    var network = new BasicNetwork();
                    FileInfo networkFile = new FileInfo(@"D:\Imagery\network\network.eg");
                    if (System.IO.File.Exists(@"D:\Imagery\network\network.eg"))
                        network = (BasicNetwork)(Encog.Persist.EncogDirectoryPersistence.LoadObject(networkFile));


                    int hCounter = 0; int Wcounter = 0;
                    int hMax = bitMap.Height / 25;
                    int wMax = bitMap.Width / 25;
                    var outputBitmap = new Bitmap(bitMap.Width, bitMap.Height);
                    while (hMax > hCounter)
                    {
                        Encog.ML.Data.Image.ImageMLDataSet testingSet = new Encog.ML.Data.Image.ImageMLDataSet(new Encog.Util.DownSample.RGBDownsample(), false, 1, -1);
                        var _25x25bitmap = new Bitmap(25, 25);

                        for (int i = 0; i < 25; i++)
                        {

                            for (int j = 0; j < 25; j++)
                            {
                                var px = bitMap.GetPixel(hCounter * 25 + i, Wcounter * 25 + j);
                                _25x25bitmap.SetPixel(i, j, px);
                            }
                        }

                        ImageMLData data = new ImageMLData(_25x25bitmap);
                        testingSet.Add(data, null);
                        testingSet.Downsample(25, 25);
                        bool isVege = false;

                        foreach (IMLDataPair pair in testingSet)
                        {
                            IMLData output = network.Compute(pair.Input);
                            if (Math.Round(output[0]) == 0 && Math.Round(output[1]) == 1)
                                isVege = true;
                        }
                        for (int i = 0; i < 25; i++)
                        {

                            for (int j = 0; j < 25; j++)
                            {

                                var px = _25x25bitmap.GetPixel(i, j);
                                if (!isVege)
                                    outputBitmap.SetPixel(hCounter * 25 + i, Wcounter * 25 + j, px);
                                else
                                    outputBitmap.SetPixel(hCounter * 25 + i, Wcounter * 25 + j, Color.FromArgb(px.R, px.R, px.R));
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
