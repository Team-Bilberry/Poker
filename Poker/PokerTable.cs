namespace Poker
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

    public partial class PokerTable : Form
    {
        #region Variables

        private readonly IPokerPlayer player;
        private readonly IPokerPlayer firstBot;
        private readonly IPokerPlayer secondBot;
        private readonly IPokerPlayer thirdBot;
        private readonly IPokerPlayer fourthBot;
        private readonly IPokerPlayer fifthBot;

        private int neededChipsToCall = 500;
        private int botsWithoutChips = 5;
        private double type;
        private double rounds = 0;
        private int raise = 0;
        private int playersLeft = 6;
        private int winners = 0;

        private bool intsadded;
        private bool changed;

        private int height;
        private int width;

        // TODO: Convert to enum
        
        private int Flop = 1;
        private int Turn = 2;
        private int River = 3;
        private int End = 4;


        //int last = 123;
        private int raisedTurn = 1;

        List<bool?> arePlayersEliminated = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        private bool restart = false;
        private bool raising = false;

        Poker.Type sorted;
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

        public PokerTable()
        {
            this.player = new PokerPlayer(new Panel());
            this.firstBot = new PokerPlayer(new Panel());
            this.secondBot = new PokerPlayer(new Panel());
            this.thirdBot = new PokerPlayer(new Panel());
            this.fourthBot = new PokerPlayer(new Panel());
            this.fifthBot = new PokerPlayer(new Panel());

            Deck deck = new Deck();

            //bools.Add(PlayerFoldTurn); bools.Add(bot1.OutOfChips); bools.Add(bot2.FoldedTurn); bools.Add(bot3.FoldedTurn); bools.Add(bot4.FoldedTurn); bools.Add(bot5.FoldedTurn);
            neededChipsToCall = this.bigBlind;
            MaximizeBox = false;
            MinimizeBox = false;
            updates.Start();
            InitializeComponent();

            width = this.Width;
            height = this.Height;

            Shuffle();

            potStatus.Enabled = false;
            playerChips.Enabled = false;
            botOneChips.Enabled = false;
            botTwoChips.Enabled = false;
            botThreeChips.Enabled = false;
            botFourChips.Enabled = false;
            botFiveChips.Enabled = false;

            playerChips.Text = "Chips : " + this.player.Chips;
            botOneChips.Text = "Chips : " + this.firstBot.Chips;
            botTwoChips.Text = "Chips : " + this.secondBot.Chips;
            botThreeChips.Text = "Chips : " + this.thirdBot.Chips;
            botFourChips.Text = "Chips : " + this.fourthBot.Chips;
            botFiveChips.Text = "Chips : " + this.fifthBot.Chips;

            timer.Interval = 1000;
            timer.Tick += TimerTick;
            updates.Interval = 100;
            updates.Tick += Update_Tick;

            raiseAmountField.Text = (this.bigBlind * 2).ToString();

            player.OutOfChips = false;
            player.AbleToMakeTurn = true;
        }

        async Task Shuffle()
        {
            arePlayersEliminated.Add(this.player.OutOfChips);
            arePlayersEliminated.Add(this.firstBot.OutOfChips);
            arePlayersEliminated.Add(this.secondBot.OutOfChips);
            arePlayersEliminated.Add(this.thirdBot.OutOfChips);
            arePlayersEliminated.Add(this.fourthBot.OutOfChips);
            arePlayersEliminated.Add(this.fifthBot.OutOfChips);

            callButton.Enabled = false;
            raiseButton.Enabled = false;
            foldButton.Enabled = false;
            checkButton.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            bool check = false;

            Bitmap backImage = new Bitmap(@"..\..\Resources\Assets\Back\Back.png");
            int horizontal = 580;
            int vertical = 480;

            ShuffleCards();

            // TODO: move it to proper place.
            const int neededCardsFromDeck = 17; // 6 players * 2 card + 5 card on table

            for (int currentCardIndex = 0; currentCardIndex < neededCardsFromDeck; currentCardIndex++)
            {
                deck[currentCardIndex] = Image.FromFile(ImgLocation[currentCardIndex]);

                // take card name, too slow i think
                var charsToRemove = new string[] { @"..\..\Resources\Assets\Cards\", ".png" };
                foreach (var c in charsToRemove)
                {
                    ImgLocation[currentCardIndex] = ImgLocation[currentCardIndex].Replace(c, string.Empty);
                }
                int lastSlashIndex = this.ImgLocation[currentCardIndex].LastIndexOf("\\");
                int lastDotIndex = this.ImgLocation[currentCardIndex].LastIndexOf(".");
                this.ImgLocation[currentCardIndex] = this.ImgLocation[currentCardIndex]
                    .Substring(lastSlashIndex + 1, lastDotIndex - lastSlashIndex - 1);

                reserve[currentCardIndex] = int.Parse(ImgLocation[currentCardIndex]) - 1;

                cardPicture[currentCardIndex] = new PictureBox();
                cardPicture[currentCardIndex].SizeMode = PictureBoxSizeMode.StretchImage;
                cardPicture[currentCardIndex].Height = 130;
                cardPicture[currentCardIndex].Width = 80;
                this.Controls.Add(cardPicture[currentCardIndex]);
                cardPicture[currentCardIndex].Name = "pb" + currentCardIndex.ToString();

                // investigate why these delay is needed
                //await Task.Delay(200);
                #region Throwing Cards
                if (currentCardIndex < 2)
                {
                    cardPicture[currentCardIndex].Tag = reserve[currentCardIndex];
                    cardPicture[currentCardIndex].Image = deck[currentCardIndex];
                    cardPicture[currentCardIndex].Anchor = AnchorStyles.Bottom;
                    cardPicture[currentCardIndex].Dock = DockStyle.Top;
                    cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                    horizontal += cardPicture[currentCardIndex].Width;

                    this.Controls.Add(this.player.Panel);
                    var playerPanelLocation = new Point(cardPicture[0].Left - 10, cardPicture[0].Top - 10);
                    this.player.InizializePanel(playerPanelLocation);
                }

                if (this.firstBot.Chips > 0)
                {
                    botsWithoutChips--;
                    if (currentCardIndex >= 2 && currentCardIndex < 4)
                    {
                        cardPicture[currentCardIndex].Tag = reserve[currentCardIndex];

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

                        cardPicture[currentCardIndex].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        cardPicture[currentCardIndex].Image = backImage;
                        cardPicture[currentCardIndex].Image = Deck[currentCardIndex];
                        cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += cardPicture[currentCardIndex].Width;
                        cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.firstBot.Panel);
                        var bot1PanelLocation = new Point(cardPicture[2].Left - 10, cardPicture[2].Top - 10);
                        this.firstBot.InizializePanel(bot1PanelLocation);
                    }
                }

                if (secondBot.Chips > 0)
                {
                    botsWithoutChips--;
                    if (currentCardIndex >= 4 && currentCardIndex < 6)
                    {
                        cardPicture[currentCardIndex].Tag = reserve[currentCardIndex];

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

                        cardPicture[currentCardIndex].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        cardPicture[currentCardIndex].Image = backImage;
                        cardPicture[currentCardIndex].Image = Deck[currentCardIndex];
                        cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += cardPicture[currentCardIndex].Width;
                        cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.secondBot.Panel);
                        var bot2PanelLocation = new Point(cardPicture[4].Left - 10, cardPicture[4].Top - 10);
                        this.secondBot.InizializePanel(bot2PanelLocation);
                    }
                }

                if (this.thirdBot.Chips > 0)
                {
                    botsWithoutChips--;
                    if (currentCardIndex >= 6 && currentCardIndex < 8)
                    {
                        cardPicture[currentCardIndex].Tag = reserve[currentCardIndex];

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

                        cardPicture[currentCardIndex].Anchor = (AnchorStyles.Top);
                        cardPicture[currentCardIndex].Image = backImage;
                        cardPicture[currentCardIndex].Image = Deck[currentCardIndex];
                        cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += cardPicture[currentCardIndex].Width;
                        cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.thirdBot.Panel);
                        var bot3PanelLocation = new Point(cardPicture[6].Left - 10, cardPicture[6].Top - 10);
                        this.thirdBot.InizializePanel(bot3PanelLocation);
                    }
                }

                if (fourthBot.Chips > 0)
                {
                    botsWithoutChips--;
                    if (currentCardIndex >= 8 && currentCardIndex < 10)
                    {
                        cardPicture[currentCardIndex].Tag = reserve[currentCardIndex];

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

                        cardPicture[currentCardIndex].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        cardPicture[currentCardIndex].Image = backImage;
                        cardPicture[currentCardIndex].Image = Deck[currentCardIndex];
                        cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += cardPicture[currentCardIndex].Width;
                        cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.fourthBot.Panel);
                        var bot4PanelLocation = new Point(cardPicture[8].Left - 10, cardPicture[8].Top - 10);
                        this.fourthBot.InizializePanel(bot4PanelLocation);
                    }
                }

                if (fifthBot.Chips > 0)
                {
                    botsWithoutChips--;
                    if (currentCardIndex >= 10 && currentCardIndex < 12)
                    {
                        cardPicture[currentCardIndex].Tag = reserve[currentCardIndex];

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

                        cardPicture[currentCardIndex].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        cardPicture[currentCardIndex].Image = backImage;
                        cardPicture[currentCardIndex].Image = Deck[currentCardIndex];
                        cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += cardPicture[currentCardIndex].Width;
                        cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.fifthBot.Panel);
                        var bot5PanelLocation = new Point(cardPicture[10].Left - 10, cardPicture[10].Top - 10);
                        this.fifthBot.InizializePanel(bot5PanelLocation);
                    }
                }

                if (currentCardIndex >= 12)
                {
                    cardPicture[currentCardIndex].Tag = reserve[currentCardIndex];

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;

                    cardPicture[currentCardIndex].Anchor = AnchorStyles.None;
                    cardPicture[currentCardIndex].Image = backImage;
                    cardPicture[currentCardIndex].Image = Deck[currentCardIndex];
                    cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                    horizontal += 110;
                }
                #endregion
                if (firstBot.Chips <= 0)
                {
                    this.firstBot.OutOfChips = true;
                    cardPicture[2].Visible = false;
                    cardPicture[3].Visible = false;
                }
                else
                {
                    this.firstBot.OutOfChips = false;
                    if (currentCardIndex == 3)
                    {
                        if (cardPicture[3] != null)
                        {
                            cardPicture[2].Visible = true;
                            cardPicture[3].Visible = true;
                        }
                    }
                }

                if (secondBot.Chips <= 0)
                {
                    this.secondBot.OutOfChips = true;
                    cardPicture[4].Visible = false;
                    cardPicture[5].Visible = false;
                }
                else
                {
                    this.secondBot.OutOfChips = false;
                    if (currentCardIndex == 5)
                    {
                        if (cardPicture[5] != null)
                        {
                            cardPicture[4].Visible = true;
                            cardPicture[5].Visible = true;
                        }
                    }
                }

                if (thirdBot.Chips <= 0)
                {
                    this.thirdBot.OutOfChips = true;
                    cardPicture[6].Visible = false;
                    cardPicture[7].Visible = false;
                }
                else
                {
                    this.thirdBot.OutOfChips = false;
                    if (currentCardIndex == 7)
                    {
                        if (cardPicture[7] != null)
                        {
                            cardPicture[6].Visible = true;
                            cardPicture[7].Visible = true;
                        }
                    }
                }

                if (fourthBot.Chips <= 0)
                {
                    this.fourthBot.OutOfChips = true;
                    cardPicture[8].Visible = false;
                    cardPicture[9].Visible = false;
                }
                else
                {
                    this.fourthBot.OutOfChips = false;
                    if (currentCardIndex == 9)
                    {
                        if (cardPicture[9] != null)
                        {
                            cardPicture[8].Visible = true;
                            cardPicture[9].Visible = true;
                        }
                    }
                }

                if (fifthBot.Chips <= 0)
                {
                    this.fifthBot.OutOfChips = true;
                    cardPicture[10].Visible = false;
                    cardPicture[11].Visible = false;
                }
                else
                {
                    this.fifthBot.OutOfChips = false;
                    if (currentCardIndex == 11)
                    {
                        if (cardPicture[11] != null)
                        {
                            cardPicture[10].Visible = true;
                            cardPicture[11].Visible = true;
                        }
                    }
                }

                if (currentCardIndex == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }

                    timer.Start();
                }
            }

            if (botsWithoutChips == 5)
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
                botsWithoutChips = 5;
            }

            raiseButton.Enabled = true;
            callButton.Enabled = true;
            raiseButton.Enabled = true;
            raiseButton.Enabled = true;
            foldButton.Enabled = true;
        }

        private void ShuffleCards()
        {
            Random rnd = new Random();
            for (int currentIndex = ImgLocation.Length; currentIndex > 0; currentIndex--)
            {
                int swapCardIndex = rnd.Next(currentIndex);
                var tempCard = ImgLocation[swapCardIndex];
                ImgLocation[swapCardIndex] = ImgLocation[currentIndex - 1];
                ImgLocation[currentIndex - 1] = tempCard;
            }
        }

        private async Task Turns()
        {
            #region Rotating
            if (!this.player.OutOfChips && this.player.AbleToMakeTurn)
            {
                FixCall(this.playerStatus, this.player, 1);

                //MessageBox.Show("Player's Turn");
                pbTimer.Visible = true;
                pbTimer.Value = 1000;
                secondsLeft = 60;

                timer.Start();
                raiseButton.Enabled = true;
                callButton.Enabled = true;
                foldButton.Enabled = true;
                turnCount++;
                FixCall(this.playerStatus, this.player, 2);
            }

            if (this.player.OutOfChips || !this.player.AbleToMakeTurn)
            {
                await AllIn();

                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (callButton.Text.Contains("All in") == false 
                        || raiseButton.Text.Contains("All in") == false)
                    {
                        //bools.RemoveAt(0);
                        //bools.Insert(0, null);
                        this.arePlayersEliminated[0] = null;
                        playersLeft--;
                        this.player.Folded = true;
                    }
                }

                await CheckRaise(0, 0);

                pbTimer.Visible = false;
                raiseButton.Enabled = false;
                callButton.Enabled = false;
                raiseButton.Enabled = false;
                raiseButton.Enabled = false;
                foldButton.Enabled = false;
                timer.Stop();
                this.firstBot.AbleToMakeTurn = true;

                if (!this.firstBot.OutOfChips)
                {
                    if (this.firstBot.AbleToMakeTurn)
                    {
                        FixCall(this.botOneStatus, this.firstBot, 1);
                        FixCall(this.botOneStatus, this.firstBot, 2);

                        Rules(2, 3, "Bot 1", this.firstBot);
                        MessageBox.Show("Bot 1's Turn");

                        AI(2, 3, this.botOneStatus, 0, this.firstBot);

                        turnCount++;
                        this.firstBot.AbleToMakeTurn = false;
                        this.secondBot.AbleToMakeTurn = true;
                    }
                }

                if (this.firstBot.OutOfChips && !this.firstBot.Folded)
                {
                    arePlayersEliminated.RemoveAt(1);
                    arePlayersEliminated.Insert(1, null);
                    playersLeft--;
                    this.firstBot.Folded = true;
                }

                if (this.firstBot.OutOfChips || !this.firstBot.AbleToMakeTurn)
                {
                    await CheckRaise(1, 1);
                    this.secondBot.AbleToMakeTurn = true;
                }

                if (!this.secondBot.OutOfChips)
                {
                    if (this.secondBot.AbleToMakeTurn)
                    {
                        FixCall(botTwoStatus, this.secondBot, 1);
                        FixCall(botTwoStatus, this.secondBot, 2);
                        Rules(4, 5, "Bot 2", this.secondBot);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, botTwoStatus, 1, this.secondBot);
                        turnCount++;
                        this.secondBot.AbleToMakeTurn = false;
                        this.thirdBot.AbleToMakeTurn = true;
                    }
                }

                if (this.secondBot.OutOfChips && !this.secondBot.Folded)
                {
                    arePlayersEliminated.RemoveAt(2);
                    arePlayersEliminated.Insert(2, null);
                    playersLeft--;
                    this.secondBot.Folded = true;
                }

                if (this.secondBot.OutOfChips || !this.secondBot.AbleToMakeTurn)
                {
                    await CheckRaise(2, 2);
                    this.thirdBot.AbleToMakeTurn = true;
                }

                if (!this.thirdBot.OutOfChips)
                {
                    if (this.thirdBot.AbleToMakeTurn)
                    {
                        FixCall(botThreeStatus, this.thirdBot, 1);
                        FixCall(botThreeStatus, this.thirdBot, 2);

                        Rules(6, 7, "Bot 3", this.thirdBot);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, botThreeStatus, 2, this.thirdBot);
                        turnCount++;
                        this.thirdBot.AbleToMakeTurn = false;
                        this.fourthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.thirdBot.OutOfChips && !this.thirdBot.Folded)
                {
                    arePlayersEliminated.RemoveAt(3);
                    arePlayersEliminated.Insert(3, null);
                    playersLeft--;
                    this.thirdBot.Folded = true;
                }

                if (this.thirdBot.OutOfChips || !this.thirdBot.AbleToMakeTurn)
                {
                    await CheckRaise(3, 3);
                    this.fourthBot.AbleToMakeTurn = true;
                }

                if (!this.fourthBot.OutOfChips)
                {
                    if (this.fourthBot.AbleToMakeTurn)
                    {
                        FixCall(botFourStatus, this.fourthBot, 1);
                        FixCall(botFourStatus, this.fourthBot, 2);
                        Rules(8, 9, "Bot 4", this.fourthBot);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, botFourStatus, 3, this.fourthBot);
                        turnCount++;
                        this.fourthBot.AbleToMakeTurn = false;
                        this.fifthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.fourthBot.OutOfChips && !this.fourthBot.Folded)
                {
                    arePlayersEliminated.RemoveAt(4);
                    arePlayersEliminated.Insert(4, null);
                    playersLeft--;
                    this.fourthBot.Folded = true;
                }

                if (this.fourthBot.OutOfChips || !this.fourthBot.AbleToMakeTurn)
                {
                    await CheckRaise(4, 4);
                    this.fifthBot.AbleToMakeTurn = true;
                }

                if (!this.fifthBot.OutOfChips)
                {
                    if (this.fifthBot.AbleToMakeTurn)
                    {
                        FixCall(botFiveStatus, this.fifthBot, 1);
                        FixCall(botFiveStatus, this.fifthBot, 2);
                        Rules(10, 11, "Bot 5", this.fifthBot);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, botFiveStatus, 4, this.fifthBot);
                        turnCount++;
                        this.fifthBot.AbleToMakeTurn = false;
                    }
                }

                if (this.fifthBot.OutOfChips && !this.fifthBot.Folded)
                {
                    arePlayersEliminated.RemoveAt(5);
                    arePlayersEliminated.Insert(5, null);
                    playersLeft--;
                    this.fifthBot.Folded = true;
                }

                if (this.fifthBot.OutOfChips || !this.fifthBot.AbleToMakeTurn)
                {
                    await CheckRaise(5, 5);
                    this.player.AbleToMakeTurn = true;
                }

                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (callButton.Text.Contains("All in") == false || raiseButton.Text.Contains("All in") == false)
                    {
                        arePlayersEliminated.RemoveAt(0);
                        arePlayersEliminated.Insert(0, null);
                        playersLeft--;
                        this.player.Folded = true;
                    }
                }

                #endregion
                await AllIn();
                if (!restart)
                {
                    await Turns();
                }

                restart = false;
            }
        }

        void Rules(int card1, int card2, string currentText, IPokerPlayer pokerPlayer)
        {
            if (card1 == 0 && card2 == 1)
            {
            }

            if (!pokerPlayer.OutOfChips || card1 == 0 && card2 == 1 && this.playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false;
                bool vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = reserve[card1];
                Straight[1] = reserve[card2];
                Straight1[0] = Straight[2] = reserve[12];
                Straight1[1] = Straight[3] = reserve[13];
                Straight1[2] = Straight[4] = reserve[14];
                Straight1[3] = Straight[5] = reserve[15];
                Straight1[4] = Straight[6] = reserve[16];
                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(Straight); Array.Sort(st1); Array.Sort(st2); Array.Sort(st3); Array.Sort(st4);
                #endregion
                for (int index = 0; index < 16; index++)
                {
                    if (reserve[index] == int.Parse(cardPicture[card1].Tag.ToString()) && reserve[index + 1] == int.Parse(cardPicture[card2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1
                        rPairFromHand(pokerPlayer, index);

                        rPairTwoPair(pokerPlayer, index);

                        rTwoPair(pokerPlayer, index);

                        rThreeOfAKind(pokerPlayer, Straight, index);

                        RStraight(pokerPlayer, Straight, index);

                        RFlush(pokerPlayer, ref vf, Straight1, index);

                        rFullHouse(pokerPlayer, ref done, Straight);

                        rFourOfAKind(pokerPlayer, Straight);

                        rStraightFlush(pokerPlayer, st1, st2, st3, st4);

                        rHighCard(pokerPlayer, index);
                    }
                }
            }
        }

        private void rStraightFlush(IPokerPlayer pokerPlayer, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (pokerPlayer.Type >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        pokerPlayer.Type = 8;
                        pokerPlayer.Power = (st1.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        pokerPlayer.Type = 9;
                        pokerPlayer.Power = (st1.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        pokerPlayer.Type = 8;
                        pokerPlayer.Power = (st2.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        pokerPlayer.Type = 9;
                        pokerPlayer.Power = (st2.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        pokerPlayer.Type = 8;
                        pokerPlayer.Power = (st3.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        pokerPlayer.Type = 9;
                        pokerPlayer.Power = (st3.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        pokerPlayer.Type = 8;
                        pokerPlayer.Power = (st4.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        pokerPlayer.Type = 9;
                        pokerPlayer.Power = (st4.Max()) / 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rFourOfAKind(IPokerPlayer pokerPlayer, int[] Straight)
        {
            if (pokerPlayer.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        pokerPlayer.Type = 7;
                        pokerPlayer.Power = (Straight[j] / 4) * 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        pokerPlayer.Type = 7;
                        pokerPlayer.Power = 13 * 4 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rFullHouse(IPokerPlayer pokerPlayer, ref bool done, int[] Straight)
        {
            if (pokerPlayer.Type >= -1)
            {
                type = pokerPlayer.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                pokerPlayer.Type = 6;
                                pokerPlayer.Power = 13 * 2 + pokerPlayer.Type * 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                pokerPlayer.Type = 6;
                                pokerPlayer.Power = fh.Max() / 4 * 2 + pokerPlayer.Type * 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                pokerPlayer.Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                pokerPlayer.Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (pokerPlayer.Type != 6)
                {
                    pokerPlayer.Power = type;
                }
            }
        }

        private void RFlush(IPokerPlayer pokerPlayer, ref bool vf, int[] straight1, int index)
        {
            if (pokerPlayer.Type >= -1)
            {
                var f1 = straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f1[0] % 4)
                    {
                        if (reserve[index] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f1.Max() / 4 && reserve[index + 1] / 4 < f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f1[0] % 4)
                    {
                        if (reserve[index] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f1[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (reserve[index] % 4 == f1[0] % 4 && reserve[index] / 4 > f1.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f1[0] % 4 && reserve[index + 1] / 4 > f1.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f1.Min() / 4 && reserve[index + 1] / 4 < f1.Min())
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = f1.Max() + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f2[0] % 4)
                    {
                        if (reserve[index] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f2.Max() / 4 && reserve[index + 1] / 4 < f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f2[0] % 4)
                    {
                        if (reserve[index] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f2[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (reserve[index] % 4 == f2[0] % 4 && reserve[index] / 4 > f2.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f2[0] % 4 && reserve[index + 1] / 4 > f2.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f2.Min() / 4 && reserve[index + 1] / 4 < f2.Min())
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = f2.Max() + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f3[0] % 4)
                    {
                        if (reserve[index] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f3.Max() / 4 && reserve[index + 1] / 4 < f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f3[0] % 4)
                    {
                        if (reserve[index] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f3[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (reserve[index] % 4 == f3[0] % 4 && reserve[index] / 4 > f3.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f3[0] % 4 && reserve[index + 1] / 4 > f3.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f3.Min() / 4 && reserve[index + 1] / 4 < f3.Min())
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = f3.Max() + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f4[0] % 4)
                    {
                        if (reserve[index] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f4.Max() / 4 && reserve[index + 1] / 4 < f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f4[0] % 4)
                    {
                        if (reserve[index] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f4[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (reserve[index] % 4 == f4[0] % 4 && reserve[index] / 4 > f4.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f4[0] % 4 && reserve[index + 1] / 4 > f4.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f4.Min() / 4 && reserve[index + 1] / 4 < f4.Min())
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = f4.Max() + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                //ace
                if (f1.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f4[0] % 4 && vf)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void RStraight(IPokerPlayer pokerPlayer, int[] Straight, int index)
        {
            if (pokerPlayer.Type >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            pokerPlayer.Type = 4;
                            pokerPlayer.Power = op.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            pokerPlayer.Type = 4;
                            pokerPlayer.Power = op[j + 4] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        pokerPlayer.Type = 4;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rThreeOfAKind(IPokerPlayer pokerPlayer, int[] Straight, int index)
        {
            if (pokerPlayer.Type >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            pokerPlayer.Type = 3;
                            pokerPlayer.Power = 13 * 3 + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            pokerPlayer.Type = 3;
                            pokerPlayer.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        private void rTwoPair(IPokerPlayer pokerPlayer, int index)
        {
            if (pokerPlayer.Type >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (reserve[index] / 4 != reserve[index + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (reserve[index] / 4 == reserve[tc] / 4 && reserve[index + 1] / 4 == reserve[tc - k] / 4 ||
                                    reserve[index + 1] / 4 == reserve[tc] / 4 && reserve[index] / 4 == reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[index] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = 13 * 4 + (reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted =
                                                Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index + 1] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = 13 * 4 + (reserve[index] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index + 1] / 4 != 0 && reserve[index] / 4 != 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (reserve[index] / 4) * 2 + (reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairTwoPair(IPokerPlayer pokerPlayer, int index)
        {
            if (pokerPlayer.Type >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (reserve[tc] / 4 == reserve[tc - k] / 4)
                            {
                                if (reserve[tc] / 4 != reserve[index] / 4 && reserve[tc] / 4 != reserve[index + 1] / 4 && pokerPlayer.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[index + 1] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (reserve[index] / 4) * 2 + 13 * 4 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (reserve[index + 1] / 4) * 2 + 13 * 4 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index + 1] / 4 != 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (reserve[tc] / 4) * 2 + (reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index] / 4 != 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (reserve[tc] / 4) * 2 + (reserve[index] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (pokerPlayer.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (reserve[index] / 4 > reserve[index + 1] / 4)
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = 13 + reserve[index] / 4 + pokerPlayer.Type * 100;
                                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = reserve[tc] / 4 + reserve[index] / 4 + pokerPlayer.Type * 100;
                                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = 13 + reserve[index + 1] + pokerPlayer.Type * 100;
                                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = reserve[tc] / 4 + reserve[index + 1] / 4 + pokerPlayer.Type * 100;
                                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairFromHand(IPokerPlayer pokerPlayer, int index)
        {
            if (pokerPlayer.Type >= -1)
            {
                bool msgbox = false;
                if (reserve[index] / 4 == reserve[index + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (reserve[index] / 4 == 0)
                        {
                            pokerPlayer.Type = 1;
                            pokerPlayer.Power = 13 * 4 + 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            pokerPlayer.Type = 1;
                            pokerPlayer.Power = (reserve[index + 1] / 4) * 4 + 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (reserve[index + 1] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[index + 1] / 4 == 0)
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = 13 * 4 + reserve[index] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = (reserve[index + 1] / 4) * 4 + reserve[index] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }

                    if (reserve[index] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[index] / 4 == 0)
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = 13 * 4 + reserve[index + 1] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = (reserve[tc] / 4) * 4 + reserve[index + 1] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }

        private void rHighCard(IPokerPlayer pokerPlayer, int index)
        {
            if (pokerPlayer.Type == -1)
            {
                if (reserve[index] / 4 > reserve[index + 1] / 4)
                {
                    pokerPlayer.Type = -1;
                    pokerPlayer.Power = reserve[index] / 4;
                    Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    pokerPlayer.Type = -1;
                    pokerPlayer.Power = reserve[index + 1] / 4;
                    Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (reserve[index] / 4 == 0 || reserve[index + 1] / 4 == 0)
                {
                    pokerPlayer.Type = -1;
                    pokerPlayer.Power = 13;
                    Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(double current, double Power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (cardPicture[j].Visible)
                {
                    cardPicture[j].Image = deck[j];
                }
            }
            if (current == sorted.Current)
            {
                if (Power == sorted.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
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
                if (winners > 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(potStatus.Text) / winners;
                        playerChips.Text = player.Chips.ToString();
                        //player.Panel.Visible = true;

                    }

                    if (CheckWinners.Contains("Bot 1"))
                    {
                        firstBot.Chips += int.Parse(potStatus.Text) / winners;
                        botOneChips.Text = firstBot.Chips.ToString();
                        //bot1.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 2"))
                    {
                        secondBot.Chips += int.Parse(potStatus.Text) / winners;
                        botTwoChips.Text = secondBot.Chips.ToString();
                        //bot2.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 3"))
                    {
                        thirdBot.Chips += int.Parse(potStatus.Text) / winners;
                        botThreeChips.Text = thirdBot.Chips.ToString();
                        //bot3.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 4"))
                    {
                        fourthBot.Chips += int.Parse(potStatus.Text) / winners;
                        botFourChips.Text = fourthBot.Chips.ToString();
                        //bot4.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 5"))
                    {
                        fifthBot.Chips += int.Parse(potStatus.Text) / winners;
                        botFiveChips.Text = fifthBot.Chips.ToString();
                        //bot5.Panel.Visible = true;
                    }

                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(potStatus.Text);
                        //await Finish(1);
                        //player.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 1"))
                    {
                        firstBot.Chips += int.Parse(potStatus.Text);
                        //await Finish(1);
                        //bot1.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 2"))
                    {
                        secondBot.Chips += int.Parse(potStatus.Text);
                        //await Finish(1);
                        //bot2.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 3"))
                    {
                        thirdBot.Chips += int.Parse(potStatus.Text);
                        //await Finish(1);
                        //bot3.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 4"))
                    {
                        fourthBot.Chips += int.Parse(potStatus.Text);
                        //await Finish(1);
                        //bot4.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 5"))
                    {
                        fifthBot.Chips += int.Parse(potStatus.Text);
                        //await Finish(1);
                        //bot5.Panel.Visible = true;
                    }
                }
            }
        }

        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (raising)
            {
                turnCount = 0;
                raising = false;
                raisedTurn = currentTurn;
                changed = true;
            }
            else
            {
                if (turnCount >= playersLeft - 1 || !changed && turnCount == playersLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == playersLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        raise = 0;
                        neededChipsToCall = 0;
                        raisedTurn = 123;
                        rounds++;
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
                            botTwoStatus.Text = string.Empty;
                        }

                        if (!this.thirdBot.OutOfChips)
                        {
                            botThreeStatus.Text = string.Empty;
                        }

                        if (!this.fourthBot.OutOfChips)
                        {
                            botFourStatus.Text = string.Empty;
                        }

                        if (!this.fifthBot.OutOfChips)
                        {
                            botFiveStatus.Text = string.Empty;
                        }
                    }
                }
            }

            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (cardPicture[j].Image != deck[j])
                    {
                        cardPicture[j].Image = deck[j];

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

            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (cardPicture[j].Image != deck[j])
                    {
                        cardPicture[j].Image = deck[j];

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

            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (cardPicture[j].Image != deck[j])
                    {
                        cardPicture[j].Image = deck[j];
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

            if (rounds == End && playersLeft == 6)
            {
                string fixedLast = string.Empty;

                if (!this.playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", this.player);
                }

                if (!this.botOneStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", this.firstBot);
                }

                if (!botTwoStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", this.secondBot);
                }

                if (!botThreeStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", this.thirdBot);
                }

                if (!botFourStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", this.fourthBot);
                }

                if (!botFiveStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", this.fifthBot);
                }
                Winner(player.Type, this.player.Power, "Player", player.Chips, fixedLast);
                Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", firstBot.Chips, fixedLast);
                Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", secondBot.Chips, fixedLast);
                Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", thirdBot.Chips, fixedLast);
                Winner(this.fourthBot.Type, this.fourthBot.Power, "Bot 4", fourthBot.Chips, fixedLast);
                Winner(this.fifthBot.Type, this.fifthBot.Power, "Bot 5", fifthBot.Chips, fixedLast);
                restart = true;
                this.player.AbleToMakeTurn = true;
                this.player.OutOfChips = false;
                this.firstBot.OutOfChips = false;
                this.secondBot.OutOfChips = false;
                this.thirdBot.OutOfChips = false;
                this.fourthBot.OutOfChips = false;
                this.fifthBot.OutOfChips = false;

                // TODO: Add chips on two place.
                if (player.Chips <= 0)
                {
                    AddChips addChips = new AddChips();
                    addChips.ShowDialog();
                    if (addChips.AddedChips != 0)
                    {
                        player.Chips = addChips.AddedChips;
                        firstBot.Chips += addChips.AddedChips;
                        secondBot.Chips += addChips.AddedChips;
                        thirdBot.Chips += addChips.AddedChips;
                        fourthBot.Chips += addChips.AddedChips;
                        fifthBot.Chips += addChips.AddedChips;
                        this.player.OutOfChips = false;
                        this.player.AbleToMakeTurn = true;
                        raiseButton.Enabled = true;
                        foldButton.Enabled = true;
                        checkButton.Enabled = true;
                        raiseButton.Text = "Raise";
                    }
                }

                this.player.Panel.Visible = false; this.firstBot.Panel.Visible = false; this.secondBot.Panel.Visible = false; this.thirdBot.Panel.Visible = false; this.fourthBot.Panel.Visible = false; this.fifthBot.Panel.Visible = false;
                this.player.Call = 0; this.player.Raise = 0;
                this.firstBot.Call = 0; this.firstBot.Raise = 0;
                this.secondBot.Call = 0; this.secondBot.Raise = 0;
                this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                this.fourthBot.Call = 0; this.fourthBot.Raise = 0;
                this.fifthBot.Call = 0; this.fifthBot.Raise = 0;
                neededChipsToCall = this.bigBlind;
                raise = 0;
                ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);
                arePlayersEliminated.Clear();
                rounds = 0;
                this.player.Power = 0; player.Type = -1;
                type = 0; this.firstBot.Power = 0; this.secondBot.Power = 0; this.thirdBot.Power = 0; this.fourthBot.Power = 0; this.fifthBot.Power = 0;
                this.firstBot.Type = -1; this.secondBot.Type = -1; this.thirdBot.Type = -1; this.fourthBot.Type = -1; this.fifthBot.Type = -1;
                ints.Clear();
                CheckWinners.Clear();
                winners = 0;
                Win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    cardPicture[os].Image = null;
                    cardPicture[os].Invalidate();
                    cardPicture[os].Visible = false;
                }
                potStatus.Text = "0";
                this.playerStatus.Text = string.Empty;
                await Shuffle();
                await Turns();
            }
        }

        void FixCall(Label status, IPokerPlayer pokerPlayer, int options)
        {
            if (rounds != 4)
            {
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
                    if (pokerPlayer.Raise != raise && pokerPlayer.Raise <= raise)
                    {
                        neededChipsToCall = raise - pokerPlayer.Raise;
                    }

                    if (pokerPlayer.Call != neededChipsToCall || pokerPlayer.Call <= neededChipsToCall)
                    {
                        neededChipsToCall = neededChipsToCall - pokerPlayer.Call;
                    }

                    // TODO: check when this is valid and change text in call label
                    if (pokerPlayer.Raise == raise && raise > 0)
                    {
                        neededChipsToCall = 0;
                        callButton.Enabled = false;
                        callButton.Text = "Callisfuckedup";
                    }
                }
            }
        }

        async Task AllIn()
        {
            #region All in
            if (player.Chips <= 0 && !intsadded)
            {
                if (this.playerStatus.Text.Contains("Raise"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
                if (this.playerStatus.Text.Contains("Call"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
            }
            intsadded = false;
            if (firstBot.Chips <= 0 && !this.firstBot.OutOfChips)
            {
                if (!intsadded)
                {
                    ints.Add(firstBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (secondBot.Chips <= 0 && !this.secondBot.OutOfChips)
            {
                if (!intsadded)
                {
                    ints.Add(secondBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (thirdBot.Chips <= 0 && !this.thirdBot.OutOfChips)
            {
                if (!intsadded)
                {
                    ints.Add(thirdBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (fourthBot.Chips <= 0 && !this.fourthBot.OutOfChips)
            {
                if (!intsadded)
                {
                    ints.Add(fourthBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (fifthBot.Chips <= 0 && !this.fifthBot.OutOfChips)
            {
                if (!intsadded)
                {
                    ints.Add(fifthBot.Chips);
                    intsadded = true;
                }
            }
            if (ints.ToArray().Length == playersLeft)
            {
                await Finish(2);
            }
            else
            {
                ints.Clear();
            }
            #endregion

            var abc = arePlayersEliminated.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = arePlayersEliminated.IndexOf(false);
                if (index == 0)
                {
                    player.Chips += int.Parse(potStatus.Text);
                    playerChips.Text = player.Chips.ToString();
                    this.player.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    firstBot.Chips += int.Parse(potStatus.Text);
                    playerChips.Text = firstBot.Chips.ToString();
                    this.firstBot.Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    secondBot.Chips += int.Parse(potStatus.Text);
                    playerChips.Text = secondBot.Chips.ToString();
                    this.secondBot.Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    thirdBot.Chips += int.Parse(potStatus.Text);
                    playerChips.Text = thirdBot.Chips.ToString();
                    this.thirdBot.Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    fourthBot.Chips += int.Parse(potStatus.Text);
                    playerChips.Text = fourthBot.Chips.ToString();
                    this.fourthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    fifthBot.Chips += int.Parse(potStatus.Text);
                    playerChips.Text = fifthBot.Chips.ToString();
                    this.fifthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    cardPicture[j].Visible = false;
                }
                await Finish(1);
            }
            intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= End)
            {
                await Finish(2);
            }
            #endregion


        }

        async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }

            // TODO : extract in method Reset or something like that
            this.player.Panel.Visible = false;
            this.firstBot.Panel.Visible = false;
            this.secondBot.Panel.Visible = false;
            this.thirdBot.Panel.Visible = false;
            this.fourthBot.Panel.Visible = false;
            this.fifthBot.Panel.Visible = false;

            neededChipsToCall = this.bigBlind;
            raise = 0;
            botsWithoutChips = 5;
            type = 0;
            rounds = 0;

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
            winners = 0;

            Flop = 1;
            Turn = 2;
            River = 3;
            End = 4;
            playersLeft = 6;
            raisedTurn = 1;

            arePlayersEliminated.Clear();
            CheckWinners.Clear();
            ints.Clear();
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            potStatus.Text = "0";
            secondsLeft = 60;
            turnCount = 0;
            this.playerStatus.Text = string.Empty;
            this.botOneStatus.Text = string.Empty;
            botTwoStatus.Text = string.Empty;
            botThreeStatus.Text = string.Empty;
            botFourStatus.Text = string.Empty;
            botFiveStatus.Text = string.Empty;

            // TODO: Here add chips, duplicate.
            if (player.Chips <= 0)
            {
                AddChips addChips = new AddChips();
                addChips.ShowDialog();
                if (addChips.AddedChips != 0)
                {
                    player.Chips = addChips.AddedChips;
                    firstBot.Chips += addChips.AddedChips;
                    secondBot.Chips += addChips.AddedChips;
                    thirdBot.Chips += addChips.AddedChips;
                    fourthBot.Chips += addChips.AddedChips;
                    fifthBot.Chips += addChips.AddedChips;
                    this.player.OutOfChips = false;
                    this.player.AbleToMakeTurn = true;
                    raiseButton.Enabled = true;
                    foldButton.Enabled = true;
                    checkButton.Enabled = true;
                    raiseButton.Text = "Raise";
                }
            }

            ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);

            for (int os = 0; os < 17; os++)
            {
                cardPicture[os].Image = null;
                cardPicture[os].Invalidate();
                cardPicture[os].Visible = false;
            }

            await Shuffle();
            //await Turns();
        }

        void FixWinners()
        {
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";

            if (!this.playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", this.player);
            }

            if (!this.botOneStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", this.firstBot);
            }

            if (!botTwoStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", this.secondBot);
            }

            if (!botThreeStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", this.thirdBot);
            }

            if (!botFourStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", this.fourthBot);
            }

            if (!botFiveStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", this.fifthBot);
            }

            Winner(player.Type, this.player.Power, "Player", player.Chips, fixedLast);
            Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", firstBot.Chips, fixedLast);
            Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", secondBot.Chips, fixedLast);
            Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", thirdBot.Chips, fixedLast);
            Winner(this.fourthBot.Type, this.fourthBot.Power, "Bot 4", fourthBot.Chips, fixedLast);
            Winner(this.fifthBot.Type, this.fifthBot.Power, "Bot 5", fifthBot.Chips, fixedLast);
        }

        void AI(int c1, int c2, Label sStatus, int name, IPokerPlayer pokerPlayer)
        {
            if (!pokerPlayer.OutOfChips)
            {
                if (pokerPlayer.Type == -1)
                {
                    HighCard(pokerPlayer, sStatus);
                }

                if (pokerPlayer.Type == 0)
                {
                    PairTable(pokerPlayer, sStatus);
                }

                if (pokerPlayer.Type == 1)
                {
                    PairHand(pokerPlayer, sStatus);
                }

                if (pokerPlayer.Type == 2)
                {
                    TwoPair(pokerPlayer, sStatus);
                }

                if (pokerPlayer.Type == 3)
                {
                    ThreeOfAKind(pokerPlayer, sStatus, name);
                }

                if (pokerPlayer.Type == 4)
                {
                    Straight(pokerPlayer, sStatus, name);
                }

                if (pokerPlayer.Type == 5 || pokerPlayer.Type == 5.5)
                {
                    Flush(pokerPlayer, sStatus, name);
                }

                if (pokerPlayer.Type == 6)
                {
                    FullHouse(pokerPlayer, sStatus, name);
                }

                if (pokerPlayer.Type == 7)
                {
                    FourOfAKind(pokerPlayer, sStatus, name);
                }

                if (pokerPlayer.Type == 8 || pokerPlayer.Type == 9)
                {
                    StraightFlush(pokerPlayer, sStatus, name);
                }
            }

            if (pokerPlayer.OutOfChips)
            {
                cardPicture[c1].Visible = false;
                cardPicture[c2].Visible = false;
            }
        }

        private void HighCard(IPokerPlayer pokerPlayer, Label sStatus)
        {
            HP(pokerPlayer, sStatus, 20, 25);
        }

        private void PairTable(IPokerPlayer pokerPlayer, Label sStatus)
        {
            HP(pokerPlayer, sStatus, 16, 25);
        }

        private void PairHand(IPokerPlayer pokerPlayer, Label sStatus)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (pokerPlayer.Power <= 199 && pokerPlayer.Power >= 140)
            {
                PH(pokerPlayer, sStatus, rCall, 6, rRaise);
            }
            if (pokerPlayer.Power <= 139 && pokerPlayer.Power >= 128)
            {
                PH(pokerPlayer, sStatus, rCall, 7, rRaise);
            }
            if (pokerPlayer.Power < 128 && pokerPlayer.Power >= 101)
            {
                PH(pokerPlayer, sStatus, rCall, 9, rRaise);
            }
        }

        private void TwoPair(IPokerPlayer pokerPlayer, Label sStatus)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (pokerPlayer.Power <= 290 && pokerPlayer.Power >= 246)
            {
                PH(pokerPlayer, sStatus, rCall, 3, rRaise);
            }
            if (pokerPlayer.Power <= 244 && pokerPlayer.Power >= 234)
            {
                PH(pokerPlayer, sStatus, rCall, 4, rRaise);
            }
            if (pokerPlayer.Power < 234 && pokerPlayer.Power >= 201)
            {
                PH(pokerPlayer, sStatus, rCall, 4, rRaise);
            }
        }

        private void ThreeOfAKind(IPokerPlayer pokerPlayer, Label sStatus, int name)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (pokerPlayer.Power <= 390 && pokerPlayer.Power >= 330)
            {
                Smooth(pokerPlayer, sStatus, name, tCall, tRaise);
            }
            if (pokerPlayer.Power <= 327 && pokerPlayer.Power >= 321)//10  8
            {
                Smooth(pokerPlayer, sStatus, name, tCall, tRaise);
            }
            if (pokerPlayer.Power < 321 && pokerPlayer.Power >= 303)//7 2
            {
                Smooth(pokerPlayer, sStatus, name, tCall, tRaise);
            }
        }

        private void Straight(IPokerPlayer pokerPlayer, Label sStatus, int name)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (pokerPlayer.Power <= 480 && pokerPlayer.Power >= 410)
            {
                Smooth(pokerPlayer, sStatus, name, sCall, sRaise);
            }
            if (pokerPlayer.Power <= 409 && pokerPlayer.Power >= 407)//10  8
            {
                Smooth(pokerPlayer, sStatus, name, sCall, sRaise);
            }
            if (pokerPlayer.Power < 407 && pokerPlayer.Power >= 404)
            {
                Smooth(pokerPlayer, sStatus, name, sCall, sRaise);
            }
        }

        private void Flush(IPokerPlayer pokerPlayer, Label sStatus, int name)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(pokerPlayer, sStatus, name, fCall, fRaise);
        }

        private void FullHouse(IPokerPlayer pokerPlayer, Label sStatus, int name)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (pokerPlayer.Power <= 626 && pokerPlayer.Power >= 620)
            {
                Smooth(pokerPlayer, sStatus, name, fhCall, fhRaise);
            }
            if (pokerPlayer.Power < 620 && pokerPlayer.Power >= 602)
            {
                Smooth(pokerPlayer, sStatus, name, fhCall, fhRaise);
            }
        }

        private void FourOfAKind(IPokerPlayer pokerPlayer, Label sStatus, int name)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (pokerPlayer.Power <= 752 && pokerPlayer.Power >= 704)
            {
                Smooth(pokerPlayer, sStatus, name, fkCall, fkRaise);
            }
        }

        private void StraightFlush(IPokerPlayer pokerPlayer, Label sStatus, int name)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (pokerPlayer.Power <= 913 && pokerPlayer.Power >= 804)
            {
                Smooth(pokerPlayer, sStatus, name, sfCall, sfRaise);
            }
        }

        private void Fold(IPokerPlayer pokerPlayer, Label sStatus)
        {
            raising = false;
            sStatus.Text = "Fold";
            pokerPlayer.AbleToMakeTurn = false;
            pokerPlayer.OutOfChips = true;
        }

        private void Check(IPokerPlayer pokerPlayer, Label cStatus)
        {
            cStatus.Text = "Check";
            pokerPlayer.AbleToMakeTurn = false;
            raising = false;
        }

        private void Call(IPokerPlayer pokerPlayer, Label sStatus)
        {
            raising = false;
            pokerPlayer.AbleToMakeTurn = false;
            pokerPlayer.Chips -= neededChipsToCall;
            sStatus.Text = "Call " + neededChipsToCall;
            potStatus.Text = (int.Parse(potStatus.Text) + neededChipsToCall).ToString();
        }

        private void Raised(IPokerPlayer pokerPlayer, Label sStatus)
        {
            pokerPlayer.Chips -= Convert.ToInt32(raise);
            sStatus.Text = "Raise " + raise;
            potStatus.Text = (int.Parse(potStatus.Text) + Convert.ToInt32(raise)).ToString();
            neededChipsToCall = Convert.ToInt32(raise);
            raising = true;
            pokerPlayer.AbleToMakeTurn = false;
        }

        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        private void HP(IPokerPlayer pokerPlayer, Label sStatus, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (neededChipsToCall <= 0)
            {
                Check(pokerPlayer, sStatus);
            }
            if (neededChipsToCall > 0)
            {
                if (rnd == 1)
                {
                    if (neededChipsToCall <= RoundN(pokerPlayer.Chips, n))
                    {
                        Call(pokerPlayer, sStatus);
                    }
                    else
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                }
                if (rnd == 2)
                {
                    if (neededChipsToCall <= RoundN(pokerPlayer.Chips, n1))
                    {
                        Call(pokerPlayer, sStatus);
                    }
                    else
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                }
            }
            if (rnd == 3)
            {
                if (raise == 0)
                {
                    raise = neededChipsToCall * 2;
                    Raised(pokerPlayer, sStatus);
                }
                else
                {
                    if (raise <= RoundN(pokerPlayer.Chips, n))
                    {
                        raise = neededChipsToCall * 2;
                        Raised(pokerPlayer, sStatus);
                    }
                    else
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                }
            }
            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.OutOfChips = true;
            }
        }

        private void PH(IPokerPlayer pokerPlayer, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (neededChipsToCall <= 0)
                {
                    Check(pokerPlayer, sStatus);
                }
                if (neededChipsToCall > 0)
                {
                    if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n1))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (raise > RoundN(pokerPlayer.Chips, n))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (!pokerPlayer.OutOfChips)
                    {
                        if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n) && neededChipsToCall <= RoundN(pokerPlayer.Chips, n1))
                        {
                            Call(pokerPlayer, sStatus);
                        }
                        if (raise <= RoundN(pokerPlayer.Chips, n) && raise >= (RoundN(pokerPlayer.Chips, n)) / 2)
                        {
                            Call(pokerPlayer, sStatus);
                        }
                        if (raise <= (RoundN(pokerPlayer.Chips, n)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(pokerPlayer.Chips, n);
                                Raised(pokerPlayer, sStatus);
                            }
                            else
                            {
                                raise = neededChipsToCall * 2;
                                Raised(pokerPlayer, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (neededChipsToCall > 0)
                {
                    if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n1 - rnd))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (raise > RoundN(pokerPlayer.Chips, n - rnd))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (!pokerPlayer.OutOfChips)
                    {
                        if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n - rnd) && neededChipsToCall <= RoundN(pokerPlayer.Chips, n1 - rnd))
                        {
                            Call(pokerPlayer, sStatus);
                        }
                        if (raise <= RoundN(pokerPlayer.Chips, n - rnd) && raise >= (RoundN(pokerPlayer.Chips, n - rnd)) / 2)
                        {
                            Call(pokerPlayer, sStatus);
                        }
                        if (raise <= (RoundN(pokerPlayer.Chips, n - rnd)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(pokerPlayer.Chips, n - rnd);
                                Raised(pokerPlayer, sStatus);
                            }
                            else
                            {
                                raise = neededChipsToCall * 2;
                                Raised(pokerPlayer, sStatus);
                            }
                        }
                    }
                }
                if (neededChipsToCall <= 0)
                {
                    raise = (int)RoundN(pokerPlayer.Chips, r - rnd);
                    Raised(pokerPlayer, sStatus);
                }
            }
            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.OutOfChips = true;
            }
        }

        void Smooth(IPokerPlayer pokerPlayer, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (neededChipsToCall <= 0)
            {
                Check(pokerPlayer, botStatus);
            }
            else
            {
                if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n))
                {
                    if (pokerPlayer.Chips > neededChipsToCall)
                    {
                        Call(pokerPlayer, botStatus);
                    }
                    else if (pokerPlayer.Chips <= neededChipsToCall)
                    {
                        raising = false;
                        pokerPlayer.AbleToMakeTurn = false;
                        pokerPlayer.Chips = 0;
                        botStatus.Text = "Call " + pokerPlayer.Chips;
                        potStatus.Text = (int.Parse(potStatus.Text) + pokerPlayer.Chips).ToString();
                    }
                }
                else
                {
                    if (raise > 0)
                    {
                        if (pokerPlayer.Chips >= raise * 2)
                        {
                            raise *= 2;
                            Raised(pokerPlayer, botStatus);
                        }
                        else
                        {
                            Call(pokerPlayer, botStatus);
                        }
                    }
                    else
                    {
                        raise = neededChipsToCall * 2;
                        Raised(pokerPlayer, botStatus);
                    }
                }
            }
            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.OutOfChips = true;
            }
        }

        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                this.player.OutOfChips = true;
                await Turns();
            }
            if (secondsLeft > 0)
            {
                secondsLeft--;
                pbTimer.Value = (secondsLeft / 6) * 100;
            }
        }

        private void Update_Tick(object sender, object e)
        {
            if (player.Chips <= 0)
            {
                playerChips.Text = "Chips : 0";
            }
            if (firstBot.Chips <= 0)
            {
                botOneChips.Text = "Chips : 0";
            }
            if (secondBot.Chips <= 0)
            {
                botTwoChips.Text = "Chips : 0";
            }
            if (thirdBot.Chips <= 0)
            {
                botThreeChips.Text = "Chips : 0";
            }
            if (fourthBot.Chips <= 0)
            {
                botFourChips.Text = "Chips : 0";
            }
            if (fifthBot.Chips <= 0)
            {
                botFiveChips.Text = "Chips : 0";
            }

            playerChips.Text = "Chips : " + player.Chips.ToString();
            botOneChips.Text = "Chips : " + firstBot.Chips.ToString();
            botTwoChips.Text = "Chips : " + secondBot.Chips.ToString();
            botThreeChips.Text = "Chips : " + thirdBot.Chips.ToString();
            botFourChips.Text = "Chips : " + fourthBot.Chips.ToString();
            botFiveChips.Text = "Chips : " + fifthBot.Chips.ToString();

            if (player.Chips <= 0)
            {
                this.player.AbleToMakeTurn = false;
                this.player.OutOfChips = true;
                callButton.Enabled = false;
                raiseButton.Enabled = false;
                foldButton.Enabled = false;
                checkButton.Enabled = false;
            }

            if (player.Chips >= neededChipsToCall)
            {
                callButton.Text = "Call " + neededChipsToCall.ToString();
            }
            else
            {
                callButton.Text = "All in";
                raiseButton.Enabled = false;
            }

            if (neededChipsToCall > 0)
            {
                checkButton.Enabled = false;
            }
            if (neededChipsToCall <= 0)
            {
                checkButton.Enabled = true;
                callButton.Text = "Call";
                callButton.Enabled = false;
            }
            if (player.Chips <= 0)
            {
                raiseButton.Enabled = false;
            }

            int parsedValue;
            if (raiseAmountField.Text != string.Empty && int.TryParse(raiseAmountField.Text, out parsedValue))
            {
                if (player.Chips <= parsedValue)
                {
                    raiseButton.Text = "All in";
                }
                else
                {
                    raiseButton.Text = "Raise";
                }
            }
            if (player.Chips < neededChipsToCall)
            {
                raiseButton.Enabled = false;
            }
        }

        private async void FoldClick(object sender, EventArgs e)
        {
            this.playerStatus.Text = "Fold";
            this.player.AbleToMakeTurn = false;
            this.player.OutOfChips = true;
            await Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (neededChipsToCall <= 0)
            {
                this.player.AbleToMakeTurn = false;
                this.playerStatus.Text = "Check";
            }
            else
            {
                //playerStatus.Text = "All in " + Chips;

                checkButton.Enabled = false;
            }

            await Turns();
        }

        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", this.player);

            if (player.Chips >= neededChipsToCall)
            {
                player.Chips -= neededChipsToCall;
                playerChips.Text = "Chips : " + player.Chips.ToString();
                if (potStatus.Text != string.Empty)
                {
                    potStatus.Text = (int.Parse(potStatus.Text) + neededChipsToCall).ToString();
                }
                else
                {
                    potStatus.Text = neededChipsToCall.ToString();
                }
                this.player.AbleToMakeTurn = false;
                this.playerStatus.Text = "Call " + neededChipsToCall;
                this.player.Call = neededChipsToCall;
            }
            else if (player.Chips <= neededChipsToCall && neededChipsToCall > 0)
            {
                potStatus.Text = (int.Parse(potStatus.Text) + player.Chips).ToString();
                this.playerStatus.Text = "All in " + player.Chips;
                player.Chips = 0;
                playerChips.Text = "Chips : " + player.Chips.ToString();
                this.player.AbleToMakeTurn = false;
                foldButton.Enabled = false;
                this.player.Call = player.Chips;
            }

            await Turns();
        }

        private async void RaiseClick(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", this.player);

            int parsedValue;
            bool isValidNumber = int.TryParse(this.raiseAmountField.Text, out parsedValue);
            if (isValidNumber)
            {
                if (this.player.Chips > this.neededChipsToCall)
                {
                    if (raise * 2 > parsedValue)
                    {
                        raiseAmountField.Text = (raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }

                    if (player.Chips >= parsedValue)
                    {
                        neededChipsToCall = parsedValue;
                        raise = parsedValue;
                        this.playerStatus.Text = "Raise " + neededChipsToCall.ToString();
                        potStatus.Text = (int.Parse(potStatus.Text) + neededChipsToCall).ToString();
                        callButton.Text = "Call";
                        player.Chips -= parsedValue;
                        raising = true;
                        this.player.Raise = raise;
                    }
                    // all in scenario
                    else
                    {
                        neededChipsToCall = player.Chips;
                        raise = player.Chips;
                        potStatus.Text = (int.Parse(potStatus.Text) + player.Chips).ToString();
                        this.playerStatus.Text = "Raise " + neededChipsToCall.ToString();
                        player.Chips = 0;
                        raising = true;
                        this.player.Raise = raise;
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            this.player.AbleToMakeTurn = false;
            await Turns();
        }

        /// <summary>
        /// Add chips to all players on table.
        /// </summary>
        private void AddChipsClick(object sender, EventArgs e)
        {
            const int maxChipsToAdd = 100000000;
            int addedChips = 0;
            bool isValidNumber = false;
            isValidNumber = int.TryParse(this.addChipsToAllAmount.Text, out addedChips);

            if (isValidNumber && addedChips > 0 && addedChips < maxChipsToAdd)
            {
                player.Chips += addedChips;
                firstBot.Chips += addedChips;
                secondBot.Chips += addedChips;
                thirdBot.Chips += addedChips;
                fourthBot.Chips += addedChips;
                fifthBot.Chips += addedChips;
                playerChips.Text = "Chips : " + player.Chips.ToString();
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
            const int maxSmallBlind = 100000;
            const int minSmallBlind = 250;
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

            if (parsedValue > maxSmallBlind)
            {
                MessageBox.Show("The maximum of the Small Blind is " + maxSmallBlind);
                this.smallBlindField.Text = this.smallBlind.ToString();
            }

            if (parsedValue < minSmallBlind)
            {
                MessageBox.Show("The minimum of the Small Blind is " + minSmallBlind);
            }

            if (parsedValue >= minSmallBlind && parsedValue <= maxSmallBlind)
            {
                this.smallBlind = parsedValue;
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play.");
            }
        }

        private void bBigBlind_Click(object sender, EventArgs e)
        {
            const int maxBigBlind = 200000;
            const int minBigBlind = 500;
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

            if (parsedValue > maxBigBlind)
            {
                MessageBox.Show("The maximum of the Big Blind is " + maxBigBlind);
                this.bigBlindField.Text = this.bigBlind.ToString();
            }
            else if (parsedValue < minBigBlind)
            {
                MessageBox.Show("The minimum of the Big Blind is " + minBigBlind);
            }

            if (parsedValue >= minBigBlind && parsedValue <= minBigBlind)
            {
                this.bigBlind = parsedValue;
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play.");
            }
        }


        // TODO : Too many invokes.
        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            width = this.Width;
            height = this.Height;
        }
        #endregion
    }
}