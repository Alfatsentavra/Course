NAME LAB1_5    
EXTRN CODE (Delay)
MAIN SEGMENT CODE
CSEG AT 0
CALL Main
CSEG AT 001BH
JMP Timer
USING 0
; �������� ���������
RSEG MAIN
Main:
CALL Delay 		; ����� ������������ ������� �������
MainCode: 			;���������� �������� ��������� �� ����� ��������
CJNE A , #00h, loop1
JMP MainCode
loop1:
JMP Main
Timer:
	DJNZ R3, resetTimer	; ��������� �������, ���� �� �� ������ �����	
				; ���� ������� ���� �����, �� ��������� ������
	CLR TR1		; ������������� ������
	CLR IE.7	 	  ; ��������� ����������
	CLR ET1		; ��������� ���������� �� Timer 1
	CLR PT1		; ������� ��������� ��������� ���������� �� Timer 
	mov a, #05h	
	resetTimer:
RETI			; ������� �� ����������
END




















































































































































