using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB.Memory
{
   public class Execute 
    {
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
                switch (instruction)
                {
                    case (byte)AssemblyInstruction.ADD:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        Console.WriteLine($"Adding Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] + VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.SUB:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        Console.WriteLine($"Subtracting Contents {VM.registers[source_1]} From Register {source_1} With Contents {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] - VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.MUL:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        Console.WriteLine($"Multiplying Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] * VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.DIV:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        Console.WriteLine($"Dividing Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] / VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;

                    case (byte)AssemblyInstruction.SWI:
                        int kernal = (int)VM.MEM[i + 1];
                        if (kernal == 1)
                        {

                            VM.fp.waitQueue[PID] = VM.fp.runningQueue[PID];
                            VM.fp.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;
                            Console.WriteLine("Waiting for User Input");
                            Console.ReadKey();
                            VM.fp.runningQueue[PID] = VM.fp.waitQueue[PID];
                            VM.fp.runningQueue[PID].processState = (int)ProcessStateEnum.running;
                        }
                        if (kernal == 2)
                        {
                            Console.WriteLine("Standard Output");
                        }
                        if (kernal == 3)
                        {
                            Console.WriteLine("Running in Kernal Mode");
                        }
                        if (kernal == 4)
                        {
                            Console.WriteLine("Running in User Mode");
                        }
                        if (kernal == 40)
                        {
                            int tempPID = Fork.fork(VM, PID, i);
                            Console.WriteLine($"fjork returned {tempPID}");
                        }
                        if (kernal == 50)
                        {
                            if (VM.fp.runningQueue[PID].parent != true && VM.fp.runningQueue[PID].child == true)
                            {

                                Fork.exec(VM, PID);
                                Console.WriteLine("Executing Fork");
                            }

                        }

                        break;
                }
            }

            VM.fp.terminatedQueue[PID] = VM.fp.runningQueue[PID];
            VM.fp.terminatedQueue[PID].processState = (int)ProcessStateEnum.terminated;
        }
    }
}
