using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Lab2_1
{
    class Program
    {
        static public int[] x;
        static public int[] y;
        static public int[,] din;
        static public int actions = 0;
        static public int actionsDin = 0;
        
        // actionsDin(n) = 4n^2 - 3n + 7

        static void Main(string[] args)
        {
            bool t1 = true;
            bool t2 = true;
            PrintToFile(String.Format("{0},{1},{2},{3},{4}", "Dydis", "Rekursijos laikas", "Operaciju kiekis", "Dinaminio laikas", "Operaciju kiekis"));
            for (int i = 1; i <= 20; i += 1)
            {

                FillArrays(i);
                actions = 0;
                actionsDin = 0;

                int m = x.Length - 1;
                int n = y.Length - 1;

                Stopwatch watch = new Stopwatch();
                int ms = 0;
                int msDin = 0;

                if (t1)
                {
                    watch.Start();
                    F(m, n);
                    watch.Stop();
                    ms = (int)watch.ElapsedMilliseconds;
                }

                if (t2)
                {
                    Fill2DArray(i);
                    watch.Reset();
                    watch.Start();
                    FDin(m, n);
                    watch.Stop();
                    msDin = (int)watch.ElapsedMilliseconds;
                }

                PrintToFile(String.Format("{0},{1},{2},{3},{4}", i, ms, actions, msDin, actionsDin));
                Console.WriteLine("Size: {0}, time: {1}, actions: {2}, timeDin: {3}, actionsDin: {4}", i, ms, actions, msDin, actionsDin);

                if(ms > 1 * 60 * 1000/2)
                {
                    t1 = false;
                }
                if(msDin >  1 * 60 * 1000/2)
                {
                    t2 = false;
                }

                if (!t1 && !t2)
                {
                    break;
                }
            }

            Console.WriteLine("Pabaiga");
            Console.ReadKey();
        }

        static void PrintToFile(string line)
        {
            using (StreamWriter writer = new StreamWriter("duom.csv", true))
            {
                writer.WriteLine(line);
            }
        }

        static void PrintArray(int[] arr)
        {
            for(int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();
        }

        static void Print2DArray(int[,] arr, int m, int n)
        {
            for(int i = 0; i <= m; i++)
            {
                for(int j = 0; j <= n; j++)
                {
                    Console.Write(arr[i,j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void FillArrays(int size)
        {
            Random rnd = new Random();

            x = new int[size];
            y = new int[size];
                        
            for(int i = 0; i < size; i++)
            {
                int value1 = rnd.Next(500);
                int value2 = rnd.Next(500);

                x[i] = value1;
                y[i] = value2;
            }
        }

        static void Fill2DArray(int size)
        {
            din = new int[size + 1, size + 1];

            for (int i = 0; i < size + 1; i++)
            {
                for (int j = 0; j < size + 1; j++)
                {
                    din[i, j] = int.MaxValue;
                }
            }
        }

        static int FDin(int m, int n)
        {
            for (int i = 0; i <= m; i++)
            {
                din[0, i] = i;
                actionsDin += 2;
            }
            for (int i = 0; i <= n; i++)
            {
                din[i, 0] = i;
                actionsDin += 2;
            }
            actionsDin += 2;


            actionsDin++;
            for (int i = 1; i <= m; i++)
            {
                actionsDin++;
                for (int j = 1; j <= n; j++)
                {
                    int value1 = din[i - 1, j] + 1;
                    int value2 = din[i, j - 1] + 1;
                    int value3 = din[i - 1, j - 1] + D(i, j);
                    din[i, j] = Least(value1, value2, value3);
                    actionsDin += 4;
                }
            }
            actionsDin++;
            return din[m, n];
        }

        static int F(int m, int n)
        {
            int returnValue;
            if (n == 0)
            {
                actions++;
                returnValue = m;
                actions++;
            }
            else if (m == 0 && n > 0)
            {
                actions += 2;
                returnValue = n;
                actions++;
            }
            else
            {
                actions += 2;
                actions += 3;
                int value1 = 1 + F(m - 1, n);
                int value2 = 1 + F(m, n - 1);
                int value3 = D(m, n) + F(m - 1, n - 1);

                returnValue = Least(value1, value2, value3);
                actions++;
            }

            actions++;
            return returnValue;
        }

        static int D(int i, int j)
        {
            return x[i] == y[j] ? 1 : 0;
        }

        static int Least(int val1, int val2, int val3)
        {
            if (val1 < val2 && val1 < val3)
                return val1;
            if (val2 < val1 && val2 < val3)
                return val2;
            return val3;
        }
    }
}
