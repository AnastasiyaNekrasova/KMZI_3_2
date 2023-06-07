using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    class SnakeEncrypter
    {
        private int tableWidth;
        private int tableHeight;
        private string text;


        public int TableHeight
        {
            get { return tableWidth; }
            set { tableWidth = value; }
        }

        public int TableWidth
        {
            get { return tableHeight; }
            set { tableHeight = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public SnakeEncrypter(int tableHeight, int tableWidth, string text)
        {
            TableHeight = tableHeight;
            TableWidth = tableWidth;
            Text = text;
        }

        public char[,] createMatrix(string input)
        {
            char[,] table = new char[tableHeight, tableWidth];
            int l = 0;
            for (int w = 0; w < tableHeight; w++)
            {
                for (int i = 0; i < tableWidth; i++)
                {
                    if (l == input.Length) break;
                    table[w, i] = input[l++];
                }
                if (l == input.Length) break;
            }
            return table;
        }

        public string createString(char[] input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int w = 0; w < input.Length; w++)
            {
                stringBuilder.Append(input[w]);
            }
            return stringBuilder.ToString();
        }

        public string createString(char[,] input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int w = 0; w < tableHeight; w++)
            {
                for (int l = 0; l < tableWidth; l++)
                    stringBuilder.Append(input[w, l]);
            }
            return stringBuilder.ToString();
        }

        public void printMatrix(char[,] input)
        {
            for (int w = 0; w < tableHeight; w++)
            {
                for (int i = 0; i < tableWidth; i++)
                {
                    Console.Write($"{(input[w, i] == ' ' ? '*' : input[w, i])} ");
                }
                Console.Write($"\n");

            }
            Console.Write($"\n");
        }

        public string Encrypt()
        {
            Console.WriteLine("---------------------------------------------------------------- Ш И Ф Р О В А Н И Е ----------------------------------------------------------------\n");
            char[,] table = createMatrix(text);
            char[] result = new char[text.Length];
            int x = 0, y = 0, l = 0;

            while (true)
            {
                if (x < tableWidth)
                {
                    if (table[y, x] != '\0')
                        result[l++] = table[y, x++];
                    else
                    {
                        x++;
                    }
                }
                else
                {
                    x++;
                }

                while (x != 0)
                {
                    if (y < tableHeight && x < tableWidth)
                    {
                        if (table[y, x] != '\0')
                            result[l++] = table[y++, x--];
                        else
                        {
                            y++; x--;
                        }
                    }
                    else
                    {
                        y++; x--;
                    }
                }


                if (y < tableHeight)
                {
                    if (table[y, x] != '\0')
                        result[l++] = table[y++, x];
                    else
                    {
                        y++;
                    }
                }
                else
                {
                    y++;
                }

                while (y != 0)
                {
                    if (y < tableHeight && x < tableWidth)
                    {
                        if (table[y, x] != '\0')
                            result[l++] = table[y--, x++];
                        else
                        {
                            y--; x++;
                        }
                    }
                    else
                    {
                        y--; x++;
                    }
                }
                if (l == text.Length) break;
            }

            return createString(result);
        }

        public string Decrypt(string input)
        {
            Console.WriteLine("------------------------------------------------------------- Р А С Ш И Ф Р О В А Н И Е -------------------------------------------------------------\n");
            char[,] table = new char[tableHeight, tableWidth];
            int x = 0, y = 0, l = 0;
            List<KeyValuePair<int, int>> decryptedMes = new List<KeyValuePair<int, int>>();

            while (true)
            {
                if (x < tableWidth)
                {
                    if (decryptedMes.Where(z => z.Key == x && z.Value == y).Count() == 0)
                        table[y, x++] = input[l++];
                    else
                    {
                        x++;
                    }
                }
                else
                {
                    x++;
                }

                while (x != 0)
                {
                    if (l == text.Length) break;

                    if (y < tableHeight && x < tableWidth)
                    {
                        if (decryptedMes.Where(z => z.Key == x && z.Value == y).Count() == 0)
                            table[y++, x--] = input[l++];
                        else
                        {
                            y++; x--;
                        }
                    }
                    else
                    {
                        y++; x--;
                    }
                }


                if (y < tableHeight)
                {
                    if (decryptedMes.Where(z => z.Key == x && z.Value == y).Count() == 0)
                        table[y++, x] = input[l++];
                    else
                    {
                        y++;
                    }
                }
                else
                {
                    y++;
                }

                while (y != 0)
                {
                    if (l == text.Length) break;

                    if (y < tableHeight && x < tableWidth)
                    {
                        if (decryptedMes.Where(z => z.Key == x && z.Value == y).Count() == 0)
                            table[y--, x++] = input[l++];
                        else
                        {
                            y--; x++;
                        }
                    }
                    else
                    {
                        y--; x++;
                    }
                }
                if (l == text.Length) break;
            }

            return createString(table);
        }
    }
}

