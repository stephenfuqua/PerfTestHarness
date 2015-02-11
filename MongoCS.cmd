SET EXE=..\MongoExperiments\MongoExperiments.CS\bin\Debug\MongoExperiments.CS.exe
SET ARGS=
SET ITERATIONS=5
SET TITLE="C# Mongo Experiment 5 Iterations"
SET FORMAT=-oc
SET OUTFILE=.\cs5

.\PerfTestHarness\bin\debug\PerfTestHarness.exe -e %EXE% -a %ARGS% -i %ITERATIONS% -o %OUTFILE% -t %TITLE% %FORMAT%

IF EXIST %OUTFILE%.csv START %OUTFILE%.csv
IF EXIST %OUTFILE%.html START %OUTFILE%.html

PAUSE
