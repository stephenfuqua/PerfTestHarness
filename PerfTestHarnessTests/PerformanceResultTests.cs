using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Safnet.PerfTestHarness.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PerformanceResultTests
    {
        [TestMethod]
        public void RunNumber()
        {
            var expected = 234234;

            var system = new PerformanceResult();

            system.RunNumber = expected;

            var actual = system.RunNumber;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExitCode()
        {
            var expected = 234234;

            var system = new PerformanceResult();

            system.ExitCode = expected;

            var actual = system.ExitCode;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PeakPagedMemory()
        {
            var expected = 234234;

            var system = new PerformanceResult();

            system.PeakPagedMemory = expected;

            var actual = system.PeakPagedMemory;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PeakWorkingSet()
        {
            var expected = 234234;

            var system = new PerformanceResult();

            system.PeakWorkingSet = expected;

            var actual = system.PeakWorkingSet;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PeakVirtualMemory()
        {
            var expected = 234234;

            var system = new PerformanceResult();

            system.PeakVirtualMemory = expected;

            var actual = system.PeakVirtualMemory;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProcessorTime()
        {
            var expected = 234234;

            var system = new PerformanceResult();

            system.ProcessorTime = expected;

            var actual = system.ProcessorTime;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FormattedStringOutput()
        {

            var system = new PerformanceResult
            {
                ExitCode = 1,
                PeakPagedMemory = 2,
                PeakVirtualMemory = 3,
                PeakWorkingSet = 4,
                ProcessorTime = 5,
                RunNumber = 6
            };

            var expected = "6, 1, 2, 3, 4, 5";

            var actual = system.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
