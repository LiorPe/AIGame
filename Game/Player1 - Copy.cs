using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1;

namespace Game
{
    internal class Player1Copy
    {
        //Stopwatch stopWatch = new Stopwatch();
        //TimeSpan _timesup;
        //public Dictionary<string, bool> memory;
        //public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        //{
        //    player1_1 = "203722814";  //id1
        //    player1_2 = "308111160";  //id2

        //}
        //public Tuple<int, int> playYourTurn
        //(
        //    Board board,
        //    TimeSpan timesup
        //)
        //{
        //    StartStopWatch();
        //    memory = new Dictionary<string, bool>();
        //    if (!CompareBoardToBoardOutlines(board))
        //        CompareBoardToBoardOutlines(board);
        //    _timesup = timesup;
        //    BoardOutlines borderOutline = new BoardOutlines(board);
        //    Tuple<int, int> toReturn = null;
        //    toReturn = FindBestMove(borderOutline);
        //    memory = new Dictionary<string, bool>();
        //    if (toReturn == null)
        //        toReturn = borderOutline.GetMoveForLosingState();
        //    Console.WriteLine("{0} ms left", (_timesup - stopWatch.Elapsed).TotalMilliseconds);

        //    return toReturn;
        //}

        //private bool CompareBoardToBoardOutlines(Board board)
        //{
        //    BoardOutlines boardOutlines = new BoardOutlines(board);
        //    for (int row=0;row< boardOutlines._mostBottomRow;row++)
        //    {
        //        int lastSquarePos = boardOutlines.RightmostAvailabeSquareAtRow[row];
        //        if (board._board[row, lastSquarePos] != 'X')
        //            return false;
        //        if (board._cols > lastSquarePos + 1 && board._board[row, lastSquarePos + 1] == 'X')
        //            return false;
        //    }
        //    for (int col =0;col< board._cols && boardOutlines._mostBottomRow + 1<board._rows; col++)
        //    {
        //        if (board._board[boardOutlines._mostBottomRow+1, col] == 'X')
        //            return false;
        //    }
        //    return true;
                
        //}

        //private Tuple<int, int> FindBestMove(BoardOutlines borderOutline)
        //{
        //    Stack<Tuple<int, int>> movesToCheck  = borderOutline.GetAllPossibleMoves();
        //    Tuple<int, int> currentMove;
        //    BoardOutlines BoardOutlinesAterMyMove;
        //    Dictionary<Tuple<int, int>, BoardOutlines> boardOutlinesAfterMyTurn = new Dictionary<Tuple<int, int>, BoardOutlines>();
        //    while (!TimeIsAboutToEnd() && movesToCheck.Count > 0)
        //    {
        //        currentMove = movesToCheck.Pop();
        //        BoardOutlinesAterMyMove = new BoardOutlines(borderOutline, currentMove);
        //        if (LoosingSituation(BoardOutlinesAterMyMove))
        //        {
        //            LoosingSituation(BoardOutlinesAterMyMove);
        //            Console.WriteLine("Gain calculated: 1");
        //            return currentMove;
        //        }
        //        boardOutlinesAfterMyTurn[currentMove] = BoardOutlinesAterMyMove;
        //    }

        //    int maxGain = Int32.MinValue;
        //    int currentGain;
        //    Tuple<int, int> chosenMove = null;
        //    int depthLevel = 7;
        //    int previousDepthLvl = depthLevel;
        //    Tuple<int, int> move;
        //    for (int i = 0; i < boardOutlinesAfterMyTurn.Keys.Count; i++)
        //    {
        //        move = boardOutlinesAfterMyTurn.Keys.ElementAt(i);
        //        if (TimeIsAboutToEnd())
        //        {
        //            break;
        //        }
        //        BoardOutlinesAterMyMove = boardOutlinesAfterMyTurn[move];

        //        if (stopWatch.Elapsed.TotalMilliseconds / _timesup.TotalMilliseconds < 0.3)
        //        {
        //            depthLevel = UpdateDepthLevel(depthLevel, ref previousDepthLvl, boardOutlinesAfterMyTurn.Keys.Count - i, boardOutlinesAfterMyTurn.Keys.Count, false,7);

        //        }
        //        else
        //        {
        //            depthLevel = UpdateDepthLevel(depthLevel, ref previousDepthLvl, boardOutlinesAfterMyTurn.Keys.Count - i, boardOutlinesAfterMyTurn.Keys.Count, true,7);
        //        }
        //        currentGain = CheckMoveValue(BoardOutlinesAterMyMove, Turn.MinPlayer_Opponent, depthLevel, int.MinValue, int.MaxValue);
        //        if (currentGain > maxGain)
        //        {
        //            maxGain = currentGain;
        //            chosenMove = move;

        //            if (maxGain == 1)
        //            {

        //                break;
        //            }

        //        }

        //    }
        //    Console.WriteLine("Gain calculated: " + maxGain);
        //    if (maxGain <= 0)
        //    {
        //        return borderOutline.GetMoveForLosingState();
        //    }
        //    return chosenMove;

        //}
        //private int UpdateDepthLevel(int depthLevel, ref int previousDepthLvl, int movesLeftToEvaluate, int totalMovesToEvaluate, bool allowDecrease, int maxDepthLevel)
        //{
        //    double timeLeft = (_timesup - stopWatch.Elapsed).TotalMilliseconds;
        //    double portionOfTimeLeft = timeLeft / _timesup.TotalMilliseconds;
        //    double portionOfMovesLeft = (double)movesLeftToEvaluate / (double)totalMovesToEvaluate;
        //    double previousPortionOfMovesLeft = (double)(movesLeftToEvaluate - 1) / (double)totalMovesToEvaluate;

        //    if (portionOfMovesLeft > portionOfTimeLeft && allowDecrease)
        //    {
        //        if (previousDepthLvl > depthLevel || previousDepthLvl == 1)
        //        {
        //            int newDepthLevel = 0;
        //            previousDepthLvl = depthLevel;
        //            //Console.WriteLine("Decreased depth level to :{0} , moves left: ({1} from {2}), time left: ({3} from {4})", newDepthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
        //            return newDepthLevel;
        //        }
        //        else
        //        {
        //            int newDepthLevel = Math.Max(depthLevel - 1, 0);
        //            previousDepthLvl = depthLevel;
        //            //Console.WriteLine("Decreased depth level to :{0} , moves left: ({1} from {2}), time left: ({3} from {4})", newDepthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
        //            return newDepthLevel;
        //        }

        //    }
        //    if (previousPortionOfMovesLeft < portionOfTimeLeft)
        //    {
        //        int newDepthLevel = Math.Min(depthLevel + 1, maxDepthLevel);//+ 1;
        //        previousDepthLvl = depthLevel;
        //        //Console.WriteLine("Increaded depth level to :{0}  , moves left: ({1} from {2}), time left: ({3} from {4})", newDepthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
        //        return newDepthLevel;
        //    }
        //    //Console.WriteLine("Depth level remained :{0}  , moves left: ({1} from {2}), time left: ({3} from {4})", depthLevel, movesLeftToEvaluate, totalMovesToEvaluate, _timesup.TotalMilliseconds - stopWatch.Elapsed.TotalMilliseconds, _timesup.TotalMilliseconds);
        //    previousDepthLvl = depthLevel;
        //    return depthLevel;
        //}

        //private bool LoosingSituation(BoardOutlines currentBorderOutlines)
        //{

        //    bool result = currentBorderOutlines.OnlyPoisonedSquareLeft() || currentBorderOutlines.TwoSameLengthStripes() || currentBorderOutlines.TwoSameLengthRows() || currentBorderOutlines.TwoSameLengthCols();
        //    return result;

        //}
        //private bool WinningSituation(BoardOutlines currentBorderOutlines)
        //{

        //    bool OneCol = currentBorderOutlines.OneCol();
        //    bool OneRow = currentBorderOutlines.OneRow();
        //    bool square = currentBorderOutlines.Square();
        //    if (square || OneCol || OneRow)
        //    {
        //        return true;
        //    }
        //    return false;



        //}

        //private int CheckMoveValue(BoardOutlines boardOutline, Turn currentTurnPlayer, int depthLevel, int alpha, int beta)
        //{
        //    if (LoosingSituation(boardOutline))
        //    {
        //        if (currentTurnPlayer == Turn.MaxPlayer_ME)
        //            return -1;
        //        else
        //            return 1;
        //    }
        //    if (WinningSituation(boardOutline))
        //    {
        //        if (currentTurnPlayer == Turn.MaxPlayer_ME)
        //            return 1;
        //        else
        //            return -1;
        //    }
        //    if (depthLevel == 0)
        //    {
        //        return 0;
        //    }

        //    int bestValue;
        //    int currentMoveValue;
        //    Turn nextPlayerTurn;
        //    if (currentTurnPlayer == Turn.MaxPlayer_ME)
        //    {
        //        nextPlayerTurn = Turn.MinPlayer_Opponent;
        //        bestValue = int.MinValue;
        //    }
        //    else
        //    {
        //        bestValue = int.MaxValue;
        //        nextPlayerTurn = Turn.MaxPlayer_ME;
        //    }
        //    Tuple<int, int> currentMove;
        //    BoardOutlines boardOutlineAfterMove;
        //    Dictionary<Tuple<int, int>, BoardOutlines> boardsOutlinesAfterThisMove = new Dictionary<Tuple<int, int>, BoardOutlines>();
        //    Stack<Tuple<int, int>> allPossibleMoves = boardOutline.GetAllPossibleMoves();
        //    while (!TimeIsAboutToEnd() && allPossibleMoves.Count > 0)
        //    {
        //        currentMove = allPossibleMoves.Pop();
        //        boardOutlineAfterMove = new BoardOutlines(boardOutline, currentMove);

        //        boardsOutlinesAfterThisMove[currentMove] = boardOutlineAfterMove;

        //        currentMoveValue = CheckMoveValue(boardOutlineAfterMove, nextPlayerTurn, depthLevel - 1, alpha, beta);
        //        if (currentTurnPlayer == Turn.MaxPlayer_ME)
        //        {
        //            bestValue = Math.Max(bestValue, currentMoveValue);
        //            alpha = Math.Max(alpha, currentMoveValue);
        //            if (bestValue >= beta || bestValue == 1)
        //            {
        //                //Console.WriteLine("Tree cutted at depth {0} by alpha beya pruning!", depthLevel);
        //                //break;
        //            }
        //        }
        //        else
        //        {
        //            bestValue = Math.Min(bestValue, currentMoveValue);
        //            beta = Math.Min(beta, currentMoveValue);
        //            if (bestValue <= alpha || bestValue == -1)
        //            {
        //                //Console.WriteLine("Tree cut at depth {0} by alpha beya pruning!", depthLevel);
        //                //break;
        //            }

        //        }
        //    }
        //    return bestValue;


        //}

        //private bool TimeIsAboutToEnd()
        //{
        //    if ((_timesup - stopWatch.Elapsed).TotalMilliseconds < 10)
        //        return true;
        //    else
        //        return false;
        //}

        //private void StartStopWatch()
        //{
        //    stopWatch.Restart();
        //}
        //private void PrintTimeElapsed()
        //{
        //    TimeSpan ts = stopWatch.Elapsed;
        //    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        //  ts.Hours, ts.Minutes, ts.Seconds,
        //  ts.Milliseconds / 10);
        //    Console.WriteLine("RunTime " + elapsedTime);
        //}

        //public enum Turn { MaxPlayer_ME, MinPlayer_Opponent };


        //private class BoardOutlines
        //{
        //    public int _rows;
        //    public int _cols;
        //    public int _mostBottomRow = 0;
        //    public char[,] _board;

        //    public BoardOutlines(Board board)
        //    {
        //        _rows = board._rows;
        //        _cols = board._cols;
        //        _board = board._board;
        //    }



        //    public BoardOutlines(BoardOutlines boardOutlines, Tuple<int, int> move)
        //    {
        //        _rows = boardOutlines._rows;
        //        _cols = boardOutlines._cols;
        //        _board = boardOutlines._board;

        //        for (int i = 0; i < _rows; i++)
        //        {
        //            for (int j = 1; j < _cols; j++)
        //            {
        //                if (i>= move.Item1)
        //                {
        //                    if (j >= move.Item2)
        //                        _board[i, j] = ' ';
        //                }
        //            }
        //        }

        //    }




        //    public Tuple<int, int> GetMoveForLosingState()
        //    {
        //        Tuple<int, int> move = null;
        //        for (int i = _rows-1 ; i > 0; i--)
        //        {
        //            for (int j = 1; j < _cols; j--)
        //            {
        //                if (_board[i, j] == 'X')
        //                    return new Tuple<int, int>(i, j);
        //            }
        //        }
        //        return move;
        //    }

        //    internal bool OnlyPoisonedSquareLeft()
        //    {
        //        return _board[1, 0] != 'X' && _board[1, 0] != 'X';
        //    }

        //    internal bool TwoSameLengthStripes()
        //    {
        //        for (int i = 1; i < _rows; i++)
        //        {
        //            for (int j = 1; j < _cols; j++)
        //            {
        //                if (_board[i, j] == 'X')
        //                {
        //                    return false;
        //                }
        //            }
        //        }

        //        return true;
        //    }

        //    internal bool TwoSameLengthRows()
        //    {

        //        //if (RightmostAvailabeSquareAtRow.ContainsKey(1) && RightmostAvailabeSquareAtRow[0] == RightmostAvailabeSquareAtRow[1] + 1 && _mostBottomRow == 1)
        //        //    return true;
        //        //else
        //            return false;
        //    }

        //    internal bool TwoSameLengthCols()
        //    {
        //        //if (RightmostAvailabeSquareAtRow[0] == 1 && _mostBottomRow > 0 && RightmostAvailabeSquareAtRow[_mostBottomRow - 1] == 1 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0)
        //        //    return true;
        //        return false;
        //    }


        //    internal bool OneRow()
        //    {
        //        return _board[1,0]!='X';
        //    }

        //    internal bool OneCol()
        //    {
        //        return _board[0, 1] != 'X' && _board[1, 1] == 'X';
        //    }

        //    internal bool Square()
        //    {
        //        int numInR = 0;
        //        for (int x = 0; x < board._rows; x++)
        //        {
        //            if (board._board[x, 0] == 'X')
        //            {
        //                numInR++;
        //            }
        //        }
        //        int numCul = 0;
        //        for (int y = 0; y < board._cols; y++)
        //        {
        //            if (board._board[0, y] == 'X')
        //            {
        //                numCul++;
        //            }
        //        }


        //        if (numCul == numInR)
        //        {

        //            for (int i = 0; i < numInR; i++)
        //            {
        //                for (int j = 0; j < numCul; j++)
        //                {
        //                    if (board._board[i, j] != 'X')
        //                    {
        //                        return false;
        //                    }
        //                }
        //            }


        //        }
        //        else return false;
        //        return true;
        //    }
        //    internal Stack<Tuple<int, int>> GetAllPossibleMoves()
        //    {
        //        Stack<Tuple<int, int>> allPossibleMoves = new Stack<Tuple<int, int>>();
        //        if (RightmostAvailabeSquareAtRow[0] == 0 && _mostBottomRow == 0)
        //            return allPossibleMoves;
        //        Tuple<int, int> possibleMove = new Tuple<int, int>(_mostBottomRow, (int)RightmostAvailabeSquareAtRow[_mostBottomRow]);
        //        allPossibleMoves.Push(possibleMove);
        //        while (true)
        //        {
        //            if (possibleMove.Item2 > 0)
        //                possibleMove = new Tuple<int, int>(possibleMove.Item1, possibleMove.Item2 - 1);
        //            else
        //                possibleMove = new Tuple<int, int>(possibleMove.Item1 - 1, (int)RightmostAvailabeSquareAtRow[possibleMove.Item1 - 1]);
        //            if (possibleMove.Item1 != 0 || possibleMove.Item2 != 0)
        //                allPossibleMoves.Push(possibleMove);
        //            else
        //                break;

        //        }
        //        return allPossibleMoves;
        //    }
        //    public override string ToString()
        //    {
        //        string result = "|";
        //        for (int row = 0; row <= _mostBottomRow; row++)
        //        {
        //            result += RightmostAvailabeSquareAtRow[row] + "|";
        //        }
        //        return result;

        //    }

        //}
    }
}