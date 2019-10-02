using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB.Memory
{
    public static class CoreDump
    {
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
    }
}
