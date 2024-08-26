.MODEL SMALL
.STACK 100h
.DATA
.CODE
START:
	mov ah, 00h			; функция инициализации порта
	mov dx, 0			; номер порта - 0
	mov al, 11100011b	; скорость 9600, нет кода четности, 1 стоп-бит, слово 8 бит
	int 14h
stop:
	cmp al, 1bh
	jne proverka
	mov ah, 4ch
	int 21h
proverka:
	mov ah, 03h ; получаем статус порта
	mov dx, 0
	int 14h
	and ah, 00000000b	; проверяем, пришел ли байт
	cmp ah, 00000000b
	jne proverka	; если не пришёл, то ждем дальше
	mov ah, 02h	; принимаем символ
	mov dx, 0
	int 14h
	mov ah, 02h	; выводим его на экран
	mov dl, al	
	int 21h
	cmp al, 1Bh
	je stop
END START
