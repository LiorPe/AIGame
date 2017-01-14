using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    internal class LeraPlayer3
    {

        Dictionary<char[,], space> resultDicPlayer1;

        Dictionary<char[,], space> resultDicPlayer2;
        Stopwatch stopwatch;
        public class space
        {
            public Tuple<int, int> place;
            public int rank;

            public space(Tuple<int, int> p, int r)
            {

                place = p;
                rank = r;

            }
        }


        public void getPlayers(ref string player1_1, ref string player1_2)  //fill players ids
        {
            // Player2.wt = new Dictionary<int, Dictionary<List<Tuple<int, int>>, Player2.wu>>();
            player1_1 = "123456789";  //id1
            player1_2 = "123456789";  //id2
        }

        public Tuple<int, int> playYourTurn
        (
            Board board,
            TimeSpan timesup
        )
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();




            //    while ((double)stopwatch.ElapsedMilliseconds < timesup.TotalMilliseconds - 10)
            //  {

            //}
            Tuple<int, int> toReturn = null;

            //if the board is square


            if (checkIfSquare(board))
            {
                toReturn = new Tuple<int, int>(1, 1);
                return toReturn;
            }


            //check if it is a 2*something



            /*
            if (checkIfReish(board))
            {
                int numR = 0;
                
                for (int x = 0; x < board._rows; x++)
                {
                    if (board._board[x, 0] == 'X')
                    {
                        numR++;
                    }
                }
                int numCul = 0;

                for (int y = 0; y < board._rows; y++)
                {
                    if (board._board[0, y] == 'X')
                    {
                        numCul++;
                    }
                }


                if (numR > numCul)
                {
                    toReturn = new Tuple<int, int>(numCul, 0);
                    return toReturn;


                }

                if (numCul > numR)
                {
                    toReturn = new Tuple<int, int>(0, numR);
                    return toReturn;


                }

            }
            */


            //end if the board is square
            resultDicPlayer1 = new Dictionary<char[,], space>();
            resultDicPlayer2 = new Dictionary<char[,], space>();


            /*
            //Thread newThread = new Thread(Work.DoWork);
            space res = null;
            Thread newThread = new Thread(() => { res = getBestMove2(board, 1); });
            newThread.Start();
            newThread.Join((int)timesup.TotalMilliseconds - 5);
            if (res!=null)
            {

                return res.place;
            }
            */
            //Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            /*

            var result = default(space);
            Action runTask = () => result = getBestMove2(board, 1);

            TimeSpan time = new TimeSpan(0, 0, 0, 0, (int)timesup.TotalMilliseconds - 5);
            var task = Task.Factory.StartNew(runTask);
            if (task.Wait(time))
            {
                Console.WriteLine("1"+stopwatch.ElapsedMilliseconds.ToString());
                return result.place;

            }
            Console.WriteLine("2" + stopwatch.ElapsedMilliseconds.ToString());

            */

            //Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            /*

            var task = Task.Run(() => getBestMove2(board, 1));
            if (task.Wait(TimeSpan.FromSeconds(timesup.TotalMilliseconds - 20)))
                return task.Result.place;
            else
            {
            */

             space res = getBestMove2(board, 1, timesup);
            // space res =GetBestMove(board, 1);

            if (res != null)
            {
                if (res.place.Item1==0&& res.place.Item2==0)
                {

                    return chooseTheLast(board);
                }
                else  return res.place;

            }

            return chooseTheLast(board);
            //Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
             
            /**/

            /*
            int randomRow;
            int randomCol;
            Random random = new Random();
            do
            {
                randomRow = random.Next(0, board._rows);
                randomCol = random.Next(0, board._cols);
            } while (board._board[randomRow, randomCol] != 'X' && check(board, randomRow, randomCol) == false); // == ' ');&& board.isTheGameEnded()
            toReturn = new Tuple<int, int>(randomRow, randomCol);
            //Random Algorithm - End
            //  Console.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
            return toReturn;
            */
        }
        //toReturn = new Tuple<int, int>(res.place.Item2, res.place.Item1);
        // Console.WriteLine(res.place);






        // }

        //System.Collections.Generic.LinkedList<string> st = new LinkedList<string>(); 

        //Tuple<int, int> toReturn = null;


        // bool stop = false;
        /*
        bool find = false;
       // int lastErase = 0;
        //Tuple<int, int> toReturn = null; 
        for (int x = 0; x < board._rows; x++)
        {
            for (int y = 1; y < board._cols; y++)
            {
                if (board.isLegalMove(x,y))
                {
                    Tuple<int, int> p = new Tuple<int, int>(x, y);
                    if (virtualMove(p, board) == true)

                    {
                        toReturn = new Tuple<int, int>(x, y); 
                       // lastErase = calcErase(p, board);
                        find = true;
                    }
                }
            }
        }

        if (find == true)
        {
            return toReturn;

        }

        else { 

    */
        //Random Algorithm - Start

        /*
        int randomRow;
            int randomCol;
            Random random = new Random();
            do
            {
                randomRow = random.Next(0, board._rows);
                randomCol = random.Next(0, board._cols);
            } while (board._board[randomRow, randomCol] != 'X'); // == ' ');&& board.isTheGameEnded()
            toReturn = new Tuple<int, int>(randomRow, randomCol);
            //Random Algorithm - End
            return toReturn;
            //}

        //return toReturn;








        int randomRow;
        int randomCol;
        Random random = new Random();
        do
        {
            randomRow = random.Next(0, board._rows);
            randomCol = random.Next(0, board._cols);
        } while (board._board[randomRow, randomCol] != 'X' && check(board, randomRow, randomCol) == false); // == ' ');&& board.isTheGameEnded()

        */






        private Tuple<int, int> chooseTheLast(Board board)
        {
            //find the last 
            Tuple<int, int> toReturn=null;
            bool stop = false;
            int maxI = 0;
            int maxJ = 0;
            for (int i = 0; i < board._rows; i++)
            {
                for (int j = 0; j < board._cols; j++)
                {

                    if (board._board[i, j] == 'X')
                    {

                        if (i > maxI)
                            maxI = i;
                        if (j > maxJ)
                            maxJ = j;
                    }

                }


            }


            /*
            if (board._board[maxI, maxJ] == 'X')
            {
                toReturn = new Tuple<int, int>(maxI, maxJ);

            }
            */
            
            for (int k= maxJ; k>=0; k--)
            {


                if (board._board[maxI, k] == 'X')
                {
                    
                    toReturn = new Tuple<int, int>(maxI, k);

                }
            }

            return toReturn;


        }
        private bool check(Board b, int randomRow, int randomCol)
        {
            if (randomRow == 0 && randomCol == 0)
                return false;
            else return true;


        }
        private space getBestMove2(Board gb, int player, TimeSpan timesup)
        {

            if (stopwatch.ElapsedMilliseconds > timesup.TotalMilliseconds - 5)
            {
                //Console.WriteLine(stopwatch.ElapsedMilliseconds);
                //Console.WriteLine(timesup.TotalMilliseconds-20);
                return null;

            }

            space bestSpace = null;
            // Console.WriteLine("bllaaablaa");



            if (player == 1)
            {

                foreach (char[,] item in resultDicPlayer1.Keys)
                {
                    bool stop = false;
                    for (int i = 0; i < gb._rows && stop == false; i++)
                    {
                        for (int j = 0; j < gb._cols && stop == false; j++)
                        {
                            if (item[i, j].Equals(gb._board[i, j]) == false)
                            {
                               
                                stop = true;
                            }
                        }
                    }

                    if (stop == false)
                    {
                        // Console.WriteLine("found");
                        return resultDicPlayer1[item];



                    }
                }
            }

            if (player == 2)
            {

                foreach (char[,] item in resultDicPlayer2.Keys)
                {
                    bool stop = false;
                    for (int i = 0; i < gb._rows && stop == false; i++)
                    {
                        for (int j = 0; j < gb._cols && stop == false; j++)
                        {
                            if (item[i, j].Equals(gb._board[i, j]) == false)
                            {
                                ///   Console.WriteLine("stopppp2222222222222222");

                                stop = true;
                            }
                        }
                    }

                    if (stop == false)
                    {
                        // Console.WriteLine("found");

                        return resultDicPlayer2[item];

                    }
                }
            }

            for (int i = 0; i < gb._rows; i++)
            {
                for (int j = 0; j < gb._cols; j++)


                {
                    if (gb._board[i, j] == 'X')
                    {
                        //for (int i = 0; i < openSpaces.Count; i++)
                        //{
                        // System.Console.WriteLine(" iteration " + i);

                        Board newBoard = new Board(gb);

                        // Console.WriteLine("board for player "+ player);


                        space newSpace = new space(new Tuple<int, int>(i, j), 0);

                        newBoard.fillPlayerMove(newSpace.place.Item1, newSpace.place.Item2);
                        //newBoard.printTheBoard();


                        //  newBoard[newSpace.Item1, newSpace.Item2] = p;

                        if (hasWinner(newBoard, player) == 0)
                        {
                            space tempMove = getBestMove2(newBoard, changePlayers(player), timesup);
                            if (stopwatch.ElapsedMilliseconds > timesup.TotalMilliseconds - 5)
                            {
                                // Console.WriteLine(stopwatch.ElapsedMilliseconds);
                                // Console.WriteLine(timesup.TotalMilliseconds - 20);
                                return null;

                            }
                            if (player == 1 && tempMove.rank == 1)
                            {
                                newSpace.rank = tempMove.rank;
                                resultDicPlayer1.Add(gb._board, newSpace);
                                // resultDicPlayer1[gb._board] = newSpace;
                                return newSpace;

                            }
                            if (player == 2 && tempMove.rank == -1)
                            {
                                newSpace.rank = tempMove.rank;
                                resultDicPlayer2.Add(gb._board, newSpace);
                                // resultDicPlayer2[gb._board] = newSpace;
                                return newSpace;
                            }
                            newSpace.rank = tempMove.rank;

                            //newSpace = tempMove; 

                        }
                        else
                        {
                            // Console.WriteLine("reach to end");
                            /*
                            if (player==1&& hasWinner(newBoard, player) == 1)
                            {
                                newSpace.rank = 1;
                                return newSpace;
                            }
                            */

                            int win = hasWinner(newBoard, player);
                            if (player == 1 && win == 2)
                            {
                                newSpace.rank = -1;

                            }
                            /*
                            else if (player==2&& hasWinner(newBoard, player) == 2)
                            {
                                newSpace.rank = -1;
                                return newSpace;

                            }
                            */
                            else if (player == 2 && win == 1)
                            {
                                newSpace.rank = 1;

                            }
                        }

                        //If the new move is better than our previous move, take it
                        //1- maximum player
                        //2 -minimum player
                        if (bestSpace == null || (player == 1 && newSpace.rank > bestSpace.rank) || (player == 2 && newSpace.rank < bestSpace.rank))
                        {
                            bestSpace = newSpace;
                        }
                    }
                }
            }
            if (player == 1)
            {

                resultDicPlayer1.Add(gb._board, bestSpace);

            }

            if (player == 2)
            {

                resultDicPlayer2.Add(gb._board, bestSpace);

            }
            return bestSpace;
        }






        //player 1= int 1
        //player 2=int 2
        private space GetBestMove(Board gb, int player)
        {
            //Console.WriteLine("i am player "+player);
            space bestSpace = null;
            //Dictionary<Board, int> AtateDic = new Dictionary<Board, int>();

            //List<space> openSpaces = getTuple(gb); 
            for (int i = 0; i < gb._rows; i++)
            {
                for (int j = 0; j < gb._cols; j++)


                {
                    if (gb._board[i, j] == 'X')
                    {
                        //for (int i = 0; i < openSpaces.Count; i++)
                        //{
                        // System.Console.WriteLine(" iteration " + i);

                        Board newBoard = new Board(gb);

                        // Console.WriteLine("board for player "+ player);


                        space newSpace = new space(new Tuple<int, int>(i, j), 0);

                        newBoard.fillPlayerMove(newSpace.place.Item1, newSpace.place.Item2);
                        //newBoard.printTheBoard();


                        //  newBoard[newSpace.Item1, newSpace.Item2] = p;

                        if (hasWinner(newBoard, player) == 0)
                        {
                            space tempMove = GetBestMove(newBoard, changePlayers(player));  //a little hacky, inverts the current player
                            if (player == 1 && tempMove.rank == 1)
                            {
                                newSpace.rank = tempMove.rank;
                                return newSpace;

                            }
                            if (player == 2 && tempMove.rank == -1)
                            {
                                newSpace.rank = tempMove.rank;
                                return newSpace;
                            }
                            newSpace.rank = tempMove.rank;
                            //newSpace = tempMove; 

                        }
                        else
                        {
                            // Console.WriteLine("reach to end");
                            /*
                            if (player==1&& hasWinner(newBoard, player) == 1)
                            {
                                newSpace.rank = 1;
                                return newSpace;
                            }
                            */

                            int win = hasWinner(newBoard, player);
                            if (player == 1 && win == 2)
                            {
                                newSpace.rank = -1;

                            }
                            /*
                            else if (player==2&& hasWinner(newBoard, player) == 2)
                            {
                                newSpace.rank = -1;
                                return newSpace;

                            }
                            */
                            else if (player == 2 && win == 1)
                            {
                                newSpace.rank = 1;

                            }
                        }

                        //If the new move is better than our previous move, take it
                        //1- maximum player
                        //2 -minimum player
                        if (bestSpace == null || (player == 1 && newSpace.rank > bestSpace.rank) || (player == 2 && newSpace.rank < bestSpace.rank))

                        {
                            bestSpace = newSpace;
                        }
                    }
                }
            }

            return bestSpace;
        }





        private int changePlayers(int p)
        {

            if (p == 1)
                return 2;
            else return 1;

        }

        private int hasWinner(Board board, int p)
        {
            if (board._squaresLeft == 0)
            {
                if (p == 1)
                    return 2;

                else
                    return 1;
            }
            else return 0;


        }
        private List<space> getTuple(Board board)
        {
            List<space> listT = new List<space>();

            //List<Tuple<int, int>> listT = new List<Tuple<int, int>>();

            for (int i = 0; i < board._rows; i++)
            {
                for (int j = 0; j < board._cols; j++)
                {
                    if (board.isLegalMove(i, j))
                    {

                        listT.Add(new space(new Tuple<int, int>(i, j), 0));

                    }
                }
            }
            return listT;


        }

        private bool checkIfSquare(Board board)
        {


            int numInR = 0;
            for (int x = 0; x < board._rows; x++)
            {
                if (board._board[x, 0] == 'X')
                {
                    numInR++;
                }
            }
            int numCul = 0;
            for (int y = 0; y < board._cols; y++)
            {
                if (board._board[0, y] == 'X')
                {
                    numCul++;
                }
            }


            if (numCul == numInR)
            {

                for (int i = 0; i < numInR; i++)
                {
                    for (int j = 0; j < numCul; j++)
                    {
                        if (board._board[i, j] != 'X')
                        {
                            return false;
                        }
                    }
                }


            }
            else return false;
            return true;


        }


        private int numInRow(Board b)
        {

            int numR = 0;
            for (int x = 0; x < b._rows; x++)
            {
                if (b._board[x, 0] == 'X')
                {
                    numR++;
                }
            }

            return numR;

        }


        private int munInCul(Board b)
        {

            int numCul = 0;

            for (int y = 0; y < b._rows; y++)
            {
                if (b._board[0, y] == 'X')
                {
                    numCul++;
                }
            }
            return numCul;
        }






        private bool checkIfReish(Board board)
        {

            for (int i = 1; i < board._rows; i++)
            {
                for (int j = 1; j < board._cols; j++)
                {
                    if (board._board[i, j] == 'X')
                    {
                        return false;
                    }
                }
            }

            return true;

        }



        private int calcErase(Tuple<int, int> p, Board board)
        {
            int count = 0;
            for (int x = p.Item1; x < board._rows; x++)
            {
                for (int y = p.Item2; y < board._cols; y++)
                {
                    if (board.isLegalMove(x, y))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        //check if you make move if player2 can win the game with this move
        private bool virtualMove(Tuple<int, int> ourPlayerVirMove, Board board)
        {

            Board virArray = new Board(board);
            virArray.fillPlayerMove(ourPlayerVirMove.Item1, ourPlayerVirMove.Item2);
            if (virArray._squaresLeft == 0)
                return false;
            /*
            for (int i = ourPlayerVirMove.Item1; i < board._rows; i++)
            {
                for (int j = ourPlayerVirMove.Item2; j < board._cols; j++)
                {
                    virArray.
                    virArray._board[i, j] = ' ';
                }
            }
            */
            Console.WriteLine("try" + ourPlayerVirMove.Item1 + ourPlayerVirMove.Item2);
            virArray.printTheBoard();
            //Thread.Sleep(2000); 
            int cCount = virArray._squaresLeft;
            for (int x = 0; x < board._rows; x++)
            {
                for (int y = 0; y < board._cols; y++)
                {
                    if (virArray.isLegalMove(x, y))
                    {
                        Tuple<int, int> player2VirMove = new Tuple<int, int>(x, y);
                        int cErase = 0;
                        for (int i = player2VirMove.Item1; i < board._rows; i++)
                        {
                            for (int j = player2VirMove.Item2; j < board._cols; j++)
                            {
                                if (virArray._board[i, j] == 'X')
                                {
                                    cErase++;
                                }
                            }
                        }
                        //
                        if ((cCount - cErase) == 1)
                        {
                            return false;
                        }

                        else
                        {



                        }
                    }
                }
            }
            return true;

        }
    }
}
