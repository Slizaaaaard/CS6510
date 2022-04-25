using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CS6510_VirtualMachine_SJB.Memory;

namespace CS6510_VirtualMachine_SJB
{
    public static class OperatingSystem1
    {
        static bool shell;
        static bool scheduleConflict;
        static string shellString;
        static int timeIn = 0;
        static int totalWait = 0;
        static int turnAround = 0;
        static int response = 0;
        static int pidTemp;
        static List<string> listString = new List<string>();
        static List<int> times = new List<int>();
        static SharedMemory sm = new SharedMemory();
        static Semaphore sem = new Semaphore();
    

        public static VirtualMachine VirtualMachine
        {
            get => default;
            set
            {
            }
        }

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

        public static Semaphore Semaphore
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
                    string[] inputs = shellString.Split(" ");
                    inputs = inputs.Skip(1).ToArray();
                    foreach (string input in inputs){
                    Load.loadProgram(VM, input);
                    }
            }

            if (shellString.Contains("run"))
            {
                string[] inputs = shellString.Split(" ");
                inputs = inputs.Skip(1).ToArray();
                foreach (string input in inputs)
                {
                    Console.WriteLine($"Execute Program {input}");
                    pidTemp = VM.fp.readyQueue.FirstOrDefault(x => x.Value.programFileName == input).Key;
                    Execute.executeProgram(VM, pidTemp);
                }
            }
            if (shellString.Contains("setpagesize"))
            {
                string[] inputs = shellString.Split(" ");
                inputs = inputs.Skip(1).ToArray();
                VM.page.setPageSize(int.Parse(inputs[0]));
            }
            if (shellString.Contains("getpagesize"))
            {
                Console.WriteLine($"Page size is {VM.page.getPageSize()}");
            }
            if (shellString.Contains("setpagenumber"))
            {
                string[] inputs = shellString.Split(" ");
                inputs = inputs.Skip(1).ToArray();
                VM.page.setPageNumber(int.Parse(inputs[0]));
            }

            if (shellString.Contains("ps"))
            {
                if (shellString.Contains("-free"))
                {
                    VM.page.free();
                }

                if (shellString.Contains("-proc"))
                {

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
             


                string[] inputs = shellString.Split(" ");
                shellString = "";
                inputs = inputs.Skip(1).ToArray();
                listString = inputs.OfType<string>().ToList();
                int tem = 0;
                for (int i = 0; i < 2; i++)
                {
                    timeIn = VM.clock++;
                    times.Add(timeIn);
                    Console.WriteLine($"Load Program {listString[tem]}");
                    Load.loadProgram(VM, listString[tem]);
                    tem++;
                }
              
                inputs = inputs.Skip(4).ToArray();
                int size;
                int.TryParse(inputs[2], out size);
                sm.shmOpen(inputs[0], inputs[1], size);

                inputs = inputs.Skip(5).ToArray();
                sem.semInit(inputs[0]);

                VM.priorityQueue.queue0 = VM.fp.readyQueue;
                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.priorityQueue.queue0)
                {
                    process.Value.ghant = process.Value.PID.ToString();
                }
                while (VM.priorityQueue.queue0.Count() != 0)
                {
                    VM.scheduler.RR(VM, sem, sm);
                }

                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.fp.terminatedQueue)
                {
                    Console.WriteLine(process.Value.ghant);
                }

            }

            //if (shellString.Contains("execute"))
            //{
            //    //if (shellString.Contains("-v"))
            //    //{

            //    string[] inputs = shellString.Split(" ");
            //    ////Removes Execute
            //    //shellString = shellString.Remove(0, 7);
            //    //shellString = shellString.Trim();
            //    ////Removes Verbose
            //    //shellString = shellString.Remove(0, 2);
            //    //shellString = shellString.Trim();
            //    //// check for space here
            //    //if (shellString.IndexOf(' ') != -1)
            //    //{
            //    //    // separates time in value from file name
            //    //    int.TryParse(shellString.Substring(shellString.IndexOf(' ') + 1), out timeIn);
            //    //    int i = (shellString.IndexOf(' ') + 1);
            //    //    shellString = shellString.Substring(0, i);
            //    //}




            //    inputs = inputs.Skip(1).ToArray();
            //    listString = inputs.OfType<string>().ToList();

            //    bool ready = true;
            //    int tem = 0;
            //    for (int i = 0; i < listString.Count; i++)
            //    {


            //        //if (inputs.Length % 2 == 0) {
            //        //    ready = false;
            //        //        if (int.TryParse(listString[i], out timeIn) == true)
            //        //        {

            //        //        foreach (int entry in times)
            //        //        {
            //        //            if (entry == timeIn)
            //        //            {
            //        //                Console.WriteLine("Process already scheduled for that time");
            //        //                scheduleConflict = true;
            //        //            }
            //        //        }
            //        //        listString.Remove(listString[i]);
            //        //        times.Add(timeIn);
            //        //        ready = true;
            //        //        }     
            //        //}
            //        //else
            //        //{
            //        timeIn = VM.clock++;
            //        times.Add(timeIn);
            //        //}


            //        if (ready == true && scheduleConflict == false)
            //        {
            //            Console.WriteLine($"Load Program {listString[tem]}");
            //            Console.WriteLine($"Time in {timeIn}");
            //            Load.loadProgram(VM, listString[tem]);
            //            tem++;
            //        }

            //    }

            //    if (scheduleConflict == false)
            //    {
            //        int round = 0;
            //        switch (VM.scheduler.setSched)
            //        {
            //            case "rr1":
            //                Console.WriteLine("Round Robin 1");
            //                VM.priorityQueue.queue0 = VM.fp.readyQueue;
            //                while (VM.priorityQueue.queue0.Count() != 0)
            //                {
            //                    round++;
            //                    Console.WriteLine($"Round {round}");
            //                    VM.scheduler.RR1(VM);
            //                }
            //                break;
            //            case "rr2":
            //                Console.WriteLine("Round Robin 2");
            //                VM.priorityQueue.queue1 = VM.fp.readyQueue;
            //                while (VM.priorityQueue.queue1.Count() != 0)
            //                {
            //                    round++;
            //                    Console.WriteLine($"Round {round}");
            //                    VM.scheduler.RR2(VM);
            //                }
            //                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.fp.terminatedQueue)
            //                {
            //                    totalWait = totalWait + process.Value.waitTime;
            //                    turnAround = turnAround + (process.Value.timeOut - process.Value.timeIn);
            //                    response = response + (process.Value.response - process.Value.timeIn);
            //                }
            //                totalWait = totalWait / VM.fp.terminatedQueue.Count();
            //                turnAround = turnAround / VM.fp.terminatedQueue.Count();
            //                response = response / VM.fp.terminatedQueue.Count();

            //                Console.WriteLine($"Average Wait {totalWait}");
            //                Console.WriteLine($"Average Turn Around {turnAround}");
            //                Console.WriteLine($"Throughput {(double)VM.fp.terminatedQueue.Count() / (double)VM.clock}");
            //                Console.WriteLine($"Average Response {response}");
            //                break;

            //            case "fcfs":
            //                Console.WriteLine("FCFS");
            //                VM.priorityQueue.queue2 = VM.fp.readyQueue;
            //                while (VM.priorityQueue.queue2.Count() != 0)
            //                {

            //                    VM.scheduler.FCFS(listString, VM);
            //                }

            //                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.fp.terminatedQueue)
            //                {
            //                    totalWait = totalWait + process.Value.waitTime;
            //                    turnAround = turnAround + (process.Value.timeOut - process.Value.timeIn);
            //                    response = response + (process.Value.response - process.Value.timeIn);
            //                }
            //                totalWait = totalWait / VM.fp.terminatedQueue.Count();
            //                turnAround = turnAround / VM.fp.terminatedQueue.Count();
            //                response = response / VM.fp.terminatedQueue.Count();

            //                Console.WriteLine($"Average Wait {totalWait}");
            //                Console.WriteLine($"Average Turn Around {turnAround}");
            //                Console.WriteLine($"Throughput {(double)VM.fp.terminatedQueue.Count() / (double)VM.clock}");
            //                Console.WriteLine($"Average Response {response}");
            //                break;

            //            case "mfq":
            //                Console.WriteLine("MFQ");
            //                VM.priorityQueue.queue0 = VM.fp.readyQueue;
            //                while (VM.priorityQueue.queue1.Count() != 0 || VM.priorityQueue.queue0.Count() != 0)
            //                {
            //                    round++;
            //                    if (VM.priorityQueue.queue1.Count() != 0)
            //                    {
            //                        Console.WriteLine($"RR2 Round {round}");
            //                        VM.scheduler.RR2(VM);
            //                    }
            //                    if (VM.priorityQueue.queue0.Count() != 0)
            //                    {
            //                        Console.WriteLine($"RR1 Round {round}");
            //                        VM.scheduler.RR1(VM);

            //                    }
            //                }
            //                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.fp.terminatedQueue)
            //                {
            //                    totalWait = totalWait + process.Value.waitTime;
            //                    turnAround = turnAround + (process.Value.timeOut - process.Value.timeIn);
            //                    if (process.Value.response != 0)
            //                    {
            //                        response = response + (process.Value.response - process.Value.timeIn);
            //                    }
            //                }
            //                totalWait = totalWait / VM.fp.terminatedQueue.Count();
            //                turnAround = turnAround / VM.fp.terminatedQueue.Count();
            //                response = response / VM.fp.terminatedQueue.Count();

            //                Console.WriteLine($"Average Wait {totalWait}");
            //                Console.WriteLine($"Average Turn Around {turnAround}");
            //                Console.WriteLine($"Throughput {(double)VM.fp.terminatedQueue.Count() / (double)VM.clock}");
            //                Console.WriteLine($"Average Response {response}");
            //                break;
            //            case "":
            //                Console.WriteLine("Default to Round Robin");

            //                VM.priorityQueue.queue0 = VM.fp.readyQueue;
            //                while (VM.priorityQueue.queue0.Count() != 0)
            //                {
            //                    round++;
            //                    Console.WriteLine($"Round {round}");
            //                    VM.scheduler.RR1(VM);

            //                }

            //                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.fp.terminatedQueue)
            //                {
            //                    totalWait = totalWait + process.Value.waitTime;
            //                    turnAround = turnAround + (process.Value.timeOut - process.Value.timeIn);
            //                    if (process.Value.response != 0)
            //                    {
            //                        response = response + (process.Value.response - process.Value.timeIn);
            //                    }

            //                }
            //                if (VM.fp.terminatedQueue.Count() != 0)
            //                {
            //                    totalWait = totalWait / VM.fp.terminatedQueue.Count();
            //                    turnAround = turnAround / VM.fp.terminatedQueue.Count();
            //                    response = response / VM.fp.terminatedQueue.Count();
            //                }


            //                Console.WriteLine($"Average Wait {totalWait}");
            //                Console.WriteLine($"Average Turn Around {turnAround}");
            //                Console.WriteLine($"Throughput {(double)VM.fp.terminatedQueue.Count() / (double)VM.clock}");
            //                Console.WriteLine($"Average Response {response}");
            //                break;
            //        }
            //    }

            //    scheduleConflict = false;


            //}



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

            if (shellString.Contains("setrr"))
            {
                string[] inputs;
                if (shellString.Contains("quantum1"))
                {
                    inputs = shellString.Split(' ');
                    int.TryParse(inputs[2], out VM.scheduler.quantum1);
                    Console.WriteLine($"Quantum 1 set to {VM.scheduler.quantum1}");
                
                }
                if (shellString.Contains("quantum2"))
                {
                    inputs = shellString.Split(' ');
                    int.TryParse(inputs[2], out VM.scheduler.quantum2);
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
                    VM.scheduler.setSched = inputs[1];
                }
                if (shellString.Contains("rr2"))
                {
                    inputs = shellString.Split(' ');
                    VM.scheduler.setSched = inputs[1];
                }
                else if (shellString.Contains("fcfs"))
                {
                    inputs = shellString.Split(' ');
                    VM.scheduler.setSched = inputs[1];
                }
                else if(shellString.Contains("mfq"))
                {
                    inputs = shellString.Split(' ');
                    VM.scheduler.setSched = inputs[1];
                }
                Console.WriteLine($"Scheduler set to {VM.scheduler.setSched}");
            }

            if (shellString.Contains("shm"))
            {
                string[] inputs = shellString.Split(" ");
                inputs = inputs.Skip(2).ToArray();
                listString = inputs.OfType<string>().ToList();

                if (shellString.Contains("open"))
                {
                    int size;
                    int.TryParse(listString[2], out size);
                    sm.shmOpen(listString[0], listString[1], size);

                }
                if (shellString.Contains("unlink"))
                {
                  
                    sm.shmUnlink(listString[0]);
                }
            }

            if (shellString.Contains("sem"))
            {
                string[] inputs = shellString.Split(" ");
                inputs = inputs.Skip(2).ToArray();
                listString = inputs.OfType<string>().ToList();

                if (shellString.Contains("init"))
                {
                    sem.semInit(listString[0]);
                    Console.WriteLine($"Semaphore {listString[0]} Created");
                }
                if (shellString.Contains("wait"))
                {
                    sem.semWait(listString[0]);
                }
                if (shellString.Contains("signal"))
                {
                    sem.semSignal(listString[0]);
                }
                if (shellString.Contains("busy waiting"))
                {
                    Console.WriteLine(listString[0]);
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

  