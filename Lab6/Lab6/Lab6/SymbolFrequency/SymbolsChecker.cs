using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class SymbolsChecker 
    {
        private string alphabet;

        public SymbolsChecker(string alphabet)
        {
            Alphabet = alphabet;
        }

        public string Alphabet
        {
            get { return alphabet; }
            set { alphabet = value; }
        }

        public Dictionary<char, int> alphabetListToDictionary()
        {
            Dictionary<char, int> dict = new Dictionary<char, int>(Alphabet.Count());
            foreach(char x in alphabet)
            {
                dict.Add(x, 0);
            }
            return dict;
        }

        public string GetAllText(string text, StreamReader reader)
        {
            if(reader == null)
            {
                throw new Exception("Document isn't open");
            }
            else
            {
                return reader.ReadToEnd();
            }
        }

        public Dictionary<char, double> getSymbolsChances( string text, Dictionary<char, int> counts)
        {
            Dictionary<char, double> chances = new Dictionary<char, double>(this.alphabet.Length);

            for (int i = 0; i < counts.Count(); i++)
            {
                chances.Add(this.alphabet[i],(double)counts[this.alphabet[i]] / text.Length);
            }

            return chances;
        }

        public void getSymbolsCounts(string text, Dictionary<char, int> alphabet)
        {
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < this.alphabet.Count(); j++)
                {
                    if (text[i] == this.alphabet[j])
                    {
                        alphabet[this.alphabet[j]]++;
                    }
                }
            }
        }
       
        public StreamReader OpenDocument(string path)
        {
            return new StreamReader(path);
        }

    }
}
