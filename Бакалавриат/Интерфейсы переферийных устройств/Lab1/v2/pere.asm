.MODEL SMALL
.STACK 100h
.DATA
.CODE
START:
	mov ah, 00h			;инициализация COM-порта
	mov al, 11100011b
	mov dx, 0
	int 14h	
stop:
	cmp al,1bh
	jne are
	mov ah,4ch
	int 21h
are:
	mov ah, 02h			; принимаем байт, переданный приемником в случае его ошибки
	mov dx, 0
	int 14h
	xor ax, ax
	mov ah, 01h			;посылаем символ через выбранный порт и выводим его на экран
	mov dx, 0
	int 21h
	int 14h
	cmp al, 1bh
	je stop
more: 					;проверяем отправлен ли предыдущий байт
	mov ah, 03h 			;получаем статус порта
	mov dx, 0
	int 14h
	and ah, 00100000b 			;делаем побитовое и для младшего бита регистра
	cmp ah, 00100000b			;сравниваем младший бит в регистре
	je are				;если младший бит в аl=1, то переходим по метке
	jmp more	
END START
