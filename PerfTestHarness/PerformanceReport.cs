using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Safnet.PerfTestHarness
{


    public class PerformanceReport
    {
        private readonly IEnvironmentInfo _environmentInfo;

        public List<PerformanceResult> Results { get; } = new List<PerformanceResult>();

        public string OutputFile { get; set; }

        public string OuputFileCsv {  get { return (OutputFile.EndsWith(".csv") ? OutputFile : OutputFile + ".csv"); } }

        public string Title { get; set; }

        public string ExecutableName { get; set; }

        public string Arguments { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public PerformanceReport(IEnvironmentInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            _environmentInfo = info;
        }



        public PerformanceReport Add(PerformanceResult result)
        {
            Results.Add(result);
            return this;
        }

        public const string CSV_ROW_HEADER = "Run Number, Exit Code, Paged Memory, Virtual Memory, Working Set, Processor Time";

        public void WriteCsvFile()
        {
            var builder = new StringBuilder()
                                .AppendLine(GenerateParameterListing())
                                .AppendLine(string.Empty)
                                .AppendLine(CSV_ROW_HEADER)
                                .AppendToReport(Results)
                                .AppendLine(string.Empty)
                                .AppendLine(GenerateStatistics());

            WriteAllText(OuputFileCsv, builder.ToString());
        }

        // Used for delegate injection in unit testing
        public static Action<string, string> WriteAllText = File.WriteAllText;

        private string GenerateStatistics()
        {
            var footer = "Averages, , {0}, {1}, {2}, {3}";

            long iterations = Results.Count();
            var avgPaged = CalculateAverageFor(Results, x => x.PeakPagedMemory);
            var avgVirtual = CalculateAverageFor(Results, x => x.PeakVirtualMemory);
            var avgWorking = CalculateAverageFor(Results, x => x.PeakWorkingSet);
            var avgtime = CalculateAverageFor(Results, x => x.ProcessorTime);

            return string.Format(footer, avgPaged, avgVirtual, avgWorking, avgtime);
        }

        public static string CalculateAverageFor(IEnumerable<PerformanceResult> results, Func<PerformanceResult, long> expression)
        {
            var valid = results.Where(x => x.ExitCode == 0).ToList();

            var avg = valid.Select(expression)
                           .Sum() / (long)valid.Count();

            return avg.ToString();
        }

        private string GenerateParameterListing()
        {
            var timeFormat = "yyyy-MM-dd HH:mm";
            var startTime = StartTime.ToString(timeFormat);
            var endTime = EndTime.ToString(timeFormat);

            return "Title, \{Title}" + Environment.NewLine +
                    "Start Time, \{startTime}" + Environment.NewLine +
                    "End Time, \{endTime}" + Environment.NewLine +
                    "Executable, \{ExecutableName}" + Environment.NewLine +
                    "Arguments, \{Arguments}" + Environment.NewLine +
                    "OS, \{_environmentInfo.OperatingSystem}" + Environment.NewLine +
                    "Physical Memory, \{MemoryToString(_environmentInfo.PhysicalMemory)}" + Environment.NewLine +
                    "Virtual Memory, \{MemoryToString(_environmentInfo.VirtualMemory)}" + Environment.NewLine +
                    "Processors, \{_environmentInfo.ProcessorCount}";
        }

        private string MemoryToString(ulong memory) => (memory / (1024 * 1024 * 1024)).ToString() + " GB";

    }

}
