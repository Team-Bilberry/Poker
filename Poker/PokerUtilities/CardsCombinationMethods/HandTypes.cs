namespace Poker.PokerUtilities.CardsCombinationMethods
{
    using System;
    using System.Windows.Forms;
    using Contracts;
    using static PlayerActions;

    public class HandTypes
    {
        PlayerActions playerActions = new PlayerActions();

        public void HighCard(ref IPokerPlayer pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            playerActions.HP(ref pokerPlayer, sStatus, 20, 25, neededChipsToCall, potStatus, ref raise, ref raising);
        }

        public void PairTable(ref IPokerPlayer pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            playerActions.HP(ref pokerPlayer, sStatus, 16, 25, neededChipsToCall, potStatus, ref raise, ref raising);
        }

        public void PairHand(ref IPokerPlayer pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (pokerPlayer.Power <= 199 && pokerPlayer.Power >= 140)
            {
                playerActions.PH(ref pokerPlayer, sStatus, rCall, 6, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power <= 139 && pokerPlayer.Power >= 128)
            {
                playerActions.PH(ref pokerPlayer, sStatus, rCall, 7, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power < 128 && pokerPlayer.Power >= 101)
            {
                playerActions.PH(ref pokerPlayer, sStatus, rCall, 9, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }
        }

        public void TwoPair(ref IPokerPlayer pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (pokerPlayer.Power <= 290 && pokerPlayer.Power >= 246)
            {
                playerActions.PH(ref pokerPlayer, sStatus, rCall, 3, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power <= 244 && pokerPlayer.Power >= 234)
            {
                playerActions.PH(ref pokerPlayer, sStatus, rCall, 4, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power < 234 && pokerPlayer.Power >= 201)
            {
                playerActions.PH(ref pokerPlayer, sStatus, rCall, 4, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }
        }

        public void ThreeOfAKind(ref IPokerPlayer pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (pokerPlayer.Power <= 390 && pokerPlayer.Power >= 330)
            {
                Smooth(ref pokerPlayer, sStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power <= 327 && pokerPlayer.Power >= 321)//10  8
            {
                Smooth(ref pokerPlayer, sStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power < 321 && pokerPlayer.Power >= 303)//7 2
            {
                Smooth(ref pokerPlayer, sStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Straight(ref IPokerPlayer pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (pokerPlayer.Power <= 480 && pokerPlayer.Power >= 410)
            {
                Smooth(ref pokerPlayer, sStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power <= 409 && pokerPlayer.Power >= 407)//10  8
            {
                Smooth(ref pokerPlayer, sStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power < 407 && pokerPlayer.Power >= 404)
            {
                Smooth(ref pokerPlayer, sStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Flush(ref IPokerPlayer pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref pokerPlayer, sStatus, name, fCall, fRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
        }

        public void FullHouse(ref IPokerPlayer pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (pokerPlayer.Power <= 626 && pokerPlayer.Power >= 620)
            {
                Smooth(ref pokerPlayer, sStatus, name, fhCall, fhRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power < 620 && pokerPlayer.Power >= 602)
            {
                Smooth(ref pokerPlayer, sStatus, name, fhCall, fhRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void FourOfAKind(ref IPokerPlayer pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (pokerPlayer.Power <= 752 && pokerPlayer.Power >= 704)
            {
                Smooth(ref pokerPlayer, sStatus, name, fkCall, fkRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void StraightFlush(ref IPokerPlayer pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (pokerPlayer.Power <= 913 && pokerPlayer.Power >= 804)
            {
                Smooth(ref pokerPlayer, sStatus, name, sfCall, sfRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        private void Smooth(ref IPokerPlayer pokerPlayer, Label botStatus, int name, int n, int r, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (neededChipsToCall <= 0)
            {
                playerActions.Check(ref pokerPlayer, botStatus, ref raising);
            }
            else
            {
                if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n))
                {
                    if (pokerPlayer.Chips > neededChipsToCall)
                    {
                        playerActions.Call(ref pokerPlayer, botStatus, ref raising, ref neededChipsToCall, potStatus);
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
                            playerActions.Raised(ref pokerPlayer, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                        }
                        else
                        {
                            playerActions.Call(ref pokerPlayer, botStatus, ref raising, ref neededChipsToCall, potStatus);
                        }
                    }
                    else
                    {
                        raise = neededChipsToCall * 2;
                        playerActions.Raised(ref pokerPlayer, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                    }
                }
            }

            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.OutOfChips = true;
            }
        }
    }
}