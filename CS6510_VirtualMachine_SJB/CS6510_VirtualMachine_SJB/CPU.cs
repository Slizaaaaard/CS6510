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

        //translate to from physical to logical
        public int logicalToPhysical(int page, int offset)
        {
           
            int physical = pageTable[page] + offset;
            Console.WriteLine($"Translating Logical Address {page}, {offset} to Physical Address {physical}");
            return physical;
        }

        public int[] getNextInstruction(VirtualMachine vm)
        {
            int previous = 0;
            int[] instruction = vm.page.memory[vm.page.nextPage][vm.page.nextOffset];
            if(vm.page.pageSize - 1 > vm.page.nextOffset)
            {
                vm.page.nextOffset++;
            }
            else
            {  
                previous =
                vm.page.nextOffset = 0;
                vm.page.nextPage = vm.page.getNextPage(vm);
            }

            return instruction;
        }
    }
}
