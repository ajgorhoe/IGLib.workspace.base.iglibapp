using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Forms;
using IG.Crypto;
    
    // REMARKS:
    // Preffix "Shared" is here because the file is included in the IGShell's project 
    // For easy maintenance of compatibility with libraries

namespace IG.App
{

    public static partial class ProgramHashGui
    {


        /// <summary>Entry point of the GUI-based file hashing application.</summary>
        /// <param name="AppArguments">Application arguments.</param>
        [STAThread]
        public static void MainHashGui(string[] args)
        {
            try
            {
                // Enable visuall styles - e.g. lsuch things that Crtl-A means "Select all".
                System.Windows.Forms.Application.EnableVisualStyles();
            }
            catch { }

            HashForm hashForm = new HashForm();
            hashForm.ShowDialog();
        }

    }


}
