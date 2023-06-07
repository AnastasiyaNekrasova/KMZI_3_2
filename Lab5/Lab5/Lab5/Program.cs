using Lab2;
using Lab2.DocumentReader;
using Lab5.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<char> germanAlph = new List<char>()
{
                '\u0061', '\u00E4', '\u0062', '\u0063', '\u0064', 
                '\u0065', '\u0066', '\u0067', '\u0068', '\u0069', 
                '\u006A', '\u006B', '\u006C', '\u006D', '\u006E', 
                '\u006F', '\u00F6', '\u0070', '\u0071', '\u0072', 
                '\u0073', '\u00DF', '\u0074', '\u0075', '\u00FC', 
                '\u0076', '\u0077', '\u0078', '\u0079', '\u007A'
                 
            };

            int[] keyH = new int[] { 5, 3, 4, 7, 1, 8, 6, 9, 2 };
            char[] keyHword = new char[] { 'n', 'e', 'k', 'r', 'a', 's', 'o', 'v', 'a' };
            int[] keyV = new int[] { 1, 6, 2, 7, 9, 3, 8, 5, 4 };
            char[] keyVword = new char[] { 'a', 'n', 'a', 's', 't', 'a', 's', 'i', 'a' };

            List<KeyValuePair<int,char>>keyVertical = new List<KeyValuePair<int, char>>();
            List<KeyValuePair<int, char>> keyHorizontal = new List<KeyValuePair<int, char>>();

            for (int i =0; i < keyV.Length;i++)
            {
                keyVertical.Add(new KeyValuePair<int,char>(keyV[i],keyVword[i]));
            }
            for (int i = 0; i < keyH.Length; i++)
            {
                keyHorizontal.Add(new KeyValuePair<int, char>(keyH[i],keyHword[i]));
            }

            try
            {
                SnakeService.MakeSnake(germanAlph);
                ReplacementService.MakeReplace(germanAlph,keyVertical,keyHorizontal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}");
            }

        }
    }
}
