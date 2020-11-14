using System;
using System.IO;

namespace WordsearchACW
{
    class Program
    {
        #region Methods
        /*Summary of HandleError
         * Displays an error message and gives the user the option to restart the program or close it
         */
        static void HandleError()
        {
            Console.Clear();
            Console.WriteLine("ERROR - The file you have selected is not formatted correctly");
            Console.WriteLine();
            Console.WriteLine("Would you like to restart the program?");
            Console.WriteLine("Please enter yes or no");

            string response = Console.ReadLine();
            string responseUpper = response.ToUpper();
            if (responseUpper == "Y" || responseUpper == "YES")
            {
                Console.Clear();
                Main();
            }
            else if (responseUpper == "N" || responseUpper == "NO")
            {
                Environment.Exit(0);
            }
        }
        /*Summary of GetDataFromFile
         * Reads the file and stores the data in the appropriate struct
         */
        static void GetDataFromAFile(string selectedFile, out WordsearchData WordsearchData, out WordData[] Word)
        {
            using (FileStream file = new FileStream(selectedFile, FileMode.Open))
            {
                using (StreamReader fileReader = new StreamReader(file))
                {
                    WordsearchData.TempData = fileReader.ReadLine();
                    WordsearchData.TempDataSplit = WordsearchData.TempData.Split(',');
                    WordsearchData.WordsearchSize.Column = int.Parse(WordsearchData.TempDataSplit[0]);
                    WordsearchData.WordsearchSize.Row = int.Parse(WordsearchData.TempDataSplit[1]);
                    WordsearchData.NumberOfWords = int.Parse(WordsearchData.TempDataSplit[2]);
                    WordsearchData.Wordsearch = new string[WordsearchData.WordsearchSize.Row, WordsearchData.WordsearchSize.Column];
                    WordsearchData.WordsFound = 0;

                    if (!(WordsearchData.WordsearchSize.Column > 0 || WordsearchData.WordsearchSize.Row > 0 || WordsearchData.NumberOfWords > 0))
                    {
                        HandleError();
                    }

                    Word = new WordData[WordsearchData.NumberOfWords];

                    try
                    {
                        for (int line = 0; line < WordsearchData.NumberOfWords; line++)
                        {
                            WordsearchData.TempData = fileReader.ReadLine();
                            WordsearchData.TempDataSplit = WordsearchData.TempData.Split(',');

                            Word[line].Word = WordsearchData.TempDataSplit[0];
                            Word[line].WordColumn = int.Parse(WordsearchData.TempDataSplit[1]);
                            Word[line].WordRow = int.Parse(WordsearchData.TempDataSplit[2]);
                            Word[line].WordDirection = WordsearchData.TempDataSplit[3];
                            Word[line].WordFound = false;
                        }
                    }
                    catch
                    {
                        HandleError();
                    }


                }
            }
        }
        /*Summary of InitializeWordsearch
         * Combines the DisplayWordsearch & FillWordsearch method as well as puts the words from the file into the wordsearch as specified
         */
        static void InitializeWordsearch(WordsearchData WordsearchData, WordData[] Word, PlayerGuess Player)
        {
            FillWordsearch(WordsearchData.Wordsearch);

            for (int line = 0; line < WordsearchData.NumberOfWords; line++)
            {
                int wordIndex = 0;

                try
                {
                    int tempColumn = Word[line].WordColumn;
                    int tempRow = Word[line].WordRow;

                    if (Word[line].WordDirection == "right" || Word[line].WordDirection == "left")
                    {
                        for (int lettersAdded = 0; lettersAdded < Word[line].Word.Length; lettersAdded++)
                        {
                            WordsearchData.Wordsearch[tempRow, tempColumn] = Word[line].Word[wordIndex].ToString().PadLeft(2).PadRight(3);

                            if (Word[line].WordDirection == "right")
                            {
                                tempColumn++;
                            }
                            else if (Word[line].WordDirection == "left")
                            {
                                tempColumn--;
                            }
                            wordIndex++;
                        }
                    }
                    else if (Word[line].WordDirection == "up" || Word[line].WordDirection == "down")
                    {
                        for (int lettersAdded = 0; lettersAdded < Word[line].Word.Length; lettersAdded++)
                        {
                            WordsearchData.Wordsearch[tempRow, tempColumn] = Word[line].Word[wordIndex].ToString().PadLeft(2).PadRight(3);

                            if (Word[line].WordDirection == "up")
                            {
                                tempRow--;
                            }
                            else if (Word[line].WordDirection == "down")
                            {
                                tempRow++;
                            }
                            wordIndex++;
                        }
                    }
                    else if (Word[line].WordDirection == "leftup" || Word[line].WordDirection == "rightup")
                    {
                        for (int lettersAdded = 0; lettersAdded < Word[line].Word.Length; lettersAdded++)
                        {
                            WordsearchData.Wordsearch[tempRow, tempColumn] = Word[line].Word[wordIndex].ToString().PadLeft(2).PadRight(3);

                            if (Word[line].WordDirection == "leftup")
                            {
                                tempColumn--;
                            }
                            else if (Word[line].WordDirection == "rightup")
                            {
                                tempColumn++;
                            }

                            wordIndex++;
                            tempRow--;

                        }
                    }
                    else if (Word[line].WordDirection == "leftdown" || Word[line].WordDirection == "rightdown")
                    {
                        for (int lettersAdded = 0; lettersAdded < Word[line].Word.Length; lettersAdded++)
                        {
                            WordsearchData.Wordsearch[tempRow, tempColumn] = Word[line].Word[wordIndex].ToString().PadLeft(2).PadRight(3);

                            if (Word[line].WordDirection == "leftdown")
                            {
                                tempColumn--;
                            }
                            else if (true)
                            {
                                tempColumn++;
                            }

                            wordIndex++;
                            tempRow++;

                        }
                    }
                }
                catch
                {
                    HandleError();
                }
            }
            DisplayWordsearch(WordsearchData, Word, Player);

            Console.WriteLine("The hidden words are:");

            for (int line = 0; line < WordsearchData.NumberOfWords; line++)
            {
                if (Word[line].WordFound == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(Word[line].Word);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        /*Summary of DisplayWordsearch
         * Writes the wordsearch into the console
         */
        static void DisplayWordsearch(WordsearchData WordsearchData, WordData[] Word, PlayerGuess Player)
        {
            for (int Column = 0; Column < WordsearchData.Wordsearch.GetLength(1); Column++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(Column.ToString().PadLeft(3));
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            for (int Row = 0; Row < WordsearchData.Wordsearch.GetLength(0); Row++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(Row);
                Console.ForegroundColor = ConsoleColor.White;
                for (int Column = 0; Column < WordsearchData.Wordsearch.GetLength(1); Column++)
                {
                    Console.Write(WordsearchData.Wordsearch[Row, Column]);        
                }
                Console.WriteLine();
            }
        }
        /*Summary of FillWordsearch
         * Fills th wordsearch with random letters
         */
        static void FillWordsearch(string[,] pWordsearch)
        {
            string alphabet = ("abcdefghijklmnopqrstuvwxyz");
            string randomLetter = ("");
            Random randomNumber = new Random();

            for (int Row = 0; Row < pWordsearch.GetLength(0); Row++)
            {

                for (int Column = 0; Column < pWordsearch.GetLength(1); Column++)
                {
                    randomLetter = ("");
                    int randomIndex = randomNumber.Next(0, alphabet.Length);
                    randomLetter += alphabet[randomIndex];
                    pWordsearch[Row, Column] = randomLetter.ToString().PadLeft(2).PadRight(3);
                }
            }
        }
        /*Summary of GetANumberWithinRangeFromUser
         * Asks the user to enter a number within a range that is specified by the programmer
         */
        static int GetANumberWithinRangeFromUser(int pMin, int pMax)
        {
            int Input = -1;
            string response;
            do
            {
                do
                {
                    Console.WriteLine("Enter a number from " + pMin + " to " + pMax + (" inclusive"));
                    response = Console.ReadLine();
                    try
                    {
                        Input = int.Parse(response);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Please enter a number");
                    }
                } while (response == (""));
            } while (!(Input >= pMin && Input <= pMax));

            return Input;
        }
        /*Summary of GetPlayerGuess
         * Asks the user to input a start column, start row, end column and end row within the range of possible values as defined by the wordsearch file
         * Then gradient of these 2 set of coordinates is calculated and then check using an if statement to check if the coordinates form a straight line
         */
        static PlayerGuess GetPlayersGuess(PlayerGuess Player, int numberOfColumns, int numberOfRows)
        {
            do
            {
                Console.WriteLine();
                Console.WriteLine("Please enter the start column");
                Player.StartColumn = GetANumberWithinRangeFromUser(0, (numberOfColumns - 1));

                Console.WriteLine();
                Console.WriteLine("Please enter the start row");
                Player.StartRow = GetANumberWithinRangeFromUser(0, (numberOfRows - 1));

                Console.WriteLine();
                Console.WriteLine("Please enter the end column");
                Player.EndColumn = GetANumberWithinRangeFromUser(0, (numberOfColumns - 1));

                Console.WriteLine();
                Console.WriteLine("Please enter the end row");
                Player.EndRow = GetANumberWithinRangeFromUser(0, (numberOfRows - 1));

                Player.Gradient = (float)(Player.EndRow - Player.StartRow) / (Player.EndColumn - Player.StartColumn);

                if (float.IsInfinity(Player.Gradient))
                {
                    Player.Gradient = 0;
                }

                if (Player.Gradient == 0 || Player.Gradient == -1 || Player.Gradient == 1)
                {
                    break;
                }
                else if (!(Player.Gradient == 0 || Player.Gradient == -1 || Player.Gradient == 1))
                {
                    Console.WriteLine();
                    Console.WriteLine("The coordinates you input must form a staright line");
                    Console.WriteLine("Please enter a new guess");
                }
            } while (!(Player.Gradient == 0 || Player.Gradient == -1 || Player.Gradient == 1));
            return Player;
        }
        /* Summary of CheckPlayerGuessInWordsearch
         * this will check the players guess to see if there coordinates match with the coordinates of any of the words that need to be found
         */
        static void CheckPlayerGuessInWordsearch(ref WordsearchData WordsearchData, PlayerGuess Player, ref WordData[] Word)
        {
            for (int check = 0; check < WordsearchData.NumberOfWords; check++)
            {
                if (Word[check].WordFound != true)
                {
                    if (Player.Gradient == 0)
                    {
                        if (Word[check].WordDirection == "right")
                        {
                            if (Word[check].Word.Length - 1 == Player.EndColumn || (Player.StartColumn - (Word[check].Word.Length - 1)) == Player.EndColumn)
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                        else if (Word[check].WordDirection == "left")
                        {
                            if (Word[check].Word.Length == Player.EndColumn || (Player.StartColumn - (Word[check].Word.Length - 1)) == Player.EndColumn)
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                        else if (Word[check].WordDirection == "down")
                        {
                            if (Player.StartRow + (Word[check].Word.Length - 1) == Player.EndRow || Player.StartRow - (Word[check].Word.Length - 1) == Player.EndRow)
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                        else if (Word[check].WordDirection == "up")
                        {
                            if (Player.StartRow + (Word[check].Word.Length - 1) == Player.EndRow || Player.StartRow - (Word[check].Word.Length - 1) == Player.EndRow)
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                    }
                    else if (Player.Gradient == 1)
                    {
                        if (Word[check].WordDirection == "leftup")
                        {
                            if ((Word[check].WordColumn - (Word[check].Word.Length - 1) == Player.EndColumn) && (Word[check].WordRow - (Word[check].Word.Length - 1) == Player.EndRow) || (Word[check].WordColumn == Player.EndColumn) && (Word[check].WordRow == Player.EndRow))
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                        else if (Word[check].WordDirection == "rightdown")
                        {
                            if ((Player.StartColumn + (Word[check].Word.Length - 1) == Player.EndColumn) && (Player.StartRow + (Word[check].Word.Length - 1) == Player.EndRow) || (Player.StartColumn - (Word[check].Word.Length - 1) == Player.EndColumn) && (Player.StartRow - (Word[check].Word.Length - 1) == Player.EndRow))
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                    }
                    else if (Player.Gradient == -1)
                    {
                        if (Word[check].WordDirection == "leftdown")
                        {
                            if ((Player.StartColumn - (Word[check].Word.Length - 1) == Player.EndColumn && Player.StartRow + (Word[check].Word.Length - 1) == Player.EndRow) || (Player.StartColumn + (Word[check].Word.Length - 1) == Player.EndColumn && Player.StartRow - (Word[check].Word.Length - 1) == Player.EndRow))
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                        else if (Word[check].WordDirection == "rightup")
                        {
                            if ((Word[check].WordColumn + (Word[check].Word.Length - 1) == Player.EndColumn) && (Word[check].WordRow - (Word[check].Word.Length - 1) == Player.EndRow) || (Word[check].WordColumn == Player.EndColumn) && (Word[check].WordRow == Player.EndRow))
                            {
                                Console.Clear();
                                Console.WriteLine("Congratulations, you found " + Word[check].Word);
                                Console.WriteLine();
                                Word[check].WordFound = true;
                                WordsearchData.WordsFound++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Sorry you did not find any of the words displayed in the list");
                    }
                }
                else if ((Player.StartColumn == Word[check].WordColumn && Player.StartRow == Word[check].WordRow))
                {
                    Console.Clear();
                    Console.WriteLine("This word has already been found");
                    Console.WriteLine();
                    break;
                }
            }
            InitializeWordsearch(WordsearchData, Word, Player);
        }
        /* Summary of Winner
         * Displays congratulations message and asks the user if they would like to play again
         * */
        static void Winner()
        {
            Console.WriteLine();
            Console.WriteLine("Congratulations! You found all the words");

            Console.WriteLine();
            Console.WriteLine("Would you like to play again?");
            Console.WriteLine("Please enter yes or no");

            string response = Console.ReadLine();
            string responseUpper = response.ToUpper();
            if (responseUpper == "Y" || responseUpper == "YES")
            {
                Console.Clear();
                Main();
            }
            else if (responseUpper == "N" || responseUpper == "NO")
            {
                Environment.Exit(0);
            }
        }
        #endregion
        #region Structs
        public struct Coordinate
        {
            public int Column;
            public int Row;
        }
        public struct WordsearchData
        {
            public string[,] Wordsearch;
            public Coordinate WordsearchSize;
            public int NumberOfWords;
            public int WordsFound;
            public string TempData;
            public string[] TempDataSplit;
        }
        public struct WordData
        {
            public string Word;
            public int WordRow;
            public int WordColumn;
            public string WordDirection;
            public bool WordFound;
        }
        public struct PlayerGuess
        {
            public int StartColumn;
            public int EndColumn;
            public int StartRow;
            public int EndRow;
            public float Gradient;
        }
        #endregion

        static void Main()
        {
            Console.WriteLine("Wordsearch ACW");

            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Use the default wordsearch");
            Console.WriteLine("2. Load a wordsearch from file");
            Console.WriteLine();

            int optionSelection = 0;
            optionSelection = (GetANumberWithinRangeFromUser(1, 2));

            string selectedFile = ("");

            PlayerGuess Player = new PlayerGuess();
            WordsearchData WordsearchData = new WordsearchData();
            WordData[] WordData;

            if (optionSelection == 1)
            {
                selectedFile = ("File01.wrd");

                Console.Clear();

                GetDataFromAFile(selectedFile, out WordsearchData, out WordData);
                InitializeWordsearch(WordsearchData, WordData, Player);

                do
                {
                    Player = GetPlayersGuess(Player, WordsearchData.WordsearchSize.Column, WordsearchData.WordsearchSize.Row);
                    CheckPlayerGuessInWordsearch(ref WordsearchData, Player, ref WordData);
                } while (WordsearchData.WordsFound < WordsearchData.NumberOfWords);

                Winner();
            }
            else if (optionSelection == 2)
            {
                Console.Clear();

                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.wrd");
                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + files[i]);
                    Console.WriteLine();
                }

                int fileSelection = 0;
                fileSelection = (GetANumberWithinRangeFromUser(1, 7));

                selectedFile = ("file0" + fileSelection + ".wrd");

                Console.Clear();

                GetDataFromAFile(selectedFile, out WordsearchData, out WordData);
                InitializeWordsearch(WordsearchData, WordData, Player);

                do
                {
                    Player = GetPlayersGuess(Player, WordsearchData.WordsearchSize.Column, WordsearchData.WordsearchSize.Row);
                    CheckPlayerGuessInWordsearch(ref WordsearchData, Player, ref WordData);
                } while (WordsearchData.WordsFound < WordsearchData.NumberOfWords);

                Winner();
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

    }
}
