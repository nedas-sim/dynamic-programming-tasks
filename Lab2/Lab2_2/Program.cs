using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Lab2_2
{

    //Turime n daiktų, kurių svoriai yra s1, s2, . . . , sn, 
    //o kaina p1, p2, . . . , pn.Reikia rasti daiktų rinkinio 
    //didžiausią vertę, kad rinkinio svoris neviršytų W


    class Program
    {
        static int[] s;
        static int[] p;
        static Random rnd = new Random();
        static int actions = 0;
        static int actionsDin = 0;

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            

            bool t1 = true;
            bool t2 = true;


            //int[] svoriai = { 1, 2 };
            //int[] kainos = { 5, 10 };
            //int dydis = 2;

            //Console.WriteLine(F(dydis, svoriai, kainos, svoriai.Length));
            //Console.WriteLine(FDin(dydis, svoriai, kainos, svoriai.Length));

            //Console.ReadKey();
            //return;

            PrintToFile(String.Format("{0},{1},{2},{3},{4}", "Dydis", "Rekursijos laikas", "Operaciju kiekis", "Dinaminio laikas", "Dinaminio oper. sk."));

            for (int n = 1; n <= 40; n += 1)
            {
                int ms = 0;
                int msDin = 0;

                actions = 0;
                actionsDin = 0;

                FillArrays(n);

                //int w = getW(s);
                int w = getBadW(s);


                //Console.WriteLine("n = {0}, w = {1}", n, w);
                //for(int i = 0; i < n; i++)
                //{
                //    Console.WriteLine("{0} kg, {1} eur", s[i], p[i]);
                //}

                int value1 = -1;
                int value2 = -2;

                if (t1)
                {
                    watch.Reset();
                    watch.Start();
                    value1 = F(w, s, p, n);
                    watch.Stop();
                    ms = (int)watch.ElapsedMilliseconds;
                }
                if (t2)
                {
                    watch.Reset();
                    watch.Start();
                    value2 = FDin(w, s, p, n);
                    watch.Stop();
                    msDin = (int)watch.ElapsedMilliseconds;
                }
                
                Console.WriteLine("Dydis: {0}, w: {1}, ms: {2}, actions: {3}, msDin: {4}, actionsDin: {5}, {6}", n, w, ms, actions, msDin, actionsDin, value1 == value2);
                PrintToFile(String.Format("{0},{1},{2},{3},{4}", n, ms, actions, msDin, actionsDin));

                if (ms > 1 * 60 * 1000)
                {
                    t1 = false;
                }
                if(msDin > 1 * 60 * 1000)
                {
                    t2 = false;
                }

                if(!t1 && !t2)
                {
                    break;
                }
            }


            Console.WriteLine("Pabaiga");
            Console.ReadKey();
        }

        static void PrintToFile(string line)
        {
            using (StreamWriter writer = new StreamWriter("duom2.csv", true))
            {
                writer.WriteLine(line);
            }
        }

        static int F(int w, int[] s, int[] p, int n)
        {
            actions++;
            if(n == 0 || w == 0)
            {
                actions++;
                return 0;
            }

            actions++;
            if (s[n-1] > w)
            {
                actions++;
                return F(w, s, p, n - 1);
            }
            else
            {
                actions++;
                return max(p[n - 1] + F(w - s[n - 1], s, p, n - 1),
                           F(w, s, p, n - 1));
            }
        }

        static int FDin(int w, int[] s, int[] p, int n)
        {
            actionsDin++;
            int[,] din = new int[n + 1, w + 1];

            actionsDin++;
            for (int i = 0; i <= n; i++)
            {
                actionsDin++;

                actionsDin++;
                for (int j = 0; j <= w; j++)
                {
                    actionsDin++;

                    actionsDin++;
                    if (i == 0 || j == 0)
                    {
                        actionsDin++;
                        din[i, j] = 0;
                    }

                    else if (s[i-1] > j)
                    {
                        actionsDin += 2;
                        din[i, j] = din[i - 1, j];
                    }

                    else
                    {
                        actionsDin += 3;
                        int value1 = p[i - 1] + din[i - 1, j - s[i - 1]];
                        int value2 = din[i - 1, j];
                        din[i, j] = max(value1, value2);
                    }
                }
            }

            actionsDin++;
            return din[n, w];
        }

        static int getW(int[] s)
        {
            int w = 0;

            for(int i = 0; i < s.Length; i++)
            {
                w += s[i];
            }

            w /= s.Length;

            w *= rnd.Next(2, 4);

            return w;
        }

        static int getBadW(int[] s)
        {
            int w = 0;

            for(int i = 0; i < s.Length; i++)
            {
                w += s[i];
            }

            w++;
            return w;
        }

        static int max(int a, int b)
        {
            return a > b ? a : b;
        }

        static void FillArrays(int n)
        {
            //Random rnd = new Random();
            s = new int[n];
            p = new int[n];

            for(int i = 0; i < n; i++)
            {
                int value1 = rnd.Next(1, 100);
                int value2 = rnd.Next(5 * value1, 6 * value1 + 1);

                s[i] = value1;
                p[i] = value2;
            }

            for(int i = 0; i < n - 1; i++)
            {
                int index = i;
                for(int j = i + 1; j < n; j++)
                {
                    if(s[index] > s[j])
                    {
                        index = j;
                    }
                }
                if(index != i)
                {
                    int temp = s[i];
                    s[i] = s[index];
                    s[index] = temp;

                    temp = p[i];
                    p[i] = p[index];
                    p[index] = temp;
                }
            }
        }
    }
}
