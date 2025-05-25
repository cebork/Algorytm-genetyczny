using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Enums
{
    public enum AlgorithmType
    {
        SUPERVISED, UNSUPERVISED
    }

    public static class AlgorithmTypeHelper
    {
        public static AlgorithmType? FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // Try parsing the string ignoring case
            if (Enum.TryParse<AlgorithmType>(value, true, out var result))
                return result;

            return null;
        }
    }

}
