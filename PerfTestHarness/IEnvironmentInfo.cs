namespace Safnet.PerfTestHarness
{
    public interface IEnvironmentInfo
    {
        string OperatingSystem { get; }
        ulong PhysicalMemory { get; }
        int ProcessorCount { get; }
        ulong VirtualMemory { get; }
    }
}