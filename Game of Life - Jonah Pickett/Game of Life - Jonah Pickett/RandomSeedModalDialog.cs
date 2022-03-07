using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_of_Life___Jonah_Pickett
{
    public partial class RandomSeedModalDialog : Form
    {
        public RandomSeedModalDialog()
        {
            InitializeComponent();
        }

        public int SeedNumericUpDown
        {
            get { return (int)numericUpDownSeed.Value; }
            set { numericUpDownSeed.Value = value; }
        }

        private void button1Randomize_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            numericUpDownSeed.Value = rng.Next();
        }
    }
}
