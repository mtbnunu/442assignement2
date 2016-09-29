using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CPEN442Assignment2.Models;

namespace CPEN442Assignment2.Analyzers
{
    public class VigenereAnalyzer
    {
        private Settings settings;
        private int keyLengthFrom;
        private int keyLengthTo;
        private char[] cipherText;
        private int scoreThres;
        //private Dictionary<string, string> results;
        private ReaderWriterLock rwl = new ReaderWriterLock();
        private int timeOut = 6000000;


        public VigenereAnalyzer(int keyLengthFrom, int keyLengthTo, string cipherText, int scoreThresold)
        {
            this.keyLengthFrom = keyLengthFrom;
            this.keyLengthTo = keyLengthTo;
            this.cipherText = cipherText.ToCharArray();
            this.scoreThres = scoreThresold;
            this.settings = new Settings();
            //this.results = new Dictionary<string, string>();
        }

        public async void Start()
        {
            Parallel.For(keyLengthFrom, keyLengthTo + 1, async i =>
              {
                  await analyzeForKeylength(i);
              });
        }

        private async Task analyzeForKeylength(int keyLength)
        {
            CConsole.WriteLine("Starting to analyze keys of length " + keyLength, ConsoleColor.Cyan);

            var key = new Key(keyLength);

            while (key.NextKey())
            {
                var plainText = new char[cipherText.Length];
                var digits = key.getKeyDigits();
                var nextDigitIndex = 0;
                var maxDigitIndex = digits.Length - 1;

                var vcCount = 0;
                var prevVc = 0;

                if (key.getKeyString() == "LEMON")
                {
                    var k = 1;
                }


                for (var ci = 0; ci < cipherText.Length; ci++)
                {
                    var p = (char)(cipherText[ci] - digits[nextDigitIndex]);
                    if (p < 'A')
                    {
                        p = (char)(p + 26);
                    }
                    if ("AEIOU".Contains(p))
                    {
                        if (prevVc == 0)
                        {
                            vcCount++;
                        }
                        else
                        {
                            vcCount = 0;
                        }
                        prevVc = 0;
                    }
                    else
                    {
                        if (prevVc == 1)
                        {
                            vcCount++;
                        }
                        else
                        {
                            vcCount = 0;
                        }
                        prevVc = 1;
                    }
                    if (vcCount > 5)
                    {
                        break;
                    }



                    plainText[ci] = p;
                    nextDigitIndex++;
                    if (nextDigitIndex > maxDigitIndex)
                    {
                        nextDigitIndex = 0;
                    }
                }
                if (vcCount > 5)
                {
                    continue;
                }
                
                var pt = new string(plainText);
                var score = (from d in settings.Dictionary where pt.Contains(d) select (d.Length - 3) into x select (x * x * x)).Sum();
                if (score >= scoreThres)
                {
                    var pts = pt;//.Substring(0, 30) + "...";
                    CConsole.WriteLine(pts, ConsoleColor.Yellow);
                    CConsole.WriteLine(key.getKeyString(), ConsoleColor.Green);
                    CConsole.WriteLine(score.ToString(), ConsoleColor.Cyan);

                    try
                    {
                        rwl.AcquireWriterLock(timeOut);
                        using (StreamWriter sw = File.AppendText("output.txt"))
                        {
                            await sw.WriteLineAsync(pt);
                            await sw.WriteLineAsync("KEY: " + key.getKeyString());
                            await sw.WriteLineAsync("SCORE: " + score);
                            await sw.WriteLineAsync();
                        }
                    }
                    finally
                    {
                        if (rwl.IsWriterLockHeld)
                        {
                            rwl.ReleaseWriterLock();
                        }
                    }

                }

            }
            CConsole.WriteLine("Finshed analyzing keys of length " + keyLength, ConsoleColor.Yellow);

        }

        //public Dictionary<string, string> Results()
        //{
        //    return this.results;
        //}
    }
}
