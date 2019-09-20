using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    class VirtualMachine
    {
        public SortedDictionary<string, int> REG = new SortedDictionary<string, int>();
        public int[] registers= {1, 2 ,3 ,4 ,5 ,6 ,7 ,8 };
        public  byte[] MEM = new byte[10000];
        public int PC;
        public int loaderAddress;
        public int fetchAddr;
        public int dataAddr;
        public int datAddr;
        public int instructionPC;
        public int clock;
        public VirtualMachine()
        {
            PC = 0;
            loaderAddress = 0;
            fetchAddr = 0;
            dataAddr = 0;
            datAddr = 0;
            instructionPC = 0;
            REG.Add("R0", 0);
            REG.Add("R1", 0);
            REG.Add("R2", 0);
            REG.Add("R3", 0);
            REG.Add("R4", 0);
            REG.Add("R5", 0);
            REG.Add("SP", 0);
            REG.Add("FP", 0);
            REG.Add("SL", 0);
            REG.Add("Z", 0);
        }


    }
}
