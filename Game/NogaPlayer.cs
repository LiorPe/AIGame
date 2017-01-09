using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class NogaPlayer
    {
        Board board;
        int[] emptyCells;
        int[,] scoresBoard;
        int noneEmptyCols;

        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            player1_1 = "203573647";  //id1
            player1_2 = "204326557";  //id2
        }
        public Tuple<int, int> playYourTurn(Board board, TimeSpan timesup)
        {
            Stopwatch timer = new Stopwatch();
            timer.Restart();
            Tuple<int, int> toReturn = null;
            this.board = board;
            scoresBoard = new int[board._rows, board._cols];
            emptyCells = EmptySquaresInBoard();

            int randomRow = 0;
            int randomCol = 0;
            if (emptyCells[0] == 1 && emptyCells[1] == 0) //finished game
            {
                toReturn = new Tuple<int, int>(randomRow, randomCol);
                return toReturn;
            }
            int[] yarivEmptyCells = new int[emptyCells.Length];
            if (VictoryStates(out toReturn, emptyCells))
                return toReturn;
            if (AlmostVictoryStates(out toReturn, emptyCells))
                return toReturn;

            Tuple<int, int> temp;
            //Console.WriteLine("printing original array:");
            //PrintEmptyCells(emptyCells);
            bool isGoodMove = true;
            HashSet<Tuple<int, int>> set = new HashSet<Tuple<int, int>>();
            set.Add(new Tuple<int, int>(0, 0));
            set.Add(new Tuple<int, int>(1, 0));
            set.Add(new Tuple<int, int>(0, 1));

            HashSet<Tuple<int, int>> noLoseSet = new HashSet<Tuple<int, int>>();

            Random random = new Random();
            do
            {
                //hash table so we wont randomaize the same move again - done
                //while until times up
                if (timer.ElapsedMilliseconds + 10 >= timesup.TotalMilliseconds) //no enough time
                {
                    if (noLoseSet.Count > 0)
                        return noLoseSet.First<Tuple<int, int>>();
                    else
                        return new Tuple<int, int>(0, 0);
                }
                isGoodMove = true;
                randomRow = random.Next(0, emptyCells[0] - 1);
                randomCol = random.Next(0, noneEmptyCols - 1);
                if (board._board[randomRow, randomCol] != 'X')
                    continue;
                toReturn = new Tuple<int, int>(randomRow, randomCol);
                if (LoseStates(emptyCells))
                    break;
                if (set.Contains(toReturn))
                {
                    isGoodMove = false;
                    continue;
                }
                else
                    set.Add(toReturn);
                yarivEmptyCells = SimulateMove(toReturn);
                //Console.WriteLine("move is: (" + toReturn.Item1 + "," + toReturn.Item2 + ")");
                //Console.WriteLine("printing simulated array:");
                //PrintEmptyCells(tempEmptyCells);
                if (VictoryStates(out temp, yarivEmptyCells) || AlmostVictoryStates(out temp, yarivEmptyCells)) //bad move for us
                {
                    isGoodMove = false;
                }
                else if (LoseStates(yarivEmptyCells)) //good move for us
                {
                    break;
                }
                else //neutral move for us, maybe we can find better
                {
                    isGoodMove = false;
                    noLoseSet.Add(toReturn);
                }
            }
            while (board._board[randomRow, randomCol] != 'X' || !isGoodMove);
            //(VictoryStates(out temp, yarivEmptyCells) || AlmostVictoryStates(out temp, yarivEmptyCells)));
            return toReturn;
        }
        //x,y :: row,col
        private int[] SimulateMove(Tuple<int, int> move)
        {
            int[] newEmptyCells = new int[emptyCells.Length];

            for (int i = 0; i < newEmptyCells.Length; i++)
            {
                if (i >= move.Item2)
                {
                    if (move.Item1 <= emptyCells[i] - 1)
                    {
                        newEmptyCells[i] = move.Item1;
                    }
                    else
                    {
                        newEmptyCells[i] = emptyCells[i];
                    }
                }
                else
                {
                    newEmptyCells[i] = emptyCells[i];
                }

            }

            return newEmptyCells;
        }

        private int[] EmptySquaresInBoard()
        {
            int[] array = new int[board._cols];
            int rowCounter;
            int i;
            for (i = 0; i < board._cols; i++)
            {
                rowCounter = 0;
                while (rowCounter < board._rows && board._board[rowCounter, i] == 'X')
                {
                    rowCounter++;
                }
                if (rowCounter == 0)
                    break;
                array[i] = rowCounter;
            }
            noneEmptyCols = i - 1;
            return array;
        }

        private bool AlmostVictoryStates(out Tuple<int, int> loc, int[] currentEmptyCells)
        {

            // 2*X
            int i;
            bool isWinner = true;
            if (currentEmptyCells[0] == 2 && currentEmptyCells[1] == 2)
            {
                for (i = 2; i < currentEmptyCells.Length; i++)
                {
                    if (currentEmptyCells[i] != 2 && currentEmptyCells[i] != 0)
                    {
                        isWinner = false;
                        break;
                    }
                }
                if (isWinner)
                {
                    loc = new Tuple<int, int>(1, i - 1);
                    return true;
                }
                else if (i < currentEmptyCells.Length - 1)
                {
                    try
                    {

                        if (currentEmptyCells[i] == 1 && currentEmptyCells[i + 1] == 1)
                        {
                            loc = new Tuple<int, int>(0, currentEmptyCells[i + 1]);
                        }
                    }
                    catch { }
                }

            }

            //X*2
            if (currentEmptyCells[0] > 1 && currentEmptyCells[0] == currentEmptyCells[1] && currentEmptyCells[2] == 0)
            {
                scoresBoard[currentEmptyCells[0] - 1, 1] = 100;
                loc = new Tuple<int, int>(currentEmptyCells[0] - 1, 1);
                return true;
            }
            else if (currentEmptyCells[0] > 1 && currentEmptyCells[1] > 1 && currentEmptyCells[2] == 0)
            {
                if (currentEmptyCells[0] - currentEmptyCells[1] >= 2)
                {
                    loc = new Tuple<int, int>(currentEmptyCells[1] + 1, 0);
                    return true;
                }
                else
                {
                    loc = new Tuple<int, int>(-1, -1);
                    return false;
                }
            }

            int counter = 1, j;
            //unequal reish
            if (currentEmptyCells[0] > 1 && currentEmptyCells[1] == 1)
            {
                for (j = 2; j < currentEmptyCells.Length; j++)
                {
                    if (currentEmptyCells[j] != 1)
                    {
                        break;
                    }
                    counter++;
                }
                //cols > row 
                if (counter > currentEmptyCells[0])
                {
                    loc = new Tuple<int, int>(0, currentEmptyCells[0]);
                    scoresBoard[0, counter] = 100;
                    return true;
                }
                if (counter < currentEmptyCells[0])
                {
                    scoresBoard[currentEmptyCells[0] - 1, 0] = 100;
                    loc = new Tuple<int, int>(counter + 1, 0);
                    return true;
                }
            }
            //square
            if (currentEmptyCells[0] == noneEmptyCols && currentEmptyCells[0] > 1)
            {
                for (i = 1; i < currentEmptyCells[0]; i++)
                {
                    if (currentEmptyCells[i] != currentEmptyCells[0])
                    {
                        loc = new Tuple<int, int>(-1, -1);
                        return false;
                    }
                }
                if (currentEmptyCells.Length == i || currentEmptyCells[i] == 0)
                {
                    scoresBoard[1, 1] = 100;
                    loc = new Tuple<int, int>(1, 1);
                    return true;
                }
            }

            loc = new Tuple<int, int>(-1, -1);
            return false;
        }

        // all the states that with a single move get us a win
        private bool VictoryStates(out Tuple<int, int> loc, int[] tempCells)
        {
            bool isWinning = true;
            if (tempCells[0] == 2 && tempCells[1] == 2) //little square
            {
                if (tempCells[2] == 0)
                {
                    scoresBoard[1, 1] = 100;
                    loc = new Tuple<int, int>(1, 1);
                    return true;
                }
            }

            if (tempCells[0] > 1 && tempCells[1] == 0) //is one column
            {
                scoresBoard[1, 0] = 100;
                loc = new Tuple<int, int>(1, 0);
                return true;
            }
            if (tempCells[0] == 1 && tempCells[1] == 1) //is one row
            {
                if (isWinning)
                {
                    scoresBoard[0, 1] = 100;
                    loc = new Tuple<int, int>(0, 1);
                    return true;
                }
            }
            loc = new Tuple<int, int>(-1, -1);
            return false;
        }

        private bool LoseStates(int[] emptyCellsSimulated)
        {
            //2,1,0... little reish
            if (emptyCellsSimulated[0] == 2 && emptyCellsSimulated[1] == 1 && emptyCellsSimulated[2] == 0)
                return true;
            // 2,2,1,0... ***/** 
            if (emptyCellsSimulated[0] == 2 && emptyCellsSimulated[1] == 2 && emptyCellsSimulated[2] == 1 && emptyCellsSimulated[3] == 0)
                return true;

            //3,2,0... ***/**
            if (emptyCellsSimulated[0] == 3 && emptyCellsSimulated[1] == 2 && emptyCellsSimulated[2] == 0)
                return true;

            //big reish
            if (emptyCellsSimulated[0] > 1)
            {
                int i;
                for (i = 1; i < emptyCellsSimulated[0]; i++)
                {
                    if (emptyCellsSimulated[i] != 1)
                        return false;
                }
                if (emptyCellsSimulated.Length == i || emptyCellsSimulated[i] == 0)
                    return true;
            }

            return false;
        }

        //change the array
        private void PrintEmptyCells(int[] array)
        {
            int[] newArr = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newArr[i] = array[i];
            }
            for (int i = 0; i < board._rows; i++)
            {
                for (int j = 0; j < board._cols; j++)
                {
                    if (newArr[j] > 0)
                    {
                        Console.Write("X");
                        Console.Write(" | ");
                        newArr[j]--;
                    }
                    else
                    {
                        Console.Write(" ");
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }
}
