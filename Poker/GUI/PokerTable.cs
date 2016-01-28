namespace Poker.GUI
{
    using Contracts;
    using Enums;
    using Models;
    using PokerUtilities;
    using PokerUtilities.CardsCombinationMethods;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Type = Poker.Type;

    public partial class PokerTable : Form
    {
        public PokerTable(IDealer dealer, ICheckHandType checkHand, IHandType handType, IWriter messageBoxWriter)
        {
            this.Player = new PokerPlayer(new Panel());
            this.FirstBot = new PokerPlayer(new Panel());
            this.SecondBot = new PokerPlayer(new Panel());
            this.ThirdBot = new PokerPlayer(new Panel());
            this.FourthBot = new PokerPlayer(new Panel());
            this.FifthBot = new PokerPlayer(new Panel());

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

            this.playerChips.Text = "Chips : " + this.Player.Chips;
            this.botOneChips.Text = "Chips : " + this.FirstBot.Chips;
            this.botTwoChips.Text = "Chips : " + this.SecondBot.Chips;
            this.botThreeChips.Text = "Chips : " + this.ThirdBot.Chips;
            this.botFourChips.Text = "Chips : " + this.FourthBot.Chips;
            this.botFiveChips.Text = "Chips : " + this.FifthBot.Chips;

            this.timer.Interval = 1000;
            this.timer.Tick += this.TimerTick;
            this.updates.Interval = 100;
            this.updates.Tick += this.Update_Tick;

            this.raiseAmountField.Text = (this.bigBlind * 2).ToString();

            this.Player.OutOfChips = false;
            this.Player.AbleToMakeTurn = true;
        }

        public IPokerPlayer Player
        {
            get;
        }

        public IPokerPlayer FirstBot
        {
            get;
        }

        public IPokerPlayer SecondBot
        {
            get;
        }

        public IPokerPlayer ThirdBot
        {
            get;
        }

        public IPokerPlayer FourthBot
        {
            get;
        }

        public IPokerPlayer FifthBot
        {
            get;
        }

        public IDealer Dealer
        {
            get; set;
        }

        public ICheckHandType CheckHand
        {
            get; set;
        }

        public IHandType HandType
        {
            get; set;
        }

        private async Task Shuffle()
        {
            this.eliminatedPlayers.Add(this.Player.OutOfChips);
            this.eliminatedPlayers.Add(this.FirstBot.OutOfChips);
            this.eliminatedPlayers.Add(this.SecondBot.OutOfChips);
            this.eliminatedPlayers.Add(this.ThirdBot.OutOfChips);
            this.eliminatedPlayers.Add(this.FourthBot.OutOfChips);
            this.eliminatedPlayers.Add(this.FifthBot.OutOfChips);

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

            this.ImgLocation = this.Dealer.ShuffleDeck(this.ImgLocation);

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
                this.cardPicture[currentCardIndex].Name = "pb" + currentCardIndex;

                await Task.Delay(150);

                #region Throwing Cards

                if (currentCardIndex < 2)
                {
                    this.cardPicture[currentCardIndex].Tag = this.reserve[currentCardIndex];
                    this.cardPicture[currentCardIndex].Image = this.deck[currentCardIndex];
                    this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Bottom;
                    this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                    horizontal += this.cardPicture[currentCardIndex].Width;

                    this.Controls.Add(this.Player.Panel);
                    var playerPanelLocation = new Point(this.cardPicture[0].Left - 10, this.cardPicture[0].Top - 10);
                    this.Player.InitializePanel(playerPanelLocation);
                }

                // TODO: extract in methods. The code from here to 520th line is repeated
                if (this.FirstBot.Chips > 0)
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

                        this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        this.cardPicture[currentCardIndex].Image = backImage;
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.FirstBot.Panel);
                        var bot1PanelLocation = new Point(this.cardPicture[2].Left - 10, this.cardPicture[2].Top - 10);
                        this.FirstBot.InitializePanel(bot1PanelLocation);
                    }
                }

                if (this.SecondBot.Chips > 0)
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

                        this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        this.cardPicture[currentCardIndex].Image = backImage;
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.SecondBot.Panel);
                        var bot2PanelLocation = new Point(this.cardPicture[4].Left - 10, this.cardPicture[4].Top - 10);
                        this.SecondBot.InitializePanel(bot2PanelLocation);
                    }
                }

                if (this.ThirdBot.Chips > 0)
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

                        this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Top;
                        this.cardPicture[currentCardIndex].Image = backImage;
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.ThirdBot.Panel);
                        var bot3PanelLocation = new Point(this.cardPicture[6].Left - 10, this.cardPicture[6].Top - 10);
                        this.ThirdBot.InitializePanel(bot3PanelLocation);
                    }
                }

                if (this.FourthBot.Chips > 0)
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

                        this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        this.cardPicture[currentCardIndex].Image = backImage;
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.FourthBot.Panel);
                        var bot4PanelLocation = new Point(this.cardPicture[8].Left - 10, this.cardPicture[8].Top - 10);
                        this.FourthBot.InitializePanel(bot4PanelLocation);
                    }
                }

                if (this.FifthBot.Chips > 0)
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

                        this.cardPicture[currentCardIndex].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        this.cardPicture[currentCardIndex].Image = backImage;
                        this.cardPicture[currentCardIndex].Location = new Point(horizontal, vertical);
                        horizontal += this.cardPicture[currentCardIndex].Width;
                        this.cardPicture[currentCardIndex].Visible = true;
                        this.Controls.Add(this.FifthBot.Panel);
                        var bot5PanelLocation = new Point(this.cardPicture[10].Left - 10, this.cardPicture[10].Top - 10);
                        this.FifthBot.InitializePanel(bot5PanelLocation);
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

                #endregion Throwing Cards

                if (this.FirstBot.Chips <= 0)
                {
                    this.FirstBot.OutOfChips = true;
                    this.cardPicture[2].Visible = false;
                    this.cardPicture[3].Visible = false;
                }
                else
                {
                    this.FirstBot.OutOfChips = false;
                    if (currentCardIndex == 3)
                    {
                        if (this.cardPicture[3] != null)
                        {
                            this.cardPicture[2].Visible = true;
                            this.cardPicture[3].Visible = true;
                        }
                    }
                }

                if (this.SecondBot.Chips <= 0)
                {
                    this.SecondBot.OutOfChips = true;
                    this.cardPicture[4].Visible = false;
                    this.cardPicture[5].Visible = false;
                }
                else
                {
                    this.SecondBot.OutOfChips = false;
                    if (currentCardIndex == 5)
                    {
                        if (this.cardPicture[5] != null)
                        {
                            this.cardPicture[4].Visible = true;
                            this.cardPicture[5].Visible = true;
                        }
                    }
                }

                if (this.ThirdBot.Chips <= 0)
                {
                    this.ThirdBot.OutOfChips = true;
                    this.cardPicture[6].Visible = false;
                    this.cardPicture[7].Visible = false;
                }
                else
                {
                    this.ThirdBot.OutOfChips = false;
                    if (currentCardIndex == 7)
                    {
                        if (this.cardPicture[7] != null)
                        {
                            this.cardPicture[6].Visible = true;
                            this.cardPicture[7].Visible = true;
                        }
                    }
                }

                if (this.FourthBot.Chips <= 0)
                {
                    this.FourthBot.OutOfChips = true;
                    this.cardPicture[8].Visible = false;
                    this.cardPicture[9].Visible = false;
                }
                else
                {
                    this.FourthBot.OutOfChips = false;
                    if (currentCardIndex == 9)
                    {
                        if (this.cardPicture[9] != null)
                        {
                            this.cardPicture[8].Visible = true;
                            this.cardPicture[9].Visible = true;
                        }
                    }
                }

                if (this.FifthBot.Chips <= 0)
                {
                    this.FifthBot.OutOfChips = true;
                    this.cardPicture[10].Visible = false;
                    this.cardPicture[11].Visible = false;
                }
                else
                {
                    this.FifthBot.OutOfChips = false;
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
                DialogResult dialogResult = this.messageBoxWriter.PrintYesNo("Would You Like To Play Again ?",
                    "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
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

            if (!this.Player.OutOfChips && this.Player.AbleToMakeTurn)
            {
                this.FixCall(this.playerStatus, this.Player);

                // await Task.Delay(2000);
                // this.messageBoxWriter.Print("Player's Turn");
                this.pbTimer.Visible = true;
                this.pbTimer.Value = 1000;
                this.secondsLeft = 60;

                this.timer.Start();
                this.raiseButton.Enabled = true;
                this.callButton.Enabled = true;
                this.foldButton.Enabled = true;
                this.turnCount++;
            }

            if (this.Player.OutOfChips || !this.Player.AbleToMakeTurn)
            {
                await this.AllIn();

                if (this.Player.OutOfChips && !this.Player.Folded)
                {
                    if (this.callButton.Text.Contains("All in") == false
                        || this.raiseButton.Text.Contains("All in") == false)
                    {
                        this.eliminatedPlayers[0] = null;
                        this.playersLeft--;
                        this.Player.Folded = true;
                    }
                }

                await this.CheckRaise(0, 0);

                this.pbTimer.Visible = false;
                this.raiseButton.Enabled = false;
                this.callButton.Enabled = false;
                this.foldButton.Enabled = false;
                this.timer.Stop();
                this.FirstBot.AbleToMakeTurn = true;

                if (!this.FirstBot.OutOfChips)
                {
                    if (this.FirstBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botOneStatus, this.FirstBot);

                        rules.TexasHoldEmRules(2, 3, "Bot 1", this.FirstBot, ref playerStatus, ref this.cardPicture,
                            ref this.Win, ref this.sorted, ref this.reserve);

                        await Task.Delay(1000);

                        // this.messageBoxWriter.Print("Bot 1's Turn");
                        this.AI(2, 3, this.botOneStatus, 0, this.FirstBot);

                        this.turnCount++;
                        this.FirstBot.AbleToMakeTurn = false;
                        this.SecondBot.AbleToMakeTurn = true;
                    }
                }

                if (this.FirstBot.OutOfChips && !this.FirstBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(1);
                    this.eliminatedPlayers.Insert(1, null);
                    this.playersLeft--;
                    this.FirstBot.Folded = true;
                }

                if (this.FirstBot.OutOfChips || !this.FirstBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(1, 1);
                    this.SecondBot.AbleToMakeTurn = true;
                }

                if (!this.SecondBot.OutOfChips)
                {
                    if (this.SecondBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botTwoStatus, this.SecondBot);
                        rules.TexasHoldEmRules(4, 5, "Bot 2", this.SecondBot, ref playerStatus, ref this.cardPicture,
                            ref this.Win, ref this.sorted, ref this.reserve);
                        await Task.Delay(1000);

                        // this.messageBoxWriter.Print("Bot 2's Turn");
                        this.AI(4, 5, this.botTwoStatus, 1, this.SecondBot);
                        this.turnCount++;
                        this.SecondBot.AbleToMakeTurn = false;
                        this.ThirdBot.AbleToMakeTurn = true;
                    }
                }

                if (this.SecondBot.OutOfChips && !this.SecondBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(2);
                    this.eliminatedPlayers.Insert(2, null);
                    this.playersLeft--;
                    this.SecondBot.Folded = true;
                }

                if (this.SecondBot.OutOfChips || !this.SecondBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(2, 2);
                    this.ThirdBot.AbleToMakeTurn = true;
                }

                if (!this.ThirdBot.OutOfChips)
                {
                    if (this.ThirdBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botThreeStatus, this.ThirdBot);

                        rules.TexasHoldEmRules(6, 7, "Bot 3", this.ThirdBot, ref playerStatus, ref this.cardPicture,
                            ref this.Win, ref this.sorted, ref this.reserve);
                        await Task.Delay(1000);

                        // this.messageBoxWriter.Print("Bot 3's Turn");
                        this.AI(6, 7, this.botThreeStatus, 2, this.ThirdBot);
                        this.turnCount++;
                        this.ThirdBot.AbleToMakeTurn = false;
                        this.FourthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.ThirdBot.OutOfChips && !this.ThirdBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(3);
                    this.eliminatedPlayers.Insert(3, null);
                    this.playersLeft--;
                    this.ThirdBot.Folded = true;
                }

                if (this.ThirdBot.OutOfChips || !this.ThirdBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(3, 3);
                    this.FourthBot.AbleToMakeTurn = true;
                }

                if (!this.FourthBot.OutOfChips)
                {
                    if (this.FourthBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botFourStatus, this.FourthBot);
                        rules.TexasHoldEmRules(8, 9, "Bot 4", this.FourthBot, ref playerStatus, ref this.cardPicture,
                            ref this.Win, ref this.sorted, ref this.reserve);
                        await Task.Delay(1000);

                        // this.messageBoxWriter.Print("Bot 4's Turn");
                        this.AI(8, 9, this.botFourStatus, 3, this.FourthBot);
                        this.turnCount++;
                        this.FourthBot.AbleToMakeTurn = false;
                        this.FifthBot.AbleToMakeTurn = true;
                    }
                }

                if (this.FourthBot.OutOfChips && !this.FourthBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(4);
                    this.eliminatedPlayers.Insert(4, null);
                    this.playersLeft--;
                    this.FourthBot.Folded = true;
                }

                if (this.FourthBot.OutOfChips || !this.FourthBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(4, 4);
                    this.FifthBot.AbleToMakeTurn = true;
                }

                if (!this.FifthBot.OutOfChips)
                {
                    if (this.FifthBot.AbleToMakeTurn)
                    {
                        this.FixCall(this.botFiveStatus, this.FifthBot);
                        rules.TexasHoldEmRules(10, 11, "Bot 5", this.FifthBot, ref playerStatus, ref this.cardPicture,
                            ref this.Win, ref this.sorted, ref this.reserve);
                        await Task.Delay(1000);

                        // this.messageBoxWriter.Print("Bot 5's Turn");
                        this.AI(10, 11, this.botFiveStatus, 4, this.FifthBot);
                        this.turnCount++;
                        this.FifthBot.AbleToMakeTurn = false;
                    }
                }

                if (this.FifthBot.OutOfChips && !this.FifthBot.Folded)
                {
                    this.eliminatedPlayers.RemoveAt(5);
                    this.eliminatedPlayers.Insert(5, null);
                    this.playersLeft--;
                    this.FifthBot.Folded = true;
                }

                if (this.FifthBot.OutOfChips || !this.FifthBot.AbleToMakeTurn)
                {
                    await this.CheckRaise(5, 5);
                    this.Player.AbleToMakeTurn = true;
                }

                if (this.Player.OutOfChips && !this.Player.Folded)
                {
                    if (this.callButton.Text.Contains("All in") == false ||
                        this.raiseButton.Text.Contains("All in") == false)
                    {
                        this.eliminatedPlayers.RemoveAt(0);
                        this.eliminatedPlayers.Insert(0, null);
                        this.playersLeft--;
                        this.Player.Folded = true;
                    }
                }

                #endregion Rotating

                await this.AllIn();
                if (!this.restart)
                {
                    await this.Turns();
                }

                this.restart = false;
            }
        }

        private void Winner(IPokerPlayer pokerPlayer, string player, int chips, string lastly)

        // redundant chips and lastly?
        {
            if (lastly == "")
            {
                lastly = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                if (this.cardPicture[j].Visible)
                {
                    this.cardPicture[j].Image = this.deck[j];
                }
            }

            if (pokerPlayer.Type == this.sorted.Current)
            {
                if (pokerPlayer.Power == this.sorted.Power)
                {
                    this.winners++;
                    this.CheckWinners.Add(pokerPlayer);

                    if (pokerPlayer.Type == -1)
                    {
                        this.messageBoxWriter.Print(player + " High Card ");
                    }

                    if (pokerPlayer.Type == 1 || pokerPlayer.Type == 0)
                    {
                        this.messageBoxWriter.Print(player + " Pair ");
                    }

                    if (pokerPlayer.Type == 2)
                    {
                        this.messageBoxWriter.Print(player + " Two Pair ");
                    }

                    if (pokerPlayer.Type == 3)
                    {
                        this.messageBoxWriter.Print(player + " Three of a Kind ");
                    }

                    if (pokerPlayer.Type == 4)
                    {
                        this.messageBoxWriter.Print(player + " Straight ");
                    }

                    if (pokerPlayer.Type == 5)
                    {
                        this.messageBoxWriter.Print(player + " Flush ");
                    }

                    if (pokerPlayer.Type == 6)
                    {
                        this.messageBoxWriter.Print(player + " Full House ");
                    }

                    if (pokerPlayer.Type == 7)
                    {
                        this.messageBoxWriter.Print(player + " Four of a Kind ");
                    }

                    if (pokerPlayer.Type == 8)
                    {
                        this.messageBoxWriter.Print(player + " Straight Flush ");
                    }

                    if (pokerPlayer.Type == 9)
                    {
                        this.messageBoxWriter.Print(player + " Royal Flush ! ");
                    }
                }
            }

            foreach (var winner in this.CheckWinners)
            {
                if (player == lastly)
                {
                    if (CheckWinners.Count >= 1)
                    {
                        winner.Chips += int.Parse(this.potStatus.Text) / this.CheckWinners.Count;

                        if (this.CheckWinners.Contains(this.Player))
                        {
                            this.playerChips.Text = this.Player.Chips.ToString();
                        }

                        if (this.CheckWinners.Contains(this.FirstBot))
                        {
                            this.botOneChips.Text = this.FirstBot.Chips.ToString();
                        }

                        if (this.CheckWinners.Contains(this.SecondBot))
                        {
                            this.botTwoChips.Text = this.SecondBot.Chips.ToString();
                        }

                        if (this.CheckWinners.Contains(this.ThirdBot))
                        {
                            this.botThreeChips.Text = this.ThirdBot.Chips.ToString();
                        }

                        if (this.CheckWinners.Contains(this.FourthBot))
                        {
                            this.botFourChips.Text = this.FourthBot.Chips.ToString();
                        }

                        if (this.CheckWinners.Contains(this.FifthBot))
                        {
                            this.botFiveChips.Text = this.FifthBot.Chips.ToString();
                        }
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
                    if (currentTurn == this.raisedTurn - 1 || !this.changed && this.turnCount == this.playersLeft ||
                        this.raisedTurn == 0 && currentTurn == 5)
                    {
                        this.changed = false;
                        this.turnCount = 0;
                        this.raise = 0;
                        this.neededChipsToCall = 0;
                        this.raisedTurn = 123;
                        this.rounds++;
                        if (!this.Player.OutOfChips)
                        {
                            this.playerStatus.Text = string.Empty;
                        }

                        if (!this.FirstBot.OutOfChips)
                        {
                            this.botOneStatus.Text = string.Empty;
                        }

                        if (!this.SecondBot.OutOfChips)
                        {
                            this.botTwoStatus.Text = string.Empty;
                        }

                        if (!this.ThirdBot.OutOfChips)
                        {
                            this.botThreeStatus.Text = string.Empty;
                        }

                        if (!this.FourthBot.OutOfChips)
                        {
                            this.botFourStatus.Text = string.Empty;
                        }

                        if (!this.FifthBot.OutOfChips)
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
                this.Player.AbleToMakeTurn = true;
                this.Player.OutOfChips = false;
                this.FirstBot.OutOfChips = false;
                this.SecondBot.OutOfChips = false;
                this.ThirdBot.OutOfChips = false;
                this.FourthBot.OutOfChips = false;
                this.FifthBot.OutOfChips = false;

                // TODO: Add chips on two place.
                if (this.Player.Chips <= 0)
                {
                    AddChips addChips = new AddChips(this.messageBoxWriter);
                    addChips.ShowDialog();
                    if (addChips.AddedChips != 0)
                    {
                        this.Player.Chips = addChips.AddedChips;
                        this.FirstBot.Chips += addChips.AddedChips;
                        this.SecondBot.Chips += addChips.AddedChips;
                        this.ThirdBot.Chips += addChips.AddedChips;
                        this.FourthBot.Chips += addChips.AddedChips;
                        this.FifthBot.Chips += addChips.AddedChips;
                        this.Player.OutOfChips = false;
                        this.Player.AbleToMakeTurn = true;
                        this.raiseButton.Enabled = true;
                        this.foldButton.Enabled = true;
                        this.checkButton.Enabled = true;
                        this.raiseButton.Text = "Raise";
                    }
                }

                this.MakePlayersNonVisible();

                this.ResetCall();

                this.ResetRaise();

                this.neededChipsToCall = this.bigBlind;

                this.raise = 0;
                this.ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png",
                    SearchOption.TopDirectoryOnly);

                this.rounds = 0;

                this.winners = 0;

                this.ResetPower();

                this.ResetType();

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
            this.Player.Type = -1;
            this.FirstBot.Type = -1;
            this.SecondBot.Type = -1;
            this.ThirdBot.Type = -1;
            this.FourthBot.Type = -1;
            this.FifthBot.Type = -1;
        }

        private void MakePlayersNonVisible()
        {
            this.Player.Panel.Visible = false;
            this.FirstBot.Panel.Visible = false;
            this.SecondBot.Panel.Visible = false;
            this.ThirdBot.Panel.Visible = false;
            this.FourthBot.Panel.Visible = false;
            this.FifthBot.Panel.Visible = false;
        }

        private void ResetPower()
        {
            this.Player.Power = 0;
            this.FirstBot.Power = 0;
            this.SecondBot.Power = 0;
            this.ThirdBot.Power = 0;
            this.FourthBot.Power = 0;
            this.FifthBot.Power = 0;
        }

        private void ResetRaise()
        {
            this.Player.Raise = 0;
            this.FirstBot.Raise = 0;
            this.SecondBot.Raise = 0;
            this.ThirdBot.Raise = 0;
            this.FourthBot.Raise = 0;
            this.FifthBot.Raise = 0;
        }

        private void ResetCall()
        {
            this.Player.Call = 0;
            this.FirstBot.Call = 0;
            this.SecondBot.Call = 0;
            this.ThirdBot.Call = 0;
            this.FourthBot.Call = 0;
            this.FifthBot.Call = 0;
        }

        private void FixCall(Label status, IPokerPlayer pokerPlayer)
        {
            if (this.rounds == 4)
            {
                return;
            }

            //set Call and Raise
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

            if (pokerPlayer.Raise < this.raise)
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

        private async Task AllIn()
        {
            #region All in

            if (this.Player.Chips <= 0 && !this.isAllIn)
            {
                if (this.playerStatus.Text.Contains("Raise"))
                {
                    this.allInChips.Add(this.Player.Chips);
                    this.isAllIn = true;
                }

                if (this.playerStatus.Text.Contains("Call"))
                {
                    this.allInChips.Add(this.Player.Chips);
                    this.isAllIn = true;
                }
            }

            this.isAllIn = false;
            if (this.FirstBot.Chips <= 0 && !this.FirstBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.FirstBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }

            if (this.SecondBot.Chips <= 0 && !this.SecondBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.SecondBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }

            if (this.ThirdBot.Chips <= 0 && !this.ThirdBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.ThirdBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }

            if (this.FourthBot.Chips <= 0 && !this.FourthBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.FourthBot.Chips);
                    this.isAllIn = true;
                }

                this.isAllIn = false;
            }

            if (this.FifthBot.Chips <= 0 && !this.FifthBot.OutOfChips)
            {
                if (!this.isAllIn)
                {
                    this.allInChips.Add(this.FifthBot.Chips);
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

            #endregion All in

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

            #endregion LastManStanding

            #region FiveOrLessLeft

            if (abc < 6 && abc > 1 && this.rounds >= (int)RoundStage.End)
            {
                await this.Finish(2);
            }

            #endregion FiveOrLessLeft
        }

        private void LastManLogic(string playerName)
        {
            this.Player.Chips += int.Parse(this.potStatus.Text);
            this.playerChips.Text = this.Player.Chips.ToString();
            this.Player.Panel.Visible = true;
            string msg = playerName + " Wins";
            this.messageBoxWriter.Print(msg);
        }

        private async Task Finish(int n)
        {
            if (n == 2)
            {
                this.FixWinners();
            }

            ResetStatsOfPlayers();

            // TODO: Here add chips, duplicate.
            if (this.Player.Chips <= 0)
            {
                AddChips addChips = new AddChips(this.messageBoxWriter);
                addChips.ShowDialog();
                if (addChips.AddedChips != 0)
                {
                    this.Player.Chips = addChips.AddedChips;
                    this.FirstBot.Chips += addChips.AddedChips;
                    this.SecondBot.Chips += addChips.AddedChips;
                    this.ThirdBot.Chips += addChips.AddedChips;
                    this.FourthBot.Chips += addChips.AddedChips;
                    this.FifthBot.Chips += addChips.AddedChips;

                    this.Player.OutOfChips = false;
                    this.Player.AbleToMakeTurn = true;
                    this.raiseButton.Enabled = true;
                    this.foldButton.Enabled = true;
                    this.checkButton.Enabled = true;
                    this.raiseButton.Text = "Raise";
                }
            }

            this.ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png",
                SearchOption.TopDirectoryOnly);

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

            this.Player.AbleToMakeTurn = true;
            this.FirstBot.AbleToMakeTurn = false;
            this.SecondBot.AbleToMakeTurn = false;
            this.ThirdBot.AbleToMakeTurn = false;
            this.FourthBot.AbleToMakeTurn = false;
            this.FifthBot.AbleToMakeTurn = false;

            this.Player.OutOfChips = false;
            this.FirstBot.OutOfChips = false;
            this.SecondBot.OutOfChips = false;
            this.ThirdBot.OutOfChips = false;
            this.FourthBot.OutOfChips = false;
            this.FifthBot.OutOfChips = false;

            this.Player.Folded = false;
            this.FirstBot.Folded = false;
            this.SecondBot.Folded = false;
            this.ThirdBot.Folded = false;
            this.FourthBot.Folded = false;
            this.FifthBot.Folded = false;

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
            string fixedLast = "";

            if (!this.playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";

                rules.TexasHoldEmRules(
                    0, 
                    1, 
                    "Player", 
                    this.Player, 
                    ref playerStatus, 
                    ref this.cardPicture, 
                    ref this.Win,
                    ref this.sorted, 
                    ref this.reserve);
            }

            if (!this.botOneStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";

                rules.TexasHoldEmRules(
                    2, 
                    3, 
                    "Bot 1", 
                    this.FirstBot, 
                    ref playerStatus, 
                    ref this.cardPicture,
                    ref this.Win, 
                    ref this.sorted, 
                    ref this.reserve);
            }

            if (!this.botTwoStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";

                rules.TexasHoldEmRules(
                    4, 
                    5, 
                    "Bot 2", 
                    this.SecondBot, 
                    ref playerStatus, 
                    ref this.cardPicture,
                    ref this.Win, 
                    ref this.sorted, 
                    ref this.reserve);
            }

            if (!this.botThreeStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";

                rules.TexasHoldEmRules(
                    6, 
                    7, 
                    "Bot 3", 
                    this.ThirdBot, 
                    ref playerStatus, 
                    ref this.cardPicture,
                    ref this.Win, 
                    ref this.sorted, 
                    ref this.reserve);
            }

            if (!this.botFourStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";

                rules.TexasHoldEmRules(
                    8, 
                    9, 
                    "Bot 4", 
                    this.FourthBot, 
                    ref playerStatus, 
                    ref this.cardPicture,
                    ref this.Win, 
                    ref this.sorted, 
                    ref this.reserve);
            }

            if (!this.botFiveStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";

                rules.TexasHoldEmRules(
                    10,
                    11, 
                    "Bot 5", 
                    this.FifthBot, 
                    ref playerStatus, 
                    ref this.cardPicture,
                    ref this.Win, 
                    ref this.sorted, 
                    ref this.reserve);
            }

            this.Winner(this.Player, "Player", this.Player.Chips, fixedLast);
            this.Winner(this.FirstBot, "Bot 1", this.FirstBot.Chips, fixedLast);
            this.Winner(this.SecondBot, "Bot 2", this.SecondBot.Chips, fixedLast);
            this.Winner(this.ThirdBot, "Bot 3", this.ThirdBot.Chips, fixedLast);
            this.Winner(this.FourthBot, "Bot 4", this.FourthBot.Chips, fixedLast);
            this.Winner(this.FifthBot, "Bot 5", this.FifthBot.Chips, fixedLast);
        }

        private void AI(int card1, int card2, Label sStatus, int name, IPokerPlayer pokerPlayer)
        {
            if (!pokerPlayer.OutOfChips)
            {
                switch (pokerPlayer.Type)
                {
                    case -1:
                        this.HandType.HighCard(
                            pokerPlayer, 
                            sStatus, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising);
                        break;

                    case 0:
                        this.HandType.PairTable(
                            pokerPlayer, 
                            sStatus, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising);
                        break;

                    case 1:
                        this.HandType.PairHand(
                            pokerPlayer, 
                            sStatus, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;

                    case 2:
                        this.HandType.TwoPair(
                            pokerPlayer, 
                            sStatus, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;

                    case 3:
                        this.HandType.ThreeOfAKind(
                            pokerPlayer, 
                            sStatus, 
                            name, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;

                    case 4:
                        this.HandType.Straight(
                            pokerPlayer, 
                            sStatus, 
                            name, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;

                    case 5:
                        this.HandType.Flush(
                            pokerPlayer, 
                            sStatus, 
                            name, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;

                    case 6:
                        this.HandType.FullHouse(
                            pokerPlayer, 
                            sStatus, 
                            name, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;

                    case 7:
                        this.HandType.FourOfAKind(
                            pokerPlayer, 
                            sStatus, 
                            name, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;

                    case 8 | 9:
                        this.HandType.StraightFlush(
                            pokerPlayer, 
                            sStatus, 
                            name, 
                            this.neededChipsToCall, 
                            this.potStatus,
                            ref this.raise, 
                            ref this.raising, 
                            ref this.rounds);
                        break;
                }
            }

            if (pokerPlayer.OutOfChips)
            {
                this.cardPicture[card1].Visible = false;
                this.cardPicture[card2].Visible = false;
            }
        }

        #region Variables

        private readonly RulesMethod rules = new RulesMethod();
        private readonly IWriter messageBoxWriter;

        private int neededChipsToCall;
        private int botsWithoutChips = 5;
        private int rounds;
        private int raise;
        private int playersLeft = 6;
        private int winners;
        private int raisedTurn = 1;

        private bool isAllIn;
        private bool changed;

        private readonly List<bool?> eliminatedPlayers = new List<bool?>();
        private List<Type> Win = new List<Type>();
        private readonly List<IPokerPlayer> CheckWinners = new List<IPokerPlayer>();
        private readonly List<int> allInChips = new List<int>();
        private bool restart;
        private bool raising;

        private Type sorted;

        private string[] ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png",
            SearchOption.TopDirectoryOnly);

        private int[] reserve = new int[17];
        private readonly Image[] deck = new Image[52];
        private PictureBox[] cardPicture = new PictureBox[52];
        private readonly Timer timer = new Timer();
        private readonly Timer updates = new Timer();

        private int secondsLeft = 60;
        private int bigBlind = 500;
        private int smallBlind = 250;
        private int turnCount;

        #endregion Variables

        #region UI

        private async void TimerTick(object sender, object e)
        {
            if (this.pbTimer.Value <= 0)
            {
                this.Player.OutOfChips = true;
                await this.Turns();
            }

            if (this.secondsLeft > 0)
            {
                this.secondsLeft--;
                this.pbTimer.Value = this.secondsLeft / 6 * 100;
            }
        }

        private void Update_Tick(object sender, object e)
        {
            if (this.Player.Chips <= 0)
            {
                this.playerChips.Text = "Chips : 0";
            }

            if (this.FirstBot.Chips <= 0)
            {
                this.botOneChips.Text = "Chips : 0";
            }

            if (this.SecondBot.Chips <= 0)
            {
                this.botTwoChips.Text = "Chips : 0";
            }

            if (this.ThirdBot.Chips <= 0)
            {
                this.botThreeChips.Text = "Chips : 0";
            }

            if (this.FourthBot.Chips <= 0)
            {
                this.botFourChips.Text = "Chips : 0";
            }

            if (this.FifthBot.Chips <= 0)
            {
                this.botFiveChips.Text = "Chips : 0";
            }

            this.playerChips.Text = "Chips : " + this.Player.Chips;
            this.botOneChips.Text = "Chips : " + this.FirstBot.Chips;
            this.botTwoChips.Text = "Chips : " + this.SecondBot.Chips;
            this.botThreeChips.Text = "Chips : " + this.ThirdBot.Chips;
            this.botFourChips.Text = "Chips : " + this.FourthBot.Chips;
            this.botFiveChips.Text = "Chips : " + this.FifthBot.Chips;

            if (this.Player.Chips <= 0)
            {
                this.Player.AbleToMakeTurn = false;
                this.Player.OutOfChips = true;
                this.callButton.Enabled = false;
                this.raiseButton.Enabled = false;
                this.foldButton.Enabled = false;
                this.checkButton.Enabled = false;
            }

            if (this.Player.Chips >= this.neededChipsToCall)
            {
                this.callButton.Text = "Call " + this.neededChipsToCall;
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

            if (this.Player.Chips <= 0)
            {
                this.raiseButton.Enabled = false;
            }

            int parsedValue;
            if (this.raiseAmountField.Text != string.Empty && int.TryParse(this.raiseAmountField.Text, out parsedValue))
            {
                if (this.Player.Chips <= parsedValue)
                {
                    this.raiseButton.Text = "All in";
                }
                else
                {
                    this.raiseButton.Text = "Raise";
                }
            }

            if (this.Player.Chips < this.neededChipsToCall)
            {
                this.raiseButton.Enabled = false;
            }
        }

        private async void FoldClick(object sender, EventArgs e)
        {
            this.playerStatus.Text = "Fold";
            this.Player.AbleToMakeTurn = false;
            this.Player.OutOfChips = true;
            await this.Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (this.neededChipsToCall <= 0)
            {
                this.Player.AbleToMakeTurn = false;
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
            rules.TexasHoldEmRules(
                0, 
                1, 
                "Player", 
                this.Player, 
                ref playerStatus, 
                ref this.cardPicture, 
                ref this.Win,
                ref this.sorted, 
                ref this.reserve);

            if (this.Player.Chips >= this.neededChipsToCall)
            {
                this.Player.Chips -= this.neededChipsToCall;
                this.playerChips.Text = "Chips : " + this.Player.Chips;

                if (this.potStatus.Text != string.Empty)
                {
                    this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
                }
                else
                {
                    this.potStatus.Text = this.neededChipsToCall.ToString();
                }

                this.Player.AbleToMakeTurn = false;
                this.playerStatus.Text = "Call " + this.neededChipsToCall;
                this.Player.Call = this.neededChipsToCall;
            }
            else if (this.Player.Chips <= this.neededChipsToCall && this.neededChipsToCall > 0)
            {
                this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.Player.Chips).ToString();
                this.playerStatus.Text = "All in " + this.Player.Chips;
                this.Player.Chips = 0;
                this.playerChips.Text = "Chips : " + this.Player.Chips;
                this.Player.AbleToMakeTurn = false;
                this.foldButton.Enabled = false;
                this.Player.Call = this.Player.Chips;
            }

            await this.Turns();
        }

        private async void RaiseClick(object sender, EventArgs e)
        {
            rules.TexasHoldEmRules(0, 1, "Player", this.Player, ref playerStatus, ref this.cardPicture, ref this.Win,
                ref this.sorted, ref this.reserve);

            int parsedValue;
            bool isValidNumber = int.TryParse(this.raiseAmountField.Text, out parsedValue);
            if (isValidNumber)
            {
                if (this.Player.Chips > this.neededChipsToCall)
                {
                    if (this.raise * 2 > parsedValue)
                    {
                        this.raiseAmountField.Text = (this.raise * 2).ToString();
                        this.messageBoxWriter.Print("You must raise atleast twice as the current raise !");
                        return;
                    }

                    if (this.Player.Chips >= parsedValue)
                    {
                        this.neededChipsToCall = parsedValue;
                        this.raise = parsedValue;
                        this.playerStatus.Text = "Raise " + this.neededChipsToCall;
                        this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
                        this.callButton.Text = "Call";
                        this.Player.Chips -= parsedValue;
                        this.raising = true;
                        this.Player.Raise = this.raise;
                    }

                    // all in scenario
                    else
                    {
                        this.neededChipsToCall = this.Player.Chips;
                        this.raise = this.Player.Chips;
                        this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.Player.Chips).ToString();
                        this.playerStatus.Text = "Raise " + this.neededChipsToCall;
                        this.Player.Chips = 0;
                        this.raising = true;
                        this.Player.Raise = this.raise;
                    }
                }
            }
            else
            {
                this.messageBoxWriter.Print("This is a number only field");
                return;
            }

            this.Player.AbleToMakeTurn = false;

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
                this.Player.Chips += addedChips;
                this.FirstBot.Chips += addedChips;
                this.SecondBot.Chips += addedChips;
                this.ThirdBot.Chips += addedChips;
                this.FourthBot.Chips += addedChips;
                this.FifthBot.Chips += addedChips;
                this.playerChips.Text = "Chips : " + this.Player.Chips;
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
                this.messageBoxWriter.Print(
                    "The changes have been saved ! They will become available the next hand you play.");
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
                this.messageBoxWriter.Print(
                    "The changes have been saved ! They will become available the next hand you play.");
            }
        }

        // TODO : Too many invokes.
        //private void Layout_Change(object sender, LayoutEventArgs e)
        //{
        //    this.width = this.Width;
        //    this.height = this.Height;
        //}

        #endregion UI
    }
}