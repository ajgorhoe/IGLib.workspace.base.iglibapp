// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Lib;
using IG.Num;
using IG.Neural;
using IG.Script;

using IG.Crypto;

using IG.App;
using System.Diagnostics;


// REMARKS:
// Prefix "Shared" is here because the file is included in the IGShell's project 
// For easy maintenance of compatibility with libraries

namespace IG.Test 
{ 


    /// <summary>Class containing the main method.</summary>
    /// <remarks><para>Follows standard scheme for IGLib-based simple application.</para></remarks>
    /// $A Igor Oct12 Jan13;
    public class ProgramIGShell 
    {
 
        /// <summary>Entry point of the application.</summary>
        /// <param name="AppArguments">Application arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {

            //if (0 == 1)
            //{
            //    // Write command-line arguments and current directory for control... This was used for migration to SDK-style projects.
            //    Console.WriteLine($"Program {Process.GetCurrentProcess().ProcessName}: command-line argumets: ");
            //    for (int i = 0; i < args.Length; ++i) { Console.WriteLine("  " + i + ": " + args[i] + " "); }
            //    Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            //    Console.WriteLine("Press any key to continue!");
            //    Console.ReadKey();
            //}


            Console.WriteLine("\n\n*************************\n\n*************************\n\n");

            //IgShellApp.Init();

            Console.WriteLine("\n\n*********************\n  After Init.\n*********************\n");

            ApplicationIgShell appInfo = new ApplicationIgShell();

            Console.WriteLine("\n\n*********************\n  After new AppInfo().\n*********************\n");

            appInfo.Run(args);

            Console.WriteLine("\n\n*********************\n  After new Run().\n*********************\n");


            //Type dummyType = typeof(MathNet.Numerics.LinearAlgebra.Double.DenseVector);

            //if (true)
            //{
            //    IG.Num.IVector v1 = null, v2 = null;
                
            //    // Lines below are to test if dependency on MathNet.Numerics is OK:
            //    v1 = new Vector(new double[] { 1.1, 1.2, 1.3 });
            //    Vector.Multiply(v1, 2, ref v2);
            //    //Console.WriteLine("Vector v1: " + v1);
            //    //Console.WriteLine("Vecto rv2: " + v2);
            //    //Console.WriteLine("Scalar product of two vectors: " + Vector.ScalarProduct(v1, v2));

            //    // Line below is to try to include MathNet.Numerics.dll in generated output of build:
            //    MathNet.Numerics.LinearAlgebra.Double.DenseVector v = 
            //        new MathNet.Numerics.LinearAlgebra.Double.DenseVector(2);
            //}



            //try
            //{
            //    // Enable visuall styles - e.g. lsuch things that Crtl-A means "Select all".
            //    System.Windows.Forms.Application.EnableVisualStyles();
            //}
            //catch (Exception ex) {
            //    Console.WriteLine(Environment.NewLine + Environment.NewLine + "Ecception was thrown: " + Environment.NewLine
            //        + "  " + ex.Message + Environment.NewLine);
            //}

            //int numArgs = 0;
            //if (args != null)
            //{
            //    numArgs = args.Length;
            //}
            //if (numArgs == 0)
            //{
            //    Console.WriteLine(Environment.NewLine 
            //        + "No arguments were specified." + Environment.NewLine + Environment.NewLine
            //        + "Run the application by typing: " +  Environment.NewLine 
            //        + "  " +  UtilSystem.GetCurrentProcessExecutableName()  + " command <arg1> <arg2> ..."  + Environment.NewLine
            //        + "or" + Environment.NewLine
            //        + "  " +  UtilSystem.GetCurrentProcessExecutableName()  + " ?" + Environment.NewLine
            //        + "for list of commands or " + Environment.NewLine 
            //        + "  " + UtilSystem.GetCurrentProcessExecutableName() + " Interactive" + Environment.NewLine
            //        + "for interactive mode or " + Environment.NewLine
            //        + "  " + UtilSystem.GetCurrentProcessExecutableName() + " Run fileName" + Environment.NewLine
            //        + "for running the specified command file."
            //        + Environment.NewLine + Environment.NewLine
            //        + "See the *.cmd files for example commmands." + Environment.NewLine);
            //    return;
            //}



            //AppIGShell application = new AppIGShell();
            //IgShellApp.Init();


            ////application.Script_Run(AppArguments);
            //application.Run(args);



        } // Main(string[]) 




    }  // class ProgramIGShell


}


