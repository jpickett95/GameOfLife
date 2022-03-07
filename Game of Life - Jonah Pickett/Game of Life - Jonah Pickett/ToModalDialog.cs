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
    public partial class ToModalDialog : Form
    {
        public ToModalDialog()
        {
            InitializeComponent();
        }

        public int GenerationNumericUpDown
        {
            get { return (int)generationNumericUpDown.Value; }
            set { generationNumericUpDown.Value = value; }
        }

        public int GenNumUpDownMin
        {
            get { return (int)generationNumericUpDown.Minimum; }
            set { generationNumericUpDown.Minimum = value; }
        }
    }
}
