using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Random
{
    public interface IRandomProvider
    {
        double NextDouble();

        int Next(int minInclusive, int maxExclusive);
    }
}
