
namespace Game_of_Life___Jonah_Pickett
{
    partial class RandomSeedModalDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RandomSeedModalDialog));
            this.button1OK = new System.Windows.Forms.Button();
            this.button1Cancel = new System.Windows.Forms.Button();
            this.numericUpDownSeed = new System.Windows.Forms.NumericUpDown();
            this.button1Randomize = new System.Windows.Forms.Button();
            this.SeedLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSeed)).BeginInit();
            this.SuspendLayout();
            // 
            // button1OK
            // 
            this.button1OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1OK.Location = new System.Drawing.Point(42, 61);
            this.button1OK.Name = "button1OK";
            this.button1OK.Size = new System.Drawing.Size(75, 23);
            this.button1OK.TabIndex = 0;
            this.button1OK.Text = "OK";
            this.button1OK.UseVisualStyleBackColor = true;
            // 
            // button1Cancel
            // 
            this.button1Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1Cancel.Location = new System.Drawing.Point(164, 61);
            this.button1Cancel.Name = "button1Cancel";
            this.button1Cancel.Size = new System.Drawing.Size(75, 23);
            this.button1Cancel.TabIndex = 1;
            this.button1Cancel.Text = "Cancel";
            this.button1Cancel.UseVisualStyleBackColor = true;
            // 
            // numericUpDownSeed
            // 
            this.numericUpDownSeed.Location = new System.Drawing.Point(64, 21);
            this.numericUpDownSeed.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownSeed.Name = "numericUpDownSeed";
            this.numericUpDownSeed.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownSeed.TabIndex = 2;
            this.numericUpDownSeed.Value = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            // 
            // button1Randomize
            // 
            this.button1Randomize.Location = new System.Drawing.Point(201, 18);
            this.button1Randomize.Name = "button1Randomize";
            this.button1Randomize.Size = new System.Drawing.Size(75, 23);
            this.button1Randomize.TabIndex = 3;
            this.button1Randomize.Text = "Randomize";
            this.button1Randomize.UseVisualStyleBackColor = true;
            this.button1Randomize.Click += new System.EventHandler(this.button1Randomize_Click);
            // 
            // SeedLabel
            // 
            this.SeedLabel.AutoSize = true;
            this.SeedLabel.Location = new System.Drawing.Point(23, 23);
            this.SeedLabel.Name = "SeedLabel";
            this.SeedLabel.Size = new System.Drawing.Size(35, 13);
            this.SeedLabel.TabIndex = 4;
            this.SeedLabel.Text = "Seed:";
            // 
            // RandomSeedModalDialog
            // 
            this.AcceptButton = this.button1OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1Cancel;
            this.ClientSize = new System.Drawing.Size(288, 105);
            this.Controls.Add(this.SeedLabel);
            this.Controls.Add(this.button1Randomize);
            this.Controls.Add(this.numericUpDownSeed);
            this.Controls.Add(this.button1Cancel);
            this.Controls.Add(this.button1OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RandomSeedModalDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Randomize From Seed";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1OK;
        private System.Windows.Forms.Button button1Cancel;
        private System.Windows.Forms.NumericUpDown numericUpDownSeed;
        private System.Windows.Forms.Button button1Randomize;
        private System.Windows.Forms.Label SeedLabel;
    }
}