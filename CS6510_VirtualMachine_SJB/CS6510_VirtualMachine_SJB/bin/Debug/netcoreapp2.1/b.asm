plus	.BYTE	'+'
minus	.BYTE	'-'
equal	.BYTE	'='
times	.BYTE	'*'
divide	.BYTE	'/'

; print 7+2=?
	MVI	R2 7
	MVI	R0 7
	SWI	3

	ADR	R3 plus
	LDRB	R0 [R3]
	SWI	3

	MVI	R1 2
	MVI	R0 2
	SWI	4

	ADR	R3 equal
	LDRB	R0 [R3]
	SWI	3

	ADD	R0 R1 R2
	SWI	4

	MVI	R0 10
	SWI	2

; print 7*2=?
	MVI	R2 7
	MVI	R0 7
	SWI	2

	ADR	R3 times
	LDRB	R0 [R3]
	SWI	2

	MVI	R1 2
	MVI	R0 2
	SWI	3

	ADR	R3 equal
	LDRB	R0 [R3]
	SWI	2

	MUL	R0 R1 R2
	SWI	2

	MVI	R0 10
	SWI	4

	DIV R2 R4 R5
	SWI	2

	SUB R5 R1 R2
	SWI 3

; terminate
	SWI	0
