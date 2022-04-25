using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CS6510_VirtualMachine_SJB
{
    class Load
    {

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
                        VM.fp.newQueue[VM.PIDCount] = newProcess;
                        newProcess.processState = (int)ProcessStateEnum.newProcess;


                        int bSize = br.ReadInt32();
              
                        VM.PC = br.ReadInt32();
                 
                        VM.fetchAddr = VM.loaderAddress;
                        VM.dataAddr = VM.loaderAddress;
                        VM.datAddr = VM.loaderAddress;
                        //Console.WriteLine($"bSize {bSize}");
                        //Console.WriteLine($"PC {VM.PC}");
                        //Console.WriteLine($"Loader Address {VM.loaderAddress}");
                        VM.fp.newQueue[newProcess.PID].startPC = VM.loaderAddress;
                        VM.fp.newQueue[newProcess.PID].startSection = VM.loaderAddress;

                        for (int i = VM.fp.newQueue[newProcess.PID].startPC; i < VM.PC; i++)
                        {
                            VM.MEM[i] = br.ReadByte();

                        }

                        VM.fp.newQueue[newProcess.PID].length = bSize;
                        int tempLoader = VM.loaderAddress;
                        for (
                            int i = VM.PC + tempLoader; i < bSize + tempLoader + VM.PC; i += 6)
                        {
                            byte Byte = br.ReadByte();
                            //Console.WriteLine(Byte + " " + i);
                            switch (Byte)
                            {
                                case (byte)AssemblyInstruction.ADD:
                                    //Console.WriteLine("Add Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.SUB:
                                    //Console.WriteLine("Subtract Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.MUL:
                                    //Console.WriteLine("Multiply Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;
                                case (byte)AssemblyInstruction.DIV:
                                    //Console.WriteLine("Divide Loaded Into Memory");
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
                                    //Console.WriteLine("Switch Loaded Into Memory");
                                    loadExpression(VM, br, Byte);
                                    break;

                            }

                        }
                        newProcess.timeIn = VM.clock;
                        VM.fp.readyQueue[VM.PIDCount] = newProcess;
                        VM.fp.readyQueue[VM.PIDCount].endPC = VM.loaderAddress;

                        newProcess.processState = (int)ProcessStateEnum.ready;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                VM.errors = ex.ToString();
            }
            VM.PIDCount++;
        }

        static void loadExpression(VirtualMachine VM, BinaryReader br, byte Byte)
        {

            List<int> temp = new List<int>();
            temp.Add(Byte);
            VM.MEM[VM.loaderAddress++] = Byte;

            Byte = br.ReadByte();
            temp.Add(Byte);
            VM.MEM[VM.loaderAddress++] = Byte;

            Byte = br.ReadByte();
            temp.Add(Byte);
            VM.MEM[VM.loaderAddress++] = Byte;

            Byte = br.ReadByte();
            temp.Add(Byte);
            VM.MEM[VM.loaderAddress++] = Byte;

            Byte = br.ReadByte();
            temp.Add(Byte);
            VM.MEM[VM.loaderAddress++] = Byte;

            Byte = br.ReadByte();
            temp.Add(Byte);
            VM.MEM[VM.loaderAddress++] = Byte;

            VM.page.add(temp.ToArray());
        }

  

    }
}
