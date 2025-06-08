using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.UI.Domain
{
    public class PatternChoosingDisplayColumns
    {
        public String PatternName { get; set; }
        public int PatternSize { get; set; }
        public Matrix<double> PatternMatrix { get; set; }
    }
}
