using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // used for Save/Open files



namespace Game_of_Life___Jonah_Pickett
{

    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[30, 30];

        // Drawing colors
        Color gridColor = Color.Gray;
        Color cellColor = Color.LightGray;
        Color gridX10Color = Color.Black;

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

            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor; // import background color settings
            gridColor = Properties.Settings.Default.GridColor; // import grid color settings
            cellColor = Properties.Settings.Default.CellColor; // import cell color settings
            gridX10Color = Properties.Settings.Default.GridX10Color; // import grid X10 settings
            // import universe size settings
            int uWidth = Properties.Settings.Default.UniverseWidth;
            int uHeight = Properties.Settings.Default.UniverseHeight;
            universe = new bool[uWidth, uHeight]; 

            // Setup the timer
            timer.Interval = Properties.Settings.Default.TimerInterval; // milliseconds; imported from settings
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
                        scratchPad[x, y] = false; // cell dies
                    }
                    else if (universe[x, y] == true && (count == 2 || count == 3)) // Rule #3
                    {
                        scratchPad[x, y] = true; // cell lives
                    }
                    else if (universe[x, y] == true && count > 3) // Rule #2
                    {
                        scratchPad[x, y] = false; // cell dies
                    }
                    else if (universe[x, y] == false && count == 3) // Rule #4
                    {
                        scratchPad[x, y] = true; // cell is born
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

            // Update to display current interval
            intervalToolStripStatusLabel.Text = "Interval: " + timer.Interval.ToString();

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
                    

                    // Displaying Color-Coded Neighbor Count
                    if(neighborCountToolStripMenuItem.Checked == true)
                    {
                        int neighbors = CountNeighbors(x, y);
                        if (neighbors != 0)
                        {
                            Font font = new Font("Arial", (cellHeight/2));                

                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Center;
                            stringFormat.LineAlignment = StringAlignment.Center;

                            // Color-Coding
                            if (universe[x, y] == true && neighbors < 2) // Rule #1
                            {
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Red, cellRect, stringFormat); // Red color since cell dies
                            }
                            else if (universe[x, y] == true && (neighbors == 2 || neighbors == 3)) // Rule #3
                            {
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Green, cellRect, stringFormat); // Green color since cell lives
                            }
                            else if (universe[x, y] == true && neighbors > 3) // Rule #2
                            {
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Red, cellRect, stringFormat); // Red color since cell dies
                            }
                            else if (universe[x, y] == false && neighbors == 3) // Rule #4
                            {
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Green, cellRect, stringFormat); // Green color since cell lives
                            }
                            else e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Red, cellRect, stringFormat);
                        }
                    }             
                }
            }

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

            // Display Grid X10
            if ((universe.GetLength(0) % 10) == 0 && (universe.GetLength(1) % 10) == 0) // check if universe width & height are divisible by 10
            {
                // new pen for grid x10
                Pen gridX10Pen = new Pen(gridX10Color, 2);                

                // iterate through universe
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // Create gridX10 rectangle
                        RectangleF gridX10Rect = RectangleF.Empty;
                        gridX10Rect.X = x * (cellWidth * 10);
                        gridX10Rect.Y = y * (cellHeight * 10);
                        gridX10Rect.Width = cellWidth * 10;
                        gridX10Rect.Height = cellHeight * 10;

                        // Draw outline 
                        e.Graphics.DrawRectangle(gridX10Pen, gridX10Rect.X, gridX10Rect.Y, gridX10Rect.Width, gridX10Rect.Height);                      
                    }
                }

                // Clean up pen
                gridX10Pen.Dispose();
            }

            // Display HUD
            if(HUD.Checked == true)
            {
                Font font = new Font("Arial", 13f, FontStyle.Bold);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Far;

                // HUD data
                string HUDString = "Generations: " + generations.ToString() + "\n" + "Living Cells: " + livingCells.ToString() + "\n" + "Universe Size: {Width: " + universe.GetLength(0).ToString() + ", Height: " + universe.GetLength(1) + "}";
                
                // check to see if boundary is toroidal/finite & append to HUD data
                if (toroidalToolStripMenuItem.Checked == true)
                    HUDString += "\nBoundary Type: Toroidal";
                else if(finiteToolStripMenuItem.Checked == true)
                    HUDString += "\nBoundary Type: Finite";

                // Display HUD
                e.Graphics.DrawString(HUDString, font, Brushes.DarkCyan, graphicsPanel1.ClientRectangle, stringFormat);
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
                float x = (e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = (e.Y / cellHeight);

                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];              

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
            HUD.Checked = !HUD.Checked;
            hUDToolStripMenuItem.Checked = !hUDToolStripMenuItem.Checked;
            graphicsPanel1.Invalidate();
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

        // Randomize -> From Current Seed button
        private void fromCurrentSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomizeUniverse();
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

        // File -> Save As button    
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!Universe Save File " + DateTime.Now);
                writer.WriteLine("!\'O\' = Living cell");
                writer.WriteLine("!\'.\' = Dead cell");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O) to the row string.
                        if (universe[x, y] == true) currentRow += 'O';

                        // Else if the universe[x,y] is dead then append '.' (period) to the string.
                        else if (universe[x, y] == false) currentRow += '.';
                    }

                    // Once the current row has been read through and the string constructed, then write it to the file using WriteLine.
                    writer.WriteLine(currentRow); 
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        // File -> Open button
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height of the data in the file
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment and should be ignored.
                    if (row[0] == '!') continue;

                    // If the row is not a comment then it is a row of cells. Increment the maxHeight variable for each row read.
                    else
                    {
                        maxHeight++;

                        // Get the length of the current row string
                        // and adjust the maxWidth variable if necessary.
                        maxWidth = row.Length;
                    }
                }

                // Resize the current universe and scratchPad to the width and height of the file calculated above.
                universe = new bool[maxWidth, maxHeight];
                int yPos = 0; // for accessing y-position of cell in universe

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();
                    

                    // If the row begins with '!' then it is a comment and should be ignored.
                    if (row[0] == '!') continue;

                    // If the row is not a comment then it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then set the corresponding cell in the universe to alive. 
                        if (row[xPos] == 'O') universe[xPos, yPos] = true;

                        // If row[xPos] is a '.' (period) then set the corresponding cell in the universe to dead.
                        else if (row[xPos] == '.') universe[xPos, yPos] = false;
                    }
                    yPos++; // increase yPos for every new row
                }

                // Close the file.
                reader.Close();
            }

            // Refresh display to update changes
            graphicsPanel1.Invalidate();
        }

        // Settings -> Back Color button
        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate color dialog box
            ColorDialog dlg = new ColorDialog();

            // set dialog box to current background color
            dlg.Color = graphicsPanel1.BackColor;

            // if a color is accepted, change background color
            if(DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }

            // Refresh screen to display updates
            graphicsPanel1.Invalidate();
        }

        // Settings -> Cell Color button
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate color dialog box
            ColorDialog dlg = new ColorDialog();

            // set dialog box to current cell color
            dlg.Color = cellColor;

            // if a color is accepted, change cell color
            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
            }

            // Refresh screen to display updates
            graphicsPanel1.Invalidate();
        }

        // Settings -> Grid Color button
        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate  color dialog box
            ColorDialog dlg = new ColorDialog();

            // set dialog box to current grid color
            dlg.Color = gridColor;

            // if a color is accepted, change grid color
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
            }

            // Refresh screen to display updates
            graphicsPanel1.Invalidate();
        }

        // Settings -> Grid X10 Color button
        private void gridX10ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // instantiate  color dialog box
            ColorDialog dlg = new ColorDialog();

            // set dialog box to current grid X10 color
            dlg.Color = gridX10Color;

            // if a color is accepted, change grid X10 color
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridX10Color = dlg.Color;
            }

            // Refresh screen to display updates
            graphicsPanel1.Invalidate();
        }

        // Settings -> Options Dialog Box
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //instantiate dialog box
            OptionsModalDialog dlg = new OptionsModalDialog();

            // set initial values in dialog box to current timer interval, universe width & height
            dlg.OptionsInterval = timer.Interval;
            dlg.OptionsWidth = universe.GetLength(0);
            dlg.OptionsHeight = universe.GetLength(1);

            // if changes are accepted, execute changes
            if (DialogResult.OK == dlg.ShowDialog())
            {
                timer.Interval = dlg.OptionsInterval; // change timer interval
                universe = new bool[dlg.OptionsWidth, dlg.OptionsHeight]; // re-size universe
            }

            // Refresh display to update
            graphicsPanel1.Invalidate();
        }

        // Randomize -> From Seed dialog box
        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomSeedModalDialog dlg = new RandomSeedModalDialog();

            // set dialog box to current seed
            dlg.SeedNumericUpDown = seed;

            // if a color is accepted, change seed & randomize universe
            if (DialogResult.OK == dlg.ShowDialog())
            {
                seed = dlg.SeedNumericUpDown;
                RandomizeUniverse();
            }
        }

        // File -> Import button
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                int yPos = 0; // for accessing y-position of cell in universe

                // Iterate through the file, reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment and should be ignored.
                    if (row[0] == '!') continue;

                    // If the row is not a comment then it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then set the corresponding cell in the universe to alive. 
                        if (row[xPos] == 'O') universe[xPos, yPos] = true;

                        // If row[xPos] is a '.' (period) then set the corresponding cell in the universe to dead.
                        else if (row[xPos] == '.') universe[xPos, yPos] = false;
                    }
                    yPos++; // increase yPos for every new row
                }

                // Close the file.
                reader.Close();
            }

            // Refresh display to update changes
            graphicsPanel1.Invalidate();
        }

        // Update Settings Properties once form is closed
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.GridX10Color = gridX10Color;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.TimerInterval = timer.Interval;
            Properties.Settings.Default.UniverseWidth = universe.GetLength(0);
            Properties.Settings.Default.UniverseHeight = universe.GetLength(1);
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;

            Properties.Settings.Default.Save(); // saves settings
        }

        // Settings -> Reset button
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset(); // revert back to default program settings

            // import properties
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor; 
            cellColor = Properties.Settings.Default.CellColor; 
            gridX10Color = Properties.Settings.Default.GridX10Color; 
            timer.Interval = Properties.Settings.Default.TimerInterval;
            // import universe size settings
            int uWidth = Properties.Settings.Default.UniverseWidth;
            int uHeight = Properties.Settings.Default.UniverseHeight;
            universe = new bool[uWidth, uHeight];
        }

        // Settings -> Reload button
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload(); // reload to last saved settings

            // import properties
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridX10Color = Properties.Settings.Default.GridX10Color;
            timer.Interval = Properties.Settings.Default.TimerInterval;
            // import universe size settings
            int uWidth = Properties.Settings.Default.UniverseWidth;
            int uHeight = Properties.Settings.Default.UniverseHeight;
            universe = new bool[uWidth, uHeight];
        }

        // Run -> To button
        private void toToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //instantiate dialog box
            ToModalDialog dlg = new ToModalDialog();

            // set to current generation
            dlg.GenerationNumericUpDown = generations;
            dlg.GenNumUpDownMin = generations; // sets minimum value to current generation

            // if changes are accepted, execute changes
            if (DialogResult.OK == dlg.ShowDialog())
            {
                int userInput = dlg.GenerationNumericUpDown;
                int genAdvance = userInput - generations;
                for(int i = 0; i < genAdvance; i++)
                {
                    NextGeneration();
                }
            }
        }
    }
}
