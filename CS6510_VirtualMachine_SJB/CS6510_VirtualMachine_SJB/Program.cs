using System;
using System.IO;
using System.Text;
namespace CS6510_VirtualMachine_SJB
{
    class Program
    {
        static VirtualMachine VM = new VirtualMachine();
        static bool shell;
        static string shellString;
        static string errors;
        static string[] readyQueue;
        static string[] jobQueue;
        static string[] IOQueue;
        static int timeIn = 0;

        static void Main(string[] args)
        {

            do
            {

                if (shell == true)
                {
                    Console.Write("MYVM->");
                }
                else
                {
                    Console.Write("VM->");
                }
                shellString = Console.ReadLine();
                shellString = shellString.ToLower();
                if (shellString == "myvm")
                {
                    shell = true;
                }
                if (shellString == "vm")
                {
                    shell = false;
                }
 
                if (shellString.Substring(0, 4) == "load")
                {

                    if (shellString.Contains("-v"))
                    {
                        shellString = shellString.Remove(0, 4);
                        shellString = shellString.Trim();
                        shellString = shellString.Remove(0, 2);
                        shellString = shellString.Trim();
                        loadProgram(shellString);
                    }
             
                }

                if (shellString.Contains("run"))
                {
                    if (shellString.Contains("-v"))
                    {
                        shellString = shellString.Remove(0, 3);
                        shellString = shellString.Trim();
                        shellString = shellString.Remove(0, 2);
                        shellString = shellString.Trim();
                        Console.WriteLine($"Execute Program {shellString}");
                        executeProgram();
                    }
                }

                if (shellString.Contains("coredump"))
                {
                    if (shellString.Contains("-v"))
                    {
                        shellString = shellString.Remove(0, 8);
                        shellString = shellString.Trim();
                        shellString = shellString.Remove(0, 2);
                        shellString = shellString.Trim();
                        Console.WriteLine($"CORE DUMP {shellString}");
                        coreDump();
                    }
                    else
                    {
                        for (int i = 100; i < VM.loaderAddress; i += 6)
                        {
                            Console.WriteLine(VM.MEM[i]);
                        }
                    }
                }

                if (shellString.Contains("execute"))
                {
                    if (shellString.Contains("-v"))
                    {
                        //Removes Execute
                        shellString = shellString.Remove(0, 7);
                        shellString = shellString.Trim();
                        //Removes Verbose
                        shellString = shellString.Remove(0, 2);
                        shellString = shellString.Trim();
                        // check for space here
                        if (shellString.IndexOf(' ') != -1)
                        {
                            // separates time in value from file name
                            int.TryParse(shellString.Substring(shellString.IndexOf(' ') + 1), out timeIn);
                            int i = (shellString.IndexOf(' ') + 1);
                            shellString = shellString.Substring(0, i);
                        }
                        Console.WriteLine($"Execute {shellString}");
                        Console.WriteLine($"Time in {timeIn}");
                        loadProgram(shellString);
                        executeProgram();
                    }
                }

                if (shellString.Contains("errordump"))
                {
                    if (shellString.Contains("-v"))
                    {
                        shellString = shellString.Remove(0, 9);
                        shellString = shellString.Trim();
                        shellString = shellString.Remove(0, 2);
                        shellString = shellString.Trim();
                        if(errors == "")
                        {
                            Console.WriteLine($"ERROR DUMP {shellString} No Errors");
                        }
                        else
                        {
                            Console.WriteLine($"ERROR DUMP {shellString} {errors}");
                        }
                   
                    }
                }

                VM.clock++;

                if (shellString == "stop")
                {
                    break;
                }
            } while (true);
            Console.WriteLine("program has ended press any key to exit");
            Console.ReadKey();
        }
     
        static void loadProgram(string programString)
        {
            if(programString == "")
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
                    }
                }
            }
            catch(FileNotFoundException ex)
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

        static void coreDump()
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
        static void executeProgram()
        {
            VM.dataAddr = VM.datAddr;
            for(int i = 100; i < VM.loaderAddress; i+=6)
            {
                int instruction = VM.MEM[i];
                int destination;
                int source_1;
                int source_2;
                switch (instruction)
                {
                    case (byte)AssemblyInstruction.ADD:
                        destination = (int)VM.MEM[i+1];
                        source_1 = (int)VM.MEM[i+2];
                        source_2 = (int)VM.MEM[i+3];
                        Console.WriteLine($"Adding Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] + VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.SUB:
                        destination = (int)VM.MEM[i+1];
                        source_1 = (int)VM.MEM[i+2];
                        source_2 = (int)VM.MEM[i+3];
                        Console.WriteLine($"Subtracting Contents {VM.registers[source_1]} From Register {source_1} With Contents {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] - VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.MUL:
                        destination = (int)VM.MEM[i+1];
                        source_1 = (int)VM.MEM[i+2];
                        source_2 = (int)VM.MEM[i+3];
                        Console.WriteLine($"Multiplying Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] * VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;
                    case (byte)AssemblyInstruction.DIV:
                        destination = (int)VM.MEM[i+1];
                        source_1 = (int)VM.MEM[i+2];
                        source_2 = (int)VM.MEM[i+3];
                        Console.WriteLine($"Dividing Contents {VM.registers[source_1]} From Register {source_1} With Contents  {VM.registers[source_2]} From Register {source_2} Into Register {destination}");
                        VM.registers[destination] = VM.registers[source_1] / VM.registers[source_2];
                        Console.WriteLine($"Register {destination}'s Contents {VM.registers[destination]}");
                        break;

                    case (byte)AssemblyInstruction.SWI:
                        int kernal = (int)VM.MEM[i + 1];
                        if (kernal == 3)
                        {
                            Console.WriteLine("Running in Kernal Mode");
                        }
                        if (kernal == 4)
                        {
                            Console.WriteLine("Running in User Mode");
                        }

                        break;
                }
            }
        }
    }
}



