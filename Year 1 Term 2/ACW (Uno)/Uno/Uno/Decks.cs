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
    abstract class Deck : IXmlSerializable
    {
        protected List<Card> mDeck;

        public List<Card> GetDeck
        {
            get
            {
                return mDeck;
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            mDeck.Clear();

            reader.MoveToContent();
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (!isEmpty)
            {
                var CardSerializer = new XmlSerializer(typeof(Card));
                reader.ReadStartElement();

                while (!reader.EOF && reader.Name == "Card")
                {
                    Card card = (Card)CardSerializer.Deserialize(reader);
                    mDeck.Add(card);
                }
            }
            reader.Close();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Deck");

            var CardSerializer = new XmlSerializer(typeof(Card));

            foreach (Card card in mDeck)
            {
                CardSerializer.Serialize(writer, card);
            }

            writer.WriteEndElement();
        }
    }
    class DrawPile : Deck
    {
        public DrawPile(GameTypes.mRuleSetOptions pRuleSet)
        {
           this.mDeck = new List<Card>();
            BuildDeck(pRuleSet);
        }

        /// <summary>BuildDeck
        /// <para>This method adds all of the cards into a draw pile in an unshuffled order</para>
        /// <para>At the end the method will then call the ShuffleDeck method to shuffle the cards</para>
        /// </summary>
        private void BuildDeck(GameTypes.mRuleSetOptions pRuleSet)
        {
            Card newCard;

            for (int Colour = 0; Colour < 4; Colour++) //Cycles colours
            {
                for (int Number = 0; Number < 10; Number++)
                {                   
                    switch (Colour)
                    {
                        default:
                        case 0:
                            newCard = new NumberedCard(Number.ToString(), Card.mColourOptions.Red);
                            break;
                        case 1:
                            newCard = new NumberedCard(Number.ToString(), Card.mColourOptions.Blue);
                            break;
                        case 2:
                            newCard = new NumberedCard(Number.ToString(), Card.mColourOptions.Green);
                            break;
                        case 3:
                            newCard = new NumberedCard(Number.ToString(), Card.mColourOptions.Yellow);
                            break;
                    }

                    if (Number == 0)
                    {
                        this.mDeck.Add(newCard);
                    }
                    else
                    {
                        this.mDeck.Add(newCard);
                        this.mDeck.Add(newCard);
                    }
                } //Cycles and creates numbered cards
                for (int ColouredSpecials = 0; ColouredSpecials < 3; ColouredSpecials++)
                {
                    if (ColouredSpecials == 0)
                    {
                        switch (Colour)
                        {
                            default:
                            case 0:
                                newCard = new SkipCard(Card.mColourOptions.Red);
                                break;
                            case 1:
                                newCard = new SkipCard(Card.mColourOptions.Blue);
                                break;
                            case 2:
                                newCard = new SkipCard(Card.mColourOptions.Green);
                                break;
                            case 3:
                                newCard = new SkipCard(Card.mColourOptions.Yellow);
                                break;
                        }
                        this.mDeck.Add(newCard);
                        this.mDeck.Add(newCard);
                    }
                    else if (ColouredSpecials == 1)
                    {
                        switch (Colour)
                        {
                            default:
                            case 0:
                                newCard = new ReverseCard(Card.mColourOptions.Red);
                                break;
                            case 1:
                                newCard = new ReverseCard(Card.mColourOptions.Blue);
                                break;
                            case 2:
                                newCard = new ReverseCard(Card.mColourOptions.Green);
                                break;
                            case 3:
                                newCard = new ReverseCard(Card.mColourOptions.Yellow);
                                break;
                        }
                        this.mDeck.Add(newCard);
                        this.mDeck.Add(newCard);
                    }
                    else if (ColouredSpecials == 2)
                    {
                        switch (Colour)
                        {
                            default:
                            case 0:
                                newCard = new Plus2Card(Card.mColourOptions.Red);
                                break;
                            case 1:
                                newCard = new Plus2Card(Card.mColourOptions.Blue);
                                break;
                            case 2:
                                newCard = new Plus2Card(Card.mColourOptions.Green);
                                break;
                            case 3:
                                newCard = new Plus2Card(Card.mColourOptions.Yellow);
                                break;
                        }
                        this.mDeck.Add(newCard);
                        this.mDeck.Add(newCard);
                    }
                } //Cycles and creates coloured special cards
            }
            for (int WildCards = 0; WildCards < 2; WildCards++)
            {
                if (WildCards == 0)
                {
                    for (int NumberOfCards = 0; NumberOfCards < 4; NumberOfCards++)
                    {
                        newCard = new WildCard();
                        this.mDeck.Add(newCard);
                    }
                }
                else
                {
                    for (int NumberOfCards = 0; NumberOfCards < 4; NumberOfCards++)
                    {
                        newCard = new Plus4Card();
                        this.mDeck.Add(newCard);
                    }
                }
            } //Cycles and creates wild cards

            if (pRuleSet == GameTypes.mRuleSetOptions.HouseRules)
            {
                newCard = new SwapHandsCard();
                this.mDeck.Add(newCard);
            } //Adds the swap hands card to the deck if the rule set in set to house rules

            ShuffleDeck();
        }

        ///<summary>ShuffleDeck
        ///<para>this is a method that suffles the draw pile deck from into a shuffled draw pile </para>
        ///<para>Fisher-Yates Shuffling Algorithm is used for this. I found an example of it at:</para>  
        ///<para>https://exceptionnotfound.net/understanding-the-fisher-yates-card-shuffling-algorithm/</para>
        ///</summary>
        private void ShuffleDeck()
        {                                                          
            Random CardPicker = new Random();
            List<Card> shuffled = new List<Card>();

            for (int numberOfUnshuffledCards = mDeck.Count; numberOfUnshuffledCards > 0; numberOfUnshuffledCards--)
            {
                int CardSelection = CardPicker.Next(numberOfUnshuffledCards);
                

                shuffled.Add(mDeck.ElementAt(CardSelection));
                mDeck.RemoveAt(CardSelection);
            }
            mDeck = shuffled;
        }

        /// <summary>FilpAndShuffleDiscardPile
        /// <para>This method will take the dicard pile and store and remove the top card</para>
        /// <para>Then whats left will be made it to the drawpile</para>
        /// <para>That will then be shuffled</para>
        /// </summary>
        /// <param name="pDiscardPile"></param>
        public void FilpAndShuffleDiscardPile(DiscardPile pDiscardPile)
        {
            List<Card> temp = new List<Card>();
            temp.Add(pDiscardPile.GetDeck.ElementAt(0));

            mDeck = pDiscardPile.GetDeck;
            mDeck.RemoveAt(0);
            ShuffleDeck();

            pDiscardPile.SetDiscardPile = temp;
        }
    }
    class DiscardPile : Deck
    {
         public DiscardPile(DrawPile pDrawPile)
        {
            mDeck = new List<Card>();
            BuildDiscardPile(pDrawPile);
        }

        /// <summary>BuildDiscarPile
        /// <para>This method will get the top card from the draw pile, list that is passed into the method through the parameter, and then add that to the discard pile and removes it from the draw pile</para>
        /// <para>As you can't start the uno game on a wild card code has been added so that if the top card of the draw pile that was taken and placed as the start card of the discard pile is a wild card then the method will be called again</para>
        /// </summary>
        /// <param name="pDrawPile"></param>
        private void BuildDiscardPile(DrawPile pDrawPile)
        {
            mDeck.Add(pDrawPile.GetDeck.ElementAt(0));
            pDrawPile.GetDeck.RemoveAt(0);

            if (GetDeck.ElementAt(0).GetColour == Card.mColourOptions.Black)
            {
                BuildDiscardPile(pDrawPile);
            }
        }

        /// <summary> DisplayTopCard
        /// <para>This method will look at the top card of the discard pile and display what that cards face and colour is to the current players turn.</para>
        /// </summary>
        /// <param name="pName"></param>
        public void DisplayTopCard(string pName)
        {
            Console.Write(pName + " it is your turn. The current card is a ");

            switch (mDeck.ElementAt(0).GetColour)
            {
                case Card.mColourOptions.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Card.mColourOptions.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Card.mColourOptions.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Card.mColourOptions.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Card.mColourOptions.Black:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine(mDeck.ElementAt(0).GetColour + ", " + mDeck.ElementAt(0).Face);

            Console.ResetColor();
        }

        internal List<Card> SetDiscardPile
        {
            set { mDeck = value; }
        }
    }
    class PlayerHand : Deck
    {
        public PlayerHand()
        {
            mDeck = new List<Card>();
        }

        /// <summary>BuildPlayerHand
        /// <para>This method gives a player one card to add to their hand</para>
        /// <para>The player is passed in using a for loop and the players array</para>
        /// </summary>
        /// <param name="pPlayerHand"></param>
        /// <param name="pDrawPile"></param>
        public void BuildPlayerHand(PlayerHand pPlayerHand, DrawPile pDrawPile)
        {
            pPlayerHand.mDeck.Add(pDrawPile.GetDeck.ElementAt(0));
            pDrawPile.GetDeck.RemoveAt(0);
        }

        private List<Card> SetPlayerHand
        {
            set { mDeck = value; }
        }
    }

}
