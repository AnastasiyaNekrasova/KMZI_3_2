using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    static class MathFunctions
    {
        public static int NODCompute(int x, int y)
        {
            while (x != 0 && y != 0)
            {
                if (x > y)
                {
                    x -= y;
                }
                else
                {
                    y -= x;
                }
            }
            return Math.Max(x, y);

        }

        public static bool IsSimple(int x)
        {
            for (int i = 2; Math.Pow(i, 2) <= x; i++)
            {
                if (x % i == 0)
                {
                    return false;
                }
            }

            return true;
        }


        public static int FindSimple(int m, int n)
        {
            int counter = 0;
            if (n < m)
            {
                Console.WriteLine("Неверный промежуток");
            }

            Console.Write($"Простые числа интервала [{m},{n}]: ");

            for (int i = m; i <= n; i++)
            {
                if (IsSimple(i))
                {
                    Console.Write(i.ToString() + " ");
                    counter++;
                }
            }
            Console.WriteLine();
            Console.WriteLine($"Количество простых чисел: {counter}");
            return counter;
        }

        public static void CanonicalDecomposition(int x)
        {
            string str = $"Каноническая форма записи {x}: 1";
            for (int i = 0; x % 2 == 0; x /= 2)
            {
                str += " * 2";
            }
                for (int i = 3; i <= x;)
                {
                    if (x % i == 0)
                    {
                        str += " * " + i.ToString();
                        x /= i;
                    }
                    else
                    {
                        i += 2;
                    }

                }
            Console.WriteLine(str);
        }
    }
}
