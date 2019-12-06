using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
    public class Page
    {
        public int pageSize;
        public int pageNumber;
        const int MEM = 100;

        public Page()
        {
            pageSize = 10;
            pageNumber = 0;
            List<int[]> temp = new List<int[]>();
            memory.Add(temp);

        }

        public List<List<int[]>> memory = new List<List<int[]>>();
        public SortedDictionary<int, int> dirtyBit = new SortedDictionary<int, int>();
        public SortedDictionary<int, int> referenceBit = new SortedDictionary<int, int>();

        public void add(int[] word)
        {
            if((MEM - (memory.Count * pageNumber)) > 0)
            {
                if(pageNumber != 0)
                {
                    if (memory[pageNumber].Count < pageSize)
                    {
                        memory[pageNumber].Add(word);
                    }
                }
                else
                {
                 
                    pageNumber += 1;
                    List<int[]> temp = new List<int[]>();
                    memory.Add(temp);
                    referenceBit.Add(pageNumber, pageNumber);
                    memory[pageNumber].Add(word);
                }
            }
            else
            {
                replacePage();
            }
        }

        public void replacePage()
        {
       
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
