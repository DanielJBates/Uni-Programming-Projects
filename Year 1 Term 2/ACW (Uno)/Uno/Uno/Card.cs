using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Uno
{
    public abstract class Card : IXmlSerializable
    {
        public enum mColourOptions { Red, Blue, Green, Yellow, Black };
        protected mColourOptions mColour;
        private string mFace;
        private int mScoreValue;

        #region Getters&Setters
        public string Face
        {
            set { mFace = value; }
            get { return mFace; }
        }
        public mColourOptions GetColour
        {
            get { return mColour; }
        }
        public int ScoreValue
        {
            set { mScoreValue = value; }
            get { return mScoreValue; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmpty)
            {
                mFace = reader.ReadElementContentAsString("Face", "");
                mColour = (mColourOptions)Enum.Parse(typeof(mColourOptions), reader.ReadElementContentAsString("Colour", ""));
                mScoreValue = reader.ReadElementContentAsInt("Score Value", "");
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Card");
            writer.WriteElementString("Face", mFace);
            writer.WriteElementString("Colour", mColour.ToString());
            writer.WriteElementString("Score Value", mScoreValue.ToString());
            writer.WriteEndElement();
        }
        #endregion
    }
    public class NumberedCard : Card
    {
        public NumberedCard(string pNumber, mColourOptions pColour)
        {
            Face = pNumber;
            ScoreValue = int.Parse(pNumber);
            mColour = pColour;
        }
    }
    public abstract class SpecialCard : Card
    {
        /// <summary>Abstract CardAffect
        /// <para>an abstarct class to ensure the all the special cards have an overidden method that will serve as that cards affect</para>
        /// <para>e.g. Plus2Card will have the affect of adding 2 cards to the next players hand</para>
        /// </summary>
        /// <param name="pGame"></param>
        abstract public void CardAffect(GameTypes pGame); 
    }
    public class SkipCard : SpecialCard
    {
        public SkipCard(mColourOptions pColour)
        {
            Face = "Skip";
            ScoreValue = 20;
            mColour = pColour;
        }

        public SkipCard()
        { }

        /// <summary>Skip CardAffect
        /// <para>This method will increase or decrease the mCurrentPlayer variable depending of the direction of play</para>
        /// </summary>
        /// <param name="pGame"></param>
        public override void CardAffect(GameTypes pGame)
        {
            if (pGame.Direction == GameTypes.mDirectionOptions.Clockwise)
            {
                if (pGame.CurrentPlayer == pGame.GetPlayers.Length - 1)
                {
                    pGame.CurrentPlayer = 0;
                }
                else
                {
                    pGame.CurrentPlayer++;
                }

            }
            else
            {

                if (pGame.CurrentPlayer - 1 == 0)
                {
                    pGame.CurrentPlayer = pGame.GetPlayers.Length;
                }
                else
                {
                    pGame.CurrentPlayer--;
                }

            }
        }
    }
    public class Plus2Card : SpecialCard
    {
        public Plus2Card(mColourOptions pColour)
        {
            Face = "Plus 2";
            ScoreValue = 20;
            mColour = pColour;
        }

        public Plus2Card()
        { }

        /// <summary>Plus 2 CardAffect
        /// <para>This method first check to make sure that the draw pile contains 2 or more cards will give the next player 2 more cards</para>
        /// <para>Then the method will add 1 or take away 1 based direction of play which will skip that players turn</para>
        /// </summary>
        /// <param name="pGame"></param>
        public override void CardAffect(GameTypes pGame)
        {
            if (pGame.GetDrawPile.GetDeck.Count < 2)
                {
                    pGame.GetDrawPile.FilpAndShuffleDiscardPile(pGame.GetDiscardPile);
                }

          /*  if (pGame.GetRuleSet == GameTypes.mRuleSetOptions.Classic)
            {*/

                if (pGame.Direction == GameTypes.mDirectionOptions.Clockwise)
                {
                    for (int loopCount = 0; loopCount < 2; loopCount++)
                    {
                        if (pGame.CurrentPlayer == pGame.GetPlayers.Length - 1)
                        {
                            pGame.GetPlayers[0].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                            pGame.GetDrawPile.GetDeck.RemoveAt(0);
                        }
                        else
                        {
                            pGame.GetPlayers[pGame.CurrentPlayer + 1].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                            pGame.GetDrawPile.GetDeck.RemoveAt(0);
                        }
                    }

                    if (pGame.CurrentPlayer == pGame.GetPlayers.Length - 1)
                    {
                        pGame.CurrentPlayer = 0;
                    }
                    else
                    {
                        pGame.CurrentPlayer++;
                    }
                }
                else
                {
                    for (int loopCount = 0; loopCount < 2; loopCount++)
                    {
                        if (pGame.CurrentPlayer == 0)
                        {
                            pGame.GetPlayers[pGame.GetPlayers.Length].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                            pGame.GetDrawPile.GetDeck.RemoveAt(0);
                        }
                        else
                        {
                            pGame.GetPlayers[pGame.CurrentPlayer - 1].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                            pGame.GetDrawPile.GetDeck.RemoveAt(0);
                        }
                    }

                    if (pGame.CurrentPlayer - 1 == 0)
                    {
                        pGame.CurrentPlayer = pGame.GetPlayers.Length;
                    }
                    else
                    {
                        pGame.CurrentPlayer--;
                    }
                }
            //} //Classic rules affect
           /* else
            {
                if (pGame.Direction == GameTypes.mDirectionOptions.Clockwise)
                {
                    if (pGame.CurrentPlayer == pGame.GetPlayers.Length - 1)
                    {
                        if (!(pGame.GetPlayers[0].GetPlayerHand.GetDeck.Exists(item => item is Plus2Card)))
                        {
                            for (int loopCount = 0; loopCount < 2; loopCount++)
                            {
                                pGame.GetPlayers[0].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                                pGame.GetDrawPile.GetDeck.RemoveAt(0);
                            }
                            pGame.CurrentPlayer++;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (!(pGame.GetPlayers[pGame.CurrentPlayer + 1].GetPlayerHand.GetDeck.Exists(item => item is Plus2Card)))
                        {
                            for (int loopCount = 0; loopCount < 2; loopCount++)
                            {
                                pGame.GetPlayers[pGame.CurrentPlayer + 1].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                                pGame.GetDrawPile.GetDeck.RemoveAt(0);
                            }
                            if (pGame.CurrentPlayer == pGame.GetPlayers.Length - 1)
                            {
                                pGame.CurrentPlayer = 0;
                            }
                            else
                            {
                                pGame.CurrentPlayer++;
                            }
                        }
                        else
                        {
                            
                        }
                    }
                }
                else
                {
                    if (pGame.CurrentPlayer == 0)
                    {
                        if (!(pGame.GetPlayers[pGame.GetPlayers.Length - 1].GetPlayerHand.GetDeck.Exists(item => item is Plus2Card)))
                        {
                            for (int loopCount = 0; loopCount < 2; loopCount++)
                            {
                                pGame.GetPlayers[pGame.GetPlayers.Length - 1].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                                pGame.GetDrawPile.GetDeck.RemoveAt(0);
                            }
                            pGame.CurrentPlayer = pGame.GetPlayers.Length - 1;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (!(pGame.GetPlayers[pGame.CurrentPlayer - 1].GetPlayerHand.GetDeck.Exists(item => item is Plus2Card)))
                        {
                            for (int loopCount = 0; loopCount < 2; loopCount++)
                            {
                                pGame.GetPlayers[pGame.CurrentPlayer - 1].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                                pGame.GetDrawPile.GetDeck.RemoveAt(0);
                            }
                        }
                        if (pGame.CurrentPlayer - 1 == 0)
                        {
                            pGame.CurrentPlayer = pGame.GetPlayers.Length;
                        }
                        else
                        {
                            pGame.CurrentPlayer--;
                        }
                    }
                }
            } //House rules affect */
        }

    }
    public class ReverseCard : SpecialCard
    {
        public ReverseCard(mColourOptions pColour)
        {
            Face = "Reverse";
            ScoreValue = 20;
            mColour = pColour;
        }

        public ReverseCard()
        { }

        /// <summary>Reverse CardAffect
        /// <para>This method changes the direction of play to the opposite of what it already is</para>
        /// </summary>
        /// <param name="pGame"></param>
        public override void CardAffect(GameTypes pGame)
        {
            if (pGame.Direction == GameTypes.mDirectionOptions.Clockwise)
            {
                pGame.Direction = GameTypes.mDirectionOptions.CounterClockwise;
                pGame.CurrentPlayer--;
            }
            else
            {
                pGame.Direction = GameTypes.mDirectionOptions.Clockwise;
                pGame.CurrentPlayer++;
            }
        }
    }   
    public class WildCard : SpecialCard
    {
        public WildCard()
        {
            Face = "Wild Card";
            ScoreValue = 50;
            mColour = mColourOptions.Black;
        }

        /// <summary>WildCard CardAffect
        /// <para>This method calls the ChangeColour method</para>
        /// </summary>
        /// <param name="pGame"></param>
        public override void CardAffect(GameTypes pGame)
        {
            ChangeColour();
        }

        /// <summary>ChangeColour
        /// <para>This method will change the colour of card based on the players selection</para>
        /// </summary>
        public void ChangeColour()
        {
            int choice;
            bool repeat = false;
            do
            {
                Console.WriteLine("What colour would you like to change it to?");
                Console.WriteLine("1 - Red");
                Console.WriteLine("2 - Blue");
                Console.WriteLine("3 - Green");
                Console.WriteLine("4 - Yellow");

                do
                {
                    choice = int.Parse(Console.ReadLine());

                    if (choice < 1 || choice > 4)
                    {
                        Console.WriteLine("Please enter a valid selection");
                    }
                } while (choice < 1 || choice > 4);

                switch (choice)
                {
                    case 1:
                        mColour = mColourOptions.Red;
                        break;
                    case 2:
                        mColour = mColourOptions.Blue;
                        break;
                    case 3:
                        mColour = mColourOptions.Green;
                        break;
                    case 4:
                        mColour = mColourOptions.Yellow;
                        break;
                    default:
                        repeat = true;
                        Console.WriteLine("Please enter a valid choice");
                        continue;
                }
            } while (repeat);
        }
    }
    public class Plus4Card : WildCard
    {
        public Plus4Card()
        {
            Face = "Plus 4";
            ScoreValue = 50;
            mColour = mColourOptions.Black;
        }

        /// <summary>Plus 4 CardAffect
        /// <para>This method will first call the ChangeColour method</para>
        /// <para>Then will check to make sure that the draw pile has at least 4 cards in it</para>
        /// <para>Then it will give the next player 4 more cards</para>
        /// <para>Then the method will add 1 or take away 1 based direction of play which will skip that players turn</para>
        /// </summary>
        /// <param name="pGame"></param>
        public override void CardAffect(GameTypes pGame)
        {
            ChangeColour();

            if (pGame.GetDrawPile.GetDeck.Count < 4)
            {
                pGame.GetDrawPile.FilpAndShuffleDiscardPile(pGame.GetDiscardPile);
            }

            if (pGame.Direction == GameTypes.mDirectionOptions.Clockwise)
            {
                for (int loopCount = 0; loopCount < 4; loopCount++)
                {
                    if (pGame.CurrentPlayer == pGame.GetPlayers.Length - 1)
                    {
                        pGame.GetPlayers[0].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                        pGame.GetDrawPile.GetDeck.RemoveAt(0);
                    }
                    else
                    {
                        pGame.GetPlayers[pGame.CurrentPlayer + 1].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                        pGame.GetDrawPile.GetDeck.RemoveAt(0);
                    }
                }

                if (pGame.CurrentPlayer == pGame.GetPlayers.Length - 1)
                {
                    pGame.CurrentPlayer = 0;
                }
                else
                {
                    pGame.CurrentPlayer++;
                }
            }
            else
            {
                for (int loopCount = 0; loopCount < 2; loopCount++)
                {
                    if (pGame.CurrentPlayer <= 0)
                    {
                        pGame.GetPlayers[pGame.GetPlayers.Length].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                        pGame.GetDrawPile.GetDeck.RemoveAt(0);
                    }
                    else
                    {
                        pGame.GetPlayers[pGame.CurrentPlayer].GetPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                        pGame.GetDrawPile.GetDeck.RemoveAt(0);
                    }
                }

                if (pGame.CurrentPlayer - 1 == 0)
                {
                    pGame.CurrentPlayer = pGame.GetPlayers.Length;
                }
                else
                {
                    pGame.CurrentPlayer--;
                }
            }
        }
    }
    public class SwapHandsCard : WildCard
    {
        public SwapHandsCard()
        {
            Face = "Swap Hands";
            ScoreValue = 50;
            mColour = mColourOptions.Black;
        }

        /// <summary> SwapHands CardAffect
        /// <para>This method will swap the players hand with another player of their choice</para> 
        /// </summary>
        public override void CardAffect(GameTypes pGame)
        {
            int NumberSelection = 1;
            int Selection;

            Console.WriteLine("Which player would you like to swap hands with?");

            foreach (Player player in pGame.GetPlayers)
            {
                if (player == pGame.GetPlayers[pGame.CurrentPlayer])
                {
                    continue;
                }
                Console.WriteLine(NumberSelection + "| " + player.GetName);

                NumberSelection++;
            }

            do
            {
                int.TryParse(Console.ReadLine(), out Selection);

                if (Selection < 1 || Selection > NumberSelection)
                {
                    Console.WriteLine("Please enter a valid selection");
                }
            } while (Selection < 1 || Selection > NumberSelection);

            if (pGame.GetPlayers[Selection - 1] == pGame.GetPlayers[pGame.CurrentPlayer] || pGame.GetPlayers[Selection] == pGame.GetPlayers[pGame.CurrentPlayer])
            {
                Selection++;

                if (Selection >= pGame.GetPlayers.Length)
                {
                    Selection = 0;
                }
            }

            foreach (Card card in pGame.GetPlayers[pGame.CurrentPlayer].GetPlayerHand.GetDeck)
            {
                if(card is SwapHandsCard)
                {
                    pGame.GetPlayers[pGame.CurrentPlayer].GetPlayerHand.GetDeck.Remove(card);
                    break;
                }
            }

            PlayerHand temp = pGame.GetPlayers[pGame.CurrentPlayer].GetPlayerHand;
            pGame.GetPlayers[pGame.CurrentPlayer].SetPlayerHand = pGame.GetPlayers[Selection - 1].GetPlayerHand;

            pGame.GetPlayers[Selection].SetPlayerHand = temp;
        }
    }
}
