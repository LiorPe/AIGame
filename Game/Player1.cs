using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class Player1 
    {
        Stopwatch stopWatch = new Stopwatch();

        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "123456789";  //id1
            player1_2 = "123456789";  //id2

        }
        public Tuple<int, int> playYourTurn
        (
            Board board,
            TimeSpan timesup
        )
        {
            StartStopWatch();
            Tuple<int, int> toReturn = null;
            BoardOutlines borderOutline = new BoardOutlines(board);

            PrintTimeElapsed();

               //Random Algorithm - Start
               int randomRow;
            int randomCol;
            Random random = new Random();
            do
            {
                randomRow = random.Next(0, board._rows);
                randomCol = random.Next(0, board._cols);
            } while (board._board[randomRow, randomCol] != 'X'); //&& board.isTheGameEnded() == ' ');
            toReturn = new Tuple<int, int>(randomRow, randomCol);
            //Random Algorithm - End

            return toReturn;
        }


        private void StartStopWatch()
        {
            stopWatch.Restart();
        }
        private void PrintTimeElapsed()
        {
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
          ts.Hours, ts.Minutes, ts.Seconds,
          ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        public enum Turn { MyTurn, OpponentTurn };


        private class BoardOutlines
        {
            public int _rows;
            public int _cols;
            public int _mostBottomRow=0;
            public Dictionary<int, int?> RightmostAvailabeSquareAtRow;

            public BoardOutlines(Board board)
            {
                _rows = board._rows;
                _cols = board._cols;
                RightmostAvailabeSquareAtRow = new Dictionary<int, int?>();
                for (int row = 0; row < board._rows; row++)
                {
                    int? rightMostAvailabeSquareAtRow = null;
                    bool found = false;
                    int col = 0;
                    while (col<board._cols && !found)
                    {
                        if (board._board[row, col] == 'X')
                            rightMostAvailabeSquareAtRow = col;
                        else
                            found = true;
                        col++;
                    }
                    RightmostAvailabeSquareAtRow[row] = rightMostAvailabeSquareAtRow;
                    if (rightMostAvailabeSquareAtRow != null)
                        _mostBottomRow = row;
                }
            }

            public BoardOutlines(BoardOutlines boardOutlines, Tuple<int,int> move)
            {
                foreach (int row in boardOutlines.RightmostAvailabeSquareAtRow.Keys)
                {
                    int? rightMostAvailabeSquareAtRow = boardOutlines.RightmostAvailabeSquareAtRow[row];
                    if (row >= move.Item1 && rightMostAvailabeSquareAtRow!= null && rightMostAvailabeSquareAtRow> move.Item2)
                    {
                        if (move.Item2 == 0)
                        {
                            RightmostAvailabeSquareAtRow[row] = null;
                        }
                        else
                            RightmostAvailabeSquareAtRow[row] = move.Item2 - 1;
                    }
                }
            }


        }
    }
}
