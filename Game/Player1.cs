﻿using System;
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
            Stack<Tuple<int, int>> allPossibleMoves = borderOutline.GetAllPossibleMoves();
            toReturn = FindBestMove(borderOutline, allPossibleMoves, Turn.MaxPlayer_ME);
            if (toReturn == null)
                toReturn = borderOutline.GetNextMove();
            Console.WriteLine("{0} ms left", (_timesup - stopWatch.Elapsed).TotalMilliseconds);
            return toReturn;
        }

        private Tuple<int, int> FindBestMove(BoardOutlines borderOutline, Stack<Tuple<int, int>> movesToCheck, Turn myTurn)
        {
            Tuple<int, int> currentMove;
            BoardOutlines BoardOutlinesAterMyMove;
            Dictionary<Tuple<int, int>, BoardOutlines> boardOutlinesAfterMyTurn = new Dictionary<Tuple<int, int>, BoardOutlines>();
            while (!TimeIsAboutToEnd() && movesToCheck.Count > 0)
            {
                currentMove = movesToCheck.Pop();
                BoardOutlinesAterMyMove = new BoardOutlines(borderOutline, currentMove);
                if (IsAWiningMove(BoardOutlinesAterMyMove))
                {
                    //Console.WriteLine("Gain: 1");
                    return currentMove;
                }
                boardOutlinesAfterMyTurn[currentMove] = BoardOutlinesAterMyMove;
            }
            int maxGain = Int32.MinValue;
            int currentGain;
            Tuple<int, int> chosenMove = null;
            int depthLevel = (int)Math.Max(1, 4- Math.Log10( boardOutlinesAfterMyTurn.Keys.Count));
            int previousDepthLvl = depthLevel;
            Tuple<int, int> move;
            for (int i = 0; i < boardOutlinesAfterMyTurn.Keys.Count; i++)
            {
                move = boardOutlinesAfterMyTurn.Keys.ElementAt(i);
                if (TimeIsAboutToEnd())
                {
                    break;
                }
                BoardOutlinesAterMyMove = boardOutlinesAfterMyTurn[move];
                
                double thisBranchTimeOut;
                if (stopWatch.Elapsed.TotalMilliseconds / _timesup.TotalMilliseconds < 0.5)
                {
                    thisBranchTimeOut = Double.MaxValue;
                    depthLevel = UpdateDepthLevel(depthLevel, ref previousDepthLvl, boardOutlinesAfterMyTurn.Keys.Count - (i + 1), boardOutlinesAfterMyTurn.Keys.Count, false);

                }
                else
                {
                    depthLevel = UpdateDepthLevel(depthLevel, ref previousDepthLvl, boardOutlinesAfterMyTurn.Keys.Count - (i + 1), boardOutlinesAfterMyTurn.Keys.Count, true);
                    thisBranchTimeOut = stopWatch.Elapsed.TotalMilliseconds + (_timesup - stopWatch.Elapsed).TotalMilliseconds / (boardOutlinesAfterMyTurn.Keys.Count - i);
                }

                if (!IsALosingMove(BoardOutlinesAterMyMove))
                    currentGain = CheckMoveValue(BoardOutlinesAterMyMove, Turn.MaxPlayer_ME, depthLevel, int.MinValue, int.MaxValue, 0);
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
            //Console.WriteLine("Gain: {0}", maxGain);
            return chosenMove;
        }
        private int UpdateDepthLevel(int depthLevel, ref int previousDepthLvl, int movesLeftToEvaluate, int totalMovesToEvaluate, bool allowDecrease)
        {
            double timeLeft = (_timesup - stopWatch.Elapsed).TotalMilliseconds;
            double portionOfTimeLeft = timeLeft / _timesup.TotalMilliseconds;
            double portionOfMovesLeft = (double)movesLeftToEvaluate / (double)totalMovesToEvaluate;
            double previousPortionOfMovesLeft = (double)movesLeftToEvaluate / (double)totalMovesToEvaluate;

            if (portionOfMovesLeft > portionOfTimeLeft && allowDecrease)
            {
                if (previousDepthLvl > depthLevel || previousDepthLvl == 1)
                {
                    int newDepthLevel = 0;
                    previousDepthLvl = depthLevel;
                    Console.WriteLine("Decreased depth level to :{0} , moves left: ({1} from {2}), time left: ({3} from {4})", newDepthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
                    return newDepthLevel;
                }
                else
                {
                    int newDepthLevel = Math.Max(depthLevel - 1, 0);
                    previousDepthLvl = depthLevel;
                    Console.WriteLine("Decreased depth level to :{0} , moves left: ({1} from {2}), time left: ({3} from {4})", newDepthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
                    return newDepthLevel;
                }

            }
            if (previousPortionOfMovesLeft < portionOfTimeLeft)
            {
                int newDepthLevel = depthLevel + 1;//+ 1;
                previousDepthLvl = depthLevel;
                Console.WriteLine("Increaded depth level to :{0}  , moves left: ({1} from {2}), time left: ({3} from {4})", newDepthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
                return newDepthLevel;
            }
            Console.WriteLine("Depth level remained :{0}  , moves left: ({1} from {2}), time left: ({3} from {4})", depthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
            previousDepthLvl = depthLevel;
            return depthLevel;
        }

        private bool IsAWiningMove(BoardOutlines currentBorderOutlines)
        {
            return   currentBorderOutlines.TwoSameLengthStripes() || currentBorderOutlines.TwoSameLengthRows() || currentBorderOutlines.TwoSameLengthCols();
        }
        private bool IsALosingMove(BoardOutlines currentBorderOutlines)
        {
            return currentBorderOutlines.OnlyPoisonedSquareLeft() || currentBorderOutlines.OneCol() || currentBorderOutlines.OneRow(); //|| currentBorderOutlines.BoardIs2XN() || currentBorderOutlines.BoardIsNX2();
        }

        private int CheckMoveValue(BoardOutlines boardOutline, Turn playedPreviousTurn, int depthLevel, int alpha, int beta, double branchTimeOut)
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
            Stack<Tuple<int, int>> allPossibleMoves = boardOutline.GetAllPossibleMoves();
            while (!TimeIsAboutToEnd() && allPossibleMoves.Count > 0)
            {
                currentMove = allPossibleMoves.Pop();
                boardOutlineAfterMove = new BoardOutlines(boardOutline, currentMove);

                boardsOutlinesAfterThisMove[currentMove] = boardOutlineAfterMove;

                currentMoveValue = CheckMoveValue(boardOutlineAfterMove, thisTurn, depthLevel - 1, alpha, beta, branchTimeOut);
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

            internal bool OnlyPoisonedSquareLeft()
            {
                return _mostBottomRow == 0 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0;
            }

            internal bool TwoSameLengthStripes()
            {
                return RightmostAvailabeSquareAtRow[0] == _mostBottomRow && RightmostAvailabeSquareAtRow[1] == 0;
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

            internal bool BoardIsNX2()
            {
                return RightmostAvailabeSquareAtRow[_mostBottomRow] == 1 && RightmostAvailabeSquareAtRow[0] == 1;
            }

            internal bool BoardIs2XN()
            {
                return _mostBottomRow == 1 && RightmostAvailabeSquareAtRow[1] == RightmostAvailabeSquareAtRow[0];
            }
            internal bool NestMoveLeadsToTwoSameLengthStripes()
            {
                return _mostBottomRow == RightmostAvailabeSquareAtRow[0] && RightmostAvailabeSquareAtRow[1] > 0;

            }

            internal Stack<Tuple<int, int>> GetAllPossibleMoves()
            {
                Stack<Tuple<int, int>> allPossibleMoves = new Stack<Tuple<int, int>>();
                if (RightmostAvailabeSquareAtRow[0] == 0 && RightmostAvailabeSquareAtRow[1] == null)
                    return allPossibleMoves;
                Tuple<int, int> possibleMove = new Tuple<int, int>(_mostBottomRow, (int)RightmostAvailabeSquareAtRow[_mostBottomRow]);
                allPossibleMoves.Push(possibleMove);
                while (true)
                {
                    if (possibleMove.Item2 > 0)
                        possibleMove = new Tuple<int, int>(possibleMove.Item1, possibleMove.Item2 - 1);
                    else
                        possibleMove = new Tuple<int, int>(possibleMove.Item1 - 1, (int)RightmostAvailabeSquareAtRow[possibleMove.Item1 - 1]);
                    if (possibleMove.Item1 != 0 || possibleMove.Item2 != 0)
                        allPossibleMoves.Push(possibleMove);
                    else
                        break;

                }
                return allPossibleMoves;
            }

        }
    }
}