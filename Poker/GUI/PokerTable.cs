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
    using Enums;
    using Models;
    using PokerUtilities;
    using PokerUtilities.CardsCombinationMethods;
    using Type = Poker.Type;
    using static PokerUtilities.CardsCombinationMethods.RulesMethod;
    using static PokerUtilities.ResetMethods;

    public partial class PokerTable : Form
    {
        #region Variables
        private IDealer dealer;
        private ICheckHandType checkHand;
        private IHandType handType;
        private readonly RulesMethod rules = new RulesMethod();
        private readonly IWriter messageBoxWriter;

        private readonly IPokerPlayer player;
        private readonly IPokerPlayer firstBot;
        private readonly IPokerPlayer secondBot;
        private readonly IPokerPlayer thirdBot;
        private readonly IPokerPlayer fourthBot;
        private readonly IPokerPlayer fifthBot;


        private int neededChipsToCall;
        private int botsWithoutChips = 5;
        private int rounds = 0;
        private int raise = 0;
        private int playersLeft = 6;
        private int winners = 0;
        private int raisedTurn = 1;

        private bool isAllIn;
        private bool changed;

        List<bool?> eliminatedPlayers = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> allInChips = new List<int>();
        private bool restart = false;
        private bool raising = false;

        Type sorted;
        string[] ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);

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

        public PokerTable(IDealer dealer, ICheckHandType checkHand, IHandType handType, IWriter messageBoxWriter)
        {
            this.player = new PokerPlayer(new Panel());
            this.firstBot = new PokerPlayer(new Panel());
            this.secondBot = new PokerPlayer(new Panel());
            this.thirdBot = new PokerPlayer(new Panel());
            this.fourthBot = new PokerPlayer(new Panel());
            this.fifthBot = new PokerPlayer(new Panel());

            this.Dealer = dealer;
            this.CheckHand = checkHand;
            this.HandType = handType;
            this.messageBoxWriter = messageBoxWriter;

            this.neededChipsToCall = this.bigBlind;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.updates.Start();
            this.InitializeComponent();

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

        public IPokerPlayer Player
        {
            get
            {
                return this.player;
            } 
        }

        public IPokerPlayer FirstBot
        {
            get
            {
                return this.firstBot;
            } 
        }

        public IPokerPlayer SecondBot
        {
            get
            {
                return this.secondBot;
            }
        }

        public IPokerPlayer ThirdBot
        {
            get
            {
                return this.thirdBot;
            }
        }

        public IPokerPlayer FourthBot
        {
            get
            {
                return this.fourthBot;
            }
        }

        public IPokerPlayer FifthBot
        {
            get
            {
                return this.fifthBot;
            }
        }

        public IDealer Dealer
        {
            get
            {
                return this.dealer;
            }
            set
            {
                this.dealer = value;
            }
        }

        public ICheckHandType CheckHand
        {
            get
            {
                return this.checkHand;
            }
            set
            {
                this.checkHand = value;
            }
        }

        public IHandType HandType
        {
            get
            {
                return this.handType;
            }
            set
            {
                this.handType = value;
            }
        }

        private async Task Shuffle()
        {
            this.eliminatedPlayers.Add(this.player.OutOfChips);
            this.eliminatedPlayers.Add(this.firstBot.OutOfChips);
            this.eliminatedPlayers.Add(this.secondBot.OutOfChips);
            this.eliminatedPlayers.Add(this.thirdBot.OutOfChips);
            this.eliminatedPlayers.Add(this.fourthBot.OutOfChips);
            this.eliminatedPlayers.Add(this.fifthBot.OutOfChips);

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

                await Task.Delay(150);

                #region Throwing Cards
                if (currentCardIndex < 2)
                {
                    this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];
                    this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                    this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Bottom;
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
                DialogResult dialogResult = this.messageBoxWriter.PrintYesNo("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
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
            this.foldButton.Enabled = true;
        }

        private async Task Turns()
        {
            #region Rotating
            if (!this.player.OutOfChips && this.player.AbleToMakeTurn)
            {
                this.FixCall(this.playerStatus, this.player, 1);
                this.messageBoxWriter.Print("Player's Turn");
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
                        this.eliminatedPlayers[0] = null;
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

                        rules.TexasHoldEmRules(2, 3, "Bot 1", this.firstBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
                       
                        this.messageBoxWriter.Print("Bot 1's Turn");

                        this.AI(2, 3, this.botOneStatus, 0, this.firstBot);

                        this.turnCount++;
                        this.firstBot.AbleToMakeTurn = false;
                        this.secondBot.AbleToMakeTurn = true;
                    }
                }

                if (this.firstBot.OutOfChips && !this.firstBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(1);
                    this.eliminatedPlayers.Insert(1, null);
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
                        rules.TexasHoldEmRules(4, 5, "Bot 2", this.secondBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
                        this.messageBoxWriter.Print("Bot 2's Turn");
                        this.AI(4, 5, this.botTwoStatus, 1, this.secondBot);
                        this.turnCount++;
                        this.secondBot.AbleToMakeTurn = false;
                        this.thirdBot.AbleToMakeTurn = true;
                    }
                }

                if (this.secondBot.OutOfChips && !this.secondBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(2);
                    this.eliminatedPlayers.Insert(2, null);
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

                        rules.TexasHoldEmRules(6, 7, "Bot 3", this.thirdBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
                        this.messageBoxWriter.Print("Bot 3's Turn");
                        this.AI(6, 7, this.botThreeStatus, 2, this.thirdBot);
                        this.turnCount++;
                        this.thirdBot.AbleToMakeTurn = false;
                        this.fourthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.thirdBot.OutOfChips && !this.thirdBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(3);
                    this.eliminatedPlayers.Insert(3, null);
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
                        rules.TexasHoldEmRules(8, 9, "Bot 4", this.fourthBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
                        this.messageBoxWriter.Print("Bot 4's Turn");
                        this.AI(8, 9, this.botFourStatus, 3, this.fourthBot);
                        this.turnCount++;
                        this.fourthBot.AbleToMakeTurn = false;
                        this.fifthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.fourthBot.OutOfChips && !this.fourthBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(4);
                    this.eliminatedPlayers.Insert(4, null);
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
                        rules.TexasHoldEmRules(10, 11, "Bot 5", this.fifthBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
                        this.messageBoxWriter.Print("Bot 5's Turn");
                        this.AI(10, 11, this.botFiveStatus, 4, this.fifthBot);
                        this.turnCount++;
                        this.fifthBot.AbleToMakeTurn = false;
                    }
                }

                if (this.fifthBot.OutOfChips && !this.fifthBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(5);
                    this.eliminatedPlayers.Insert(5, null);
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
                        this.eliminatedPlayers.RemoveAt(0);
                        this.eliminatedPlayers.Insert(0, null);
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

        void Winner(int current, double Power, string player, int chips)//redundant chips and lastly?
        {
            for (int j = 0; j <= 16; j++)
            {
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
                    this.CheckWinners.Add(player);
                    if (current == -1)
                    {
                        this.messageBoxWriter.Print(player + " High Card ");
                    }

                    if (current == 1 || current == 0)
                    {
                        this.messageBoxWriter.Print(player + " Pair ");
                    }

                    if (current == 2)
                    {
                        this.messageBoxWriter.Print(player + " Two Pair ");
                    }

                    if (current == 3)
                    {
                        this.messageBoxWriter.Print(player + " Three of a Kind ");
                    }

                    if (current == 4)
                    {
                        this.messageBoxWriter.Print(player + " Straight ");
                    }

                    if (current == 5)
                    {
                        this.messageBoxWriter.Print(player + " Flush ");
                    }

                    if (current == 6)
                    {
                        this.messageBoxWriter.Print(player + " Full House ");
                    }

                    if (current == 7)
                    {
                        this.messageBoxWriter.Print(player + " Four of a Kind ");
                    }

                    if (current == 8)
                    {
                        this.messageBoxWriter.Print(player + " Straight Flush ");
                    }

                    if (current == 9)
                    {
                        this.messageBoxWriter.Print(player + " Royal Flush ! ");
                    }
                }
            }

           
            if (this.winners > 1)
            {
                if (this.CheckWinners.Contains("Player"))
                {
                    this.player.Chips += int.Parse(this.potStatus.Text) / this.winners;
                    this.playerChips.Text = this.player.Chips.ToString();
                }

                if (this.CheckWinners.Contains("Bot 1"))
                {
                    this.firstBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                    this.botOneChips.Text = this.firstBot.Chips.ToString();
                }

                if (this.CheckWinners.Contains("Bot 2"))
                {
                    this.secondBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                    this.botTwoChips.Text = this.secondBot.Chips.ToString();
                }

                if (this.CheckWinners.Contains("Bot 3"))
                {
                    this.thirdBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                    this.botThreeChips.Text = this.thirdBot.Chips.ToString();
                }

                if (this.CheckWinners.Contains("Bot 4"))
                {
                    this.fourthBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                    this.botFourChips.Text = this.fourthBot.Chips.ToString();
                }

                if (this.CheckWinners.Contains("Bot 5"))
                {
                    this.fifthBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                    this.botFiveChips.Text = this.fifthBot.Chips.ToString();
                }

                    //await Finish(1);


                if (this.winners == 1)
                {
                    if (this.CheckWinners.Contains("Player"))
                    {
                        this.player.Chips += int.Parse(this.potStatus.Text);
                    }

                    if (this.CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBot.Chips += int.Parse(this.potStatus.Text);
                    }

                    if (this.CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBot.Chips += int.Parse(this.potStatus.Text);
                    }

                    if (this.CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBot.Chips += int.Parse(this.potStatus.Text);
                    }

                    if (this.CheckWinners.Contains("Bot 4"))
                    {
                        this.fourthBot.Chips += int.Parse(this.potStatus.Text);
                    }

                    if (this.CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBot.Chips += int.Parse(this.potStatus.Text);
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

                        ResetCall();
                        
                        ResetRaise();
                    }
                }
            }

            if (this.rounds == (int)RoundStage.Turn)
            {               
                this.cardPicture[15].Image = this.deck[15];

                ResetCall();

                ResetRaise();
            }

            if (this.rounds == (int)RoundStage.River)
            {             
                this.cardPicture[16].Image = this.deck[16];

                ResetCall();                
                
                ResetRaise();
            }

            if (this.rounds == (int)RoundStage.End && this.playersLeft == 6)
            {
                FixWinners();
                
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
                    AddChips addChips = new AddChips(this.messageBoxWriter);
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

                MakePlayersNonVisible();

                ResetCall();

                ResetRaise();

                this.neededChipsToCall = this.bigBlind;

                this.raise = 0;
                this.ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);
                
                this.rounds = 0;

                this.winners = 0;

                ResetPower();

                ResetType();

                this.allInChips.Clear();
                this.CheckWinners.Clear();
                this.eliminatedPlayers.Clear();
                this.Win.Clear();

                this.sorted.Current = 0;
                this.sorted.Power = 0;

                for (int index = 0; index < 17; index++)
                {
                    this.cardPicture[index].Image = null;
                    this.cardPicture[index].Invalidate();
                    this.cardPicture[index].Visible = false;
                }
                this.potStatus.Text = "0";
                this.playerStatus.Text = string.Empty;

                await this.Shuffle();
                await this.Turns();
            }
        }

        private void ResetType()
        {
            this.player.Type = -1;
            this.firstBot.Type = -1;
            this.secondBot.Type = -1;
            this.thirdBot.Type = -1;
            this.fourthBot.Type = -1;
            this.fifthBot.Type = -1;
        }

        private void MakePlayersNonVisible()
        {
            this.player.Panel.Visible = false;
            this.firstBot.Panel.Visible = false;
            this.secondBot.Panel.Visible = false;
            this.thirdBot.Panel.Visible = false;
            this.fourthBot.Panel.Visible = false;
            this.fifthBot.Panel.Visible = false;
        }

        private void ResetPower()
        {
            this.player.Power = 0;
            this.firstBot.Power = 0;
            this.secondBot.Power = 0;
            this.thirdBot.Power = 0;
            this.fourthBot.Power = 0;
            this.fifthBot.Power = 0;
        }

        private void ResetRaise()
        {
            this.player.Raise = 0;
            this.firstBot.Raise = 0;
            this.secondBot.Raise = 0;
            this.thirdBot.Raise = 0;
            this.fourthBot.Raise = 0;
            this.fifthBot.Raise = 0;
        }

        private void ResetCall()
        {
            this.player.Call = 0;
            this.firstBot.Call = 0;
            this.secondBot.Call = 0;
            this.thirdBot.Call = 0;
            this.fourthBot.Call = 0;
            this.fifthBot.Call = 0;
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
            if (this.player.Chips <= 0 && !this.isAllIn)
            {
                if (this.playerStatus.Text.Contains("Raise"))
                {
                    this.allInChips.Add(this.player.Chips);
                    this.isAllIn = true;
                }

                if (this.playerStatus.Text.Contains("Call"))
                {
                    this.allInChips.Add(this.player.Chips);
                    this.isAllIn = true;
                }
            }

            this.isAllIn = false;
            if (this.firstBot.Chips <= 0 && !this.firstBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.firstBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }
            if (this.secondBot.Chips <= 0 && !this.secondBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.secondBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }
            if (this.thirdBot.Chips <= 0 && !this.thirdBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.thirdBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }
            if (this.fourthBot.Chips <= 0 && !this.fourthBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.fourthBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }
            if (this.fifthBot.Chips <= 0 && !this.fifthBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.fifthBot.Chips);
                    this.isAllIn = true;
                }
            }
            if (this.allInChips.ToArray().Length == this.playersLeft)
            {
                await this.Finish(2);
            }
            else
            {
                this.allInChips.Clear();
            }
            #endregion

            var abc = this.eliminatedPlayers.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = this.eliminatedPlayers.IndexOf(false);
                if (index == 0)
                {
                    LastManLogic("Player");
                }
                else if (index == 1)
                {
                    LastManLogic("Bot 1");
                }
                else if (index == 2)
                {
                    LastManLogic("Bot 2");
                }
                else if (index == 3)
                {
                    LastManLogic("Bot 3");
                }
                else if (index == 4)
                {
                    LastManLogic("Bot 4");
                }
                else
                {
                    LastManLogic("Bot 5");
                }

                for (int j = 0; j <= 16; j++)
                {
                    this.cardPicture[j].Visible = false;
                }
                await this.Finish(1);
            }

            this.isAllIn = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && this.rounds >= (int)RoundStage.End)
            {
                await this.Finish(2);
            }
            #endregion
        }

        private void LastManLogic(string playerName)
        {
            this.player.Chips += int.Parse(this.potStatus.Text);
            this.playerChips.Text = this.player.Chips.ToString();
            this.player.Panel.Visible = true;
            string msg = playerName + " Wins";
            this.messageBoxWriter.Print(msg);
        }

        async Task Finish(int n)
        {
            if (n == 2)
            {
                this.FixWinners();
            }

            ResetStatsOfPlayers();

            // TODO: Here add chips, duplicate.
            if (this.player.Chips <= 0)
            {
                AddChips addChips = new AddChips(this.messageBoxWriter);
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

            for (int index = 0; index < 17; index++)
            {
                this.cardPicture[index].Image = null;
                this.cardPicture[index].Invalidate();
                this.cardPicture[index].Visible = false;
            }

            await this.Shuffle();
            //await Turns();
        }

        private void ResetStatsOfPlayers()
        {
            MakePlayersNonVisible();

            this.neededChipsToCall = this.bigBlind;
            this.raise = 0;
            this.botsWithoutChips = 5;
            this.rounds = 0;

            ResetPower();

            ResetType();

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

            ResetCall();

            ResetRaise();

            this.winners = 0;
            this.playersLeft = 6;
            this.raisedTurn = 1;

            this.eliminatedPlayers.Clear();
            this.CheckWinners.Clear();
            this.allInChips.Clear();
            this.Win.Clear();

            this.sorted.Current = 0;
            this.sorted.Power = 0;

            this.potStatus.Text = "0";
            this.secondsLeft = 60;
            this.turnCount = 0;

            ResetPlayerStatusText();
        }

        private void ResetPlayerStatusText()
        {
            this.playerStatus.Text = string.Empty;
            this.botOneStatus.Text = string.Empty;
            this.botTwoStatus.Text = string.Empty;
            this.botThreeStatus.Text = string.Empty;
            this.botFourStatus.Text = string.Empty;
            this.botFiveStatus.Text = string.Empty;
        }

        private void FixWinners()
        {
            this.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;

            if (!this.playerStatus.Text.Contains("Fold"))
            {
                rules.TexasHoldEmRules(0, 1, "Player", this.player, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
            }

            if (!this.botOneStatus.Text.Contains("Fold"))
            {
                rules.TexasHoldEmRules(2, 3, "Bot 1", this.firstBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
            }

            if (!this.botTwoStatus.Text.Contains("Fold"))
            {
                rules.TexasHoldEmRules(4, 5, "Bot 2", this.secondBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
            }

            if (!this.botThreeStatus.Text.Contains("Fold"))
            {
                rules.TexasHoldEmRules(6, 7, "Bot 3", this.thirdBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
            }

            if (!this.botFourStatus.Text.Contains("Fold"))
            {
                rules.TexasHoldEmRules(8, 9, "Bot 4", this.fourthBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
            }

            if (!this.botFiveStatus.Text.Contains("Fold"))
            {
                rules.TexasHoldEmRules(10, 11, "Bot 5", this.fifthBot, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);
            }

            this.Winner(this.player.Type, this.player.Power, "Player", this.player.Chips);
            this.Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", this.firstBot.Chips);
            this.Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", this.secondBot.Chips);
            this.Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", this.thirdBot.Chips);
            this.Winner(this.fourthBot.Type, this.fourthBot.Power, "Bot 4", this.fourthBot.Chips);
            this.Winner(this.fifthBot.Type, this.fifthBot.Power, "Bot 5", this.fifthBot.Chips);
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

                if (pokerPlayer.Type == 5)
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
            rules.TexasHoldEmRules(0, 1, "Player", this.player, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);

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
            rules.TexasHoldEmRules(0, 1, "Player", this.player, ref playerStatus, ref this.cardPicture, ref this.Win, ref this.sorted, ref this.reserve);

            int parsedValue;
            bool isValidNumber = int.TryParse(this.raiseAmountField.Text, out parsedValue);
            if (isValidNumber)
            {
                if (this.player.Chips > this.neededChipsToCall)
                {
                    if (this.raise * 2 > parsedValue)
                    {
                        this.raiseAmountField.Text = (this.raise * 2).ToString();
                        this.messageBoxWriter.Print("You must raise atleast twice as the current raise !");
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
                this.messageBoxWriter.Print("This is a number only field");
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
                this.messageBoxWriter.Print("Chips should be positive round number!");
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
                this.messageBoxWriter.Print("The Small Blind can be only round number !");
                this.smallBlindField.Text = this.smallBlind.ToString();
                return;
            }

            if (!isParsed)
            {
                this.messageBoxWriter.Print("This is a number only field");
                this.smallBlindField.Text = this.smallBlind.ToString();
                return;
            }

            if (parsedValue > GlobalConstants.MaxSmallBlind)
            {
                this.messageBoxWriter.Print("The maximum of the Small Blind is " + GlobalConstants.MaxSmallBlind);
                this.smallBlindField.Text = this.smallBlind.ToString();
            }

            if (parsedValue < GlobalConstants.MinSmallBlind)
            {
                this.messageBoxWriter.Print("The minimum of the Small Blind is " + GlobalConstants.MinSmallBlind);
            }

            if (parsedValue >= GlobalConstants.MinSmallBlind && parsedValue <= GlobalConstants.MaxSmallBlind)
            {
                this.smallBlind = parsedValue;
                this.messageBoxWriter.Print("The changes have been saved ! They will become available the next hand you play.");
            }
        }

        private void bBigBlind_Click(object sender, EventArgs e)
        {

            int parsedValue;
            bool isParsed = int.TryParse(this.bigBlindField.Text, out parsedValue);

            if (this.bigBlindField.Text.Contains(",") || this.bigBlindField.Text.Contains("."))
            {
                this.messageBoxWriter.Print("The Big Blind can be only round number!");
                this.bigBlindField.Text = this.bigBlind.ToString();
                return;
            }

            if (!isParsed)
            {
                this.messageBoxWriter.Print("This is a number only field");
                this.bigBlindField.Text = this.bigBlind.ToString();
                return;
            }

            if (parsedValue > GlobalConstants.MaxBigBlind)
            {
                this.messageBoxWriter.Print("The maximum of the Big Blind is " + GlobalConstants.MaxBigBlind);
                this.bigBlindField.Text = this.bigBlind.ToString();
            }
            else if (parsedValue < GlobalConstants.MinBigBlind)
            {
                this.messageBoxWriter.Print("The minimum of the Big Blind is " + GlobalConstants.MinBigBlind);
            }

            if (parsedValue >= GlobalConstants.MinBigBlind && parsedValue <= GlobalConstants.MaxBigBlind)
            {
                this.bigBlind = parsedValue;
                this.messageBoxWriter.Print("The changes have been saved ! They will become available the next hand you play.");
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