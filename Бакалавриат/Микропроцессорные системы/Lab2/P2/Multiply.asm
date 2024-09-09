NAME Mull
PUBLIC  Mull
EXTRN data (mult_1, mult_2, mult_3, mult_4) 
BITVAR	SEGMENT	data
RSEG  BITVAR
Mull_ROUTINES SEGMENT CODE
RSEG Mull_ROUTINES
JMP Mull
Mull:
;�������� ������� ���� ������� ����� �� ���� ������� � ���������� ��������� �� ��������� ������ 
	MOV A, R3
	MUL AB
	MOV R0, #mult_1
	MOV @R0, A
	MOV A, B
	MOV R0, A
;�������� ������� ���� ������� ����� �� ���� ������� � ���������� ��������� �� ��������� ������	
	MOV A, R2
	MOV B, R4
	MUL AB
	ADD A, R0
	MOV R0, #mult_2
	MOV @R0, A
	MOV A, B
	ADDC A, #0
	MOV R0, A
;�������� ������� ���� ������� ����� �� ���� ������� � ���������� ��������� �� ��������� ������	
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































































































































































































































