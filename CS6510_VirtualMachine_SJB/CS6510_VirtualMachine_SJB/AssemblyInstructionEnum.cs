using System;
using System.Collections.Generic;
using System.Text;

namespace CS6510_VirtualMachine_SJB
{
 public enum AssemblyInstruction
    {
        ADD = 16,
        SUB = 17,
        MUL = 18,
        DIV = 19,
        MOV = 1,
        MVI = 22,
        ADR = 0,
        STR = 2,
        STRB = 3,
        LDR = 4,
        LDRB = 5,
        B = 7,
        BX = 6,
        BNE = 8,
        BGT = 9,
        BLT = 10,
        BEQ = 11, 
        CMP = 12,
        AND = 13,
        ORR = 14,
        EOR = 15, 
        SWI = 20,
        sixtyNine = 69
    }
}
