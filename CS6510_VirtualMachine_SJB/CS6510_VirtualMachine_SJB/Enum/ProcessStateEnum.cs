using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public enum ProcessStateEnum
    {
        newProcess = 0,
        running = 1,
        waiting = 2,
        ready = 3,
        terminated = 5,
    }
}
