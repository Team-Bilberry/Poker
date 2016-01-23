namespace Poker.PokerUtilities.CardsCombinationMethods
{
    using System;
    using System.Windows.Forms;
    using Contracts;
    using static PlayerActions;

    public class HandTypes
    {
        private readonly PlayerActions playerActions;
        private IRandomProvider randomGenerator;

        public HandTypes(IRandomProvider randomGenerator)
        {
            this.randomGenerator = randomGenerator;
            this.playerActions = new PlayerActions();
        }

        public void HighCard(IPokerPlayer pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            this.playerActions.HP(pokerPlayer, sStatus, 20, 25, neededChipsToCall, potStatus, ref raise, ref raising);
        }

        public void PairTable(IPokerPlayer pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            this.playerActions.HP(pokerPlayer, sStatus, 16, 25, neededChipsToCall, potStatus, ref raise, ref raising);
        }

        public void PairHand(IPokerPlayer  pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rCall = this.randomGenerator.Next(10, 16);
            int rRaise = this.randomGenerator.Next(10, 13);
            if (pokerPlayer.Power <= 199 && pokerPlayer.Power >= 140)
            {
                this.playerActions.PH(pokerPlayer, sStatus, rCall, 6, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power <= 139 && pokerPlayer.Power >= 128)
            {
                this.playerActions.PH(pokerPlayer, sStatus, rCall, 7, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power < 128 && pokerPlayer.Power >= 101)
            {
                this.playerActions.PH(pokerPlayer, sStatus, rCall, 9, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }
        }

        public void TwoPair(IPokerPlayer  pokerPlayer, Label sStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rCall = this.randomGenerator.Next(6, 11);
            int rRaise = this.randomGenerator.Next(6, 11);
            if (pokerPlayer.Power <= 290 && pokerPlayer.Power >= 246)
            {
                this.playerActions.PH(pokerPlayer, sStatus, rCall, 3, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power <= 244 && pokerPlayer.Power >= 234)
            {
                this.playerActions.PH(pokerPlayer, sStatus, rCall, 4, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (pokerPlayer.Power < 234 && pokerPlayer.Power >= 201)
            {
                this.playerActions.PH(pokerPlayer, sStatus, rCall, 4, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }
        }

        public void ThreeOfAKind(IPokerPlayer  pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int tCall = this.randomGenerator.Next(3, 7);
            int tRaise = this.randomGenerator.Next(4, 8);
            if (pokerPlayer.Power <= 390 && pokerPlayer.Power >= 330)
            {
                this.Smooth(pokerPlayer, sStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power <= 327 && pokerPlayer.Power >= 321)//10  8
            {
                this.Smooth(pokerPlayer, sStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power < 321 && pokerPlayer.Power >= 303)//7 2
            {
                this.Smooth(pokerPlayer, sStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Straight(IPokerPlayer  pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int sCall = this.randomGenerator.Next(3, 6);
            int sRaise = this.randomGenerator.Next(3, 8);
            if (pokerPlayer.Power <= 480 && pokerPlayer.Power >= 410)
            {
                this.Smooth(pokerPlayer, sStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power <= 409 && pokerPlayer.Power >= 407)//10  8
            {
                this.Smooth(pokerPlayer, sStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power < 407 && pokerPlayer.Power >= 404)
            {
                this.Smooth(pokerPlayer, sStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Flush(IPokerPlayer  pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fCall = this.randomGenerator.Next(2, 6);
            int fRaise = this.randomGenerator.Next(3, 7);
            this.Smooth(pokerPlayer, sStatus, name, fCall, fRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
        }

        public void FullHouse(IPokerPlayer  pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fhCall = this.randomGenerator.Next(1, 5);
            int fhRaise = this.randomGenerator.Next(2, 6);
            if (pokerPlayer.Power <= 626 && pokerPlayer.Power >= 620)
            {
                this.Smooth(pokerPlayer, sStatus, name, fhCall, fhRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (pokerPlayer.Power < 620 && pokerPlayer.Power >= 602)
            {
                this.Smooth(pokerPlayer, sStatus, name, fhCall, fhRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void FourOfAKind(IPokerPlayer  pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fkCall = this.randomGenerator.Next(1, 4);
            int fkRaise = this.randomGenerator.Next(2, 5);
            if (pokerPlayer.Power <= 752 && pokerPlayer.Power >= 704)
            {
                this.Smooth(pokerPlayer, sStatus, name, fkCall, fkRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void StraightFlush(IPokerPlayer  pokerPlayer, Label sStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int sfCall = this.randomGenerator.Next(1, 3);
            int sfRaise = this.randomGenerator.Next(1, 3);
            if (pokerPlayer.Power <= 913 && pokerPlayer.Power >= 804)
            {
                this.Smooth(pokerPlayer, sStatus, name, sfCall, sfRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        private void Smooth(IPokerPlayer  pokerPlayer, Label botStatus, int name, int n, int r, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rnd = this.randomGenerator.Next(1, 3);
            if (neededChipsToCall <= 0)
            {
                this.playerActions.Check(pokerPlayer, botStatus, ref raising);
            }
            else
            {
                if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n))
                {
                    if (pokerPlayer.Chips > neededChipsToCall)
                    {
                        this.playerActions.Call(pokerPlayer, botStatus, ref raising, ref neededChipsToCall, potStatus);
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
                            this.playerActions.Raised(pokerPlayer, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                        }
                        else
                        {
                            this.playerActions.Call(pokerPlayer, botStatus, ref raising, ref neededChipsToCall, potStatus);
                        }
                    }
                    else
                    {
                        raise = neededChipsToCall * 2;
                        this.playerActions.Raised(pokerPlayer, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
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