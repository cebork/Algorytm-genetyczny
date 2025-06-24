using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Enums
{
    public enum MutationType
    {
        EQUALY, BIT_SWAPING, UNIFORM_BLOCK, RANDOM_COORDS
    }

    public static class MutationTypeHelper
    {
        public static MutationType? FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (Enum.TryParse<MutationType>(value, true, out var result))
                return result;

            return null;
        }
    }
}
