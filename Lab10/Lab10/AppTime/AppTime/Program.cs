using System;
using System.Diagnostics;
using System.Numerics;

namespace AppTime
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            BigInteger[] a = { 20, 35 };
            BigInteger p = 712456789024389;
            BigInteger q = 944922463590643219;
            BigInteger n = p * q;

            string binary1024Bits = "10001000001110000000000010110101001011111100010000000111000011100000000011111000000000110111000000011000001110000000110110000000111100000011011000110111011011000111111101110111011101110000000000111011011111000011110000011001101111011101111011111111011010000101100";
            string binary2048Bits = "1000100000111000000000001011010100101111110001000000011100001110000000001111100000000011011100000001100000111000000011011000000011110000001101100011011101101100011111110111011101110111000000000011101101111100001111000001100110111101110111101111111101101000010110010001000001110000000000010110101001011111100010000000111000011100000000011111000000000110111000000011000001110000000110110000000111100000011011000110111011011000111111101110111011101110000000000111011011111000011110000011001101111011101111011111111011010000101100";

            BigInteger decimal1024Bits = BigInteger.Parse(binary1024Bits, System.Globalization.NumberStyles.AllowHexSpecifier); // переводим число из двоичной системы счисления в десятичную систему счисления
            BigInteger decimal2048Bits = BigInteger.Parse(binary2048Bits, System.Globalization.NumberStyles.AllowHexSpecifier); // переводим число из двоичной системы счисления в десятичную систему счисления

            Console.WriteLine("+----+------+------+-----------------------------------+----------------------+");
            Console.WriteLine("| No |   a  |   x  |                 y                 |    Execution time    |");
            Console.WriteLine("+----+------+------+-----------------------------------+----------------------+");

            foreach (BigInteger a0 in a)
            {
                for (int i = 0; i < 5; i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    long freq = Stopwatch.Frequency;

                    BigInteger x = random.Next(1000, 100000);
                    while (!isSimple(x))
                    {
                        x = random.Next(103, 10100);
                    }

                    stopwatch.Start();

                    //123456789059
                    BigInteger y = Y(a0, x, decimal1024Bits);

                    stopwatch.Stop();
                    Console.WriteLine("|{0,4}|{1,6}|{2,6}|{3,35}|{4,22}|",
                        i, a0, x, y, (double)stopwatch.ElapsedTicks / freq + " sec");

                }
                Console.WriteLine("+----+------+------+-----------------------------------+----------------------+");
            }
        }           

        static BigInteger Y(BigInteger a, BigInteger x, BigInteger n)
        {
            BigInteger aPowX = a;
            for (BigInteger i = 1; i < x; i++)
            {
                aPowX *= a;
            }
            return aPowX % n;
        }

        static bool isSimple(BigInteger n)
        {
            bool isSimple = true;
            for (int i = 2; i < n; i++)
                if (n % i == 0) isSimple = false;
            return isSimple;
        }
    }
}

