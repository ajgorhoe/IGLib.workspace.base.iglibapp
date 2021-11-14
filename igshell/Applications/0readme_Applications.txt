
This directory contains source files of other IGLib-based applications which
are "packed" in the current project for easier maintenance of their code.

REMARKS ON BUILD ISSUES:
Sometimes build fails (e.g. for Guest_IGShell). HashForm usually
builds without any issues.
In this case, try one or several of the following:
  * Remove the output obj/ and bin/ directories
  * In project file, comment the <OutputPath> element, such that
    default base path bin/ inside project directory is used.
  * sometimes, simply running the build several times or building
    alternately with some other application (usually HashForm) in 
	this directory helps.



====
Warning: text below is outdated and needs cleaning!


In this way, these external applications do not need their project in the 
source repository, but they can have a project (e.g. to enable users extend
the applications) in the deployed directory.

The current project acts as host project for such applications. Typically,
there is an option to run the whole application for the host project (e.g.
via special command-line options or via shell interpreter), which makes 
comfortable debugging possible.


LIST OF HOSTED EXTERNAL APPLICATIONS:

**** directory HashForm:
  GUI-based Application for calculation and verification of file hashes;
Files:
  ProgramHashGui.cs 
      - main program (just a class of the same name, Main() method and call to 
	  MainHashGui()), not included in host project. Class is partial.
  SharedProgramHashGui:
      - contains other part of the ProgramHashGui class containing the 
	  MainHashGui() method. Also contains some other classes for application.
	  More things are put to one file, such that hosted application (together
	  with its external project in the deploy directory) is easy to maintain.

**** directory HashShell:
  Shell (command-line) application for calculation and verification of file hashes;
Files:
  ProgramHashShell.cs 
      - main program (just a class of the same name, Main() method and call to 
	  MainHashShell()), not included in host project. Class is partial.
  SharedProgramHashShell:
      - contains other part of the ProgramHashShell class containing the 
	  MainHashShell() method. Also contains some other classes for application.
	  More things are put to one file, such that hosted application (together
	  with its external project in the deploy directory) is easy to maintain.

