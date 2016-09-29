using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CPEN442Assignment2.Analyzers
{
    public class SimpleSubstitutionAnalyzer
    {
        private Settings settings;
        private string ciphertext;
        private char[] keyOrderArr;
        private string keyOrderString;
        public SimpleSubstitutionAnalyzer(string cipherText)
        {
            this.settings = new Settings();
            this.ciphertext = cipherText;

            keyOrderArr = this.settings.FreqDictionary.OrderByDescending(a => a.Value).Select(a => a.Key).ToArray();
            keyOrderString = (new string(keyOrderArr)).ToLower();
        }

        public void Start()
        {
            var letterCounts = new Dictionary<char, int>();
            for (int i = 0; i < 26; i++)
            {
                letterCounts.Add((char)('A' + i), 0);
            }

            for (int i = 0; i < this.ciphertext.Length; i++)
            {
                letterCounts[this.ciphertext[i]]++;
            }
            var key = new Dictionary<char, char>();
            var orderedLetters = letterCounts.OrderByDescending(a => a.Value).Select(a => a.Key).ToArray();
            for (int i = 0; i < 26; i++)
            {
                key.Add(orderedLetters[i], keyOrderArr[i]);
            }

            Console.WriteLine("Commands");
            Console.WriteLine("'A b' will modify the key to match A to b");
            Console.WriteLine("'a b' will modify the key so what's currently a becomes b");
            Console.WriteLine("'     >' will shift the keys starting at the given position");
            Console.WriteLine("'     <' will shift the keys starting at the given position");


            while (true)
            {
                writeDecipher(key);

                CConsole.WriteLine("TO   : " + keyOrderString, ConsoleColor.White);

                var ksb = new StringBuilder();
                for (int i = 0; i < 26; i++)
                {
                    if (key.Any(a => a.Value.Equals(keyOrderArr[i])))
                    {
                        ksb.Append(key.First(a => a.Value.Equals(keyOrderArr[i])).Key);
                    }
                    else
                    {
                        ksb.Append(' ');
                    }
                }
                var keystr = ksb.ToString();
                //var keystr = new string(key.Select(a=>a.Value).ToArray());


                CConsole.WriteLine("FROM : " + keystr, ConsoleColor.Yellow);

                Console.Write("CMD  : ");
                var keyInput = Console.ReadLine();
                if (Regex.IsMatch(keyInput, @"^[A-z]\s[a-z]$"))
                {
                    if (string.IsNullOrEmpty(keyInput))
                    {
                        break;
                    }

                    var ft = keyInput.Split(' ');
                    if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(ft[0]))
                    {
                        var ch = ft[0].ToCharArray()[0];
                        var to = ft[1].ToUpper().ToCharArray()[0];
                        char had = '~';
                        if (key.Any(a => a.Value.Equals(to)))
                        {
                            had = key.First(a => a.Value.Equals(to)).Key;
                        }
                        key[ch] = to;
                        if (had != '~')
                        {
                            key[had] = ' ';
                        }
                    }
                    else
                    {
                        var q1 = ft[0].ToUpper().ToCharArray()[0];
                        var q2 = ft[1].ToUpper().ToCharArray()[0];
                        var k = key.First(a => a.Value.Equals(q1)).Key;
                        char had = '~';
                        if (key.Any(a => a.Value.Equals(q2)))
                        {
                            had = key.First(a => a.Value.Equals(q2)).Key;
                        }
                        key[k] = q2;
                        if (had != '~')
                        {
                            key[had] = ' ';
                        }

                    }
                }
                else if (keyInput.Last() == '>')
                {
                    var left = keyInput.IndexOf(">");
                    var leave = keystr.Substring(0, left);
                    var right = keystr.Substring(left, 26 - left);
                    var tk = leave + " " + right;
                    key = keyFromString(tk.ToUpper());
                }
                else if (keyInput.Last() == '<')
                {

                }

            }


        }

        private void writeDecipher(Dictionary<char, char> key)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < this.ciphertext.Length; i++)
            {
                if (key[this.ciphertext[i]] == ' ')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(this.ciphertext[i]);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.Write((char)(key[this.ciphertext[i]] + 32));
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("\n");
        }

        private Dictionary<char, char> keyFromString(string k)
        {
            var key = new Dictionary<char, char>();
            for (int i = 0; i < 26; i++)
            {
                key.Add(k[i], keyOrderArr[i]);
            }
            return key;
        }
    }
}
