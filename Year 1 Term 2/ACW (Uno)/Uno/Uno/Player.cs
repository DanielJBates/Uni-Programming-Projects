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
    public class Player : IXmlSerializable
    {
        private string mName;
        private PlayerHand mPlayerHand;
        private int mPlayerScore = 0;

        public Player(string pName)
        {
            mName = pName;
            mPlayerHand = new PlayerHand();
        }

        /// <summary>PlayerTurn
        /// <para>This method first displays an ordered list of the players hand based on playable options</para>
        /// <para>The method then gets the players selection and places that card onto the discard pile</para>
        /// <para>If the chosen card is a card with an affect then that cards CardAffect method will be called</para>
        /// </summary>
        /// <param name="pGame"></param>
        public void PlayersTurn(GameTypes pGame)
        {
            pGame.GetDiscardPile.DisplayTopCard(mName);

            foreach (Player player in pGame.GetPlayers)
            {
                if (player.GetPlayerHand.GetDeck.Count == 1)
                {
                    Console.WriteLine(player.GetName + " has Uno");
                    Console.WriteLine();
                }
            } //Displays all the players that have uno

            PlayerHand OrderedOptions = new PlayerHand();
            int NumberSelection = 1;

            foreach (Card card in mPlayerHand.GetDeck)
            {
                if (card.GetColour == pGame.GetDiscardPile.GetDeck.ElementAt(0).GetColour || card.Face == pGame.GetDiscardPile.GetDeck.ElementAt(0).Face || card.GetColour == Card.mColourOptions.Black)
                {
                    OrderedOptions.GetDeck.Insert(0, card);
                }
                else
                {
                    OrderedOptions.GetDeck.Insert(OrderedOptions.GetDeck.Count, card);
                }
            } //This sorts a copy of the players hand based on viable options to play

            mPlayerHand = OrderedOptions; //This then copies that list into the player hand list

            foreach (Card card in mPlayerHand.GetDeck)
            {

                if (card.GetColour == pGame.GetDiscardPile.GetDeck.ElementAt(0).GetColour || card.Face == pGame.GetDiscardPile.GetDeck.ElementAt(0).Face || card.GetColour == Card.mColourOptions.Black)
                {
                    Console.Write(NumberSelection + "| ");

                    NumberSelection++;
                }
                else
                {
                    Console.Write("-| ");
                }

                switch (card.GetColour)
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

                Console.WriteLine(card.GetColour + ", " + card.Face);

                Console.ResetColor();
            } //Finally this prints this to the console

            Console.WriteLine(NumberSelection + "| " + "Draw");

            Console.WriteLine();
            Console.WriteLine("Which card would you like to play? (Or to save & exit type save)");

            int selection;

            string input = Console.ReadLine();

            if (input.ToLower() == "save")
            {
                Console.Clear();
                Console.WriteLine("1| Save File 1");
                Console.WriteLine("2| Save File 2");
                Console.WriteLine("3| Save File 3");

                int.TryParse(Console.ReadLine(), out selection);

                while (selection < 1 || selection > 3)
                {
                    Console.WriteLine("Please enter a valid selection");
                }

                switch (selection)
                {
                    default:

                    case 1:
                        pGame.SaveToXml("SaveFile1.xml");
                        break;
                    case 2:
                        pGame.SaveToXml("SaveFile2.xml");
                        break;
                    case 3:
                        pGame.SaveToXml("SaveFile1.xml");
                        break;
                }

                Environment.Exit(0);
            }
            else
            {
                int.TryParse(input, out selection);

                while (selection < 1 || selection > NumberSelection)
                {
                    Console.WriteLine("Please enter a valid option");
                    int.TryParse(Console.ReadLine(), out selection);
                }

                Console.Clear();

                if (selection != NumberSelection)
                {
                    if (mPlayerHand.GetDeck.ElementAt(selection - 1) is SpecialCard)
                    {
                        (mPlayerHand.GetDeck.ElementAt(selection - 1) as SpecialCard).CardAffect(pGame);
                    }

                    if (mPlayerHand.GetDeck.ElementAt(selection - 1) is SwapHandsCard == false)
                    {
                        pGame.GetDiscardPile.GetDeck.Insert(0, mPlayerHand.GetDeck[selection - 1]);
                        mPlayerHand.GetDeck.RemoveAt(selection - 1);
                    }
                }
                else
                {
                    if (pGame.GetDrawPile.GetDeck.Count < 1)
                    {
                        pGame.GetDrawPile.FilpAndShuffleDiscardPile(pGame.GetDiscardPile);
                    }

                    mPlayerHand.GetDeck.Add(pGame.GetDrawPile.GetDeck.ElementAt(0));
                    pGame.GetDrawPile.GetDeck.RemoveAt(0);
                }
                Console.Clear();
            }

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
                mName = reader.ReadElementContentAsString("Player's Name", "");
                mPlayerScore = reader.ReadElementContentAsInt("Player's Score", "");
                mPlayerHand.ReadXml(reader);
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Player's Name", mName);
            writer.WriteElementString("Player's Score", mPlayerScore.ToString());

            mPlayerHand.WriteXml(writer);         
        }

        #region Getters&Setters
        internal PlayerHand SetPlayerHand
        {
            set { mPlayerHand = value; }
        }
        internal PlayerHand GetPlayerHand
        {
            get
            {
                return mPlayerHand;
            }
        }
        public string GetName
        {
            get { return mName; }
        }
        public int Score
        {
            set { mPlayerScore = value; }
            get { return mPlayerScore; }
        }
        #endregion
    }
}
