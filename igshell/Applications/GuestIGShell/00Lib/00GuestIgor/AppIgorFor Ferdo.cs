// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TESTING SCRIPT FILE: various examples.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using IG.Num;
using IG.Lib;

using IG.Plot2d;
using IG.Gr3d;
using IG.Gr;

using Utilities = IG.Ferdo.UtilitiesFerdo;
using Vector = IG.Num.Vector;

namespace IG.Script
{


    /// <summary>Internal script for running embedded applications.
    /// <para>This script will contain examples that are prepared for Ferdo by Igor Gresovnik.</para></summary>
    /// <remarks>
    /// <para>In the applications that have the command-line interpreter, embedded applications from this class can typically be
    /// run in the following way:</para>
    /// <para>  AppName Internal IG.Script.AppShellExt CommandName arg1 arg2 ...</para>
    /// <para>where AppName is the application name, IG.Script.AppBase is the full name of the script class that contains
    /// embedded applications, CommandName is name of the command that launches embedded application, and arg1, arg2, etc.
    /// are command arguments for the embedded application.</para></remarks>
    /// <seealso cref="ScriptAppBase"/>
    /// $A Igor Jan01;
    public class AppIgorForFerdo: AppBase, ILoadableScript
    {

        public AppIgorForFerdo()
            : base()
        { }


        #region Directories

        /// <summary>Default name of the working directory for Ferdo's utilities and programs.</summary>
        public const string DefaultWorkingDirectoryName = "Ferdo";

        /// <summary>Default name of the file where solution is written.</summary>
        public const string DefaultSolutionFileName = "Solution.txt";

        /// <summary>Default name of the application's log file.</summary>
        public const string DefaultLogFileName = "Log.txt";
        
        /// <summary>Returns parent directory of the working directory. 
        /// Delegated to <see cref="Utilities.ParentWorkingDirectoryPath"/></summary>
        public virtual string ParentWorkingDirectory
        {
            get { return Utilities.ParentWorkingDirectoryPath; }
        }


        protected string _workingDirectoryName = DefaultWorkingDirectoryName;


        /// <summary>Name of the working directory.</summary>
        public virtual string WorkingDirectoryName
        {
            get 
            {
                return _workingDirectoryName;
            }
            protected set 
            {
                if (string.IsNullOrEmpty(value))  
                {
                    // Can not be set to null or empty string, restore default directory name:
                    value = DefaultWorkingDirectoryName;
                }
                if (value != _workingDirectoryName)
                {
                    _workingDirectoryName = value;
                    // Invalidate dependencies:
                    WorkingDirectoryPath = null;
                }
            }
        }


        protected string _workingDirectoryPath;

        /// <summary>Path of the application's working directory.</summary>
        public virtual string WorkingDirectoryPath
        {
            get
            {
                if (string.IsNullOrEmpty(_workingDirectoryPath))
                {
                    _workingDirectoryPath = Path.Combine(ParentWorkingDirectory, WorkingDirectoryName);
                    if (!Directory.Exists(_workingDirectoryPath))
                    {
                        if (Directory.Exists(ParentWorkingDirectory))
                        {
                            Directory.CreateDirectory(_workingDirectoryPath);
                        }
                        if (!Directory.Exists(_workingDirectoryPath))
                            throw new IOException("Working directory does not exist and could not be created. " + Environment.NewLine
                                + "  Parent directory: " + ParentWorkingDirectory);
                    }
                }
                return _workingDirectoryPath;
            }
            protected set
            {
                if (value != null)
                    throw new ArgumentException("Application output directory can not be set explicitly, except to null value.");
                else
                {
                    _workingDirectoryPath = null;
                    // Invalidate dependencies:
                    SolutionFilePath = null;
                }
            }
        }


        protected string _solutionFileName = DefaultSolutionFileName;

        /// <summary>Name of the file where solution is written to.</summary>
        public virtual string SolutionFileName
        {
            get
            { return _solutionFileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = DefaultSolutionFileName;  // can not be null or empty string; reset to default value
                }
                if (value != _solutionFileName)
                {
                    _solutionFileName = value;
                    // Invalidate dependencies:
                    SolutionFilePath = null;
                }
            }
        }

        protected string _solutionFilePath;

        /// <summary>Path to the solution file.</summary>
        public virtual string SolutionFilePath
        {
            get
            {
                if (_solutionFilePath == null)
                {
                    _solutionFilePath = Path.Combine(WorkingDirectoryPath, SolutionFileName);
                }
                return _solutionFilePath;
            }
            set
            {
                if (value != null)
                    throw new ArgumentException("Solution file path can not be set explicitly, except to null value.");
                else
                {
                    _solutionFilePath = null;
                }
            }
        }



        protected string _logFileName = DefaultLogFileName;

        /// <summary>Name of the file where notes on operation can be logged.</summary>
        public virtual string LogFileName
        {
            get
            { return _logFileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = DefaultLogFileName;  // can not be null or empty string; reset to default value
                }
                if (value != _logFileName)
                {
                    _logFileName = value;
                    // Invalidate dependencies:
                    LogFilePath = null;
                }
            }
        }

        protected string _logFilePath;

        /// <summary>Path to the log file where notes on operations can be logged.</summary>
        public virtual string LogFilePath
        {
            get
            {
                if (_logFilePath == null)
                {
                    _logFilePath = Path.Combine(WorkingDirectoryPath, LogFileName);
                }
                return _logFilePath;
            }
            set
            {
                if (value != null)
                    throw new ArgumentException("Log file path can not be set explicitly, except to null value.");
                else
                {
                    _logFilePath = null;
                    // Invalidate dependencies:
                    if (!_externalLogWriterUsed)
                        LogWriter = null;  // log file path changed, we need to close the current log writer if open
                }
            }
        }



        /// <summary>Initializes a text writer used for logging.
        /// <para>Initialization consists of writing introductory text that includes the current time.</para></summary>
        /// <param name="writer">Logging text writer that is initialized.</param>
        protected virtual void InitLogWriter(TextWriter writer)
        {
            writer.WriteLine(Environment.NewLine + Environment.NewLine 
                + "** New log session opened on " + DateTime.Now 
                + Environment.NewLine + Environment.NewLine);
        }

        private TextWriter _logWriter;

        protected bool _externalLogWriterUsed = false;

        /// <summary>File writer used for logging notes on operation of the application.</summary>
        /// <remarks><para>Getter always provides an initialized file writer that can be immediately used for writing.</para>
        /// <para>Setting to null closes the log file writer, meaning also that all contents are flushed.
        /// In this case, the next call togetter will open a new writer and re-initialize it, together with writing introductory
        /// text by the <see cref="InitLogWriter"/> method.</para></remarks>
        public TextWriter LogWriter
        {
            get
            {
                if (_logWriter == null)
                {
                    _externalLogWriterUsed = false;
                    try
                    {
                        _logWriter = new StreamWriter(LogFilePath, true);
                        InitLogWriter(_logWriter);
                    }
                    catch (Exception originalException)
                    {
                        // Error occurred when trying to initialize file writer, throw exception:
                        _logWriter = null;
                        throw new IOException("Internal log writer can not be initialized. " + Environment.NewLine
                            + "  File path: " + LogFilePath, originalException);
                    }
                }
                return _logWriter;
            }
            protected set
            {
                if (value == null)
                {
                    if (_logWriter != null)
                    {
                        if (!_externalLogWriterUsed)
                        {
                            // Perform finalization before the text writer goes out of scope:
                            try { _logWriter.Close(); }
                            catch { }
                        }
                        _logWriter = null;
                        _externalLogWriterUsed = false;
                    }
                } else
                {
                    // Set external log writer, initialize and try if it works properly:
                    try
                    {
                        InitLogWriter(value);
                        _logWriter = value;
                        _externalLogWriterUsed = true;
                    }
                    catch
                    {
                        // External writer does not work properly; issue a notification and reset it, 
                        // (so it will be opn with default file path on demand):
                        _logWriter = null;
                        _externalLogWriterUsed = false;
                        Console.WriteLine(Environment.NewLine + Environment.NewLine
                            + "WARNING: Provided external logger does not work, reset to default."
                            + Environment.NewLine);
                    }
                }
            }
        }


        #endregion Directories



        #region Commands

        /// <summary>Name of the command that runns the main thesis application.</summary>
        public const string ConstExGraphics = "ExGraphics";
        public const string ConstHelpExGraphics = "Customized function for running some examples.";

        /// <summary>Name of the command that runns the main thesis application.</summary>
        public const string ConstExVector = "ExVector";
        public const string ConstHelpExVector = "Customized function for running some vector examples.";

        /// <summary>Name of the command that runns the main thesis application.</summary>
        public const string ConstExFunction = "ExFunction";
        public const string ConstHelpExFunction = "Customized function for running some real function examples.";


        #endregion Commands




        /// <summary>Adds application commands to the application interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstExGraphics, AppExGraphics, ConstHelpExGraphics);
            Script_AddCommand(interpreter, helpStrings, ConstExVector, AppExVector, ConstHelpExVector);
            Script_AddCommand(interpreter, helpStrings, ConstExFunction, AppExFunction, ConstHelpExFunction);
        }



        #region Actions

        /// <summary>Runs some graphics examples.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string AppExGraphics(string[] arguments)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.WriteLine("=========================================================");
            Console.WriteLine("SOME CUSTOM GRAPHICS EXAMPLSES.");
            Console.WriteLine();


            int numArguments = 0;
            if (arguments != null)
            {
                if (arguments.Length > 0)
                    numArguments = arguments.Length;
            }
            if (numArguments > 0)
            {
                Console.WriteLine("Ukaz, ki je sprožil to metodo: " + arguments[0]);
                if (numArguments > 1)
                    Console.WriteLine("1. argument: " + arguments[1]);
                if (numArguments > 2)
                    Console.WriteLine("2. argument: " + arguments[1]);
            }


            LoadableScriptBase script;

            // Some 2D graphics examples:

            // Plot sine functions through script command:
            script = new ScriptGraphics2dBase();
            script.Run(new string[] { "Graph", "SinePlots", "5", "200" });
            // Do a similar thing directly rather than through script:
            int nCurves = 3;
            int nPoints = 150;
            PlotterZedGraph.ExempleSinePlots(nCurves, nPoints);


            // Plot Lissajous curves through script command:
            script = new ScriptGraphics2dBase();
            script.Run(new string[] {"Graph", "CurvePlotLissajous", "5", "4"});
            // Do a similar thing directly rather than through script:
            int nX = 8;
            int nY = 7;
            PlotterZedGraph.ExampleLissajous(nX, nY);

            // Some 3D graphics examples:

            // Plot some surfaces in 3D:
            script = new ScriptGraphics3DBase();
            script.Run(new string[] { "Plot3d", "SurfaceComparison" });
            // Do a similar thing directly rather than through script:
            VtkPlotBase.ExampleCustomSurfaceComparison();


            // Plot some parametric curves in 3D:
            script = new ScriptGraphics3DBase();
            script.Run(new string[] { "Plot3d", "CurvePlotLissajous" });
            // Do a similar thing directly rather than through script:
            VtkPlotBase.ExampleCurvePlotLissajous();

            Console.WriteLine();
            Console.WriteLine("Application with graphics examples finished.");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            return null;
        }


        
        /// <summary>Runs some vector examples.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string AppExVector(string[] arguments)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.WriteLine("=========================================================");
            Console.WriteLine("SOME CUSTOM VECTOR EXAMPLSES.");
            Console.WriteLine();


            int numArguments = 0;
            if (arguments != null)
            {
                if (arguments.Length > 0)
                    numArguments = arguments.Length;
            }
            if (numArguments > 0)
            {
                Console.WriteLine("Ukaz, ki je sprožil to metodo: " + arguments[0]);
                if (numArguments > 1)
                    Console.WriteLine("1. argument: " + arguments[1]);
                if (numArguments > 2)
                    Console.WriteLine("2. argument: " + arguments[1]);
            }



            //// $$$$ WARNING: Lines below were commented due to compiler errors:
            //IG.Num.IVector v1 = null, v2=null; 
            //v1 = new Vector(new double[] { 1.1, 1.2, 1.3 });
            //Vector.Multiply(v1, 2, ref v2);
            //Console.WriteLine("Vector v1: " + v1);
            //Console.WriteLine("Vecto rv2: " + v2);
            //Console.WriteLine("Scalar product of two vectors: " + Vector.ScalarProduct(v1, v2));


            Console.WriteLine();
            Console.WriteLine("Application with graphics examples finished.");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            return null;
        }


        public string AppExFunction(string[] arguments)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.WriteLine("=========================================================");
            Console.WriteLine("SOME CUSTOM FUNCTION EXAMPLES.");
            Console.WriteLine();


            double 
                start = 0, end = 0.1;
            int num = 11;
            IRealFunction f = Func.GetQuadratic(1, 0.5, -3);
            Console.WriteLine("Quadratic function: " + f.ToString());
            Console.WriteLine("Table of values of a quadratic function:");
            f.Tabulate(start, end, num, true);

            Console.WriteLine();
            Console.WriteLine("User defined real function: ");
            // First way: definition of a function via anonymous delegates that define how 
            // values and derivatives are defined:
            f = new RealFunction(
                /* Anonymous delegate for function value: */
                delegate(double x) { return x * x + Math.Sin(x); },
                /* Anonymous delegate for function derivative: */
                delegate(double x) { return 2 * x + Math.Cos(x); },
                /* Anonymous delegate for function second derivativevalue: */
                delegate(double x) { return 2 - Math.Sin(x); }
            );
            // Alternative but equivalent way - by using lambda expressions:
            f = new RealFunction(
                /* Expression for function value: */
                (double x) => x * x + Math.Sin(x),
                /* Expression for function derivative: */
                 (double x) =>  2 * x + Math.Cos(x)
                /* Second derivative not defined in this case */
            );
            f.Tabulate(start, end, num, true);

            Console.WriteLine();
            Console.WriteLine("Application with graphics examples finished.");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            return null;
        }


        #endregion Actions




    }  // class AppIgorGresovnik

}