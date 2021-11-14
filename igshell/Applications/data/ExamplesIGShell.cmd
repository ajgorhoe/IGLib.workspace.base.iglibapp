
C C This file contains example commands for the IGShell application.

C C Run the application (igs.exe) with commandline arguments
C C Copied from here!
C C You can also uncomment some lines and interpret this file. In order
C C to do this, run the application with Run as the first argument and
C C name of tis file as second argument.



C C ***************************************************
C C INTERPRETER UTILITIES:

C C Running shell interpreter in INTERACTIVE MODE:
C Shell Interactive
C Shell Interactive Help
C Shell Interactive Calc

C C Running a caluclator:
C C State of the calculator is persistent, therefoere expressions (including variable definition)
C C can be evaluated in separate blocks.
C C To calculate a single expression:
C Calc 5*cos(const_pi)/3

C C In interactive mode, ? prints calculator help.
C C To run an interactive shell where several expressions can be evaluated:
C Calc
C C JS>log(pow(const_e,12))
C C      = 12
C C JS>a=5
C C      = 5
C C JS>b=2
C C      = 2
C C JS>3*(a+b)
C C      = 21
C C JS>/q
C C Command-line JavaScript expression evaluator stopped.

C C Running SYSTEM COMMANDS:
C C Run a single system command:
C Sys dir
C C Run a shell where several system commands can be run:
C C To exit the shell, insert an empty line, then 1.
C Sys
C C System> dir
C C System> cls
C C System>
C C System>Exit the system command-line interpreter (0/1)? 1
C C Exiting system command-line interpreter...



C C ***************************************************
C C EMBEDDED CRYPTOGRAPHIC applicationS (Crypto):

C C Launch a form for calculation and verification of file and text
C C hash functions (checksums - MD5, SHA-1, SHA-256, SHA-512):
C Crypto HashForm
C C Calculate file hashes for the specified file, eventually write information
C C to a file:
C C Crypto GetFileHash FileName <WriteToFile>
C Crypto GetFileHash readme.txt false


C C Crypto CheckSum:
C C Calculate / verify hashes of files or strings by the CheckSum command:
C C Crypto CheckSum <-c> <-s string> <-h hash> <-t hashType> <-o outputFile> <inputFile1> <inputFile2> ...: 
C C   Calculates or verifies various types of hash values for files or strings. Calculated file hashes
C C   can be saved to a file.
C C     -t hashType: specifies hash type (MD5, SHA-1, SHA-256, SHA-512)
C C     -c: verification rather than calculation of hashes.
C C     -s: hash is calculated or verified for the specified string rather than file(s).
C C     -h hash: hash value to be verified.
C C     -o outputFile: output file where calculated hashes are written.
C C     - inputFile1 inputFile2 ...: input files, either files whose hashes are calculated, or files
C C       containing hash values to be verified (in the case of -c option)

C C Calculate MD5 checksums of several files & store them to hashes.MD5:
C Crypto CheckSum -t MD5 -o hashes.MD5 app.cmd examples.cmd excrypto.cmd 

C C Verify previously calculated checksums:
C Crypto CheckSum -t MD5 -c hashes.MD5 

C C Calculate & verify other kind of checksum (e.g. SHA-1, SHA-256, SHA-512):
C Crypto CheckSum -t SHA-256 -o hashes.SHA256 app.cmd examples.cmd excrypto.cmd
C Crypto CheckSum -t SHA-256 -c hashes.SHA256

C C Calculate hash for a single file (without writing to a file): 
C Crypto CheckSum -t SHA-1 app.cmd 

C C Direct verification of specified hash value of a file
C Crypto CheckSum -t MD5 -c app.cmd -h 207fbc52a83a3f2538713e6e1f73697d

C C Calculation of hashes (of various types) of the string "My String":
C C MD5 = 4545102cc40ea0a85124cf4b31574661
C C SHA1 = 07841b2e0fda6cfbf7c6bf00f179233cf4e3247b
C C SHA256 = 8a7046a0b97e45470b13f30448c9d7d959aa5eea583d2f007921736b2141ac75
C C SHA512 = d159694d78c06886143e08dadc50cb89a96a41c766b603fa07fe3de91e170bf2942545c1ca17e280f572fc829de6059a80e75f4623e736915265f8938bb19e39
C Crypto CheckSum -t MD5 -s "My String"
C Crypto CheckSum -t SHA1 -s "My String"
C Crypto CheckSum -t SHA256 -s "My String"
C Crypto CheckSum -t SHA512 -s "My String"

C C Verification of a string hash value:
C Crypto CheckSum -t MD5 -c -s "My String" -h 4545102cc40ea0a85124cf4b31574661
C Crypto CheckSum -t SHA256 -c -s "My String" -h 8a7046a0b97e45470b13f30448c9d7d959aa5eea583d2f007921736b2141ac75



C C ***************************************************
C C EXAMPLES of using IGLib:

C C Some examples run through the Shell Internal command:
C 
C C 2D Plots: 
C 
C C Check which tests are available:
C Shell Internal IG.Script.ScriptGraphics2dBase Graph ?
C C Various graphics demonstrations:
C Shell Internal IG.Script.ScriptGraphics2dBase Graph CurvePlotLissajous 5 4
C Shell Internal IG.Script.ScriptGraphics2dBase Graph SinePlots 3 200
C Shell Internal IG.Script.ScriptGraphics2dBase Graph Decorations
C Shell Internal IG.Script.ScriptGraphics2dBase Graph CurveStylesWithSave %temp%/test.bmp
C 
C C 3D Plots:
C
C C Instructions: when graphic window appears, press the 'r' key if the plot is
C C not centered or zoomed correctly (before this, make the window active by 
C C clicking on it)!
C Other useful commands: 
C 'j'/'t' - jojstick / trackball mode, 'c'/'a' - camera / actor mode (in the
C latter, mouse events affect object under the pointer rather than camera),
C 's'/'w' - all actors represented as surfaces, 'f' - fly-to the point under
C the cursor (allowing rotations about that point), 'r' - resets the camera
C (centers actors, all become visible).
C
C C Check which tests are available:
C Shell Internal IG.Script.ScriptExtGraphics3d Plot3d ? 
C
C Shell Internal IG.Script.ScriptGraphics3DBase Plot3d CurvePlotLissajous 
C Shell Internal IG.Script.ScriptGraphics3DBase Plot3d SurfaceComparison 
C Shell Internal IG.Script.ScriptGraphics3DBase Plot3d SurfacePlot 
C Shell Internal IG.Script.ScriptGraphics3DBase Plot3d ContourPlot 
C Shell Internal IG.Script.ScriptGraphics3DBase Plot3d Decoration 
C  
C 
C C Some VTK tests:
C C Use the same instructions as for 3D Plots!
C 
C Shell Internal IG.Script.ScriptGraphics3DBase VtkTest StructuredGrid 20 20 3C Shell Internal 
C Shell Internal IG.Script.ScriptGraphics3DBase VtkTest QuadCells 20 20 
C Shell Internal IG.Script.ScriptGraphics3DBase VtkTest CellGridContours 20 20 20
C Shell Internal IG.Script.ScriptGraphics3DBase VtkTest StructuredGridVolumeContours 20 20 4
C
C C Checks what other commands are available in this nested application:
C 
C Shell Internal IG.Script.ScriptGraphics3DBase VtkTest ? 
C
CC





C C ***************************************************
C Guests' utilities:

C C ***************************************************
C C Ferdo's utilities

C C Print available commands:
C Ferdo ?

C C Main Ferdo's application: 
C Ferdo FerdoMain arg1 arg2 arg3 ...

C C The Ferdo's test application (to check that installed applications work properly):
C FerdoTest arg1 arg2 arg3 ...




