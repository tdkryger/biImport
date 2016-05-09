using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BIDataImport
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.WriteLine("Running imports");


            if (execute("BIImportCategories.exe"))
            {
                if (execute("BIImportCustomers.exe"))
                {
                    if (execute("BIImportProducts.exe"))
                    {
                        if (execute("BIImportSuppliers.exe"))
                        {
                            if (execute("BIImportZipCodes.exe"))
                            {
                                if (execute("BIImportOrderData.exe"))
                                {
                                    Console.WriteLine("All imports ran without errors");
                                    clShared.SharedInfo.handleLogging("BIDataImport", "All imports ran without errors", clShared.SharedInfo.LogTypes.Info);
                                }
                            }
                        }
                    }
                }
            }
           Console.WriteLine("Done");            
        }

        static bool execute(string programName)
        {
            clShared.SharedInfo.handleLogging("BIDataImport", "Running " + programName, clShared.SharedInfo.LogTypes.Info);
            Console.WriteLine("Running " + programName);

            clShared.SharedInfo.ExitCodes returnValue = executeAProgram(programName);
            switch(returnValue )
            {
                case clShared.SharedInfo.ExitCodes.FileNotFound:
                    Console.WriteLine("FileNotFound " + programName);
                    return false;
                case clShared.SharedInfo.ExitCodes.InsertError:
                    Console.WriteLine("InsertError " + programName);
                    return false;
                case clShared.SharedInfo.ExitCodes.InvalidData:
                    Console.WriteLine("InvalidData " + programName);
                    return false;
                case clShared.SharedInfo.ExitCodes.Ok:
                    Console.WriteLine("OK " + programName);
                    return true;
                case clShared.SharedInfo.ExitCodes.PKConversionError:
                    Console.WriteLine("PKConversionError " + programName);
                    return false;
                default:
                    Console.WriteLine("Unknown exitcode " + programName);
                    return false;
            }


        }

        static clShared.SharedInfo.ExitCodes executeAProgram(string programName)
        {
            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "";// arguments;
            // Enter the executable to run, including the complete path
            start.FileName = programName;
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;


            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
            return (clShared.SharedInfo.ExitCodes)exitCode;
        }

        

        
        

        

       

        
    }
}
