using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;

namespace Uno
{
    abstract public class GameTypes : IXmlSerializable
    {
        public enum mDirectionOptions { Clockwise, CounterClockwise };
        public enum mRuleSetOptions { Classic, HouseRules};
        private mDirectionOptions mDirection = mDirectionOptions.Clockwise;
        private mRuleSetOptions mRuleSet; 
        private DrawPile mDrawPile;
        private DiscardPile mDiscardPile;
        private Player[] mPlayers;
        private int mCurrentPlayer = 0;

        public GameTypes()
        {
            mRuleSet = RetrieveRuleSet();   

            mPlayers = new Player[RetrieveNumOfPlayers()];
            PopulatePlayers();

            mDrawPile = new DrawPile(mRuleSet);

            DealCards();

            mDiscardPile = new DiscardPile(mDrawPile);
        }      

        /// <summary>RetrieveRuleSet 
        /// <para>This method gets from the user which rule set they want to play with</para>
        /// </summary>
        /// <returns></returns>
        private mRuleSetOptions RetrieveRuleSet()
        {
            int optionSelection;

            Console.WriteLine("Which rule set would you like to play?");
            Console.WriteLine("1 - Classic");
            Console.WriteLine("2 - House Rules");

            do
            {
                int.TryParse(Console.ReadLine(), out optionSelection);

                if (optionSelection < 1 || optionSelection > 2)
                {
                    Console.WriteLine("Please enter a valid selection");
                }
            } while (optionSelection < 1 || optionSelection > 2);

            switch(optionSelection)
            {
                default:
     
                case 1:
                    Console.Clear();
                    return mRuleSetOptions.Classic;
                case 2:
                    Console.Clear();
                    return mRuleSetOptions.HouseRules;
            }

            
        }

        /// <summary>PopulatePlayers
        /// <para>This method will get each players name and create a new instance of the player class for that player then add them to the array of players</para>
        /// </summary>
        private void PopulatePlayers()
        {
            for (int PlayersInGame = 0; PlayersInGame < mPlayers.Length; PlayersInGame++)
            {
                Console.WriteLine("Hello player " + (PlayersInGame + 1) + " whats your name?");

                Player newPlayer = new Player( Console.ReadLine());
                mPlayers[PlayersInGame] = newPlayer;

                Console.Clear();
            }
        }

        /// <summary>RetrieveNumOfPlayers
        /// <para>This method gets how many player are in the game from a user input</para>
        /// </summary>
        private int RetrieveNumOfPlayers()
        {
            int numberOfPlayers;

            do
            {
                Console.WriteLine("How many players are playing? (2 - 10)");
                int.TryParse(Console.ReadLine(), out numberOfPlayers);

                Console.Clear();

            } while (numberOfPlayers < 2 || numberOfPlayers > 10);

            return numberOfPlayers;
        }

        /// <summary>DealCards
        /// <para>This method cycles through the players array giving each player 1 card at a time until everyone has 7 cards</para>
        /// </summary>
        private void DealCards()
        {
            for (int NumberOfCards = 0; NumberOfCards < 7; NumberOfCards++)
            {
                for (int PlayerNumber = 0; PlayerNumber < mPlayers.Length; PlayerNumber++)
                {
                    mPlayers[PlayerNumber].GetPlayerHand.BuildPlayerHand(mPlayers[PlayerNumber].GetPlayerHand, mDrawPile);
                }
            }
        }

        /// <summary>NewGame
        /// <para>This creates a new game</para>
        /// </summary>
        public void NewGame()
        {
            mDrawPile = new DrawPile(mRuleSet);
            mDiscardPile = new DiscardPile(mDrawPile);

            foreach (Player player in mPlayers)
            {
                player.GetPlayerHand.GetDeck.Clear();
            }

            DealCards();
        }

        /// <summary>CheckForSingleWinner
        /// <para>This method cycles through each player and checks how many cards they have in their hand</para>
        /// <para>If a player has 0 cards left they have won and the method will return true</para>
        /// </summary>
        /// <param name="pPlayers"></param>
        public bool CheckForSingleWinner(Player[] pPlayers)
        {
            for (int players = 0; players < pPlayers.Length; players++)
            {
                int numberOfCards = pPlayers[players].GetPlayerHand.GetDeck.Count;

                if (numberOfCards == 0)
                {
                    mCurrentPlayer = players;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Abstract CheckforWinner
        /// <para>This ensures all match types have a way to check if someone has won the game</para>
        /// </summary>
        /// <param name="pPlayers"></param>
        /// <returns></returns>
        abstract public bool CheckforWinner(Player[] pPlayers);

        abstract public void SaveToXml(string pFileName);
        abstract public GameTypes LoadXml(string pFileName);

        public XmlSchema GetSchema()
        {
            return null;
        }
        public abstract void ReadXml(XmlReader reader);
        public abstract void WriteXml(XmlWriter writer);

        #region Getters & Setters
        internal DrawPile GetDrawPile
        {
            get { return mDrawPile; }
        }
        internal DiscardPile GetDiscardPile
        {
            get { return mDiscardPile; }
        }
        internal Player[] GetPlayers
        {
            get { return mPlayers; }
        }
        public mRuleSetOptions GetRuleSet
        {
            get { return mRuleSet; }
        }
        protected mRuleSetOptions SetRuleSet
        {
            set { mRuleSet = value; }
        }
        public mDirectionOptions Direction
        {
            set { mDirection = value; }
            get { return mDirection; }
        }
        public int CurrentPlayer
        {
            set { mCurrentPlayer = value; }
            get { return mCurrentPlayer; }
        }
        #endregion      
    }
    public class SingleGame : GameTypes
    {
        /// <summary>SingleGame CheckforWinner
        /// <para>This method will call the CheckForSingleWinner method</para>
        /// </summary>
        /// <param name="pPlayers"></param>
        /// <returns></returns>
        public override bool CheckforWinner(Player[] pPlayers)
        {
            return CheckForSingleWinner(pPlayers);
        }

        public override void SaveToXml(string pFileName)
        {
            XmlSerializer GameSerializer = new XmlSerializer(typeof(SingleGame));
            using (TextWriter writer = new StreamWriter(pFileName))
            {
                GameSerializer.Serialize(writer, this);
            }
        }
        public override GameTypes LoadXml(string pFileName)
        {
            GameTypes LoadedGame;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SingleGame));
            try
            {
                using (StreamReader reader = new StreamReader(pFileName))
                {
                    LoadedGame = (SingleGame)xmlSerializer.Deserialize(reader);
                }
            }
            catch
            {
                LoadedGame = new SingleGame();
            }
            return LoadedGame;
        }


        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Direction", Direction.ToString());
            writer.WriteElementString("Rule Set", GetRuleSet.ToString());

            GetDrawPile.WriteXml(writer);
            GetDiscardPile.WriteXml(writer);

            writer.WriteElementString("Number Of Players", GetPlayers.Length.ToString());

            for (int player = 0; player < GetPlayers.Length; player++)
            {
                GetPlayers[player].WriteXml(writer);
            }

            writer.WriteElementString("Current Players Turn", CurrentPlayer.ToString());
        }
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmpty)
            {
                Direction = (mDirectionOptions)Enum.Parse(typeof(mDirectionOptions), reader.ReadElementContentAsString("Direction", ""));
                SetRuleSet = (mRuleSetOptions)Enum.Parse(typeof(mRuleSetOptions), reader.ReadElementContentAsString("Rule Set", ""));
                GetDrawPile.ReadXml(reader);
                GetDiscardPile.ReadXml(reader);

                int numberOfPlayers = reader.ReadElementContentAsInt("Number Of Players", "");

                for (int player = 0; player < numberOfPlayers; player++)
                {
                    GetPlayers[player].ReadXml(reader);
                }

                CurrentPlayer = reader.ReadElementContentAsInt("Current Players Turn", "");
                reader.ReadEndElement();
            }

        }
    }
    public class Match : GameTypes
    {
        private int mScoreWin;

        public Match()
        {
            SetScoreWin();           
        } 

        /// <summary>SetScoreWin
        /// <para>This method get the score reqiured to win for the match from a user input</para>
        /// </summary>
        private void SetScoreWin()
        {
            do
            {
                Console.WriteLine("What would you like the score reqiured to win be?");
                Console.WriteLine("A recommended score would be 500");

                while (!int.TryParse(Console.ReadLine(), out mScoreWin))
                {
                    Console.WriteLine("Please enter a valid number");
                }

                if (mScoreWin < 0)
                {
                    Console.Clear();

                    Console.WriteLine("The score required to win must be greater than 0");
                    Console.WriteLine("A recommended score would be 500");

                    while (!int.TryParse(Console.ReadLine(), out mScoreWin) || mScoreWin < 0)
                    {
                        Console.WriteLine("Please enter a valid number");
                    }
                }
            } while (mScoreWin < 0);

            Console.Clear();
        }

        /// <summary>Match CheckforWinner
        /// <para>This method checks each players score to see if they have reach the score required to win</para>
        /// <para>If someone has then the method will return true</para>
        /// </summary>
        /// <param name="pPlayers"></param>
        /// <returns></returns>
        public override bool CheckforWinner(Player[] pPlayers)
        {
            for (int players = 0; players < pPlayers.Length; players++)
            {
                int Score = pPlayers[players].Score;

                if (Score >= mScoreWin)
                {
                    CurrentPlayer = players;
                    return true;
                }
            }
            return false;
        }



        public override void SaveToXml(string pFileName)
        {
            XmlSerializer GameSerializer = new XmlSerializer(typeof(Match));
            using (TextWriter writer = new StreamWriter(pFileName))
            {
                GameSerializer.Serialize(writer, this);
            }
        }
        public override GameTypes LoadXml(string pFileName)
        {
            GameTypes LoadedGame;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Match));
            try
            {
                using (StreamReader reader = new StreamReader(pFileName))
                {
                    LoadedGame = (Match)xmlSerializer.Deserialize(reader);
                }
            }
            catch
            {
                LoadedGame = new Match();
            }
            return LoadedGame;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Direction", Direction.ToString());
            writer.WriteElementString("Rule Set", GetRuleSet.ToString());

            GetDrawPile.WriteXml(writer);
            GetDiscardPile.WriteXml(writer);

            writer.WriteElementString("Number Of Players", GetPlayers.Length.ToString());

            for (int player = 0; player < GetPlayers.Length; player++)
            {
                GetPlayers[player].WriteXml(writer);
            }

            writer.WriteElementString("Current Players Turn", CurrentPlayer.ToString());
            writer.WriteElementString("Winning Score", mScoreWin.ToString());
        }
        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmpty)
            {
                Direction = (mDirectionOptions)Enum.Parse(typeof(mDirectionOptions), reader.ReadElementContentAsString("Direction", ""));
                SetRuleSet = (mRuleSetOptions)Enum.Parse(typeof(mRuleSetOptions), reader.ReadElementContentAsString("Rule Set", ""));
                GetDrawPile.ReadXml(reader);
                GetDiscardPile.ReadXml(reader);

                int numberOfPlayers = reader.ReadElementContentAsInt("Number Of Players", "");

                for (int player = 0; player < numberOfPlayers; player++)
                {
                    GetPlayers[player].ReadXml(reader);
                }

                CurrentPlayer = reader.ReadElementContentAsInt("Current Players Turn", "");
                mScoreWin = reader.ReadElementContentAsInt("Winning Score", "");
                reader.ReadEndElement();
            }
        }
    }

}
