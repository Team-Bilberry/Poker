namespace Poker.PokerUtilities.CardsCombinationMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    public class CheckHandType
    {
        public CheckHandType()
        {
        }

        public void rStraightFlush(ref IPokerPlayer pokerPlayer, int[] st1, int[] st2, int[] st3, int[] st4, ref List<Type> Win, ref Type sorted)
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

        public void rFourOfAKind(ref IPokerPlayer pokerPlayer, int[] Straight, ref List<Type> Win, ref Type sorted)
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

        public void rFullHouse(ref IPokerPlayer pokerPlayer, ref bool done, int[] Straight, ref List<Type> Win, ref Type sorted, ref double type)
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

        public void rFlush(ref IPokerPlayer pokerPlayer, ref bool vf, int[] Straight1, ref int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve)
        {
            if (pokerPlayer.Type >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f1.Max() / 4 && Reserve[index + 1] / 4 < f1.Max() / 4)
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
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
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

                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
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
                    if (Reserve[index] % 4 == f1[0] % 4 && Reserve[index] / 4 > f1.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[index + 1] % 4 == f1[0] % 4 && Reserve[index + 1] / 4 > f1.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f1.Min() / 4 && Reserve[index + 1] / 4 < f1.Min())
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
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f2.Max() / 4 && Reserve[index + 1] / 4 < f2.Max() / 4)
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
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
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

                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
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
                    if (Reserve[index] % 4 == f2[0] % 4 && Reserve[index] / 4 > f2.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[index + 1] % 4 == f2[0] % 4 && Reserve[index + 1] / 4 > f2.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f2.Min() / 4 && Reserve[index + 1] / 4 < f2.Min())
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
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f3.Max() / 4 && Reserve[index + 1] / 4 < f3.Max() / 4)
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
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
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

                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
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
                    if (Reserve[index] % 4 == f3[0] % 4 && Reserve[index] / 4 > f3.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[index + 1] % 4 == f3[0] % 4 && Reserve[index + 1] / 4 > f3.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f3.Min() / 4 && Reserve[index + 1] / 4 < f3.Min())
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
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (Reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f4.Max() / 4 && Reserve[index + 1] / 4 < f4.Max() / 4)
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
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
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

                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            pokerPlayer.Type = 5;
                            pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
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
                    if (Reserve[index] % 4 == f4[0] % 4 && Reserve[index] / 4 > f4.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (Reserve[index + 1] % 4 == f4[0] % 4 && Reserve[index + 1] / 4 > f4.Min() / 4)
                    {
                        pokerPlayer.Type = 5;
                        pokerPlayer.Power = Reserve[index + 1] + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f4.Min() / 4 && Reserve[index + 1] / 4 < f4.Min())
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
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f4[0] % 4 && vf)
                    {
                        pokerPlayer.Type = 5.5;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rStraight(ref IPokerPlayer pokerPlayer, int[] Straight, int index, ref List<Type> Win, ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (index = 0; index < op.Length - 4; index++)
                {
                    if (op[index] + 4 == op[index + 4])
                    {
                        if (op.Max() - 4 == op[index])
                        {
                            pokerPlayer.Type = 4;
                            pokerPlayer.Power = op.Max() + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            pokerPlayer.Type = 4;
                            pokerPlayer.Power = op[index + 4] + pokerPlayer.Type * 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[index] == 0 && op[index + 1] == 9 && op[index + 2] == 10 && op[index + 3] == 11 && op[index + 4] == 12)
                    {
                        pokerPlayer.Type = 4;
                        pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                        Win.Add(new Type() { Power = pokerPlayer.Power, Current = 4 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void rThreeOfAKind(ref IPokerPlayer pokerPlayer, int[] Straight, int index, ref List<Type> Win, ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                for (index = 0; index <= 12; index++)
                {
                    var fh = Straight.Where(o => o / 4 == index).ToArray();
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

        public void rTwoPair(ref IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve)
        {
            if (pokerPlayer.Type >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (Reserve[index] / 4 != Reserve[index + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (Reserve[index] / 4 == Reserve[tc] / 4 && Reserve[index + 1] / 4 == Reserve[tc - k] / 4 ||
                                    Reserve[index + 1] / 4 == Reserve[tc] / 4 && Reserve[index] / 4 == Reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[index] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = 13 * 4 + (Reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted =
                                                    Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (Reserve[index + 1] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = 13 * 4 + (Reserve[index] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (Reserve[index + 1] / 4 != 0 && Reserve[index] / 4 != 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (Reserve[index] / 4) * 2 + (Reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
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

        public void rPairTwoPair(ref IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve)
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
                            if (Reserve[tc] / 4 == Reserve[tc - k] / 4)
                            {
                                if (Reserve[tc] / 4 != Reserve[index] / 4 && Reserve[tc] / 4 != Reserve[index + 1] / 4 && pokerPlayer.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[index + 1] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (Reserve[index] / 4) * 2 + 13 * 4 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (Reserve[index] / 4 == 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (Reserve[index + 1] / 4) * 2 + 13 * 4 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (Reserve[index + 1] / 4 != 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (Reserve[tc] / 4) * 2 + (Reserve[index + 1] / 4) * 2 + pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (Reserve[index] / 4 != 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (Reserve[tc] / 4) * 2 + (Reserve[index] / 4) * 2 + pokerPlayer.Type * 100;
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
                                        if (Reserve[index] / 4 > Reserve[index + 1] / 4)
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = 13 + Reserve[index] / 4 + pokerPlayer.Type * 100;
                                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = Reserve[tc] / 4 + Reserve[index] / 4 + pokerPlayer.Type * 100;
                                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = 13 + Reserve[index + 1] + pokerPlayer.Type * 100;
                                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                pokerPlayer.Type = 0;
                                                pokerPlayer.Power = Reserve[tc] / 4 + Reserve[index + 1] / 4 + pokerPlayer.Type * 100;
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

        public void rPairFromHand(ref IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve)
        {
            if (pokerPlayer.Type >= -1)
            {
                bool msgbox = false;
                if (Reserve[index] / 4 == Reserve[index + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (Reserve[index] / 4 == 0)
                        {
                            pokerPlayer.Type = 1;
                            pokerPlayer.Power = 13 * 4 + 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            pokerPlayer.Type = 1;
                            pokerPlayer.Power = (Reserve[index + 1] / 4) * 4 + 100;
                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (Reserve[index + 1] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[index + 1] / 4 == 0)
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = 13 * 4 + Reserve[index] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = (Reserve[index + 1] / 4) * 4 + Reserve[index] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }

                    if (Reserve[index] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[index] / 4 == 0)
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = 13 * 4 + Reserve[index + 1] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = (Reserve[tc] / 4) * 4 + Reserve[index + 1] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }

        public void rHighCard(ref IPokerPlayer pokerPlayer, int index, ref List<Type> Win, ref Type sorted, ref int[] Reserve)
        {
            if (pokerPlayer.Type == -1)
            {
                if (Reserve[index] / 4 > Reserve[index + 1] / 4)
                {
                    pokerPlayer.Type = -1;
                    pokerPlayer.Power = Reserve[index] / 4;
                    Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    pokerPlayer.Type = -1;
                    pokerPlayer.Power = Reserve[index + 1] / 4;
                    Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (Reserve[index] / 4 == 0 || Reserve[index + 1] / 4 == 0)
                {
                    pokerPlayer.Type = -1;
                    pokerPlayer.Power = 13;
                    Win.Add(new Type() { Power = pokerPlayer.Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }
    }
}