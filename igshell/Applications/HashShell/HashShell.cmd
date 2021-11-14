
C C This file contains example commands for the HashShell application.

C C Run the application (HashShell.exe) with commandline arguments
C C Copied from this file!
C C You can also uncomment some lines and interpret this file. In order
C C to do this, run the application with Run as the first argument and
C C name of tis file as the second argument, i.e.
C C 
C C   HashShell.exe Run HashShell.cmd
C C 
C C Command names are NOT CASE SENSITIVE.


WriteLine "When running this file, uncomment the lines that you want to execte!"



C C ****************************************************************************
C C CALCULATION OF HASH VALUES

C C Launch a form for calculation and verification of file and text
C C hash functions (checksums - MD5, SHA-1, SHA-256, SHA-512):

C HashForm


C C Calculate file hashes for the specified file, eventually write information
C C to a file:
C C GetFileHash FileName <WriteToFile>

C GetFileHash readme.txt false

C C CheckSum:
C C Calculate / verify hashes of files or strings by the CheckSum command:
C C CheckSum <-c> <-s string> <-h hash> <-t hashType> <-o outputFile> <inputFile1> <inputFile2> ...: 
C C   Calculates or verifies various types of hash values for files or strings. Calculated file hashes
C C   can be saved to a file.
C C     -t hashType: specifies hash type (MD5, SHA-1, SHA-256, SHA-512)
C C     -c: verification rather than calculation of hashes.
C C     -s: hash is calculated or verified for the specified string rather than file(s).
C C     -h hash: hash value to be verified.
C C     -o outputFile: output file where calculated hashes are written.
C C     inputFile1 inputFile2 ...: input files, either files whose hashes are calculated, or files
C C       containing hash values to be verified (in the case of -c option)


C C Calculate MD5 checksums of several files & store them to hashes.MD5:
C CheckSum -t MD5 -o hashes.MD5 app.cmd examples.cmd excrypto.cmd 

C C Verify previously calculated checksums:
C CheckSum -t MD5 -c hashes.MD5 

C C Calculate & verify other kind of checksum (e.g. SHA-1, SHA-256, SHA-512):
C CheckSum -t SHA-256 -o hashes.SHA256 app.cmd examples.cmd excrypto.cmd
C CheckSum -t SHA-256 -c hashes.SHA256

C C Calculate hash for a single file (without writing to a file): 
C CheckSum -t SHA-1 app.cmd 

C C Direct verification of specified hash value of a file
C CheckSum -t MD5 -c app.cmd -h 207fbc52a83a3f2538713e6e1f73697d

C C Calculation of hashes (of various types) of the string "My String":
C C MD5 = 4545102cc40ea0a85124cf4b31574661
C C SHA1 = 07841b2e0fda6cfbf7c6bf00f179233cf4e3247b
C C SHA256 = 8a7046a0b97e45470b13f30448c9d7d959aa5eea583d2f007921736b2141ac75
C C SHA512 = d159694d78c06886143e08dadc50cb89a96a41c766b603fa07fe3de91e170bf2942545c1ca17e280f572fc829de6059a80e75f4623e736915265f8938bb19e39
C CheckSum -t MD5 -s "My String"
C CheckSum -t SHA1 -s "My String"
C CheckSum -t SHA256 -s "My String"
C CheckSum -t SHA512 -s "My String"

C C Verification of a string hash value:
C CheckSum -t MD5 -c -s "My String" -h 4545102cc40ea0a85124cf4b31574661
C CheckSum -t SHA256 -c -s "My String" -h 8a7046a0b97e45470b13f30448c9d7d959aa5eea583d2f007921736b2141ac75


C C ****************************************************************************
C C OTHER CRYPTOGRAPHIC FUNCTIONS

C C All cryptographic functions currently available in the official version can
C C be launchet through the "Crypto" command. This includes the functions for
C C calculation of cryptographic hashes; therefore, instead of:

C CheckSum -t SHA1 -s "My String"

C C one can call:

C Crypto CheckSum -t SHA1 -s "My String"

C C The "Crypto" command provides access to a much larger set of crtyptographic
C C utilities, because not all of these utilities have direct commands.

C C For example, you can use functions for symmetric encryption and decryption:
C Crypto EncryptPlain -s -pw MyPassword11 -iv MyInitializationVector "Encrypted string No. 1"  "Encrypted string No. 2"
C Crypto DecryptPlain -s -pw MyPassword11 -iv MyInitializationVector  dggQxTn7y2m2tDw44rR/oU4ffveVClIsO/chAo5TT30=  dggQxTn7y2m2tDw44rR/oYfhlHfVt6j1Ug0f8UYI2fM=

C C Or you can use any of convenient auxiliary utilities, such as conversons:
C Crypto Convert Luxembourg London Ljubljana 

C C or password generation or checking generation times:
C Crypto GetKey -pwlen 14 -sllen 22 36 3200
C Internal IG.Script.AppBase Crypto TimeKeygeneration -pw MyPassword -sl MySalt111 -pwit 1000 

C C For countless other possibilities of using cryptographic utilities, see the
C C corresponding example file (.../examples/crypto/excrypto.cmd). When 
C C executing examples from this command file, the current directory should
C C be the directory that contains the command file.


C C ****************************************************************************
C C OTHER COMMANDS:


C C INTERACTIVE MODE:
C Interactive
C C Or:

C C SYSTEM COMMANDS:

C C Run a single system command:
C Sys dir

C C Run a shell where several system commands can be run:
C C To exit the shell, insert an empty line, then 1.
C Sys


C C CALCULATOR: 
C C State of the calculator is persistent, therefoere expressions (including variable definition)
C C can be evaluated in separate blocks.
C C To calculate a single expression:

C Calc 5*cos(const_pi)/3   // calculate an algebraic expression
C Calc function P(x, p) { return Math.pow(x, p); }   // definition of custom power functin 
C Calc P(2, 8)    // use oof function - 2 raised to power of 8
C Calc P(2, 10)   // use oof function - 2 raised to power of 10

C C To run an interactive shell where several expressions can be evaluated:

C Calc

C C In interactive mode, ? prints calculator help.
C C Example:
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




