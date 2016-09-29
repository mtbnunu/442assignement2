using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEN442Assignment2.Models
{
    public class LetterFreqs
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        public double d { get; set; }
        public double e { get; set; }
        public double f { get; set; }
        public double g { get; set; }
        public double h { get; set; }
        public double i { get; set; }
        public double j { get; set; }
        public double k { get; set; }
        public double l { get; set; }
        public double m { get; set; }
        public double n { get; set; }
        public double o { get; set; }
        public double p { get; set; }
        public double q { get; set; }
        public double r { get; set; }
        public double s { get; set; }
        public double t { get; set; }
        public double u { get; set; }
        public double v { get; set; }
        public double w { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class Setting
    {
        public LetterFreqs LetterFreqs { get; set; }
        public List<string> bigrams { get; set; }
        public List<string> trigrams { get; set; }
    }
}

