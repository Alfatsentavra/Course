format mz

BASE equ 3F8h			; ������� ����� �����

cli				; ������ ����������� ����������

in al, 21h			; ������ ���������� ���������� (IRQ4)
and al, 11101111b
out 21h, al

mov al, 0Ch			; ��������� ������� ����������
mov ah, 35h
int 21h

push es 			; ��������� ����� ��������� �����������
push bx 			; ��������� ��������

mov ax, cs			; ������ ��������� ����������� ���������� �����
mov ds, ax			; cs - ����� �������� �������� ����� ���������
mov dx, w			; w - �������� ������ �����������
mov al, 0Ch
mov ah, 25h
int 21h

in al, 21h			; ���������� ���������� ����������
and al, 11111111b
out 21h, al

sti				; ���������� ����������� ����������

mov ax, 10000000b		; ������������� �����
mov dx, BASE+3
out dx, al
mov ax, 00h
mov dx, BASE+1
out dx, al
mov ax, 0Ch
mov dx, BASE
out dx, al
mov ax, 00111011b
mov dx, BASE+3
out dx, al
mov ax, 00000010b
mov dx, BASE+1
out dx, al

t:				; ������� ���� ��������� (�����������)
	jmp t

w:				; ��� ���������� ����������


    xor al, al 
    mov dx, 3FAh
	in al, dx;
	and al, 00000010b
	jz t



	mov ah, 00h		; ������ ������ � ����������
	int 16h
 
	mov dx, BASE		; ��
	out dx, al
	
	
	 
  
	
	
	

	cmp al, 1Bh		; ��������: ����� �� ESC
	je exit 		; ���� �� -- ������� � exit

	mov ah, 0Eh		; ����� ������� �� �����
	mov dl, al
	int 10h

	mov al, 20h		; �������� ����������, ��� ���������� ����������
	out 20h, al

	iret			; ���������� ���������� ����������

exit:
	pop dx			; ��������������� �������� ���������� ����������
	pop ds
	mov al, 0Ch
	mov ah, 25h
	int 21h