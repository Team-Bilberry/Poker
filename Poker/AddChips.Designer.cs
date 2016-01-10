namespace Poker
{
    partial class AddChips
    {
        private System.Windows.Forms.Label lblOutOfChips;
        private System.Windows.Forms.Button bAddChips;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.TextBox tbAddChips;

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
            this.lblOutOfChips = new System.Windows.Forms.Label();
            this.bAddChips = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.tbAddChips = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblOutOfChips
            // 
            this.lblOutOfChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblOutOfChips.Location = new System.Drawing.Point(48, 49);
            this.lblOutOfChips.Name = "lblOutOfChips";
            this.lblOutOfChips.Size = new System.Drawing.Size(176, 23);
            this.lblOutOfChips.TabIndex = 0;
            this.lblOutOfChips.Text = "You ran out of chips!";
            this.lblOutOfChips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bAddChips
            // 
            this.bAddChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bAddChips.Location = new System.Drawing.Point(12, 226);
            this.bAddChips.Name = "bAddChips";
            this.bAddChips.Size = new System.Drawing.Size(75, 23);
            this.bAddChips.TabIndex = 1;
            this.bAddChips.Text = "Add Chips";
            this.bAddChips.UseVisualStyleBackColor = true;
            this.bAddChips.Click += new System.EventHandler(this.bAddChips_Click);
            // 
            // bExit
            // 
            this.bExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bExit.Location = new System.Drawing.Point(197, 226);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(75, 23);
            this.bExit.TabIndex = 2;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tbAddChips
            // 
            this.tbAddChips.Location = new System.Drawing.Point(91, 229);
            this.tbAddChips.Name = "tbAddChips";
            this.tbAddChips.Size = new System.Drawing.Size(100, 20);
            this.tbAddChips.TabIndex = 3;
            // 
            // AddChips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tbAddChips);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bAddChips);
            this.Controls.Add(this.lblOutOfChips);
            this.Name = "AddChips";
            this.Text = "You Ran Out Of Chips";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}