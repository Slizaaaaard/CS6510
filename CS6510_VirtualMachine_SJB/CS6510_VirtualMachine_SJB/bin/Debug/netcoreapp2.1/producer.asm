; Shared memory producer

SHM	.BYTE 'S'
	.BYTE 'H'
	.BYTE 'A'
	.BYTE 'R'
	.BYTE 'E'
	.BYTE 'D'

;;; Get the shared message pointer ;;;
	; Load the address of the shared memory name in R0
	ADR	R0 SHM
	; Get the shared memory pointer in R0
	SWI	80
	SWI	80
	SWI	80
	SWI	80
	SWI	80
	SWI	80
	SWI	80
	SWI	80
	MOV	R1 R0

;;; Print the message from the shared memory pointer ;;;
	; Move a 1 into R3 for incrementing the MSG pointer
	MVI	R3 1
	; Print the message
PRINTF	LDRB	R0 [R1] ; Load the char in SHM into R0
	SWI	10 ; Print the char in R0 to the stdout buffer
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	LDRB	Z [R1] ; Load the char in MSG into the Z register
	BNE	PRINTF ; Branch to PRINTF if Z != 0
	SWI	11 ; Flush stdout

;;; Release the shared memory ;;;
	ADR	R0 SHM


; terminate
OUT	SWI	100
