SET EXE=..\MongoExperiments\MongoExperiments.CS\bin\Debug\MongoExperiments.CS.exe
SET ARGS=
SET ITERATIONS=50
SET TITLE="C# Mongo Experiment 50 Iterations"
SET FORMAT=-oc
SET OUTFILE=.\cs50

.\PerfTestHarness\bin\debug\PerfTestHarness.exe -e %EXE% -a %ARGS% -i %ITERATIONS% -o %OUTFILE% -t %TITLE% %FORMAT%

IF EXIST %OUTFILE%.csv START %OUTFILE%.csv
IF EXIST %OUTFILE%.html START %OUTFILE%.html

PAUSE
