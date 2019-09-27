using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CS6510_VirtualMachine_SJB
{
    static class MemoryManagement
    {
        public static string errors;
        public static void executeProgram(VirtualMachine VM, int PID)
        {
            VM.runningQueue[PID] = VM.readyQueue[PID];
            VM.runningQueue[PID].processState = (int)ProcessStateEnum.running;

            VM.dataAddr = VM.datAddr;
            for (int i = VM.runningQueue[PID].startPC; i < VM.loaderAddress; i += 6)
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

                            VM.waitQueue[PID] = VM.runningQueue[PID];
                            VM.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;
                            Console.WriteLine("Waiting for User Input");
                            Console.ReadKey();
                            VM.runningQueue[PID] = VM.waitQueue[PID];
                            VM.runningQueue[PID].processState = (int)ProcessStateEnum.running;
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
                            Console.WriteLine("Running in Fjork");
                            //// if child execute program
                            //if (PID == 0)
                            //{

                            //    executeProgram(VM, PID);

                            //}
                            //else // If parent then put in wait queue and wait for child to complete
                            //{
                       

                            //    VM.waitQueue[PID] = VM.runningQueue[PID];
                            //    VM.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;

                                   int tempPID = fork(VM, PID, i);
                                   Console.WriteLine($"fjork returned {tempPID}");

                            //    // check to see if complete
                            //    while (true)
                            //    {

                            //        break;

                            //    }
                            //    VM.runningQueue[PID] = VM.waitQueue[PID];
                            //    VM.runningQueue[PID].processState = (int)ProcessStateEnum.running;
                            //}

                        }
                        if(kernal == 50)
                        {
                      //      exec();
                        }

                        break;
                }
            }

            VM.terminatedQueue[PID] = VM.runningQueue[PID];
            VM.terminatedQueue[PID].processState = (int)ProcessStateEnum.terminated;
        }


        public static void loadProgram(VirtualMachine VM, string programString)
        {
            if (programString == "")
            {
                Console.WriteLine("Please Enter a File Name");
                return;
            }
            try
            {
                using (FileStream fs = new FileStream(programString, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs, new ASCIIEncoding()))
                    {
                        ProcessControlBlock newProcess = new ProcessControlBlock();
                        newProcess.programFileName = programString;
                        newProcess.PID = VM.PIDCount;
                        VM.newQueue[VM.PIDCount++] = newProcess;
                        newProcess.processState = (int)ProcessStateEnum.newProcess;


                        int bSize = br.ReadInt32();
                        VM.PC = br.ReadInt32();
                        VM.loaderAddress = br.ReadInt32();
                        VM.fetchAddr = VM.loaderAddress;
                        VM.dataAddr = VM.loaderAddress;
                        VM.datAddr = VM.loaderAddress;
                        Console.WriteLine($"bSize {bSize}");
                        Console.WriteLine($"PC {VM.PC}");
                        Console.WriteLine($"Loader Address {VM.loaderAddress}");

                        for (int i = 0; i < VM.PC; i++)
                        {
                            VM.MEM[i] = br.ReadByte();
                        }

                        VM.newQueue[newProcess.PID].startPC = VM.PC;

                        for (int i = VM.PC; i < bSize; i += 6)
                        {
                            byte Byte = br.ReadByte();
                            switch (Byte)
                            {
                                case (byte)AssemblyInstruction.ADD:
                                    Console.WriteLine("Add Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.SUB:
                                    Console.WriteLine("Subtract Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.MUL:
                                    Console.WriteLine("Multiply Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.DIV:
                                    Console.WriteLine("Divide Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.MOV:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.MVI:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.ADR:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.STR:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.STRB:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.LDR:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.LDRB:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.B:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.BX:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.BNE:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.BGT:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.BLT:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.BEQ:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.CMP:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.AND:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.ORR:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.EOR:
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.SWI:
                                    Console.WriteLine("Switch Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;

                            }

                        }
                        VM.readyQueue[VM.PIDCount] = newProcess;
                        newProcess.processState = (int)ProcessStateEnum.ready;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                errors = ex.ToString();
            }
        }

        static void loadExpression(VirtualMachine VM, BinaryReader br, byte Byte)
        {
            VM.MEM[VM.loaderAddress++] = Byte;
            VM.MEM[VM.loaderAddress++] = br.ReadByte();
            VM.MEM[VM.loaderAddress++] = br.ReadByte();
            VM.MEM[VM.loaderAddress++] = br.ReadByte();
            VM.MEM[VM.loaderAddress++] = br.ReadByte();
            VM.MEM[VM.loaderAddress++] = br.ReadByte();
        }

        public static void coreDump(VirtualMachine VM)
        {
            for (int i = 100; i < VM.loaderAddress; i += 6)
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
                        Console.WriteLine($"Add Register {source_1} With Register {source_2} Into Register {destination}");
                        break;
                    case (byte)AssemblyInstruction.SUB:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        Console.WriteLine($"SUB Register {source_1} From Register {source_2} Into Register {destination}");
                        break;
                    case (byte)AssemblyInstruction.MUL:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        Console.WriteLine($"MUL Register {source_1} With Register {source_2} Into Register {destination}");
                        break;
                    case (byte)AssemblyInstruction.DIV:
                        destination = (int)VM.MEM[i + 1];
                        source_1 = (int)VM.MEM[i + 2];
                        source_2 = (int)VM.MEM[i + 3];
                        Console.WriteLine($"DIV  Register {source_1} With Register {source_2} Into Register {destination}");
                        break;
                    case (byte)AssemblyInstruction.MOV:
                        Console.WriteLine($"MOV");
                        break;
                    case (byte)AssemblyInstruction.MVI:
                        Console.WriteLine($"MVI");
                        break;
                    case (byte)AssemblyInstruction.ADR:
                        Console.WriteLine($"ADR");
                        break;
                    case (byte)AssemblyInstruction.STR:
                        Console.WriteLine($"STR");
                        break;
                    case (byte)AssemblyInstruction.STRB:
                        Console.WriteLine($"STRB");
                        break;
                    case (byte)AssemblyInstruction.LDR:
                        Console.WriteLine($"LDR");
                        break;
                    case (byte)AssemblyInstruction.LDRB:
                        Console.WriteLine($"LDRB");
                        break;
                    case (byte)AssemblyInstruction.B:
                        Console.WriteLine($"B");
                        break;
                    case (byte)AssemblyInstruction.BX:
                        Console.WriteLine($"BX");
                        break;
                    case (byte)AssemblyInstruction.BNE:
                        Console.WriteLine($"BNE");
                        break;
                    case (byte)AssemblyInstruction.BGT:
                        Console.WriteLine($"BGT");
                        break;
                    case (byte)AssemblyInstruction.BLT:
                        Console.WriteLine($"BLT");
                        break;
                    case (byte)AssemblyInstruction.BEQ:
                        Console.WriteLine($"BEQ");
                        break;
                    case (byte)AssemblyInstruction.CMP:
                        Console.WriteLine($"CMP");
                        break;
                    case (byte)AssemblyInstruction.AND:
                        Console.WriteLine($"AND");
                        break;
                    case (byte)AssemblyInstruction.ORR:
                        Console.WriteLine($"ORR");
                        break;
                    case (byte)AssemblyInstruction.EOR:
                        Console.WriteLine($"EOR");
                        break;
                    case (byte)AssemblyInstruction.SWI:
                        int kernal = (int)VM.MEM[i + 1];
                        Console.WriteLine($"SWI Instruction {kernal}");
                        break;
                }
            }
        }

        public static int fork(VirtualMachine VM, int PID, int PC)
        {
            //int pcCounter = VM.runningQueue[PID].endPC + 1;


            ProcessControlBlock newProcess = new ProcessControlBlock();
            // Move child into new process
            VM.newQueue[VM.PIDCount++] = newProcess;
            newProcess.startPC = PC + 1;
            newProcess.processState = (int)ProcessStateEnum.newProcess;
            newProcess.programFileName = VM.runningQueue[PID].programFileName + " Child";

            // USED TO COPY MEMORY, NOT USED ANYMORE   
            //for (int i = VM.runningQueue[PID].startPC; i < VM.runningQueue[PID].endPC; i++)
            //{
            //    VM.MEM[pcCounter++] = VM.MEM[i];
            //}

            // Move Child into Ready
            VM.readyQueue[newProcess.PID] = VM.newQueue[newProcess.PID];
            VM.waitQueue[newProcess.PID].processState = (int)ProcessStateEnum.ready;

            // Put Parent Process Into Waiting
            VM.waitQueue[PID] = VM.runningQueue[PID];
            VM.waitQueue[PID].processState = (int)ProcessStateEnum.waiting;

            exec(VM, newProcess.PID);

            VM.runningQueue[PID] = VM.readyQueue[PID];
            VM.runningQueue[PID].processState = (int)ProcessStateEnum.running;

                return PID;




        }

        public static int exec(VirtualMachine VM, int PID)
        {
            executeProgram(VM, PID);
            return 0;
        }

    }
}
