name Sub
main segment code;���������� �������� ������ ��������
cseg at 0;�������� ������� ������ � ������� 0h
jmp start;�� ������ ���������
using 0;������������ 0 ���� ���������
rseg main;����� �������� main
start: MOV DPTR, #98H
MOVX A, @DPTR
SUBB A, #5
MOV R1, #78H
MOV @R1, A
CLR A
ADDC A, #0
MOV R0, #79H
MOV @R0, A
JMP start
END 
