using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Enums
{
    public enum AlgorithmOption
    {
        CLASSICAL, MODIFIED
    }

    public static class AlgorithmOptionHelper
    {
        public static AlgorithmOption? FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (Enum.TryParse<AlgorithmOption>(value, true, out var result))
                return result;

            return null;
        }
    }
}
