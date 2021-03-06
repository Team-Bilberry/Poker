﻿namespace Poker.PokerUtilities.CardsCombinationMethods
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Type = Poker.Type;

    public class RulesMethod
    {
        private readonly CheckHandType checkHand = new CheckHandType();

        // TODO: if someone knows how to extract the method and retain a simpler signature, be my guest :D
        public void TexasHoldEmRules(
            int card1,
            int card2,
            string currentText,
            IPokerPlayer pokerPlayer,
            ref Label playerStatus,
            ref PictureBox[] cardPicture,
            ref List<Type> Win,
            ref Type sorted,
            ref int[] reserve)
        {
            if (card1 == 0 && card2 == 1)
            {
            }

            if (!pokerPlayer.OutOfChips || card1 == 0 && card2 == 1 && playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables

                int[] Straight = new int[7];

                Straight[0] = reserve[card1];
                Straight[1] = reserve[card2];
                Straight[2] = reserve[12];
                Straight[3] = reserve[13];
                Straight[4] = reserve[14];
                Straight[5] = reserve[15];
                Straight[6] = reserve[16];

                int[] getClubes = Straight.Where(o => o % 4 == 0).ToArray();
                int[] getDimonds = Straight.Where(o => o % 4 == 1).ToArray();
                int[] getHearts = Straight.Where(o => o % 4 == 2).ToArray();
                int[] getSpades = Straight.Where(o => o % 4 == 3).ToArray();

                int[] clubes = getClubes.Select(o => o / 4).Distinct().ToArray();
                int[] diamonds = getDimonds.Select(o => o / 4).Distinct().ToArray();
                int[] hearts = getHearts.Select(o => o / 4).Distinct().ToArray();
                int[] spades = getSpades.Select(o => o / 4).Distinct().ToArray();

                #endregion Variables

                Array.Sort(Straight);
                Array.Sort(clubes);
                Array.Sort(diamonds);
                Array.Sort(hearts);
                Array.Sort(spades);

                for (int index = 0; index < 16; index++)
                {
                    if (reserve[index] == int.Parse(cardPicture[card1].Tag.ToString()) &&
                        reserve[index + 1] == int.Parse(cardPicture[card2].Tag.ToString()))
                    {
                        this.checkHand.PairFromHand(pokerPlayer, index, ref Win, ref sorted, ref reserve);

                        this.checkHand.PairTwoPair(pokerPlayer, index, ref Win, ref sorted, ref reserve);

                        this.checkHand.TwoPair(pokerPlayer, index, ref Win, ref sorted, ref reserve);

                        this.checkHand.ThreeOfAKind(pokerPlayer, Straight, ref Win, ref sorted);

                        this.checkHand.Straight(pokerPlayer, Straight, ref Win, ref sorted);

                        this.checkHand.Flush(pokerPlayer, Straight, ref Win, ref sorted);

                        this.checkHand.FullHouse(pokerPlayer, Straight, ref Win, ref sorted);

                        this.checkHand.FourOfAKind(pokerPlayer, Straight, ref Win, ref sorted);

                        this.checkHand.StraightFlush(pokerPlayer, clubes, diamonds, hearts, spades, ref Win, ref sorted);

                        this.checkHand.HighCard(pokerPlayer, index, ref Win, ref sorted, ref reserve);
                    }
                }
            }
        }
    }
}