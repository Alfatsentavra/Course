NAME SUM ; �������� ���������
MAIN SEGMENT CODE ; ���������� �������� ������ ��������
CSEG AT 0 ; ����������� ����������� �������� ������ � ������� 0h
JMP start ; ������� �� ������ ���������
USING 0 ; ������������� 0 ���� ���������
RSEG MAIN
start:
MOV R0, #60H;                                                                              
MOV A, @R0;
ADD A, #5;
MOV DPTR, #60H
MOVX @DPTR, A
CLR A
ADDC A, #0
MOV DPTR, #61H                                                           
MOVX @DPTR, A            
JMP start;
END
