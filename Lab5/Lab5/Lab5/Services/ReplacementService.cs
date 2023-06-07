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
    static class ReplacementService
    {
        public static void MakeReplace(List<char> germanAlph,List<KeyValuePair<int,char>> keyVertical, List<KeyValuePair<int, char>> keyHorizontal)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("                 МНОЖЕСТВЕННАЯ ПЕРЕСТАНОВКА");
            Console.WriteLine("------------------------------------------------------------");
            Console.ResetColor();
            EntropyChecker germanChecker = new EntropyChecker(germanAlph, 0, "Немецкий");
            string germanText = germanChecker.OpenDocument("german.txt").ReadToEnd().ToLower();
            //Regex regex = new Regex(@"\W");
            //germanText = regex.Replace(germanText, "");

            List<char> germanTextTrimmedList = germanText.Take(keyVertical.Count * keyHorizontal.Count).ToList();
            StringBuilder germanTextTrimmedBuilder = new();
            foreach(char x in germanTextTrimmedList)
            {
                germanTextTrimmedBuilder.Append(x);
            }
            string germanTextTrimmed = germanTextTrimmedBuilder.ToString();
            MultipleReplacement replaceEncrypter = new MultipleReplacement(germanTextTrimmed, keyVertical, keyHorizontal);

            Dictionary<char, int> germanDict = germanChecker.alphabetListToDictionary();
            germanChecker.getSymbolsCounts(germanTextTrimmed, germanDict);
            Dictionary<char, double> germanChances = germanChecker.getSymbolsChances(germanTextTrimmed, germanDict);

            germanChecker.printAlphabet();

            replaceEncrypter.printMatrix(replaceEncrypter.createMatrix(replaceEncrypter.Text));

            Stopwatch stopwatch = new Stopwatch();
            long freq = Stopwatch.Frequency;

            stopwatch.Start();
            string resultEnc = replaceEncrypter.Encrypt();
            stopwatch.Stop();

            Console.WriteLine($"Исходный текст:       {germanTextTrimmed}");
            Console.WriteLine($"Зашифрованный текст:  {resultEnc}");
            Console.WriteLine($"Время шифрования:     {(double)stopwatch.ElapsedTicks / freq} sec \n"); 

            Console.WriteLine("====================================================================\n");

            replaceEncrypter.printMatrix(replaceEncrypter.encryptedMatrix);

            stopwatch.Start();
            string resultDecr = replaceEncrypter.Decrypt();
            stopwatch.Stop();

            Console.WriteLine($"Зашифрованный текст:  {resultEnc}");
            Console.WriteLine($"Расшифрованный текст: {resultDecr}");
            Console.WriteLine($"Время расшифрования:  {(double)stopwatch.ElapsedTicks / freq} sec \n");

            Console.WriteLine("==========================U'VE DID IT!!!===========================\n");

            Dictionary<char, double> germanEncChances = germanChecker.getSymbolsChances(resultEnc, germanDict);

            ExcelDocumentCreator<char, double> excel = new ExcelDocumentCreator<char, double>(new System.IO.FileInfo("Lab5.xlsx"));
            excel.createWorksheet("first");
            excel.addValuesFromDict(germanChances, "first", 6);
            excel.addValuesFromDict(germanEncChances, "first", 8);
            excel.pack.Save();

        }
    }
}
