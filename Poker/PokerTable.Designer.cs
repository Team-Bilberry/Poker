namespace Poker
{
    using System.Windows.Forms;

    partial class PokerTable
    {
        private Button bFold;
        private Button bCheck;
        private Button bCall;
        private Button bRaise;
        private ProgressBar pbTimer;
        private TextBox tbChips;
        private Button bAdd;
        private TextBox tbAddChips;
        private TextBox tbBotChips5;
        private TextBox tbBotChips4;
        private TextBox tbBotChips3;
        private TextBox tbBotChips2;
        private TextBox tbBotChips1;
        private TextBox tbPot;
        private Button bOptions;
        private Button bBigBlind;
        private TextBox tbSmallBlind;
        private Button bSmallBlind;
        private TextBox tbBigBlind;
        private Label b5Status;
        private Label b4Status;
        private Label b3Status;
        private Label bot1Status;
        private Label playerStatus;
        private Label b2Status;
        private Label label1;
        private TextBox tbRaise;
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
            this.bFold = new Button();
            this.bCheck = new Button();
            this.bCall = new Button();
            this.bRaise = new Button();
            this.pbTimer = new ProgressBar();
            this.tbChips = new TextBox();
            this.bAdd = new Button();
            this.tbAddChips = new TextBox();
            this.tbBotChips5 = new TextBox();
            this.tbBotChips4 = new TextBox();
            this.tbBotChips3 = new TextBox();
            this.tbBotChips2 = new TextBox();
            this.tbBotChips1 = new TextBox();
            this.tbPot = new TextBox();
            this.bOptions = new Button();
            this.bBigBlind = new Button();
            this.tbSmallBlind = new TextBox();
            this.bSmallBlind = new Button();
            this.tbBigBlind = new TextBox();
            this.b5Status = new Label();
            this.b4Status = new Label();
            this.b3Status = new Label();
            this.bot1Status = new Label();
            this.playerStatus = new Label();
            this.b2Status = new Label();
            this.label1 = new Label();
            this.tbRaise = new TextBox();
            this.SuspendLayout();
            // 
            // bFold
            // 
            this.bFold.Anchor = AnchorStyles.Bottom;
            this.bFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bFold.Location = new System.Drawing.Point(335, 660);
            this.bFold.Name = "bFold";
            this.bFold.Size = new System.Drawing.Size(130, 62);
            this.bFold.TabIndex = 0;
            this.bFold.Text = "Fold";
            this.bFold.UseVisualStyleBackColor = true;
            this.bFold.Click += new System.EventHandler(this.bFold_Click);
            // 
            // bCheck
            // 
            this.bCheck.Anchor = AnchorStyles.Bottom;
            this.bCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCheck.Location = new System.Drawing.Point(494, 660);
            this.bCheck.Name = "bCheck";
            this.bCheck.Size = new System.Drawing.Size(134, 62);
            this.bCheck.TabIndex = 2;
            this.bCheck.Text = "Check";
            this.bCheck.UseVisualStyleBackColor = true;
            this.bCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // bCall
            // 
            this.bCall.Anchor = AnchorStyles.Bottom;
            this.bCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCall.Location = new System.Drawing.Point(667, 661);
            this.bCall.Name = "bCall";
            this.bCall.Size = new System.Drawing.Size(126, 62);
            this.bCall.TabIndex = 3;
            this.bCall.Text = "Call";
            this.bCall.UseVisualStyleBackColor = true;
            this.bCall.Click += new System.EventHandler(this.bCall_Click);
            // 
            // bRaise
            // 
            this.bRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bRaise.Location = new System.Drawing.Point(835, 661);
            this.bRaise.Name = "bRaise";
            this.bRaise.Size = new System.Drawing.Size(124, 62);
            this.bRaise.TabIndex = 4;
            this.bRaise.Text = "Raise";
            this.bRaise.UseVisualStyleBackColor = true;
            this.bRaise.Click += new System.EventHandler(this.bRaise_Click);
            // 
            // pbTimer
            // 
            this.pbTimer.Anchor = AnchorStyles.Bottom;
            this.pbTimer.BackColor = System.Drawing.SystemColors.Control;
            this.pbTimer.Location = new System.Drawing.Point(335, 631);
            this.pbTimer.Maximum = 1000;
            this.pbTimer.Name = "pbTimer";
            this.pbTimer.Size = new System.Drawing.Size(667, 23);
            this.pbTimer.TabIndex = 5;
            this.pbTimer.Value = 1000;
            // 
            // tbChips
            // 
            this.tbChips.Anchor = AnchorStyles.Bottom;
            this.tbChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbChips.Location = new System.Drawing.Point(755, 553);
            this.tbChips.Name = "tbChips";
            this.tbChips.Size = new System.Drawing.Size(163, 23);
            this.tbChips.TabIndex = 6;
            this.tbChips.Text = "Chips : 0";
            // 
            // bAdd
            // 
            this.bAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.bAdd.Location = new System.Drawing.Point(12, 697);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 25);
            this.bAdd.TabIndex = 7;
            this.bAdd.Text = "AddChips";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAddChips_Click);
            // 
            // tbAddChips
            // 
            this.tbAddChips.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.tbAddChips.Location = new System.Drawing.Point(93, 700);
            this.tbAddChips.Name = "tbAddChips";
            this.tbAddChips.Size = new System.Drawing.Size(125, 20);
            this.tbAddChips.TabIndex = 8;
            // 
            // tbBotChips5
            // 
            this.tbBotChips5.Anchor = AnchorStyles.Bottom | AnchorStyles.None | AnchorStyles.Right;
            this.tbBotChips5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips5.Location = new System.Drawing.Point(1012, 553);
            this.tbBotChips5.Name = "tbBotChips5";
            this.tbBotChips5.Size = new System.Drawing.Size(152, 23);
            this.tbBotChips5.TabIndex = 9;
            this.tbBotChips5.Text = "Chips : 0";
            // 
            // tbBotChips4
            // 
            this.tbBotChips4.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.tbBotChips4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips4.Location = new System.Drawing.Point(970, 81);
            this.tbBotChips4.Name = "tbBotChips4";
            this.tbBotChips4.Size = new System.Drawing.Size(123, 23);
            this.tbBotChips4.TabIndex = 10;
            this.tbBotChips4.Text = "Chips : 0";
            // 
            // tbBotChips3
            // 
            this.tbBotChips3.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.tbBotChips3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips3.Location = new System.Drawing.Point(755, 81);
            this.tbBotChips3.Name = "tbBotChips3";
            this.tbBotChips3.Size = new System.Drawing.Size(125, 23);
            this.tbBotChips3.TabIndex = 11;
            this.tbBotChips3.Text = "Chips : 0";
            // 
            // tbBotChips2
            // 
            this.tbBotChips2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips2.Location = new System.Drawing.Point(276, 81);
            this.tbBotChips2.Name = "tbBotChips2";
            this.tbBotChips2.Size = new System.Drawing.Size(133, 23);
            this.tbBotChips2.TabIndex = 12;
            this.tbBotChips2.Text = "Chips : 0";
            // 
            // tbBotChips1
            // 
            this.tbBotChips1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.tbBotChips1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBotChips1.Location = new System.Drawing.Point(181, 553);
            this.tbBotChips1.Name = "tbBotChips1";
            this.tbBotChips1.Size = new System.Drawing.Size(142, 23);
            this.tbBotChips1.TabIndex = 13;
            this.tbBotChips1.Text = "Chips : 0";
            // 
            // tbPot
            // 
            this.tbPot.Anchor = AnchorStyles.None;
            this.tbPot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPot.Location = new System.Drawing.Point(606, 212);
            this.tbPot.Name = "tbPot";
            this.tbPot.Size = new System.Drawing.Size(125, 23);
            this.tbPot.TabIndex = 14;
            this.tbPot.Text = "0";
            // 
            // bOptions
            // 
            this.bOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bOptions.Location = new System.Drawing.Point(12, 12);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(75, 36);
            this.bOptions.TabIndex = 15;
            this.bOptions.Text = "BB/SB";
            this.bOptions.UseVisualStyleBackColor = true;
            this.bOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // bBigBlind
            // 
            this.bBigBlind.Location = new System.Drawing.Point(12, 254);
            this.bBigBlind.Name = "bBigBlind";
            this.bBigBlind.Size = new System.Drawing.Size(75, 23);
            this.bBigBlind.TabIndex = 16;
            this.bBigBlind.Text = "Big Blind";
            this.bBigBlind.UseVisualStyleBackColor = true;
            this.bBigBlind.Click += new System.EventHandler(this.bBigBlind_Click);
            // 
            // tbSmallBlind
            // 
            this.tbSmallBlind.Location = new System.Drawing.Point(12, 228);
            this.tbSmallBlind.Name = "tbSmallBlind";
            this.tbSmallBlind.Size = new System.Drawing.Size(75, 20);
            this.tbSmallBlind.TabIndex = 17;
            this.tbSmallBlind.Text = "250";
            // 
            // bSmallBlind
            // 
            this.bSmallBlind.Location = new System.Drawing.Point(12, 199);
            this.bSmallBlind.Name = "bSmallBlind";
            this.bSmallBlind.Size = new System.Drawing.Size(75, 23);
            this.bSmallBlind.TabIndex = 18;
            this.bSmallBlind.Text = "Small Blind";
            this.bSmallBlind.UseVisualStyleBackColor = true;
            this.bSmallBlind.Click += new System.EventHandler(this.bSmallBlind_Click);
            // 
            // tbBigBlind
            // 
            this.tbBigBlind.Location = new System.Drawing.Point(12, 283);
            this.tbBigBlind.Name = "tbBigBlind";
            this.tbBigBlind.Size = new System.Drawing.Size(75, 20);
            this.tbBigBlind.TabIndex = 19;
            this.tbBigBlind.Text = "500";
            // 
            // b5Status
            // 
            this.b5Status.Anchor = AnchorStyles.Bottom | AnchorStyles.None | AnchorStyles.Right;
            this.b5Status.Location = new System.Drawing.Point(1012, 579);
            this.b5Status.Name = "b5Status";
            this.b5Status.Size = new System.Drawing.Size(152, 32);
            this.b5Status.TabIndex = 26;
            // 
            // b4Status
            // 
            this.b4Status.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.b4Status.Location = new System.Drawing.Point(970, 107);
            this.b4Status.Name = "b4Status";
            this.b4Status.Size = new System.Drawing.Size(123, 32);
            this.b4Status.TabIndex = 27;
            // 
            // b3Status
            // 
            this.b3Status.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.b3Status.Location = new System.Drawing.Point(755, 107);
            this.b3Status.Name = "b3Status";
            this.b3Status.Size = new System.Drawing.Size(125, 32);
            this.b3Status.TabIndex = 28;
            // 
            // bot1Status
            // 
            this.bot1Status.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.bot1Status.Location = new System.Drawing.Point(181, 579);
            this.bot1Status.Name = "bot1Status";
            this.bot1Status.Size = new System.Drawing.Size(142, 32);
            this.bot1Status.TabIndex = 29;
            // 
            // playerStatus
            // 
            this.playerStatus.Anchor = AnchorStyles.Bottom;
            this.playerStatus.Location = new System.Drawing.Point(755, 579);
            this.playerStatus.Name = "playerStatus";
            this.playerStatus.Size = new System.Drawing.Size(163, 32);
            this.playerStatus.TabIndex = 30;
            // 
            // b2Status
            // 
            this.b2Status.Location = new System.Drawing.Point(276, 107);
            this.b2Status.Name = "b2Status";
            this.b2Status.Size = new System.Drawing.Size(133, 32);
            this.b2Status.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.Anchor = AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(654, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pot";
            // 
            // tbRaise
            // 
            this.tbRaise.Anchor = AnchorStyles.Bottom;
            this.tbRaise.Location = new System.Drawing.Point(965, 703);
            this.tbRaise.Name = "tbRaise";
            this.tbRaise.Size = new System.Drawing.Size(108, 20);
            this.tbRaise.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.pokerTable;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.tbRaise);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b2Status);
            this.Controls.Add(this.playerStatus);
            this.Controls.Add(this.bot1Status);
            this.Controls.Add(this.b3Status);
            this.Controls.Add(this.b4Status);
            this.Controls.Add(this.b5Status);
            this.Controls.Add(this.tbBigBlind);
            this.Controls.Add(this.bSmallBlind);
            this.Controls.Add(this.tbSmallBlind);
            this.Controls.Add(this.bBigBlind);
            this.Controls.Add(this.bOptions);
            this.Controls.Add(this.tbPot);
            this.Controls.Add(this.tbBotChips1);
            this.Controls.Add(this.tbBotChips2);
            this.Controls.Add(this.tbBotChips3);
            this.Controls.Add(this.tbBotChips4);
            this.Controls.Add(this.tbBotChips5);
            this.Controls.Add(this.tbAddChips);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.tbChips);
            this.Controls.Add(this.pbTimer);
            this.Controls.Add(this.bRaise);
            this.Controls.Add(this.bCall);
            this.Controls.Add(this.bCheck);
            this.Controls.Add(this.bFold);
            this.DoubleBuffered = true;
            this.Name = "PokerTable";
            this.Text = "GLS Texas Poker";
            this.Layout += new LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

