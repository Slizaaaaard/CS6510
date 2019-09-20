using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    class ProcessControlBlock
    {
        public string newProcess;
        public string running;
        public string waiting;
        public string ready;
        public string terminated;
        public int PC;

    }
}
