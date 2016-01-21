namespace Poker.PokerUtilities.CardsCombinationMethods
{
    using System;
    using System.Windows.Forms;
    using Contracts;

    public class PlayerActions
    {
        public PlayerActions()
        {
        }

        public void Fold(ref IPokerPlayer pokerPlayer, Label sStatus, ref bool rising)
        {
            rising = false;
            sStatus.Text = "Fold";
            pokerPlayer.AbleToMakeTurn = false;
            pokerPlayer.OutOfChips = true;
        }

        public void Check(ref IPokerPlayer pokerPlayer, Label cStatus, ref bool raising)
        {
            cStatus.Text = "Check";
            pokerPlayer.AbleToMakeTurn = false;
            raising = false;
        }

        public void Call(ref IPokerPlayer pokerPlayer, Label sStatus, ref bool raising, ref int neededChipsToCall, TextBox potStatus)
        {
            raising = false;
            pokerPlayer.AbleToMakeTurn = false;
            pokerPlayer.Chips -= neededChipsToCall;
            sStatus.Text = "Call " + neededChipsToCall;
            potStatus.Text = (int.Parse(potStatus.Text) + neededChipsToCall).ToString();
        }

        public void Raised(ref IPokerPlayer pokerPlayer, Label sStatus, ref bool raising, ref int raise, ref int neededChipsToCall, TextBox potStatus)
        {
            pokerPlayer.Chips -= Convert.ToInt32(raise);
            sStatus.Text = "Raise " + raise;
            potStatus.Text = (int.Parse(potStatus.Text) + Convert.ToInt32(raise)).ToString();
            neededChipsToCall = Convert.ToInt32(raise);
            raising = true;
            pokerPlayer.AbleToMakeTurn = false;
        }

        public static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        // TODO: think of more proper names to these methods.
        public void HP(ref IPokerPlayer pokerPlayer, Label sStatus, int n, int n1, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (neededChipsToCall <= 0)
            {
                Check(ref pokerPlayer, sStatus, ref raising);
            }

            if (neededChipsToCall > 0)
            {
                if (rnd == 1)
                {
                    if (neededChipsToCall <= RoundN(pokerPlayer.Chips, n))
                    {
                        Call(ref pokerPlayer, sStatus, ref raising, ref neededChipsToCall, potStatus);
                    }
                    else
                    {
                        Fold(ref pokerPlayer, sStatus, ref raising);
                    }
                }

                if (rnd == 2)
                {
                    if (neededChipsToCall <= RoundN(pokerPlayer.Chips, n1))
                    {
                        Call(ref pokerPlayer, sStatus, ref raising, ref neededChipsToCall, potStatus);
                    }
                    else
                    {
                        Fold(ref pokerPlayer, sStatus, ref raising);
                    }
                }
            }

            if (rnd == 3)
            {
                if (raise == 0)
                {
                    raise = neededChipsToCall * 2;
                    Raised(ref pokerPlayer, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                }
                else
                {
                    if (raise <= RoundN(pokerPlayer.Chips, n))
                    {
                        raise = neededChipsToCall * 2;
                        Raised(ref pokerPlayer, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                    }
                    else
                    {
                        Fold(ref pokerPlayer, sStatus, ref raising);
                    }
                }
            }

            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.OutOfChips = true;
            }
        }

        public void PH(ref IPokerPlayer pokerPlayer, Label sStatus, int n, int n1, int r, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, int rounds)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (neededChipsToCall <= 0)
                {
                    Check(ref pokerPlayer, sStatus, ref raising);
                }

                if (neededChipsToCall > 0)
                {
                    if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n1))
                    {
                        Fold(ref pokerPlayer, sStatus, ref raising);
                    }

                    if (raise > RoundN(pokerPlayer.Chips, n))
                    {
                        Fold(ref pokerPlayer, sStatus, ref raising);
                    }

                    if (!pokerPlayer.OutOfChips)
                    {
                        if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n) && neededChipsToCall <= RoundN(pokerPlayer.Chips, n1))
                        {
                            Call(ref pokerPlayer, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= RoundN(pokerPlayer.Chips, n) && raise >= (RoundN(pokerPlayer.Chips, n)) / 2)
                        {
                            Call(ref pokerPlayer, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= (RoundN(pokerPlayer.Chips, n)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(pokerPlayer.Chips, n);
                                Raised(ref pokerPlayer, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                            }
                            else
                            {
                                raise = neededChipsToCall * 2;
                                Raised(ref pokerPlayer, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
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
                        Fold(ref pokerPlayer, sStatus, ref raising);
                    }

                    if (raise > RoundN(pokerPlayer.Chips, n - rnd))
                    {
                        Fold(ref pokerPlayer, sStatus, ref raising);
                    }

                    if (!pokerPlayer.OutOfChips)
                    {
                        if (neededChipsToCall >= RoundN(pokerPlayer.Chips, n - rnd) && neededChipsToCall <= RoundN(pokerPlayer.Chips, n1 - rnd))
                        {
                            Call(ref pokerPlayer, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= RoundN(pokerPlayer.Chips, n - rnd) && raise >= (RoundN(pokerPlayer.Chips, n - rnd)) / 2)
                        {
                            Call(ref pokerPlayer, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= (RoundN(pokerPlayer.Chips, n - rnd)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(pokerPlayer.Chips, n - rnd);
                                Raised(ref pokerPlayer, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                            }
                            else
                            {
                                raise = neededChipsToCall * 2;
                                Raised(ref pokerPlayer, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                            }
                        }
                    }
                }

                if (neededChipsToCall <= 0)
                {
                    raise = (int)RoundN(pokerPlayer.Chips, r - rnd);
                    Raised(ref pokerPlayer, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                }
            }

            if (pokerPlayer.Chips <= 0)
            {
                pokerPlayer.OutOfChips = true;
            }
        }
    }
}