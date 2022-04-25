using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB.Memory
{
    static class Fork
    {
        public static int fork(VirtualMachine VM, int PID, int PC)
        {
            //int pcCounter = VM.runningQueue[PID].endPC + 1;


            ProcessControlBlock newProcess = new ProcessControlBlock();
            // Move child into new process
            newProcess.startPC = PC;
            newProcess.endPC = VM.fp.runningQueue[PID].endPC;
            newProcess.length = VM.fp.runningQueue[PID].length + VM.fp.runningQueue[PID].startPC - PC;
            newProcess.processState = (int)ProcessStateEnum.newProcess;
            newProcess.programFileName = VM.fp.runningQueue[PID].programFileName + " Child";
            newProcess.PID = VM.PIDCount;
            newProcess.child = true;
            VM.fp.newQueue[newProcess.PID] = newProcess;

            // USED TO COPY MEMORY, NOT USED ANYMORE   
            //for (int i = VM.runningQueue[PID].startPC; i < VM.runningQueue[PID].endPC; i++)
            //{
            //    VM.MEM[pcCounter++] = VM.MEM[i];
            //}

            // Move Child into Ready
            VM.fp.readyQueue[newProcess.PID] = VM.fp.newQueue[newProcess.PID];
            VM.fp.readyQueue[newProcess.PID].processState = (int)ProcessStateEnum.ready;

            // Store child PID in parent PCB
            VM.fp.readyQueue[PID].childPid = newProcess.PID;
            VM.fp.readyQueue[PID].parent = true;
            VM.PIDCount++;
            return newProcess.PID;
        }

        public static void exec()
        {
            Console.WriteLine("Type in Command to Exec");
            string execCommand = Console.ReadLine();
            Console.WriteLine($"Executing {execCommand}");
        }
        public static int wait(VirtualMachine VM, int PID)
        {
          
            Execute.executeProgram(VM, PID);
            return 0;
        }

    }
}
