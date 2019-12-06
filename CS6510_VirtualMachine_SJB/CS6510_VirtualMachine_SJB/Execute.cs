using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB.Memory
{

 
    public class Execute
    {
        static string[] message = new string[10] { "This", "is" , "the", "message", "from", "the" ,"producer", "fin" , "Test", "Test" };
        static int messageCount = 0;
        static Random number = new Random();
        public static void executeProgram(VirtualMachine VM, int PID)
        {
            
            VM.fp.runningQueue[PID] = VM.fp.readyQueue[PID];
            VM.fp.runningQueue[PID].processState = (int)ProcessStateEnum.running;

            VM.dataAddr = VM.datAddr;

            //for (int i = VM.runningQueue[PID].startPC; i < VM.loaderAddress; i++)
            //{
            //    Console.WriteLine(VM.MEM[i] + " at postition " + i);
            //}

            for (int i = VM.fp.runningQueue[PID].startPC; i < VM.fp.runningQueue[PID].endPC; i += 6)
            {
                int instruction = VM.MEM[i];
                int destination;
                int source_1;
                int source_2;
                int tempPID = 0;
                switch (instruction)
                {
                    case (byte)AssemblyInstruction.ADD:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Adding Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] + VM.registers[source_2];
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.SUB:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Subtracting Contents {VM.registers[source_1]} From Register {source_1} With Contents {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] - VM.registers[source_2];
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.MUL:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Multiplying Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] * VM.registers[source_2];
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.DIV:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Dividing Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] / VM.registers[source_2];
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;

                    case (byte)AssemblyInstruction.SWI:
                        int kernal = (int)VM.MEM[i + 1];
                        if (kernal == 1)
                        {

                            VM.fp.waitQueue[PID] = VM.fp.runningQueue[PID];
                            VM.fp.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;
                            //Console.WriteLine("Waiting for User Input");
                            Console.ReadLine();
                            VM.fp.runningQueue[PID] = VM.fp.waitQueue[PID];
                            VM.fp.runningQueue[PID].processState = (int)ProcessStateEnum.running;
                        }
                        if (kernal == 2)
                        {
                            //Console.WriteLine("Standard Output");
                        }
                        if (kernal == 3)
                        {
                            //Console.WriteLine("Running in Kernal Mode");
                        }
                        if (kernal == 4)
                        {
                            //Console.WriteLine("Running in User Mode");
                        }
                        if (kernal == 40)
                        {
                            if (VM.fp.runningQueue[PID].child == false)
                            {
                                tempPID = Fork.fork(VM, PID, i);
                            }
                         
                        }
                        if (kernal == 50)
                        {
                            if (VM.fp.runningQueue[PID].parent != true && VM.fp.runningQueue[PID].child == true)
                            {
                                Fork.exec();
                                VM.fp.readyQueue[PID].endPC = i;
                            }

                        }
                        if (kernal == 60)
                        {
                            if (VM.fp.runningQueue[PID].parent == true)
                            {
                                // Put Parent Process Into Waiting
                                VM.fp.waitQueue[PID] = VM.fp.runningQueue[PID];

                                VM.fp.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;

                                //Console.WriteLine("\nParent Waiting\n");
                                Fork.wait(VM, VM.fp.runningQueue[PID].childPid);

                                //Console.WriteLine($"fjork returned {VM.fp.runningQueue[PID].childPid}");
                            }

                       
                        }
                        if (kernal == 70)
                        {
 
                            

                        }

                        break;
                }
                VM.clock++;
                foreach (KeyValuePair<int, ProcessControlBlock> process in VM.priorityQueue.queue2)
                {
                    switch (process.Value.processState)
                    {
                        case (int)ProcessStateEnum.waiting:
                            Console.Write("W");
                            process.Value.waitTime++;
                            break;
                        case (int)ProcessStateEnum.running:
                            Console.Write("R");
                            break;
                        case (int)ProcessStateEnum.terminated:
                            Console.Write("T");
                            break;
                        case (int)ProcessStateEnum.ready:
                            Console.Write("N");
                            process.Value.response = VM.clock + 1;
                            break;
                    }

                }
  //              Console.Write("\n");
            }
            VM.fp.terminatedQueue[PID] = VM.fp.runningQueue[PID];
            VM.fp.terminatedQueue[PID].processState = (int)ProcessStateEnum.terminated;
            VM.fp.readyQueue[PID].processState = (int)ProcessStateEnum.terminated;
        }

        public static void executePartial(VirtualMachine VM, SharedMemory sm, Semaphore sem, int PID, SortedDictionary<int, ProcessControlBlock> queue)
        {

            for (int i = queue[PID].startSection; i < queue[PID].endSection; i += 6)
            {
                int instruction = VM.MEM[i];
                int destination;
                int source_1;
                int source_2;
                int tempPID = 0;
                switch (instruction)
                {
                    case (byte)AssemblyInstruction.ADD:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Adding Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] + VM.registers[source_2];
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.SUB:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Subtracting Contents {VM.registers[source_1]} From Register {source_1} With Contents {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] - VM.registers[source_2];
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.MUL:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Multiplying Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] * VM.registers[source_2];
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.DIV:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        //Console.WriteLine($"Dividing Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        if(VM.registers[source_2] != 0)
                        {
                            VM.registers[destination] = VM.registers[source_1] / VM.registers[source_2];
                        }
                        //Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;

                    case (byte)AssemblyInstruction.SWI:
                        int kernal = (int)VM.MEM[i + 1];
                        if (kernal == 1)
                        {

                            //VM.fp.waitQueue[PID] = VM.fp.runningQueue[PID];
                            //VM.fp.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;
                            Console.WriteLine("Waiting for User Input");
                            Console.ReadLine();
                            //VM.fp.runningQueue[PID] = VM.fp.waitQueue[PID];
                            //VM.fp.runningQueue[PID].processState = (int)ProcessStateEnum.running;
                        }
                        if (kernal == 2)
                        {
                            //Console.WriteLine("Standard Output");
                        }
                        if (kernal == 3)
                        {
                            //Console.WriteLine("Running in Kernal Mode");
                        }
                        if (kernal == 4)
                        {
                            //Console.WriteLine("Running in User Mode");
                        }
                        if (kernal == 40)
                        {
                            if (VM.fp.runningQueue[PID].child == false)
                            {
                                tempPID = Fork.fork(VM, PID, i);
                            }

                        }
                        if (kernal == 50)
                        {
                            if (VM.fp.runningQueue[PID].parent != true && VM.fp.runningQueue[PID].child == true)
                            {
                                Fork.exec();
                                VM.fp.readyQueue[PID].endPC = i;
                            }

                        }
                        if (kernal == 60)
                        {
                            if (VM.fp.runningQueue[PID].parent == true)
                            {
                                // Put Parent Process Into Waiting
                                VM.fp.waitQueue[PID] = VM.fp.runningQueue[PID];

                                VM.fp.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;

                                //Console.WriteLine("\nParent Waiting\n");
                                Fork.wait(VM, VM.fp.runningQueue[PID].childPid);

                                //Console.WriteLine($"fork returned {VM.fp.runningQueue[PID].childPid}");
                            }
                        }
                        if (kernal == 70)
                        {

                            queue[PID].ioBurst = number.Next(0, 20);
                            queue[PID].processState = (int)ProcessStateEnum.running;
                        }
                        if(kernal == 80)
                        {
                            //Console.WriteLine("produce");
                            if(sm.send(sem, message[messageCount]) != true)
                            {
                                sem.busyWaiting("Waiting for Consumer", queue[PID], i);
                                     
                            }
                            else
                            {
                                messageCount++;
                            }
                         
                        

                        }
                        if(kernal == 81)
                        {
                            //Console.WriteLine("consume");
                            if (sm.receive(sem) != true)
                            {
                                sem.busyWaiting("Waiting for Producer", queue[PID], i);                            
                            }
                            
                        }
            
                        break;
                }

                VM.clock++;

                foreach(KeyValuePair<int, ProcessControlBlock> process in queue)
                {
                    switch (process.Value.processState)
                    {
                        case (int)ProcessStateEnum.waiting:
                            process.Value.ghant = process.Value.ghant + "W";
                            process.Value.waitTime++;
                            break;
                        case (int)ProcessStateEnum.running:
                            if(VM.scheduler.setSched == "mfq")
                            {
                          
                                if(queue[PID].ioBurst == 0)
                                {
                                    process.Value.processState = (int)ProcessStateEnum.waiting;
                                    Console.Write("W");
                                }
                                else
                                {
                                    Console.Write("R");
                                    queue[PID].ioBurst--;
                                }
                            }
                            else
                            {
                                process.Value.ghant = process.Value.ghant + "R";
                            }
                          
                            break;
                        case (int)ProcessStateEnum.terminated:
                            process.Value.ghant = process.Value.ghant + "T";
                            break;
                        case (int)ProcessStateEnum.ready:
                            process.Value.ghant = process.Value.ghant + "N";
                            process.Value.response = VM.clock + 1;
                            break;
                    }
                    
                }
            }

       

        }
        static void fetch()
        {

        }

        static void decode()
        {

        }
    }
}
