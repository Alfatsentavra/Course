NAME Delay
PUBLIC	Delay
Delay SEGMENT CODE
RSEG Delay
	MOV R3, #068H 
	MOV TH1, #039H
	MOV TL1, #000H				; ������������� ����� �� �������.
	SETB IE.7					; ��������� ����������
	SETB ET1					; ��������� ���������� �� Timer 1
	SETB PT1		; ������ ���������� �� Timer 1 ��������� ���������
	MOV TMOD, #00010001B		; Timer 1: ����� 1 (16-������ ������)
	SETB TR1						; ��������� ������
RET
END 






























































































