using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Safnet.PerfTestHarness;
using System.Threading.Tasks;
using Moq;

namespace Safnet.PerfTestHarness.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PerformanceReportTests
    {
        private MockRepository _repo = new MockRepository(MockBehavior.Strict);
        private Mock<IEnvironmentInfo> _mockInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockInfo = _repo.Create<IEnvironmentInfo>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repo.VerifyAll();
        }

        [TestMethod]
        public void ConstructorHappyPath()
        {
            var system = GivenTheSystemUnderTest();

            Assert.IsNotNull(system.Results);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorRejectsNullArgument()
        {
            new PerformanceReport(null);
        }

        [TestMethod]
        public void AddAnotherResult()
        {
            var system = GivenTheSystemUnderTest();

            var input = new PerformanceResult();

            var output = system.Add(input);

            Assert.AreSame(system, output, "fluent interface");

            Assert.AreSame(input, system.Results.First(), "item in list");
        }

        [TestMethod]
        public void OutputFile()
        {
            var expected = "asdfasdf";

            var system = GivenTheSystemUnderTest();

            system.OutputFile = expected;

            var actual = system.OutputFile;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Title()
        {
            var expected = "asdfasdf";

            var system = GivenTheSystemUnderTest();

            system.Title = expected;

            var actual = system.Title;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExecutableName()
        {
            var expected = "asdfasdf";

            var system = GivenTheSystemUnderTest();

            system.ExecutableName = expected;

            var actual = system.ExecutableName;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Arguments()
        {
            var expected = "asdfasdf";

            var system = GivenTheSystemUnderTest();

            system.Arguments = expected;

            var actual = system.Arguments;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StartTime()
        {
            var expected = new DateTime(2013, 02, 03, 04, 3, 2, 58);

            var system = GivenTheSystemUnderTest();

            system.StartTime = expected;

            var actual = system.StartTime;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EndTime()
        {
            var expected = new DateTime(2013, 02, 03, 04, 3, 2, 58);

            var system = GivenTheSystemUnderTest();

            system.EndTime = expected;

            var actual = system.EndTime;

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void CalculateAverageDurationIgnoresNonZeroExitCode()
        {
            var r1 = new PerformanceResult { ExitCode = 0, ProcessorTime = 5 };
            var r2 = new PerformanceResult { ExitCode = 0, ProcessorTime = 3 };
            var r3 = new PerformanceResult { ExitCode = 1, ProcessorTime = 1 };
            var expected = "4";

            var actual = PerformanceReport.CalculateAverageFor(new[] { r1, r2, r3 }, x => x.ProcessorTime);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WriteCsvReport()
        {
            var r1 = new PerformanceResult { ExitCode = 0, ProcessorTime = 5, PeakPagedMemory = 234232342342, PeakVirtualMemory = 7238423234, PeakWorkingSet = 09809809, RunNumber=0 };
            var r2 = new PerformanceResult { ExitCode = 0, ProcessorTime = 3, PeakPagedMemory = 234232342344, PeakVirtualMemory = 7238423236, PeakWorkingSet = 09809811, RunNumber = 1 };
            var r3 = new PerformanceResult { ExitCode = 1, ProcessorTime = 1, PeakPagedMemory = 23423232342, PeakVirtualMemory = 723842234, PeakWorkingSet = 0980809, RunNumber = 2 };

            var system = GivenTheSystemUnderTest();
            system.Add(r1).Add(r2).Add(r3);

            system.Arguments = "arrrrrgs";
            system.ExecutableName = "test.exe";
            system.StartTime = DateTime.Parse("2015-02-07 13:06");
            system.EndTime = DateTime.Parse("2015-02-07 14:02");
            system.Title = "Tes t i n g";
            system.OutputFile = ".\\output.csv";

            GivenDefaultEnvironmentValues();

            var expected = @"Title, Tes t i n g
Start Time, 2015-02-07 13:06
End Time, 2015-02-07 14:02
Executable, test.exe
Arguments, arrrrrgs
OS, Foo
Physical Memory, 7 GB
Virtual Memory, 8 GB
Processors, 323

Run Number, Exit Code, Paged Memory, Virtual Memory, Working Set, Processor Time
0, 0, 234232342342, 7238423234, 9809809, 5
1, 0, 234232342344, 7238423236, 9809811, 3
2, 1, 23423232342, 723842234, 980809, 1

Averages, , 234232342343, 7238423235, 9809810, 4
";
            // Mock the output file delegate
            PerformanceReport.WriteAllText = (string actualFileName, string actualContents) =>
            {
                Assert.AreEqual(system.OutputFile, actualFileName, "OutputFile");
                Assert.AreEqual(expected, actualContents, "contents");
            };

            // Execute the system under test
            system.WriteCsvFile();

            // There is nothing left to validate
        }

        private PerformanceReport GivenTheSystemUnderTest()
        {
            return new PerformanceReport(_mockInfo.Object);
        }

        private void GivenDefaultEnvironmentValues()
        {
            _mockInfo.SetupGet(x => x.OperatingSystem).Returns("Foo");
            _mockInfo.SetupGet(x => x.PhysicalMemory).Returns((ulong)1024 * (ulong)1024 * (ulong)1024 * (ulong)7);
            _mockInfo.SetupGet(x => x.VirtualMemory).Returns((ulong)1024 * (ulong)1024 * (ulong)1024 * (ulong)8);
            _mockInfo.SetupGet(x => x.ProcessorCount).Returns(323);
        }
    }
}
