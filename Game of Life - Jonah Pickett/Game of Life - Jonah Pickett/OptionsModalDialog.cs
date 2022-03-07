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
    public partial class OptionsModalDialog : Form
    {
        public OptionsModalDialog()
        {
            InitializeComponent();
        }

        // Interval Property
        public int OptionsInterval
        {
            get
            {
                return (int)numericUpDownInterval.Value;
            }
            set
            {
                numericUpDownInterval.Value = value;
            }
        }

        // Width Property
        public int OptionsWidth
        {
            get { return (int)numericUpDownWidth.Value; }
            set { numericUpDownWidth.Value = value; }
        }

        //Height Property
        public int OptionsHeight
        {
            get { return (int)numericUpDownHeight.Value; }
            set { numericUpDownHeight.Value = value; }
        }
    }
}
