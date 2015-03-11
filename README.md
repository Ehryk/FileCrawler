FileCrawler v1.2
================

This Windows console application will enumerate the contents of a directory and can perform various tasks with the output, including displaying in the console window, loading into a database, saving into an .xml or .json file, and can be extended into more.

Usage:
---
 - ``FileCrawler``, ``FileCrawler.exe``, ``FileCrawler -h``
   - Displays help screen
 - ``FileCrawler "Path"``
   - Crawls the given path (file or directory)
 - Options:
   - ``-h``, ``--help``: Displays help screen

Latest Changes:
---
 - Initial Release (2015.03.01)

Release History:
---
 - v1.2 2015.03.10 Supporting crawling individual files and additional output options (quiet, verbose)
 - v1.1 2015.03.02 Additional features: Container Enumeration with 7zip, Reporting Inaccessible Files and Directories
 - v1.0 2015.03.01 This release is a functional parser enumerate a directory with little other functionality

Author:
 - Eric Menze ([@Ehryk42](https://twitter.com/Ehryk42))

Build Requirements:
---
 - Visual Studio (Built with Visual Studio 2013)
 - NuGet (packages should restore)
   - [SevenZipSharp](https://www.nuget.org/packages/SevenZipSharp/) - Managed wrappers around 7z.dll for extraction
   - [CommandLineParser](https://www.nuget.org/packages/CommandLineParser/) - Command Line Parsing
   - [Fody](https://www.nuget.org/packages/Fody/) - .NET Assembly Weaving
   - [Costura.Fody](https://www.nuget.org/packages/Costura.Fody/) - Making the .exe a standalone executable
   - [log4net](https://www.nuget.org/packages/log4net/) - Centralized logging

Contact:
---
Eric Menze
 - [Email Me](mailto:rhaistlin+gh@gmail.com)
 - [www.ericmenze.com](http://ericmenze.com)
 - [Github](https://github.com/Ehryk)
 - [Twitter](https://twitter.com/Ehryk42)
 - [Source Code](https://github.com/Ehryk/sde2string)
