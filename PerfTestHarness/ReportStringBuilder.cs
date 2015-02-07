using System.Collections.Generic;
using System.Text;

namespace Safnet.PerfTestHarness
{

    public static class ReportStringBuilder
    {
        public static StringBuilder AppendToReport(this StringBuilder builder, IEnumerable<PerformanceResult> results)
        {
            foreach (var item in results)
            {
                builder.AppendLine(item.ToString());
            }

            return builder;
        }
    }
}
