.MODEL SMALL
.STACK 100h
.DATA
.CODE
START:
	mov ah, 00h			; ������� ������������� �����
	mov dx, 0			; ����� ����� - 0
	mov al, 11100011b	; �������� 9600, ��� ���� ��������, 1 ����-���, ����� 8 ���
	int 14h
stop:
	cmp al, 1bh
	jne proverka
	mov ah, 4ch
	int 21h
proverka:
	mov ah, 03h ; �������� ������ �����
	mov dx, 0
	int 14h
	and ah, 00000000b	; ���������, ������ �� ����
	cmp ah, 00000000b
	jne proverka	; ���� �� ������, �� ���� ������
	mov ah, 02h	; ��������� ������
	mov dx, 0
	int 14h
	mov ah, 02h	; ������� ��� �� �����
	mov dl, al	
	int 21h
	cmp al, 1Bh
	je stop
END START
