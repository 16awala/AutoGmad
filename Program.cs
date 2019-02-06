using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmadAuto
{

    class Program
    {
        private static string[] gmaFiles;
        private static string[] exeFiles;
        private static string myDir;
        private static string outPath;

        static void Main(string[] args)
        {
            Console.WriteLine("Searching for files to extract...");

            myDir = Directory.GetCurrentDirectory();
            gmaFiles = Directory.GetFiles(myDir, "*.gma");
            outPath = myDir + "\\extracted\\";
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-out":
                        i++;
                        outPath = args[i];
                        break;
                    default:
                        break;
                }
            }

            if (gmaFiles.Length == 0)
            {
                Console.WriteLine("There are no gma files in the current directory (" + myDir + ").");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                exeFiles = Directory.GetFiles(myDir, "gmad.exe");
                if (exeFiles.Length == 0)
                {
                    Console.WriteLine("There is no gmad executable in the current directory (" + myDir + "\\gmad.exe).");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    if (gmaFiles.Length == 1)
                    {
                        Console.WriteLine("Found 1 gma file in current directory:\n");
                    }
                    else
                    {
                        Console.WriteLine("Found {0} gma files in current directory:\n", gmaFiles.Length);
                    }

                    for (int i = 0; i < gmaFiles.Length; i++)
                    {
                        Console.WriteLine("     " + Path.GetFileName(gmaFiles[i]));
                    }

                    Console.WriteLine("Extracting to " + outPath + ".");
                    Console.WriteLine("\nContinue? (y/n)");
                    bool bad = true;
                    while (bad)
                    {
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.KeyChar == 'y' || key.KeyChar == 'Y')
                        {
                            bad = false;
                            Console.WriteLine("Starting Extraction process...");

                            extract(0);

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                        }
                        else
                        {
                            if (key.KeyChar == 'n' || key.KeyChar == 'N')
                            {
                                bad = false;
                                Console.WriteLine("Exit without extracting.");
                            }
                        }
                    }
                }
            }
        }

        public static void extract(int number)
        {
            if (number < gmaFiles.Length)
            {
                Console.WriteLine("[File " + (number+1) + " of "+ gmaFiles.Length +" ] " + Path.GetFileName(gmaFiles[number]));
                System.Diagnostics.Process execute = new System.Diagnostics.Process();
                execute.StartInfo.FileName = myDir + "\\gmad.exe";
                execute.StartInfo.Arguments = "extract -file \"" + gmaFiles[number] + "\" -out \"" + outPath;
                execute.EnableRaisingEvents = true;
                execute.Exited += (sender, e) => { extract(number + 1); };
                execute.Start();
            }
            else
            {
                Console.WriteLine("File list exhausted.");
                return;
            }
        }
    }
}
