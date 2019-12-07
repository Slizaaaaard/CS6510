using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class CPU
    {
        public List<int> pageTable = new List<int>();
        public CPU()
        {

        }
        static void getLogicalAddress(int pageNumber, int offset)
        {
            //page number + offset
        }
    }
}
