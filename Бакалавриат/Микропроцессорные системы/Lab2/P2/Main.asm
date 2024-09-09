NAME MainCode
EXTRN 	CODE 	(MainCode, Mull, Summa, Output)
EXTRN data (mult_1, mult_2, mult_3, mult_4, summ_1, summ_2, summ_3, summ_4, addr1, addr2, addr3, multres_1_1, multres_1_2, multres_1_3, multres_1_4, multres_2_1, multres_2_2, multres_2_3, multres_2_4, multres_3_1, multres_3_2, multres_3_3, multres_3_4, multread_1, multread_2, multread_3, multread_4, output_1, output_2, output_3, output_4, output_5, output_6)
MAIN SEGMENT CODE
CSEG AT 0
PROG	SEGMENT	CODE
CONST	SEGMENT	CODE
STACK	SEGMENT	IDATA
RSEG  STACK
	DS    10H 
	CSEG  AT   0
	USING	0  
JMP start
;RSEG  PROG
start:
; Загружаем данные из внешней памяти
MOV DPTR, #addr1
	MOVX A, @DPTR	
	MOV R1, A			
	MOV DPTR, #addr2	
	MOVX A, @DPTR
	MOV R2, A	
	MOV DPTR, #addr3
	MOVX A, @DPTR
	MOV R3, A	
; Получаем 3 байта из порта 2, от старшего к младшему
	MOV R5, P2
	MOV R6, P2
	MOV R7, P2
; Переносим  младший байт второго числа в регистры
	MOV B, R7
	MOV R4, B
	CALL Mull
	;переносим результат из временных ячеек
   MOV R0, #multread_1
	MOV A, @R0
	MOV R0, #multres_1_1
	MOV @R0, A
	MOV R0, #multread_2
	MOV A, @R0
	MOV R0, #multres_1_2
	MOV @R0, A
	MOV R0, #multread_3
	MOV A, @R0
	MOV R0, #multres_1_3
	MOV @R0, A
	MOV R0, #multread_4
	MOV A, @R0
	MOV R0, #multres_1_4
	MOV @R0, A
	; Переносим  средний байт второго числа в регистры
	MOV B, R6
	MOV R4, B
	CALL Mull				;вызываем подпрограмму умножения
	;переносим результат из временных ячеек
	MOV R0, #multread_1
	MOV A, @R0
	MOV R0, #multres_2_1
	MOV @R0, A
	MOV R0, #multread_2
	MOV A, @R0
	MOV R0, #multres_2_2
	MOV @R0, A
	MOV R0, #multread_3
	MOV A, @R0
	MOV R0, #multres_2_3
	MOV @R0, A
	MOV R0, #multread_4
	MOV A, @R0
	MOV R0, #multres_2_4
	MOV @R0, A
	; Переносим  старший  байт второго числа в регистры
	MOV B, R5
	MOV R4, B
	CALL Mull	;вызываем подпрограмму умножения
	;переносим результат из временных ячеек
	MOV R0, #multread_1
	MOV A, @R0
	MOV R0, #multres_3_1
	MOV @R0, A
	MOV R0, #multread_2
	MOV A, @R0
	MOV R0, #multres_3_2
	MOV @R0, A
	MOV R0, #multread_3
	MOV A, @R0
	MOV R0, #multres_3_3
	MOV @R0, A
	MOV R0, #multread_4
	MOV A, @R0
	MOV R0, #multres_3_4
	MOV @R0, A
	;записываем результат первого и второго умножения в регистры
	MOV R0, #multres_1_3
   MOV A, @R0
   MOV R1, A
   MOV R0, #multres_1_2
   MOV A, @R0
   MOV R3, A
   MOV R0, #multres_1_1
   MOV A, @R0
   MOV R5, A
   MOV R0, #multres_2_4
   MOV A, @R0
   MOV R2, A
   MOV R0, #multres_2_3
   MOV A, @R0
   MOV R4, A
   MOV R0, #multres_2_2
   MOV A, @R0
   MOV R6, A
   MOV R0, #multres_2_1
   MOV A, @R0
   MOV R7, A
;записываем младший байт конечного результата
   MOV R0, #multres_1_4
   MOV A, @R0
   MOV R0, #output_1
   MOV @R0, A
   CALL Summa;вызываем подпрограмму сложения
;записываем следующий байт конечного результата
   MOV R0, #summ_1
   MOV A, @R0
   MOV R0, #output_2
   MOV @R0, A	
;записываем результат суммирования и третьего умножения в регистры
MOV R0, #multres_3_4
   MOV A, @R0
   MOV R1, A
   MOV R0, #multres_3_3
   MOV A, @R0
   MOV R3, A
   MOV R0, #multres_3_2
   MOV A, @R0
   MOV R5, A

   MOV R0, #multres_3_1 
   MOV A, @R0
   MOV R7, A  
   MOV R0, #summ_2
   MOV A, @R0
   MOV R2, A    
   MOV R0, #summ_3
   MOV A, @R0
   MOV R4, A
   MOV R0, #summ_4
   MOV A, @R0
   MOV R6, A	
	CALL Summa;вызываем подпрограмму сложения
   ;записываем байты конечного результата	
   MOV R0, #summ_4
   MOV A, @R0
   MOV R0, #output_6
   MOV @R0, A
   MOV R0, #summ_3
   MOV A, @R0
   MOV R0, #output_5
   MOV @R0, A
   MOV R0, #summ_2
   MOV A, @R0
   MOV R0, #output_4
   MOV @R0, A
   MOV R0, #summ_1
   MOV A, @R0
   MOV R0, #output_3 
   MOV @R0, A
    ; Включаем UART
 ; Используем TIMER 1 для генерации опорной частоты
   MOV   TMOD,#00100000B		;C/T = 0, Mode = 3 режим таймера с автоперезагрузкой
	MOV   TH1, #0F8H; перегружаемое число
	SETB  TR1;разрешаем таймер
	MOV  PCON, #01011100B;SMOD=1	
	MOV   SCON,#01010010B;Режим 9-разрядной посылки
	;последовательно начиная со старшего байта выводим результат
	MOV R0, #output_6
    MOV A, @R0	
	CALL output	
	MOV R0, #output_5                             	
    MOV A, @R0
	CALL output	
	MOV R0, #output_4
    MOV A, @R0
	CALL output	
	MOV R0, #output_3
    MOV A, @R0
	CALL output	
	MOV R0, #output_2
    MOV A, @R0
	CALL output	
	MOV R0, #output_1
    MOV A, @R0
	CALL Output 	
	JMP start	
END









































