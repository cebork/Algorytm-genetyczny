using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.UI.Domain
{
    public class ReferenceMatrixDisplayColumns
    {
        public string ReferenceMatrixName { get; set; }

        public int MatrixSize { get; set; }
        public bool[,] ReferenceMatrix { get; set; }
    }
}
