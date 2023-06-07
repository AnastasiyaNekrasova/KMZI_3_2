using System;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;

namespace Lab8
{
    internal class RSA
    {
        private static void Main()
        {
            Stopwatch time = new Stopwatch();

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string text = "Nekrasova Anastasiya Pavlovna";
            Console.WriteLine($"Сообщение: {text}\n");

            time.Start();
            byte[] bytetext = Encoding.UTF8.GetBytes($"{text}");
            byte[] crypted = RSAcl.Encryption(bytetext, RSA.ExportParameters(true), false);
            string cryptedText = "";
            time.Stop();
            foreach (byte b in crypted) { 
                cryptedText += b + " ";
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Зашифрованное сообщение:\n{cryptedText} | {(float)time.ElapsedMilliseconds / 1000} c");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();

            time.Reset();

            time.Start();
            string decryptedText = RSAcl.Decryption(crypted, RSA.ExportParameters(true), false);
            time.Stop();
            Console.WriteLine($"Расшифрованное сообщение:\n{decryptedText} | {(float)time.ElapsedMilliseconds / 1000} c");

            Console.ReadKey();
        }
    }


    public class RSAcl
    {
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(512))
            {
                int P = 593, Q = 8;
                RSA.ImportParameters(RSAKey);
                encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
            }
            return encryptedData;
        }

        static public string Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(512))
            {
                RSA.ImportParameters(RSAKey);
                decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
            }
            return Encoding.UTF8.GetString(decryptedData);
        }
    }

}

// p и q

// n e d
// n e - открытый
// n d - закрытый

// n = p * q
// (p – 1)(q – 1) - ф-я Эйлера
// e и ф-я Эйлера взаимно обратные числа
// ed ≡ 1 (mod φ(n)). - находим d
// c ≡ m * e mod n
// m ≡ c * d mod n
