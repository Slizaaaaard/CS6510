using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace CS6510_VirtualMachine_SJB
{
    public class PriorityQueue
    {

        public SortedDictionary<int, ProcessControlBlock> queue0 = new SortedDictionary<int, ProcessControlBlock>();
        public SortedDictionary<int, ProcessControlBlock> queue1 = new SortedDictionary<int, ProcessControlBlock>();
        public SortedDictionary<int, ProcessControlBlock> queue2 = new SortedDictionary<int, ProcessControlBlock>();
        public int Count0;
        public int Count1;
        public int Count2;
        PriorityQueue()
        {
            Count0 = 0;
            Count1 = 0;
            Count2 = 0;
        }

        public void addQueue0(ProcessControlBlock pcb)
        {
            queue0.Add(Count0, pcb);
            Count0++;
        }
        public void removeQueue0(int id)
        {
            int temp = queue0.FirstOrDefault(x => x.Key == id).Key;
            queue0.Remove(temp);
        }


        public void addQueue1(ProcessControlBlock pcb)
        {
            queue1.Add(Count1, pcb);
            Count1++;
        }
        public void removeQueue1(int id)
        {
            int temp = queue1.FirstOrDefault(x => x.Key == id).Key;
            queue1.Remove(temp);
        }


        public void addQueue2(ProcessControlBlock pcb)
        {
            queue0.Add(Count2, pcb);
            Count2++;
        }
        public void removeQueue2(int id)
        {
            int temp = queue2.FirstOrDefault(x => x.Key == id).Key;
            queue2.Remove(temp);
        }
    }
}
