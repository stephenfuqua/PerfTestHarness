using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Safnet.PerfTestHarness.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ReportStringBuilderTests
    {
        [TestMethod]
        public void AppendTwoDetailResultsToStringBuilder()
        {
            var input = new List<PerformanceResult>
            {
                new PerformanceResult { ExitCode = 0, PeakPagedMemory = 1, PeakVirtualMemory = 2, PeakWorkingSet = 3, ProcessorTime = 4, RunNumber =0 },
                new PerformanceResult { ExitCode = 10, PeakPagedMemory = 11, PeakVirtualMemory = 12, PeakWorkingSet = 13, ProcessorTime = 14, RunNumber =1 }
            };

            var expectded = input[0].ToString() + Environment.NewLine + input[1].ToString() + Environment.NewLine;

            var builder = new StringBuilder();

            var result = builder.AppendToReport(input);

            Assert.AreSame(builder, result, "fluent interface");

            var actual = result.ToString();

            Assert.AreEqual(expectded, actual, "string result");
        }
    }
}
