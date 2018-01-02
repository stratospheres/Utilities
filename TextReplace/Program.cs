using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.IO;
using PowerArgs;
using System.Text;

namespace TextReplace
{
    class Program
    {
        static int Main(string[] args)
        {
            var parsedArgs = Args.Parse<Arguments>(args);

            try
            {
                ValidateArguments(parsedArgs);
                ValidateFilePath(parsedArgs.ReplacementsFile);
                ValidateFilePath(parsedArgs.InputFile);

                // validate input
                try
                {
                    Console.WriteLine("Processing...");

                    Console.WriteLine($"Replacements : {parsedArgs.ReplacementsFile}");
                    Console.WriteLine($"Input File   : {parsedArgs.InputFile}");

                    var inputFileData = "";

                    // Create an instance of StreamReader to read from a file.
                    // The using statement also closes the StreamReader.
                    using (StreamReader sr = new StreamReader(parsedArgs.InputFile))
                    {
                        inputFileData = sr.ReadToEnd();
                    }

                    using (StreamReader sr = new StreamReader(parsedArgs.ReplacementsFile))
                    {
                        using (CsvHelper.CsvReader csvReader = new CsvHelper.CsvReader(sr))
                        {
                            csvReader.Configuration.HasHeaderRecord = false;

                            while (csvReader.Read())
                            {
                                var replacement = csvReader.GetRecord<Replacement>();
                                Console.WriteLine($"From : {replacement.From}");
                                Console.WriteLine($"To   : {replacement.To}");

                                inputFileData =  
                                    (parsedArgs.CaseInsensitive) ?
                                    inputFileData.Replace(replacement.From, replacement.To, StringComparison.OrdinalIgnoreCase):
                                    inputFileData.Replace(replacement.From, replacement.To);
                            }

                            // write it back out...
                            System.IO.File.WriteAllText(parsedArgs.InputFile, inputFileData);

                            return 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                    return -1;
                }
 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught another error:");
                Console.WriteLine(ex.ToString());
                return -1;
            }

        }

        private static void ValidateArguments(Arguments args)
        {
            if (string.IsNullOrWhiteSpace(args.ReplacementsFile))
                throw new InvalidOperationException(
                    "Need a 'replace' CSV file to proceed.");
            if (string.IsNullOrWhiteSpace(args.InputFile))
                throw new InvalidOperationException(
                    "Need an 'input' file to process replacements against.");
        }

        public static void ValidateFilePath(string _filePath)
        {
            Console.WriteLine($"Validating path -> {_filePath}");

            if (!System.IO.File.Exists(_filePath))
                throw new ArgumentException($"File path {_filePath} does not exist.");
        }
    }
}
