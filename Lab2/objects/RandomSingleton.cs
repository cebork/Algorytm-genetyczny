using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.objects
{
    public sealed class RandomSingleton
    {
        // Lazy<T> ensures that the instance is created only when it's needed in a thread-safe manner
        private static readonly Lazy<Random> instance = new Lazy<Random>(() => new Random());

        // Private constructor to prevent instantiation
        private RandomSingleton() { }

        // Public property to access the singleton instance
        public static Random Instance => instance.Value;
    }
}
