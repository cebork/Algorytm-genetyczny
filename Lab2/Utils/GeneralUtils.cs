using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal static class GeneralUtils
    {
        public static int GetPrecision(double precision)
        {
            string numberAsString = precision.ToString("0.#############################", System.Globalization.CultureInfo.InvariantCulture);
            string trimmedString = numberAsString.TrimEnd('0');
            var numberSplited = trimmedString.Split(".");
            if (numberSplited != null && numberSplited.Length > 0) return numberSplited[1].Length;
            return 0;
        }

        public static int GetChromosomeLength(int a, int b, double precision)
        {
            return (int)Math.Ceiling(Math.Log2(((b - a) / precision) + 1));
        }

        public static double GetMantissa(double doubleValue)
        {
            return doubleValue - Math.Truncate(doubleValue);
        }

        public static double SetMark(double xReal, int precision)
        {
            return Math.Round(Math.Round(GetMantissa(xReal), precision) * (Math.Cos(20 * Math.PI * xReal) - Math.Sin(xReal)), precision);
        }


        public static int RealToInt(int a, int b, double xReal, int chromosomeLength)
        {
            double part1 = (double)1 / (b - a);
            double part2 = xReal - a;
            double part3 = Math.Pow(2, chromosomeLength) - 1;
            return (int)(part1 * part2 * part3);
        }

        public static string IntegerToBinary(int chromosomeLength, int xInt)
        {
            string binString = Convert.ToString(xInt, 2);
            while (binString.Length < chromosomeLength)
            {
                binString = "0" + binString;
            }

            return binString;
        }

        public static int BinaryToInteger(string xBin)
        {
            return Convert.ToInt32(xBin, 2);
        }

        public static double IntegerToReal(int a, int b, int xInt, int l, int precision)
        {
            double part1 = (b - a) * xInt;
            double part2 = Math.Pow(2, l) - 1;
            return Math.Round((part1 / part2) + a, precision);
        }

        public static string GenerateBinaryString(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be greater than 0");
            }

            
            StringBuilder binaryStringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                binaryStringBuilder.Append(RandomSingleton.Instance.Next(2) == 0 ? '0' : '1');
            }

            return binaryStringBuilder.ToString();
        }
    }
}
