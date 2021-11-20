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

using IG.Ferdo;

namespace IG.Script
{


    /// <summary>Internal script for running embedded applications.</summary>
    /// <remarks>
    /// <para>In the applications that have the command-line interpreter, embedded applications from this class can typically be
    /// run in the following way:</para>
    /// <para>  AppName Internal IG.Script.AppShellExt CommandName arg1 arg2 ...</para>
    /// <para>where AppName is the application name, IG.Script.AppBase is the full name of the script class that contains
    /// embedded applications, CommandName is name of the command thar launches embedded application, and arg1, arg2, etc.
    /// are command arguments for the embedded application.</para></remarks>
    /// <seealso cref="ScriptAppBase"/>
    /// $A Igor Jan13; Ferdo Mar13;
    public class AppFerdo :  AppIgorForFerdo, 
        ILoadableScript, ILockable
    {

        public AppFerdo()
            : base()
        { }


        public AppFerdo(string masterCommand)
            : base()
        { EmbeddedCommandName = masterCommand; }


        #region Commands
        
        // Embedded shell application:

        /// <summary>Name of the command that performs my custom test.</summary>
        public const string ConstShell = "Shell";
        public const string ConstHelpShell = 
@"Command shell that enables running pre-installed commands.";

        /// <summary>Name of the command that runns the main thesis application.</summary>
        public const string ConstFerdoMain = "MainFerdo";
        public const string ConstHelpFerdoMain = "Main application for Ferdo's programs.";

        /// <summary>Name of the command that performs my custom test.</summary>
        public const string ConstFerdoTest = "TestFerdo";
        public const string ConstHelpFerdoTest = "Custom test application from Ferdo.";

        #endregion Commands

        /// <summary>Adds application commands to the application interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);

            // Shell Application:
            Script_AddCommand(interpreter, helpStrings, ConstShell, AppShell, ConstHelpShell);

            Script_AddCommand(interpreter, helpStrings, ConstFerdoMain, AppFerdoMain, ConstHelpFerdoMain);
            Script_AddCommand(interpreter, helpStrings, ConstFerdoTest, AppFerdoTest, ConstHelpFerdoTest);

        }


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
            new ShellIGLib().Main(shellArguments);
            return null;
        }


        /// <summary>Runs the main application for Ferdo's utilities and programs .</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string AppFerdoMain(string[] arguments)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.WriteLine("=========================================================");
            Console.WriteLine("MAIN APPLICATION - Ferdo's utilities and programs.");
            Console.WriteLine();

            LogWriter.WriteLine("Pognan izračun. Delovni direktorij: " + WorkingDirectoryPath + Environment.NewLine);
            Console.WriteLine("Delovni direktorij: " + WorkingDirectoryPath + Environment.NewLine);
            LogWriter.Flush();
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
                {
                    for (int i = 1; i < numArguments; ++i)
                        Console.WriteLine(i + ". " + " argument: " + arguments[i]);
                }
            }
            Console.WriteLine(Environment.NewLine + Environment.NewLine);

            // #### INSERT HERE YOUR CODE THAT EXECUTES YOUR COMMAND (application) FerdoMain!

            Console.WriteLine("To je moj prv izpis v aplikaciji FerdoMain.");

            // KONEC TVOJE KODE, zaključne stvari
            Console.WriteLine(Environment.NewLine + Environment.NewLine);
            Console.WriteLine("Main application finished.");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine(Environment.NewLine + Environment.NewLine);

            LogWriter.Close();  // finalize the log file writer
            return null;
        }

        /// <summary>Test application, just to demonstrate how applications are added to the script.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string AppFerdoTest(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("THIS IS TEST APPLICATION from the Ferdo's utilities and programs.");
            Console.WriteLine("This script is alive.");
            Script_PrintArguments("Application arguments: ", arguments);
            Console.WriteLine(Environment.NewLine);

            // #### INSERT HERE YOUR CODE THAT EXECUTES YOUR COMMAND (application) FerdoTest!

            Console.WriteLine("To je moj prv izpis v aplikaciji FerdoTest.");



            // KONEC TVOJE KODE, zaključne stvari
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "==== END of the test application.");
            Console.WriteLine();
            return null;
        }


        #endregion Actions




    }  // AppFerdo

}