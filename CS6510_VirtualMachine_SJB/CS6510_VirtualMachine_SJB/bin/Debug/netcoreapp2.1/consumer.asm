; Shared memory producer

SHM	.BYTE 'S'
	.BYTE 'H'
	.BYTE 'A'
	.BYTE 'R'
	.BYTE 'E'
	.BYTE 'D'

MSG	.BYTE 'H'
	.BYTE 'e'
	.BYTE 'l'
	.BYTE 'l'
	.BYTE 'o'
	.BYTE \32
	.BYTE 'w'
	.BYTE 'o'
	.BYTE 'r'
	.BYTE 'l'
	.BYTE 'd'

;;; Get the shared message pointer ;;;
	; Load the address of the shared memory name in R0
	ADR	R0 SHM
	; Get the shared memory pointer in R0
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	SWI	81
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	SWI	81
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	SWI	81
	SWI	81
	SWI	81
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	SWI	81
	SWI	81
	SWI	81
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	SWI	81
	SWI	81

;;; Load the message into the shared memory pointer ;;;
	; Load MSG pointer into R1
	ADR	R1 MSG
	; Move a 1 into R3 for incrementing the MSG pointer
	MVI	R3 1
	; Print the message
FPRINTF	LDRB	R2 [R1] ; Load the char in MSG into R2
	STRB	R2 [R0] ; Store the char in R2 into the Shared Memory pointer
	ADD	R1 R1 R3 ; Increment the pointer to MSG
	ADD	R0 R0 R3 ; Increment the store pointer to Shared Memory
	LDRB	Z [R1] ; Load the char in MSG into the Z register
	BNE	FPRINTF ; Branch to FPRINTF if Z != 0
	MVI	R2 0
	STRB	R2 [R0] ; Store a NULL terminating character in the Shared Memory pointer

; terminate
OUT	SWI	100
