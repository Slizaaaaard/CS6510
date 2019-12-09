using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class Page
    {
        public int pageSize;
        public int pageNumber;
        public int index = 0;
        const int MEM = 100;
        public int nextPage = 0;
        public int nextOffset = 0;
        public bool replaced = false; 

        public Page()
        {
            pageSize = 10;
            pageNumber = 0;
        }

        public List<List<int[]>> memory = new List<List<int[]>>();
        public SortedDictionary<int, int> dirtyBit = new SortedDictionary<int, int>();
        public SortedDictionary<int, int> referenceBit = new SortedDictionary<int, int>();
        public SortedDictionary<int, int> validInvalidBit = new SortedDictionary<int, int>();

        public int getNextPage(VirtualMachine vm)
        {
            if(nextPage < memory.Count() - 1 && replaced == false)
                {
                    return nextPage + 1;
            }
            else
            {
                int location = vm.cpu.logicalToPhysical(nextPage, nextOffset) + 6;
                int replaced = replacePage();
                loadNextPage(location, vm, replaced);
                return replaced;
            }
        }

        public void loadNextPage(int location, VirtualMachine vm, int replaced)
        {
            for(int i = 0; i < pageSize; i++)
            {
                List<int> temp = new List<int>();
                for(int j = 0; j < 6; j++)
                {
                    temp.Add(vm.MEM[location]);
                }
                memory[replaced].Add(temp.ToArray());
            }  
        }

        public void add(int[] word, int location, VirtualMachine vm)
        {
            if((MEM - (memory.Count * pageSize)) >= 0)
            {

                if (memory.Count != 0)
                {
                    if (memory[pageNumber].Count < pageSize)
                    {
                        memory[pageNumber].Add(word);
                    }
                    else if((MEM - (memory.Count * pageSize + pageSize)) >= 0)
                    {
                        List<int[]> temp = new List<int[]>();
                        memory.Add(temp);
                        pageNumber++;
                        dirtyBit.Add(pageNumber, 0);
                        referenceBit.Add(pageNumber, pageNumber);
                        validInvalidBit.Add(pageNumber, 0);
                        vm.cpu.pageTable.Add(location);

                    }
                }
                else 
                {
                    dirtyBit.Add(pageNumber, 0);
                    referenceBit.Add(pageNumber, pageNumber);
                    validInvalidBit.Add(pageNumber, 0);
                    List<int[]> temp = new List<int[]>();
                    memory.Add(temp);
                    memory[pageNumber].Add(word);
                    vm.cpu.pageTable.Add(location);
                    // use for replace
                    //vm.cpu.pageTable[pageNumber] = location;

                }
            }
   
        
        }

        public int replacePage()
        {
            int temp = 0;
            bool found = false;
            if(dirtyBit.ContainsValue(0) == true)
            {
                while(found == false)
                {
                    var valueList = referenceBit.ToList();
                    valueList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                    temp = valueList[0].Key;
                  
            
                    if (dirtyBit[temp] == 0)
                    {
                        memory[temp] = new List<int[]>();
                        referenceBit[temp] = (referenceBit.Values.Max() + 1);
                        found = true;
                    }
                    else
                    {
                        referenceBit[temp] = (referenceBit.Values.Max() + 1);
                    }
                }
            }
            else
            {
                temp = referenceBit.Values.Min();
                memory[temp] = new List<int[]>();
                referenceBit[temp] = (referenceBit.Values.Max() + 1);
            }
            Console.WriteLine($"Replaced Page {temp}");
            replaced = true;
            return temp;
        }

        public void free()
        {
            int used = memory.Count * pageNumber;
            int totalPage = MEM / pageSize;
            int freePage = totalPage - memory.Count;
            int free = (MEM - used) * 6;
            Console.WriteLine($"Free Memory Size {free} Bytes, Free Pages {freePage}");
        }

        public void proc()
        {
            int usedPage = memory.Count * pageNumber;
            int used = usedPage * pageSize;
            int totalPage = MEM / pageSize;
            int freePage = totalPage - memory.Count;
            int free = (MEM - used) * 6;
            Console.WriteLine(($"Used Memory is {used}, Total Memory is {MEM}"));
        }

        public void setPageSize(int size)
        {
            pageSize = size;
        }
        public int getPageSize()
        {
            return pageSize;
        }
        public void setPageNumber(int size)
        {
            pageNumber = size;
        }
    }
}
