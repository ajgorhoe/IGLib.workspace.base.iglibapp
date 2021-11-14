using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IG.App
{

    public static partial class ProgramHashShell
    {

        /// <summary>Entry point of the file hashing shell application.</summary>
        /// <param name="args">Application arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            MainHashShell(args);
        }

    }

}
