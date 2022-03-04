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

    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[5, 5];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        // Living Cells count
        int livingCells = 0;

        // Initial 'seed' variable & Random # Generator
        Random rng = new Random();
        int seed = 0;


        public Form1()
        {
            InitializeComponent(); // constructor; calling designer code

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            // Create scratchPad
            bool[,] scratchPad = new bool[universe.GetLength(0), universe.GetLength(1)];

            for ( int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for(int x = 0; x < universe.GetLength(0); x++)
                {
                    int count = CountNeighbors(x, y);

                    // Apply the rules & turn on/off in scratchPad
                    if (universe[x, y] == true && count < 2) // Rule #1
                    {
                        scratchPad[x, y] = false;
                    }
                    else if (universe[x, y] == true && (count == 2 || count == 3)) // Rule #3
                    {
                        scratchPad[x, y] = true;
                    }
                    else if (universe[x, y] == true && count > 3) // Rule #2
                    {
                        scratchPad[x, y] = false;
                    }
                    else if (universe[x, y] == false && count == 3) // Rule #4
                    {
                        scratchPad[x, y] = true;
                    }
                }
            }

            // Copy from scratchPad to universe; swap
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            // Update status strip living cells
            livingCells = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (universe[x, y] == true)
                        livingCells++;
                }
            }
            toolStripStatusLabelAlive.Text = "Alive = " + livingCells.ToString();

            // Invalidate graphics panel
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Update to display current seed
            if (seed == 0) seed = rng.Next(); // initial random seed to start 
            seedToolStripStatusLabel1.Text = "Seed = " + seed.ToString();

            // FLOATS!!!
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    if(gridToolStripMenuItem.Checked == true) // check if view option is checked
                    {
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }
                    

                    // Displaying Neighbor Count
                    if(neighborCountToolStripMenuItem.Checked == true)
                    {
                        int neighbors = CountNeighbors(x, y);
                        if (neighbors != 0)
                        {
                            Font font = new Font("Arial", 20f);

                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Center;
                            stringFormat.LineAlignment = StringAlignment.Center;

                            e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);
                        }
                    }             
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // FLOATS!!!
                // Calculate the width and height of each cell in pixels
                float cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Update living cells count
                if (universe[x, y] == true) // toggle on
                    livingCells++;
                else if (universe[x, y] == false) // toggle off
                    livingCells--;
                toolStripStatusLabelAlive.Text = "Alive = " + livingCells.ToString();

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate(); // used after every click, every new generation, after "new", turnthings on/off, etc;
                // NEVER put 'Invalidate()' in your 'Paint'
            }
        }

        // File -> Exit button (click event)
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes the program
        }

        // Play button (click event)
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        // Pause button (click event)
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        // Next button (click event)
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            NextGeneration(); // advance one generation
        }

        // File -> New button (click event)
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {        
            // Generate blank universe
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            
            timer.Enabled = false; // stops generation count 

            // resets generation count to 0
            generations = 0; 
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            // resets living cells count to 0 
            livingCells = 0;          
            toolStripStatusLabelAlive.Text = "Alive = " + livingCells.ToString();

            // refresh to display update
            graphicsPanel1.Invalidate();
        }       

        // Counting Neighbors
        private int CountNeighbors(int x, int y)
        {
            int neighbors = 0;

            // Determine whether toroidal/finite is toggled on/off, and applies appropriate CountNeighbors() function
            if(toroidalToolStripMenuItem.Checked == true)
            {
                neighbors = CountNeighborsToroidal(x, y);
            }
            else if (finiteToolStripMenuItem.Checked == true)
            {
                neighbors = CountNeighborsFinite(x, y);
            }

            return neighbors;
        }
        // Finite
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            for(int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if(xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    if(xCheck < 0)
                    {
                        continue;
                    }
                    if ( yCheck < 0)
                    {
                        continue;
                    }
                    if(xCheck >= xLen)
                    {
                        continue;
                    }
                    if(yCheck >= yLen)
                    {
                        continue;
                    }

                    // check to see if neighboring cell is alive, and add to count
                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        // Toroidal
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    // check to see if neighboring cell is alive, and add to count
                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        // View -> HUD
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        // View -> Toroidal button
        private void toroidalToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            bool toroidalChecked = toroidalToolStripMenuItem.Checked;
            // Toggle finite view based on whether toroidal is checked (mutually exclusive)
            if (finiteToolStripMenuItem.Checked == true && toroidalChecked == true)
            {
                finiteToolStripMenuItem.Checked = false;
            }
            else if (finiteToolStripMenuItem.Checked == false && toroidalChecked == false)
            {
                finiteToolStripMenuItem.Checked = true;
            }

            // Refresh screen to update
            graphicsPanel1.Invalidate();
        }
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toroidalToolStripMenuItem.Checked = !toroidalToolStripMenuItem.Checked; // toggle toroidal view
        }

        // View -> Finite button
        private void finiteToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            bool finiteChecked = finiteToolStripMenuItem.Checked;
            // Toggle toroidal view based on whether finite is checked (mutually exclusive)
            if(finiteChecked == true && toroidalToolStripMenuItem.Checked == true)
            {
                toroidalToolStripMenuItem.Checked = false;
            }
            else if (finiteChecked == false && toroidalToolStripMenuItem.Checked == false)
            {
                toroidalToolStripMenuItem.Checked = true;
            }

            // Refresh screen to update
            graphicsPanel1.Invalidate();
        }
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            finiteToolStripMenuItem.Checked = !finiteToolStripMenuItem.Checked; // toggle fintie view
        }

        // Initial Randomize Universe
        private void RandomizeUniverse()
        {
            rng = new Random(seed); // use current seed variable
            
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // Generate random # (0-2) to see if that cell will be alive/dead
                    int number = rng.Next(0, 3);
                    if (number == 0)
                    {
                        universe[x, y] = true;
                    }
                    else
                        universe[x, y] = false;                        
                }
            }

            // Refresh screen
            graphicsPanel1.Invalidate();
        }

        // Randomize Universe by Time
        private void RandomizeUniverseByTime()
        {
            //Random # Generator - based off current time in ticks
            seed = (int)DateTime.Now.Ticks;
            rng = new Random(seed);

            // Loop through universe
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // Generate random # (0-2) to see if that cell will be alive/dead
                    int number = rng.Next(0, 3);
                    if (number == 0)
                    {
                        universe[x, y] = true;
                    }
                    else
                        universe[x, y] = false;
                }
            }

            // Refresh screen to update
            graphicsPanel1.Invalidate();
        }

        // Randomize -> From Time button
        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomizeUniverseByTime();
        }

        // View -> Counting Numbers button
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            neighborCountToolStripMenuItem1.Checked = !neighborCountToolStripMenuItem1.Checked; // Context menu strip (View -> Neighbor Count button)
            neighborCountToolStripMenuItem.Checked = !neighborCountToolStripMenuItem.Checked; // toggle neighbor count
            graphicsPanel1.Invalidate();
        }

        // View -> Grid button
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridToolStripMenuItem1.Checked = !gridToolStripMenuItem1.Checked; // context menu strip (View -> Grid button)
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked; // toggle grid
            graphicsPanel1.Invalidate();
        }

        // Randomize -> From Current Seed button
        private void fromCurrentSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomizeUniverse();
        }
    }
}
