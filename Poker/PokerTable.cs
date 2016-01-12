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
        private readonly IPokerPlayer forthBot;
        private readonly IPokerPlayer fifthBoth;

        private int call = 500;
        private int foldedPlayers = 5;
        private double type;
        private double rounds = 0;
        private int raise = 0;

        private bool intsadded;
        private bool changed;

        private int height;
        private int width;

        // TODO: Convert to enum
        private int winners = 0;
        private int Flop = 1;
        private int Turn = 2;
        private int River = 3;
        private int End = 4;
        private int maxLeft = 6;

        //int last = 123;
        private int raisedTurn = 1;

        List<bool?> bools = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        private bool playerFoldTurn = false;
        private bool playerTurn = true;
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
        PictureBox[] holder = new PictureBox[52];
        Timer timer = new Timer();
        Timer updates = new Timer();
        private int t = 60;

        // private int i;
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
            this.forthBot = new PokerPlayer(new Panel());
            this.fifthBoth = new PokerPlayer(new Panel());

            //bools.Add(PlayerFoldTurn); bools.Add(bot1.FoldedTurn); bools.Add(bot2.FoldedTurn); bools.Add(bot3.FoldedTurn); bools.Add(bot4.FoldedTurn); bools.Add(bot5.FoldedTurn);
            call = this.bigBlind;
            MaximizeBox = false;
            MinimizeBox = false;
            updates.Start();
            InitializeComponent();

            // width = this.Width;
            // height = this.Height;
            Shuffle();
            tbPot.Enabled = false;
            tbChips.Enabled = false;
            tbBotChips1.Enabled = false;
            tbBotChips2.Enabled = false;
            tbBotChips3.Enabled = false;
            tbBotChips4.Enabled = false;
            tbBotChips5.Enabled = false;
            tbChips.Text = "Chips : " + this.player.Chips;
            tbBotChips1.Text = "Chips : " + this.firstBot.Chips;
            tbBotChips2.Text = "Chips : " + this.secondBot.Chips;
            tbBotChips3.Text = "Chips : " + this.thirdBot.Chips;
            tbBotChips4.Text = "Chips : " + this.forthBot.Chips;
            tbBotChips5.Text = "Chips : " + this.fifthBoth.Chips;
            timer.Interval = (1 * 1 * 1000);
            timer.Tick += TimerTick;
            updates.Interval = (1 * 1 * 100);
            updates.Tick += Update_Tick;
            this.tbBigBlind.Visible = true;
            this.tbSmallBlind.Visible = true;
            this.bBigBlind.Visible = true;
            this.bSmallBlind.Visible = true;
            //this.tbBigBlind.Visible = true;
            //this.tbSmallBlind.Visible = true;
            //this.bBigBlind.Visible = true;
            //this.bSmallBlind.Visible = true;
            //this.tbBigBlind.Visible = false;
            //this.tbSmallBlind.Visible = false;
            //this.bBigBlind.Visible = false;
            //this.bSmallBlind.Visible = false;
            tbRaise.Text = (this.bigBlind * 2).ToString();
        }

        async Task Shuffle()
        {
            bools.Add(this.player.FoldedTurn);
            bools.Add(this.firstBot.FoldedTurn);
            bools.Add(this.secondBot.FoldedTurn);
            bools.Add(this.thirdBot.FoldedTurn);
            bools.Add(this.forthBot.FoldedTurn);
            bools.Add(this.fifthBoth.FoldedTurn);

            bCall.Enabled = false;
            bRaise.Enabled = false;
            bFold.Enabled = false;
            bCheck.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            bool check = false;

            Bitmap backImage = new Bitmap(@"..\..\Resources\Assets\Back\Back.png");
            int horizontal = 580;
            int vertical = 480;
            Random rnd = new Random();
            for (int currentIndex = ImgLocation.Length; currentIndex > 0; currentIndex--)
            {
                int swapCardIndex = rnd.Next(currentIndex);
                var tempCard = ImgLocation[swapCardIndex];
                ImgLocation[swapCardIndex] = ImgLocation[currentIndex - 1];
                ImgLocation[currentIndex - 1] = tempCard;
            }

            // TODO: move it to proper place.
            const int neededCardsFromDack = 17; // 6 players * 2 card + 5 card on table
            for (int index = 0; index < neededCardsFromDack; index++)
            {
                deck[index] = Image.FromFile(ImgLocation[index]);

                // take card name, too slow i think
                //var charsToRemove = new string[] { @"..\..\Resources\Assets\Cards\", ".png" };
                //foreach (var c in charsToRemove)
                //{
                //    ImgLocation[i] = ImgLocation[i].Replace(c, string.Empty);
                //}
                int lastSlashIndex = this.ImgLocation[index].LastIndexOf("\\");
                int lastDotIndex = this.ImgLocation[index].LastIndexOf(".");
                this.ImgLocation[index] = this.ImgLocation[index].Substring(lastSlashIndex + 1, lastDotIndex - lastSlashIndex - 1);

                reserve[index] = int.Parse(ImgLocation[index]) - 1;
                holder[index] = new PictureBox();
                holder[index].SizeMode = PictureBoxSizeMode.StretchImage;
                holder[index].Height = 130;
                holder[index].Width = 80;
                this.Controls.Add(holder[index]);
                holder[index].Name = "pb" + index.ToString();

                // investigate why these delay is needed
                //await Task.Delay(200);
                #region Throwing Cards
                if (index < 2)
                {
                    holder[index].Tag = reserve[index];
                    holder[index].Image = deck[index];
                    holder[index].Anchor = AnchorStyles.Bottom;
                    //Holder[i].Dock = DockStyle.Top;
                    holder[index].Location = new Point(horizontal, vertical);
                    horizontal += holder[index].Width;

                    this.Controls.Add(this.player.Panel);
                    var playerPanelLocation = new Point(holder[0].Left - 10, holder[0].Top - 10);
                    this.player.InizializePanel(playerPanelLocation);
                }

                if (this.firstBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 2 && index < 4)
                    {
                        holder[index].Tag = reserve[index];

                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        if (index == 3)
                        {
                            check = false;
                        }

                        holder[index].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        holder[index].Location = new Point(horizontal, vertical);
                        horizontal += holder[index].Width;
                        holder[index].Visible = true;
                        this.Controls.Add(this.firstBot.Panel);
                        var bot1PanelLocation = new Point(holder[2].Left - 10, holder[2].Top - 10);
                        this.firstBot.InizializePanel(bot1PanelLocation);
                    }
                }

                if (secondBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 4 && index < 6)
                    {
                        holder[index].Tag = reserve[index];

                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }
                        check = true;
                        if (index == 5)
                        {
                            check = false;
                        }

                        holder[index].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        holder[index].Location = new Point(horizontal, vertical);
                        horizontal += holder[index].Width;
                        holder[index].Visible = true;
                        this.Controls.Add(this.secondBot.Panel);
                        var bot2PanelLocation = new Point(holder[4].Left - 10, holder[4].Top - 10);
                        this.secondBot.InizializePanel(bot2PanelLocation);
                    }
                }

                if (this.thirdBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 6 && index < 8)
                    {
                        holder[index].Tag = reserve[index];

                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        if (index == 7)
                        {
                            check = false;
                        }

                        holder[index].Anchor = (AnchorStyles.Top);
                        holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        holder[index].Location = new Point(horizontal, vertical);
                        horizontal += holder[index].Width;
                        holder[index].Visible = true;
                        this.Controls.Add(this.thirdBot.Panel);
                        var bot3PanelLocation = new Point(holder[6].Left - 10, holder[6].Top - 10);
                        this.thirdBot.InizializePanel(bot3PanelLocation);
                    }
                }

                if (forthBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 8 && index < 10)
                    {
                        holder[index].Tag = reserve[index];

                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }
                        check = true;
                        if (index == 9)
                        {
                            check = false;
                        }

                        holder[index].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        holder[index].Location = new Point(horizontal, vertical);
                        horizontal += holder[index].Width;
                        holder[index].Visible = true;
                        this.Controls.Add(this.forthBot.Panel);
                        var bot4PanelLocation = new Point(holder[8].Left - 10, holder[8].Top - 10);
                        this.forthBot.InizializePanel(bot4PanelLocation);
                    }
                }

                if (fifthBoth.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 10 && index < 12)
                    {
                        holder[index].Tag = reserve[index];

                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;
                        if (index == 11)
                        {
                            check = false;
                        }

                        holder[index].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        holder[index].Location = new Point(horizontal, vertical);
                        horizontal += holder[index].Width;
                        holder[index].Visible = true;
                        this.Controls.Add(this.fifthBoth.Panel);
                        var bot5PanelLocation = new Point(holder[10].Left - 10, holder[10].Top - 10);
                        this.fifthBoth.InizializePanel(bot5PanelLocation);
                    }
                }

                if (index >= 12)
                {
                    holder[index].Tag = reserve[index];

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;

                    holder[index].Anchor = AnchorStyles.None;
                    holder[index].Image = backImage;
                    //Holder[i].Image = Deck[i];
                    holder[index].Location = new Point(horizontal, vertical);
                    horizontal += 110;
                }
                #endregion
                if (firstBot.Chips <= 0)
                {
                    this.firstBot.FoldedTurn = true;
                    holder[2].Visible = false;
                    holder[3].Visible = false;
                }
                else
                {
                    this.firstBot.FoldedTurn = false;
                    if (index == 3)
                    {
                        if (holder[3] != null)
                        {
                            holder[2].Visible = true;
                            holder[3].Visible = true;
                        }
                    }
                }

                if (secondBot.Chips <= 0)
                {
                    this.secondBot.FoldedTurn = true;
                    holder[4].Visible = false;
                    holder[5].Visible = false;
                }
                else
                {
                    this.secondBot.FoldedTurn = false;
                    if (index == 5)
                    {
                        if (holder[5] != null)
                        {
                            holder[4].Visible = true;
                            holder[5].Visible = true;
                        }
                    }
                }

                if (thirdBot.Chips <= 0)
                {
                    this.thirdBot.FoldedTurn = true;
                    holder[6].Visible = false;
                    holder[7].Visible = false;
                }
                else
                {
                    this.thirdBot.FoldedTurn = false;
                    if (index == 7)
                    {
                        if (holder[7] != null)
                        {
                            holder[6].Visible = true;
                            holder[7].Visible = true;
                        }
                    }
                }

                if (forthBot.Chips <= 0)
                {
                    this.forthBot.FoldedTurn = true;
                    holder[8].Visible = false;
                    holder[9].Visible = false;
                }
                else
                {
                    this.forthBot.FoldedTurn = false;
                    if (index == 9)
                    {
                        if (holder[9] != null)
                        {
                            holder[8].Visible = true;
                            holder[9].Visible = true;
                        }
                    }
                }

                if (fifthBoth.Chips <= 0)
                {
                    this.fifthBoth.FoldedTurn = true;
                    holder[10].Visible = false;
                    holder[11].Visible = false;
                }
                else
                {
                    this.fifthBoth.FoldedTurn = false;
                    if (index == 11)
                    {
                        if (holder[11] != null)
                        {
                            holder[10].Visible = true;
                            holder[11].Visible = true;
                        }
                    }
                }

                if (index == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }

                    timer.Start();
                }
            }

            if (foldedPlayers == 5)
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
                foldedPlayers = 5;
            }

            bRaise.Enabled = true;
            bCall.Enabled = true;
            bRaise.Enabled = true;
            bRaise.Enabled = true;
            bFold.Enabled = true;
        }

        private async Task Turns()
        {
            #region Rotating
            if (!this.playerFoldTurn && this.playerTurn)
            {
                FixCall(this.playerStatus, this.player, 1);

                //MessageBox.Show("Player's Turn");
                pbTimer.Visible = true;
                pbTimer.Value = 1000;
                t = 60;

                timer.Start();
                bRaise.Enabled = true;
                bCall.Enabled = true;
                bFold.Enabled = true;
                turnCount++;
                FixCall(this.playerStatus, this.player, 2);
            }

            if (this.playerFoldTurn || !this.playerTurn)
            {
                await AllIn();

                if (this.playerFoldTurn && !this.player.Folded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        //bools.RemoveAt(0);
                        //bools.Insert(0, null);
                        this.bools[0] = null;
                        maxLeft--;
                        this.player.Folded = true;
                    }
                }

                await CheckRaise(0, 0);

                pbTimer.Visible = false;
                bRaise.Enabled = false;
                bCall.Enabled = false;
                bRaise.Enabled = false;
                bRaise.Enabled = false;
                bFold.Enabled = false;
                timer.Stop();
                this.firstBot.Turn = true;

                if (!this.firstBot.FoldedTurn)
                {
                    if (this.firstBot.Turn)
                    {
                        FixCall(this.bot1Status, this.firstBot, 1);
                        FixCall(this.bot1Status, this.firstBot, 2);

                        Rules(2, 3, "Bot 1", this.firstBot);
                        MessageBox.Show("Bot 1's Turn");

                        AI(2, 3, this.bot1Status, 0, this.firstBot);

                        turnCount++;
                        this.firstBot.Turn = false;
                        this.secondBot.Turn = true;
                    }
                }

                if (this.firstBot.FoldedTurn && !this.firstBot.Folded)
                {
                    bools.RemoveAt(1);
                    bools.Insert(1, null);
                    maxLeft--;
                    this.firstBot.Folded = true;
                }

                if (this.firstBot.FoldedTurn || !this.firstBot.Turn)
                {
                    await CheckRaise(1, 1);
                    this.secondBot.Turn = true;
                }

                if (!this.secondBot.FoldedTurn)
                {
                    if (this.secondBot.Turn)
                    {
                        FixCall(b2Status, this.secondBot, 1);
                        FixCall(b2Status, this.secondBot, 2);
                        Rules(4, 5, "Bot 2", this.secondBot);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, b2Status, 1, this.secondBot);
                        turnCount++;
                        this.secondBot.Turn = false;
                        this.thirdBot.Turn = true;
                    }
                }

                if (this.secondBot.FoldedTurn && !this.secondBot.Folded)
                {
                    bools.RemoveAt(2);
                    bools.Insert(2, null);
                    maxLeft--;
                    this.secondBot.Folded = true;
                }

                if (this.secondBot.FoldedTurn || !this.secondBot.Turn)
                {
                    await CheckRaise(2, 2);
                    this.thirdBot.Turn = true;
                }

                if (!this.thirdBot.FoldedTurn)
                {
                    if (this.thirdBot.Turn)
                    {
                        FixCall(b3Status, this.thirdBot, 1);
                        FixCall(b3Status, this.thirdBot, 2);

                        Rules(6, 7, "Bot 3", this.thirdBot);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, b3Status, 2, this.thirdBot);
                        turnCount++;
                        this.thirdBot.Turn = false;
                        this.forthBot.Turn = true;
                    }
                }

                if (this.thirdBot.FoldedTurn && !this.thirdBot.Folded)
                {
                    bools.RemoveAt(3);
                    bools.Insert(3, null);
                    maxLeft--;
                    this.thirdBot.Folded = true;
                }

                if (this.thirdBot.FoldedTurn || !this.thirdBot.Turn)
                {
                    await CheckRaise(3, 3);
                    this.forthBot.Turn = true;
                }

                if (!this.forthBot.FoldedTurn)
                {
                    if (this.forthBot.Turn)
                    {
                        FixCall(b4Status, this.forthBot, 1);
                        FixCall(b4Status, this.forthBot, 2);
                        Rules(8, 9, "Bot 4", this.forthBot);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, b4Status, 3, this.forthBot);
                        turnCount++;
                        this.forthBot.Turn = false;
                        this.fifthBoth.Turn = true;
                    }
                }

                if (this.forthBot.FoldedTurn && !this.forthBot.Folded)
                {
                    bools.RemoveAt(4);
                    bools.Insert(4, null);
                    maxLeft--;
                    this.forthBot.Folded = true;
                }

                if (this.forthBot.FoldedTurn || !this.forthBot.Turn)
                {
                    await CheckRaise(4, 4);
                    this.fifthBoth.Turn = true;
                }

                if (!this.fifthBoth.FoldedTurn)
                {
                    if (this.fifthBoth.Turn)
                    {
                        FixCall(b5Status, this.fifthBoth, 1);
                        FixCall(b5Status, this.fifthBoth, 2);
                        Rules(10, 11, "Bot 5", this.fifthBoth);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, b5Status, 4, this.fifthBoth);
                        turnCount++;
                        this.fifthBoth.Turn = false;
                    }
                }

                if (this.fifthBoth.FoldedTurn && !this.fifthBoth.Folded)
                {
                    bools.RemoveAt(5);
                    bools.Insert(5, null);
                    maxLeft--;
                    this.fifthBoth.Folded = true;
                }

                if (this.fifthBoth.FoldedTurn || !this.fifthBoth.Turn)
                {
                    await CheckRaise(5, 5);
                    this.playerTurn = true;
                }

                if (this.playerFoldTurn && !this.player.Folded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
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

        void Rules(int c1, int c2, string currentText, IPokerPlayer pokerPlayer)
        {
            if (c1 == 0 && c2 == 1)
            {
            }

            if (!pokerPlayer.FoldedTurn || c1 == 0 && c2 == 1 && this.playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = reserve[c1];
                Straight[1] = reserve[c2];
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
                    if (reserve[index] == int.Parse(holder[c1].Tag.ToString()) && reserve[index + 1] == int.Parse(holder[c2].Tag.ToString()))
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
                        Win.Add(new Type() {Power = pokerPlayer.Power, Current = 8});
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
                            Win.Add(new Type() {Power = pokerPlayer.Power, Current = 5});
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
                                if (reserve[index]/4 == reserve[tc]/4 && reserve[index + 1]/4 == reserve[tc - k]/4 ||
                                    reserve[index + 1]/4 == reserve[tc]/4 && reserve[index]/4 == reserve[tc - k]/4)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[index] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = 13*4 + (reserve[index + 1]/4)*2 + pokerPlayer.Type*100;
                                            Win.Add(new Type() {Power = pokerPlayer.Power, Current = 2});
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
                if (holder[j].Visible)
                {
                    holder[j].Image = deck[j];
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
                        player.Chips += int.Parse(tbPot.Text) / winners;
                        tbChips.Text = player.Chips.ToString();
                        //player.Panel.Visible = true;

                    }

                    if (CheckWinners.Contains("Bot 1"))
                    {
                        firstBot.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips1.Text = firstBot.Chips.ToString();
                        //bot1.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 2"))
                    {
                        secondBot.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips2.Text = secondBot.Chips.ToString();
                        //bot2.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 3"))
                    {
                        thirdBot.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips3.Text = thirdBot.Chips.ToString();
                        //bot3.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 4"))
                    {
                        forthBot.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips4.Text = forthBot.Chips.ToString();
                        //bot4.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 5"))
                    {
                        fifthBoth.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips5.Text = fifthBoth.Chips.ToString();
                        //bot5.Panel.Visible = true;
                    }

                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //player.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 1"))
                    {
                        firstBot.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot1.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 2"))
                    {
                        secondBot.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot2.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 3"))
                    {
                        thirdBot.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot3.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 4"))
                    {
                        forthBot.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot4.Panel.Visible = true;
                    }

                    if (CheckWinners.Contains("Bot 5"))
                    {
                        fifthBoth.Chips += int.Parse(tbPot.Text);
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
                if (turnCount >= maxLeft - 1 || !changed && turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        raise = 0;
                        call = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!this.playerFoldTurn)
                        {
                            this.playerStatus.Text = string.Empty;
                        }

                        if (!this.firstBot.FoldedTurn)
                        {
                            this.bot1Status.Text = string.Empty;
                        }

                        if (!this.secondBot.FoldedTurn)
                        {
                            b2Status.Text = string.Empty;
                        }

                        if (!this.thirdBot.FoldedTurn)
                        {
                            b3Status.Text = string.Empty;
                        }

                        if (!this.forthBot.FoldedTurn)
                        {
                            b4Status.Text = string.Empty;
                        }

                        if (!this.fifthBoth.FoldedTurn)
                        {
                            b5Status.Text = string.Empty;
                        }
                    }
                }
            }

            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (holder[j].Image != deck[j])
                    {
                        holder[j].Image = deck[j];
                        this.player.Call = 0;
                        this.player.Raise = 0;
                        this.firstBot.Call = 0;
                        this.firstBot.Raise = 0;
                        this.secondBot.Call = 0;
                        this.secondBot.Raise = 0;
                        this.thirdBot.Call = 0;
                        this.thirdBot.Raise = 0;
                        this.forthBot.Call = 0;
                        this.forthBot.Raise = 0;
                        this.fifthBoth.Call = 0;
                        this.fifthBoth.Raise = 0;
                    }
                }
            }

            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (holder[j].Image != deck[j])
                    {
                        holder[j].Image = deck[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.firstBot.Call = 0; this.firstBot.Raise = 0;
                        this.secondBot.Call = 0; this.secondBot.Raise = 0;
                        this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                        this.forthBot.Call = 0; this.forthBot.Raise = 0;
                        this.fifthBoth.Call = 0; this.fifthBoth.Raise = 0;
                    }
                }
            }

            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (holder[j].Image != deck[j])
                    {
                        holder[j].Image = deck[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.firstBot.Call = 0; this.firstBot.Raise = 0;
                        this.secondBot.Call = 0; this.secondBot.Raise = 0;
                        this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                        this.forthBot.Call = 0; this.forthBot.Raise = 0;
                        this.fifthBoth.Call = 0; this.fifthBoth.Raise = 0;
                    }
                }
            }

            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = string.Empty;
                if (!this.playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", this.player);
                }
                if (!this.bot1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", this.firstBot);
                }
                if (!b2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", this.secondBot);
                }
                if (!b3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", this.thirdBot);
                }
                if (!b4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", this.forthBot);
                }
                if (!b5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", this.fifthBoth);
                }
                Winner(player.Type, this.player.Power, "Player", player.Chips, fixedLast);
                Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", firstBot.Chips, fixedLast);
                Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", secondBot.Chips, fixedLast);
                Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", thirdBot.Chips, fixedLast);
                Winner(this.forthBot.Type, this.forthBot.Power, "Bot 4", forthBot.Chips, fixedLast);
                Winner(this.fifthBoth.Type, this.fifthBoth.Power, "Bot 5", fifthBoth.Chips, fixedLast);
                restart = true;
                this.playerTurn = true;
                this.playerFoldTurn = false;
                this.firstBot.FoldedTurn = false;
                this.secondBot.FoldedTurn = false;
                this.thirdBot.FoldedTurn = false;
                this.forthBot.FoldedTurn = false;
                this.fifthBoth.FoldedTurn = false;

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
                        forthBot.Chips += addChips.AddedChips;
                        fifthBoth.Chips += addChips.AddedChips;
                        this.playerFoldTurn = false;
                        this.playerTurn = true;
                        bRaise.Enabled = true;
                        bFold.Enabled = true;
                        bCheck.Enabled = true;
                        bRaise.Text = "Raise";
                    }
                }

                this.player.Panel.Visible = false; this.firstBot.Panel.Visible = false; this.secondBot.Panel.Visible = false; this.thirdBot.Panel.Visible = false; this.forthBot.Panel.Visible = false; this.fifthBoth.Panel.Visible = false;
                this.player.Call = 0; this.player.Raise = 0;
                this.firstBot.Call = 0; this.firstBot.Raise = 0;
                this.secondBot.Call = 0; this.secondBot.Raise = 0;
                this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                this.forthBot.Call = 0; this.forthBot.Raise = 0;
                this.fifthBoth.Call = 0; this.fifthBoth.Raise = 0;
                call = this.bigBlind;
                raise = 0;
                ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bools.Clear();
                rounds = 0;
                this.player.Power = 0; player.Type = -1;
                type = 0; this.firstBot.Power = 0; this.secondBot.Power = 0; this.thirdBot.Power = 0; this.forthBot.Power = 0; this.fifthBoth.Power = 0;
                this.firstBot.Type = -1; this.secondBot.Type = -1; this.thirdBot.Type = -1; this.forthBot.Type = -1; this.fifthBoth.Type = -1;
                ints.Clear();
                CheckWinners.Clear();
                winners = 0;
                Win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    holder[os].Image = null;
                    holder[os].Invalidate();
                    holder[os].Visible = false;
                }
                tbPot.Text = "0";
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
                        call = raise - pokerPlayer.Raise;
                    }

                    if (pokerPlayer.Call != call || pokerPlayer.Call <= call)
                    {
                        call = call - pokerPlayer.Call;
                    }

                    // TODO: check when this is valid and change text in call label
                    if (pokerPlayer.Raise == raise && raise > 0)
                    {
                        call = 0;
                        bCall.Enabled = false;
                        bCall.Text = "Callisfuckedup";
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
            if (firstBot.Chips <= 0 && !this.firstBot.FoldedTurn)
            {
                if (!intsadded)
                {
                    ints.Add(firstBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (secondBot.Chips <= 0 && !this.secondBot.FoldedTurn)
            {
                if (!intsadded)
                {
                    ints.Add(secondBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (thirdBot.Chips <= 0 && !this.thirdBot.FoldedTurn)
            {
                if (!intsadded)
                {
                    ints.Add(thirdBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (forthBot.Chips <= 0 && !this.forthBot.FoldedTurn)
            {
                if (!intsadded)
                {
                    ints.Add(forthBot.Chips);
                    intsadded = true;
                }

                intsadded = false;
            }
            if (fifthBoth.Chips <= 0 && !this.fifthBoth.FoldedTurn)
            {
                if (!intsadded)
                {
                    ints.Add(fifthBoth.Chips);
                    intsadded = true;
                }
            }
            if (ints.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                ints.Clear();
            }
            #endregion

            var abc = bools.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = bools.IndexOf(false);
                if (index == 0)
                {
                    player.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = player.Chips.ToString();
                    this.player.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    firstBot.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = firstBot.Chips.ToString();
                    this.firstBot.Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    secondBot.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = secondBot.Chips.ToString();
                    this.secondBot.Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    thirdBot.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = thirdBot.Chips.ToString();
                    this.thirdBot.Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    forthBot.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = forthBot.Chips.ToString();
                    this.forthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    fifthBoth.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = fifthBoth.Chips.ToString();
                    this.fifthBoth.Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    holder[j].Visible = false;
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

            // TODO : extract in method Reset or smtg like that
            this.player.Panel.Visible = false; this.firstBot.Panel.Visible = false; this.secondBot.Panel.Visible = false; this.thirdBot.Panel.Visible = false; this.forthBot.Panel.Visible = false; this.fifthBoth.Panel.Visible = false;
            call = this.bigBlind; raise = 0;
            foldedPlayers = 5;
            type = 0; rounds = 0; this.firstBot.Power = 0; this.secondBot.Power = 0; this.thirdBot.Power = 0; this.forthBot.Power = 0; this.fifthBoth.Power = 0; this.player.Power = 0; player.Type = -1; raise = 0;
            this.firstBot.Type = -1; this.secondBot.Type = -1; this.thirdBot.Type = -1; this.forthBot.Type = -1; this.fifthBoth.Type = -1;
            this.firstBot.Turn = false; this.secondBot.Turn = false; this.thirdBot.Turn = false; this.forthBot.Turn = false; this.fifthBoth.Turn = false;
            this.firstBot.FoldedTurn = false; this.secondBot.FoldedTurn = false; this.thirdBot.FoldedTurn = false; this.forthBot.FoldedTurn = false; this.fifthBoth.FoldedTurn = false;
            this.player.Folded = false; this.firstBot.Folded = false; this.secondBot.Folded = false; this.thirdBot.Folded = false; this.forthBot.Folded = false; this.fifthBoth.Folded = false;
            this.playerFoldTurn = false; this.playerTurn = true; restart = false; raising = false;
            this.player.Call = 0;
            this.firstBot.Call = 0;
            this.secondBot.Call = 0;
            this.thirdBot.Call = 0;
            this.forthBot.Call = 0;
            this.fifthBoth.Call = 0;
            this.player.Raise = 0;
            this.firstBot.Raise = 0;
            this.secondBot.Raise = 0;
            this.thirdBot.Raise = 0;
            this.forthBot.Raise = 0;
            this.fifthBoth.Raise = 0;
            // height = 0;
            // width = 0;
            winners = 0;
            Flop = 1;
            Turn = 2;
            River = 3;
            End = 4;
            maxLeft = 6;
            raisedTurn = 1;
            bools.Clear();
            CheckWinners.Clear();
            ints.Clear();
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            tbPot.Text = "0";
            t = 60; turnCount = 0;
            this.playerStatus.Text = string.Empty;
            this.bot1Status.Text = string.Empty;
            b2Status.Text = string.Empty;
            b3Status.Text = string.Empty;
            b4Status.Text = string.Empty;
            b5Status.Text = string.Empty;

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
                    forthBot.Chips += addChips.AddedChips;
                    fifthBoth.Chips += addChips.AddedChips;
                    this.playerFoldTurn = false;
                    this.playerTurn = true;
                    bRaise.Enabled = true;
                    bFold.Enabled = true;
                    bCheck.Enabled = true;
                    bRaise.Text = "Raise";
                }
            }
            ImgLocation = Directory.GetFiles(@"..\..\Resources\Assets\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                holder[os].Image = null;
                holder[os].Invalidate();
                holder[os].Visible = false;
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
            if (!this.bot1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", this.firstBot);
            }
            if (!b2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", this.secondBot);
            }
            if (!b3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", this.thirdBot);
            }
            if (!b4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", this.forthBot);
            }
            if (!b5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", this.fifthBoth);
            }
            Winner(player.Type, this.player.Power, "Player", player.Chips, fixedLast);
            Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", firstBot.Chips, fixedLast);
            Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", secondBot.Chips, fixedLast);
            Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", thirdBot.Chips, fixedLast);
            Winner(this.forthBot.Type, this.forthBot.Power, "Bot 4", forthBot.Chips, fixedLast);
            Winner(this.fifthBoth.Type, this.fifthBoth.Power, "Bot 5", fifthBoth.Chips, fixedLast);
        }

        void AI(int c1, int c2, Label sStatus, int name, IPokerPlayer pokerPlayer)
        {
            if (!pokerPlayer.FoldedTurn)
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
            if (pokerPlayer.FoldedTurn)
            {
                holder[c1].Visible = false;
                holder[c2].Visible = false;
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
            pokerPlayer.Turn = false;
            pokerPlayer.FoldedTurn = true;
        }

        private void Check(IPokerPlayer pokerPlayer, Label cStatus)
        {
            cStatus.Text = "Check";
            pokerPlayer.Turn = false;
            raising = false;
        }

        private void Call(IPokerPlayer pokerPlayer, Label sStatus)
        {
            raising = false;
            pokerPlayer.Turn = false;
            pokerPlayer.Chips -= call;
            sStatus.Text = "Call " + call;
            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
        }

        private void Raised(IPokerPlayer pokerPlayer, Label sStatus)
        {
            pokerPlayer.Chips -= Convert.ToInt32(raise);
            sStatus.Text = "Raise " + raise;
            tbPot.Text = (int.Parse(tbPot.Text) + Convert.ToInt32(raise)).ToString();
            call = Convert.ToInt32(raise);
            raising = true;
            pokerPlayer.Turn = false;
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
            if (call <= 0)
            {
                Check(pokerPlayer, sStatus);
            }
            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(pokerPlayer.Chips, n))
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
                    if (call <= RoundN(pokerPlayer.Chips, n1))
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
                    raise = call * 2;
                    Raised(pokerPlayer, sStatus);
                }
                else
                {
                    if (raise <= RoundN(pokerPlayer.Chips, n))
                    {
                        raise = call * 2;
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
                pokerPlayer.FoldedTurn = true;
            }
        }

        private void PH(IPokerPlayer pokerPlayer, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(pokerPlayer, sStatus);
                }
                if (call > 0)
                {
                    if (call >= RoundN(pokerPlayer.Chips, n1))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (raise > RoundN(pokerPlayer.Chips, n))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (!pokerPlayer.FoldedTurn)
                    {
                        if (call >= RoundN(pokerPlayer.Chips, n) && call <= RoundN(pokerPlayer.Chips, n1))
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
                                raise = call * 2;
                                Raised(pokerPlayer, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(pokerPlayer.Chips, n1 - rnd))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (raise > RoundN(pokerPlayer.Chips, n - rnd))
                    {
                        Fold(pokerPlayer, sStatus);
                    }
                    if (!pokerPlayer.FoldedTurn)
                    {
                        if (call >= RoundN(pokerPlayer.Chips, n - rnd) && call <= RoundN(pokerPlayer.Chips, n1 - rnd))
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
                                raise = call * 2;
                                Raised(pokerPlayer, sStatus);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    raise = (int)RoundN(pokerPlayer.Chips, r - rnd);
                    Raised(pokerPlayer, sStatus);
                }
            }
            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.FoldedTurn = true;
            }
        }

        void Smooth(IPokerPlayer pokerPlayer, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(pokerPlayer, botStatus);
            }
            else
            {
                if (call >= RoundN(pokerPlayer.Chips, n))
                {
                    if (pokerPlayer.Chips > call)
                    {
                        Call(pokerPlayer, botStatus);
                    }
                    else if (pokerPlayer.Chips <= call)
                    {
                        raising = false;
                        pokerPlayer.Turn = false;
                        pokerPlayer.Chips = 0;
                        botStatus.Text = "Call " + pokerPlayer.Chips;
                        tbPot.Text = (int.Parse(tbPot.Text) + pokerPlayer.Chips).ToString();
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
                        raise = call * 2;
                        Raised(pokerPlayer, botStatus);
                    }
                }
            }
            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.FoldedTurn = true;
            }
        }

        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                this.playerFoldTurn = true;
                await Turns();
            }
            if (t > 0)
            {
                t--;
                pbTimer.Value = (t / 6) * 100;
            }
        }

        private void Update_Tick(object sender, object e)
        {
            if (player.Chips <= 0)
            {
                tbChips.Text = "Chips : 0";
            }
            if (firstBot.Chips <= 0)
            {
                tbBotChips1.Text = "Chips : 0";
            }
            if (secondBot.Chips <= 0)
            {
                tbBotChips2.Text = "Chips : 0";
            }
            if (thirdBot.Chips <= 0)
            {
                tbBotChips3.Text = "Chips : 0";
            }
            if (forthBot.Chips <= 0)
            {
                tbBotChips4.Text = "Chips : 0";
            }
            if (fifthBoth.Chips <= 0)
            {
                tbBotChips5.Text = "Chips : 0";
            }

            tbChips.Text = "Chips : " + player.Chips.ToString();
            tbBotChips1.Text = "Chips : " + firstBot.Chips.ToString();
            tbBotChips2.Text = "Chips : " + secondBot.Chips.ToString();
            tbBotChips3.Text = "Chips : " + thirdBot.Chips.ToString();
            tbBotChips4.Text = "Chips : " + forthBot.Chips.ToString();
            tbBotChips5.Text = "Chips : " + fifthBoth.Chips.ToString();

            if (player.Chips <= 0)
            {
                this.playerTurn = false;
                this.playerFoldTurn = true;
                bCall.Enabled = false;
                bRaise.Enabled = false;
                bFold.Enabled = false;
                bCheck.Enabled = false;
            }

            if (player.Chips >= call)
            {
                bCall.Text = "Call " + call.ToString();
            }
            else
            {
                bCall.Text = "All in";
                bRaise.Enabled = false;
            }

            if (call > 0)
            {
                bCheck.Enabled = false;
            }
            if (call <= 0)
            {
                bCheck.Enabled = true;
                bCall.Text = "Call";
                bCall.Enabled = false;
            }
            if (player.Chips <= 0)
            {
                bRaise.Enabled = false;
            }

            int parsedValue;
            if (tbRaise.Text != string.Empty && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (player.Chips <= parsedValue)
                {
                    bRaise.Text = "All in";
                }
                else
                {
                    bRaise.Text = "Raise";
                }
            }
            if (player.Chips < call)
            {
                bRaise.Enabled = false;
            }
        }

        private async void FoldClick(object sender, EventArgs e)
        {
            this.playerStatus.Text = "Fold";
            this.playerTurn = false;
            this.playerFoldTurn = true;
            await Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                this.playerTurn = false;
                this.playerStatus.Text = "Check";
            }
            else
            {
                //playerStatus.Text = "All in " + Chips;

                bCheck.Enabled = false;
            }

            await Turns();
        }

        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", this.player);

            if (player.Chips >= call)
            {
                player.Chips -= call;
                tbChips.Text = "Chips : " + player.Chips.ToString();
                if (tbPot.Text != string.Empty)
                {
                    tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                }
                else
                {
                    tbPot.Text = call.ToString();
                }
                this.playerTurn = false;
                this.playerStatus.Text = "Call " + call;
                this.player.Call = call;
            }
            else if (player.Chips <= call && call > 0)
            {
                tbPot.Text = (int.Parse(tbPot.Text) + player.Chips).ToString();
                this.playerStatus.Text = "All in " + player.Chips;
                player.Chips = 0;
                tbChips.Text = "Chips : " + player.Chips.ToString();
                this.playerTurn = false;
                bFold.Enabled = false;
                this.player.Call = player.Chips;
            }

            await Turns();
        }

        private async void RaiseClick(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", this.player);

            int parsedValue;
            bool isValidNumber = int.TryParse(this.tbRaise.Text, out parsedValue);
            if (isValidNumber)
            {
                if (this.player.Chips > this.call)
                {
                    if (raise * 2 > parsedValue)
                    {
                        tbRaise.Text = (raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }

                    if (player.Chips >= parsedValue)
                    {
                        call = parsedValue;
                        raise = parsedValue;
                        this.playerStatus.Text = "Raise " + call.ToString();
                        tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                        bCall.Text = "Call";
                        player.Chips -= parsedValue;
                        raising = true;
                        this.player.Raise = raise;
                    }
                    // all in scenario
                    else
                    {
                        call = player.Chips;
                        raise = player.Chips;
                        tbPot.Text = (int.Parse(tbPot.Text) + player.Chips).ToString();
                        this.playerStatus.Text = "Raise " + call.ToString();
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

            this.playerTurn = false;
            await Turns();
        }

        /// <summary>
        /// Add chips to all players on table.
        /// </summary>
        private void AddChipsClick(object sender, EventArgs e)
        {
            int addedChips = 0;
            bool isValidNumber = false;
            isValidNumber = int.TryParse(this.tbAddChips.Text, out addedChips);

            if (isValidNumber && addedChips > 0)
            {
                player.Chips += addedChips;
                firstBot.Chips += addedChips;
                secondBot.Chips += addedChips;
                thirdBot.Chips += addedChips;
                forthBot.Chips += addedChips;
                fifthBoth.Chips += addedChips;
                tbChips.Text = "Chips : " + player.Chips.ToString();
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
            this.tbBigBlind.Text = this.bigBlind.ToString();
            this.tbSmallBlind.Text = this.smallBlind.ToString();
            if (this.tbBigBlind.Visible == false)
            {
                this.tbBigBlind.Visible = true;
                this.tbSmallBlind.Visible = true;
                this.bBigBlind.Visible = true;
                this.bSmallBlind.Visible = true;
            }
            else
            {
                this.tbBigBlind.Visible = false;
                this.tbSmallBlind.Visible = false;
                this.bBigBlind.Visible = false;
                this.bSmallBlind.Visible = false;
            }
        }

        private void SmallBlindClick(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.tbSmallBlind.Text.Contains(",") || this.tbSmallBlind.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                this.tbSmallBlind.Text = this.smallBlind.ToString();
                return;
            }
            if (!int.TryParse(this.tbSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.tbSmallBlind.Text = this.smallBlind.ToString();
                return;
            }
            if (int.Parse(this.tbSmallBlind.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                this.tbSmallBlind.Text = this.smallBlind.ToString();
            }
            if (int.Parse(this.tbSmallBlind.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(this.tbSmallBlind.Text) >= 250 && int.Parse(this.tbSmallBlind.Text) <= 100000)
            {
                this.smallBlind = int.Parse(this.tbSmallBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play.");
            }
        }

        private void bBigBlind_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.tbBigBlind.Text.Contains(",") || this.tbBigBlind.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number!");
                this.tbBigBlind.Text = this.bigBlind.ToString();
                return;
            }
            if (!int.TryParse(this.tbSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.tbSmallBlind.Text = this.bigBlind.ToString();
                return;
            }
            if (int.Parse(this.tbBigBlind.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                this.tbBigBlind.Text = this.bigBlind.ToString();
            }
            if (int.Parse(this.tbBigBlind.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500");
            }
            if (int.Parse(this.tbBigBlind.Text) >= 500 && int.Parse(this.tbBigBlind.Text) <= 200000)
            {
                this.bigBlind = int.Parse(this.tbBigBlind.Text);
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