using System;
using System.Diagnostics;
using System.Linq;

namespace lab_13
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            long freq = Stopwatch.Frequency;

            while (true)
            {
                Console.WriteLine("Enter the task number (1, 2, or 3) or enter 'exit' to quit:");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                    break;

                int taskNumber;
                if (!int.TryParse(input, out taskNumber) || taskNumber < 1 || taskNumber > 3)
                {
                    Console.WriteLine("Invalid task number. Please try again.");
                    continue;
                }

                int xmin = 516, xmax = 550, a = -1, b = 1, p = 751;
                int[] P, Q, R;
                string text;
                int[] point;
                int k, l;
                int[] digitalSign;


                switch (taskNumber)
                {
                    case 1:
                        // Task 1.1
                        sw.Start();
                        for (int x = xmin; x <= xmax; x++)
                        {
                            Console.WriteLine($"x = {x}, y = {Math.Sqrt((x * x * x - x + b) % p)}");
                        }
                        sw.Stop();

                        Console.WriteLine($"time: {(double)sw.ElapsedTicks / freq} sec \n");
                        sw.Reset();

                        // Task 1.2
                        P = new int[] { 96, 386 };
                        Q = new int[] { 61, 129 };
                        R = new int[] { 100, 364 };
                        Console.WriteLine($"Given: P({P[0]}, {P[1]}), Q({Q[0]}, {Q[1]}), R({R[0]}, {R[1]})\n");

                        sw.Start();
                        int[] kP = EllipticCurves.kP(8, P, a, p);
                        Console.WriteLine($"kP = 8P = {kP.Select(el => el.ToString()).Aggregate((prev, current) => "R(" + prev + ", " + current + ")")}");
                        sw.Stop();
                        Console.WriteLine($"time: {(double)sw.ElapsedTicks / freq} sec \n");
                        sw.Reset();

                        sw.Start();
                        Console.WriteLine($"P + Q = {EllipticCurves.CalculateSum(Q, R, p).Select(el => el.ToString()).Aggregate((prev, current) => "R(" + prev + ", " + current + ")")}");
                        sw.Stop();
                        Console.WriteLine($"time: {(double)sw.ElapsedTicks / freq} sec \n");
                        sw.Reset();

                        sw.Start();
                        int[] lQ = EllipticCurves.kP(5, Q, a, p);
                        Console.WriteLine($"kP + lQ - R = 8P + 5Q - R = {EllipticCurves.CalculateSum(EllipticCurves.CalculateSum(kP, lQ, p), EllipticCurves.InversePoint(R), p).Select(el => el.ToString()).Aggregate((prev, current) => "R(" + prev + ", " + current + ")")}");
                        sw.Stop();
                        Console.WriteLine($"time: {(double)sw.ElapsedTicks / freq} sec \n");
                        sw.Reset();

                        sw.Start();
                        Console.WriteLine($"P - Q + R = {EllipticCurves.CalculateSum(EllipticCurves.CalculateSum(P, EllipticCurves.InversePoint(Q), p), R, p).Select(el => el.ToString()).Aggregate((prev, current) => "(" + prev + ", " + current + ")")}");
                        sw.Stop();
                        Console.WriteLine($"time: {(double)sw.ElapsedTicks / freq} sec \n");
                        sw.Reset();

                        break;

                    case 2:
                        // Task 2
                        text = "некрасоваанастасияпавловна";
                        Console.WriteLine($"Text: {text}");
                        sw.Start();
                        int[,] encryptedText = EllipticCurves.Encrypt(text, new int[] { 0, 1 }, a, p, 16);
                        sw.Stop();
                        Console.WriteLine($"Encrypted text: {string.Join(" ", encryptedText.Cast<int>())}");
                        Console.WriteLine($"\nEncryption time: {(double)sw.ElapsedTicks / freq} sec");
                        sw.Restart();
                        Console.WriteLine($"Decrypted text: {EllipticCurves.Decrypt(encryptedText, a, p, 16)}");
                        sw.Stop();
                        Console.WriteLine($"Decryption time: {(double)sw.ElapsedTicks / freq} sec \n");
                        break;

                    case 3:
                        // Task 3
                        point = new int[] { 416, 55 };
                        k = 13;
                        l = 4;
                        digitalSign = EllipticCurves.CreateDigitalSign(point, k, l, a, p);
                        Console.WriteLine($"Digital sign: {digitalSign.Select(el => el.ToString()).Aggregate((prev, current) => prev + " " + current)}");
                        Console.WriteLine($"Result of checking digital sign: {EllipticCurves.VerifyDigitalSign(digitalSign, point, k, l, a, p)}");
                        break;

                    default:
                        Console.WriteLine("Invalid task number. Exiting...");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}