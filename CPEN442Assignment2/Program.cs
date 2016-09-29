using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CPEN442Assignment2.Analyzers;


namespace CPEN442Assignment2
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Info
            Console.WriteLine("Author: Jae Yeong Bae 44316107");
            Console.WriteLine("Undergraduate student at");
            Console.WriteLine("University Of British Columbia");
            Console.WriteLine("");
            Console.WriteLine("Date: 2016-09-27");
            Console.WriteLine("This tool has been written for Assignement #2 of CPEN 442 Course");
            Console.WriteLine("Winter Term 2016");
            Console.WriteLine("");
            Console.WriteLine("All code is written by me as required in the outline of the assignment.");
            Console.WriteLine("This tool and/or code will not be shared until the deadline of the assignemtn.");
            Console.WriteLine("");
            Console.WriteLine("");
            CConsole.WriteLine("What do you want to solve?", ConsoleColor.Yellow);

            Console.WriteLine("(s)imple-substitution, (v)igenere, (c)rc-32 duplicate-find");
            var mode = Console.ReadLine();
            if (mode == "v")
            {
                CConsole.WriteLine("Vigenere selected", ConsoleColor.Cyan);
                Console.WriteLine("Enter length of Key to search (From)");
                var keyLengthFrom = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter length of Key to search (To)");
                var keyLengthTo = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter the minimum dictionary match score to consider (try 10-50)");
                var scoreThresold = Convert.ToInt32(Console.ReadLine());
                
                Console.WriteLine("Select the ciphertext file");
                var ciphertextFilename = "";
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        ciphertextFilename = ofd.FileName;
                    }
                    else
                    {
                        Exit(-1, "Must select a file");
                    }
                }

                var cipherText = File.ReadAllText(ciphertextFilename);

                if (!Regex.IsMatch(cipherText, @"^[A-Z]+$"))
                {
                    Exit(-1, "Cipher File is not valid"); ;
                }

                var vigenere = new VigenereAnalyzer(keyLengthFrom, keyLengthTo, cipherText, scoreThresold);
                CConsole.WriteLine("Starting analysis.", ConsoleColor.Green);
                vigenere.Start();
            }
            else if (mode == "s")
            {
                Console.WriteLine("Select the ciphertext file");
                var ciphertextFilename = "";
                using (var ofd = new OpenFileDialog())
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        ciphertextFilename = ofd.FileName;
                    }
                    else
                    {
                        Exit(-1, "Must select a file");
                    }
                }

                var cipherText = File.ReadAllText(ciphertextFilename);

                var ssa = new SimpleSubstitutionAnalyzer(cipherText);
                ssa.Start();
                
            }
            else if (mode == "c")
            {
                //var inp = Console.ReadLine();
                //var crc = CRC.test(inp);

                Console.WriteLine("write string (x)");
                var inp = Console.ReadLine();
                Console.WriteLine("write another string (start point of y)");
                var inp2 = Console.ReadLine();
                var crc = CRC.FindDuplicateFromString(inp, inp2);


                //Console.WriteLine("write hex");
                //var inp = Console.ReadLine();
                //var crc = CRC.FindDuplicateFromHex(inp);

                Console.WriteLine(crc);
            }
            else
            {
                Exit(-1, "Invalid input");
            }


            Exit(0);
        }


        static void Exit(int exitCode, string errmsg)
        {
            if (!string.IsNullOrEmpty(errmsg))
            {
                CConsole.WriteLine(errmsg, ConsoleColor.Red);
            }
            Exit(exitCode);
        }

        static void Exit(int exitCode)
        {
            Console.WriteLine("Press any key to exit.");
            while (!Console.KeyAvailable)
            {
            }
            Environment.Exit(exitCode);
        }
    }
}
