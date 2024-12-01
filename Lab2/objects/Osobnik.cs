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

        public int MatrixWidth { get; set; }
        public int MatrixHeight { get; set; }
        private double d;
        private double pk;
        private double pm;

        public bool[,] Matrix { get; set; }

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

        public bool[,] MatrixAfterSelection { get; set; }

        public bool[,] MatrixParents { get; set; }

        public bool[,] MatrixChild { get; set; }

        public bool[,] MatrixAfterCross { get; set; }

        public bool[,] MatrixAfterMutation { get; set; }

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
        public Osobnik(int lp, int MatrixWidth, int MatrixHeight, double d, double pk, double pm) 
        {
            Lp = lp;
            this.MatrixWidth = MatrixWidth;
            this.MatrixHeight = MatrixHeight;
            this.d = d;
            this.pk = pk;
            this.pm = pm;
            InitOsobnikMatrix();
            //RealToInt();
            //IntegerToBinary();
            //BinaryToInteger();
            //IntegerToReal();
            SetOcena();
        }

        public Osobnik(int lp, int a, int b, double d, double pk, double pm, bool[,] nextMatrix)
        {
            Lp = lp;
            this.MatrixWidth = a;
            this.MatrixHeight = b;
            this.d = d;
            this.pk = pk;
            this.pm = pm;
            Matrix = nextMatrix;
            //RealToInt();
            //IntegerToBinary();
            //BinaryToInteger();
            //IntegerToReal();
            SetOcena();
        }

        private void InitOsobnikMatrix()
        {
            Matrix = new bool[MatrixWidth, MatrixHeight];
            Random random = RandomSingleton.Instance;
            for (int i = 0; i < MatrixWidth; i++)
            {
                for (int j = 0; j < MatrixHeight; j++)
                {
                    Matrix[i, j] = random.Next(2) == 0;
                }
            }

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
            //return (int) Math.Ceiling(Math.Log2(((MatrixWidth/2) / d) + 1));
            return MatrixWidth;
        }

        private double getMantysa()
        {
            return xReal - Math.Truncate(xReal);
        }

        private void SetOcena()
        {

            int count = 0;
            for (int i = 0; i < MatrixWidth; i++)
            {
                for (int j = 0; j < MatrixHeight; j++)
                {
                    if (Matrix[i, j] && IsSurroundedByFalses(Matrix, i, j, MatrixWidth, MatrixHeight))
                    {
                        count++;
                    }
                }
            }

            Mark = Math.Round((double) count / (double)(MatrixWidth * MatrixHeight), getPrecision(d));
        }

        static bool IsSurroundedByFalses(bool[,] matrix, int x, int y, int rows, int cols)
        {
            int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };

            for (int dir = 0; dir < 8; dir++)
            {
                int nx = x + dx[dir];
                int ny = y + dy[dir];

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && matrix[nx, ny])
                {
                    return false;
                }
            }
            return true;
        }

        private void RealToInt()
        {
            double part1 = (double)1 / (MatrixHeight - MatrixWidth);
            double part2 = xReal - MatrixWidth;
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
            double part1 = (MatrixHeight - MatrixWidth) * xInt2;
            double part2 = Math.Pow(2, getL()) - 1;
            xReal2 = Math.Round((part1 / part2) + MatrixWidth, getPrecision(d));
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
            double part1 = (double)1 / (MatrixHeight - MatrixWidth);
            double part2 = real - MatrixWidth;
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
                MatrixParents = MatrixAfterSelection;
            }
            else
            {
                MatrixParents = null;
            }
        }

        public void Mutate()
        {
            MutationPosition = "";
            List<string> mutationLog = new List<string>();
            int rows = MatrixAfterCross.GetLength(0);
            int cols = MatrixAfterCross.GetLength(1);

            bool[,] afterMutation = (bool[,])MatrixAfterCross.Clone();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    double randomDouble = RandomSingleton.Instance.NextDouble();

                    if (randomDouble <= pm)
                    {
                        MutationPosition += $"[{i}, {j}],";
                        string oldValue = MatrixAfterCross[i, j] == false ? "true" : "false";
                        string newValue = MatrixAfterCross[i, j] == true ? "false" : "true";

                        afterMutation[i, j] = MatrixAfterCross[i, j] == true ? false : true;

                        mutationLog.Add($"[{i}, {j}] {oldValue} -> {newValue}");
                    }
                }
            }

            MatrixAfterMutation = afterMutation;
        }


        private int BinaryToInteger(string binary)
        {
            return Convert.ToInt32(binary, 2);
        }

        private double IntegerToReal(int integerValue)
        {
            double part1 = (MatrixHeight - MatrixWidth) * integerValue;
            double part2 = Math.Pow(2, getL()) - 1;
            return Math.Round((part1 / part2) + MatrixWidth, getPrecision(d));
        }

        public double BinaryToReal(string binary)
        {
            int tempInteger = BinaryToInteger(binary);
            return IntegerToReal(tempInteger);
        }


        public double SetOcena(bool[,] xRealLocal)
        {
            int count = 0;
            for (int i = 0; i < MatrixWidth; i++)
            {
                for (int j = 0; j < MatrixHeight; j++)
                {
                    if (Matrix[i, j] && IsSurroundedByFalses(Matrix, i, j, MatrixWidth, MatrixHeight))
                    {
                        count++;
                    }
                }
            }

            return Math.Round((double)count / (double)(MatrixWidth * MatrixHeight), getPrecision(d));
        }
    }
}
