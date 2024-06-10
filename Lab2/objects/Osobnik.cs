using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.objects
{
    internal class Osobnik
    {

        private int a;
        private int b;
        private double d;
        private double pk;
        private double pm;

        public int Lp { get; set; }
        public double xReal;
        public int xInt;
        public string xBin;
        public int xInt2;
        public double xReal2;
        public double Mark;
        public double FitValue;
        public double Probability;
        public double Distribuator;
        public double RandomValueToCheck;
        public double XRealAfterSelection { get; set; }
        private int xIntAfterSelection;
        public string xBinAfterSelection { get; set; }
        public string xBinParents { get; set; }
        public int CutPoint { get; set; }
        public string xBinChild { get; set; }
        public string xBinAfterCross { get; set; }
        public string MutationPosition { get; set; }
        public string xBinAfterMutation { get; set; }
        public double XRealAfterMutation { get; set; }
        public double MarkAfterMutation { get; set; }
        public Osobnik(int lp, int a, int b, double d, double pk, double pm) 
        {
            Lp = lp;
            this.a = a;
            this.b = b;
            this.d = d;
            this.pk = pk;
            this.pm = pm;
            InitGeneration();
            RealToInt();
            IntegerToBinary();
            BinaryToInteger();
            IntegerToReal();
            SetOcena();
        }

        public Osobnik(int lp, int a, int b, double d, double pk, double pm, double nextXreal)
        {
            Lp = lp;
            this.a = a;
            this.b = b;
            this.d = d;
            this.pk = pk;
            this.pm = pm;
            xReal = nextXreal;
            RealToInt();
            IntegerToBinary();
            BinaryToInteger();
            IntegerToReal();
            SetOcena();
        }

        private void InitGeneration()
        {
            int numberOfSteps = (int)((this.b - this.a) / this.d) + 1;
            int randomStep = RandomSingleton.Instance.Next(numberOfSteps);
            double number = this.a + randomStep * this.d;
            int x = getPrecision(d);
            xReal = Math.Round(number, getPrecision(d));
        }

        private int getPrecision(double number)
        {
            string numberAsString = number.ToString("0.#############################", System.Globalization.CultureInfo.InvariantCulture);
            string trimmedString = numberAsString.TrimEnd('0');
            var numberSplited = trimmedString.Split(".");
            if (numberSplited != null && numberSplited.Length > 0) return numberSplited[1].Length;
            return 0;
        }

        public int getL()
        {
            return (int) Math.Ceiling(Math.Log2(((b - a) / d) + 1));
        }

        private double getMantysa()
        {
            return xReal - Math.Truncate(xReal);
        }

        private void SetOcena()
        {
            Mark = Math.Round(Math.Round(getMantysa(), getPrecision(d)) * (Math.Cos(20 * Math.PI * xReal) - Math.Sin(xReal)), getPrecision(d));
        }


        private void RealToInt()
        {
            double part1 = (double)1 / (b - a);
            double part2 = xReal - a;
            double part3 = Math.Pow(2, getL()) - 1;
            xInt = (int)(part1 * part2 * part3);
        }

        private void IntegerToBinary()
        {
            int l = getL();
            string binString = Convert.ToString(xInt, 2);
            while (binString.Length < getL()) 
            {
                binString = "0" + binString;
            }
            
            xBin = binString;
        }

        private void BinaryToInteger()
        {
            xInt2 = Convert.ToInt32(xBin, 2);
        }

        private void IntegerToReal()
        {
            double part1 = (b - a) * xInt2;
            double part2 = Math.Pow(2, getL()) - 1;
            xReal2 = Math.Round((part1 / part2) + a, getPrecision(d));
        }

        public void SetFitValue(double minValue)
        {
            FitValue = Mark - minValue + d;
        }

        public void SetProbability(double sumValue)
        {
            Probability = FitValue / sumValue;
        }

        public int RealToInt(double real)
        {
            double part1 = (double)1 / (b - a);
            double part2 = real - a;
            double part3 = Math.Pow(2, getL()) - 1;
            return (int)(part1 * part2 * part3);
        }

        public string IntToBin(int intNumber)
        {
            int l = getL();
            string binString = Convert.ToString(intNumber, 2);
            while (binString.Length < getL())
            {
                binString = "0" + binString;
            }

            return binString;
        }

        public void RealToBin(double real)
        {
            int integerNumebr = RealToInt(real);
            xBinAfterSelection = IntToBin(integerNumebr);
        }

        public void SetCutPoint()
        {
            if (xBinParents != "-")
            {
                CutPoint = RandomSingleton.Instance.Next(0, getL() - 2);
            }
            else
            {
                CutPoint = -1;
            }
            
        }


        public void SetParent()
        {
            double random = RandomSingleton.Instance.NextDouble();
            if (random <= pk)
            {
                xBinParents = xBinAfterSelection;
            }
            else
            {
                xBinParents = "-";
            }
        }

        public void Mutate()
        {
            MutationPosition = "";
            string afterMutation = "";
            for (int i = 0; i < getL(); i++)
            {
                double randomDouble = RandomSingleton.Instance.NextDouble();
                
                if (randomDouble <= pm)
                {
                    MutationPosition += i + ",";
                    if (xBinAfterCross[i] == '1')
                    {
                        afterMutation += "0";
                    }
                    else
                    {
                        afterMutation += "1";
                    }
                }
                else
                {
                    afterMutation += xBinAfterCross[i];
                }
            }
            xBinAfterMutation = afterMutation;
        }


        private int BinaryToInteger(string binary)
        {
            return Convert.ToInt32(binary, 2);
        }

        private double IntegerToReal(int integerValue)
        {
            double part1 = (b - a) * integerValue;
            double part2 = Math.Pow(2, getL()) - 1;
            return Math.Round((part1 / part2) + a, getPrecision(d));
        }

        public double BinaryToReal(string binary)
        {
            int tempInteger = BinaryToInteger(binary);
            return IntegerToReal(tempInteger);
        }


        public double SetOcena(double xRealLocal)
        {
            return Math.Round(Math.Round(getMantysa(), getPrecision(d)) * (Math.Cos(20 * Math.PI * xRealLocal) - Math.Sin(xRealLocal)), getPrecision(d));
        }
    }
}
