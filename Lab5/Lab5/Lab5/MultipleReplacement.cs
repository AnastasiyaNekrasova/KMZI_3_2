using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    class MultipleReplacement
    {
        private List<KeyValuePair<int, char>> keyHorizontal;
        private List<KeyValuePair<int, char>> keyVertical;
        public char[,] encryptedMatrix;
        public char[,] decryptedMatrix;

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public List<KeyValuePair<int, char>> KeyVertical
        {
            get { return keyVertical; }
            set { keyVertical = value; }
        }

        public List<KeyValuePair<int, char>> KeyHorizontal
        {
            get { return keyHorizontal; }
            set { keyHorizontal = value; }
        }

        public MultipleReplacement(string text, List<KeyValuePair<int, char>> keyVertical, List<KeyValuePair<int, char>> keyHorizontal)
        {
            if (keyVertical.Count * keyHorizontal.Count >= text.Length)
            {
                Text = text;
            }
            else
            {
                throw new Exception("Сообщение слишком длинное для данных ключей!");
            }
            KeyVertical = keyVertical;
            KeyHorizontal = keyHorizontal;
        }

        public char[,] createMatrix(string input)
        {
            int tableHeight = keyVertical.Count + 2;
            int tableWidth = KeyHorizontal.Count + 2;
            char[,] table = new char[tableHeight, tableWidth];
            int l = 0;
            for (int w = 0; w < tableHeight; w++)
            {

                for (int i = 0; i < tableWidth; i++)
                {
                    if ((i == 0 || i == 1) && (w == 0 || w == 1))
                    {
                        table[w, i] = '-';
                    }
                    else if (i == 0 || i == 1)
                    {
                        if (i == 1) table[w, i] = Convert.ToChar(Convert.ToString(keyVertical[w - 2].Key));
                        else table[w, i] = Convert.ToChar(keyVertical[w - 2].Value);
                    }
                    else if (w == 0 || w == 1)
                    {
                        if (w == 1) table[w, i] = Convert.ToChar(Convert.ToString(keyHorizontal[i - 2].Key));
                        else table[w, i] = Convert.ToChar(keyHorizontal[i - 2].Value);
                    }
                    else
                    {
                        table[w, i] = input[l++];
                    }
                }
                if (l == input.Length) break;
            }
            return table;
        }

        public void printMatrix(char[,] input)
        {
            int tableHeight = keyVertical.Count + 2;
            int tableWidth = KeyHorizontal.Count + 2;
            for (int w = 0; w < tableHeight; w++)
            {
                for (int i = 0; i < tableWidth; i++)
                {
                    if (i == 0 || i == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (w == 0 || w == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write($"{(input[w, i] == ' ' || input[w, i] == '\0' ? '_' : input[w, i]),5}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write($"\n");

            }
            Console.Write($"\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public string createString(char[,] input)
        {
            int tableHeight = keyVertical.Count + 2;
            int tableWidth = KeyHorizontal.Count + 2;
            StringBuilder stringBuilder = new StringBuilder();
            for (int w = 2; w < tableHeight; w++)
            {
                for (int l = 2; l < tableWidth; l++)
                    stringBuilder.Append(input[w, l]);
            }
            return stringBuilder.ToString();
        }

        public string Encrypt()
        {
            Console.WriteLine("-------------------------ШИФРОВАНИЕ-------------------------\n");
            char[,] table = createMatrix(Text);

            int tableHeight = keyVertical.Count + 2;
            int tableWidth = KeyHorizontal.Count + 2;

            char[,] result = new char[tableHeight, tableWidth];
            int iteration = 0;
            while (iteration++ < tableWidth - 2)
            {
                int k = 0;
                for (int y = 0; y < tableHeight; y++)
                {
                    while (k != 2)
                    {
                        result[y, k] = table[y, k];
                        k++;
                    }
                    k = keyHorizontal.IndexOf(keyHorizontal.Where(l => l.Key == iteration).First());
                    result[y, iteration + 1] = table[y, k + 2];
                    k = 0;

                }
            }

            Console.WriteLine("Горизонтальная перестановка:\n");
            printMatrix(result);

            char[,] resultV = new char[tableHeight, tableWidth];
            iteration = 0;
            while (iteration++ < tableHeight - 2)
            {
                int k = 0;
                for (int y = 0; y < tableWidth; y++)
                {
                    while (k != 2)
                    {
                        resultV[k, y] = result[k, y];
                        k++;
                    }
                    k = keyVertical.IndexOf(keyVertical.Where(l => l.Key == iteration).First());
                    resultV[iteration + 1, y] = result[k + 2, y];
                    k = 0;
                }
            }

            Console.WriteLine("Вертикальная перестановка:\n");
            printMatrix(resultV);

            encryptedMatrix = resultV;
            return createString(encryptedMatrix);
        }

        public string Decrypt()
        {
            Console.WriteLine("-----------------------РАСШИФРОВАНИЕ------------------------\n");

            char[,] table = encryptedMatrix;

            int tableHeight = keyVertical.Count + 2;
            int tableWidth = KeyHorizontal.Count + 2;

            char[,] resultV = new char[tableHeight, tableWidth];
            int iteration = 0;
            while (iteration++ < tableHeight - 2)
            {
                int k = 0;
                for (int y = 0; y < tableWidth; y++)
                {
                    while (k != 2)
                    {
                        resultV[k, y] = table[k, y];
                        k++;
                    }
                    k = keyVertical.IndexOf(keyVertical.Where(l => l.Key == iteration).First());
                    resultV[k + 2, y] = table[iteration + 1, y];
                    k = 0;
                }
            }

            Console.WriteLine("Вертикальная перестановка:\n");
            printMatrix(resultV);

            char[,] result = new char[tableHeight, tableWidth];
            iteration = 0;
            while (iteration++ < tableWidth - 2)
            {
                int k = 0;
                for (int y = 0; y < tableHeight; y++)
                {
                    while (k != 2)
                    {
                        result[y, k] = resultV[y, k];
                        k++;
                    }
                    k = keyHorizontal.IndexOf(keyHorizontal.Where(l => l.Key == iteration).Min());
                    result[y, k + 2] = resultV[y, iteration + 1];
                    k = 0;

                }
            }

            Console.WriteLine("Горизонтальная перестановка:\n");
            printMatrix(result);

            decryptedMatrix = result;
            return createString(decryptedMatrix);
        }
    }
}
