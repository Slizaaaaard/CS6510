using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CS6510_VirtualMachine_SJB.Memory;

namespace CS6510_VirtualMachine_SJB
{
    public static class Shell
    {
        static bool shell;
        static bool scheduleConflict;
        static string shellString;
        static int timeIn = 0;
        static int pidTemp;
        static List<string> listString = new List<string>();
        static List<int> times = new List<int>();

        internal static Load Load
        {
            get => default;
            set
            {
            }
        }

        public static Execute Execute
        {
            get => default;
            set
            {
            }
        }

        internal static OperatingSystem OperatingSystem
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
                    pidTemp = VM.fp.readyQueue.FirstOrDefault(x => x.Value.programFileName == shellString && x.Value.processState != (int)ProcessStateEnum.terminated).Key;
                    CoreDump.coreDump(VM, pidTemp);
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
                    listString = inputs.OfType<string>().ToList();
             
                    bool ready = true;
                    int tem = 0;
                    for (int i = 0; i < listString.Count; i++)
                    {
                        if (inputs.Length % 2 == 0) {
                            ready = false;
                                if (int.TryParse(listString[i], out timeIn) == true)
                                {

                                foreach (int entry in times)
                                {
                                    if (entry == timeIn)
                                    {
                                        Console.WriteLine("Process already scheduled for that time");
                                        scheduleConflict = true;
                                    }
                                }
                                listString.Remove(listString[i]);
                                times.Add(timeIn);
                                ready = true;
                                }     
                        }
                        else
                        {
                            timeIn = VM.clock++;
                            times.Add(timeIn);
                        }


                        if (ready == true && scheduleConflict == false)
                        {
                            Console.WriteLine($"\nLoad Program {listString[tem]}");
                            Console.WriteLine($"Time in {timeIn} \n");
                            Load.loadProgram(VM, listString[tem]);
                            tem++;
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
                        Console.WriteLine($"ERROR DUMP {shellString} {VM.errors}");
                    }

                }
            }

            if (shellString.Contains("setRR"))
            {
                string[] inputs;
                if (shellString.Contains("quantum1"))
                {
                    inputs = shellString.Split(' ');
                    int.TryParse(inputs[3], out VM.scheduler.quantum1);
                    Console.WriteLine($"Quantum 1 set to {VM.scheduler.quantum1}");
                
                }
                if (shellString.Contains("quantum2"))
                {
                    inputs = shellString.Split(' ');
                    int.TryParse(inputs[3], out VM.scheduler.quantum2);
                    Console.WriteLine($"Quantum 2 set to {VM.scheduler.quantum2}");
                }


            }

            if (shellString.Contains("setsched"))
            {
                string[] inputs;
                shellString.ToLower();
                if (shellString.Contains("rr1"))
                {
                    inputs = shellString.Split(' ');
                    VM.scheduler.setSched = inputs[3];
                }
                else if (shellString.Contains("fcfs"))
                {
                    inputs = shellString.Split(' ');
                    VM.scheduler.setSched = inputs[3];
                }
                else if(shellString.Contains("mfq"))
                {
                    inputs = shellString.Split(' ');
                    VM.scheduler.setSched = inputs[3];
                }
                Console.WriteLine($"Scheduler set to {VM.scheduler.setSched}");
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

  