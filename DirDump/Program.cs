using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerArgs;

namespace DirDump
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Arguments parsed = Args.Parse<Arguments>(args);

                // null/empty means current working directory
                if (string.IsNullOrWhiteSpace(parsed.RootDirectory))
                {
                    parsed.RootDirectory = System.IO.Directory.GetCurrentDirectory();
                    DumpDirectory(parsed.RootDirectory, parsed.RootDirectory, 0);
                }
                else
                {
                    // specified a specific directory, but we should make sure that it's valid...
                    if (System.IO.Directory.Exists(parsed.RootDirectory))
                    {
                        // not the current directory, but we can proceed
                        DumpDirectory(parsed.RootDirectory, parsed.RootDirectory, 0);
                    }
                    else
                    {
                        Console.WriteLine(parsed.RootDirectory + " doesn't appear to be a valid directory.");
                        Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<Arguments>());
                    }
                } 
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<Arguments>());
            }
            finally
            {
                Console.ReadLine();
            }
        }
        
        static void DumpDirectory(string rootDirectory, string thisDirectory, int nestingLevel)
        {
            // dump the name of this directory...
            string directoryNameToPrint;
            if (rootDirectory == thisDirectory)
                directoryNameToPrint = thisDirectory;
            else
                // don't repeat what we've already said
                directoryNameToPrint = thisDirectory.Substring(rootDirectory.Length);

            Console.WriteLine(BuildLeftPaddingForSubDirectories(nestingLevel) + directoryNameToPrint);
            
            // loop through the files, dump
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(thisDirectory);
            foreach (string file in files)
            {
                Console.WriteLine(BuildLeftPaddingForFiles(nestingLevel) + file.Substring(thisDirectory.Length + 1));
            }

            // loop through the directories, recurse
            IEnumerable<string> subDirectories = System.IO.Directory.EnumerateDirectories(thisDirectory);
            foreach (string subDirectory in subDirectories)
            {
                DumpDirectory(thisDirectory, subDirectory, nestingLevel + 1);
            }
        }

        static string BuildLeftPaddingForFiles(int nestingLevel)
        {
            return new string(' ', nestingLevel * 3);
        }

        static string BuildLeftPaddingForSubDirectories(int nestingLevel)
        {
            int charLen = nestingLevel * 3;
            if (charLen == 0)
                return "";
            else
                return "+" + new string('-', (nestingLevel * 3) - 2) + "+";
        }

    }
}
