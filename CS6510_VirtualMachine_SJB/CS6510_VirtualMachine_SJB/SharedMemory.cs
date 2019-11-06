using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class SharedMemory
    {
        public SortedDictionary<string, int[]> sharedMemory;
        public SharedMemory()
        {
            sharedMemory = new SortedDictionary<string, int[]>();
        }
        public void shmOpen(string name, string mode, int size)
        {
            int[] memory = new int[size];
            sharedMemory.Add(name, memory);
            return;
        }
        public void shmUnlink(string name)
        {
            sharedMemory.Remove(name);
            return;
        }

    }
}
