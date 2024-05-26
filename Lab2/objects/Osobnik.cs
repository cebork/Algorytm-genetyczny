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
        private Random rnd = new Random();

        private int a;
        private int b;
        private double d;


        public int Lp { get; set; }
        public double xReal { get; set; }
        public int xInt;
        public string xBin;
        public int xInt2;
        public double xReal2;
        public double Mark { get; set; }
        public double FitValue { get; set; }
        public double Probability { get; set; }
        public double Distribuator { get; set; }
        public double RandomValueToCheck { get; set; }
        public double XRealAfterSelection { get; set; }

        public Osobnik(int lp, int a, int b, double d) 
        {
            Lp = lp;
            this.a = a;
            this.b = b;
            this.d = d;
            InitGeneration();
            RealToInt();
            IntegerToBinary();
            BinaryToInteger();
            IntegerToReal();
            SetOcena();
        }
        
        private void InitGeneration()
        {
            int numberOfSteps = (int)((this.b - this.a) / this.d) + 1;
            int randomStep = rnd.Next(numberOfSteps);
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

        private int getL()
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
    }
}
