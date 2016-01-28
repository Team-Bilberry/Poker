namespace Poker.GUI
{
    using System.Windows.Forms;

    partial class PokerTable
    {
        private Button foldButton;
        private Button checkButton;
        private Button callButton;
        private Button raiseButton;
        private ProgressBar pbTimer;
        private TextBox playerChips;
        private Button addChipsToAllButton;
        private TextBox addChipsToAllAmount;
        private TextBox botFiveChips;
        private TextBox botFourChips;
        private TextBox botThreeChips;
        private TextBox botTwoChips;
        private TextBox botOneChips;
        private TextBox potStatus;
        private Button toggleShowBlindButton;
        private Button bigBlindButton;
        private TextBox smallBlindField;
        private Button smallBlindButton;
        private TextBox bigBlindField;
        private Label botFiveStatus;
        private Label botFourStatus;
        private Label botThreeStatus;
        private Label botOneStatus;
        private Label playerStatus;
        private Label botTwoStatus;
        private Label potLabel;
        private TextBox raiseAmountField;
        
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
            this.foldButton = new Button();
            this.checkButton = new Button();
            this.callButton = new Button();
            this.raiseButton = new Button();
            this.pbTimer = new ProgressBar();
            this.playerChips = new TextBox();
            this.addChipsToAllButton = new Button();
            this.addChipsToAllAmount = new TextBox();
            this.botFiveChips = new TextBox();
            this.botFourChips = new TextBox();
            this.botThreeChips = new TextBox();
            this.botTwoChips = new TextBox();
            this.botOneChips = new TextBox();
            this.potStatus = new TextBox();
            this.toggleShowBlindButton = new Button();
            this.bigBlindButton = new Button();
            this.smallBlindField = new TextBox();
            this.smallBlindButton = new Button();
            this.bigBlindField = new TextBox();
            this.botFiveStatus = new Label();
            this.botFourStatus = new Label();
            this.botThreeStatus = new Label();
            this.botOneStatus = new Label();
            this.playerStatus = new Label();
            this.botTwoStatus = new Label();
            this.potLabel = new Label();
            this.raiseAmountField = new TextBox();
            this.SuspendLayout();
            // 
            // foldButton
            // 
            this.foldButton.Anchor = AnchorStyles.Bottom;
            this.foldButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.foldButton.Location = new System.Drawing.Point(335, 660);
            this.foldButton.Name = "bFold";
            this.foldButton.Size = new System.Drawing.Size(130, 62);
            this.foldButton.TabIndex = 0;
            this.foldButton.Text = "Fold";
            this.foldButton.UseVisualStyleBackColor = true;
            this.foldButton.Click += new System.EventHandler(this.FoldClick);
            // 
            // bCheck
            // 
            this.checkButton.Anchor = AnchorStyles.Bottom;
            this.checkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkButton.Location = new System.Drawing.Point(494, 660);
            this.checkButton.Name = "bCheck";
            this.checkButton.Size = new System.Drawing.Size(134, 62);
            this.checkButton.TabIndex = 2;
            this.checkButton.Text = "Check";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // bCall
            // 
            this.callButton.Anchor = AnchorStyles.Bottom;
            this.callButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.callButton.Location = new System.Drawing.Point(667, 661);
            this.callButton.Name = "bCall";
            this.callButton.Size = new System.Drawing.Size(126, 62);
            this.callButton.TabIndex = 3;
            this.callButton.Text = "Call";
            this.callButton.UseVisualStyleBackColor = true;
            this.callButton.Click += new System.EventHandler(this.bCall_Click);
            // 
            // bRaise
            // 
            this.raiseButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.raiseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.raiseButton.Location = new System.Drawing.Point(835, 661);
            this.raiseButton.Name = "bRaise";
            this.raiseButton.Size = new System.Drawing.Size(124, 62);
            this.raiseButton.TabIndex = 4;
            this.raiseButton.Text = "Raise";
            this.raiseButton.UseVisualStyleBackColor = true;
            this.raiseButton.Click += new System.EventHandler(this.RaiseClick);
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
            this.playerChips.Anchor = AnchorStyles.Bottom;
            this.playerChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.playerChips.Location = new System.Drawing.Point(755, 553);
            this.playerChips.Name = "tbChips";
            this.playerChips.Size = new System.Drawing.Size(163, 23);
            this.playerChips.TabIndex = 6;
            this.playerChips.Text = "Chips : 0";
            // 
            // bAdd
            // 
            this.addChipsToAllButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.addChipsToAllButton.Location = new System.Drawing.Point(12, 697);
            this.addChipsToAllButton.Name = "bAdd";
            this.addChipsToAllButton.Size = new System.Drawing.Size(75, 25);
            this.addChipsToAllButton.TabIndex = 7;
            this.addChipsToAllButton.Text = "AddChips";
            this.addChipsToAllButton.UseVisualStyleBackColor = true;
            this.addChipsToAllButton.Click += new System.EventHandler(this.AddChipsClick);
            // 
            // tbAddChips
            // 
            this.addChipsToAllAmount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.addChipsToAllAmount.Location = new System.Drawing.Point(93, 700);
            this.addChipsToAllAmount.Name = "tbAddChips";
            this.addChipsToAllAmount.Size = new System.Drawing.Size(125, 20);
            this.addChipsToAllAmount.TabIndex = 8;
            // 
            // tbBotChips5
            // 
            this.botFiveChips.Anchor = AnchorStyles.Bottom | AnchorStyles.None | AnchorStyles.Right;
            this.botFiveChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.botFiveChips.Location = new System.Drawing.Point(1012, 553);
            this.botFiveChips.Name = "tbBotChips5";
            this.botFiveChips.Size = new System.Drawing.Size(152, 23);
            this.botFiveChips.TabIndex = 9;
            this.botFiveChips.Text = "Chips : 0";
            // 
            // tbBotChips4
            // 
            this.botFourChips.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.botFourChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.botFourChips.Location = new System.Drawing.Point(970, 81);
            this.botFourChips.Name = "tbBotChips4";
            this.botFourChips.Size = new System.Drawing.Size(123, 23);
            this.botFourChips.TabIndex = 10;
            this.botFourChips.Text = "Chips : 0";
            // 
            // botThreeChips
            // 
            this.botThreeChips.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.botThreeChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.botThreeChips.Location = new System.Drawing.Point(755, 81);
            this.botThreeChips.Name = "botThreeChips";
            this.botThreeChips.Size = new System.Drawing.Size(125, 23);
            this.botThreeChips.TabIndex = 11;
            this.botThreeChips.Text = "Chips : 0";
            // 
            // tbBotChips2
            // 
            this.botTwoChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.botTwoChips.Location = new System.Drawing.Point(276, 81);
            this.botTwoChips.Name = "tbBotChips2";
            this.botTwoChips.Size = new System.Drawing.Size(133, 23);
            this.botTwoChips.TabIndex = 12;
            this.botTwoChips.Text = "Chips : 0";
            // 
            // tbBotChips1
            // 
            this.botOneChips.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.botOneChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.botOneChips.Location = new System.Drawing.Point(181, 553);
            this.botOneChips.Name = "tbBotChips1";
            this.botOneChips.Size = new System.Drawing.Size(142, 23);
            this.botOneChips.TabIndex = 13;
            this.botOneChips.Text = "Chips : 0";
            // 
            // tbPot
            // 
            this.potStatus.Anchor = AnchorStyles.None;
            this.potStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.potStatus.Location = new System.Drawing.Point(606, 212);
            this.potStatus.Name = "tbPot";
            this.potStatus.Size = new System.Drawing.Size(125, 23);
            this.potStatus.TabIndex = 14;
            this.potStatus.Text = "0";
            // 
            // bOptions
            // 
            this.toggleShowBlindButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toggleShowBlindButton.Location = new System.Drawing.Point(12, 12);
            this.toggleShowBlindButton.Name = "bOptions";
            this.toggleShowBlindButton.Size = new System.Drawing.Size(75, 36);
            this.toggleShowBlindButton.TabIndex = 15;
            this.toggleShowBlindButton.Text = "BB/SB";
            this.toggleShowBlindButton.UseVisualStyleBackColor = true;
            this.toggleShowBlindButton.Click += new System.EventHandler(this.OptionsClick);
            // 
            // bBigBlind
            // 
            this.bigBlindButton.Location = new System.Drawing.Point(12, 254);
            this.bigBlindButton.Name = "bBigBlind";
            this.bigBlindButton.Size = new System.Drawing.Size(75, 23);
            this.bigBlindButton.TabIndex = 16;
            this.bigBlindButton.Text = "Big Blind";
            this.bigBlindButton.UseVisualStyleBackColor = true;
            this.bigBlindButton.Click += new System.EventHandler(this.bBigBlind_Click);
            // 
            // tbSmallBlind
            // 
            this.smallBlindField.Location = new System.Drawing.Point(12, 228);
            this.smallBlindField.Name = "tbSmallBlind";
            this.smallBlindField.Size = new System.Drawing.Size(75, 20);
            this.smallBlindField.TabIndex = 17;
            this.smallBlindField.Text = "250";
            // 
            // bSmallBlind
            // 
            this.smallBlindButton.Location = new System.Drawing.Point(12, 199);
            this.smallBlindButton.Name = "bSmallBlind";
            this.smallBlindButton.Size = new System.Drawing.Size(75, 23);
            this.smallBlindButton.TabIndex = 18;
            this.smallBlindButton.Text = "Small Blind";
            this.smallBlindButton.UseVisualStyleBackColor = true;
            this.smallBlindButton.Click += new System.EventHandler(this.SmallBlindClick);
            // 
            // tbBigBlind
            // 
            this.bigBlindField.Location = new System.Drawing.Point(12, 283);
            this.bigBlindField.Name = "tbBigBlind";
            this.bigBlindField.Size = new System.Drawing.Size(75, 20);
            this.bigBlindField.TabIndex = 19;
            this.bigBlindField.Text = "500";
            // 
            // b5Status
            // 
            this.botFiveStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.None | AnchorStyles.Right;
            this.botFiveStatus.Location = new System.Drawing.Point(1012, 579);
            this.botFiveStatus.Name = "b5Status";
            this.botFiveStatus.Size = new System.Drawing.Size(152, 32);
            this.botFiveStatus.TabIndex = 26;
            // 
            // b4Status
            // 
            this.botFourStatus.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.botFourStatus.Location = new System.Drawing.Point(970, 107);
            this.botFourStatus.Name = "b4Status";
            this.botFourStatus.Size = new System.Drawing.Size(123, 32);
            this.botFourStatus.TabIndex = 27;
            // 
            // b3Status
            // 
            this.botThreeStatus.Anchor = AnchorStyles.None | AnchorStyles.Right | AnchorStyles.Top;
            this.botThreeStatus.Location = new System.Drawing.Point(755, 107);
            this.botThreeStatus.Name = "b3Status";
            this.botThreeStatus.Size = new System.Drawing.Size(125, 32);
            this.botThreeStatus.TabIndex = 28;
            // 
            // bot1Status
            // 
            this.botOneStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.None;
            this.botOneStatus.Location = new System.Drawing.Point(181, 579);
            this.botOneStatus.Name = "bot1Status";
            this.botOneStatus.Size = new System.Drawing.Size(142, 32);
            this.botOneStatus.TabIndex = 29;
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
            this.botTwoStatus.Location = new System.Drawing.Point(276, 107);
            this.botTwoStatus.Name = "b2Status";
            this.botTwoStatus.Size = new System.Drawing.Size(133, 32);
            this.botTwoStatus.TabIndex = 31;
            // 
            // label1
            // 
            this.potLabel.Anchor = AnchorStyles.None;
            this.potLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.potLabel.Location = new System.Drawing.Point(654, 188);
            this.potLabel.Name = "label1";
            this.potLabel.Size = new System.Drawing.Size(31, 21);
            this.potLabel.TabIndex = 0;
            this.potLabel.Text = "Pot";
            // 
            // tbRaise
            // 
            this.raiseAmountField.Anchor = AnchorStyles.Bottom;
            this.raiseAmountField.Location = new System.Drawing.Point(965, 703);
            this.raiseAmountField.Name = "tbRaise";
            this.raiseAmountField.Size = new System.Drawing.Size(108, 20);
            this.raiseAmountField.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.pokerTable;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.raiseAmountField);
            this.Controls.Add(this.potLabel);
            this.Controls.Add(this.botTwoStatus);
            this.Controls.Add(this.playerStatus);
            this.Controls.Add(this.botOneStatus);
            this.Controls.Add(this.botThreeStatus);
            this.Controls.Add(this.botFourStatus);
            this.Controls.Add(this.botFiveStatus);
            this.Controls.Add(this.bigBlindField);
            this.Controls.Add(this.smallBlindButton);
            this.Controls.Add(this.smallBlindField);
            this.Controls.Add(this.bigBlindButton);
            this.Controls.Add(this.toggleShowBlindButton);
            this.Controls.Add(this.potStatus);
            this.Controls.Add(this.botOneChips);
            this.Controls.Add(this.botTwoChips);
            this.Controls.Add(this.botThreeChips);
            this.Controls.Add(this.botFourChips);
            this.Controls.Add(this.botFiveChips);
            this.Controls.Add(this.addChipsToAllAmount);
            this.Controls.Add(this.addChipsToAllButton);
            this.Controls.Add(this.playerChips);
            this.Controls.Add(this.pbTimer);
            this.Controls.Add(this.raiseButton);
            this.Controls.Add(this.callButton);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.foldButton);
            this.DoubleBuffered = true;
            this.Name = "PokerTable";
            this.Text = "GLS Texas Poker";
            //this.Layout += new LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}