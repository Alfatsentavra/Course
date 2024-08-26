format mz

BASE equ 3F8h			; Базовый адрес порта

cli				; Запрет маскируемых прерываний

in al, 21h			; Запрет аппаратных прерываний (IRQ4)
and al, 11101111b
out 21h, al

mov al, 0Ch			; Получение вектора прерываний
mov ah, 35h
int 21h

push es 			; Сохраняем адрес исходного обработчика
push bx 			; Сохраняем смещение

mov ax, cs			; Замена исходного обработчика прерывания нашим
mov ds, ax			; cs - адрес кодового сегмента нашей программы
mov dx, w			; w - смещение нашего обработчика
mov al, 0Ch
mov ah, 25h
int 21h

in al, 21h			; Разрешение аппаратных прерываний
and al, 11111111b
out 21h, al

sti				; Разрешение маскируемых прерываний

mov ax, 10000000b		; Инициализация порта
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

t:				; Главный цикл программы (бесконечный)
	jmp t

w:				; Наш обработчик прерывания


    xor al, al 
    mov dx, 3FAh
	in al, dx;
	and al, 00000010b
	jz t



	mov ah, 00h		; Вводим символ с клавиатуры
	int 16h
 
	mov dx, BASE		; От
	out dx, al
	
	
	 
  
	
	
	

	cmp al, 1Bh		; Проверка: нажат ли ESC
	je exit 		; Если да -- переход к exit

	mov ah, 0Eh		; Вывод символа на экран
	mov dl, al
	int 10h

	mov al, 20h		; Сообщаем процессору, что прерывание обработано
	out 20h, al

	iret			; Возвращаем управление процессору

exit:
	pop dx			; Восстанавливаем исходный обработчик прерывания
	pop ds
	mov al, 0Ch
	mov ah, 25h
	int 21h