.MODEL SMALL
.STACK 100h
.DATA
.CODE
START:
	mov ah, 00h			;������������� COM-�����
	mov al, 11100011b
	mov dx, 0
	int 14h	
stop:
	cmp al,1bh
	jne are
	mov ah,4ch
	int 21h
are:
	mov ah, 02h			; ��������� ����, ���������� ���������� � ������ ��� ������
	mov dx, 0
	int 14h
	xor ax, ax
	mov ah, 01h			;�������� ������ ����� ��������� ���� � ������� ��� �� �����
	mov dx, 0
	int 21h
	int 14h
	cmp al, 1bh
	je stop
more: 					;��������� ��������� �� ���������� ����
	mov ah, 03h 			;�������� ������ �����
	mov dx, 0
	int 14h
	and ah, 00100000b 			;������ ��������� � ��� �������� ���� ��������
	cmp ah, 00100000b			;���������� ������� ��� � ��������
	je are				;���� ������� ��� � �l=1, �� ��������� �� �����
	jmp more	
END START
