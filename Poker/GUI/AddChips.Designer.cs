namespace Poker.GUI
{
    partial class AddChips
    {
        private System.Windows.Forms.Label outOfChipsLabel;
        private System.Windows.Forms.Button addMoreChipsButton;
        private System.Windows.Forms.Button exitAddChipsMenuButton;
        private System.Windows.Forms.TextBox addMoreChipsAmount;

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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.outOfChipsLabel = new System.Windows.Forms.Label();
            this.addMoreChipsButton = new System.Windows.Forms.Button();
            this.exitAddChipsMenuButton = new System.Windows.Forms.Button();
            this.addMoreChipsAmount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // outOfChipsLabel
            // 
            this.outOfChipsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.outOfChipsLabel.Location = new System.Drawing.Point(48, 49);
            this.outOfChipsLabel.Name = "outOfChipsLabel";
            this.outOfChipsLabel.Size = new System.Drawing.Size(176, 23);
            this.outOfChipsLabel.TabIndex = 0;
            this.outOfChipsLabel.Text = "You ran out of chips!";
            this.outOfChipsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addChipsButton
            // 
            this.addMoreChipsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addMoreChipsButton.Location = new System.Drawing.Point(12, 226);
            this.addMoreChipsButton.Name = "addChipsButton";
            this.addMoreChipsButton.Size = new System.Drawing.Size(75, 23);
            this.addMoreChipsButton.TabIndex = 1;
            this.addMoreChipsButton.Text = "Add Chips";
            this.addMoreChipsButton.UseVisualStyleBackColor = true;
            this.addMoreChipsButton.Click += new System.EventHandler(this.bAddChips_Click);
            // 
            // exitButton
            // 
            this.exitAddChipsMenuButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exitAddChipsMenuButton.Location = new System.Drawing.Point(197, 226);
            this.exitAddChipsMenuButton.Name = "exitButton";
            this.exitAddChipsMenuButton.Size = new System.Drawing.Size(75, 23);
            this.exitAddChipsMenuButton.TabIndex = 2;
            this.exitAddChipsMenuButton.Text = "Exit";
            this.exitAddChipsMenuButton.UseVisualStyleBackColor = true;
            this.exitAddChipsMenuButton.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tbAddChips
            // 
            this.addMoreChipsAmount.Location = new System.Drawing.Point(91, 229);
            this.addMoreChipsAmount.Name = "tbAddChips";
            this.addMoreChipsAmount.Size = new System.Drawing.Size(100, 20);
            this.addMoreChipsAmount.TabIndex = 3;
            // 
            // AddChips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.addMoreChipsAmount);
            this.Controls.Add(this.exitAddChipsMenuButton);
            this.Controls.Add(this.addMoreChipsButton);
            this.Controls.Add(this.outOfChipsLabel);
            this.Name = "AddChips";
            this.Text = "You Ran Out Of Chips";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}