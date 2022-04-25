using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class FiveProcess
    {
        public SortedDictionary<int, ProcessControlBlock> newQueue = new SortedDictionary<int, ProcessControlBlock>();
        public SortedDictionary<int, ProcessControlBlock> readyQueue = new SortedDictionary<int, ProcessControlBlock>();
        public SortedDictionary<int, ProcessControlBlock> runningQueue = new SortedDictionary<int, ProcessControlBlock>();
        public SortedDictionary<int, ProcessControlBlock> waitQueue = new SortedDictionary<int, ProcessControlBlock>();
        public SortedDictionary<int, ProcessControlBlock> terminatedQueue = new SortedDictionary<int, ProcessControlBlock>();
        public FiveProcess()
        {

        }
    }
}
