using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Enums
{
    public enum SelectionType
    {
        ROULETTE, TOURNAMENT
    }
    public static class SelectionTypeHelper
    {
        public static SelectionType? FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (Enum.TryParse<SelectionType>(value, true, out var result))
                return result;

            return null;
        }
    }
}
