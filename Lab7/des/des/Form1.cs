using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace des
{
    public partial class Form1 : Form
    {
        string path = "note1.txt";

        Stopwatch stopwatch = new Stopwatch();
        long freq = Stopwatch.Frequency;

        public string X, Xbin, k, XIP;

        public string encoded, decoded;
        
        private const int sizeOfBlock = 64; //размер блока
        private const int sizeOfChar = 8; //размер одного символа 
        string[] Blocks, binBlocks; //сами блоки

        public int[] IP = new int[] //начальная пер. (initial permutation)
        {
            58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17,  9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 
        };

        // ПРЕОБРАЗОВАНИЯ КЛЮЧА

        static public int[] keyP = new int[] //перестановка ключа
        {
            57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4
        };

        //число битов сдвига ключа в зависимости от этапа 
        static public int[] shiftKey = new int[] { 2, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        public int[] PwCompress = new int[] //перестановка со сжатием
        {
            14, 17, 11, 24, 1,  5,  3,  28, 15, 6,  21, 10,
            23, 19, 12, 4,  26, 8,  16, 7,  27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
        };

        // ПЕРЕСТАНОВКА С РАСШИРЕНИЕМ

        public int[] PwExtens = new int[] //перестановка с расширением
        {
            32, 1,  2,  3,  4,  5,  4,  5,  6,  7,  8,  9,
            8,  9,  10, 11, 12, 13, 12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1
        };

        // ПОДСТАНОВКА С ПОМОЩЬЮ S-БЛОКОВ
        static int[,] s1 = new int[,]
        {
            { 14, 4,  13, 1, 2,  15, 11, 8,  3,  10, 6,  12, 5,  9,  0, 7  },
            { 0,  15, 7,  4, 14, 2,  13, 1,  10, 6,  12, 11, 9,  5,  3, 8  },
            { 4,  1,  14, 8, 13, 6,  2,  11, 15, 12, 9,  7,  3,  10, 5, 0  },
            { 15, 12, 8,  2, 4,  9,  1,  7,  5,  11, 3,  14, 10, 0,  6, 13 } 
        };

        static int[,] s2 = new int[,]
        {
            { 15, 1,  8,  14, 6,  11, 3,  4,  9,  7, 2,  13, 12, 0, 5,  10 },
            { 3,  13, 4,  7,  15, 2,  8,  14, 12, 0, 1,  10, 6,  9, 11, 5  },
            { 0,  14, 7,  11, 10, 4,  13, 1,  5,  8, 12, 6,  9,  3, 2,  15 },
            { 13, 8,  10, 1,  3,  15, 4,  2,  11, 6, 7,  12, 0,  5, 14, 9  } 
        };

        static int[,] s3 = new int[,]
        {
            { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
            { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
            { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
            { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 } 
        };

        static int[,] s4 = new int[,]
        {
            { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
            { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
            { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
            { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 } 
        };

        static int[,] s5 = new int[,]
        {
            { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
            { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
            { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
            { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 } 
        };

        static int[,] s6 = new int[,]
        {
            { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
            { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
            { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
            { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 } 
        };

        static int[,] s7 = new int[,]
        {
            { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
            { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
            { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
            { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 } 
        };

        static int[,] s8 = new int[,]
        {
            { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
            { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
            { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
            { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 } 
        };

        static List<int[,]> listSMatrix = new List<int[,]> { s1, s2, s3, s4, s5, s6, s7, s8 };

        // ПЕРЕСТАНОВКА С ПОМОЩЬЮ P-БЛОКОВ

        public int[] P = new int[]
        {
            16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25
        };

        public int[] FP = new int[] //конечная пер. (final permutation)
        {
            40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void encodeBtn_Click(object sender, EventArgs e)
        {
            infoEN.Text = $"----- ENCODE -----\n{textBox1.Text}\n";
            stopwatch.Start();
            DESencode(textBox1.Text, textBox2.Text);
            infoEN.Text += $"{encoded}\n";
            DESencode(encoded, textBox3.Text);
            infoEN.Text += $"{encoded}\n";
            DESencode(encoded, textBox2.Text);
            infoEN.Text += $"{encoded}\n";
            stopwatch.Stop();
            time.Text = $"{(double)stopwatch.ElapsedTicks / freq} sec \n";
            stopwatch.Reset();
        }

        private void decodeBtn_Click(object sender, EventArgs e)
        {
            infoDE.Text += $"----- DECODE -----\n{encoded}\n";
            stopwatch.Start();
            DESdecode(encoded, textBox2.Text);
            infoDE.Text += $"{encoded}\n";
            DESdecode(encoded, textBox3.Text);
            infoDE.Text += $"{encoded}\n";
            DESdecode(encoded, textBox2.Text);
            infoDE.Text += $"{encoded}\n";
            stopwatch.Stop();
            time.Text = $"{(double)stopwatch.ElapsedTicks / freq} sec \n";
            stopwatch.Reset();
        }

        public void DESencode(string mes, string key)
        {
            X = ""; Xbin = ""; k = ""; XIP = "";
            Blocks = new string[0];
            binBlocks = new string[0];
            richTextBox.Text = "";
            
            X = mes;
            X = StringToRightLength(X);
            CutStringIntoBlocks(X);
            string blockBin;
            foreach (string block in Blocks)
            {
                blockBin = BinaryConvert.ToBinary(BinaryConvert.ConvertToByteArray(block, Encoding.Default));
                Xbin += blockBin;
            }
            richTextBox.Text += "mes = " + Xbin;
            k = Permut(BinaryConvert.ToBinary(BinaryConvert.ConvertToByteArray(key, Encoding.Default)), keyP);
            richTextBox.Text += "\nk = " + k + '\n';
            CutBinaryStringIntoBlocks(Xbin);
            foreach (string binBlock in binBlocks)
            {
                string binBlockIP = Permut(binBlock, IP);
                XIP += binBlockIP;
            }
            CutBinaryStringIntoBlocks(XIP);

            string L, R, L1, R1, R2, K, Y, mess = "";
            foreach (string binBlock in binBlocks)
            {
                richTextBox.Text += $"BLOCK {Array.IndexOf(binBlocks, binBlock)}" + '\n';
                Y = "";
                string block = binBlock;
                for (int i = 1; i <= shiftKey.Length; i++)
                {
                    L = block.Substring(0, 32);
                    R = block.Substring(32);
                    K = GenKey(k, i);
                    R1 = f(R, K);
                    L1 = R;
                    R2 = XOR(L, R1);
                    Y = String.Concat(L1, R2);
                    richTextBox.Text += $"Round {i} = " + Y + '\n';
                    block = Y;
                }
                Y = Permut(Y, FP);
                richTextBox.Text += $"Final Permutation = " + Y + '\n';
                mess += Y;
            }
            encoded = BinaryConvert.ConvertBytesToString(BinaryConvert.GetBytes(mess), Encoding.Default);
            richTextBox.Text += $"ENCODED " + encoded + '\n';
            textBox1.Text = "";
            foreach (char symbol in encoded)
            {
                textBox1.Text += symbol;
            }
        }

        public void DESdecode(string mes, string key)
        {
            X = ""; Xbin = ""; k = ""; XIP = "";
            Blocks = new string[0];
            binBlocks = new string[0];

            X = mes;
            X = StringToRightLength(X);
            CutStringIntoBlocks(X);
            string blockBin;
            foreach (string block in Blocks)
            {
                blockBin = BinaryConvert.ToBinary(BinaryConvert.ConvertToByteArray(block, Encoding.Default));
                Xbin += blockBin;
            }
            k = Permut(BinaryConvert.ToBinary(BinaryConvert.ConvertToByteArray(key, Encoding.Default)), keyP);
            CutBinaryStringIntoBlocks(Xbin);
            foreach (string binBlock in binBlocks)
            {
                string binBlockIP = Permut(binBlock, IP);
                XIP += binBlockIP;
            }
            CutBinaryStringIntoBlocks(XIP);

            string L, R, L1, R1, L2, K, Y, mess = "";
            foreach (string binBlock in binBlocks)
            {
                richTextBox.Text += $"BLOCK {Array.IndexOf(binBlocks, binBlock)}" + '\n';
                Y = "";
                string block = binBlock;
                for (int i = shiftKey.Length; i > 0; i--)
                {
                    L = block.Substring(0, 32);
                    R = block.Substring(32);
                    K = GenKey(k, i);
                    L1 = f(L, K);
                    R1 = L;
                    L2 = XOR(R, L1);
                    Y = String.Concat(L2, R1);
                    richTextBox.Text += $"Round {i} = " + Y + '\n';
                    block = Y;
                }
                Y = Permut(Y, FP);
                richTextBox.Text += $"Final Permutation = " + Y + '\n';
                mess += Y;
            }
            encoded = BinaryConvert.ConvertBytesToString(BinaryConvert.GetBytes(mess), Encoding.Default);
            richTextBox.Text += $"DECODED " + encoded + '\n';
            textBox1.Text = encoded;
        }


        private string StringToRightLength(string input)
        {
            while (((input.Length * sizeOfChar) % sizeOfBlock) != 0)
                input += "#";

            return input;
        }

        private void CutStringIntoBlocks(string input)
        {
            Blocks = new string[(input.Length * sizeOfChar) / sizeOfBlock];
            int lengthOfBlock = input.Length / Blocks.Length;
            for (int i = 0; i < Blocks.Length; i++)
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);

        }

        private void CutBinaryStringIntoBlocks(string input)
        {
            binBlocks = new string[input.Length / sizeOfBlock];
            int lengthOfBlock = input.Length / Blocks.Length;
            for (int i = 0; i < Blocks.Length; i++)
                binBlocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
        }

        public string ChangeSMatrix(string mess6Bit, int numSMatrix)
        {
            StringBuilder temp = new StringBuilder();
            int adrX = Convert.ToInt32(temp.Append(mess6Bit[0]).Append(mess6Bit[5]).ToString(), 2);
            int adrY = Convert.ToInt32(mess6Bit.Substring(1, 4), 2);
            string res = Convert.ToString(listSMatrix[numSMatrix][adrX, adrY], 2);
            if (res.Length < 4)
                res = res.PadLeft(4, '0');
            return res;
        }

        public string GenKey(string oldKey56, int round)
        {
            var temp1 = oldKey56.Substring(0, 28);
            var temp2 = oldKey56.Substring(28);
            StringBuilder key1 = new StringBuilder();
            key1.Append(temp1.Substring(temp1.Length - shiftKey[round - 1]));
            key1.Append(temp1.Substring(shiftKey[round - 1]));
            StringBuilder key2 = new StringBuilder();
            key2.Append(temp2.Substring(temp2.Length - shiftKey[round - 1]));
            key2.Append(temp2.Substring(shiftKey[round - 1]));
            StringBuilder sb = new StringBuilder();
            foreach (var item in PwCompress)
            {
                sb.Append(key1.Append(key2)[item - 1]);
            }
            return sb.ToString();
        }

        public string Permut(string str, int[] matrix)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in matrix)
            {
                sb.Append(str[item - 1]);
            }
            return sb.ToString();
        }

        public string f(string mess32Bit, string key)
        {
            StringBuilder resStr = new StringBuilder();
            string mess = XOR(Permut(mess32Bit, PwExtens), key);
            for (int i = 0; i < mess.Length; i = i + 6)
            {
                resStr.Append(ChangeSMatrix(mess.Substring(i, 6), i / 6));
            }
            return Permut(resStr.ToString(), P);
        }
        public string XOR(string value1, string value2)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < value1.Length; i++)
            {
                if (value1[i].Equals(value2[i]))
                    sb.Append("0");
                else
                    sb.Append("1");
            }
            return sb.ToString();
        }

       
    }
}