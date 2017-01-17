using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PlayerFromScratch
    {
        Stopwatch stopWatch = new Stopwatch();
        TimeSpan _timesup;
        List<string> log = new List<string>();
        //Dictionary<string, BoardSituation> memory = new Dictionary<string, BoardSituation>();
        Dictionary<string, bool> LeadingToWinSituations = new Dictionary<string, bool>();
        Dictionary<string, bool> LeadingToLossSituations = new Dictionary<string, bool>();
        Dictionary<string, bool> CertainLossSituations = new Dictionary<string, bool>();
        Dictionary<string,  int> maxPlayerBoardsValues = new Dictionary<string, int> ();
        Dictionary<string, int> minPlayerBoardsValues = new Dictionary<string, int>();



        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "203722814";  //id1
            player1_2 = "308111160";  //id2

        }
        public enum Turn { MaxPlayer_ME, MinPlayer_Opponent};
        public Tuple<int, int> playYourTurn
        (
            Board board,
            TimeSpan timesup
        )
        {
            stopWatch.Start();
            _timesup = timesup;
            Tuple<int, int> chosenMove = null;
            BoardOutlines boardOutline = new BoardOutlines(board);
            int value;
            chosenMove = FindBestMove(boardOutline , out value);
            if (chosenMove == null)
                chosenMove = boardOutline.GetMoveForLosingState();
            Console.WriteLine("{0} ms left, value calculated: {1}", (_timesup - stopWatch.Elapsed).TotalMilliseconds, value);

                 return chosenMove;
        }

        private Tuple<int, int> FindBestMove(BoardOutlines boardOutline, out int value)
        {
            Stack<Tuple<int, int>> allPssobleMoves = boardOutline.GetAllPossibleMoves();
            int bestValue  = int.MinValue;
            Tuple<int, int> bestMove = null;
            int currentValue;
            Tuple<int, int> currentMove = null;
            int totalMovesToCheck = boardOutline.NumOfPossibleMoves;
            int movesChecked = 0;
            // to win on 7X5 - depthlevel = 6
            int maxDepthLevel = 6;
            int alpha = int.MinValue;
            BoardOutlines boardAfterMyMove;
            while (!TimeIsAboutToEnd() && allPssobleMoves.Count>0)
            {
                maxDepthLevel = UpdateMAxDepthLevel(totalMovesToCheck, movesChecked, maxDepthLevel);
                currentMove = allPssobleMoves.Pop();
                boardAfterMyMove = new BoardOutlines(boardOutline, currentMove);
                //if (currentMove.Item1 > 0 && currentMove.Item2 > 0)
                //    alpha = alpha;
                currentValue = CalculateMoveValue(boardAfterMyMove, 0, maxDepthLevel, Turn.MinPlayer_Opponent, alpha, Int32.MaxValue);
                alpha = Math.Max(alpha, currentValue);
                if (currentValue>=bestValue)
                {
                    bestValue = currentValue;
                    bestMove = currentMove;
                }
                if (bestValue >= 1)
                    break;
            }
            value = bestValue;
            return bestMove;
        }

        //

        private int CalculateMoveValue(BoardOutlines boardOutline, int depthLevel, int maxDepthLevel, Turn turn, int alpha, int beta)
        {



            if (CretainLoss(boardOutline))
            {
                if (turn == Turn.MaxPlayer_ME)
                {
                    return Int32.MinValue;
                }
                else
                {
                    return Int32.MaxValue;
                }
            }
            else if (LeadingToWin(boardOutline))
            {
                if (turn == Turn.MaxPlayer_ME)
                {
                    return 1;

                }
                else
                {
                    return -1;

                }
            }
            else if (LeadingToLoss(boardOutline))
            {
                if (turn == Turn.MaxPlayer_ME)
                {
                    return -1;

                }
                else
                {
                    return 1;
                }
            }
            else if (depthLevel == maxDepthLevel)
            {
                return 0;

            }

            Stack<Tuple<int, int>> movesToCheck = boardOutline.GetAllPossibleMoves();
            int bestValue;
            Tuple<int, int> bestMove = null;
            int currentValue;
            Tuple<int, int> currentMove=null;
            Turn nextPlayerTurn;
            BoardOutlines boardAfterMove = null;
            if (turn == Turn.MaxPlayer_ME)
            {
                bestValue = int.MinValue;
                nextPlayerTurn = Turn.MinPlayer_Opponent;
            }
            else
            {
                bestValue = int.MaxValue;
                nextPlayerTurn = Turn.MaxPlayer_ME;
            }
            while (movesToCheck.Count > 0 )
            {   if (TimeIsAboutToEnd())
                {
                    if (turn == Turn.MaxPlayer_ME)
                        return 0;
                    else
                        return 0;


                }
                currentMove = movesToCheck.Pop();
                boardAfterMove = new BoardOutlines(boardOutline, currentMove);
                if (turn == Turn.MaxPlayer_ME && maxPlayerBoardsValues.ContainsKey(boardAfterMove.Key))
                    currentValue = maxPlayerBoardsValues[boardAfterMove.Key];
                else if (turn == Turn.MinPlayer_Opponent && minPlayerBoardsValues.ContainsKey(boardAfterMove.Key))
                    currentValue =  minPlayerBoardsValues[boardAfterMove.Key];
                else
                {
                    currentValue = CalculateMoveValue(boardAfterMove, depthLevel + 1, maxDepthLevel, nextPlayerTurn, alpha, beta);
                    if (turn == Turn.MaxPlayer_ME && currentValue>=1)
                        maxPlayerBoardsValues[boardAfterMove.Key] = currentValue;
                    else if (turn == Turn.MinPlayer_Opponent && currentValue <= -1)
                        minPlayerBoardsValues[boardAfterMove.Key] = currentValue;
                }
                if (turn == Turn.MaxPlayer_ME)
                {
                    if (currentValue > bestValue)
                    {
                        bestValue = currentValue;
                        bestMove = currentMove;
                    }
                    alpha = Math.Max(alpha, bestValue);
                    if (bestValue >= beta || bestValue >= 1)
                        break;
                }
                else
                {

                    if (currentValue < bestValue)
                    {
                        bestValue = currentValue;
                        bestMove = currentMove;
                    }
                    beta = Math.Min(beta, bestValue);
                    if (bestValue <= alpha || bestValue <= -1)
                        break;

                }
            }


            return bestValue;
        }

        private bool LeadingToLoss(BoardOutlines boardOutline)
        {
            if (LeadingToLossSituations.ContainsKey(boardOutline.Key))
                    return LeadingToLossSituations[boardOutline.Key];
            bool result = boardOutline.LeadingToLoss();
            LeadingToLossSituations[boardOutline.Key] = result;
            return result;
        }


        private bool LeadingToWin(BoardOutlines boardOutline)
        {
            if (LeadingToWinSituations.ContainsKey(boardOutline.Key))
                return LeadingToWinSituations[boardOutline.Key];
            bool result = boardOutline.Square() || boardOutline.OneRow() || boardOutline.OneCol();
            LeadingToWinSituations[boardOutline.Key] = result;
            return result;
        }

        private bool CretainLoss(BoardOutlines boardOutline)
        {
            if (CertainLossSituations.ContainsKey(boardOutline.Key))
                return CertainLossSituations[boardOutline.Key];
            bool result = boardOutline.OnlyPoisonedSquareLeft();
            CertainLossSituations[boardOutline.Key] = result;
            return result;
        }

        private bool TimeIsAboutToEnd()
        {
            if ((_timesup - stopWatch.Elapsed).TotalMilliseconds < 10)
                return true;
            else
                return false;
        }

        private int UpdateMAxDepthLevel(int totalMovesToCheck, int movesChecked, int maxDepthLevel)
        {
            return maxDepthLevel;
        }

        internal class BoardOutlines
        {
            internal Board _board;
            public int NumOfPossibleMoves;
            public string Key;
            Stack<Tuple<int, int>> _allPossibleMoves = new Stack<Tuple<int, int>>();
            public int _rows;
            public int _cols;
            public int _mostBottomRow = 0;
            public Dictionary<int, int> RightmostAvailabeSquareAtRow;

            public BoardOutlines(Board board)
            {
                _board = new Board(board);
                Init();
                NewInit(board);
            }

            private void NewInit(Board board)
            {
                _rows = board._rows;
                _cols = board._cols;
                int? rightMostSquareInCurrentRow;
                RightmostAvailabeSquareAtRow = new Dictionary<int, int>();
                int row = 0;
                for (; row < board._rows; row++)
                {
                    rightMostSquareInCurrentRow = FindMostRightSquare(board._board, row, _cols);
                    if (rightMostSquareInCurrentRow != null)
                        RightmostAvailabeSquareAtRow[row] = (int)rightMostSquareInCurrentRow;
                    else
                        break;
                }
                _mostBottomRow = Math.Max(0, row - 1);
            }

            private int? FindMostRightSquare(char[,] _board, int row, int rowLength)
            {
                if (_board[row, 0] != 'X')
                    return null;
                if (rowLength == 1 || _board[row, rowLength - 1] == 'X')
                    return rowLength - 1;
                int l = 0;
                int r = rowLength - 1;
                int m;
                while (l <= r)
                {
                    m = (l + r) / 2;
                    if (_board[row, m] == 'X' && _board[row, m + 1] != 'X')
                        return m;
                    if (_board[row, m] == 'X')
                        l = m + 1;
                    else
                        r = m - 1;

                }
                return FindMostRightSquare(_board, row, rowLength);
            }

            private void Init()
            {
                Key = String.Empty;
                for (int row = 0; row < _board._rows; row++)
                {
                    for (int col = 0; col < _board._cols; col++)
                    {
                        Key += _board._board[row, col];
                        if (_board._board[row, col] == 'X' && (row > 0 || col > 0))
                        {
                            _allPossibleMoves.Push(new Tuple<int, int>(row, col));
                        }
                    }
                    Key += "|";
                }
                
                NumOfPossibleMoves = _allPossibleMoves.Count;
            }

            public BoardOutlines(BoardOutlines boardOutlines, Tuple<int, int> move)
            {
                _board = new Board(boardOutlines._board);
                _board.fillPlayerMove(move.Item1, move.Item2);
                Init();
                NewFlipSquaure(boardOutlines, move);

            }

            private void NewFlipSquaure(BoardOutlines boardOutlines, Tuple<int, int> move)
            {
                _rows = boardOutlines._rows;
                _cols = boardOutlines._cols;
                RightmostAvailabeSquareAtRow = new Dictionary<int, int>();
                int row = 0;
                for (; row < _rows && row < move.Item1; row++)
                    RightmostAvailabeSquareAtRow[row] = boardOutlines.RightmostAvailabeSquareAtRow[row];
                for (; row < _rows; row++)
                {
                    if (move.Item2 == 0 || !boardOutlines.RightmostAvailabeSquareAtRow.ContainsKey(row))
                    {
                        break;
                    }
                    else
                        RightmostAvailabeSquareAtRow[row] = Math.Min(move.Item2 - 1, boardOutlines.RightmostAvailabeSquareAtRow[row]);
                }
                _mostBottomRow = Math.Max(0, row - 1);
            }

            public Tuple<int, int> GetMoveForLosingState()
            {
                int row = 0;
                int col = 0;
                Console.WriteLine("Random");
                for (int i = 0; i < 100; i++) 
                {
                    Random r = new Random();
                    int currentRow = r.Next(0, _board._rows - 1);
                    int currentCol = r.Next(0, _board._cols - 1);
                    if (_board._board[currentRow, currentCol] == 'X' && (currentRow > 0 || currentCol > 0))
                    {
                        if (currentCol + currentRow > row + col)
                        {
                            row = currentRow;
                            col = currentCol;
                        }

                    }
                }
                return new Tuple<int, int>(row, col);

            }

            internal bool OnlyPoisonedSquareLeft()
            {
                bool goodResult = OldOnlyPoisonedSquareLeft();
                bool newResult = NewOnlyPoisonedSquareLeft();
                if (goodResult != newResult)
                    throw new Exception();
                return newResult;

            }

            private bool NewOnlyPoisonedSquareLeft()
            {
                return _mostBottomRow == 0 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0;
            }

            private bool OldOnlyPoisonedSquareLeft()
            {
                for (int row = 0; row < _board._rows; row++)
                {
                    for (int col = 0; col < _board._cols; col++)
                    {
                        if ((row > 0 || col > 0) && _board._board[row, col] == 'X')
                            return false;
                    }
                }
                return true;
            }

            internal bool Square()
            {
                bool goodResult = OldSquare();
                bool newResult = NewSquare();
                if (goodResult != newResult)
                    throw new Exception();
                return newResult;

            }

            private bool NewSquare()
            {
                if (_mostBottomRow == RightmostAvailabeSquareAtRow[0] && RightmostAvailabeSquareAtRow[_mostBottomRow] == RightmostAvailabeSquareAtRow[0])
                    return true;
                return false;
            }

            private bool OldSquare()
            {
                int row = 0;
                int squareLength = -1;
                for (; row < _board._rows; row++)
                {
                    if (_board._board[row, 0] != 'X')
                        break;
                    int currentRowLength = 0;
                    for (int col = 0; col < _board._cols; col++)
                    {
                        if (_board._board[row, col] == 'X')
                            currentRowLength++;
                        else
                            break;
                    }
                    if (squareLength != -1 && currentRowLength != squareLength)
                        return false;
                    else
                        squareLength = currentRowLength;
                }
                if (row != squareLength)
                    return false;
                return true;
            }

            internal Stack<Tuple<int, int>> GetAllPossibleMoves()
            {
                return new Stack<Tuple<int, int>>( _allPossibleMoves);

            }

            internal bool OneRow()
            {
                bool goodResult = OldOneRow();
                bool newResult = NewOneRow();
                if (goodResult != newResult)
                    throw new Exception();
                return newResult;

            }

            private bool NewOneRow()
            {
                return _mostBottomRow == 0 && RightmostAvailabeSquareAtRow[_mostBottomRow] > 0;
            }

            private bool OldOneRow()
            {

                for (int row = 1; row < _board._rows; row++)
                {
                    for (int col = 0; col < _board._cols; col++)
                    {
                        if (_board._board[row, col] == 'X')
                            return false;
                    }
                }
                return true;
            }

            internal bool OneCol()
            {
                bool goodResult = OldOneCol();
                bool newResult = NewOneCol();
                if (goodResult != newResult)
                    throw new Exception();
                return newResult;

            }

            private bool NewOneCol()
            {
                return RightmostAvailabeSquareAtRow[0] == 0 && _mostBottomRow > 0;
            }

            private bool OldOneCol()
            {
                for (int row = 0; row < _board._rows; row++)
                {
                    for (int col = 1; col < _board._cols; col++)
                    {
                        if (_board._board[row, col] == 'X')
                            return false;
                    }
                }
                return true;
            }

            internal bool SamzeSizeOneRowAndOneCol()
            {
                bool goodResult = OldSamzeSizeOneRowAndOneCol();
                bool newResult = NewSamzeSizeOneRowAndOneCol();
                if (goodResult != newResult)
                    throw new Exception();
                return newResult;

            }

            private bool NewSamzeSizeOneRowAndOneCol()
            {
                return RightmostAvailabeSquareAtRow[0] == _mostBottomRow && RightmostAvailabeSquareAtRow.ContainsKey(1) && RightmostAvailabeSquareAtRow[1] == 0;
            }

            private bool OldSamzeSizeOneRowAndOneCol()
            {

                int rowLength = 0;
                for (; rowLength < _board._cols && _board._board[0, rowLength] == 'X'; rowLength++) ;
                int colLength = 0;
                for (; colLength < _board._rows && _board._board[colLength, 0] == 'X'; colLength++) ;
                if (colLength != rowLength || rowLength == 0)
                    return false;
                for (int row = 1; row < _board._rows; row++)
                {
                    for (int col = 1; col < _board._cols; col++)
                    {
                        if (_board._board[row, col] == 'X')
                            return false;
                    }
                }
                return true;
            }

            internal bool TwoCols()
            {
                bool goodResult = OldTwoCols();
                bool newResult = NewTwoCols();
                if (goodResult != newResult)
                    throw new Exception();
                return newResult;

            }

            private bool NewTwoCols()
            {
                if (RightmostAvailabeSquareAtRow[0] == 1 && _mostBottomRow > 0 && RightmostAvailabeSquareAtRow[_mostBottomRow - 1] == 1 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0)
                    return true;
                return false;
            }

            private bool OldTwoCols()
            {
                int firstColLength = 0;
                for (; firstColLength < _board._rows && _board._board[firstColLength, 0] == 'X'; firstColLength++) ;
                int secondColLength = 0;
                for (; secondColLength < _board._rows && _board._board[secondColLength, 1] == 'X'; secondColLength++) ;
                if (firstColLength != secondColLength + 1)
                    return false;
                for (int row = 0; row < _board._rows; row++)
                {
                    for (int col = 2; col < _board._cols; col++)
                    {
                        if (_board._board[row, col] == 'X')
                            return false;
                    }
                }
                return true;
            }

            internal bool TwoRows()
            {
                bool goodResult = OldTwoRows();
                bool newResult = NewTwoRows();
                if (goodResult != newResult)
                    throw new Exception();
                return newResult;


            }

            private bool NewTwoRows()
            {
                if (RightmostAvailabeSquareAtRow.ContainsKey(1) && RightmostAvailabeSquareAtRow[0] == RightmostAvailabeSquareAtRow[1] + 1 && _mostBottomRow == 1)
                    return true;
                else
                    return false;
            }

            private bool OldTwoRows()
            {

                int firstRowLength = 0;
                for (; firstRowLength < _board._cols && _board._board[0, firstRowLength] == 'X'; firstRowLength++) ;
                int secondRowLenth = 0;
                for (; secondRowLenth < _board._cols && _board._board[1, secondRowLenth] == 'X'; secondRowLenth++) ;
                if (firstRowLength != secondRowLenth + 1)
                    return false;
                for (int row = 2; row < _board._rows; row++)
                {
                    for (int col = 0; col < _board._cols; col++)
                    {
                        if (_board._board[row, col] == 'X')
                            return false;
                    }
                }
                return true;
            }

            internal bool LeadingToLoss()
            {
                return  SamzeSizeOneRowAndOneCol() || TwoCols() || TwoRows();
            }
        }
    }
}
