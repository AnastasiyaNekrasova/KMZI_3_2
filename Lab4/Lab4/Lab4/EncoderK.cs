using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    class EncoderK
    {
        public List<char> alphabet;
        public int k;
        public string editedAlphabet;

        public EncoderK(List<char> alphabet, int k)
        {
            this.alphabet = alphabet;
            this.k = k;
            this.editedAlphabet = editAlphabet(alphabet,k);
        }

        public string editAlphabet(List<char> alphabet,int k)
        {
            StringBuilder newAlphabet = new StringBuilder();
            for(int iter = 0; iter < alphabet.Count;iter++)
            {
                newAlphabet.Append(alphabet[(iter + k) % alphabet.Count]);
            }
            return newAlphabet.ToString();
        }

        public void printEditedAlphabet()
        {
            Console.WriteLine($"\nАлфавит для шифрования:"); 
            foreach (char x in this.editedAlphabet)
            {
                Console.Write(x); Console.Write(" ");
            }

        }

        public string encode(string text)
        {
            StringBuilder encodedText = new StringBuilder();
            for(int iter = 0;iter <text.Length;iter++)
            {
                int pos = this.alphabet.IndexOf(text[iter]);
                char encSymbol = this.editedAlphabet[pos];
                encodedText.Append(encSymbol);
            }
            return encodedText.ToString();
        }

        public string decode(string text)
        {
            StringBuilder decodedText = new StringBuilder();
            for (int iter = 0; iter < text.Length; iter++)
            {
                int pos = this.editedAlphabet.IndexOf(text[iter]);
                char decSymbol = this.alphabet[pos];
                decodedText.Append(decSymbol);
            }
            return decodedText.ToString();
        }
    }
}
