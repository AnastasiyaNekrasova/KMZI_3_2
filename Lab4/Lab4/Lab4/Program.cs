using Lab2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<char> germanAlph = new List<char>()
{
                '\u0061', '\u0062', '\u0063', '\u0064',
                '\u0065', '\u0066', '\u0067', '\u0068', 
                '\u0069', '\u006A', '\u006B', '\u006C', 
                '\u006D', '\u006E', '\u006F', '\u0070',
                '\u0071', '\u0072', '\u0073', '\u0074',
                '\u0075', '\u0076', '\u0077', '\u0078', 
                '\u0079', '\u007A', 
                '\u00E4', '\u00F6', '\u00FC', '\u00DF'
            };
            string keyWord = "enigma";
            const string fileName = "Lab4-1.xls";
            int k = 7;
            EntropyChecker germanChecker = new EntropyChecker(germanAlph, 0, "Немецкий");
            string germanText = germanChecker.OpenDocument("german.txt").ReadToEnd().ToLower();
            Regex regex = new Regex(@"\W");
            germanText = regex.Replace(germanText, "");
            ExcelDocumentCreator<char, double> excel = new ExcelDocumentCreator<char, double>(new System.IO.FileInfo(fileName));


            int c = 0;
            while (true)
            {
                Console.WriteLine("Введите номер задания:");
                Console.WriteLine("1- на основе соотношений при k = 7");
                Console.WriteLine("2- таблица Трисемуса, ключевое слово – enigma ");
                Console.WriteLine("3- выход");

                if (!int.TryParse(Console.ReadLine(), out c))
                {
                    c = -1;
                }
                switch (c)
                {
                    case -1:
                        {
                            Console.Clear();
                            break;
                        }
                    case 1:
                        {
                            EncoderK encoderK = new EncoderK(germanAlph, k);

                            Stopwatch stopwatch = new Stopwatch();
                            long freq = Stopwatch.Frequency;

                            germanChecker.printAlphabet();
                            encoderK.printEditedAlphabet();
                            
                            Console.WriteLine($"\n\nТекст для шифрования:\n{germanText}");

                            Dictionary<char, int> alphCounts = germanChecker.alphabetListToDictionary();
                            germanChecker.getSymbolsCounts(germanText, alphCounts);

                            Dictionary<char, double> chances = germanChecker.getSymbolsChances(germanText, alphCounts);
                            germanChecker.printChances(chances);

                            excel.createWorksheet("first");
                            excel.addValuesFromDict(chances, "first", 1);

                            stopwatch.Start();
                            string encodedText = encoderK.encode(germanText);
                            stopwatch.Stop();

                            Console.WriteLine($"\n-------------------------------------------------------");
                            Console.WriteLine($"Время шифрования: {(double)stopwatch.ElapsedTicks / freq} sec");
                            Console.WriteLine($"-------------------------------------------------------\n");

                            Dictionary<char, int> alphCountsEnc = germanChecker.alphabetListToDictionary();
                            germanChecker.getSymbolsCounts(encodedText, alphCountsEnc);

                            Console.WriteLine($"Зашифрованный текст:\n{encodedText}");

                            chances = germanChecker.getSymbolsChances(germanText, alphCountsEnc);
                            germanChecker.printChances(chances);

                            stopwatch.Start();
                            string decodedText = encoderK.decode(encodedText);
                            stopwatch.Stop();

                            Console.WriteLine($"\n-------------------------------------------------------");
                            Console.WriteLine($"Время расшифрования: {(double)stopwatch.ElapsedTicks / freq} sec");
                            Console.WriteLine($"-------------------------------------------------------\n");

                            Console.WriteLine($"Расшифрованный текст:\n{decodedText}");

                            excel.createWorksheet("first");
                            excel.addValuesFromDict(chances, "first", 3);
                            excel.pack.Save();

                            Console.ReadKey();
                            Console.Clear();
                            break;

                        }

                    case 2:
                        {
                            EncoderTrisemus EncoderTrisemus = new EncoderTrisemus(germanAlph, keyWord);

                            Stopwatch stopwatch = new Stopwatch();
                            long freq = Stopwatch.Frequency;

                            Console.WriteLine($"Матрица для шифрования:\n");

                            EncoderTrisemus.printMatrix(EncoderTrisemus.tableAlphabet);

                            Console.WriteLine($"Текст для шифрования:\n{germanText}");

                            Dictionary<char, int> alphCounts = germanChecker.alphabetListToDictionary();
                            germanChecker.getSymbolsCounts(germanText, alphCounts);

                            Dictionary<char, double> chances = germanChecker.getSymbolsChances(germanText, alphCounts);
                            germanChecker.printChances(chances);

                            excel.createWorksheet("first");
                            excel.addValuesFromDict(chances, "first", 6);

                            stopwatch.Start();
                            string encodedText = EncoderTrisemus.encode(germanText);
                            stopwatch.Stop();
                            
                            Console.WriteLine($"\n-------------------------------------------------------");
                            Console.WriteLine($"Время шифрования: {(double)stopwatch.ElapsedTicks / freq} sec");
                            Console.WriteLine($"-------------------------------------------------------\n");

                            Dictionary<char, int> alphCountsEnc = germanChecker.alphabetListToDictionary();
                            germanChecker.getSymbolsCounts(encodedText, alphCountsEnc);

                            Console.WriteLine($"Зашифрованный текст:\n{encodedText}");

                            chances = germanChecker.getSymbolsChances(germanText, alphCountsEnc);
                            germanChecker.printChances(chances);

                            stopwatch.Start();
                            string decodedText = EncoderTrisemus.decode(encodedText);
                            stopwatch.Stop();

                            Console.WriteLine($"\n-------------------------------------------------------");
                            Console.WriteLine($"Время расшифрования: {(double)stopwatch.ElapsedTicks / freq} sec");
                            Console.WriteLine($"-------------------------------------------------------\n");

                            Console.WriteLine($"Расшифрованный текст:\n{decodedText}");

                            excel.addValuesFromDict(chances, "first", 8);
                            excel.pack.Save();

                            Console.ReadKey();
                            Console.Clear();
                            break;

                        }
                    case 3:
                        {
                            return;
                        }
                    default:
                        {
                            Console.Clear();
                            break;
                        }
                }
            }
        }
    }
}
