using System;
using System.Diagnostics;
using Safnet.PerfTestHarness;
using System.Linq;
using System.Collections.Generic;

namespace PerfTestHarness
{  

    class Program
    {

        static void Main(string[] args)
        {
            //var iterations = 10;
            ////var fileName = @"..\..\..\MongoExperiments.CS\bin\debug\MongoExperiments.cs.exe";
            //var fileName = @"""C:\Program Files (x86)\nodejs\node.exe""";
            ////var arguments = "";
            //var arguments = @"..\..\..\MongoExperiments.JS\app.js";
            //var reportName = ".\\Mongo.JS.10.html";
            //var title = "Ten iterations with Mongo";

            var arguments = TestHarnessArguments.BuildFrom(args);


            var start = new ProcessStartInfo
            {
                Arguments = arguments.Arguments,
                FileName = arguments.ExecutableName,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            var report = new PerformanceReport(new EnvironmentInfo())
            {
                OutputFile = arguments.OutputFile,
                ExecutableName = arguments.ExecutableName,
                Arguments = arguments.Arguments,
                StartTime = DateTime.Now
            };

            for (int i = 0; i < arguments.Iterations; i++)
            {
                report = RunIteration(start, report, i);
            }

            report.EndTime = DateTime.Now;
            report.WriteCsvFile();

        }

        private static PerformanceReport RunIteration(ProcessStartInfo start, PerformanceReport report, int i)
        {
            var result = new PerformanceResult
            {
                RunNumber = i,
            };

            using (var proc = Process.Start(start))
            {
                do
                {
                    if (!proc.HasExited)
                    {
                        proc.Refresh();

                        result.PeakPagedMemory = proc.PeakPagedMemorySize64;
                        result.PeakVirtualMemory = proc.PeakVirtualMemorySize64;
                        result.PeakWorkingSet = proc.PeakWorkingSet64;
                    }
                }
                while (!proc.WaitForExit(10));

                result.ProcessorTime = (long)Math.Round(proc.TotalProcessorTime.TotalMilliseconds, 0);
                result.ExitCode = proc.ExitCode;
            }
            report.Add(result);

            return report;
        }
    }
}
