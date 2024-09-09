name Sub
main segment code;объ€вление сегмента пам€ти программ
cseg at 0;выбираем сегмент пам€ти с адресом 0h
jmp start;на начало программы
using 0;использовать 0 банк регистров
rseg main;выбор сегмента main
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
