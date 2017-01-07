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
        TimeSpan _timesup;
        int branchingFactor;
        int _depthLevel=5;
        int _calculatedNodes=1;
        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "203722814";  //id1
            player1_2 = "123456789";  //id2

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
            branchingFactor = allPossibleMoves.Count();
            toReturn = FindBestMove(borderOutline, allPossibleMoves, Turn.MyTurn);
            if (toReturn == null)
                toReturn = borderOutline.GetNextMove();
            Console.WriteLine("{0} ms left", (_timesup - stopWatch.Elapsed).TotalMilliseconds);
            return toReturn;
        }

        private Tuple<int, int> FindBestMove(BoardOutlines borderOutline, Queue<Tuple<int, int>> movesToCheck, Turn myTurn)
        {
            Tuple<int, int> currentMove;
            BoardOutlines currentBorderOutline;
            Dictionary<Tuple<int, int>, BoardOutlines> boardOutlinesAfterMove = new Dictionary<Tuple<int, int>, BoardOutlines>();
            while (!TimeIsAboutToEnd()&&  movesToCheck.Count > 0)
            {
                currentMove = movesToCheck.Dequeue();
                currentBorderOutline = new BoardOutlines(borderOutline, currentMove);
                if (IsAWiningMove(currentBorderOutline))
                {
                    bool ans = IsAWiningMove(currentBorderOutline);
                    return currentMove;
                }
                boardOutlinesAfterMove[currentMove] = currentBorderOutline;
            }
            int maxGain = Int32.MaxValue;
            int currentGain;
            Tuple<int, int> chosenMove=null;
            foreach (Tuple<int, int> move in boardOutlinesAfterMove.Keys)
            {
                if (TimeIsAboutToEnd())
                    break;
                currentBorderOutline = boardOutlinesAfterMove[move];
                currentGain = CheckMoveGain(currentBorderOutline, move, Turn.MyTurn, _depthLevel);
                if (currentGain> maxGain)
                {
                    maxGain = currentGain;
                    chosenMove = move;
                }
            }
            return chosenMove;
        }


        private bool IsAWiningMove(BoardOutlines currentBorderOutlines)
        {
            return currentBorderOutlines.LastSquareLefat() || currentBorderOutlines.TwoSameLengthStripes() || currentBorderOutlines.TwoSameLengthRows() || currentBorderOutlines.TwoSameLengthCols();
        }

        private int CheckMoveGain(BoardOutlines borderOutlineBeforeMove, Tuple<int, int> previousMove, Turn playedPreviousTurn, int depthLevel)
        {
            _calculatedNodes++;
            Tuple<int, int> currentMove;
            BoardOutlines currentBorderOutline;
            Dictionary<Tuple<int, int>, BoardOutlines> boardOutlinesAfterMove = new Dictionary<Tuple<int, int>, BoardOutlines>();
            Queue<Tuple<int, int>> allPossibleMoves = borderOutlineBeforeMove.GetAllPossibleMoves();
            while ((_timesup - stopWatch.Elapsed).TotalMilliseconds > 10 && allPossibleMoves.Count > 0)
            {
                currentMove = allPossibleMoves.Dequeue();
                currentBorderOutline = new BoardOutlines(borderOutlineBeforeMove, currentMove);
                if (IsAWiningMove(currentBorderOutline))
                {
                    if (playedPreviousTurn == Turn.MyTurn)
                        return 1;
                    else
                        return - 1;
                }
                boardOutlinesAfterMove[currentMove] = currentBorderOutline;
            }
            if (depthLevel == 0)
            {
                return 0;
            }
            int totalGain;
            int currentGain;
            Turn nextTurn;
            if (playedPreviousTurn == Turn.MyTurn)
            {
                nextTurn = Turn.OpponentTurn;
                totalGain = int.MinValue;
            }
            else
            {
                totalGain = int.MaxValue;
                nextTurn = Turn.MyTurn;
            }
            int counter = 0;
            foreach (Tuple<int, int> move in boardOutlinesAfterMove.Keys)
            {
                if (TimeIsAboutToEnd())
                    break;
                currentBorderOutline = boardOutlinesAfterMove[move];
                currentGain = CheckMoveGain(currentBorderOutline, move, Turn.MyTurn, depthLevel-1);
                if (playedPreviousTurn == Turn.MyTurn)
                    totalGain = Math.Max(totalGain, currentGain);
                else
                    totalGain = Math.Min(totalGain, currentGain);
                counter++;
            }
            //Console.WriteLine("{0} moves were checked from {1} possible moves", counter, boardOutlinesAfterMove.Count);
            return totalGain;

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

        public enum Turn { MyTurn, OpponentTurn };


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
                int row = 1;

                    bool result = true;
                    int? stripeLength = RightmostAvailabeSquareAtRow[0];
                    if (stripeLength == null)
                        return false;
                    
                    for (; row < _rows; row++)
                    {
                        if (RightmostAvailabeSquareAtRow[row] != 0)
                            break;
                    }
                    if (row-1 != stripeLength)
                        return false;
                    for (; row < _rows; row++)
                    {
                        if (RightmostAvailabeSquareAtRow[row] != null)
                            return false;
                    }

                    return true;

            }

            internal bool TwoSameLengthRows()
            {
                if (RightmostAvailabeSquareAtRow[0] != RightmostAvailabeSquareAtRow[1] + 1)
                    return false; 
                else
                {
                    for (int row =2; row < _rows; row++)
                    {
                        if (RightmostAvailabeSquareAtRow[row]!=null)
                            return false;
                    }
                    return true;
                }
            }

            internal bool TwoSameLengthCols()
            {
                try
                {
                    int colLength = 0;
                    while (colLength < _rows)
                    {
                        if (RightmostAvailabeSquareAtRow[colLength] == 1)
                            colLength++;
                        else
                            break;

                    }
                    if (colLength + 1 >= _rows || RightmostAvailabeSquareAtRow[colLength + 1] != 0)
                        return false;
                    for (int row = colLength + 2; row < _rows; row++)
                    {
                        if (RightmostAvailabeSquareAtRow[row] != null)
                            return false;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }

            }

            internal Queue<Tuple<int, int>> GetAllPossibleMoves()
            {
                Queue<Tuple<int, int>> allPossibleMoves = new Queue<Tuple<int, int>>();
                Tuple < int, int> possibleMove = new Tuple<int, int>(_mostBottomRow, (int)RightmostAvailabeSquareAtRow[_mostBottomRow]);
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
