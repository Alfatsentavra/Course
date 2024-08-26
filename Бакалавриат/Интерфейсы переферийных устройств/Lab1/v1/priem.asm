.MODEL SMALL
.STACK 100h

.DATA
.CODE
START:
    	mov ax, 80h
	mov dx, 3fbh
	out dx,al
	mov ax, 0Ch ; Заносим младший байт скорости 9600 bit/s 
	mov dx, 3f8h ;Заносим в dx адрес регистра 3f8h
	out dx, al ;Передаем байт по заданному адресу
	mov al, 0 ;заносим старший байт скорости
	mov dx, 3f9h ;заносим в dx адрес регистра 3f9h
	out dx, al ;передаем байт по заданному адресу
	mov ax, 3bh ;передаем байт настройки(8 бит данных, бит паритета отсутствует, DLab=0,1 стоп-бит)
	mov dx, 3fbh ;заносим в dx адрес регистра управления линией 3fbh
	out dx, al ;передаем байт по заданному адресу
stop:
	cmp al, 1bh
	jne waitread
	mov ah,4ch
	int 21h
waitread:
	xor al, al
	mov dx, 3fdh ;заносим в dx адрес регистра статуса
	in al, dx ;читаем из регистра статуса
	and al, 1 ;делаем побитовое и для младшего бита регистра
	cmp al, 1 ;сравниваем младший бит в регистре
	jne waitread ;если младший бит в аl=0, то переходим по метке
	xor al, al ;
	mov dx, 3f8h ;заносим в регистр dx адрес f8h
	in al, dx ;читаем принятый байт
	cmp al, 1Bh
	je stop
	mov ah, 02h
	mov dx, ax
	int 21h
	jmp waitread
END START