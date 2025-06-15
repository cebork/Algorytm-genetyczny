using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Enums
{
    public enum CrossType
    {
        SINGLE_POINT, MULTI_POINT
    }

    public static class CrossTypeHelper
    {
        public static CrossType? FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (Enum.TryParse<CrossType>(value, true, out var result))
                return result;

            return null;
        }
    }
}
