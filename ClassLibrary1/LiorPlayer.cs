using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Game


namespace ClassLibrary
{
    public class LiorPlayer
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
            cla gffg = new cla(board);
            Tuple<int, int> nbnbnbnb = null;
            Stack<Tuple<int, int>> nbnbnb = gffg.foo();
            nbnbnbnb = vbvbvb(gffg, nbnbnb);
            if (nbnbnbnb == null)
                nbnbnbnb = gffg.nbnbnbnb();
            Console.WriteLine("{0} ms left", (_timesup - stopWatch.Elapsed).TotalMilliseconds);
            return nbnbnbnb;
        }

        private Tuple<int, int> vbvbvb(cla jkj, Stack<Tuple<int, int>> kjkjkjjk)
        {
            Tuple<int, int> kjkjkjkjkjdf;
            cla fhh;
            Dictionary<Tuple<int, int>, cla> jkjkkj = new Dictionary<Tuple<int, int>, cla>();
            while (!g() && kjkjkjjk.Count > 0)
            {
                kjkjkjkjkjdf = kjkjkjjk.Pop();
                fhh = new cla(jkj, kjkjkjkjkjdf);
                if (dfd(fhh))
                {
                    //Console.WriteLine("Gain: 1");
                    return kjkjkjkjkjdf;
                }
                jkjkkj[kjkjkjkjkjdf] = fhh;
            }
            int cghj = Int32.MinValue;
            int mbbb;
            Tuple<int, int> bmbbm = null;
            int bnbnb = 3;
            int gfgfgfgf = bnbnb;
            Tuple<int, int> bvcxvbvb;
            for (int i = 0; i < jkjkkj.Keys.Count; i++)
            {
                bvcxvbvb = jkjkkj.Keys.ElementAt(i);
                if (g())
                {
                    break;
                }
                fhh = jkjkkj[bvcxvbvb];

                if (stopWatch.Elapsed.TotalMilliseconds / _timesup.TotalMilliseconds < 0.3)
                {
                    bnbnb = d(bnbnb, ref gfgfgfgf, jkjkkj.Keys.Count - i, jkjkkj.Keys.Count, false, 4);

                }
                else
                {
                    bnbnb = d(bnbnb, ref gfgfgfgf, jkjkkj.Keys.Count - i, jkjkkj.Keys.Count, true, 4);
                }

                if (!sdf(fhh))
                    mbbb = d2(fhh, goo.foo, bnbnb, int.MinValue, int.MaxValue);
                else
                    mbbb = -1;
                if (mbbb > cghj)
                {
                    cghj = mbbb;
                    bmbbm = bvcxvbvb;

                    if (cghj == 1)
                    {

                        break;
                    }

                }

            }
            Console.WriteLine("Gain calculated: " + cghj);
            if (cghj <= 0)
            {
                return jkj.nbnbnbnb();
            }
            return bmbbm;
        }
        private int d(int d, ref int dd, int ddd, int dddd, bool dddddd, int qwqw)
        {
            double fgf = (_timesup - stopWatch.Elapsed).TotalMilliseconds;
            double fggfgf = fgf / _timesup.TotalMilliseconds;
            double vbvbbvv = (double)ddd / (double)dddd;
            double dfdgf = (double)(ddd - 1) / (double)dddd;

            if (vbvbbvv > fggfgf && dddddd)
            {
                if (dd > d || dd == 1)
                {
                    int vbvb = 0;
                    dd = d;
                    return vbvb;
                }
                else
                {
                    int gfgffg = Math.Max(d - 1, 0);
                    dd = d;
                    return gfgffg;
                }

            }
            if (dfdgf < fggfgf)
            {
                int gfgffg = Math.Min(d + 1, qwqw);//+ 1;
                dd = d;
                return gfgffg;
            }
            dd = d;
            return d;
        }

        private bool dfd(cla pio)
        {
            return pio.f() || pio.e() || pio.d() || pio.g();
        }
        private bool sdf(cla mnmnmn)
        {
            if (mnmnmn.b() || mnmnmn.a() || mnmnmn.c())
            {
                return true;
            }
            return false;



        }

        private int d2(cla ser, goo fde, int ddf, int dfdf, int dfdd)
        {


            if (dfd(ser))
            {
                if (fde == goo.foo)
                    return 1;
                else
                    return -1;
            }
            if (sdf(ser))
            {
                if (fde == goo.foo)
                    return -1;
                else
                    return 1;
            }
            if (ddf == 0)
            {
                return 0;
            }

            int doo;
            int sh;
            goo shoo;
            if (fde == goo.foo)
            {
                shoo = goo.MinPlayer_Opponent;
                doo = int.MaxValue;
            }
            else
            {
                doo = int.MinValue;
                shoo = goo.foo;
            }
            Tuple<int, int> w;
            cla e;
            Dictionary<Tuple<int, int>, cla> f = new Dictionary<Tuple<int, int>, cla>();
            Stack<Tuple<int, int>> aq = ser.foo();
            while (!g() && aq.Count > 0)
            {
                w = aq.Pop();
                e = new cla(ser, w);

                f[w] = e;

                sh = d2(e, shoo, ddf - 1, dfdf, dfdd);
                if (shoo == goo.foo)
                {
                    doo = Math.Max(doo, sh);
                    dfdf = Math.Max(dfdf, sh);
                    if (doo >= dfdd || doo == 1)
                    {
                        //Console.WriteLine("Tree cutted at depth {0} by alpha beya pruning!", depthLevel);
                        break;
                    }
                }
                else
                {
                    doo = Math.Min(doo, sh);
                    dfdd = Math.Min(dfdd, sh);
                    if (doo <= dfdf || doo == -1)
                    {
                        //Console.WriteLine("Tree cut at depth {0} by alpha beya pruning!", depthLevel);
                        break;
                    }

                }
            }
            return doo;

        }

        private bool g()
        {
            if ((_timesup - stopWatch.Elapsed).TotalMilliseconds < 10)
                return true;
            else
                return false;
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

        public enum goo { foo, MinPlayer_Opponent };


        private class cla
        {
            public int _rows;
            public int _cols;
            public int _aaa = 0;
            public Dictionary<int, int> _bbb;
            private Tuple<int, int> currentMove = null;

            public cla(Board board)
            {
                _rows = board._rows;
                _cols = board._cols;
                int? rightMostSquareInCurrentRow;
                _bbb = new Dictionary<int, int>();
                int row = 0;
                for (; row < board._rows; row++)
                {
                    rightMostSquareInCurrentRow = FindMostRightSquare(board._board, row, _cols);
                    if (rightMostSquareInCurrentRow != null)
                        _bbb[row] = (int)rightMostSquareInCurrentRow;
                    else
                        break;
                }
                _aaa = Math.Max(0, row - 1);

            }


            private int? FindMostRightSquare(char[,] cbvbvv, int fggfgfgfgf, int bvvbvbvbvb)
            {
                if (cbvbvv[fggfgfgfgf, 0] != 'X')
                    return null;
                if (bvvbvbvbvb == 1 || cbvbvv[fggfgfgfgf, bvvbvbvbvb - 1] == 'X')
                    return bvvbvbvbvb - 1;
                int vbvbvb = 0;
                int zcxcx = bvvbvbvbvb - 1;
                int m;
                while (vbvbvb <= zcxcx)
                {
                    m = (vbvbvb + zcxcx) / 2;
                    if (cbvbvv[fggfgfgfgf, m] == 'X' && cbvbvv[fggfgfgfgf, m + 1] != 'X')
                        return m;
                    if (cbvbvv[fggfgfgfgf, m] == 'X')
                        vbvbvb = m + 1;
                    else
                        zcxcx = m - 1;

                }
                return FindMostRightSquare(cbvbvv, fggfgfgfgf, bvvbvbvbvb);
            }


            public cla(cla cvcvcvc, Tuple<int, int> vcvcvcvc)
            {
                _rows = cvcvcvc._rows;
                _cols = cvcvcvc._cols;
                _bbb = new Dictionary<int, int>();
                int bvvbvbvb = 0;
                for (; bvvbvbvb < _rows && bvvbvbvb < vcvcvcvc.Item1; bvvbvbvb++)
                    _bbb[bvvbvbvb] = cvcvcvc._bbb[bvvbvbvb];
                for (; bvvbvbvb < _rows; bvvbvbvb++)
                {
                    if (vcvcvcvc.Item2 == 0 || !cvcvcvc._bbb.ContainsKey(bvvbvbvb))
                    {
                        break;
                    }
                    else
                        _bbb[bvvbvbvb] = Math.Min(vcvcvcvc.Item2 - 1, cvcvcvc._bbb[bvvbvbvb]);
                }
                _aaa = Math.Max(0, bvvbvbvb - 1);

            }




            public Tuple<int, int> nbnbnbnb()
            {

                return new Tuple<int, int>(_aaa / 2, _bbb[_aaa / 2]);
            }

            internal bool g()
            {
                return _aaa == 0 && _bbb[_aaa] == 0;
            }

            internal bool f()
            {
                return _bbb[0] == _aaa && _bbb.ContainsKey(1) && _bbb[1] == 0;
            }

            internal bool e()
            {

                if (_bbb.ContainsKey(1) && _bbb[0] == _bbb[1] + 1 && _aaa == 1)
                    return true;
                else
                    return false;
            }

            internal bool d()
            {
                if (_bbb[0] == 1 && _aaa > 0 && _bbb[_aaa - 1] == 1 && _bbb[_aaa] == 0)
                    return true;
                return false;
            }


            internal bool a()
            {
                return _aaa == 0 && _bbb[_aaa] > 0;
            }

            internal bool b()
            {
                return _bbb[0] == 0 && _aaa > 0;
            }

            internal bool c()
            {
                return _aaa == _bbb[0] && _bbb[_aaa] == _bbb[0];
            }
            internal Stack<Tuple<int, int>> foo()
            {
                Stack<Tuple<int, int>> vnbvnnv = new Stack<Tuple<int, int>>();
                if (_bbb[0] == 0 && _aaa == 0)
                    return vnbvnnv;
                Tuple<int, int> vxvxccvcc = new Tuple<int, int>(_aaa, (int)_bbb[_aaa]);
                vnbvnnv.Push(vxvxccvcc);
                while (true)
                {
                    if (vxvxccvcc.Item2 > 0)
                        vxvxccvcc = new Tuple<int, int>(vxvxccvcc.Item1, vxvxccvcc.Item2 - 1);
                    else
                        vxvxccvcc = new Tuple<int, int>(vxvxccvcc.Item1 - 1, (int)_bbb[vxvxccvcc.Item1 - 1]);
                    if (vxvxccvcc.Item1 != 0 || vxvxccvcc.Item2 != 0)
                        vnbvnnv.Push(vxvxccvcc);
                    else
                        break;

                }
                return vnbvnnv;
            }
            public override string ToString()
            {
                string result = "|";
                for (int fdfdfdfd = 0; fdfdfdfd <= _aaa; fdfdfdfd++)
                {
                    result += _bbb[fdfdfdfd] + "|";
                }
                result += "|";
                return result;

            }

        }
    }
}