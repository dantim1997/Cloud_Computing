using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class SplitHelper
    {
        public static List<List<T>> Split<T>(List<T> source)
        {
            int maxInBatch = Int32.Parse(Environment.GetEnvironmentVariable("MaxItemsInABatch"));
            return (List<List<T>>)source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / maxInBatch)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
