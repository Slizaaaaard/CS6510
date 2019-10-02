using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace CS6510_VirtualMachine_SJB
{
    public static class Shell
    {
        static bool shell;
        static bool scheduleConflict;
        static string shellString;
        static int timeIn = 0;
        static int pidTemp;

        internal static Kernel Kernel
        {
            get => default;
            set
            {
            }
        }

        public static bool shellCommand(VirtualMachine VM)
        {
            if (shell == true)
            {
                Console.Write("MYVM->");
            }
            else
            {
                Console.Write("VM->");
            }
            shellString = Console.ReadLine();
            shellString = shellString.ToLower();
            if (shellString == "myvm")
            {
                shell = true;
            }
            if (shellString == "vm")
            {
                shell = false;
            }
    

            if (shellString.Contains("load"))
            {

                if (shellString.Contains("-v"))
                {
                    shellString = shellString.Remove(0, 4);
                    shellString = shellString.Trim();
                    shellString = shellString.Remove(0, 2);
                    shellString = shellString.Trim();
                    Load.loadProgram(VM, shellString);
                }

            }

            if (shellString.Contains("run"))
            {
                if (shellString.Contains("-v"))
                {
                    shellString = shellString.Remove(0, 3);
                    shellString = shellString.Trim();
                    shellString = shellString.Remove(0, 2);
                    shellString = shellString.Trim();
                    Console.WriteLine($"Execute Program {shellString}");
                    pidTemp = VM.fp.readyQueue.FirstOrDefault(x => x.Value.programFileName == shellString).Key;
                    Execute.executeProgram(VM, pidTemp);
                }
            }

            if (shellString.Contains("coredump"))
            {
                if (shellString.Contains("-v"))
                {
                    shellString = shellString.Remove(0, 8);
                    shellString = shellString.Trim();
                    shellString = shellString.Remove(0, 2);
                    shellString = shellString.Trim();
                    Console.WriteLine($"CORE DUMP {shellString}");
                    coredump.coreDump(VM);
                }
                else
                {
                    for (int i = 100; i < VM.loaderAddress; i += 6)
                    {
                        Console.WriteLine(VM.MEM[i]);
                    }
                }
            }

            if (shellString.Contains("execute"))
            {
                if (shellString.Contains("-v"))
                {
                    string[] inputs = shellString.Split(" ");
                    ////Removes Execute
                    //shellString = shellString.Remove(0, 7);
                    //shellString = shellString.Trim();
                    ////Removes Verbose
                    //shellString = shellString.Remove(0, 2);
                    //shellString = shellString.Trim();
                    //// check for space here
                    //if (shellString.IndexOf(' ') != -1)
                    //{
                    //    // separates time in value from file name
                    //    int.TryParse(shellString.Substring(shellString.IndexOf(' ') + 1), out timeIn);
                    //    int i = (shellString.IndexOf(' ') + 1);
                    //    shellString = shellString.Substring(0, i);
                    //}
                    inputs = inputs.Skip(2).ToArray();
                    for(int i = 0; i < inputs.Length; i++)
                    {
                        if (inputs.Length % 2 == 0)
                        {
                            foreach (string input in inputs)
                            {
                                if (int.TryParse(input, out timeIn) == false)
                                {
                                    timeIn = VM.clock++;
                                    inputs = inputs.Skip(0).ToArray();
                                }     
                            }
                        }
                        else
                        {
                            timeIn = VM.clock++;
                        }


                        foreach (KeyValuePair<int, ProcessControlBlock> entry in VM.fp.newQueue)
                        {
                            if (entry.Value.startPC == timeIn)
                            {
                                Console.WriteLine("Process already scheduled for that time");
                                scheduleConflict = true;
                            }
                        }

                        Console.WriteLine($"\nLoad Program {inputs[i]}");
                        Load.loadProgram(VM, inputs[i]);
                    }

                    foreach (string input in inputs)
                    {
                        if (scheduleConflict == false)
                        {
           
                            Console.WriteLine($"\nExecute Program {input}");
                            Console.WriteLine($"Time in {timeIn}");
                            pidTemp = VM.fp.readyQueue.FirstOrDefault(x => x.Value.programFileName == input).Key;
                            .executeProgram(VM, pidTemp);
                        }
                    }

                    scheduleConflict = false;
       
                }
            }

            if (shellString.Contains("errordump"))
            {
                if (shellString.Contains("-v"))
                {
                    shellString = shellString.Remove(0, 9);
                    shellString = shellString.Trim();
                    shellString = shellString.Remove(0, 2);
                    shellString = shellString.Trim();
                    if (MemoryManagement.errors == "")
                    {
                        Console.WriteLine($"ERROR DUMP {shellString} No Errors");
                    }
                    else
                    {
                        Console.WriteLine($"ERROR DUMP {shellString} {MemoryManagement.errors}");
                    }

                }
            }

            VM.clock++;

            if (shellString == "stop" || shellString == "quit" || shellString == "exit")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}

  