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
        }

       public List<List<int[]>> memory = new List<List<int[]>>();

        public void add(int[] word)
        {
            if((MEM - (memory.Count * pageNumber)) > 0)
            {
                if (memory[pageNumber].Count < pageSize)
                {
                    memory[pageNumber].Add(word);
                }
                else
                {
                    pageNumber += 1;
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
