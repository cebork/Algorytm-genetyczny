using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.objects
{
    public sealed class RandomSingleton
    {
        private static Lazy<Random> instance;
        private static bool initialized = false;
        private static int usedSeed;

        private RandomSingleton() { }

        public static void SetSeed(int seed)
        {
            if (initialized)
                throw new InvalidOperationException("Random already initialized.");

            usedSeed = seed;
            instance = new Lazy<Random>(() => new Random(seed));
            initialized = true;
        }

        public static Random Instance
        {
            get
            {
                if (!initialized)
                {
                    instance = new Lazy<Random>(() => new Random());
                    initialized = true;
                }

                return instance.Value;
            }
        }

        public static int GetUsedSeed() => usedSeed;

        public static void Reset()
        {
            initialized = false;
            instance = null;
            usedSeed = 0;
        }
    }
}
