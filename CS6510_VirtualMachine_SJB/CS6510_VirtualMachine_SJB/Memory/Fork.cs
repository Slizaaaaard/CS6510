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
            newProcess.startPC = PC + 30;
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

            // Put Parent Process Into Waiting
            VM.fp.waitQueue[PID] = VM.fp.runningQueue[PID];
            VM.fp.waitQueue[PID].parent = true;
            VM.fp.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;


            VM.fp.runningQueue[PID] = VM.fp.readyQueue[PID];
            VM.fp.runningQueue[PID].processState = (int)ProcessStateEnum.running;

            return PID;
        }

        public static int exec(VirtualMachine VM, int PID)
        {
            
            Execute.executeProgram(VM, PID);
            return 0;
        }

    }
}
