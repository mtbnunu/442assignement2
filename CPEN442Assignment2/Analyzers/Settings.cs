using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CPEN442Assignment2.Models;
using Newtonsoft.Json;

namespace CPEN442Assignment2.Analyzers
{
    public class Settings
    {
        public Setting Setting { get; set; }
        public string[] Dictionary { get; set; }

        public Dictionary<char,double> FreqDictionary { get; set; }

        public Settings()
        {
            Console.WriteLine("reading dictionary");
            var dictionaryText = File.ReadAllText("dictionary.txt");
            this.Dictionary = dictionaryText.Split('\n').Select(a => a.ToUpper().Trim()).ToArray();
            
            if (this.Dictionary.Any(a => !Regex.IsMatch(a, @"^[A-z]+$")))
            {
                throw new InvalidDataException("dictionary file is not valid");
            }

            Console.WriteLine("reading settings.json");
            var settingsText = File.ReadAllText("settings.json");
            this.Setting = (Setting)Newtonsoft.Json.JsonConvert.DeserializeObject(settingsText, typeof(Setting));
            this.FreqDictionary = new Dictionary<char, double>();
            this.FreqDictionary.Add('A', this.Setting.LetterFreqs.a);
            this.FreqDictionary.Add('B', this.Setting.LetterFreqs.b);
            this.FreqDictionary.Add('C', this.Setting.LetterFreqs.c);
            this.FreqDictionary.Add('D', this.Setting.LetterFreqs.d);
            this.FreqDictionary.Add('E', this.Setting.LetterFreqs.e);
            this.FreqDictionary.Add('F', this.Setting.LetterFreqs.f);
            this.FreqDictionary.Add('G', this.Setting.LetterFreqs.g);
            this.FreqDictionary.Add('H', this.Setting.LetterFreqs.h);
            this.FreqDictionary.Add('I', this.Setting.LetterFreqs.i);
            this.FreqDictionary.Add('J', this.Setting.LetterFreqs.j);
            this.FreqDictionary.Add('K', this.Setting.LetterFreqs.k);
            this.FreqDictionary.Add('L', this.Setting.LetterFreqs.l);
            this.FreqDictionary.Add('M', this.Setting.LetterFreqs.m);
            this.FreqDictionary.Add('N', this.Setting.LetterFreqs.n);
            this.FreqDictionary.Add('O', this.Setting.LetterFreqs.o);
            this.FreqDictionary.Add('P', this.Setting.LetterFreqs.p);
            this.FreqDictionary.Add('Q', this.Setting.LetterFreqs.q);
            this.FreqDictionary.Add('R', this.Setting.LetterFreqs.r);
            this.FreqDictionary.Add('S', this.Setting.LetterFreqs.s);
            this.FreqDictionary.Add('T', this.Setting.LetterFreqs.t);
            this.FreqDictionary.Add('U', this.Setting.LetterFreqs.u);
            this.FreqDictionary.Add('V', this.Setting.LetterFreqs.v);
            this.FreqDictionary.Add('W', this.Setting.LetterFreqs.w);
            this.FreqDictionary.Add('X', this.Setting.LetterFreqs.x);
            this.FreqDictionary.Add('Y', this.Setting.LetterFreqs.y);
            this.FreqDictionary.Add('Z', this.Setting.LetterFreqs.z);
        }

    }
}
