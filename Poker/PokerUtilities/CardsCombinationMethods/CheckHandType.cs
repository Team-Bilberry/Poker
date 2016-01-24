namespace Poker.PokerUtilities.CardsCombinationMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    public class CheckHandType : ICheckHandType
    {
        private static void RankHand(
            IPokerPlayer pokerPlayer,
            List<Type> Win,
            int power,
            int rankOfHand,
            out Type sorted)
        {
            pokerPlayer.Type = rankOfHand;
            pokerPlayer.Power = power + pokerPlayer.Type * 100;

            Win.Add(new Type() { Power = pokerPlayer.Power, Current = rankOfHand });

            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        }

        public void StraightFlush(
            IPokerPlayer pokerPlayer,
            int[] clubes,
            int[] dimonds,
            int[] hearts,
            int[] spades,
            ref List<Type> Win,
            ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                if (clubes.Length >= 5)
                {
                    CheckForStraightFlush(pokerPlayer, clubes, Win, out sorted);
                }

                if (dimonds.Length >= 5)
                {
                    CheckForStraightFlush(pokerPlayer, dimonds, Win, out sorted);
                }

                if (hearts.Length >= 5)
                {
                    CheckForStraightFlush(pokerPlayer, hearts, Win, out sorted);
                }

                if (spades.Length >= 5)
                {
                    CheckForStraightFlush(pokerPlayer, spades, Win, out sorted);
                }
            }
        }

        private static void CheckForStraightFlush(IPokerPlayer pokerPlayer, int[] colour, List<Type> Win, out Type sorted)
        {
            if (colour[0] + 4 == colour[4])
            {
                //Straight Flush
                RankHand(pokerPlayer, Win, colour.Max() / 4, 8, out sorted);
            }

            if (colour[0] == 0 && colour[1] == 9 && colour[2] == 10 && colour[3] == 11 && colour[4] == 12)
            {
                //Royal Straight Flush
                RankHand(pokerPlayer, Win, colour.Max() / 4, 9, out sorted);
            }

            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        }

        public void FourOfAKind(IPokerPlayer pokerPlayer, int[] Straight, ref List<Type> Win, ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 &&
                        Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        RankHand(pokerPlayer, Win, Straight[j] / 4, 7, out sorted);
                    }

                    if (Straight[j] / 4 == 0 &&
                        Straight[j + 1] / 4 == 0 &&
                        Straight[j + 2] / 4 == 0 &&
                        Straight[j + 3] / 4 == 0)
                    {
                        RankHand(pokerPlayer, Win, 13, 7, out sorted);
                    }
                }
            }
        }

        public void FullHouse(
            IPokerPlayer pokerPlayer,
            int[] Straight,
            ref List<Type> Win,
            ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                bool done = false;
                double power = pokerPlayer.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var currentKind = Straight.Where(o => o / 4 == j).ToArray();
                    if (currentKind.Length == 3 || done)
                    {
                        if (currentKind.Length == 2)
                        {
                            if (currentKind.Max() / 4 == 0)
                            {
                                RankHand(pokerPlayer, Win, 13, 6, out sorted);

                                break;
                            }

                            if (currentKind.Max() / 4 > 0)
                            {
                                RankHand(pokerPlayer, Win, currentKind.Max() / 4, 7, out sorted);

                                break;
                            }
                        }

                        if (!done)
                        {
                            if (currentKind.Max() / 4 == 0)
                            {
                                pokerPlayer.Power = 13;
                            }
                            else
                            {
                                pokerPlayer.Power = currentKind.Max() / 4;
                            }
                            done = true;
                            j = -1;
                        }
                    }
                }

                if (pokerPlayer.Type != 6)
                {
                    pokerPlayer.Power = power;
                }
            }
        }
        private static void CheckForFlush(IPokerPlayer pokerPlayer, List<Type> Win, int[] colour)
        {
            if (colour.Length >= 5)
            {
                pokerPlayer.Type = 5;
                pokerPlayer.Power = colour.Max() / 4 + pokerPlayer.Type * 100;

                if (colour.Max() / 4 == 0)
                {
                    pokerPlayer.Power = 13 + pokerPlayer.Type * 100;
                }

                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 5 });
            }
        }
        public void Flush(
            IPokerPlayer pokerPlayer,
            int[] Straight1,
            ref List<Type> Win,
            ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                var clubes = Straight1.Where(o => o % 4 == 0).ToArray();
                var diamonds = Straight1.Where(o => o % 4 == 1).ToArray();
                var hearts = Straight1.Where(o => o % 4 == 2).ToArray();
                var spades = Straight1.Where(o => o % 4 == 3).ToArray();

                CheckForFlush(pokerPlayer, Win, clubes);

                CheckForFlush(pokerPlayer, Win, diamonds);

                CheckForFlush(pokerPlayer, Win, hearts);

                CheckForFlush(pokerPlayer, Win, spades);


            }
        }

        public void Straight(
            IPokerPlayer pokerPlayer,
            int[] Straight,
            ref List<Type> Win,
            ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                var kind = Straight.Select(o => o / 4).Distinct().ToArray();

                for (int index = 0; index < kind.Length - 4; index++)
                {
                    if (kind[index] + 4 == kind[index + 4])
                    {
                        if (kind.Max() - 4 == kind[index])
                        {
                            RankHand(pokerPlayer, Win, kind.Max(), 4, out sorted);
                        }
                        else
                        {
                            RankHand(pokerPlayer, Win, kind[index + 4], 4, out sorted);
                        }
                    }

                    if (kind[index] == 0 && kind[index + 1] == 9 && kind[index + 2] == 10 && kind[index + 3] == 11 && kind[index + 4] == 12)
                    {
                        RankHand(pokerPlayer, Win, 13, 4, out sorted);
                    }
                }
            }
        }

        public void ThreeOfAKind(IPokerPlayer pokerPlayer, int[] Straight, ref List<Type> Win, ref Type sorted)
        {
            if (pokerPlayer.Type >= -1)
            {
                for (int index = 0; index <= 12; index++)
                {
                    var fh = Straight.Where(o => o / 4 == index).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            RankHand(pokerPlayer, Win, 13, 3, out sorted);
                        }
                        else
                        {
                            RankHand(pokerPlayer, Win, fh[0] / 4, 3, out sorted);
                        }
                    }
                }
            }
        }

        public void TwoPair(
            IPokerPlayer pokerPlayer,
            int index,
            ref List<Type> Win,
            ref Type sorted,
            ref int[] reserve)
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
                                            sorted =
                                                Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index + 1] / 4 != 0 && reserve[index] / 4 != 0)
                                        {
                                            pokerPlayer.Type = 2;
                                            pokerPlayer.Power = (reserve[index] / 4) * 2 + (reserve[index + 1] / 4) * 2 +
                                                                pokerPlayer.Type * 100;
                                            Win.Add(new Type() { Power = pokerPlayer.Power, Current = 2 });
                                            sorted =
                                                Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
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
        //opraveni dotuk bez flusha

        public void PairTwoPair(
            IPokerPlayer pokerPlayer,
            int index,
            ref List<Type> Win,
            ref Type sorted,
            ref int[] reserve)
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

        public void PairFromHand(
            IPokerPlayer pokerPlayer,
            int index,
            ref List<Type> Win,
            ref Type sorted,
            ref int[] Reserve)
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
                                sorted =
                                    Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = (Reserve[index + 1] / 4) * 4 + Reserve[index] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted =
                                    Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                sorted =
                                    Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                pokerPlayer.Type = 1;
                                pokerPlayer.Power = (Reserve[tc] / 4) * 4 + Reserve[index + 1] / 4 + 100;
                                Win.Add(new Type() { Power = pokerPlayer.Power, Current = 1 });
                                sorted =
                                    Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }

        public void HighCard(
            IPokerPlayer pokerPlayer,
            int index,
            ref List<Type> Win,
            ref Type sorted,
            ref int[] reserve)
        {
            if (pokerPlayer.Type == -1)
            {
                if (reserve[index] / 4 > reserve[index + 1] / 4)
                {
                    RankHand(pokerPlayer, Win, reserve[index] / 4, -1, out sorted);
                }
                else
                {
                    RankHand(pokerPlayer, Win, reserve[index + 1] / 4, -1, out sorted);
                }

                if (reserve[index] / 4 == 0 || reserve[index + 1] / 4 == 0)
                {
                    RankHand(pokerPlayer, Win, reserve[index] / 4, -1, out sorted);
                }
            }
        }
    }
}