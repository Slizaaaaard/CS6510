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
        public int ioBurst = 0;
        public int lastLoad = 0;
        public string ghant = "";
        public List<int> pageNumbers = new List<int>(); 

        public void proc()
        {
            pageNumbers.Add(1);
            pageNumbers.Add(4);
            
            Console.Write($"Proccess ID {PID}, Pages Numbers");
            foreach(int page in pageNumbers)
            {
                Console.Write($" page ");
            }
            Console.WriteLine($"Status {processState}");
           
        }

        public Scheduler Scheduler
        {
            get => default;
            set
            {
            }
        }

        public SharedMemory SharedMemory
        {
            get => default;
            set
            {
            }
        }
    }
}
