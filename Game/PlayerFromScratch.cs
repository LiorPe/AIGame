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
        Dictionary<string, int> maxPlayerBoardsValues = new Dictionary<string, int>();
        Dictionary<string, int> minPlayerBoardsValues = new Dictionary<string, int>();


        public enum BoardState { LeadingToWin, LeadingToLoss, CertainLoss, GoodState, Unknow };
        Dictionary<string, BoardState> boardsStates = new Dictionary<string, BoardState>();

        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "203722814";  //id1
            player1_2 = "308111160";  //id2

        }
        public enum Turn { MaxPlayer_ME, MinPlayer_Opponent };
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
            chosenMove = FindBestMove(boardOutline, out value);
            if (chosenMove == null)
                chosenMove = new Tuple<int, int>(0, 0);
            Console.WriteLine("{0} ms left, value calculated: {1}", (_timesup - stopWatch.Elapsed).TotalMilliseconds, value);

            return chosenMove;
        }

        private Tuple<int, int> FindBestMove(BoardOutlines boardOutline, out int value)
        {
            List<Tuple<int, int>> allPssobleMoves = boardOutline.GetAllPossibleMoves();
            int bestValue = int.MinValue;
            Tuple<int, int> bestMove = null;
            int currentValue;
            Tuple<int, int> currentMove = null;
            int totalMovesToCheck = boardOutline.NumOfPossibleMoves;
            int movesChecked = 0;
            // to win on 7X5 - depthlevel = 6
            int maxDepthLevel = 7;
            int alpha = int.MinValue;
            BoardOutlines boardAfterMyMove;
            for (int i = 0; !TimeIsAboutToEnd() && i < allPssobleMoves.Count; i++)
            {
                maxDepthLevel = UpdateMAxDepthLevel(totalMovesToCheck, movesChecked, maxDepthLevel);
                currentMove = allPssobleMoves.ElementAt(i);
                boardAfterMyMove = new BoardOutlines(boardOutline, currentMove);
                currentValue = CalculateMoveValue(boardAfterMyMove, 0, maxDepthLevel, Turn.MinPlayer_Opponent, alpha, Int32.MaxValue);
                alpha = Math.Max(alpha, currentValue);
                if (currentValue >= bestValue)
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
            if (turn == Turn.MaxPlayer_ME && maxPlayerBoardsValues.ContainsKey(boardOutline.Key))
                return maxPlayerBoardsValues[boardOutline.Key];
            else if (turn == Turn.MinPlayer_Opponent && minPlayerBoardsValues.ContainsKey(boardOutline.Key))
                return minPlayerBoardsValues[boardOutline.Key];

            int value;
            if (TryGetFinalValue(out value, turn, boardOutline))
                return value;
            else if (depthLevel == maxDepthLevel)
                return 0;
            List<Tuple<int, int>> movesToCheck = boardOutline.GetAllPossibleMoves();
            int bestValue;
            Tuple<int, int> bestMove = null;
            int currentValue;
            Tuple<int, int> currentMove = null;
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
            for (int i = 0; i < movesToCheck.Count; i++)
            {
                if (TimeIsAboutToEnd())
                {
                    return 0;
                }
                currentMove = movesToCheck.ElementAt(i);
                boardAfterMove = new BoardOutlines(boardOutline, currentMove);

                currentValue = CalculateMoveValue(boardAfterMove, depthLevel + 1, maxDepthLevel, nextPlayerTurn, alpha, beta);


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

            if (turn == Turn.MaxPlayer_ME && bestValue >= 1)
            {
                maxPlayerBoardsValues[boardOutline.Key] = bestValue;
                minPlayerBoardsValues[boardOutline.Key] = bestValue * -1;
            }
            else if (turn == Turn.MinPlayer_Opponent && bestValue <= -1)
            {
                maxPlayerBoardsValues[boardOutline.Key] = bestValue * -1;
                minPlayerBoardsValues[boardOutline.Key] = bestValue;
            }
            return bestValue;
        }

        private bool TryGetFinalValue(out int value, Turn turn, BoardOutlines boardOutline)
        {
            value = 0;
            string boardKey = boardOutline.Key;
            BoardState boardState;
            if (boardsStates.ContainsKey(boardKey))
            {
                boardState = boardsStates[boardKey];
                if (boardState == BoardState.Unknow)
                {
                    return false;
                }
                else
                {
                    if (boardState == BoardState.CertainLoss)
                    {
                        if (turn == Turn.MaxPlayer_ME)
                            value = -10;
                        else
                            value = 10;
                    }
                    else if (boardState == BoardState.LeadingToLoss)
                    {
                        if (turn == Turn.MaxPlayer_ME)
                            value = -1;
                        else
                            value = 1;
                    }
                    else if (boardState == BoardState.LeadingToWin)
                    {
                        if (turn == Turn.MaxPlayer_ME)
                            value = 1;
                        else
                            value = -1;
                    }
                    return true;
                }
            }
            else
            {
                if (boardOutline.OnlyPoisonedSquareLeft())
                {
                    if (turn == Turn.MaxPlayer_ME)
                        value = -10;
                    else
                        value = 10;
                    boardsStates[boardKey] = BoardState.CertainLoss;
                    return true;
                }

                else if (boardOutline.LeadingToLoss())
                {
                    if (turn == Turn.MaxPlayer_ME)
                        value = -1;
                    else
                        value = 1;
                    boardsStates[boardKey] = BoardState.LeadingToLoss;
                    return true;
                }
                else if (boardOutline.Square() || boardOutline.OneRow() || boardOutline.OneCol())
                {
                    if (turn == Turn.MaxPlayer_ME)
                        value = 1;
                    else
                        value = -1;
                    boardsStates[boardKey] = BoardState.LeadingToWin;
                    return true;
                }
                else
                {
                    boardsStates[boardKey] = BoardState.Unknow;
                    return false;
                }

            }

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
            public int NumOfPossibleMoves;
            public string Key;
            List<Tuple<int, int>> _allPossibleMoves = null;
            public int _rows;
            public int _cols;
            public int _mostBottomRow = 0;
            public Dictionary<int, int> RightmostAvailabeSquareAtRow;

            public BoardOutlines(Board board)
            {
                _rows = board._rows;
                _cols = board._cols;
                int? rightMostSquareInCurrentRow;
                RightmostAvailabeSquareAtRow = new Dictionary<int, int>();
                int row = 0;
                int rowLength;
                for (; row < board._rows; row++)
                {
                    rightMostSquareInCurrentRow = FindMostRightSquare(board._board, row, _cols);
                    if (rightMostSquareInCurrentRow != null)
                    {
                        rowLength = (int)rightMostSquareInCurrentRow;
                        RightmostAvailabeSquareAtRow[row] = rowLength;
                        Key += rowLength + "|";

                    }
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


            public BoardOutlines(BoardOutlines boardOutlines, Tuple<int, int> move)
            {
                _rows = boardOutlines._rows;
                _cols = boardOutlines._cols;
                RightmostAvailabeSquareAtRow = new Dictionary<int, int>();
                int row = 0;
                int rowLength;
                for (; row < _rows && row < move.Item1; row++)
                {
                    rowLength = boardOutlines.RightmostAvailabeSquareAtRow[row];
                    RightmostAvailabeSquareAtRow[row] = rowLength;
                    Key += rowLength + "|";
                }
                for (; row < _rows; row++)
                {
                    if (move.Item2 == 0 || !boardOutlines.RightmostAvailabeSquareAtRow.ContainsKey(row))
                    {
                        break;
                    }
                    else
                    {
                        rowLength = Math.Min(move.Item2 - 1, boardOutlines.RightmostAvailabeSquareAtRow[row]);
                        RightmostAvailabeSquareAtRow[row] = rowLength;
                        Key += rowLength + "|";
                    }
                }
                _mostBottomRow = Math.Max(0, row - 1);
            }

            private void FindAllPossiblMoves()
            {
                _allPossibleMoves = new List<Tuple<int, int>>();
                for (int row = 0; row <= _mostBottomRow; row++)
                {
                    int rowLength = RightmostAvailabeSquareAtRow[row];
                    for (int col = 0; col <= rowLength; col++)
                    {
                        if (row > 0 || col > 0)
                            _allPossibleMoves.Add(new Tuple<int, int>(row, col));
                    }
                }

                NumOfPossibleMoves = _allPossibleMoves.Count;

            }

            internal bool OnlyPoisonedSquareLeft()
            {
                return _mostBottomRow == 0 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0;
            }



            internal bool Square()
            {
                if (_mostBottomRow == RightmostAvailabeSquareAtRow[0] && RightmostAvailabeSquareAtRow[_mostBottomRow] == RightmostAvailabeSquareAtRow[0])
                    return true;
                return false;
            }

            internal List<Tuple<int, int>> GetAllPossibleMoves()
            {
                if (_allPossibleMoves == null)
                    FindAllPossiblMoves();
                return _allPossibleMoves;

            }

            internal bool OneRow()
            {
                return _mostBottomRow == 0 && RightmostAvailabeSquareAtRow[_mostBottomRow] > 0;


            }

            internal bool OneCol()
            {
                return RightmostAvailabeSquareAtRow[0] == 0 && _mostBottomRow > 0;
            }




            internal bool SamzeSizeOneRowAndOneCol()
            {
                return RightmostAvailabeSquareAtRow[0] == _mostBottomRow && RightmostAvailabeSquareAtRow.ContainsKey(1) && RightmostAvailabeSquareAtRow[1] == 0;
            }



            internal bool TwoCols()
            {
                if (RightmostAvailabeSquareAtRow[0] == 1 && _mostBottomRow > 0 && RightmostAvailabeSquareAtRow[_mostBottomRow - 1] == 1 && RightmostAvailabeSquareAtRow[_mostBottomRow] == 0)
                    return true;
                return false;
            }



            internal bool TwoRows()
            {
                if (RightmostAvailabeSquareAtRow.ContainsKey(1) && RightmostAvailabeSquareAtRow[0] == RightmostAvailabeSquareAtRow[1] + 1 && _mostBottomRow == 1)
                    return true;
                else
                    return false;
            }

            internal bool LeadingToLoss()
            {
                return SamzeSizeOneRowAndOneCol() || TwoCols() || TwoRows();
            }
            public override string ToString()
            {
                return Key;

            }
        }
    }
}
