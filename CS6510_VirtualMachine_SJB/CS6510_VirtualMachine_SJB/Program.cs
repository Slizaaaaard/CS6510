using System;
using System.IO;
using System.Text;
namespace CS6510_VirtualMachine_SJB
{
    class Program
    {
        static VirtualMachine VM = new VirtualMachine();
        static void Main(string[] args)
        {

            do
            {
                loadProgram();
                executeProgram();
                Console.WriteLine("\n");
            } while (Console.ReadLine() != "s");
            Console.ReadKey();
        }
     
        static void loadProgram()
        {
            using (FileStream fs = new FileStream("cpuBound.osx", FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs, new ASCIIEncoding()))
                {

                    int bSize = br.ReadInt32();
                    VM.PC = br.ReadInt32();
                    VM.loaderAddress = br.ReadInt32();
                    VM.fetchAddr = VM.loaderAddress;
                    VM.dataAddr = VM.loaderAddress;
                    VM.datAddr = VM.loaderAddress;  
                    Console.WriteLine(bSize);
                    Console.WriteLine(VM.PC);
                    Console.WriteLine(VM.loaderAddress);

                    for (int i = 0; i < VM.PC; i++)
                    {
                        VM.MEM[i] = br.ReadByte();
                        Console.WriteLine(VM.MEM[i]);
                    }
                    Console.WriteLine("end");
                    for (int i = VM.PC; i < bSize; i += 6)
                    {
                        byte Byte = br.ReadByte();
                        Console.WriteLine(Byte);
                        switch (Byte)
                        {
                            case (byte)AssemblyInstruction.ADD:
                                Console.WriteLine("\nAdd");
                                VM.MEM[VM.loaderAddress++] = Byte;
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                br.ReadByte();
                                br.ReadByte();
                                break;
                            case (byte)AssemblyInstruction.SUB:
                                Console.WriteLine("\nSubtract");
                                VM.MEM[VM.loaderAddress++] = Byte;
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                br.ReadByte();
                                br.ReadByte();
                                break;
                            case (byte)AssemblyInstruction.MUL:
                                Console.WriteLine("\nMultiply");
                                VM.MEM[VM.loaderAddress++] = Byte;
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                br.ReadByte();
                                br.ReadByte();
                                break;
                            case (byte)AssemblyInstruction.DIV:
                                Console.WriteLine("\nDivide");
                                VM.MEM[VM.loaderAddress++] = Byte;
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                VM.MEM[VM.loaderAddress++] = br.ReadByte();
                                br.ReadByte();
                                br.ReadByte();
                                break;
                        }

                    }
                }
            }
        }
        static void executeProgram()
        {
            VM.dataAddr = VM.datAddr;
            for(int i = VM.fetchAddr; i < VM.fetchAddr+VM.instructionPC; i++)
            {
                int instruction = VM.MEM[i++];
                int destination;
                int source_1;
                int source_2;
                switch (instruction)
                {
                    case (byte)AssemblyInstruction.ADD:
                        destination = (int)VM.MEM[i++];
                        source_1 = (int)VM.MEM[i++];
                        source_2 = (int)VM.MEM[i++];
                        VM.registers[destination] = VM.registers[source_1] + VM.registers[source_2];
                        i--;
                        break;
                    case (byte)AssemblyInstruction.SUB:
                        destination = (int)VM.MEM[i++];
                        source_1 = (int)VM.MEM[i++];
                        source_2 = (int)VM.MEM[i++];
                        VM.registers[destination] = VM.registers[source_1] - VM.registers[source_2];
                        i--;
                        break;
                    case (byte)AssemblyInstruction.MUL:
                        destination = (int)VM.MEM[i++];
                        source_1 = (int)VM.MEM[i++];
                        source_2 = (int)VM.MEM[i++];
                        VM.registers[destination] = VM.registers[source_1] * VM.registers[source_2];
                        i--;
                        break;
                    case (byte)AssemblyInstruction.DIV:
                        destination = (int)VM.MEM[i++];
                        source_1 = (int)VM.MEM[i++];
                        source_2 = (int)VM.MEM[i++];
                        VM.registers[destination] = VM.registers[source_1] / VM.registers[source_2];
                        i--;
                        break;
                }
            }
        }
    }
}



