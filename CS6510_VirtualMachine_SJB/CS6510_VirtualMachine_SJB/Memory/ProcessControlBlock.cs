using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class ProcessControlBlock
    {
        public int PID;
        public int ParentPID;
        public int[] registers = { 1, 2, 3, 4, 5, 6, 7, 8 };
        public int startPC;
        public int endPC;
        public int startSection;
        public int endSection;
        public string programFileName;
        public int processState;
        public int PC;
        public bool parent = false;
        public bool child = false;
        public int childPid = 0;
        public int length;
        public int round = 0;
        public string errors;
        public int timeIn = 0;
        public int timeOut = 0;
        public int waitTime = 0;
        public int response = 0;

        public Scheduler Scheduler
        {
            get => default;
            set
            {
            }
        }
    }
}
