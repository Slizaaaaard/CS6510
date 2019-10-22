using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CS6510_VirtualMachine_SJB.Memory;

namespace CS6510_VirtualMachine_SJB
{
    public class Scheduler
    {
        public string setSched = "";
        public int quantum1;
        public int quantum2;
        void setSchedule()
        {

        }

  
        public void FCFS(bool scheduleConflict, string[] listString, int pidTemp, VirtualMachine VM)
        {
            foreach (string input in listString)
            {
                if (scheduleConflict == false)
                {

                    Console.WriteLine($"\nExecute Program {input}");
                    pidTemp = VM.fp.readyQueue.FirstOrDefault(x => x.Value.programFileName == input && x.Value.processState != (int)ProcessStateEnum.terminated).Key;
                    Execute.executeProgram(VM, pidTemp);
                    foreach (KeyValuePair<int, ProcessControlBlock> entry in VM.fp.readyQueue)
                    {
                        Console.Write($"Pid {entry.Value.PID} process state {ProcessStateEnum.GetName(typeof(ProcessStateEnum), entry.Value.processState)} ");
                        makeChart(entry.Value);
                    }

                }

            }
        }

        public void RR1()
        {

        }

        public void RR2()
        {

        }

        public void promoteToRR2()
        {

        }


        public void demoteToRR1()
        {


        }



        public static void makeChart(ProcessControlBlock entry)
        {
            int length = entry.length;
            length = length / 10;
            string progress = "";
            for (int i = 0; i < length; i++)
            {
                if (entry.processState == (int)ProcessStateEnum.ready)
                {
                    progress = progress + "-";
                }
                else
                {
                    progress = progress + "X";
                }

            }
            Console.WriteLine(progress + " " + entry.child);
        }

    }
}
