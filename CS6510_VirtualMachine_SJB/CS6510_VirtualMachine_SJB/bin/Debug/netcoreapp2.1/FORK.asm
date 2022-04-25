PRG1    .BYTE 'E'
    .BYTE 'X'
    .BYTE 'E'
    .BYTE 'C'
    .BYTE '.'
    .BYTE 'o'
    .BYTE 's'
    .BYTE 'x'

; ADD 7 + 2 a bunch of times
    MVI     R2 7
    MVI     R1 2
    ADD     R0 R1 R2
    MVI     R2 7
    MVI     R1 2
	SWI 40
    SWI 50
	SWI 60
    ADD     R0 R1 R2
    MVI     R2 7
    MVI     R1 2
    ADD     R0 R1 R2
    MVI     R2 7
    MVI     R1 2
    ADD     R0 R1 R2


; terminate
OUT SWI 100