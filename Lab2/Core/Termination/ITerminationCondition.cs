using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Termination
{
    public interface ITerminationCondition
    {
        bool ShouldStop(int iteration);
    }
}
