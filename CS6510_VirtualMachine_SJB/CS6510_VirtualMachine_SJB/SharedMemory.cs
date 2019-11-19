using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class SharedMemory
    {
        string memoryName;
        string memoryMode;
        string[] memory;
        Semaphore sem = new Semaphore();
        public SharedMemory()
        {

        }
        public void shmOpen(string name, string mode, int size)
        {
            Console.WriteLine($"Name is {name}");
            Console.WriteLine($"Mode is {mode}");
            Console.WriteLine($"Size is {size}");
            memoryName = name;
            memoryMode = mode;
            memory = new string[size];
            return;
        }
        public void shmUnlink(string name)
        {
            memoryName = "";
            memoryMode = "";
            memory = null;
            Console.WriteLine($"{name} Unlinked");
            return;
        }

        public bool send(Semaphore process, string message)
        {
                if (process.semSignal(memoryName) == true)
                {
                memory[0] = message;
                return true;
                }
            return false;
        }
        public bool receive(Semaphore process)
        {
               if( process.semWait(memoryName) == true)
            {
                Console.WriteLine(memory[0]);
                return true;
            }
            return false;
        }

    }
}
