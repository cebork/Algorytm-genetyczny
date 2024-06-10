using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.GEO.domain
{
    internal class InitialData
    {
        public int a { get; set; }
        public int b { get; set; }
        public double Precision { get; set; }
        public int T { get; set; }
        public double Teta { get; set; }
        public int ChromosomeLength { get; set; }
        public int PrecisionAsInteger { get; set; }

        public InitialData(int a, int b, double precision, int t, double teta, int chromosomeLength, int precisionAsInteger)
        {
            this.a = a;
            this.b = b;
            Precision = precision;
            T = t;
            Teta = teta;
            ChromosomeLength = chromosomeLength;
            PrecisionAsInteger = precisionAsInteger;
        }
    }
}
