namespace Safnet.PerfTestHarness
{

    public class PerformanceResult
    {
        public int RunNumber { get; set; }
        public int ExitCode { get; set; }
        public long PeakPagedMemory { get; set; }
        public long PeakWorkingSet { get; set; }
        public long PeakVirtualMemory { get; set; }
        public long ProcessorTime { get; set; }

        public override string ToString() => "\{RunNumber}, \{ExitCode}, \{PeakPagedMemory}, \{PeakVirtualMemory}, \{PeakWorkingSet}, \{ProcessorTime}";
    }

}
