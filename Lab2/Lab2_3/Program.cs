using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Lab2_3
{
    class CustomData
    {
        public int TNum;
        public int TResult;
    }

    class Program
    {
        static public int[] x;
        static public int[] y;

        static void Main(string[] args)
        {

            bool t1 = true;
            bool t2 = true;

            PrintToFile(String.Format("{0},{1},{2}", "Dydis", "Rekursijos laikas", "Lygiagretus laikas"));

            for(int i = 1; i <= 20; i += 1)
            {
                FillArrays(i);

                int value1 = -1;
                int value2 = -2;

                int m = x.Length - 1;
                int n = y.Length - 1;

                Stopwatch watch = new Stopwatch();
                int ms = 0;
                int ms2 = 0;

                if (t1)
                {
                    watch.Start();
                    value1 = F1(m, n);
                    watch.Stop();
                    ms = (int)watch.ElapsedMilliseconds;
                }

                if (t2)
                {
                    watch.Reset();
                    watch.Start();
                    value2 = F2(m, n);
                    watch.Stop();
                    ms2 = (int)watch.ElapsedMilliseconds;
                }

                Console.WriteLine("Dydis: {0}, ms: {1}, ms2: {2}, Lygus?: {3}", i, ms, ms2, value1 == value2);
                PrintToFile(String.Format("{0},{1},{2}", i, ms, ms2));


                if (ms > 10 * 1000)
                    t1 = false;
                if (ms2 > 10 * 1000)
                    t2 = false;

                if (!t1 && !t2)
                    break;
            }


            Console.WriteLine("Pabaiga");
            Console.ReadKey();
        }

        static void PrintToFile(string line)
        {
            using (StreamWriter writer = new StreamWriter("duom3.csv", true))
            {
                writer.WriteLine(line);
            }
        }

        static void FillArrays(int size)
        {
            Random rnd = new Random();

            x = new int[size];
            y = new int[size];

            for (int i = 0; i < size; i++)
            {
                int value1 = rnd.Next(500);
                int value2 = rnd.Next(500);

                x[i] = value1;
                y[i] = value2;
            }
        }

        static int F1(int m, int n)
        {
            int returnValue;
            if (n == 0)
            {
                returnValue = m;
            }
            else if (m == 0 && n > 0)
            {
                returnValue = n;
            }
            else
            {
                int value1 = 1 + F1(m - 1, n);
                int value2 = 1 + F1(m, n - 1);
                int value3 = D(m, n) + F1(m - 1, n - 1);

                returnValue = Least(value1, value2, value3);
            }
            return returnValue;
        }

        static int F2(int m, int n)
        {
            int returnValue;
            if (n == 0)
                return m;
            if (m == 0 && n > 0)
                return n;
            
            int countCPU = 3;
            Task[] tasks = new Task[countCPU];
            for (int j = 0; j < countCPU; j++)
            {
                tasks[j] = Task.Factory.StartNew(
                    (Object p) =>
                    {
                        var data = p as CustomData; if (data == null) return;
                        data.TResult = F1(j == 1 ? m : m - 1, 
                                          j == 0 ? n : n - 1);
                    },
                    new CustomData() { TNum = j });
            }

            Task.WaitAll(tasks);
            returnValue = 
                Least((tasks[0].AsyncState as CustomData).TResult + 1,
                      (tasks[1].AsyncState as CustomData).TResult + 1,
                      (tasks[2].AsyncState as CustomData).TResult + D(m, n));

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
