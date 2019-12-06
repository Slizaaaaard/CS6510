using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CS6510_VirtualMachine_SJB.Memory;

namespace CS6510_VirtualMachine_SJB
{
    public class Scheduler
    {
        public string setSched = "";
        public int quantum1;
        public int quantum2;
        const int BLOCK = 6;
        void setSchedule()
        {

        }

        public Scheduler()
        {
            quantum1 = 9999;
            quantum2 = 20;
        }

  
        public void FCFS(List<string> listString, VirtualMachine VM)
        {
   
            int pidTemp;
            foreach (string input in listString)
            {
                    Console.WriteLine($"\nExecute Program {input}");
                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.priorityQueue.queue2)
                {
                    Console.Write(process.Value.PID);
                    if (process.Value.processState != (int)ProcessStateEnum.ready)
                    {
                        process.Value.processState = (int)ProcessStateEnum.waiting;
                    }
                }
                pidTemp = VM.fp.readyQueue.FirstOrDefault(x => x.Value.programFileName == input && x.Value.processState != (int)ProcessStateEnum.terminated).Key;
                Execute.executeProgram(VM, pidTemp);
                VM.priorityQueue.queue2[pidTemp].processState = (int)ProcessStateEnum.terminated;

                ProcessControlBlock terminateProcess = VM.priorityQueue.queue0.FirstOrDefault(x => x.Key == pidTemp).Value;
                //VM.fp.terminatedQueue.Add(pidTemp, terminateProcess);
                VM.priorityQueue.queue2[pidTemp].timeOut = VM.clock;
                VM.priorityQueue.removeQueue2(pidTemp);

            }
        }

        public void RR(VirtualMachine VM, Semaphore sem, SharedMemory SM)
        {
            List<int> toBeDeleted = new List<int>();
            List<int> toBePromoted = new List<int>();



            foreach (KeyValuePair<int, ProcessControlBlock> process in VM.priorityQueue.queue0)
            {

                if (process.Value.processState != (int)ProcessStateEnum.ready)
                {
                    process.Value.processState = (int)ProcessStateEnum.waiting;
                }

            }

            foreach (KeyValuePair<int, ProcessControlBlock> runningProcess in VM.priorityQueue.queue0)
            {
                runningProcess.Value.processState = (int)ProcessStateEnum.running;
                runningProcess.Value.endSection = runningProcess.Value.startSection + (quantum1 * BLOCK);
                if (runningProcess.Value.endSection > runningProcess.Value.endPC)
                {
                    VM.priorityQueue.queue0[runningProcess.Key].endSection = runningProcess.Value.endPC;
                }
                Execute.executePartial(VM, SM, sem, runningProcess.Key, VM.priorityQueue.queue0);
  //              runningProcess.Value.startSection = runningProcess.Value.endSection;


                if (VM.priorityQueue.queue0[runningProcess.Key].endSection == runningProcess.Value.endPC)
                {
                    VM.priorityQueue.queue0[runningProcess.Key].processState = (int)ProcessStateEnum.terminated;
                    toBeDeleted.Add(runningProcess.Key);
                }
                if (VM.scheduler.setSched == "mfq" && VM.priorityQueue.queue0[runningProcess.Key].processState != (int)ProcessStateEnum.terminated)
                {
                    if (VM.priorityQueue.queue0[runningProcess.Key].processState == (int)ProcessStateEnum.running)
                    {
                        VM.priorityQueue.queue0[runningProcess.Key].round++;
                    }

                    if (VM.priorityQueue.queue0[runningProcess.Key].round == 3)
                    {
                        VM.priorityQueue.queue0[runningProcess.Key].round = 0;
                        toBePromoted.Add(runningProcess.Key);
                    }
                }
                if (VM.priorityQueue.queue0[runningProcess.Key].processState != (int)ProcessStateEnum.terminated)
                {
                    runningProcess.Value.processState = (int)ProcessStateEnum.waiting;
                }


            }
            foreach (int id in toBeDeleted)
            {
                ProcessControlBlock process = VM.priorityQueue.queue0.FirstOrDefault(x => x.Key == id).Value;
                process.timeOut = VM.clock;
                VM.fp.terminatedQueue.Add(id, process);
                VM.priorityQueue.removeQueue0(id);
            }
            foreach (int id in toBePromoted)
            {

                ProcessControlBlock process = VM.priorityQueue.queue0.FirstOrDefault(x => x.Key == id).Value;
                VM.priorityQueue.addQueue1(process);
                VM.priorityQueue.removeQueue0(id);
                Console.WriteLine($"Process {process.PID} was Promoted");
            }
        }

        public void RR1(VirtualMachine VM)
        {
            List<int> toBeDeleted = new List<int>();
            List<int> toBePromoted = new List<int>();

           

            foreach (KeyValuePair<int, ProcessControlBlock> process in VM.priorityQueue.queue0)
            {
                Console.Write(process.Value.PID);

                if (process.Value.processState != (int)ProcessStateEnum.ready)
                {
                    process.Value.processState = (int)ProcessStateEnum.waiting;
                }
               
            }

            Console.WriteLine();

            foreach (KeyValuePair<int, ProcessControlBlock> runningProcess in VM.priorityQueue.queue0)
            {
                runningProcess.Value.processState = (int)ProcessStateEnum.running;
                runningProcess.Value.endSection = runningProcess.Value.startSection + (quantum1 * BLOCK);
                if (runningProcess.Value.endSection > runningProcess.Value.endPC)
                {
                    VM.priorityQueue.queue0[runningProcess.Key].endSection = runningProcess.Value.endPC;
                }         
 //               Execute.executePartial(VM, sm, sem, runningProcess.Key, VM.priorityQueue.queue0);
                runningProcess.Value.startSection = runningProcess.Value.endSection;
            

                if (VM.priorityQueue.queue0[runningProcess.Key].endSection == runningProcess.Value.endPC)
                {
                    VM.priorityQueue.queue0[runningProcess.Key].processState = (int)ProcessStateEnum.terminated;
                    toBeDeleted.Add(runningProcess.Key);
                }
                if (VM.scheduler.setSched == "mfq" && VM.priorityQueue.queue0[runningProcess.Key].processState != (int)ProcessStateEnum.terminated)
                {
                    if(VM.priorityQueue.queue0[runningProcess.Key].processState == (int)ProcessStateEnum.running)
                    {
                        VM.priorityQueue.queue0[runningProcess.Key].round++;
                    }
                   
                    if (VM.priorityQueue.queue0[runningProcess.Key].round == 3)
                    {
                        VM.priorityQueue.queue0[runningProcess.Key].round = 0;
                        toBePromoted.Add(runningProcess.Key);
                    }
                }
                if (VM.priorityQueue.queue0[runningProcess.Key].processState != (int)ProcessStateEnum.terminated)
                {
                    runningProcess.Value.processState = (int)ProcessStateEnum.waiting;
                }


            }
          foreach(int id in toBeDeleted)
            {
                ProcessControlBlock process = VM.priorityQueue.queue0.FirstOrDefault(x => x.Key == id).Value;
                process.timeOut = VM.clock;
                VM.fp.terminatedQueue.Add(id, process);
                VM.priorityQueue.removeQueue0(id);
            }
            foreach (int id in toBePromoted)
            {
            
                ProcessControlBlock process = VM.priorityQueue.queue0.FirstOrDefault(x => x.Key == id).Value;
                VM.priorityQueue.addQueue1(process);
                VM.priorityQueue.removeQueue0(id);
                Console.WriteLine($"Process {process.PID} was Promoted");
            }
        }

        public void RR2(VirtualMachine VM)
        {
          List<int> toBeDeleted = new List<int>();
          List<int> toBeDemoted = new List<int>();

            foreach (KeyValuePair<int, ProcessControlBlock> process in VM.priorityQueue.queue1)
            {
                Console.Write(process.Value.PID);
                process.Value.processState = (int)ProcessStateEnum.waiting;
            }
            Console.WriteLine();

            foreach (KeyValuePair<int, ProcessControlBlock> runningProcess in VM.priorityQueue.queue1)
            {
                runningProcess.Value.processState = (int)ProcessStateEnum.running;
                runningProcess.Value.endSection = runningProcess.Value.startSection + (quantum2 * BLOCK);
                if (runningProcess.Value.endSection > runningProcess.Value.endPC)
                {
                    VM.priorityQueue.queue1[runningProcess.Key].endSection = runningProcess.Value.endPC;
                }         
 //             Execute.executePartial(VM, runningProcess.Key, VM.priorityQueue.queue1);
                runningProcess.Value.startSection = runningProcess.Value.endSection;


                if (VM.priorityQueue.queue1[runningProcess.Key].endSection == runningProcess.Value.endPC)
                {
                    VM.priorityQueue.queue1[runningProcess.Key].processState = (int)ProcessStateEnum.terminated;
                    toBeDeleted.Add(runningProcess.Key);
                }

                if (VM.scheduler.setSched == "mfq" && VM.priorityQueue.queue1[runningProcess.Key].processState != (int)ProcessStateEnum.terminated)
                {
                    if (VM.priorityQueue.queue1[runningProcess.Key].processState == (int)ProcessStateEnum.waiting)
                    {
                        VM.priorityQueue.queue1[runningProcess.Key].round++;
                    }

                    if (VM.priorityQueue.queue1[runningProcess.Key].round == 3)
                    {
                        VM.priorityQueue.queue1[runningProcess.Key].round = 0;
                        toBeDemoted.Add(runningProcess.Key);
                    }
            
                }

                if (VM.priorityQueue.queue1[runningProcess.Key].processState != (int)ProcessStateEnum.terminated)
                {
                    runningProcess.Value.processState = (int)ProcessStateEnum.waiting;
                }

            }
          foreach(int id in toBeDeleted)
            {
                ProcessControlBlock process = VM.priorityQueue.queue1.FirstOrDefault(x => x.Key == id).Value;
                process.timeOut = VM.clock;
                VM.fp.terminatedQueue.Add(id, process);
                VM.priorityQueue.removeQueue1(id);
            }

            foreach (int id in toBeDemoted)
            {
                ProcessControlBlock process = VM.priorityQueue.queue1.FirstOrDefault(x => x.Key == id).Value;

                VM.priorityQueue.addQueue0(process);
                VM.priorityQueue.removeQueue1(id);
                Console.WriteLine($"Process {process.PID} was Demoted");
            }

        }




        public static void makeChart(ProcessControlBlock entry)
        {
            int length = entry.length;
            length = length / 10;
            string progress = "";
            for (int i = 0; i < length; i++)
            {
                if (entry.processState == (int)ProcessStateEnum.ready)
                {
                    progress = progress + "-";
                }
                else
                {
                    progress = progress + "X";
                }

            }
            Console.WriteLine(progress + " " + entry.child);
        }
    }
}
