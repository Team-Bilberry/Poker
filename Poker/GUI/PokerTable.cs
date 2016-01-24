namespace Poker.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Contracts;
    using Models;
    using PokerUtilities;
    using PokerUtilities.CardsCombinationMethods;
    using Type = Poker.Type;

    public partial class PokerTable : Form
    {
        #region Variables
        private readonly ICheckHandType checkHand;
        private readonly IHandType handType;

        private readonly IPokerPlayer player;
        private readonly IPokerPlayer firstBot;
        private readonly IPokerPlayer secondBot;
        private readonly IPokerPlayer thirdBot;
        private readonly IPokerPlayer fourthBot;
        private readonly IPokerPlayer fifthBot;

        private readonly IDealer dealer;

        private int neededChipsToCall;
        private int botsWithoutChips = 5;
        private double type;
        private int rounds = 0;
        private int raise = 0;
        private int playersLeft = 6;
        private int winners = 0;

        private bool intsadded;
        private bool changed;

        //private int height;
        //private int width;

        // TODO: Convert to enum

        //private int Flop = 1;
        //private int Turn = 2;
        //private int River = 3;
        //private int End = 4;


        //int last = 123;
        private int raisedTurn = 1;

        List<bool?> arePlayersEliminated = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        private bool restart = false;
        private bool raising = false;

        Type sorted;
        string[] ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);

        /*string[] ImgLocation ={
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/
        int[] reserve = new int[17];
        Image[] deck = new Image[52];
        PictureBox[] cardPicture = new PictureBox[52];
        Timer timer = new Timer();
        Timer updates = new Timer();

        private int secondsLeft = 60;
        private int bigBlind = 500;
        private int smallBlind = 250;
        int turnCount = 0;
        #endregion

        public PokerTable(IDealer dealer, ICheckHandType checkHand, IHandType handType)
        {
            this.player = new PokerPlayer(new Panel());
            this.firstBot = new PokerPlayer(new Panel());
            this.secondBot = new PokerPlayer(new Panel());
            this.thirdBot = new PokerPlayer(new Panel());
            this.fourthBot = new PokerPlayer(new Panel());
            this.fifthBot = new PokerPlayer(new Panel());

            this.dealer = dealer;
            this.checkHand = checkHand;
            this.handType = handType;

            //bools.Add(PlayerFoldTurn); bools.Add(bot1.OutOfChips); bools.Add(bot2.FoldedTurn); bools.Add(bot3.FoldedTurn); bools.Add(bot4.FoldedTurn); bools.Add(bot5.FoldedTurn);
            this.neededChipsToCall = this.bigBlind;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.updates.Start();
            this.InitializeComponent();

            //this.width = this.Width;
            //this.height = this.Height;

            this.Shuffle();

            this.potStatus.Enabled = false;
            this.playerChips.Enabled = false;
            this.botOneChips.Enabled = false;
            this.botTwoChips.Enabled = false;
            this.botThreeChips.Enabled = false;
            this.botFourChips.Enabled = false;
            this.botFiveChips.Enabled = false;

            this.playerChips.Text = "Chips : " + this.player.Chips;
            this.botOneChips.Text = "Chips : " + this.firstBot.Chips;
            this.botTwoChips.Text = "Chips : " + this.secondBot.Chips;
            this.botThreeChips.Text = "Chips : " + this.thirdBot.Chips;
            this.botFourChips.Text = "Chips : " + this.fourthBot.Chips;
            this.botFiveChips.Text = "Chips : " + this.fifthBot.Chips;

            this.timer.Interval = 1000;
            this.timer.Tick += this.TimerTick;
            this.updates.Interval = 100;
            this.updates.Tick += this.Update_Tick;

            this.raiseAmountField.Text = (this.bigBlind * 2).ToString();

            this.player.OutOfChips = false;
            this.player.AbleToMakeTurn = true;
        }

        private async Task Shuffle()
        {
            this.arePlayersEliminated.Add(this.player.OutOfChips);
            this.arePlayersEliminated.Add(this.firstBot.OutOfChips);
            this.arePlayersEliminated.Add(this.secondBot.OutOfChips);
            this.arePlayersEliminated.Add(this.thirdBot.OutOfChips);
            this.arePlayersEliminated.Add(this.fourthBot.OutOfChips);
            this.arePlayersEliminated.Add(this.fifthBot.OutOfChips);

            this.callButton.Enabled = false;
            this.raiseButton.Enabled = false;
            this.foldButton.Enabled = false;
            this.checkButton.Enabled = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            bool check = false;

            Bitmap backImage = new Bitmap(@"..\..\Resources\Assets\Back\Back.png");
            int horizontal = 580;
            int vertical = 480;

            //this.ShuffleCards();
            this.ImgLocation = this.dealer.ShuffleDeck(this.ImgLocation);

            // TODO: move it to proper place.

            for (int currentCardIndex = 0; currentCardIndex < GlobalConstants.NeededCardsFromDeck; currentCardIndex++)
            {
                this.deck[currentCardIndex] = Image.FromFile(this.ImgLocation[currentCardIndex]);

                // take card name, too slow i think
                //var charsToRemove = new string[] { @"..\..\Resources\Assets\Cards\", ".png" };
                //foreach (var c in charsToRemove)
                //{
                //    this.ImgLocation[currentCardIndex] = this.ImgLocation[currentCardIndex].Replace(c, string.Empty);
                //}
                int lastSlashIndex = this.ImgLocation[currentCardIndex].LastIndexOf("\\");
                int lastDotIndex = this.ImgLocation[currentCardIndex].LastIndexOf(".");
                this.ImgLocation[currentCardIndex] = this.ImgLocation[currentCardIndex]
                    .Substring(lastSlashIndex + 1, lastDotIndex - lastSlashIndex - 1);

                this.reserve[currentCardIndex] = int.Parse(this.ImgLocation[currentCardIndex]) - 1;

                this.cardPicture[currentCardIndex] = new PictureBox();
                this.cardPicture[currentCardIndex].SizeMode = PictureBoxSizeMode.StretchImage;
                this.cardPicture[currentCardIndex].Height = 130;
                this.cardPicture[currentCardIndex].Width = 80;
                this.Controls.Add(this.cardPicture[currentCardIndex]);
                this.cardPicture[currentCardIndex].Name = "pb" + currentCardIndex.ToString();

                // investigate why these delay is needed
                await Task.Delay(150);
                #region Throwing Cards
                if (currentCardIndex < 2)
                {
                    this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];
                    this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                    this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Bottom;
                    //this.cardPicture[currentCardIndex].Dock = DockStyle.Top;
                    this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                    horizontal += this.cardPicture[currentCardIndex].Width;

                    this.Controls.Add(this.player.Panel);
                    var playerPanelLocation = new Point(this.cardPicture[0].Left - 10, this.cardPicture[0].Top - 10);
                    this.player.InitializePanel(playerPanelLocation);
                }

                if (this.firstBot.Chips > 0)
                {
                    this.botsWithoutChips--;
                    if (currentCardIndex >= 2 && currentCardIndex < 4)
                    {
                        this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];

                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        if (currentCardIndex == 3)
                        {
                            check = false;
                        }

                        this.cardPicture[currentCardIndex].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        this.cardPicture[currentCardIndex].Image = backImage;
                        //this.cardPicture[currentCardIndex].Image = deck[currentCardIndex];
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.firstBot.Panel);
                        var bot1PanelLocation = new Point(this.cardPicture[2].Left - 10, this.cardPicture[2].Top - 10);
                        this.firstBot.InitializePanel(bot1PanelLocation);
                    }
                }

                if (this.secondBot.Chips > 0)
                {
                    this.botsWithoutChips--;
                    if (currentCardIndex >= 4 && currentCardIndex < 6)
                    {
                        this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];

                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }
                        check = true;
                        if (currentCardIndex == 5)
                        {
                            check = false;
                        }

                        this.cardPicture[currentCardIndex].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        this.cardPicture[currentCardIndex].Image = backImage;
                        //this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.secondBot.Panel);
                        var bot2PanelLocation = new Point(this.cardPicture[4].Left - 10, this.cardPicture[4].Top - 10);
                        this.secondBot.InitializePanel(bot2PanelLocation);
                    }
                }

                if (this.thirdBot.Chips > 0)
                {
                    this.botsWithoutChips--;
                    if (currentCardIndex >= 6 && currentCardIndex < 8)
                    {
                        this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];

                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        if (currentCardIndex == 7)
                        {
                            check = false;
                        }

                        this.cardPicture[currentCardIndex].Anchor = (AnchorStyles.Top);
                        this.cardPicture[currentCardIndex].Image = backImage;
                        //this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.thirdBot.Panel);
                        var bot3PanelLocation = new Point(this.cardPicture[6].Left - 10, this.cardPicture[6].Top - 10);
                        this.thirdBot.InitializePanel(bot3PanelLocation);
                    }
                }

                if (this.fourthBot.Chips > 0)
                {
                    this.botsWithoutChips--;
                    if (currentCardIndex >= 8 && currentCardIndex < 10)
                    {
                        this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];

                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }
                        check = true;
                        if (currentCardIndex == 9)
                        {
                            check = false;
                        }

                        this.cardPicture[currentCardIndex].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        this.cardPicture[currentCardIndex].Image = backImage;
                        // this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.fourthBot.Panel);
                        var bot4PanelLocation = new Point(this.cardPicture[8].Left - 10, this.cardPicture[8].Top - 10);
                        this.fourthBot.InitializePanel(bot4PanelLocation);
                    }
                }

                if (this.fifthBot.Chips > 0)
                {
                    this.botsWithoutChips--;
                    if (currentCardIndex >= 10 && currentCardIndex < 12)
                    {
                        this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];

                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;
                        if (currentCardIndex == 11)
                        {
                            check = false;
                        }

                        this.cardPicture[currentCardIndex].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        this.cardPicture[currentCardIndex].Image = backImage;
                        //this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.fifthBot.Panel);
                        var bot5PanelLocation = new Point(this.cardPicture[10].Left - 10, this.cardPicture[10].Top - 10);
                        this.fifthBot.InitializePanel(bot5PanelLocation);
                    }
                }

                if (currentCardIndex >= 12)
                {
                    this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;

                    this.cardPicture[currentCardIndex].Anchor = AnchorStyles.None;
                    this.cardPicture[currentCardIndex].Image = backImage;
                    //this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                    this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                    horizontal += 110;
                }
                #endregion
                if (this.firstBot.Chips <= 0)
                {
                    this.firstBot.OutOfChips = true;
                    this.cardPicture[2].Visible = false;
                    this.cardPicture[3].Visible = false;
                }
                else
                {
                    this.firstBot.OutOfChips = false;
                    if (currentCardIndex == 3)
                    {
                        if (this.cardPicture[3] != null)
                        {
                            this.cardPicture[2].Visible = true;
                            this.cardPicture[3].Visible = true;
                        }
                    }
                }

                if (this.secondBot.Chips <= 0)
                {
                    this.secondBot.OutOfChips = true;
                    this.cardPicture[4].Visible = false;
                    this.cardPicture[5].Visible = false;
                }
                else
                {
                    this.secondBot.OutOfChips = false;
                    if (currentCardIndex == 5)
                    {
                        if (this.cardPicture[5] != null)
                        {
                            this.cardPicture[4].Visible = true;
                            this.cardPicture[5].Visible = true;
                        }
                    }
                }

                if (this.thirdBot.Chips <= 0)
                {
                    this.thirdBot.OutOfChips = true;
                    this.cardPicture[6].Visible = false;
                    this.cardPicture[7].Visible = false;
                }
                else
                {
                    this.thirdBot.OutOfChips = false;
                    if (currentCardIndex == 7)
                    {
                        if (this.cardPicture[7] != null)
                        {
                            this.cardPicture[6].Visible = true;
                            this.cardPicture[7].Visible = true;
                        }
                    }
                }

                if (this.fourthBot.Chips <= 0)
                {
                    this.fourthBot.OutOfChips = true;
                    this.cardPicture[8].Visible = false;
                    this.cardPicture[9].Visible = false;
                }
                else
                {
                    this.fourthBot.OutOfChips = false;
                    if (currentCardIndex == 9)
                    {
                        if (this.cardPicture[9] != null)
                        {
                            this.cardPicture[8].Visible = true;
                            this.cardPicture[9].Visible = true;
                        }
                    }
                }

                if (this.fifthBot.Chips <= 0)
                {
                    this.fifthBot.OutOfChips = true;
                    this.cardPicture[10].Visible = false;
                    this.cardPicture[11].Visible = false;
                }
                else
                {
                    this.fifthBot.OutOfChips = false;
                    if (currentCardIndex == 11)
                    {
                        if (this.cardPicture[11] != null)
                        {
                            this.cardPicture[10].Visible = true;
                            this.cardPicture[11].Visible = true;
                        }
                    }
                }

                if (currentCardIndex == 16)
                {
                    if (!this.restart)
                    {
                        this.MaximizeBox = true;
                        this.MinimizeBox = true;
                    }

                    this.timer.Start();
                }
            }

            if (this.botsWithoutChips == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                this.botsWithoutChips = 5;
            }

            this.raiseButton.Enabled = true;
            this.callButton.Enabled = true;
            this.raiseButton.Enabled = true;
            this.raiseButton.Enabled = true;
            this.foldButton.Enabled = true;
        }

        //private void ShuffleCards()
        //{
        //    Random rnd = new Random();
        //    for (int currentIndex = this.ImgLocation.Length; currentIndex > 0; currentIndex--)
        //    {
        //        int swapCardIndex = rnd.Next(currentIndex);
        //        var tempCard = this.ImgLocation[swapCardIndex];
        //        this.ImgLocation[swapCardIndex] = this.ImgLocation[currentIndex - 1];
        //        this.ImgLocation[currentIndex - 1] = tempCard;
        //    }
        //}

        private async Task Turns()
        {
            #region Rotating
            if (!this.player.OutOfChips && this.player.AbleToMakeTurn)
            {
                this.FixCall(this.playerStatus, this.player, 1);

                //MessageBox.Show("Player's Turn");
                this.pbTimer.Visible = true;
                this.pbTimer.Value = 1000;
                this.secondsLeft = 60;

                this.timer.Start();
                this.raiseButton.Enabled = true;
                this.callButton.Enabled = true;
                this.foldButton.Enabled = true;
                this.turnCount++;
                this.FixCall(this.playerStatus, this.player, 2);
            }

            if (this.player.OutOfChips || !this.player.AbleToMakeTurn)
            {
                await this.AllIn();

                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (this.callButton.Text.Contains("All in") == false
                        || this.raiseButton.Text.Contains("All in") == false)
                    {
                        //bools.RemoveAt(0);
                        //bools.Insert(0, null);
                        this.arePlayersEliminated[0] = null;
                        this.playersLeft--;
                        this.player.Folded = true;
                    }
                }

                await this.CheckRaise(0, 0);

                this.pbTimer.Visible = false;
                this.raiseButton.Enabled = false;
                this.callButton.Enabled = false;
                this.raiseButton.Enabled = false;
                this.raiseButton.Enabled = false;
                this.foldButton.Enabled = false;
                this.timer.Stop();
                this.firstBot.AbleToMakeTurn = true;

                if (!this.firstBot.OutOfChips)
                {
                    if (this.firstBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botOneStatus, this.firstBot, 1);
                        this.FixCall(this.botOneStatus, this.firstBot, 2);

                        this.Rules(2, 3, "Bot 1", this.firstBot);
                        MessageBox.Show("Bot 1's Turn");

                        this.AI(2, 3, this.botOneStatus, 0, this.firstBot);

                        this.turnCount++;
                        this.firstBot.AbleToMakeTurn = false;
                        this.secondBot.AbleToMakeTurn = true;
                    }
                }

                if (this.firstBot.OutOfChips && !this.firstBot.Folded)
                {
                    this.arePlayersEliminated.RemoveAt(1);
                    this.arePlayersEliminated.Insert(1, null);
                    this.playersLeft--;
                    this.firstBot.Folded = true;
                }

                if (this.firstBot.OutOfChips || !this.firstBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(1, 1);
                    this.secondBot.AbleToMakeTurn = true;
                }

                if (!this.secondBot.OutOfChips)
                {
                    if (this.secondBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botTwoStatus, this.secondBot, 1);
                        this.FixCall(this.botTwoStatus, this.secondBot, 2);
                        this.Rules(4, 5, "Bot 2", this.secondBot);
                        MessageBox.Show("Bot 2's Turn");
                        this.AI(4, 5, this.botTwoStatus, 1, this.secondBot);
                        this.turnCount++;
                        this.secondBot.AbleToMakeTurn = false;
                        this.thirdBot.AbleToMakeTurn = true;
                    }
                }

                if (this.secondBot.OutOfChips && !this.secondBot.Folded)
                {
                    this.arePlayersEliminated.RemoveAt(2);
                    this.arePlayersEliminated.Insert(2, null);
                    this.playersLeft--;
                    this.secondBot.Folded = true;
                }

                if (this.secondBot.OutOfChips || !this.secondBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(2, 2);
                    this.thirdBot.AbleToMakeTurn = true;
                }

                if (!this.thirdBot.OutOfChips)
                {
                    if (this.thirdBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botThreeStatus, this.thirdBot, 1);
                        this.FixCall(this.botThreeStatus, this.thirdBot, 2);

                        this.Rules(6, 7, "Bot 3", this.thirdBot);
                        MessageBox.Show("Bot 3's Turn");
                        this.AI(6, 7, this.botThreeStatus, 2, this.thirdBot);
                        this.turnCount++;
                        this.thirdBot.AbleToMakeTurn = false;
                        this.fourthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.thirdBot.OutOfChips && !this.thirdBot.Folded)
                {
                    this.arePlayersEliminated.RemoveAt(3);
                    this.arePlayersEliminated.Insert(3, null);
                    this.playersLeft--;
                    this.thirdBot.Folded = true;
                }

                if (this.thirdBot.OutOfChips || !this.thirdBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(3, 3);
                    this.fourthBot.AbleToMakeTurn = true;
                }

                if (!this.fourthBot.OutOfChips)
                {
                    if (this.fourthBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botFourStatus, this.fourthBot, 1);
                        this.FixCall(this.botFourStatus, this.fourthBot, 2);
                        this.Rules(8, 9, "Bot 4", this.fourthBot);
                        MessageBox.Show("Bot 4's Turn");
                        this.AI(8, 9, this.botFourStatus, 3, this.fourthBot);
                        this.turnCount++;
                        this.fourthBot.AbleToMakeTurn = false;
                        this.fifthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.fourthBot.OutOfChips && !this.fourthBot.Folded)
                {
                    this.arePlayersEliminated.RemoveAt(4);
                    this.arePlayersEliminated.Insert(4, null);
                    this.playersLeft--;
                    this.fourthBot.Folded = true;
                }

                if (this.fourthBot.OutOfChips || !this.fourthBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(4, 4);
                    this.fifthBot.AbleToMakeTurn = true;
                }

                if (!this.fifthBot.OutOfChips)
                {
                    if (this.fifthBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botFiveStatus, this.fifthBot, 1);
                        this.FixCall(this.botFiveStatus, this.fifthBot, 2);
                        this.Rules(10, 11, "Bot 5", this.fifthBot);
                        MessageBox.Show("Bot 5's Turn");
                        this.AI(10, 11, this.botFiveStatus, 4, this.fifthBot);
                        this.turnCount++;
                        this.fifthBot.AbleToMakeTurn = false;
                    }
                }

                if (this.fifthBot.OutOfChips && !this.fifthBot.Folded)
                {
                    this.arePlayersEliminated.RemoveAt(5);
                    this.arePlayersEliminated.Insert(5, null);
                    this.playersLeft--;
                    this.fifthBot.Folded = true;
                }

                if (this.fifthBot.OutOfChips || !this.fifthBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(5, 5);
                    this.player.AbleToMakeTurn = true;
                }

                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (this.callButton.Text.Contains("All in") == false || this.raiseButton.Text.Contains("All in") == false)
                    {
                        this.arePlayersEliminated.RemoveAt(0);
                        this.arePlayersEliminated.Insert(0, null);
                        this.playersLeft--;
                        this.player.Folded = true;
                    }
                }

                #endregion
                await this.AllIn();
                if (!this.restart)
                {
                    await this.Turns();
                }

                this.restart = false;
            }
        }
        // TODO: extract the method below in PokerUtilities
        void Rules(int card1, int card2, string currentText, IPokerPlayer pokerPlayer)
        {
            if (card1 == 0 && card2 == 1)
            {
            }

            if (!pokerPlayer.OutOfChips || card1 == 0 && card2 == 1 && this.playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables              
                int[] Straight = new int[7];

                Straight[0] = this.reserve[card1];
                Straight[1] = this.reserve[card2];
                Straight[2] = this.reserve[12];
                Straight[3] = this.reserve[13];
                Straight[4] = this.reserve[14];
                Straight[5] = this.reserve[15];
                Straight[6] = this.reserve[16];

                int[] getClubes = Straight.Where(o => o % 4 == 0).ToArray();
                int[] getDimonds = Straight.Where(o => o % 4 == 1).ToArray();
                int[] getHearts = Straight.Where(o => o % 4 == 2).ToArray();
                int[] getSpades = Straight.Where(o => o % 4 == 3).ToArray();

                int[] clubes = getClubes.Select(o => o / 4).Distinct().ToArray();
                int[] diamonds = getDimonds.Select(o => o / 4).Distinct().ToArray();
                int[] hearts = getHearts.Select(o => o / 4).Distinct().ToArray();
                int[] spades = getSpades.Select(o => o / 4).Distinct().ToArray();

                #endregion

                Array.Sort(Straight);
                Array.Sort(clubes);
                Array.Sort(diamonds);
                Array.Sort(hearts);
                Array.Sort(spades);

                for (int index = 0; index < 16; index++)
                {
                    if (this.reserve[index] == int.Parse(this.cardPicture[card1].Tag.ToString()) &&
                        this.reserve[index + 1] == int.Parse(this.cardPicture[card2].Tag.ToString()))
                    {
                        ////Pair from Hand current = 1
                        //this.rPairFromHand(pokerPlayer, index);

                        //this.rPairTwoPair(pokerPlayer, index);

                        //this.rTwoPair(pokerPlayer, index);

                        //this.rThreeOfAKind(pokerPlayer, Straight, index);

                        //this.RStraight(pokerPlayer, Straight, index);

                        //this.RFlush(pokerPlayer, ref vf, Straight1, index);

                        //this.rFullHouse(pokerPlayer, ref done, Straight);

                        //this.rFourOfAKind(pokerPlayer, Straight);

                        //this.rStraightFlush(pokerPlayer, st1, st2, st3, st4);

                        //this.rHighCard(pokerPlayer, index);

                        this.checkHand.PairFromHand(pokerPlayer, index, ref this.Win, ref this.sorted, ref reserve);

                        this.checkHand.PairTwoPair(pokerPlayer, index, ref this.Win, ref this.sorted, ref reserve);

                        this.checkHand.TwoPair(pokerPlayer, index, ref this.Win, ref this.sorted, ref this.reserve);

                        this.checkHand.ThreeOfAKind(pokerPlayer, Straight, ref this.Win, ref this.sorted);

                        this.checkHand.Straight(pokerPlayer, Straight, ref this.Win, ref this.sorted);

                        this.checkHand.Flush(pokerPlayer, Straight, ref this.Win, ref this.sorted);

                        this.checkHand.FullHouse(pokerPlayer, Straight, ref this.Win, ref this.sorted);

                        this.checkHand.FourOfAKind(pokerPlayer, Straight, ref this.Win, ref this.sorted);

                        this.checkHand.StraightFlush(pokerPlayer, clubes, diamonds, hearts, spades, ref this.Win, ref this.sorted);

                        this.checkHand.HighCard(pokerPlayer, index, ref this.Win, ref this.sorted, ref this.reserve);
                    }
                }
            }
        }

        //private void rStraightFlush(IPokerPlayer pokerPlayer, int[] st1, int[] st2, int[] st3, int[] st4)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        if (st1.Length >= 5)
        //        {
        //            if (st1[0] + 4 == st1[4])
        //            {
        //                pokerPlayer.Type = 8;
        //                pokerPlayer.Power = (st1.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
        //            {
        //                pokerPlayer.Type = 9;
        //                pokerPlayer.Power = (st1.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }

        //        if (st2.Length >= 5)
        //        {
        //            if (st2[0] + 4 == st2[4])
        //            {
        //                pokerPlayer.Type = 8;
        //                pokerPlayer.Power = (st2.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
        //            {
        //                pokerPlayer.Type = 9;
        //                pokerPlayer.Power = (st2.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }

        //        if (st3.Length >= 5)
        //        {
        //            if (st3[0] + 4 == st3[4])
        //            {
        //                pokerPlayer.Type = 8;
        //                pokerPlayer.Power = (st3.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
        //            {
        //                pokerPlayer.Type = 9;
        //                pokerPlayer.Power = (st3.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }

        //        if (st4.Length >= 5)
        //        {
        //            if (st4[0] + 4 == st4[4])
        //            {
        //                pokerPlayer.Type = 8;
        //                pokerPlayer.Power = (st4.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
        //            {
        //                pokerPlayer.Type = 9;
        //                pokerPlayer.Power = (st4.Max()) / 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }
        //    }
        //}

        //private void rFourOfAKind(IPokerPlayer pokerPlayer, int[] straight)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        for (int j = 0; j <= 3; j++)
        //        {
        //            if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4 &&
        //                straight[j] / 4 == straight[j + 3] / 4)
        //            {
        //                pokerPlayer.Type = 7;
        //                pokerPlayer.Power = (straight[j] / 4) * 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 7 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
        //            {
        //                pokerPlayer.Type = 7;
        //                pokerPlayer.Power = 13 * 4 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 7 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }
        //    }
        //}

        //private void rFullHouse(IPokerPlayer pokerPlayer, ref bool done, int[] straight)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        this.type = pokerPlayer.Power;
        //        for (int j = 0; j <= 12; j++)
        //        {
        //            var fh = straight.Where(o => o / 4 == j).ToArray();
        //            if (fh.Length == 3 || done)
        //            {
        //                if (fh.Length == 2)
        //                {
        //                    if (fh.Max() / 4 == 0)
        //                    {
        //                        pokerPlayer.Type = 6;
        //                        pokerPlayer.Power = 13 * 2 + pokerPlayer.Type * 100;
        //                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 6 });
        //                        this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                        break;
        //                    }

        //                    if (fh.Max() / 4 > 0)
        //                    {
        //                        pokerPlayer.Type = 6;
        //                        pokerPlayer.Power = fh.Max() / 4 * 2 + pokerPlayer.Type * 100;
        //                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 6 });
        //                        this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                        break;
        //                    }
        //                }

        //                if (!done)
        //                {
        //                    if (fh.Max() / 4 == 0)
        //                    {
        //                        pokerPlayer.Power = 13;
        //                        done = true;
        //                        j = -1;
        //                    }
        //                    else
        //                    {
        //                        pokerPlayer.Power = fh.Max() / 4;
        //                        done = true;
        //                        j = -1;
        //                    }
        //                }
        //            }
        //        }

        //        if (pokerPlayer.Type != 6)
        //        {
        //            pokerPlayer.Power = this.type;
        //        }
        //    }
        //}

        //private void RFlush(IPokerPlayer pokerPlayer, ref bool vf, int[] straight1, int index)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        var f1 = straight1.Where(o => o % 4 == 0).ToArray();
        //        var f2 = straight1.Where(o => o % 4 == 1).ToArray();
        //        var f3 = straight1.Where(o => o % 4 == 2).ToArray();
        //        var f4 = straight1.Where(o => o % 4 == 3).ToArray();
        //        if (f1.Length == 3 || f1.Length == 4)
        //        {
        //            if (this.reserve[index] % 4 == this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f1[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f1.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }

        //                if (this.reserve[index + 1] / 4 > f1.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else if (this.reserve[index] / 4 < f1.Max() / 4 && this.reserve[index + 1] / 4 < f1.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f1.Length == 4)//different cards in hand
        //        {
        //            if (this.reserve[index] % 4 != this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f1[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f1.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }

        //            if (this.reserve[index + 1] % 4 != this.reserve[index] % 4 && this.reserve[index + 1] % 4 == f1[0] % 4)
        //            {
        //                if (this.reserve[index + 1] / 4 > f1.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f1.Length == 5)
        //        {
        //            if (this.reserve[index] % 4 == f1[0] % 4 && this.reserve[index] / 4 > f1.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }

        //            if (this.reserve[index + 1] % 4 == f1[0] % 4 && this.reserve[index + 1] / 4 > f1.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //            else if (this.reserve[index] / 4 < f1.Min() / 4 && this.reserve[index + 1] / 4 < f1.Min())
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //        }

        //        if (f2.Length == 3 || f2.Length == 4)
        //        {
        //            if (this.reserve[index] % 4 == this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f2[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f2.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }

        //                if (this.reserve[index + 1] / 4 > f2.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else if (this.reserve[index] / 4 < f2.Max() / 4 && this.reserve[index + 1] / 4 < f2.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f2.Length == 4)//different cards in hand
        //        {
        //            if (this.reserve[index] % 4 != this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f2[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f2.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }

        //            if (this.reserve[index + 1] % 4 != this.reserve[index] % 4 && this.reserve[index + 1] % 4 == f2[0] % 4)
        //            {
        //                if (this.reserve[index + 1] / 4 > f2.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f2.Length == 5)
        //        {
        //            if (this.reserve[index] % 4 == f2[0] % 4 && this.reserve[index] / 4 > f2.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }

        //            if (this.reserve[index + 1] % 4 == f2[0] % 4 && this.reserve[index + 1] / 4 > f2.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //            else if (this.reserve[index] / 4 < f2.Min() / 4 && this.reserve[index + 1] / 4 < f2.Min())
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //        }

        //        if (f3.Length == 3 || f3.Length == 4)
        //        {
        //            if (this.reserve[index] % 4 == this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f3[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f3.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }

        //                if (this.reserve[index + 1] / 4 > f3.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else if (this.reserve[index] / 4 < f3.Max() / 4 && this.reserve[index + 1] / 4 < f3.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f3.Length == 4)//different cards in hand
        //        {
        //            if (this.reserve[index] % 4 != this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f3[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f3.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }

        //            if (this.reserve[index + 1] % 4 != this.reserve[index] % 4 && this.reserve[index + 1] % 4 == f3[0] % 4)
        //            {
        //                if (this.reserve[index + 1] / 4 > f3.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f3.Length == 5)
        //        {
        //            if (this.reserve[index] % 4 == f3[0] % 4 && this.reserve[index] / 4 > f3.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }

        //            if (this.reserve[index + 1] % 4 == f3[0] % 4 && this.reserve[index + 1] / 4 > f3.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //            else if (this.reserve[index] / 4 < f3.Min() / 4 && this.reserve[index + 1] / 4 < f3.Min())
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //        }

        //        if (f4.Length == 3 || f4.Length == 4)
        //        {
        //            if (this.reserve[index] % 4 == this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f4[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f4.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }

        //                if (this.reserve[index + 1] / 4 > f4.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else if (this.reserve[index] / 4 < f4.Max() / 4 && this.reserve[index + 1] / 4 < f4.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f4.Length == 4)//different cards in hand
        //        {
        //            if (this.reserve[index] % 4 != this.reserve[index + 1] % 4 && this.reserve[index] % 4 == f4[0] % 4)
        //            {
        //                if (this.reserve[index] / 4 > f4.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }

        //            if (this.reserve[index + 1] % 4 != this.reserve[index] % 4 && this.reserve[index + 1] % 4 == f4[0] % 4)
        //            {
        //                if (this.reserve[index + 1] / 4 > f4.Max() / 4)
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 5;
        //                    pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                    vf = true;
        //                }
        //            }
        //        }

        //        if (f4.Length == 5)
        //        {
        //            if (this.reserve[index] % 4 == f4[0] % 4 && this.reserve[index] / 4 > f4.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }

        //            if (this.reserve[index + 1] % 4 == f4[0] % 4 && this.reserve[index + 1] / 4 > f4.Min() / 4)
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //            else if (this.reserve[index] / 4 < f4.Min() / 4 && this.reserve[index + 1] / 4 < f4.Min())
        //            {
        //                pokerPlayer.Type = 5;
        //                pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                vf = true;
        //            }
        //        }

        //        //ace
        //        if (f1.Length > 0)
        //        {
        //            if (this.reserve[index] / 4 == 0 && this.reserve[index] % 4 == f1[0] % 4 && vf && f1.Length > 0)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (this.reserve[index + 1] / 4 == 0 && this.reserve[index + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }

        //        if (f2.Length > 0)
        //        {
        //            if (this.reserve[index] / 4 == 0 && this.reserve[index] % 4 == f2[0] % 4 && vf && f2.Length > 0)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (this.reserve[index + 1] / 4 == 0 && this.reserve[index + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }

        //        if (f3.Length > 0)
        //        {
        //            if (this.reserve[index] / 4 == 0 && this.reserve[index] % 4 == f3[0] % 4 && vf && f3.Length > 0)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (this.reserve[index + 1] / 4 == 0 && this.reserve[index + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }

        //        if (f4.Length > 0)
        //        {
        //            if (this.reserve[index] / 4 == 0 && this.reserve[index] % 4 == f4[0] % 4 && vf && f4.Length > 0)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }

        //            if (this.reserve[index + 1] / 4 == 0 && this.reserve[index + 1] % 4 == f4[0] % 4 && vf)
        //            {
        //                pokerPlayer.Type = 5.5;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }
        //    }
        //}

        //private void RStraight(IPokerPlayer pokerPlayer, int[] straight, int index)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        var op = straight.Select(o => o / 4).Distinct().ToArray();
        //        for (index = 0; index < op.Length - 4; index++)
        //        {
        //            if (op[index] + 4 == op[index + 4])
        //            {
        //                if (op.Max() - 4 == op[index])
        //                {
        //                    pokerPlayer.Type = 4;
        //                    pokerPlayer.Power = op.Max() + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 4;
        //                    pokerPlayer.Power = op[index + 4] + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
        //                    this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //                }
        //            }

        //            if (op[index] == 0 && op[index + 1] == 9 && op[index + 2] == 10 && op[index + 3] == 11 && op[index + 4] == 12)
        //            {
        //                pokerPlayer.Type = 4;
        //                pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
        //                this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
        //                this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //            }
        //        }
        //    }
        //}

        //private void rThreeOfAKind(IPokerPlayer pokerPlayer, int[] straight, int index)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        for (index = 0; index <= 12; index++)
        //        {
        //            var fh = straight.Where(o => o / 4 == index).ToArray();
        //            if (fh.Length == 3)
        //            {
        //                if (fh.Max() / 4 == 0)
        //                {
        //                    pokerPlayer.Type = 3;
        //                    pokerPlayer.Power = 13 * 3 + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 3 });
        //                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 3;
        //                    pokerPlayer.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + pokerPlayer.Type * 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 3 });
        //                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                }
        //            }
        //        }
        //    }
        //}

        //private void rTwoPair(IPokerPlayer pokerPlayer, int index)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        bool msgbox = false;
        //        for (int tc = 16; tc >= 12; tc--)
        //        {
        //            int max = tc - 12;
        //            if (this.reserve[index] / 4 != this.reserve[index + 1] / 4)
        //            {
        //                for (int k = 1; k <= max; k++)
        //                {
        //                    if (tc - k < 12)
        //                    {
        //                        max--;
        //                    }
        //                    if (tc - k >= 12)
        //                    {
        //                        if (this.reserve[index] / 4 == this.reserve[tc] / 4 && this.reserve[index + 1] / 4 == this.reserve[tc - k] / 4 ||
        //                            this.reserve[index + 1] / 4 == this.reserve[tc] / 4 && this.reserve[index] / 4 == this.reserve[tc - k] / 4)
        //                        {
        //                            if (!msgbox)
        //                            {
        //                                if (this.reserve[index] / 4 == 0)
        //                                {
        //                                    pokerPlayer.Type = 2;
        //                                    pokerPlayer.Power = 13 * 4 + (this.reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
        //                                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
        //                                    this.sorted =
        //                                        this.Win.OrderByDescending(op => op.Current)
        //                                            .ThenByDescending(op => op.Power)
        //                                            .First();
        //                                }

        //                                if (this.reserve[index + 1] / 4 == 0)
        //                                {
        //                                    pokerPlayer.Type = 2;
        //                                    pokerPlayer.Power = 13 * 4 + (this.reserve[index] / 4) * 2 + pokerPlayer.Type * 100;
        //                                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
        //                                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                }

        //                                if (this.reserve[index + 1] / 4 != 0 && this.reserve[index] / 4 != 0)
        //                                {
        //                                    pokerPlayer.Type = 2;
        //                                    pokerPlayer.Power = (this.reserve[index] / 4) * 2 + (this.reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
        //                                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
        //                                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                }
        //                            }
        //                            msgbox = true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void rPairTwoPair(IPokerPlayer pokerPlayer, int index)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        bool msgbox = false;
        //        bool msgbox1 = false;
        //        for (int tc = 16; tc >= 12; tc--)
        //        {
        //            int max = tc - 12;
        //            for (int k = 1; k <= max; k++)
        //            {
        //                if (tc - k < 12)
        //                {
        //                    max--;
        //                }

        //                if (tc - k >= 12)
        //                {
        //                    if (this.reserve[tc] / 4 == this.reserve[tc - k] / 4)
        //                    {
        //                        if (this.reserve[tc] / 4 != this.reserve[index] / 4 && this.reserve[tc] / 4 != this.reserve[index + 1] / 4 && pokerPlayer.Type == 1)
        //                        {
        //                            if (!msgbox)
        //                            {
        //                                if (this.reserve[index + 1] / 4 == 0)
        //                                {
        //                                    pokerPlayer.Type = 2;
        //                                    pokerPlayer.Power = (this.reserve[index] / 4) * 2 + 13 * 4 + pokerPlayer.Type * 100;
        //                                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
        //                                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                }

        //                                if (this.reserve[index] / 4 == 0)
        //                                {
        //                                    pokerPlayer.Type = 2;
        //                                    pokerPlayer.Power = (this.reserve[index + 1] / 4) * 2 + 13 * 4 + pokerPlayer.Type * 100;
        //                                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
        //                                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                }

        //                                if (this.reserve[index + 1] / 4 != 0)
        //                                {
        //                                    pokerPlayer.Type = 2;
        //                                    pokerPlayer.Power = (this.reserve[tc] / 4) * 2 + (this.reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
        //                                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
        //                                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                }

        //                                if (this.reserve[index] / 4 != 0)
        //                                {
        //                                    pokerPlayer.Type = 2;
        //                                    pokerPlayer.Power = (this.reserve[tc] / 4) * 2 + (this.reserve[index] / 4) * 2 + pokerPlayer.Type * 100;
        //                                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
        //                                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                }
        //                            }

        //                            msgbox = true;
        //                        }

        //                        if (pokerPlayer.Type == -1)
        //                        {
        //                            if (!msgbox1)
        //                            {
        //                                if (this.reserve[index] / 4 > this.reserve[index + 1] / 4)
        //                                {
        //                                    if (this.reserve[tc] / 4 == 0)
        //                                    {
        //                                        pokerPlayer.Type = 0;
        //                                        pokerPlayer.Power = 13 + this.reserve[index] / 4 + pokerPlayer.Type * 100;
        //                                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                    }
        //                                    else
        //                                    {
        //                                        pokerPlayer.Type = 0;
        //                                        pokerPlayer.Power = this.reserve[tc] / 4 + this.reserve[index] / 4 + pokerPlayer.Type * 100;
        //                                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (this.reserve[tc] / 4 == 0)
        //                                    {
        //                                        pokerPlayer.Type = 0;
        //                                        pokerPlayer.Power = 13 + this.reserve[index + 1] + pokerPlayer.Type * 100;
        //                                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                    }
        //                                    else
        //                                    {
        //                                        pokerPlayer.Type = 0;
        //                                        pokerPlayer.Power = this.reserve[tc] / 4 + this.reserve[index + 1] / 4 + pokerPlayer.Type * 100;
        //                                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                                    }
        //                                }
        //                            }

        //                            msgbox1 = true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void rPairFromHand(IPokerPlayer pokerPlayer, int index)
        //{
        //    if (pokerPlayer.Type >= -1)
        //    {
        //        bool msgbox = false;
        //        if (this.reserve[index] / 4 == this.reserve[index + 1] / 4)
        //        {
        //            if (!msgbox)
        //            {
        //                if (this.reserve[index] / 4 == 0)
        //                {
        //                    pokerPlayer.Type = 1;
        //                    pokerPlayer.Power = 13 * 4 + 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                }
        //                else
        //                {
        //                    pokerPlayer.Type = 1;
        //                    pokerPlayer.Power = (this.reserve[index + 1] / 4) * 4 + 100;
        //                    this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                    this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                }
        //            }

        //            msgbox = true;
        //        }

        //        for (int tc = 16; tc >= 12; tc--)
        //        {
        //            if (this.reserve[index + 1] / 4 == this.reserve[tc] / 4)
        //            {
        //                if (!msgbox)
        //                {
        //                    if (this.reserve[index + 1] / 4 == 0)
        //                    {
        //                        pokerPlayer.Type = 1;
        //                        pokerPlayer.Power = 13 * 4 + this.reserve[index] / 4 + 100;
        //                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                    }
        //                    else
        //                    {
        //                        pokerPlayer.Type = 1;
        //                        pokerPlayer.Power = (this.reserve[index + 1] / 4) * 4 + this.reserve[index] / 4 + 100;
        //                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                    }
        //                }

        //                msgbox = true;
        //            }

        //            if (this.reserve[index] / 4 == this.reserve[tc] / 4)
        //            {
        //                if (!msgbox)
        //                {
        //                    if (this.reserve[index] / 4 == 0)
        //                    {
        //                        pokerPlayer.Type = 1;
        //                        pokerPlayer.Power = 13 * 4 + this.reserve[index + 1] / 4 + 100;
        //                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                    }
        //                    else
        //                    {
        //                        pokerPlayer.Type = 1;
        //                        pokerPlayer.Power = (this.reserve[tc] / 4) * 4 + this.reserve[index + 1] / 4 + 100;
        //                        this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
        //                        this.sorted = this.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
        //                    }
        //                }
        //                msgbox = true;
        //            }
        //        }
        //    }
        //}

        //private void rHighCard(IPokerPlayer pokerPlayer, int index)
        //{
        //    if (pokerPlayer.Type == -1)
        //    {
        //        if (this.reserve[index] / 4 > this.reserve[index + 1] / 4)
        //        {
        //            pokerPlayer.Type = -1;
        //            pokerPlayer.Power = this.reserve[index] / 4;
        //            this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
        //            this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //        }
        //        else
        //        {
        //            pokerPlayer.Type = -1;
        //            pokerPlayer.Power = this.reserve[index + 1] / 4;
        //            this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
        //            this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //        }

        //        if (this.reserve[index] / 4 == 0 || this.reserve[index + 1] / 4 == 0)
        //        {
        //            pokerPlayer.Type = -1;
        //            pokerPlayer.Power = 13;
        //            this.Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
        //            this.sorted = this.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        //        }
        //    }
        //}

        void Winner(double current, double Power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (this.cardPicture[j].Visible)
                {
                    this.cardPicture[j].Image = this.deck[j];
                }
            }
            if (current == this.sorted.Current)
            {
                if (Power == this.sorted.Power)
                {
                    this.winners++;
                    this.CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }

                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }

                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }

                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }

                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }

                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }

                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }

                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }

                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }

                    if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }
            if (currentText == lastly)//lastfixed
            {
                if (this.winners > 1)
                {
                    if (this.CheckWinners.Contains("Player"))
                    {
                        this.player.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.playerChips.Text = this.player.Chips.ToString();
                        //player.Panel.Visible = true;

                    }

                    if (this.CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.botOneChips.Text = this.firstBot.Chips.ToString();
                        //bot1.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.botTwoChips.Text = this.secondBot.Chips.ToString();
                        //bot2.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.botThreeChips.Text = this.thirdBot.Chips.ToString();
                        //bot3.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 4"))
                    {
                        this.fourthBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.botFourChips.Text = this.fourthBot.Chips.ToString();
                        //bot4.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.botFiveChips.Text = this.fifthBot.Chips.ToString();
                        //bot5.Panel.Visible = true;
                    }

                    //await Finish(1);
                }
                if (this.winners == 1)
                {
                    if (this.CheckWinners.Contains("Player"))
                    {
                        this.player.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //player.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot1.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot2.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot3.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 4"))
                    {
                        this.fourthBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot4.Panel.Visible = true;
                    }

                    if (this.CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot5.Panel.Visible = true;
                    }
                }
            }
        }

        private async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (this.raising)
            {
                this.turnCount = 0;
                this.raising = false;
                this.raisedTurn = currentTurn;
                this.changed = true;
            }
            else
            {
                if (this.turnCount >= this.playersLeft - 1 || !this.changed && this.turnCount == this.playersLeft)
                {
                    if (currentTurn == this.raisedTurn - 1 || !this.changed && this.turnCount == this.playersLeft || this.raisedTurn == 0 && currentTurn == 5)
                    {
                        this.changed = false;
                        this.turnCount = 0;
                        this.raise = 0;
                        this.neededChipsToCall = 0;
                        this.raisedTurn = 123;
                        this.rounds++;
                        if (!this.player.OutOfChips)
                        {
                            this.playerStatus.Text = string.Empty;
                        }

                        if (!this.firstBot.OutOfChips)
                        {
                            this.botOneStatus.Text = string.Empty;
                        }

                        if (!this.secondBot.OutOfChips)
                        {
                            this.botTwoStatus.Text = string.Empty;
                        }

                        if (!this.thirdBot.OutOfChips)
                        {
                            this.botThreeStatus.Text = string.Empty;
                        }

                        if (!this.fourthBot.OutOfChips)
                        {
                            this.botFourStatus.Text = string.Empty;
                        }

                        if (!this.fifthBot.OutOfChips)
                        {
                            this.botFiveStatus.Text = string.Empty;
                        }
                    }
                }
            }

            if (this.rounds == (int)RoundStage.Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.cardPicture[j].Image != this.deck[j])
                    {
                        this.cardPicture[j].Image = this.deck[j];

                        this.player.Call = 0;
                        this.player.Raise = 0;

                        this.firstBot.Call = 0;
                        this.firstBot.Raise = 0;

                        this.secondBot.Call = 0;
                        this.secondBot.Raise = 0;

                        this.thirdBot.Call = 0;
                        this.thirdBot.Raise = 0;

                        this.fourthBot.Call = 0;
                        this.fourthBot.Raise = 0;

                        this.fifthBot.Call = 0;
                        this.fifthBot.Raise = 0;
                    }
                }
            }

            if (this.rounds == (int)RoundStage.Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.cardPicture[j].Image != this.deck[j])
                    {
                        this.cardPicture[j].Image = this.deck[j];

                        this.player.Call = 0;
                        this.player.Raise = 0;

                        this.firstBot.Call = 0;
                        this.firstBot.Raise = 0;

                        this.secondBot.Call = 0;
                        this.secondBot.Raise = 0;

                        this.thirdBot.Call = 0;
                        this.thirdBot.Raise = 0;

                        this.fourthBot.Call = 0;
                        this.fourthBot.Raise = 0;

                        this.fifthBot.Call = 0;
                        this.fifthBot.Raise = 0;
                    }
                }
            }

            if (this.rounds == (int)RoundStage.River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.cardPicture[j].Image != this.deck[j])
                    {
                        this.cardPicture[j].Image = this.deck[j];
                        this.player.Call = 0;
                        this.player.Raise = 0;

                        this.firstBot.Call = 0;
                        this.firstBot.Raise = 0;

                        this.secondBot.Call = 0;
                        this.secondBot.Raise = 0;

                        this.thirdBot.Call = 0;
                        this.thirdBot.Raise = 0;

                        this.fourthBot.Call = 0;
                        this.fourthBot.Raise = 0;

                        this.fifthBot.Call = 0;
                        this.fifthBot.Raise = 0;
                    }
                }
            }

            if (this.rounds == (int)RoundStage.End && this.playersLeft == 6)
            {
                string fixedLast = string.Empty;

                if (!this.playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    this.Rules(0, 1, "Player", this.player);
                }

                if (!this.botOneStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    this.Rules(2, 3, "Bot 1", this.firstBot);
                }

                if (!this.botTwoStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    this.Rules(4, 5, "Bot 2", this.secondBot);
                }

                if (!this.botThreeStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    this.Rules(6, 7, "Bot 3", this.thirdBot);
                }

                if (!this.botFourStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    this.Rules(8, 9, "Bot 4", this.fourthBot);
                }

                if (!this.botFiveStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    this.Rules(10, 11, "Bot 5", this.fifthBot);
                }
                this.Winner(this.player.Type, this.player.Power, "Player", this.player.Chips, fixedLast);
                this.Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", this.firstBot.Chips, fixedLast);
                this.Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", this.secondBot.Chips, fixedLast);
                this.Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", this.thirdBot.Chips, fixedLast);
                this.Winner(this.fourthBot.Type, this.fourthBot.Power, "Bot 4", this.fourthBot.Chips, fixedLast);
                this.Winner(this.fifthBot.Type, this.fifthBot.Power, "Bot 5", this.fifthBot.Chips, fixedLast);
                this.restart = true;
                this.player.AbleToMakeTurn = true;
                this.player.OutOfChips = false;
                this.firstBot.OutOfChips = false;
                this.secondBot.OutOfChips = false;
                this.thirdBot.OutOfChips = false;
                this.fourthBot.OutOfChips = false;
                this.fifthBot.OutOfChips = false;

                // TODO: Add chips on two place.
                if (this.player.Chips <= 0)
                {
                    AddChips addChips = new AddChips();
                    addChips.ShowDialog();
                    if (addChips.AddedChips != 0)
                    {
                        this.player.Chips = addChips.AddedChips;
                        this.firstBot.Chips += addChips.AddedChips;
                        this.secondBot.Chips += addChips.AddedChips;
                        this.thirdBot.Chips += addChips.AddedChips;
                        this.fourthBot.Chips += addChips.AddedChips;
                        this.fifthBot.Chips += addChips.AddedChips;
                        this.player.OutOfChips = false;
                        this.player.AbleToMakeTurn = true;
                        this.raiseButton.Enabled = true;
                        this.foldButton.Enabled = true;
                        this.checkButton.Enabled = true;
                        this.raiseButton.Text = "Raise";
                    }
                }

                this.player.Panel.Visible = false; this.firstBot.Panel.Visible = false; this.secondBot.Panel.Visible = false; this.thirdBot.Panel.Visible = false; this.fourthBot.Panel.Visible = false; this.fifthBot.Panel.Visible = false;
                this.player.Call = 0; this.player.Raise = 0;
                this.firstBot.Call = 0; this.firstBot.Raise = 0;
                this.secondBot.Call = 0; this.secondBot.Raise = 0;
                this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                this.fourthBot.Call = 0; this.fourthBot.Raise = 0;
                this.fifthBot.Call = 0; this.fifthBot.Raise = 0;
                this.neededChipsToCall = this.bigBlind;
                this.raise = 0;
                this.ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);
                this.arePlayersEliminated.Clear();
                this.rounds = 0;
                this.player.Power = 0; this.player.Type = -1;
                this.type = 0; this.firstBot.Power = 0; this.secondBot.Power = 0; this.thirdBot.Power = 0; this.fourthBot.Power = 0; this.fifthBot.Power = 0;
                this.firstBot.Type = -1; this.secondBot.Type = -1; this.thirdBot.Type = -1; this.fourthBot.Type = -1; this.fifthBot.Type = -1;
                this.ints.Clear();
                this.CheckWinners.Clear();
                this.winners = 0;
                this.Win.Clear();
                this.sorted.Current = 0;
                this.sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    this.cardPicture[os].Image = null;
                    this.cardPicture[os].Invalidate();
                    this.cardPicture[os].Visible = false;
                }
                this.potStatus.Text = "0";
                this.playerStatus.Text = string.Empty;
                await this.Shuffle();
                await this.Turns();
            }
        }

        void FixCall(Label status, IPokerPlayer pokerPlayer, int options)
        {
            if (this.rounds == 4)
            {
                return;
            }

            if (options == 1)
            {
                if (status.Text.Contains("Raise"))
                {
                    var changeRaise = status.Text.Substring(6);
                    pokerPlayer.Raise = int.Parse(changeRaise);
                }

                if (status.Text.Contains("Call"))
                {
                    var changeCall = status.Text.Substring(5);
                    pokerPlayer.Call = int.Parse(changeCall);
                }

                if (status.Text.Contains("Check"))
                {
                    pokerPlayer.Raise = 0;
                    pokerPlayer.Call = 0;
                }
            }

            if (options == 2)
            {
                if (pokerPlayer.Raise != this.raise && pokerPlayer.Raise <= this.raise)
                {
                    this.neededChipsToCall = this.raise - pokerPlayer.Raise;
                }

                if (pokerPlayer.Call != this.neededChipsToCall || pokerPlayer.Call <= this.neededChipsToCall)
                {
                    this.neededChipsToCall = this.neededChipsToCall - pokerPlayer.Call;
                }

                // TODO: check when this is valid and change text in call label
                if (pokerPlayer.Raise == this.raise && this.raise > 0)
                {
                    this.neededChipsToCall = 0;
                    this.callButton.Enabled = false;
                    this.callButton.Text = "Callisfuckedup";
                }
            }
        }

        async Task AllIn()
        {
            #region All in
            if (this.player.Chips <= 0 && !this.intsadded)
            {
                if (this.playerStatus.Text.Contains("Raise"))
                {
                    this.ints.Add(this.player.Chips);
                    this.intsadded = true;
                }

                if (this.playerStatus.Text.Contains("Call"))
                {
                    this.ints.Add(this.player.Chips);
                    this.intsadded = true;
                }
            }

            this.intsadded = false;
            if (this.firstBot.Chips <= 0 && !this.firstBot.OutOfChips)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.firstBot.Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }
            if (this.secondBot.Chips <= 0 && !this.secondBot.OutOfChips)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.secondBot.Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }
            if (this.thirdBot.Chips <= 0 && !this.thirdBot.OutOfChips)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.thirdBot.Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }
            if (this.fourthBot.Chips <= 0 && !this.fourthBot.OutOfChips)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.fourthBot.Chips);
                    this.intsadded = true;
                }

                this.intsadded = false;
            }
            if (this.fifthBot.Chips <= 0 && !this.fifthBot.OutOfChips)
            {
                if (!this.intsadded)
                {
                    this.ints.Add(this.fifthBot.Chips);
                    this.intsadded = true;
                }
            }
            if (this.ints.ToArray().Length == this.playersLeft)
            {
                await this.Finish(2);
            }
            else
            {
                this.ints.Clear();
            }
            #endregion

            var abc = this.arePlayersEliminated.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = this.arePlayersEliminated.IndexOf(false);
                if (index == 0)
                {
                    this.player.Chips += int.Parse(this.potStatus.Text);
                    this.playerChips.Text = this.player.Chips.ToString();
                    this.player.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }

                if (index == 1)
                {
                    this.firstBot.Chips += int.Parse(this.potStatus.Text);
                    this.playerChips.Text = this.firstBot.Chips.ToString();
                    this.firstBot.Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }

                if (index == 2)
                {
                    this.secondBot.Chips += int.Parse(this.potStatus.Text);
                    this.playerChips.Text = this.secondBot.Chips.ToString();
                    this.secondBot.Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }

                if (index == 3)
                {
                    this.thirdBot.Chips += int.Parse(this.potStatus.Text);
                    this.playerChips.Text = this.thirdBot.Chips.ToString();
                    this.thirdBot.Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }

                if (index == 4)
                {
                    this.fourthBot.Chips += int.Parse(this.potStatus.Text);
                    this.playerChips.Text = this.fourthBot.Chips.ToString();
                    this.fourthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }

                if (index == 5)
                {
                    this.fifthBot.Chips += int.Parse(this.potStatus.Text);
                    this.playerChips.Text = this.fifthBot.Chips.ToString();
                    this.fifthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }

                for (int j = 0; j <= 16; j++)
                {
                    this.cardPicture[j].Visible = false;
                }
                await this.Finish(1);
            }

            this.intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && this.rounds >= (int)RoundStage.End)
            {
                await this.Finish(2);
            }
            #endregion
        }

        async Task Finish(int n)
        {
            if (n == 2)
            {
                this.FixWinners();
            }

            // TODO : extract in method Reset or something like that
            this.player.Panel.Visible = false;
            this.firstBot.Panel.Visible = false;
            this.secondBot.Panel.Visible = false;
            this.thirdBot.Panel.Visible = false;
            this.fourthBot.Panel.Visible = false;
            this.fifthBot.Panel.Visible = false;

            this.neededChipsToCall = this.bigBlind;
            this.raise = 0;
            this.botsWithoutChips = 5;
            this.type = 0;
            this.rounds = 0;

            this.firstBot.Power = 0;
            this.secondBot.Power = 0;
            this.thirdBot.Power = 0;
            this.fourthBot.Power = 0;
            this.fifthBot.Power = 0;
            this.player.Power = 0;

            this.player.Type = -1;
            this.firstBot.Type = -1;
            this.secondBot.Type = -1;
            this.thirdBot.Type = -1;
            this.fourthBot.Type = -1;
            this.fifthBot.Type = -1;

            this.player.AbleToMakeTurn = true;
            this.firstBot.AbleToMakeTurn = false;
            this.secondBot.AbleToMakeTurn = false;
            this.thirdBot.AbleToMakeTurn = false;
            this.fourthBot.AbleToMakeTurn = false;
            this.fifthBot.AbleToMakeTurn = false;

            this.player.OutOfChips = false;
            this.firstBot.OutOfChips = false;
            this.secondBot.OutOfChips = false;
            this.thirdBot.OutOfChips = false;
            this.fourthBot.OutOfChips = false;
            this.fifthBot.OutOfChips = false;

            this.player.Folded = false;
            this.firstBot.Folded = false;
            this.secondBot.Folded = false;
            this.thirdBot.Folded = false;
            this.fourthBot.Folded = false;
            this.fifthBot.Folded = false;
            this.restart = false;
            this.raising = false;

            this.player.Call = 0;
            this.firstBot.Call = 0;
            this.secondBot.Call = 0;
            this.thirdBot.Call = 0;
            this.fourthBot.Call = 0;
            this.fifthBot.Call = 0;

            this.player.Raise = 0;
            this.firstBot.Raise = 0;
            this.secondBot.Raise = 0;
            this.thirdBot.Raise = 0;
            this.fourthBot.Raise = 0;
            this.fifthBot.Raise = 0;
            // height = 0;
            // width = 0;
            this.winners = 0;

            //this.Flop = 1;
            //this.Turn = 2;
            //this.River = 3;
            //this.End = 4;
            this.playersLeft = 6;
            this.raisedTurn = 1;

            this.arePlayersEliminated.Clear();
            this.CheckWinners.Clear();
            this.ints.Clear();
            this.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            this.potStatus.Text = "0";
            this.secondsLeft = 60;
            this.turnCount = 0;
            this.playerStatus.Text = string.Empty;
            this.botOneStatus.Text = string.Empty;
            this.botTwoStatus.Text = string.Empty;
            this.botThreeStatus.Text = string.Empty;
            this.botFourStatus.Text = string.Empty;
            this.botFiveStatus.Text = string.Empty;

            // TODO: Here add chips, duplicate.
            if (this.player.Chips <= 0)
            {
                AddChips addChips = new AddChips();
                addChips.ShowDialog();
                if (addChips.AddedChips != 0)
                {
                    this.player.Chips = addChips.AddedChips;
                    this.firstBot.Chips += addChips.AddedChips;
                    this.secondBot.Chips += addChips.AddedChips;
                    this.thirdBot.Chips += addChips.AddedChips;
                    this.fourthBot.Chips += addChips.AddedChips;
                    this.fifthBot.Chips += addChips.AddedChips;
                    this.player.OutOfChips = false;
                    this.player.AbleToMakeTurn = true;
                    this.raiseButton.Enabled = true;
                    this.foldButton.Enabled = true;
                    this.checkButton.Enabled = true;
                    this.raiseButton.Text = "Raise";
                }
            }

            this.ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);

            for (int os = 0; os < 17; os++)
            {
                this.cardPicture[os].Image = null;
                this.cardPicture[os].Invalidate();
                this.cardPicture[os].Visible = false;
            }

            await this.Shuffle();
            //await Turns();
        }

        private void FixWinners()
        {
            this.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            string fixedLast = string.Empty;

            if (!this.playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                this.Rules(0, 1, "Player", this.player);
            }

            if (!this.botOneStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                this.Rules(2, 3, "Bot 1", this.firstBot);
            }

            if (!this.botTwoStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                this.Rules(4, 5, "Bot 2", this.secondBot);
            }

            if (!this.botThreeStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                this.Rules(6, 7, "Bot 3", this.thirdBot);
            }

            if (!this.botFourStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                this.Rules(8, 9, "Bot 4", this.fourthBot);
            }

            if (!this.botFiveStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                this.Rules(10, 11, "Bot 5", this.fifthBot);
            }

            this.Winner(this.player.Type, this.player.Power, "Player", this.player.Chips, fixedLast);
            this.Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", this.firstBot.Chips, fixedLast);
            this.Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", this.secondBot.Chips, fixedLast);
            this.Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", this.thirdBot.Chips, fixedLast);
            this.Winner(this.fourthBot.Type, this.fourthBot.Power, "Bot 4", this.fourthBot.Chips, fixedLast);
            this.Winner(this.fifthBot.Type, this.fifthBot.Power, "Bot 5", this.fifthBot.Chips, fixedLast);
        }

        void AI(int c1, int c2, Label sStatus, int name, IPokerPlayer pokerPlayer)
        {
            if (!pokerPlayer.OutOfChips)
            {
                if (pokerPlayer.Type == -1)
                {
                    //this.HighCard(pokerPlayer, sStatus);

                    this.handType.HighCard(pokerPlayer, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                }

                if (pokerPlayer.Type == 0)
                {
                    //this.PairTable(pokerPlayer, sStatus);

                    this.handType.PairTable(pokerPlayer, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                }

                if (pokerPlayer.Type == 1)
                {
                    //this.PairHand(pokerPlayer, sStatus);

                    this.handType.PairHand(pokerPlayer, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (pokerPlayer.Type == 2)
                {
                    //this.TwoPair(pokerPlayer, sStatus);

                    this.handType.TwoPair(pokerPlayer, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (pokerPlayer.Type == 3)
                {
                    //this.ThreeOfAKind(pokerPlayer, sStatus, name);

                    this.handType.ThreeOfAKind(pokerPlayer, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (pokerPlayer.Type == 4)
                {
                    //this.Straight(pokerPlayer, sStatus, name);

                    this.handType.Straight(pokerPlayer, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (pokerPlayer.Type == 5 || pokerPlayer.Type == 5.5)
                {
                    //this.Flush(pokerPlayer, sStatus, name);

                    this.handType.Flush(pokerPlayer, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (pokerPlayer.Type == 6)
                {
                    //this.FullHouse(pokerPlayer, sStatus, name);

                    this.handType.FullHouse(pokerPlayer, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (pokerPlayer.Type == 7)
                {
                    //this.FourOfAKind(pokerPlayer, sStatus, name);

                    this.handType.FourOfAKind(pokerPlayer, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (pokerPlayer.Type == 8 || pokerPlayer.Type == 9)
                {
                    //this.StraightFlush(pokerPlayer, sStatus, name);

                    this.handType.StraightFlush(pokerPlayer, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }
            }

            if (pokerPlayer.OutOfChips)
            {
                this.cardPicture[c1].Visible = false;
                this.cardPicture[c2].Visible = false;
            }
        }

        //private void HighCard(IPokerPlayer pokerPlayer, Label sStatus)
        //{
        //    this.HP(pokerPlayer, sStatus, 20, 25);
        //}

        //private void PairTable(IPokerPlayer pokerPlayer, Label sStatus)
        //{
        //    this.HP(pokerPlayer, sStatus, 16, 25);
        //}

        //private void PairHand(IPokerPlayer pokerPlayer, Label sStatus)
        //{
        //    Random rPair = new Random();
        //    int rCall = rPair.Next(10, 16);
        //    int rRaise = rPair.Next(10, 13);
        //    if (pokerPlayer.Power <= 199 && pokerPlayer.Power >= 140)
        //    {
        //        this.PH(pokerPlayer, sStatus, rCall, 6, rRaise);
        //    }

        //    if (pokerPlayer.Power <= 139 && pokerPlayer.Power >= 128)
        //    {
        //        this.PH(pokerPlayer, sStatus, rCall, 7, rRaise);
        //    }

        //    if (pokerPlayer.Power < 128 && pokerPlayer.Power >= 101)
        //    {
        //        this.PH(pokerPlayer, sStatus, rCall, 9, rRaise);
        //    }
        //}

        //private void TwoPair(IPokerPlayer pokerPlayer, Label sStatus)
        //{
        //    Random rPair = new Random();
        //    int rCall = rPair.Next(6, 11);
        //    int rRaise = rPair.Next(6, 11);
        //    if (pokerPlayer.Power <= 290 && pokerPlayer.Power >= 246)
        //    {
        //        this.PH(pokerPlayer, sStatus, rCall, 3, rRaise);
        //    }

        //    if (pokerPlayer.Power <= 244 && pokerPlayer.Power >= 234)
        //    {
        //        this.PH(pokerPlayer, sStatus, rCall, 4, rRaise);
        //    }

        //    if (pokerPlayer.Power < 234 && pokerPlayer.Power >= 201)
        //    {
        //        this.PH(pokerPlayer, sStatus, rCall, 4, rRaise);
        //    }
        //}

        //private void ThreeOfAKind(IPokerPlayer pokerPlayer, Label sStatus, int name)
        //{
        //    Random tk = new Random();
        //    int tCall = tk.Next(3, 7);
        //    int tRaise = tk.Next(4, 8);
        //    if (pokerPlayer.Power <= 390 && pokerPlayer.Power >= 330)
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, tCall, tRaise);
        //    }

        //    if (pokerPlayer.Power <= 327 && pokerPlayer.Power >= 321)//10  8
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, tCall, tRaise);
        //    }

        //    if (pokerPlayer.Power < 321 && pokerPlayer.Power >= 303)//7 2
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, tCall, tRaise);
        //    }
        //}

        //private void Straight(IPokerPlayer pokerPlayer, Label sStatus, int name)
        //{
        //    Random str = new Random();
        //    int sCall = str.Next(3, 6);
        //    int sRaise = str.Next(3, 8);
        //    if (pokerPlayer.Power <= 480 && pokerPlayer.Power >= 410)
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, sCall, sRaise);
        //    }

        //    if (pokerPlayer.Power <= 409 && pokerPlayer.Power >= 407)//10  8
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, sCall, sRaise);
        //    }

        //    if (pokerPlayer.Power < 407 && pokerPlayer.Power >= 404)
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, sCall, sRaise);
        //    }
        //}

        //private void Flush(IPokerPlayer pokerPlayer, Label sStatus, int name)
        //{
        //    Random fsh = new Random();
        //    int fCall = fsh.Next(2, 6);
        //    int fRaise = fsh.Next(3, 7);
        //    this.Smooth(pokerPlayer, sStatus, name, fCall, fRaise);
        //}

        //private void FullHouse(IPokerPlayer pokerPlayer, Label sStatus, int name)
        //{
        //    Random flh = new Random();
        //    int fhCall = flh.Next(1, 5);
        //    int fhRaise = flh.Next(2, 6);
        //    if (pokerPlayer.Power <= 626 && pokerPlayer.Power >= 620)
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, fhCall, fhRaise);
        //    }

        //    if (pokerPlayer.Power < 620 && pokerPlayer.Power >= 602)
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, fhCall, fhRaise);
        //    }
        //}

        //private void FourOfAKind(IPokerPlayer pokerPlayer, Label sStatus, int name)
        //{
        //    Random fk = new Random();
        //    int fkCall = fk.Next(1, 4);
        //    int fkRaise = fk.Next(2, 5);
        //    if (pokerPlayer.Power <= 752 && pokerPlayer.Power >= 704)
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, fkCall, fkRaise);
        //    }
        //}

        //private void StraightFlush(IPokerPlayer pokerPlayer, Label sStatus, int name)
        //{
        //    Random sf = new Random();
        //    int sfCall = sf.Next(1, 3);
        //    int sfRaise = sf.Next(1, 3);
        //    if (pokerPlayer.Power <= 913 && pokerPlayer.Power >= 804)
        //    {
        //        this.Smooth(pokerPlayer, sStatus, name, sfCall, sfRaise);
        //    }
        //}

        //private void Fold(IPokerPlayer pokerPlayer, Label sStatus)
        //{
        //    this.raising = false;
        //    sStatus.Text = "Fold";
        //    pokerPlayer.AbleToMakeTurn = false;
        //    pokerPlayer.OutOfChips = true;
        //}

        //private void Check(IPokerPlayer pokerPlayer, Label cStatus)
        //{
        //    cStatus.Text = "Check";
        //    pokerPlayer.AbleToMakeTurn = false;
        //    this.raising = false;
        //}

        //private void Call(IPokerPlayer pokerPlayer, Label sStatus)
        //{
        //    this.raising = false;
        //    pokerPlayer.AbleToMakeTurn = false;
        //    pokerPlayer.Chips -= this.neededChipsToCall;
        //    sStatus.Text = "Call " + this.neededChipsToCall;
        //    this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
        //}

        //private void Raised(IPokerPlayer pokerPlayer, Label sStatus)
        //{
        //    pokerPlayer.Chips -= Convert.ToInt32(this.raise);
        //    sStatus.Text = "Raise " + this.raise;
        //    this.potStatus.Text = (int.Parse(this.potStatus.Text) + Convert.ToInt32(this.raise)).ToString();
        //    this.neededChipsToCall = Convert.ToInt32(this.raise);
        //    this.raising = true;
        //    pokerPlayer.AbleToMakeTurn = false;
        //}

        //private static double RoundN(int sChips, int n)
        //{
        //    double a = Math.Round((sChips / n) / 100d, 0) * 100;
        //    return a;
        //}

        //private void HP(IPokerPlayer pokerPlayer, Label sStatus, int n, int n1)
        //{
        //    Random rand = new Random();
        //    int rnd = rand.Next(1, 4);
        //    if (this.neededChipsToCall <= 0)
        //    {
        //        this.Check(pokerPlayer, sStatus);
        //    }

        //    if (this.neededChipsToCall > 0)
        //    {
        //        if (rnd == 1)
        //        {
        //            if (this.neededChipsToCall <= RoundN(pokerPlayer.Chips, n))
        //            {
        //                this.Call(pokerPlayer, sStatus);
        //            }
        //            else
        //            {
        //                this.Fold(pokerPlayer, sStatus);
        //            }
        //        }

        //        if (rnd == 2)
        //        {
        //            if (this.neededChipsToCall <= RoundN(pokerPlayer.Chips, n1))
        //            {
        //                this.Call(pokerPlayer, sStatus);
        //            }
        //            else
        //            {
        //                this.Fold(pokerPlayer, sStatus);
        //            }
        //        }
        //    }

        //    if (rnd == 3)
        //    {
        //        if (this.raise == 0)
        //        {
        //            this.raise = this.neededChipsToCall * 2;
        //            this.Raised(pokerPlayer, sStatus);
        //        }
        //        else
        //        {
        //            if (this.raise <= RoundN(pokerPlayer.Chips, n))
        //            {
        //                this.raise = this.neededChipsToCall * 2;
        //                this.Raised(pokerPlayer, sStatus);
        //            }
        //            else
        //            {
        //                this.Fold(pokerPlayer, sStatus);
        //            }
        //        }
        //    }

        //    if (pokerPlayer.Chips <= 0)
        //    {
        //        pokerPlayer.OutOfChips = true;
        //    }
        //}

        //private void PH(IPokerPlayer pokerPlayer, Label sStatus, int n, int n1, int r)
        //{
        //    Random rand = new Random();
        //    int rnd = rand.Next(1, 3);
        //    if (this.rounds < 2)
        //    {
        //        if (this.neededChipsToCall <= 0)
        //        {
        //            this.Check(pokerPlayer, sStatus);
        //        }

        //        if (this.neededChipsToCall > 0)
        //        {
        //            if (this.neededChipsToCall >= RoundN(pokerPlayer.Chips, n1))
        //            {
        //                this.Fold(pokerPlayer, sStatus);
        //            }

        //            if (this.raise > RoundN(pokerPlayer.Chips, n))
        //            {
        //                this.Fold(pokerPlayer, sStatus);
        //            }

        //            if (!pokerPlayer.OutOfChips)
        //            {
        //                if (this.neededChipsToCall >= RoundN(pokerPlayer.Chips, n) && this.neededChipsToCall <= RoundN(pokerPlayer.Chips, n1))
        //                {
        //                    this.Call(pokerPlayer, sStatus);
        //                }

        //                if (this.raise <= RoundN(pokerPlayer.Chips, n) && this.raise >= (RoundN(pokerPlayer.Chips, n)) / 2)
        //                {
        //                    this.Call(pokerPlayer, sStatus);
        //                }

        //                if (this.raise <= (RoundN(pokerPlayer.Chips, n)) / 2)
        //                {
        //                    if (this.raise > 0)
        //                    {
        //                        this.raise = (int)RoundN(pokerPlayer.Chips, n);
        //                        this.Raised(pokerPlayer, sStatus);
        //                    }
        //                    else
        //                    {
        //                        this.raise = this.neededChipsToCall * 2;
        //                        this.Raised(pokerPlayer, sStatus);
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    if (this.rounds >= 2)
        //    {
        //        if (this.neededChipsToCall > 0)
        //        {
        //            if (this.neededChipsToCall >= RoundN(pokerPlayer.Chips, n1 - rnd))
        //            {
        //                this.Fold(pokerPlayer, sStatus);
        //            }

        //            if (this.raise > RoundN(pokerPlayer.Chips, n - rnd))
        //            {
        //                this.Fold(pokerPlayer, sStatus);
        //            }

        //            if (!pokerPlayer.OutOfChips)
        //            {
        //                if (this.neededChipsToCall >= RoundN(pokerPlayer.Chips, n - rnd) && this.neededChipsToCall <= RoundN(pokerPlayer.Chips, n1 - rnd))
        //                {
        //                    this.Call(pokerPlayer, sStatus);
        //                }

        //                if (this.raise <= RoundN(pokerPlayer.Chips, n - rnd) && this.raise >= (RoundN(pokerPlayer.Chips, n - rnd)) / 2)
        //                {
        //                    this.Call(pokerPlayer, sStatus);
        //                }

        //                if (this.raise <= (RoundN(pokerPlayer.Chips, n - rnd)) / 2)
        //                {
        //                    if (this.raise > 0)
        //                    {
        //                        this.raise = (int)RoundN(pokerPlayer.Chips, n - rnd);
        //                        this.Raised(pokerPlayer, sStatus);
        //                    }
        //                    else
        //                    {
        //                        this.raise = this.neededChipsToCall * 2;
        //                        this.Raised(pokerPlayer, sStatus);
        //                    }
        //                }
        //            }
        //        }

        //        if (this.neededChipsToCall <= 0)
        //        {
        //            this.raise = (int)RoundN(pokerPlayer.Chips, r - rnd);
        //            this.Raised(pokerPlayer, sStatus);
        //        }
        //    }

        //    if (pokerPlayer.Chips <= 0)
        //    {
        //        pokerPlayer.OutOfChips = true;
        //    }
        //}

        //void Smooth(IPokerPlayer pokerPlayer, Label botStatus, int name, int n, int r)
        //{
        //    Random rand = new Random();
        //    int rnd = rand.Next(1, 3);
        //    if (this.neededChipsToCall <= 0)
        //    {
        //        this.Check(pokerPlayer, botStatus);
        //    }
        //    else
        //    {
        //        if (this.neededChipsToCall >= RoundN(pokerPlayer.Chips, n))
        //        {
        //            if (pokerPlayer.Chips > this.neededChipsToCall)
        //            {
        //                this.Call(pokerPlayer, botStatus);
        //            }
        //            else if (pokerPlayer.Chips <= this.neededChipsToCall)
        //            {
        //                this.raising = false;
        //                pokerPlayer.AbleToMakeTurn = false;
        //                pokerPlayer.Chips = 0;
        //                botStatus.Text = "Call " + pokerPlayer.Chips;
        //                this.potStatus.Text = (int.Parse(this.potStatus.Text) + pokerPlayer.Chips).ToString();
        //            }
        //        }
        //        else
        //        {
        //            if (this.raise > 0)
        //            {
        //                if (pokerPlayer.Chips >= this.raise * 2)
        //                {
        //                    this.raise *= 2;
        //                    this.Raised(pokerPlayer, botStatus);
        //                }
        //                else
        //                {
        //                    this.Call(pokerPlayer, botStatus);
        //                }
        //            }
        //            else
        //            {
        //                this.raise = this.neededChipsToCall * 2;
        //                this.Raised(pokerPlayer, botStatus);
        //            }
        //        }
        //    }

        //    if (pokerPlayer.Chips <= 0)
        //    {
        //        pokerPlayer.OutOfChips = true;
        //    }
        //}

        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (this.pbTimer.Value <= 0)
            {
                this.player.OutOfChips = true;
                await this.Turns();
            }

            if (this.secondsLeft > 0)
            {
                this.secondsLeft--;
                this.pbTimer.Value = (this.secondsLeft / 6) * 100;
            }
        }

        private void Update_Tick(object sender, object e)
        {
            if (this.player.Chips <= 0)
            {
                this.playerChips.Text = "Chips : 0";
            }

            if (this.firstBot.Chips <= 0)
            {
                this.botOneChips.Text = "Chips : 0";
            }

            if (this.secondBot.Chips <= 0)
            {
                this.botTwoChips.Text = "Chips : 0";
            }

            if (this.thirdBot.Chips <= 0)
            {
                this.botThreeChips.Text = "Chips : 0";
            }

            if (this.fourthBot.Chips <= 0)
            {
                this.botFourChips.Text = "Chips : 0";
            }

            if (this.fifthBot.Chips <= 0)
            {
                this.botFiveChips.Text = "Chips : 0";
            }

            this.playerChips.Text = "Chips : " + this.player.Chips.ToString();
            this.botOneChips.Text = "Chips : " + this.firstBot.Chips.ToString();
            this.botTwoChips.Text = "Chips : " + this.secondBot.Chips.ToString();
            this.botThreeChips.Text = "Chips : " + this.thirdBot.Chips.ToString();
            this.botFourChips.Text = "Chips : " + this.fourthBot.Chips.ToString();
            this.botFiveChips.Text = "Chips : " + this.fifthBot.Chips.ToString();

            if (this.player.Chips <= 0)
            {
                this.player.AbleToMakeTurn = false;
                this.player.OutOfChips = true;
                this.callButton.Enabled = false;
                this.raiseButton.Enabled = false;
                this.foldButton.Enabled = false;
                this.checkButton.Enabled = false;
            }

            if (this.player.Chips >= this.neededChipsToCall)
            {
                this.callButton.Text = "Call " + this.neededChipsToCall.ToString();
            }
            else
            {
                this.callButton.Text = "All in";
                this.raiseButton.Enabled = false;
            }

            if (this.neededChipsToCall > 0)
            {
                this.checkButton.Enabled = false;
            }

            if (this.neededChipsToCall <= 0)
            {
                this.checkButton.Enabled = true;
                this.callButton.Text = "Call";
                this.callButton.Enabled = false;
            }

            if (this.player.Chips <= 0)
            {
                this.raiseButton.Enabled = false;
            }

            int parsedValue;
            if (this.raiseAmountField.Text != string.Empty && int.TryParse(this.raiseAmountField.Text, out parsedValue))
            {
                if (this.player.Chips <= parsedValue)
                {
                    this.raiseButton.Text = "All in";
                }
                else
                {
                    this.raiseButton.Text = "Raise";
                }
            }

            if (this.player.Chips < this.neededChipsToCall)
            {
                this.raiseButton.Enabled = false;
            }
        }

        private async void FoldClick(object sender, EventArgs e)
        {
            this.playerStatus.Text = "Fold";
            this.player.AbleToMakeTurn = false;
            this.player.OutOfChips = true;
            await this.Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (this.neededChipsToCall <= 0)
            {
                this.player.AbleToMakeTurn = false;
                this.playerStatus.Text = "Check";
            }
            else
            {
                //playerStatus.Text = "All in " + Chips;

                this.checkButton.Enabled = false;
            }

            await this.Turns();
        }

        private async void bCall_Click(object sender, EventArgs e)
        {
            this.Rules(0, 1, "Player", this.player);

            if (this.player.Chips >= this.neededChipsToCall)
            {
                this.player.Chips -= this.neededChipsToCall;
                this.playerChips.Text = "Chips : " + this.player.Chips.ToString();

                if (this.potStatus.Text != string.Empty)
                {
                    this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
                }
                else
                {
                    this.potStatus.Text = this.neededChipsToCall.ToString();
                }

                this.player.AbleToMakeTurn = false;
                this.playerStatus.Text = "Call " + this.neededChipsToCall;
                this.player.Call = this.neededChipsToCall;
            }
            else if (this.player.Chips <= this.neededChipsToCall && this.neededChipsToCall > 0)
            {
                this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.player.Chips).ToString();
                this.playerStatus.Text = "All in " + this.player.Chips;
                this.player.Chips = 0;
                this.playerChips.Text = "Chips : " + this.player.Chips.ToString();
                this.player.AbleToMakeTurn = false;
                this.foldButton.Enabled = false;
                this.player.Call = this.player.Chips;
            }

            await this.Turns();
        }

        private async void RaiseClick(object sender, EventArgs e)
        {
            this.Rules(0, 1, "Player", this.player);

            int parsedValue;
            bool isValidNumber = int.TryParse(this.raiseAmountField.Text, out parsedValue);
            if (isValidNumber)
            {
                if (this.player.Chips > this.neededChipsToCall)
                {
                    if (this.raise * 2 > parsedValue)
                    {
                        this.raiseAmountField.Text = (this.raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }

                    if (this.player.Chips >= parsedValue)
                    {
                        this.neededChipsToCall = parsedValue;
                        this.raise = parsedValue;
                        this.playerStatus.Text = "Raise " + this.neededChipsToCall.ToString();
                        this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
                        this.callButton.Text = "Call";
                        this.player.Chips -= parsedValue;
                        this.raising = true;
                        this.player.Raise = this.raise;
                    }
                    // all in scenario
                    else
                    {
                        this.neededChipsToCall = this.player.Chips;
                        this.raise = this.player.Chips;
                        this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.player.Chips).ToString();
                        this.playerStatus.Text = "Raise " + this.neededChipsToCall.ToString();
                        this.player.Chips = 0;
                        this.raising = true;
                        this.player.Raise = this.raise;
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            this.player.AbleToMakeTurn = false;
            await this.Turns();
        }

        /// <summary>
        /// Add chips to all players on table.
        /// </summary>
        private void AddChipsClick(object sender, EventArgs e)
        {
            int addedChips = 0;
            bool isValidNumber = false;
            isValidNumber = int.TryParse(this.addChipsToAllAmount.Text, out addedChips);
            if (isValidNumber && addedChips > 0 && addedChips < GlobalConstants.MaxChipsToAdd)
            {
                this.player.Chips += addedChips;
                this.firstBot.Chips += addedChips;
                this.secondBot.Chips += addedChips;
                this.thirdBot.Chips += addedChips;
                this.fourthBot.Chips += addedChips;
                this.fifthBot.Chips += addedChips;
                this.playerChips.Text = "Chips : " + this.player.Chips.ToString();
            }
            else
            {
                MessageBox.Show("Chips should be positive round number!");
            }
        }

        /// <summary>
        /// Hide or show blind boxes on screen.
        /// </summary>
        private void OptionsClick(object sender, EventArgs e)
        {
            this.bigBlindField.Text = this.bigBlind.ToString();
            this.smallBlindField.Text = this.smallBlind.ToString();
            if (this.bigBlindField.Visible == false)
            {
                this.bigBlindField.Visible = true;
                this.smallBlindField.Visible = true;
                this.bigBlindButton.Visible = true;
                this.smallBlindButton.Visible = true;
            }
            else
            {
                this.bigBlindField.Visible = false;
                this.smallBlindField.Visible = false;
                this.bigBlindButton.Visible = false;
                this.smallBlindButton.Visible = false;
            }
        }

        private void SmallBlindClick(object sender, EventArgs e)
        {

            int parsedValue;
            bool isParsed = int.TryParse(this.smallBlindField.Text, out parsedValue);

            if (this.smallBlindField.Text.Contains(",") || this.smallBlindField.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                this.smallBlindField.Text = this.smallBlind.ToString();
                return;
            }

            if (!isParsed)
            {
                MessageBox.Show("This is a number only field");
                this.smallBlindField.Text = this.smallBlind.ToString();
                return;
            }

            if (parsedValue > GlobalConstants.MaxSmallBlind)
            {
                MessageBox.Show("The maximum of the Small Blind is " + GlobalConstants.MaxSmallBlind);
                this.smallBlindField.Text = this.smallBlind.ToString();
            }

            if (parsedValue < GlobalConstants.MinSmallBlind)
            {
                MessageBox.Show("The minimum of the Small Blind is " + GlobalConstants.MinSmallBlind);
            }

            if (parsedValue >= GlobalConstants.MinSmallBlind && parsedValue <= GlobalConstants.MaxSmallBlind)
            {
                this.smallBlind = parsedValue;
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play.");
            }
        }

        private void bBigBlind_Click(object sender, EventArgs e)
        {

            int parsedValue;
            bool isParsed = int.TryParse(this.bigBlindField.Text, out parsedValue);

            if (this.bigBlindField.Text.Contains(",") || this.bigBlindField.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number!");
                this.bigBlindField.Text = this.bigBlind.ToString();
                return;
            }

            if (!isParsed)
            {
                MessageBox.Show("This is a number only field");
                this.bigBlindField.Text = this.bigBlind.ToString();
                return;
            }

            if (parsedValue > GlobalConstants.MaxBigBlind)
            {
                MessageBox.Show("The maximum of the Big Blind is " + GlobalConstants.MaxBigBlind);
                this.bigBlindField.Text = this.bigBlind.ToString();
            }
            else if (parsedValue < GlobalConstants.MinBigBlind)
            {
                MessageBox.Show("The minimum of the Big Blind is " + GlobalConstants.MinBigBlind);
            }

            if (parsedValue >= GlobalConstants.MinBigBlind && parsedValue <= GlobalConstants.MinBigBlind)
            {
                this.bigBlind = parsedValue;
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play.");
            }
        }


        // TODO : Too many invokes.
        //private void Layout_Change(object sender, LayoutEventArgs e)
        //{
        //    this.width = this.Width;
        //    this.height = this.Height;
        //}
        #endregion
    }
}