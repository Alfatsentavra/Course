NAME Summa
EXTRN	data   (summ_1, summ_2, summ_3, summ_4)
PUBLIC	summa
summ_ROUTINES SEGMENT CODE
RSEG summ_ROUTINES
JMP summa
summa:
	;складываем младшие байты
	MOV A, R1
	ADD A, R2
	MOV R0, #summ_1
	MOV @R0, A
	;складываем средние байты
	MOV A, R3
	ADDC A, R4
	MOV R0, #summ_2
	MOV @R0, A
  	;складываем следующий байт	
	MOV A, R5
	ADDC A, R6
	MOV R0, #summ_3
	MOV @R0, A
  ;складываем старший байт второго числа с переносом от предыдущего сложения
	MOV A, R7
	ADDC A, #0
	MOV R0, #summ_4
	MOV @R0, A
RET
END 




































































































































































































































