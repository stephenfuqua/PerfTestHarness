using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safnet.PerfTestHarness
{
    public class EnvironmentInfo : IEnvironmentInfo
    {
        private ComputerInfo _compInfo = new ComputerInfo();

        public string OperatingSystem => _compInfo.OSFullName;

        public ulong PhysicalMemory => _compInfo.TotalPhysicalMemory;

        public ulong VirtualMemory => _compInfo.TotalVirtualMemory;

        public int ProcessorCount => Environment.ProcessorCount;


    }
}
