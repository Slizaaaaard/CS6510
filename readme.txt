#Download and Extract Zip Folder

#Create an osx file using math.asm
Open Command prompt
Open to path C:\CS6510_VirtualMachine_SJB\CS6510_VirtualMachine_SJB\bin\Debug\netcoreapp2.1
In Command Prompt type: osx math.asm 100

#Change shell
Open path C:\CS6510_VirtualMachine_SJB in file explorer.
Open CS6510_VirtualMachine_SJB.sln in visual studio.
In console app type in myvm to switch to myvm shell.
In console app type in vm to switch to vm shell.

#Test core dump
In console app coredump -v math.osx to show nothing in memory
in console app type load -v math.osx
in console app type coredump -v math.osx

#Test error dump
In console app type errordump -v math.osx to show no errors
in console app type load -v anyfile.osx
in console app type errordump -v anyfile.osx to show errors;

#Test load and run
in console app type load -v math.osx
in console app type run -v math.osx
