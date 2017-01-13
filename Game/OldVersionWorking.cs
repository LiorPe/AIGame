using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class OldVersionWorking
    {
        Stopwatch stopWatch = new Stopwatch();
        TimeSpan _timesup;
        int _MAxDepthLevel = 14;
        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "203722814";  //id1
            player1_2 = "308111160";  //id2

        }
        public Tuple<int, int> playYourTurn
        (
            Board board,
            TimeSpan timesup
        )
        {
            StartStopWatch();
            _timesup = timesup;
            BoardOutlines borderOutline = new BoardOutlines(board);
            Tuple<int, int> toReturn = null;
            Queue<Tuple<int, int>> allPossibleMoves = borderOutline.GetAllPossibleMoves();
            toReturn = FindBestMove(borderOutline, allPossibleMoves, Turn.MaxPlayer_ME);
            if (toReturn == null)
                toReturn = borderOutline.GetNextMove();
            Console.WriteLine("{0} ms left", (_timesup - stopWatch.Elapsed).TotalMilliseconds);
            return toReturn;
        }

        private Tuple<int, int> FindBestMove(BoardOutlines borderOutline, Queue<Tuple<int, int>> movesToCheck, Turn myTurn)
        {
            Tuple<int, int> currentMove;
            BoardOutlines BoardOutlinesAterMyMove;
            Dictionary<Tuple<int, int>, BoardOutlines> boardOutlinesAfterMyTurn = new Dictionary<Tuple<int, int>, BoardOutlines>();
            while (!TimeIsAboutToEnd() && movesToCheck.Count > 0)
            {
                currentMove = movesToCheck.Dequeue();
                BoardOutlinesAterMyMove = new BoardOutlines(borderOutline, currentMove);
                if (IsAWiningMove(BoardOutlinesAterMyMove))
                {
                    return currentMove;
                }
                boardOutlinesAfterMyTurn[currentMove] = BoardOutlinesAterMyMove;
            }
            int maxGain = Int32.MinValue;
            int currentGain;
            Tuple<int, int> chosenMove = null;
            int i = 0;
            int depthLevel = 8;
            foreach (Tuple<int, int> move in boardOutlinesAfterMyTurn.Keys)
            {
                if (TimeIsAboutToEnd())
                {
                    break;
                }
                BoardOutlinesAterMyMove = boardOutlinesAfterMyTurn[move];
                if (!IsALosingMove(BoardOutlinesAterMyMove))
                    currentGain = CheckMoveValue(BoardOutlinesAterMyMove, Turn.MaxPlayer_ME, depthLevel, int.MinValue, int.MaxValue);
                else
                    currentGain = -1;
                if (currentGain > maxGain)
                {
                    maxGain = currentGain;
                    chosenMove = move;

                    if (maxGain == 1)
                    {

                        break;
                    }

                }
                i++;
            }
            return chosenMove;
        }


        private bool IsAWiningMove(BoardOutlines currentBorderOutlines)
        {
            try
            {
                return currentBorderOutlines.TwoSameLengthStripes() || currentBorderOutlines.TwoSameLengthRows() || currentBorderOutlines.TwoSameLengthCols();
            }
            catch
            {
                return currentBorderOutlines.TwoSameLengthStripes() || currentBorderOutlines.TwoSameLengthRows() || currentBorderOutlines.TwoSameLengthCols();

            }

        }
        private bool IsALosingMove(BoardOutlines currentBorderOutlines)
        {
            try
            {
                return currentBorderOutlines.LastSquareLefat() || currentBorderOutlines.OneCol() || currentBorderOutlines.OneRow();

            }
            catch
            {
                return currentBorderOutlines.LastSquareLefat() || currentBorderOutlines.OneCol() || currentBorderOutlines.OneRow();

            }
        }

        private int CheckMoveValue(BoardOutlines boardOutline, Turn playedPreviousTurn, int depthLevel, int alpha, int beta)
        {


            if (IsAWiningMove(boardOutline))
            {
                if (playedPreviousTurn == Turn.MaxPlayer_ME)
                    return 1;
                else
                    return -1;
            }
            if (IsALosingMove(boardOutline))
            {
                if (playedPreviousTurn == Turn.MaxPlayer_ME)
                    return -1;
                else
                    return 1;
            }
            if (depthLevel == 0)
            {
                return 0;
            }

            int bestValue;
            int currentMoveValue;
            Turn thisTurn;
            if (playedPreviousTurn == Turn.MaxPlayer_ME)
            {
                thisTurn = Turn.MinPlayer_Opponent;
                bestValue = int.MaxValue;
            }
            else
            {
                bestValue = int.MinValue;
                thisTurn = Turn.MaxPlayer_ME;
            }
            Tuple<int, int> currentMove;
            BoardOutlines boardOutlineAfterMove;
            Dictionary<Tuple<int, int>, BoardOutlines> boardsOutlinesAfterThisMove = new Dictionary<Tuple<int, int>, BoardOutlines>();
            Queue<Tuple<int, int>> allPossibleMoves = boardOutline.GetAllPossibleMoves();
            while (!TimeIsAboutToEnd() && allPossibleMoves.Count > 0)
            {
                currentMove = allPossibleMoves.Dequeue();
                boardOutlineAfterMove = new BoardOutlines(boardOutline, currentMove);

                boardsOutlinesAfterThisMove[currentMove] = boardOutlineAfterMove;

                currentMoveValue = CheckMoveValue(boardOutlineAfterMove, thisTurn, depthLevel - 1, alpha, beta);
                if (thisTurn == Turn.MaxPlayer_ME)
                {
                    bestValue = Math.Max(bestValue, currentMoveValue);
                    alpha = Math.Max(alpha, currentMoveValue);
                    if (bestValue >= beta || bestValue == 1)
                    {
                        //Console.WriteLine("Tree cutted at depth {0} by alpha beya pruning!", depthLevel);
                        break;
                    }
                }
                else
                {
                    bestValue = Math.Min(bestValue, currentMoveValue);
                    beta = Math.Min(beta, currentMoveValue);
                    if (bestValue <= alpha || bestValue == -1)
                    {
                        //Console.WriteLine("Tree cut at depth {0} by alpha beya pruning!", depthLevel);
                        break;
                    }

                }
            }
            return bestValue;



            foreach (Tuple<int, int> move in boardsOutlinesAfterThisMove.Keys)
            {
                if (TimeIsAboutToEnd())
                {
                    break;
                }

                boardOutlineAfterMove = boardsOutlinesAfterThisMove[move];
            }

        }

        private bool TimeIsAboutToEnd()
        {
            return (_timesup - stopWatch.Elapsed).TotalMilliseconds < 10;
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

        public enum Turn { MaxPlayer_ME, MinPlayer_Opponent };


        private class BoardOutlines
        {
            public int _rows;
            public int _cols;
            public int _mostBottomRow = 0;
            public Dictionary<int, int?> RightmostAvailabeSquareAtRow;
            private Tuple<int, int> currentMove = null;

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
                    while (col < board._cols && !found)
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

            public BoardOutlines(BoardOutlines boardOutlines, Tuple<int, int> move)
            {
                _rows = boardOutlines._rows;
                _cols = boardOutlines._cols;
                RightmostAvailabeSquareAtRow = new Dictionary<int, int?>();

                foreach (int row in boardOutlines.RightmostAvailabeSquareAtRow.Keys)
                {
                    int? rightMostAvailabeSquareAtRow = boardOutlines.RightmostAvailabeSquareAtRow[row];
                    if (row >= move.Item1 && rightMostAvailabeSquareAtRow != null && rightMostAvailabeSquareAtRow >= move.Item2)
                    {
                        if (move.Item2 == 0)
                        {
                            RightmostAvailabeSquareAtRow[row] = null;
                        }
                        else
                            RightmostAvailabeSquareAtRow[row] = move.Item2 - 1;

                    }
                    else
                        RightmostAvailabeSquareAtRow[row] = rightMostAvailabeSquareAtRow;
                    if (RightmostAvailabeSquareAtRow[row] != null)
                        _mostBottomRow = row;
                }
            }

            public Tuple<int, int> GetNextMove()
            {
                if (currentMove == null)
                    currentMove = new Tuple<int, int>(_mostBottomRow, (int)RightmostAvailabeSquareAtRow[_mostBottomRow]);
                else
                {
                    if (currentMove.Item2 > 0)
                        currentMove = new Tuple<int, int>(currentMove.Item1, currentMove.Item2 - 1);
                    else
                        currentMove = new Tuple<int, int>(currentMove.Item1 - 1, (int)RightmostAvailabeSquareAtRow[currentMove.Item1 - 1]);
                }
                return currentMove;
            }

            internal bool LastSquareLefat()
            {
                return _mostBottomRow == 0 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0;
            }

            internal bool TwoSameLengthStripes()
            {
                return _mostBottomRow == RightmostAvailabeSquareAtRow[0] && RightmostAvailabeSquareAtRow[1] == 0;
            }

            internal bool TwoSameLengthRows()
            {

                if (RightmostAvailabeSquareAtRow[1] != null && RightmostAvailabeSquareAtRow[0] == RightmostAvailabeSquareAtRow[1] + 1 && _mostBottomRow == 1)
                    return true;
                else
                    return false;
            }

            internal bool TwoSameLengthCols()
            {
                if (RightmostAvailabeSquareAtRow[0] == 1 && _mostBottomRow > 0 && RightmostAvailabeSquareAtRow[_mostBottomRow - 1] == 1 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0)
                    return true;
                return false;
            }



            internal bool OneRow()
            {
                return _mostBottomRow == 0;
            }

            internal bool OneCol()
            {
                return RightmostAvailabeSquareAtRow[0] == 0;
            }



            internal Queue<Tuple<int, int>> GetAllPossibleMoves()
            {
                Queue<Tuple<int, int>> allPossibleMoves = new Queue<Tuple<int, int>>();
                Tuple<int, int> possibleMove = new Tuple<int, int>(_mostBottomRow, (int)RightmostAvailabeSquareAtRow[_mostBottomRow]);
                allPossibleMoves.Enqueue(possibleMove);
                while (true)
                {
                    if (possibleMove.Item2 > 0)
                        possibleMove = new Tuple<int, int>(possibleMove.Item1, possibleMove.Item2 - 1);
                    else
                        possibleMove = new Tuple<int, int>(possibleMove.Item1 - 1, (int)RightmostAvailabeSquareAtRow[possibleMove.Item1 - 1]);
                    if (possibleMove.Item1 != 0 || possibleMove.Item2 != 0)
                        allPossibleMoves.Enqueue(possibleMove);
                    else
                        break;

                }
                return allPossibleMoves;
            }

        }
    }
}