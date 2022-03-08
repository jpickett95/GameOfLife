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
    public partial class HelpModelessDialog : Form
    {
        public HelpModelessDialog()
        {
            InitializeComponent();
        }

        // Developer LinkedIn Link
        private void devLinkedInlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.devLinkedInlinkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/jonahpickett/");
        }

        // Conway's Game of Life Wiki Link
        private void conwayGOLWikiLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.conwayGOLWikiLinkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life");
        }

        // Life Lexicon Link
        private void lifeLexiconLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.lifeLexiconLinkLabel.LinkVisited = true;
            System.Diagnostics.Process.Start("https://bitstorm.org/gameoflife/lexicon/");
        }
    }
}
