using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Uno
{
    class Program
    {
        static void Main(string[] args)
        {
            #region SetUp
            GameTypes game;
            int selcetion;

            Console.WriteLine("Would you like to play a single game or a match?");
            Console.WriteLine("1 - Single game");
            Console.WriteLine("2 - Match");
            int.TryParse(Console.ReadLine(), out selcetion);

            Console.Clear();

            if (selcetion == 1)
            {
                 game = new SingleGame();
            }
            else
            {
                game = new Match();
            }
            #endregion

            if (game is SingleGame)
            {
                game.CurrentPlayer = 0;

                do
                {
                    if (game.GetDrawPile.GetDeck.Count == 0)
                    {
                        game.GetDrawPile.FilpAndShuffleDiscardPile(game.GetDiscardPile);
                    }

                    if (game.Direction == GameTypes.mDirectionOptions.Clockwise)
                    {
                        if (game.CurrentPlayer >= game.GetPlayers.Length)
                        {
                            game.CurrentPlayer = 0;
                        }

                        game.GetPlayers[game.CurrentPlayer].PlayersTurn(game);

                        if (game.CheckforWinner(game.GetPlayers))
                        {
                            break;
                        }

                        game.CurrentPlayer++;
                    }
                    else
                    {
                        if (game.CurrentPlayer <= 0)
                        {
                            game.CurrentPlayer = game.GetPlayers.Length;
                        }

                        game.GetPlayers[game.CurrentPlayer - 1].PlayersTurn(game);

                        if (game.CheckforWinner(game.GetPlayers))
                        {
                            for (int Losers = game.GetPlayers.Length; Losers < 0; Losers--)
                            {
                                if (Losers == game.CurrentPlayer)
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (Card card in game.GetPlayers[Losers].GetPlayerHand.GetDeck)
                                    {
                                        game.GetPlayers[game.CurrentPlayer].Score += card.ScoreValue;
                                    }
                                }
                            }

                            break;
                        }

                        game.CurrentPlayer--;
                    }
                } while (game.CheckforWinner(game.GetPlayers) == false);

                Console.WriteLine("Congratulations " + game.GetPlayers[game.CurrentPlayer].GetName + " you have won");
            }
            else
            {
                do
                {
                    game.CurrentPlayer = 0;
                    game.NewGame(); 

                    do
                    {
                        if (game.GetDrawPile.GetDeck.Count == 0)
                        {
                            game.GetDrawPile.FilpAndShuffleDiscardPile(game.GetDiscardPile);
                        }

                        if (game.Direction == GameTypes.mDirectionOptions.Clockwise)
                        {
                            if (game.CurrentPlayer >= game.GetPlayers.Length)
                            {
                                game.CurrentPlayer = 0;
                            }

                            game.GetPlayers[game.CurrentPlayer].PlayersTurn(game);

                            if (game.CheckForSingleWinner(game.GetPlayers))
                            {
                                for (int Losers = 0; Losers < game.GetPlayers.Length; Losers++)
                                {
                                    if (Losers == game.CurrentPlayer)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        foreach (Card card in game.GetPlayers[Losers].GetPlayerHand.GetDeck)
                                        {
                                            game.GetPlayers[game.CurrentPlayer].Score += card.ScoreValue;
                                        }
                                    }
                                }

                                break;
                            }

                            game.CurrentPlayer++;
                        }
                        else
                        {
                            if (game.CurrentPlayer <= 0)
                            {
                                game.CurrentPlayer = game.GetPlayers.Length;
                            }

                            game.GetPlayers[game.CurrentPlayer - 1].PlayersTurn(game);

                            if (game.CheckForSingleWinner(game.GetPlayers))
                            {
                                for (int Losers = game.GetPlayers.Length; Losers < 0; Losers--)
                                {
                                    if (Losers == game.CurrentPlayer)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        foreach (Card card in game.GetPlayers[Losers].GetPlayerHand.GetDeck)
                                        {
                                            game.GetPlayers[game.CurrentPlayer].Score += card.ScoreValue;
                                        }
                                    }
                                }

                                break;
                            }

                            game.CurrentPlayer--;
                        }
                    } while (!game.CheckForSingleWinner(game.GetPlayers));

                    Console.WriteLine("Congratulations " + game.GetPlayers[game.CurrentPlayer].GetName + " you have won this round");

                } while (!game.CheckforWinner(game.GetPlayers));

                Console.WriteLine("Congratulations " + game.GetPlayers[game.CurrentPlayer].GetName + " you have won the match");

                }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
