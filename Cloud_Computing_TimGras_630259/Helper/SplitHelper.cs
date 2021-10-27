using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Computing_TimGras_630259.Helper
{
    public static class SplitHelper
    {
        public static List<List<T>> Split<T>(List<T> source)
        {
            return (List<List<T>>)source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 20)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
