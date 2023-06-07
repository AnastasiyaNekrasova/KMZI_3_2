using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Lab6
{
    public partial class MainWindow : Window
    {
        string original_alphabet = "abcdefghijklmnopqrstuvwxyz";
        Rotor L, M, R;
        Reflector reflector;
        public int count = 3;

        public MainWindow()
        {
            original_alphabet.ToCharArray();
            InitializeComponent();
            L = new Rotor
            {
                Alphabet = new Char[] { 'f', 'k', 'q', 'h', 't', 'l', 'x', 'o', 'c',
                                        'b', 'j', 's', 'p', 'd', 'z', 'r', 'a', 'm',
                                        'e', 'w', 'n', 'i', 'u', 'y', 'g', 'v' },
                Shift = 1
            };
            M = new Rotor
            {
                Alphabet = new Char[] { 'a', 'j', 'd', 'k', 's', 'i', 'r', 'u','x',
                                        'b', 'l', 'h', 'w', 't', 'm', 'c', 'q','g',
                                        'z', 'n', 'p', 'y', 'f', 'v','o','e' },
                Shift = 0,

            };
            R = new Rotor
            {
                Alphabet = new Char[] { 'e', 's', 'o', 'v', 'p', 'z', 'j', 'a','y',
                                        'q', 'u', 'i', 'r', 'h', 'x', 'l', 'n','f',
                                        't', 'g', 'k', 'd', 'c', 'm','w','b' },
                Shift = 1,
            };

            reflector = new Reflector();
            reflector.Alphabet = new Dictionary<char, char> { { 'a', 'y' }, { 'b', 'r' }, { 'c', 'u' }, { 'd', 'h' }, { 'e', 'q' }, { 'f', 's' }, { 'g', 'l' },
                                                              { 'i', 'p' }, { 'j', 'x' }, { 'k', 'n' }, { 'm', 'o' }, { 't', 'z' }, { 'v', 'w' }};
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            string original_message = Orig_message_input.Text.ToLower().Replace(" ", "");
            char encrypt_symbol;
            string encrypt_message = "";

            SymbolsChecker germanChecker = new SymbolsChecker(original_alphabet);
            Dictionary<char, int> germanDict = germanChecker.alphabetListToDictionary();
            germanChecker.getSymbolsCounts(original_message, germanDict);
            Dictionary<char, double> origMesSymbChances = germanChecker.getSymbolsChances(original_message, germanDict);

            L.PickStartPosition(Start_position_Rot_L.Items[Start_position_Rot_L.SelectedIndex].ToString());
            M.PickStartPosition(Start_position_Rot_M.Items[Start_position_Rot_M.SelectedIndex].ToString());
            R.PickStartPosition(Start_position_Rot_R.Items[Start_position_Rot_R.SelectedIndex].ToString());

            for (int i = 0; i < original_message.Length; i++)
            {
                encrypt_symbol = R.Alphabet[original_alphabet.IndexOf(original_message[i])];
                encrypt_symbol = M.Alphabet[original_alphabet.IndexOf(encrypt_symbol) ];
                encrypt_symbol = L.Alphabet[original_alphabet.IndexOf(encrypt_symbol) ];
                char encrypt_symbolFromRefl;
                if (!reflector.Alphabet.TryGetValue(encrypt_symbol, out encrypt_symbolFromRefl))
                    encrypt_symbol = reflector.Alphabet.First(key => key.Value == encrypt_symbol).Key;
                else
                    encrypt_symbol = encrypt_symbolFromRefl;
                encrypt_symbol = original_alphabet[L.GetSymbolPosition(encrypt_symbol)];
                encrypt_symbol = original_alphabet[M.GetSymbolPosition(encrypt_symbol)];
                encrypt_message += original_alphabet[R.GetSymbolPosition(encrypt_symbol)];
                L.DoShift(M.DoShift(R.DoShift(0)));
            }
            Encrypt_Output.Text = encrypt_message;

            germanChecker.getSymbolsCounts(encrypt_message, germanDict);
            Dictionary<char, double> encrMesSymbChances = germanChecker.getSymbolsChances(encrypt_message, germanDict);

            ExcelDocumentCreator<char, double> excel = new ExcelDocumentCreator<char, double>(new System.IO.FileInfo("Lab6.xlsx"));
            excel.createWorksheet("first");
            excel.addValuesFromDict(origMesSymbChances, "first", 1);
            if (count != 0)
            {
                excel.addValuesFromDict(encrMesSymbChances, "first", count);
                count += 2;
            }
            excel.pack.Save();
        }
    }
}
