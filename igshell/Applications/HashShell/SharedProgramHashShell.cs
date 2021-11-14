using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Script;
using IG.Forms;
using IG.Crypto;

// REMARKS:
// Preffix "Shared" is here because the file is included in the IGShell's project 
// For easy maintenance of compatibility with libraries

namespace IG.App
{

    /// <summary>Main class of the HashShell application.</summary>
    /// <remarks>Defined as partial class such that one paart can be included in the host
    /// project. The other part (which is excluded from the host project) only contains
    /// the Main method that just calls the <see cref="MainHashShell"/> method of this class.</remarks>
    public static partial class ProgramHashShell
    {

        /// <summary>Entry point of the file hashing shell application.</summary>
        /// <param name="AppArguments">Application arguments.</param>
        public static void MainHashShell(string[] args)
        {

            try
            {
                // Enable visuall styles - e.g. lsuch things that Crtl-A means "Select all".
                System.Windows.Forms.Application.EnableVisualStyles();
            }
            catch { }

            int numArgs = 0;
            if (args != null)
            {
                numArgs = args.Length;
            }
            if (numArgs == 0)
            {
                Console.WriteLine(Environment.NewLine
                    + "No arguments were specified." + Environment.NewLine + Environment.NewLine
                    + "Run the application by typing: " + Environment.NewLine
                    + "  " + UtilSystem.GetCurrentProcessExecutableName() + " command <arg1> <arg2> ..."
                    + "or"
                    + "  " + UtilSystem.GetCurrentProcessExecutableName() + " ?" + Environment.NewLine
                    + "for list of commands."
                    + Environment.NewLine + Environment.NewLine
                    + "See the *.cmd files for example commmands." + Environment.NewLine);
                return;
            }
            AppEmbeddedCryptoIGShell application = new AppEmbeddedCryptoIGShell(null /* masterCommand, since it is not embedded here. */);
            //application.Script_Run(AppArguments);
            application.Run(args);
        }

    }


    /// <summary>Shell application to be installed in script application.</summary>
    /// $A Igor Jul10;
    [Obsolete("Currently not used.")]
    public class ShellHash : ShellApplication<CommandLineApplicationInterpreter>
    {

        public ShellHash()
            : base()
        {
            DefaultActiveDir = @"../../testdata/scripts";
            OptDir = DefaultActiveDir;
        }
    }


    /// <summary>Internal script for running embedded applications.</summary>
    /// <remarks>
    /// <para>In the applications that have the command-line interpreter, embedded applications from this class can typically be
    /// run in the following way:</para>
    /// <para>  AppName Internal IG.Script.AppShellExt CommandName arg1 arg2 ...</para>
    /// <para>where AppName is the application name, IG.Script.AppBase is the full name of the script class that contains
    /// embedded applications, CommandName is name of the command thar launches embedded application, and arg1, arg2, etc.
    /// are command arguments for the embedded application.</para></remarks>
    /// <seealso cref="ScriptAppBase"/>
    /// $A Igor Jul10 Jan13;
    [Obsolete("Currently not used.")]
    public class AppHashShell : AppBase, // ScriptAppIgBase, 
        ILoadableScript
    {

        public AppHashShell()
            : base()
        { }

        #region Commands.EmbeddedScripts

        /// <summary>Name of the command that runs the main thesis application.</summary>
        public const string ConstEmbeddedTest = "EmbeddedTest";
        public const string ConstHelpEmbeddedTest = "Embedded test script providing some nested commands.";

        #endregion Commands.EmbeddedScripts

        #region Commands

        /// <summary>Name of the command that performs my custom test.</summary>
        public const string ConstShell = "Shell";
        public const string ConstHelpShell = @"Command shell that enables running pre-installed commands.";

        #endregion Commands

        #region Commands.Hash


        #endregion Commands.Hash


        /// <summary>Adds application commands to the application interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            // Perform the base script's AddCommands:
            base.Script_AddCommands(interpreter, helpStrings);

            // AppShell
            Script_AddCommand(interpreter, helpStrings, ConstShell, AppShell, ConstHelpShell);

            #region EmbeddedScripts
            
            Script_AddCommand(interpreter, helpStrings, ConstEmbeddedTest, AppEmbeddedTest, ConstHelpEmbeddedTest);

            #endregion EmbeddedScripts

        }

        #region Actions.EmbeddedScripts


        /// <summary>Runs a command form the embedded scritp class <see cref="ScriptAppIgBase"/>.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string AppEmbeddedTest(string[] arguments)
        {
            if (arguments == null)
                throw new ArgumentException("Arguments not specified (null array).");
            if (arguments.Length < 1)
                throw new ArgumentException("There are no arguments (empty array). At least command name should be specified, or '?' for help.");

            // Create a new interpreter object for running embedded script's commands:
            AppBase embeddedScriptApplication = new AppBase();  // pass the 1st arg. as nested command name
            embeddedScriptApplication.EmbeddedCommandName = arguments[0];

            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.WriteLine("=========================================================");
            Console.WriteLine("EMBEDDED APPLICATION - class " + embeddedScriptApplication.GetType().FullName + ".");
            Console.WriteLine("Executing command: " + embeddedScriptApplication.EmbeddedCommandName);
            Console.WriteLine();

            // Extract commandline:
            // Transcribe arguments from argument array of the external command's:
            string[] args = new string[arguments.Length - 1];
            for (int i = 1; i < arguments.Length; ++i)
            {
                args[i - 1] = arguments[i];
            }

            /// Run the command by the embedded scripting object:
            embeddedScriptApplication.Run(args);

            Console.WriteLine();
            Console.WriteLine("Embedded script's application finished.");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(Environment.NewLine + Environment.NewLine);

            //LogWriter.Close();  // finalize the log file writer
            return null;
        }


        #endregion Actions.EmbeddedScripts

        #region AuxShells

        AppBase _appBase;

        AppBase AppBase0
        {
            get
            {
                lock (Lock)
                {
                    if (_appBase == null)
                        _appBase = new AppBase();
                }
                return _appBase;
            }
        }


        ShellHash _shell;

        /// <summary>Standard shell, used by the "Shell" command.</summary>
        ShellHash Shell
        {
            get
            {
                lock (Lock)
                {
                    if (_shell == null)
                        _shell = new ShellHash();
                }
                return _shell;
            }
        }

        #endregion AuxShells

        #region Actions


        /// <summary>Shell application. Enables running pre-installed commands.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string AppShell(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Launching shell interpreter...");
            Console.WriteLine("  " + ConstShell + " Internal - runs commands interactively/");
            Console.WriteLine("  " + ConstShell + " Internal ? - runs commands interactively, lists installed commands");
            Console.WriteLine();
            Script_PrintArguments("Application arguments: ", arguments);
            Console.WriteLine("");
            int numShellArguments = 0;
            if (arguments != null)
            {
                if (arguments.Length > 0)
                    numShellArguments = arguments.Length - 1;
            }
            string[] shellArguments = null;
            if (numShellArguments > 0)
                shellArguments = new string[numShellArguments];
            for (int i = 1; i < arguments.Length; ++i)
            {
                shellArguments[i - 1] = arguments[i];
            }
            // new ShellHash().Main(shellArguments);
            Shell.Main(shellArguments);
            return null;
        }


        #endregion Actions


    }  // AppHashShell



}
