using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPEN442Assignment2.Analyzers
{
    public static class CRC
    {
        public static string test(string str)
        {
            Console.WriteLine(FormatCRC32Result(BitConverter.GetBytes(Crc32.Crc32Algorithm.DefaultPolynomial)));
            Console.WriteLine(FormatCRC32Result(BitConverter.GetBytes(Crc32.Crc32Algorithm.DefaultSeed)));
            var x = new ASCIIEncoding();
            var buffer = x.GetBytes(str);
            var hash = Crc32.Crc32Algorithm.Compute(buffer);
            var bytes = BitConverter.GetBytes(hash);
            return FormatCRC32Result(bytes);
        }

        public static string FindDuplicateFromString(string str, string str2)
        {
            var x = new ASCIIEncoding();
            var buffer = x.GetBytes("The quick brown fox jumps over the lazy dog");
            var buffer2 = x.GetBytes("Another string");
            //var buffer = x.GetBytes(str);
            //var buffer2 = x.GetBytes(str);
            return FindDuplicate(buffer, buffer2);
        }

        public static string FindDuplicate(byte[] buffer, byte[] buffer2)
        {
            //Crc32 crc32 = new Crc32();
            var hash = Crc32.Crc32Algorithm.Compute(buffer);
            var twobufferList = buffer2.ToList();
            twobufferList.Add(0x0);
            twobufferList.Add(0x0);
            twobufferList.Add(0x0);
            twobufferList.Add(0x0);
            var twobuffer = twobufferList.ToArray();
            var hash2 = Crc32.Crc32Algorithm.Compute(twobuffer);
            var xor = hash ^ hash2;
            var check1 = FormatCRC32Result(BitConverter.GetBytes(hash));
            var check2 = FormatCRC32Result(BitConverter.GetBytes(hash2));
            var xorbyte = BitConverter.GetBytes(xor);
            var checkxor = FormatCRC32Result(BitConverter.GetBytes(xor));
            var reversed = BitReverse(xorbyte);
            var checkreversed = FormatCRC32Result(reversed);
            var reversedUint = BitConverter.ToUInt32(reversed, 0);


            //MULTIPLICATIVE INVERSE OF X^32
            var magic1 = 0xCBF1ACDA;
            //CRC32 polinomial
            var magic2 = 0x104C11DB7;

            BigInteger bigReversed = reversedUint;
            var multiMod = (BigInteger.Multiply(bigReversed, magic1) % magic2);
            var checkfinal = FormatCRC32Result(BitConverter.GetBytes((uint)multiMod));

            Console.WriteLine("HEX of first string is " + FormatCRC32Result(buffer));
            Console.WriteLine("CRC32 of first string is " + check1);
            Console.WriteLine("HEX of second string, padding with 4 bytes of zeros is " + FormatCRC32Result(twobuffer));
            Console.WriteLine("CRC32 of second string, padded, is " + check2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Do this and replace trailing 4 zero bytes with result :" + FormatCRC32Result(twobuffer));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(string.Format("{0} * {1} mod {2}", checkreversed, FormatCRC32Result(BitConverter.GetBytes(magic1)), FormatCRC32Result(BitConverter.GetBytes(magic2))));
            Console.ForegroundColor = ConsoleColor.White;

            return "I..can't........handle..........anymore..................";
        }

        public static byte[] BitReverse(byte[] b)
        {
            BitArray array = new BitArray(b);
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }

            byte[] bytes = new byte[b.Length];
            array.CopyTo(bytes, 0);
            return bytes;
        }

        public static string FormatCRC32Result(byte[] result)
        {
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }
            return "0x" + BitConverter.ToUInt32(result, 0).ToString("X8");
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        static UInt32[] InitializeTable(UInt32 polynomial)
        {
            var createTable = new UInt32[256];
            for (var i = 0; i < 256; i++)
            {
                var entry = (UInt32)i;
                for (var j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            return createTable;
        }

    }
}
