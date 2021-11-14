using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IG.Ferdo
{

    /// <summary>Various general utilities used by the application <see cref="IG.Script.AppFerdo"/>.</summary>
    public class UtilitiesFerdo
    {

        /// <summary>Process-wide locking object.</summary>
        public static object LockGlobal = new object();


        private static string _parentWorkingDirectoryPath;

        /// <summary>Gets parent directory of the application's working directory.</summary>
        public static string ParentWorkingDirectoryPath
        {
            get
            {
                if (_parentWorkingDirectoryPath == null)
                {
                    lock (LockGlobal)
                    {
                        if (_parentWorkingDirectoryPath == null)
                        {
                            string trialPath = @"../../testdata";
                            if (Directory.Exists(trialPath))
                            {
                                _parentWorkingDirectoryPath = trialPath;
                            }
                            else
                            {
                                trialPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                if (Directory.Exists(trialPath))
                                {
                                    _parentWorkingDirectoryPath = trialPath;
                                } else
                                {
                                    _parentWorkingDirectoryPath = Directory.GetCurrentDirectory();
                                    // Issue a warning message when there is no better choice and the parent directory must
                                    // be set to the current directory:
                                    Console.WriteLine(Environment.NewLine + Environment.NewLine
                                        + "WARNING: Parend directory of the application working directory set to "
                                        + Environment.NewLine + "  " + _parentWorkingDirectoryPath 
                                        + Environment.NewLine + Environment.NewLine);
                                }
                            }
                        }
                    }
                }
                return _parentWorkingDirectoryPath;
            }
        }


    }  // class Utilities

}