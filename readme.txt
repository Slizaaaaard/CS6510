#Download and Extract Zip Folder

##Create an osx file using math.asm
#Open Command prompt
#Open to path C:\CS6510_VirtualMachine_SJB\CS6510_VirtualMachine_SJB\bin\Debug\netcoreapp2.1
#In Command Prompt type: osx math.asm 100
#Pass Case:
#In path C:\CS6510_VirtualMachine_SJB\CS6510_VirtualMachine_SJB\bin\Debug\netcoreapp2.1 
#Pass Case:
#You should have a file named
math.osx

###MileStone 1 Acceptance Testing

##Change shell
#Open path C:\CS6510_VirtualMachine_SJB in file explorer.
#Open CS6510_VirtualMachine_SJB.sln in visual studio.
In console app type in myvm to switch to myvm shell.
Pass Case:
VM->myvm
MYVM->

#In console app type in vm to switch to vm shell.
#Pass Case:
MYVM->vm
VM->

##Test core dump
In console app coredump -v math.osx to show nothing in memory
#Pass Case:
VM->coredump -v math.osx
CORE DUMP math.osx
VM->

#in console app type load -v math.osx
#in console app type coredump -v math.osx
#Pass Case:
VM->coredump -v math.osx
CORE DUMP math.osx
MVI
MVI
SWI Instruction 3
ADR
LDRB
SWI Instruction 3
MVI
MVI
SWI Instruction 4
ADR
LDRB
SWI Instruction 3
Add Register 1 With Register 2 Into Register 0
SWI Instruction 4
MVI
SWI Instruction 2
MVI
MVI
SWI Instruction 1
ADR
LDRB
SWI Instruction 2
MVI
MVI
SWI Instruction 3
ADR
LDRB
SWI Instruction 2
MUL Register 1 With Register 2 Into Register 0
SWI Instruction 1
MVI
SWI Instruction 4
DIV  Register 4 With Register 5 Into Register 2
VM->

##Test error dump
In console app type errordump -v math.osx to show no errors
Pass Case:
VM->errordump -v math.osx
ERROR DUMP math.osx
VM->

#in console app type load -v anyfile.osx
#in console app type errordump -v anyfile.osx to show errors;
Pass Case:
VM->load -v anyfile.osx
VM->errordump -v anyfile.osx
ERROR DUMP anyfile.osx System.IO.FileNotFoundException: Could not find file 'C:\Users\10784099\Desktop\School\CS6510\CS6510_VirtualMachine_SJB\CS6510_VirtualMachine_SJB\bin\Debug\netcoreapp2.1\anyfile.osx'.
File name: 'C:\Users\10784099\Desktop\School\CS6510\CS6510_VirtualMachine_SJB\CS6510_VirtualMachine_SJB\bin\Debug\netcoreapp2.1\anyfile.osx'
   at System.IO.FileStream.ValidateFileHandle(SafeFileHandle fileHandle)
   at System.IO.FileStream.CreateFileOpenHandle(FileMode mode, FileShare share, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access)
   at CS6510_VirtualMachine_SJB.Load.loadProgram(VirtualMachine VM, String programString) in C:\Users\10784099\Desktop\School\CS6510\CS6510_VirtualMachine_SJB\CS6510_VirtualMachine_SJB\Memory\Load.cs:line 19
VM->

##Test load and run
in console app type load -v math.osx
Pass Case:
VM->load -v math.osx
bSize 227
PC 5
Loader Address 0
Switch Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Add Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Multiply Loaded Into Memory
Switch Loaded Into Memory
Switch Loaded Into Memory
Divide Loaded Into Memory
VM->

#in console app type run -v math.osx
#Pass Case:
VM->run -v math.osx
Execute Program math.osx
Running in Kernal Mode
Running in Kernal Mode
Running in User Mode
Running in Kernal Mode
Adding Contents 2 From Register 1 With Contents  3 From Register 2 Into Register 0
Register 0's Contents 5
Running in User Mode
Standard Output
Waiting for User Input
dStandard Output
Running in Kernal Mode
Standard Output
Multiplying Contents 2 From Register 1 With Contents  3 From Register 2 Into Register 0
Register 0's Contents 6
Waiting for User Input
dRunning in User Mode
Dividing Contents 5 From Register 4 With Contents  6 From Register 5 Into Register 2
Register 2's Contents 0
VM->

###MileStone 2 Acceptance Testing

