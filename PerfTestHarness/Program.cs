using System;
using System.Diagnostics;
using Safnet.PerfTestHarness;

namespace PerfTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var iterations = 10;
            //var fileName = @"..\..\..\MongoExperiments.CS\bin\debug\MongoExperiments.cs.exe";
            var fileName = @"""C:\Program Files (x86)\nodejs\node.exe""";
            //var arguments = "";
            var arguments = @"..\..\..\MongoExperiments.JS\app.js";
            var reportName = ".\\Mongo.JS.10.html";
            var title = "Ten iterations with Mongo";

            var start = new ProcessStartInfo
            {
                Arguments = arguments,
                FileName = fileName,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            var report = new PerformanceReport(new EnvironmentInfo())
            {
                OutputFile = reportName,
                ExecutableName = fileName,
                Arguments = arguments,
                StartTime = DateTime.Now
            };

            for (int i = 0; i < iterations; i++)
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
            }

            report.EndTime = DateTime.Now;
            report.WriteCsvFile();

        }
    }
}
