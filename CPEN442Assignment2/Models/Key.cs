using System.Linq;

namespace CPEN442Assignment2.Models
{
    public class Key
    {
        private char[] key;
        private int length;

        public Key(int length)
        {
            key = new char[length];
            this.length = length;
            for (int i = 0; i < length-1; i++)
            {
                key[i] = ('A');
            }
            //since NextKey will be called before first retrieval
            key[length-1] = (char)('A'-1);
        }

        public bool NextKey()
        {
            var ind = length-1;
            key[ind]++;
            while (key[ind] > 'Z')
            {
                if (ind == 0)
                {
                    return false;
                }
                key[ind] = 'A';
                ind--;
                key[ind]++;
            }
            return true;
        }

        public string getKeyString()
        {
            return new string(key.ToArray());
        }

        public char[] getKeyDigits()
        {
            var na = new char[length];
            for (var i = 0; i < length; i++)
            {
                na[i] = (char) (key[i] - 'A');
            }
            return na;
        }
    }
}
