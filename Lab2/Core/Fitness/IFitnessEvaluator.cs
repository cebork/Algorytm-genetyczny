using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Fitness
{
    public interface IFitnessEvaluator
    {
        decimal Evaluate(bool[,] genotype);
    }
}
