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
        public string programFileName;
        public int processState;
        public int PC;

    }
}
