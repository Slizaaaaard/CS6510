using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class Semaphore
    {
        public SortedDictionary<string, int> semList;

        public Semaphore()
        {
            semList = new SortedDictionary<string, int>();
        }
        public void semInit(string name)
        {
            if(semList.ContainsKey(name) == false)
            {
                semList.Add(name, 0);
            }

        }
        public bool semWait(string name)
        {
            if ( semList[name] != 0) {
                semList[name] -= 1;
                return true;
            }
            return false;
        }
        public bool semSignal(string name)
        {
            if (semList[name] != 1)
            {
                semList[name] += 1;
                return true;
            }
            return false;
        }
        public void busyWaiting(string condition)
        {
            Console.WriteLine(condition);
        }


    }
}
