NAME VARIABLES
PUBLIC mult_1, mult_2, mult_3, mult_4, summ_1, summ_2, summ_3, summ_4, addr1, addr2, addr3, multres_1_1, multres_1_2, multres_1_3, multres_1_4, multres_2_1, multres_2_2, multres_2_3, multres_2_4, multres_3_1, multres_3_2, multres_3_3, multres_3_4, multread_1, multread_2, multread_3, multread_4, output_1, output_2, output_3, output_4, output_5, output_6
BITVAR	SEGMENT	data
RSEG  BITVAR
;адреса внешней пам€ти
addr1 equ 0000h
addr2 equ addr1+1
addr3 equ addr2+1
; временные значени€ умножени€
mult_1 equ 01fh  
mult_2 equ 01eh
mult_3 equ 01dh
mult_4 equ 01ch  
; временные значени€ суммы
summ_1 equ 056h
summ_2 equ 055h
summ_3 equ 054h
summ_4 equ 053h
;временное хранение результата умножени€  младших байтов
multres_1_1 equ 034h 
multres_1_2 equ multres_1_1 + 1		;035
multres_1_3 equ multres_1_2 + 1		;036
multres_1_4 equ multres_1_3 + 1		;037
;временное хранение результата умножени€  средних байтов
multres_2_1 equ 03Bh 
multres_2_2 equ multres_2_1 + 1		;03c
multres_2_3 equ multres_2_2 + 1		;03d
multres_2_4 equ multres_2_3 + 1		;03e
;временное хранение результата умножени€  старших байтов
multres_3_1 equ 042h 
multres_3_2 equ multres_3_1 + 1		;043
multres_3_3 equ multres_3_2 + 1		;044
multres_3_4 equ multres_3_3 + 1		;045
;считывание результатов умножени€ байтов
multread_1 equ 01CH  
multread_2 equ multread_1+1		;01d
multread_3 equ multread_2+1		;01e
multread_4 equ multread_3+1		;01f
;¬ывод                                         
output_1 equ 04Fh
output_2 equ 04Eh 
output_3 equ 04Dh 
output_4 equ 04Ch  
output_5 equ 04Bh 
output_6 equ 04Ah                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
END 
























































































































































