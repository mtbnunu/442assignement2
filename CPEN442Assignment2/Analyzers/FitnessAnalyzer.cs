using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CPEN442Assignment2.Analyzers
{
    public static class FitnessAnalyzer
    {
        public static int DictScore(string input, Settings settings)
        {
            return
                (from d in settings.Dictionary where input.Contains(d) select (d.Length - 3) into x select (x * x * x)).Sum();
        }

        public static double VowelsFreqScore(string input, Settings settings)
        {
            var letterCounts = new Dictionary<char,int>();
            for (int i = 0; i < 26; i++)
            {
                letterCounts.Add((char)('A'+i),0);
            }

            var prevVc = 0;
            var vcCount = 0;
            for (int i = 0; i < input.Length; i++)
            {
                letterCounts[input[i]]++;
                if ("AEIOU".Contains(input[i]))
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
                    return -1;
                }
            }

            var letterFreqs = letterCounts.ToDictionary(lc => lc.Key, lc => lc.Value/(double) input.Length);
            var freqScore = letterFreqs.Sum(lf => Math.Abs(settings.FreqDictionary[lf.Key] - lf.Value));

            return freqScore;
        }
    }
}