using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;
using System.Diagnostics;

namespace lab8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Stopwatch stopwatch = new Stopwatch();
        long freq = Stopwatch.Frequency;

        private void resPSPButton_Click(object sender, RoutedEventArgs e)
        {
            int n = 256;

            BigInteger p = 19, q = 11;
            Random rand = new Random();
            BigInteger seed = new BigInteger(rand.Next(0, 10));
            //BigInteger seed = 123456789;

            resultPSPText.Text = "Последовательность: \n";
            BBSGenerator bbs = new BBSGenerator(p, q, seed);
            for (int i = 0; i < 10; i++)
            {
                resultPSPText.Text += (bbs.GetNext() + " ");
            }           
           
        }

        byte[] result;

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Start();
            int[] keyArr = new int[] { 61, 60, 23, 22, 21, 20 };
            
            string s = "";
            for (int i = 0; i < keyArr.Length; i++)
            {
                s += Encoding.ASCII.GetString(new byte[] { Convert.ToByte(keyArr[i]) });
            }

            byte[] key = ASCIIEncoding.ASCII.GetBytes(s);
            RC4 encoder = new RC4(key);
            string testString = encryptTextBox.Text;
            byte[] testBytes = ASCIIEncoding.ASCII.GetBytes(testString);
            result = encoder.Encode(testBytes, testBytes.Length);
            stopwatch.Stop();

            decryptTextBox.Text = ASCIIEncoding.ASCII.GetString(result);
            time.Content = $"{(double)stopwatch.ElapsedTicks / freq} sec \n";
            stopwatch.Reset();

        }

        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Start();
            int[] keyArr = new int[] { 61, 60, 23, 22, 21, 20 };
            string s = "";
            for (int i = 0; i < keyArr.Length; i++)
            {
                s += Encoding.ASCII.GetString(new byte[] { Convert.ToByte(keyArr[i]) });
            }

            byte[] key = ASCIIEncoding.ASCII.GetBytes(s);

            RC4 decoder = new RC4(key);
            byte[] decryptedBytes = decoder.Decode(result, result.Length);
            string decryptedString = ASCIIEncoding.ASCII.GetString(decryptedBytes);
            stopwatch.Stop();

            encryptTextBox.Text = decryptedString;
            time.Content = $"{(double)stopwatch.ElapsedTicks / freq} sec \n";
        }
    }
}
