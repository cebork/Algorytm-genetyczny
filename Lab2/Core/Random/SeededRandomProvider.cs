using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Random
{
    public class SeededRandomProvider : IRandomProvider
    {
        private readonly System.Random _random;

        public int Seed { get; }

        public SeededRandomProvider(int seed)
        {
            Seed = seed;
            _random = new System.Random(seed);
        }

        public double NextDouble()
        {
            return _random.NextDouble();
        }

        public int Next(int minInclusive, int maxExclusive)
        {
            return _random.Next(minInclusive, maxExclusive);
        }
    }
}
