using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Safnet.PerfTestHarness.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestHarnessArgumentsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RejectNullArgList()
        {
            TestHarnessArguments.BuildFrom(null);
        }

        [TestMethod]
        public void BuildFromFullComplementOfArguments()
        {
            var args = new[]
            {
                "-a",
                "these",
                "are",
                "arguments",
                "-e",
                "\"directory with space\\the.exe\"",
                "-i",
                "231",
                "-t",
                "\"A Title With Spaces\"",
                "-o",
                "output",
                "-oc",
                "-oh"
            };

            var result = TestHarnessArguments.BuildFrom(args);

            Assert.AreEqual("these are arguments", result.Arguments, "Arguments");
            Assert.AreEqual("\"directory with space\\the.exe\"", result.ExecutableName, "ExecutableName");
            Assert.AreEqual(231, result.Iterations, "Iterations");
            Assert.AreEqual("A Title With Spaces", result.Title, "Title");
            Assert.AreEqual("output", result.OutputFile, "OutputFile");
            Assert.IsTrue(result.OutputCsv, "OutputCsv");
            Assert.IsTrue(result.OutputHtml, "OutputHtml");
        }

        [TestMethod]
        public void BuildFromMinimumArgumentList()
        {
            var args = new[]
            {
                "-e",
                "\"directory with space\\the.exe\"",
                "-i",
                "231",
                "-o",
                "output",
            };

            var result = TestHarnessArguments.BuildFrom(args);

            Assert.AreEqual(string.Empty, result.Arguments, "Arguments");
            Assert.AreEqual("\"directory with space\\the.exe\"", result.ExecutableName, "ExecutableName");
            Assert.AreEqual(231, result.Iterations, "Iterations");
            Assert.AreEqual("output", result.Title, "Title");
            Assert.AreEqual("output", result.OutputFile, "OutputFile");
            Assert.IsTrue(result.OutputCsv, "OutputCsv");
            Assert.IsFalse(result.OutputHtml, "OutputHtml");
        }

        

        [TestMethod]
        public void InvalidIterations()
        {
            var args = new[]
            {
                "-e",
                "\"directory with space\\the.exe\"",
                "-i",
                "asdfa",
                "-o",
                "output",
            };

            object exitCalled = null;
            TestHarnessArguments.Exit = (int actualExitCode) =>
            {
                exitCalled = new object();
                Assert.AreEqual(TestHarnessArguments.ERROR_CODE, actualExitCode, "actualExitCode");
            };
            object readCalled = null;
            TestHarnessArguments.WaitForKeyPress = () =>
            {
                readCalled = new object();
                return 1;
            };

            object consoleWriteCalled = null;
            TestHarnessArguments.WriteErrorMessage = (string actualString) =>
            {
                // don't evaluate actual string - not worth it.
                consoleWriteCalled = new object();
            };


            // execute the system under test
            var result = TestHarnessArguments.BuildFrom(args);

            ValidateNegativeResult(exitCalled, readCalled, consoleWriteCalled, result);
        }


        [TestMethod]
        public void MissingIterationFlag()
        {
            var args = new[]
            {
                "-e",
                "\"directory with space\\the.exe\"",
                //"-i",
                "123",
                "-o",
                "output",
            };

            object exitCalled = null;
            TestHarnessArguments.Exit = (int actualExitCode) =>
            {
                exitCalled = new object();
                Assert.AreEqual(TestHarnessArguments.ERROR_CODE, actualExitCode, "actualExitCode");
            };
            object readCalled = null;
            TestHarnessArguments.WaitForKeyPress = () =>
            {
                readCalled = new object();
                return 1;
            };

            object consoleWriteCalled = null;
            TestHarnessArguments.WriteErrorMessage = (string actualString) =>
            {
                // don't evaluate actual string - not worth it.
                consoleWriteCalled = new object();
            };


            // execute the system under test
            var result = TestHarnessArguments.BuildFrom(args);

            ValidateNegativeResult(exitCalled, readCalled, consoleWriteCalled, result);
        }


        [TestMethod]
        public void MissingIterationValue()
        {
            var args = new[]
            {
                "-e",
                "\"directory with space\\the.exe\"",
                "-i",
                //"123",
                "-o",
                "output",
            };

            object exitCalled = null;
            TestHarnessArguments.Exit = (int actualExitCode) =>
            {
                exitCalled = new object();
                Assert.AreEqual(TestHarnessArguments.ERROR_CODE, actualExitCode, "actualExitCode");
            };
            object readCalled = null;
            TestHarnessArguments.WaitForKeyPress = () =>
            {
                readCalled = new object();
                return 1;
            };

            object consoleWriteCalled = null;
            TestHarnessArguments.WriteErrorMessage = (string actualString) =>
            {
                // don't evaluate actual string - not worth it.
                consoleWriteCalled = new object();
            };


            // execute the system under test
            var result = TestHarnessArguments.BuildFrom(args);

            ValidateNegativeResult(exitCalled, readCalled, consoleWriteCalled, result);
        }

        [TestMethod]
        public void MissingExecutableFlag()
        {
            var args = new[]
            {
                //"-e",
                "\"directory with space\\the.exe\"",
                "-i",
                "123",
                "-o",
                "output",
            };

            object exitCalled = null;
            TestHarnessArguments.Exit = (int actualExitCode) =>
            {
                exitCalled = new object();
                Assert.AreEqual(TestHarnessArguments.ERROR_CODE, actualExitCode, "actualExitCode");
            };
            object readCalled = null;
            TestHarnessArguments.WaitForKeyPress = () =>
            {
                readCalled = new object();
                return 1;
            };

            object consoleWriteCalled = null;
            TestHarnessArguments.WriteErrorMessage = (string actualString) =>
            {
                // don't evaluate actual string - not worth it.
                consoleWriteCalled = new object();
            };


            // execute the system under test
            var result = TestHarnessArguments.BuildFrom(args);

            ValidateNegativeResult(exitCalled, readCalled, consoleWriteCalled, result);
        }



        [TestMethod]
        public void MissingExecutableValue()
        {
            var args = new[]
            {
                "-e",
                //"\"directory with space\\the.exe\"",
                "-i",
                "123",
                "-o",
                "output",
            };

            object exitCalled = null;
            TestHarnessArguments.Exit = (int actualExitCode) =>
            {
                exitCalled = new object();
                Assert.AreEqual(TestHarnessArguments.ERROR_CODE, actualExitCode, "actualExitCode");
            };
            object readCalled = null;
            TestHarnessArguments.WaitForKeyPress = () =>
            {
                readCalled = new object();
                return 1;
            };

            object consoleWriteCalled = null;
            TestHarnessArguments.WriteErrorMessage = (string actualString) =>
            {
                // don't evaluate actual string - not worth it.
                consoleWriteCalled = new object();
            };


            // execute the system under test
            var result = TestHarnessArguments.BuildFrom(args);

            ValidateNegativeResult(exitCalled, readCalled, consoleWriteCalled, result);
        }


        [TestMethod]
        public void MissingOutputFileFlag()
        {
            var args = new[]
            {
                "-e",
                "\"directory with space\\the.exe\"",
                "-i",
                "123",
                //"-o",
                "output",
            };

            object exitCalled = null;
            TestHarnessArguments.Exit = (int actualExitCode) =>
            {
                exitCalled = new object();
                Assert.AreEqual(TestHarnessArguments.ERROR_CODE, actualExitCode, "actualExitCode");
            };
            object readCalled = null;
            TestHarnessArguments.WaitForKeyPress = () =>
            {
                readCalled = new object();
                return 1;
            };

            object consoleWriteCalled = null;
            TestHarnessArguments.WriteErrorMessage = (string actualString) =>
            {
                // don't evaluate actual string - not worth it.
                consoleWriteCalled = new object();
            };


            // execute the system under test
            var result = TestHarnessArguments.BuildFrom(args);

            ValidateNegativeResult(exitCalled, readCalled, consoleWriteCalled, result);
        }



        [TestMethod]
        public void MissingOutputFileValue()
        {
            var args = new[]
            {
                "-e",
                "\"directory with space\\the.exe\"",
                "-i",
                "123",
                "-o",
                //"output",
            };

            object exitCalled = null;
            TestHarnessArguments.Exit = (int actualExitCode) =>
            {
                exitCalled = new object();
                Assert.AreEqual(TestHarnessArguments.ERROR_CODE, actualExitCode, "actualExitCode");
            };
            object readCalled = null;
            TestHarnessArguments.WaitForKeyPress = () =>
            {
                readCalled = new object();
                return 1;
            };

            object consoleWriteCalled = null;
            TestHarnessArguments.WriteErrorMessage = (string actualString) =>
            {
                // don't evaluate actual string - not worth it.
                consoleWriteCalled = new object();
            };


            // execute the system under test
            var result = TestHarnessArguments.BuildFrom(args);

            ValidateNegativeResult(exitCalled, readCalled, consoleWriteCalled, result);
        }

        private static void ValidateNegativeResult(object exitCalled, object readCalled, object consoleWriteCalled, TestHarnessArguments result)
        {
            Assert.IsNull(result, "There shouldn't be a result");
            Assert.IsNotNull(exitCalled, "application did not exit");
            Assert.IsNotNull(readCalled, "must wait for key press");
            Assert.IsNotNull(consoleWriteCalled, "error message should be written to the console");
        }
    }
}
