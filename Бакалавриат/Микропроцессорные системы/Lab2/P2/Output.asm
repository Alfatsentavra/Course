NAME Output

PUBLIC	Output 
Output_ROUTINES SEGMENT CODE
RSEG Output_ROUTINES
JMP Output
Output:
	JNB  TI,$		; ����, ���� �������� ���� ���������� �����������
	CLR  TI			; ������� ���� ���������� �����������
	MOV  SBUF,A		; ���������� ���� � ����� ��������
	mov A, #0 
RET
END








































































































































