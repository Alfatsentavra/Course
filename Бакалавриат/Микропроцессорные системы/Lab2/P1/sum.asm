NAME SUM ; название программы
MAIN SEGMENT CODE ; объ€вление сегмента пам€ти программ
CSEG AT 0 ; определение абсолютного сегмента пам€ти с адресом 0h
JMP start ; переход на начало программы
USING 0 ; использование 0 банк регистров
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
