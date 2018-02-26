using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrassIdentifier
{
    public partial class NeuralForm : Form
    {
        public NeuralForm()
        {
            InitializeComponent();
        }

        public Bitmap BitMap { get; internal set; }

        private void NeuralForm_Load(object sender, EventArgs e)
        {

            this.Width = BitMap.Width;
            this.Height = BitMap.Height;
            pbOutPut.Image = BitMap;
        }
    }
}
