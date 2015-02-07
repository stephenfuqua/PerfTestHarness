using Microsoft.VisualBasic.Devices;
using System.Globalization;

namespace Safnet.PerfTestHarness.Tests
{
    public class FakeComputerInfo : ComputerInfo
    {
        
        public new ulong AvailablePhysicalMemory { get; set; }

        public new ulong AvailableVirtualMemory { get; set; }

        public new CultureInfo InstalledUICulture { get; set; }

        public new string OSFullName { get; set; }

        public new string OSPlatform { get; set; }

        public new string OSVersion { get; set; }

        public new ulong TotalPhysicalMemory { get; set; }

        public new ulong TotalVirtualMemory { get; set; }
    }
}
