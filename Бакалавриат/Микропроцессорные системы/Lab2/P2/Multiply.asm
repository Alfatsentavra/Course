NAME Mull
PUBLIC  Mull
EXTRN data (mult_1, mult_2, mult_3, mult_4) 
BITVAR	SEGMENT	data
RSEG  BITVAR
Mull_ROUTINES SEGMENT CODE
RSEG Mull_ROUTINES
JMP Mull
Mull:
;умножаем младший байт первого числа на байт второго и записываем результат во временную €чейку 
	MOV A, R3
	MUL AB
	MOV R0, #mult_1
	MOV @R0, A
	MOV A, B
	MOV R0, A
;умножаем средний байт первого числа на байт второго и записываем результат во временную €чейку	
	MOV A, R2
	MOV B, R4
	MUL AB
	ADD A, R0
	MOV R0, #mult_2
	MOV @R0, A
	MOV A, B
	ADDC A, #0
	MOV R0, A
;умножаем старший байт первого числа на байт второго и записываем результат во временную €чейку	
	MOV A, R1
	MOV B, R4
	MUL AB
	ADDC A, R0
	MOV R0, #mult_3
	MOV @R0, A
	MOV A, B
	ADDC A, #0
	MOV R0, #mult_4
	MOV @R0, A
RET
end































































































































































































































