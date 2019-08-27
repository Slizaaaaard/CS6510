using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    class VirtualMachine
    {
        public SortedDictionary<string, int> REG = new SortedDictionary<string, int>();
        public  byte[] MEM = new byte[10000];
        public int PC = 0;
        public int loaderAddress = 0;
        public VirtualMachine()
        {
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
