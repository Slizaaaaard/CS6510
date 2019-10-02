using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class VirtualMachine
    {
        public int[] registers= {1, 2 ,3 ,4 ,5 ,6 ,7 ,8};
        public  byte[] MEM = new byte[10000];
        public int PC;
        public int loaderAddress;
        public int fetchAddr;
        public int dataAddr;
        public int datAddr;
        public int instructionPC;
        public int clock;
        public int PIDCount;
        public FiveProcess fp;
        public VirtualMachine()
        {
            PC = 0;
            loaderAddress = 0;
            fetchAddr = 0;
            dataAddr = 0;
            datAddr = 0;
            instructionPC = 0;
            fp = new FiveProcess();
        }
    }
}
