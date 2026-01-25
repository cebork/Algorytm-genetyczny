using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Termination
{
    public class MaxIterationCondition : ITerminationCondition
    {
        public int MaxIterations { get; }

        public MaxIterationCondition(int maxIterations)
        {
            MaxIterations = maxIterations;
        }

        public bool ShouldStop(int iteration)
        {
            return iteration >= MaxIterations;
        }
    }
}
