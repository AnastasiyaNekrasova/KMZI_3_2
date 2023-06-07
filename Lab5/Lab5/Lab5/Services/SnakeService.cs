using Lab2;
using Lab2.DocumentReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab5.Services
{
    static class SnakeService
    {
        public static void MakeSnake(List<char> germanAlph)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("              МАРШРУТНАЯ ПЕРЕСТАНОВКА (ЗМЕЙКА)");
            Console.WriteLine("------------------------------------------------------------");
            Console.ResetColor();

            EntropyChecker germanChecker = new EntropyChecker(germanAlph, 0, "Немецкий");
            string germanText = germanChecker.OpenDocument("german.txt").ReadToEnd().ToLower();
            Regex regex = new Regex(@"\W");
            germanText = regex.Replace(germanText, "");
            Dictionary<char, int> germanDict = germanChecker.alphabetListToDictionary();
            germanChecker.getSymbolsCounts(germanText, germanDict);
            Dictionary<char, double> germanSnakeChances = germanChecker.getSymbolsChances(germanText, germanDict);
            germanChecker.printAlphabet();

            int tableParam = Convert.ToInt32(Math.Sqrt(germanText.Length));
            while (tableParam * tableParam != germanText.Length)
            {
                germanText+='*';
            }
            SnakeEncrypter snakeEncrypter = new SnakeEncrypter(tableParam, tableParam, germanText);

            snakeEncrypter.printMatrix(snakeEncrypter.createMatrix(snakeEncrypter.Text));

            Stopwatch stopwatch = new Stopwatch();
            long freq = Stopwatch.Frequency;

            stopwatch.Start();
            string resultEnc = snakeEncrypter.Encrypt();
            stopwatch.Stop();

            Console.WriteLine($"ИСХОДНЫЙ ТЕСКТ:  {germanText}");
            Console.WriteLine($"\nЗАШИФРОВАННЫЙ ТЕКСТ:  {resultEnc}");
            Console.WriteLine($"\nВРЕМЯ ШИФРОВАНИЯ:  {(double)stopwatch.ElapsedTicks / freq} sec \n");

            stopwatch.Start();
            string resultDecr = snakeEncrypter.Decrypt(resultEnc);
            stopwatch.Stop();

            //snakeEncrypter.printMatrix(snakeEncrypter.createMatrix(resultEnc));

            Console.WriteLine($"ЗАШИФРОВАННЫЙ ТЕСКТ:  {resultEnc}");
            Console.WriteLine($"\nРАСШИФРОВАННЫЙ ТЕКСТ:  {resultDecr}");
            Console.WriteLine($"\nВРЕМЯ РАСШИФРОВАНИЯ:  {(double)stopwatch.ElapsedTicks / freq} sec \n");

            Dictionary<char, double> germanEncSnakeChances = germanChecker.getSymbolsChances(resultEnc, germanDict);

            ExcelDocumentCreator<char, double> excel = new ExcelDocumentCreator<char, double>(new System.IO.FileInfo("Lab5.xlsx"));
            excel.createWorksheet("first");
            excel.addValuesFromDict(germanSnakeChances, "first", 1);
            excel.addValuesFromDict(germanEncSnakeChances, "first", 3);
            excel.pack.Save();
           
        }
    }
}
