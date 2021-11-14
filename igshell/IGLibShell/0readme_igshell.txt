
This is an IGLib official shell application.

Default directory:  ../../../../../workspaceprojects/00tests/examples  
               or: ../../../../../workspaceprojects/00tests/runscript

C Example command-line arguments:

C Calculator, within interactive mode:
Interactive Calc

C StopWatch: 
Interactive Internal IG.Script.AppExtBase FormDemo StopWatch

C 2D parametric plot (Lissajous curves)
Interactive Internal IG.Script.ScriptGraphics2dBase Graph CurvePlotLissajous 5 4

C Executing a system command ("dir" in this case):
Interactive Sys dir

C Launch file, text, or all files in a directory hash calculation forms:
Internal IG.Script.AppExtBase Crypto HashForm
Internal IG.Script.AppExtBase Crypto HashDirForm
Internal IG.Script.AppExtBase Crypto HashAllForm

C Run cryptographic utility - calculation of file hashes:
Internal IG.Script.AppBase Crypto CheckSum -t MD5 -o hashes.MD5 app.cmd examples.cmd excrypto.cmd 

TESTING CRYPTO:
Default Directory ../../../../../../../../workspaceprojects/00tests/examples/crypto
Arguments:
    interactive Internal IG.Script.AppBase Crypto AsymTest

TESTING SCRIPTING & INTERPRETERS:
Default directory: ../../../../../../../../workspaceprojects/00tests/runscript
Arguments: 
    Interactive Run RunAppBase.cmd
  

Remarks:
  Old build directory: ..\..\..\..\output\bin\AnyCPU\Debug\
  New build directory: bin\Debug\