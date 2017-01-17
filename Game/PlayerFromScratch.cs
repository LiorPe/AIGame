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
            int maxDepthLevel = 7;
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
            //if (bestValue == -1)
            //    FindBestMove(boardOutline, out value);
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
                //if (nextPlayerTurn == Turn.MaxPlayer_ME && maxPlayerBoardsValues.ContainsKey(boardAfterMove.Key))
                //    currentValue = maxPlayerBoardsValues[boardAfterMove.Key];
                //else if (turn == Turn.MinPlayer_Opponent && minPlayerBoardsValues.ContainsKey(boardAfterMove.Key))
                //    currentValue= minPlayerBoardsValues[boardAfterMove.Key];
                //else
                //{
                    currentValue = CalculateMoveValue(boardAfterMove, depthLevel + 1, maxDepthLevel, nextPlayerTurn, alpha, beta);
                    //if (nextPlayerTurn == Turn.MaxPlayer_ME && !maxPlayerBoardsValues.ContainsKey(boardAfterMove.Key))
                    //{
                    //    maxPlayerBoardsValues[boardOutline.Key] = currentValue;
                    //    //minPlayerBoardsValues[boardOutline.Key] = result.Item2  * -1;
                    //}
                    //if (nextPlayerTurn == Turn.MinPlayer_Opponent && !minPlayerBoardsValues.ContainsKey(boardAfterMove.Key))
                    //{
                    //    //maxPlayerBoardsValues[boardOutline.Key] = result.Item2*-1;
                    //    minPlayerBoardsValues[boardOutline.Key] = currentValue;
                //    //}
                //}

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
            //if (turn == Turn.MaxPlayer_ME && bestValue<0)
            //{
            //    Console.WriteLine("loosing situation for me on depth {0}", depthLevel);
            //    boardOutline._board.printTheBoard();
            //    CalculateMoveValue(boardOutline, depthLevel, maxDepthLevel, turn, alpha, beta);
            //}
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
            
            public BoardOutlines(Board board)
            {
                _board = new Board(board);
                Init();
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
                    int currentCol = r.Next(0, _board._rows - 1);
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
                int rowLength=0;
                for (; rowLength < _board._cols && _board._board[0, rowLength] == 'X'; rowLength++) ;
                int colLength = 0;
                for (; colLength < _board._rows && _board._board[colLength, 0] == 'X'; colLength++) ;
                if (colLength != rowLength || rowLength==0 )
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
