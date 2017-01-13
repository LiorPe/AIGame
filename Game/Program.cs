using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1;
using System.IO;

namespace Game
{
    class Program
    {
        public const int m_numberOfGames = 1 ; //100;
        public const int m_boardRows          = 4;
        public const int m_boardCols          = 5;
        public const int m_gameLevel          = 5;
        public const bool m_printAllResults   /*= false; */ = true;
        static Logger logger;
        static void Main(string[] args)
        {
            string[] fi = Directory.GetFiles(@"log");
            foreach(string file in fi)
            {
                File.Delete(file);
            }
            char playerTurn         = '1';
            char firstPlayer        = '1';
            char winner             = ' ';
            Board board             = createEmptyBoard();
            int player1wins         = 0;
            int player2wins         = 0;
            bool legalTurn          = true;
            Random random           = new Random();
            for (int game = 0; game < m_numberOfGames; game++)
            {
                logger = new Logger();
                if (game > 0)
                {
                    board           = createEmptyBoard();
                    winner          = ' ';
                    playerTurn      = firstPlayer;
                    legalTurn       = true;
                    switchTurns(ref firstPlayer);
                }
                do
                {
                    try
                    {
                        if (playerTurn == '1')
                        {
                            legalTurn = Turn(board, playerTurn, true);             //Your Turn             
                        }
                        else if (playerTurn == '2')
                        {
                            legalTurn = Turn(board, playerTurn, false);
                        }
                    }
                    catch(Exception e)
                    {
                        legalTurn = false;
                        legalTurn = Turn(board, playerTurn, true);             //Your Turn             
                    }
                    switchTurns(ref playerTurn);
                    if (board.isTheGameEnded() || !legalTurn)
                    {
                        winner = playerTurn;
                    }
                } while (winner == ' ');

                if (winner == '1')
                    player1wins++;
                else if (winner == '2')
                {
                    player2wins++;
                    logger.WriteToFile(@"log\AIGameLog" + game + ".txt");
                }
            }

            if(m_printAllResults)
                printAllGamesResult(player1wins, player2wins);                  //Print all games result
             
        }

        private static Board createEmptyBoard()
        {
            return new Board(m_boardRows, m_boardCols);
        }

        private static void switchTurns(ref char playerTurn)
        {
            if (playerTurn == '1')
                playerTurn = '2';
            else
                playerTurn = '1';
        }

        private static void printAllGamesResult
        (
            int player1wins, 
            int player2wins
        )
        {
            Console.WriteLine("Lior wins:  " + player1wins + "\nNoga wins:  " + player2wins);
            Console.ReadLine();
        }

        private static bool Turn
        (
            Board board, 
            char player,
            bool timeLimit
        )
        {
            int stopMilliseconds;
            if (timeLimit == false)
                stopMilliseconds = 100;
            else
                stopMilliseconds = timeByLevel();
            System.GC.Collect();
            Stopwatch timer      = Stopwatch.StartNew();
            Tuple<int, int> move = new Tuple<int, int>(-1, -1);
            if (player == '1')
            {
                move = (new Player1()).playYourTurn(new Board(board), new TimeSpan(0, 0, 0, 0, stopMilliseconds));
                logger.Add(String.Format("Lior`s Move: ({0},{1})", move.Item1, move.Item2));
            }
            else if (player == '2')
            {
                move = (new NogaPlayer()).playYourTurn(new Board(board), new TimeSpan(0, 0, 0, 0, stopMilliseconds));
                logger.Add(String.Format("Noga`s Move: ({0},{1})", move.Item1, move.Item2));
            }
            timer.Stop();
            //Console.WriteLine("({0},{1})", move.Item1, move.Item2);

            TimeSpan timespan    = timer.Elapsed;
            if (timespan.TotalMilliseconds > stopMilliseconds ||
                !board.isLegalMove(move.Item1, move.Item2))
            {
                if (player == '1')
                    move = (new Player1()).playYourTurn(new Board(board), new TimeSpan(0, 0, 0, 0, stopMilliseconds));
                return false;

            }
            else
            {
                if (player == '2' && move.Item1 == 1 && move.Item2 == 2)
                    Console.WriteLine();
                board.fillPlayerMove(move.Item1, move.Item2);
                return true;
            }
            
        }

        private static int timeByLevel()
        {
            if (m_gameLevel == 1)
                return 200;
            else if (m_gameLevel == 2)
                return 150;
            else if (m_gameLevel == 3)
                return 100;
            else if (m_gameLevel == 4)
                return 80;
            else
                return 50; //1000000 * 60 * 2;//
        }

        public class Logger{
            List<string> log = new List<string>();
            public void Add(string s)
            {
                log.Add(s);
            }
            public void WriteToFile(string path)
            {
                File.WriteAllLines(path, log.ToArray());
            }
        }
    }
}
