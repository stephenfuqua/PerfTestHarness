using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safnet.PerfTestHarness
{
    public class TestHarnessArguments
    {
        public const string FLAG_ARGUMENTS = "-a";
        public const string FLAG_EXECUTABLE = "-e";
        public const string FLAG_ITERATIONS = "-i";
        public const string FLAG_TITLE = "-t";
        public const string FLAG_OUTPUT_FILE = "-o";
        public const string FLAG_CSV = "-oc";
        public const string FLAG_HTML = "-oh";
        public const string DASH = "-";
        public const int ERROR_CODE = 1;

        private TestHarnessArguments()
        {
            // Disable public instantiation
        }

        public string Arguments { get; private set; }
        public string ExecutableName { get; private set; }
        public int Iterations { get; private set; }
        public string Title { get; private set; }
        public string OutputFile { get; private set; }
        public bool OutputCsv { get; private set; }
        public bool OutputHtml { get; private set; }

        public static TestHarnessArguments BuildFrom(string[] args)
        {
            var result = new TestHarnessArguments();

            if (args.Count() < 4
               || !args.Any(x => x == FLAG_EXECUTABLE)
               || !args.Any(x => x == FLAG_OUTPUT_FILE)
               )
            {
                WarnUserAndExit();
                return null;
            }

            var argList = args.ToList();

            result.Arguments = ExtractArguments(argList);
            result.ExecutableName = ExtractExecutableName(argList);
            result.Iterations = ExtractIterations(argList);
            result.OutputCsv = ExtractOutputCsv(argList);
            result.OutputFile = ExtractOutputFileName(argList);
            result.OutputHtml = ExtractOutputHtml(argList);
            result.Title = ExtractTitle(argList);

            if (result.Iterations < 1)
            {
                WarnUserAndExit();
                return null;
            }
            if (result.ExecutableName.Length == 0)
            {
                WarnUserAndExit();
                return null;
            }
            if (result.OutputFile.Length == 0)
            {
                WarnUserAndExit();
                return null;
            }

            result.OutputCsv = !(result.OutputCsv || result.OutputHtml) ? true : result.OutputCsv;
            result.Title = result.Title.Length == 0 ? result.OutputFile : result.Title;

            return result;
        }

        private static int ExtractIterations(List<string> argList)
        {
            var temp = ExtractValuesUpToNextFlag(argList, FLAG_ITERATIONS);

            var result = 0;
            if (int.TryParse(temp, out result))
            {
                return result;
            }
            return -1;
        }

        private static string ExtractOutputFileName(List<string> argList)
        {
            return ExtractValuesUpToNextFlag(argList, FLAG_OUTPUT_FILE);
        }

        private static bool ExtractOutputHtml(List<string> argList)
        {
            if (!argList.Any(x => x == FLAG_HTML))
            {
                return false;
            }
            return argList.Any(x => x == FLAG_HTML);
        }

        private static string ExtractTitle(List<string> argList)
        {
            if (!argList.Any(x => x == FLAG_TITLE))
            {
                return string.Empty;
            }
            return ExtractValuesUpToNextFlag(argList, FLAG_TITLE).Replace("\"", string.Empty);
        }

        private static bool ExtractOutputCsv(List<string> argList)
        {
            if (!argList.Any(x => x == FLAG_CSV))
            {
                return false;
            }
            return argList.Any(x => x == FLAG_CSV);
        }

        private static string ExtractExecutableName(List<string> argList)
        {
            return ExtractValuesUpToNextFlag(argList, FLAG_EXECUTABLE);
        }

        private static string ExtractArguments(List<string> argList)
        {
            if (!argList.Any(x => x == FLAG_ARGUMENTS))
            {
                return string.Empty;
            }
            return ExtractValuesUpToNextFlag(argList, FLAG_ARGUMENTS);
        }

        private static string ExtractValuesUpToNextFlag(List<string> argList, string flag)
        {
            var position = argList.IndexOf(flag);
            var nextDash = FindNextDash(argList, position);

            if (nextDash == position + 1
                || argList.Count <= position + 1)
            {
                return string.Empty;
            }
            if (nextDash > 0)
            {
                var temp = argList.Skip(position + 1)
                    .Take(nextDash - position - 1)
                    .Aggregate((x, y) => x + " " + y);

                return temp.Trim();
            }
            return argList[position + 1];
        }

        private static int FindNextDash(List<string> argList, int position)
        {
            var next = argList.Skip(position + 1)
                              .ToList()
                              .FirstOrDefault(x => x.StartsWith(DASH));
            if (next != null)
            {
                return argList.IndexOf(next);
            }
            return -1;
        }

        private static void WarnUserAndExit()
        {
            WriteErrorMessage("The application must be called with at least these arguments: -e -o -i. Available flags: ");
            WriteErrorMessage(string.Empty);
            WriteErrorMessage("-a  :  arguments for the test run (optional)");
            WriteErrorMessage("-e  :  executable to run (required)");
            WriteErrorMessage("-i  :  number of iterations to run (required)");
            WriteErrorMessage("-t  :  report title (optional - will use output file name if title is not provided)");
            WriteErrorMessage("-o  :  output file name (required)");
            WriteErrorMessage("-oc :  write output as CSV (default)");
            WriteErrorMessage("-oh :  write output as HTML (FUTURE - not yet supported)");
            WriteErrorMessage(string.Empty);
            WriteErrorMessage("Press Enter to exit");
            WaitForKeyPress();

            Exit(ERROR_CODE);
        }


        public static Action<string> WriteErrorMessage = Console.WriteLine;
        public static Func<int> WaitForKeyPress = Console.Read;
        public static Action<int> Exit = Environment.Exit;
    }
}
