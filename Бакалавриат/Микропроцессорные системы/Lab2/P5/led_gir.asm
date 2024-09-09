NAME SUBB 
USING 0					;���� ���������

ZADER20_H EQU 20H
ZADER20_L EQU 21H
GR_TWO 	EQU  R7
COUNT_CADR EQU R0		;������� ������ � �������
SPEED EQU R1			;������� ��������� ��������
DELAY EQU R2			;���������� ��������� ��������
NOMER_EFECTA EQU R3		;����� �������
COUNT_GREEN EQU R4		;������� ��������� ��������


;������ ��������;
F1  SEGMENT CODE    	;������������� ������������ �������� ��������� ����� � ������������ ��������
F2  SEGMENT CODE
F3  SEGMENT CODE
F4  SEGMENT CODE
F5  SEGMENT CODE
F6  SEGMENT CODE
F7  SEGMENT CODE
F8  SEGMENT CODE
F9  SEGMENT CODE
F10 SEGMENT CODE
F11 SEGMENT CODE
F12 SEGMENT CODE
F13 SEGMENT CODE
F14 SEGMENT CODE
F15 SEGMENT CODE
F16 SEGMENT CODE


PROG SEGMENT CODE		;���������� �������� ���� ���������
STACK SEGMENT IDATA		;���������� �������� �����

RSEG STACK				;�������� ��������� ������������ ������� � ������ ��� ��������
DS 16H					;��� ���� ������������� 16 ����
        ;DB ����������� ����������� ���������� ���� � ������ ��������
rseg F1						;db ����������� ����������� ���������� ���� � ������ ��������
	db 10000001b, 01000010b, 00100100b, 00011000b, 00011000b, 00100100b, 01000010b, 10000001b 	; �� ���� � �����											 			
rseg F2
	db 10101010b, 01010101b, 10101010b, 01010101b, 10101010b, 01010101b, 10101010b, 01010101b 	; ����� ������ � �����									
rseg F3  
	db 00000000b, 00000001b, 00000010b, 00000011b, 00000100b, 00000101b, 00000110b, 00000111b 	; ���� �� 0-7										
rseg F4 
	db 10100101b, 01011010b, 10010110b, 01101001b, 10011001b, 01100110b, 10100101b, 01011010b 	; ���������� 10 � 01										
rseg F5  
	db 10000001b, 11000011b, 11100111b, 11111111b, 11100111b, 11000011b, 10000001b, 00000000b 	; ���������� 1 �� ���� � ������										
rseg F6  
	db 01111110b, 00111100b, 00011000b, 00000000b, 00011000b, 00111100b, 01111110b, 11111111b 	; ���������� 0 �� ���� � ������										
rseg F7  
	db 10010010b, 01001001b, 00100100b, 00010010b, 00001001b, 00000100b, 00000010b, 00000001b	; ����� ������ ��� ����										
rseg F8  
	db 10110110b, 01101100b, 11011000b, 10110000b, 01100000b, 11000000b, 10000000b, 00000000b  	; ����� ����� ��� ����										
rseg F9
	db 00000000b, 11111111b, 00000000b, 11111111b, 00000000b, 11111111b, 00000000b, 11111111b   	; �������� - �������										
rseg F10
	db 11110000b, 01111000b, 00111100b, 00011110b, 00001111b, 10000111b, 11000011b, 11100001b 	; �������� �����, ����� ������ � ����� 									
rseg F11
	db 00001111b, 00011110b, 00111100b, 01111000b, 11110000b, 11100001b, 11000011b, 10000111b       ; �������� �����, ����� ����� � �����                         
rseg F12
	db 00001111b, 10001110b, 11001100b, 11101000b, 11110000b, 01110001b, 00110011b, 00010111b 	; �������� �����, ������ �������									
rseg F13
	db 10000000b, 10000001b, 01000000b, 01000010b, 00100000b, 00100100b, 00010000b, 00011000b 	; �� ���� � ����� � ������������										
rseg F14
	db 10010110b, 01101001b, 10010110b, 01101001b, 10010110b, 01101001b, 10010110b, 01101001b	; �������� 1001 - 0110											
rseg F15
	db 11111111b, 11111110b, 11111100b, 11111000b, 11110000b, 11100000b, 11000000b, 10000000b 	; ���������� 0										
rseg F16
	db 10000000b, 11000000b, 11100000b, 11110000b, 11111000b, 11111100b, 11111110b, 11111111b   	; ���������� 1
  
CSEG AT 0
   LJMP go					;��� ��������� ������� ����� ��������� ����������� ������� �� ����� START

;ORG - ������������ ��� �������� ���������� ������ ������� � ������   

ORG 0BH						;����� ������� ���������� �� �\�0
   CALL EFFECT				;������� ������������, ��������� ���� ������� 
   RETI
  
ORG 1BH						;����� ������� ���������� �� �\�1	
   CALL GREENOMER_EFECTA  	;������� ������������, �������������� �������� ������� ����������
   RETI

   
   
   
   
   
RSEG PROG					;������� ���� ���������:

DELAY1:						; ������ �� ��������
	MOV R5, #0FFH           
	MMMM1:
		MOV R6, #0C4H   
		MMMM2:
		DJNZ R6, MMMM2 
	DJNZ R5, MMMM1
	MOV R5, #0FFH	
RET


GREENOMER_EFECTA:				;�������� �� �������� �������� ����������
	CLR TCON.6               
   MOV TH1, #03CH           
   MOV TL1, #0AFH
   DJNZ COUNT_GREEN, ST_GREEN   
     
	   CJNE GR_TWO, #02H, LIGHT 
		 MOV COUNT_GREEN, #5 ; ��������� �������� � 0.25���
		 DEC GR_TWO    
		 SETB P3.7
	   LJMP ST_GREEN    
	   LIGHT:  
	   CJNE GR_TWO, #01H, OUT
	   ; ��������� ������� � 0.5���
		 MOV COUNT_GREEN, #10
		 DEC GR_TWO  
		 CLR P3.7  
	   LJMP ST_GREEN  
	   OUT:  
	   CJNE GR_TWO, #00H, END_GREEN
		SETB P3.7                                                                                                                                                                               
		LJMP END_GREEN 
           
   ST_GREEN:                         
     SETB TCON.6                 
 END_GREEN:      
RET

SET_EFFECT:							;����� ������
     CJNE NOMER_EFECTA, #1, L1		;��������� ������������ � ��������������� ������ � �������, ���� �� �����
         MOV DPTR, #F1 -  1			;DPTR �������� ��������
L1:  CJNE NOMER_EFECTA, #2, L2      
         MOV DPTR, #F2 -  1
L2:  CJNE NOMER_EFECTA, #3, L3      
         MOV DPTR, #F3 -  1
L3:  CJNE NOMER_EFECTA, #4, L4      
         MOV DPTR, #F4 -  1
L4:  CJNE NOMER_EFECTA, #5,L5      
         MOV DPTR, #F5 -  1
L5:  CJNE NOMER_EFECTA, #6, L6      
         MOV DPTR, #F6 -  1
L6:  CJNE NOMER_EFECTA, #7, L7      
         MOV DPTR, #F7 -  1
L7:  CJNE NOMER_EFECTA, #8, L8      
         MOV DPTR, #F8 -  1
L8:  CJNE NOMER_EFECTA, #9, L9      
         MOV DPTR, #F9 -  1
L9:  CJNE NOMER_EFECTA, #10,L10      
         MOV DPTR, #F10 - 1
L10: CJNE NOMER_EFECTA, #11,L11      
         MOV DPTR, #F11 - 1
L11: CJNE NOMER_EFECTA, #12,L12      
         MOV DPTR, #F12 - 1
L12: CJNE NOMER_EFECTA, #13,L13      
         MOV DPTR, #F13 - 1
L13: CJNE NOMER_EFECTA, #14,L14      
         MOV DPTR, #F14 - 1
L14: CJNE NOMER_EFECTA, #15, L15      
         MOV DPTR, #F15 - 1
L15: CJNE NOMER_EFECTA, #16, L16   
         MOV DPTR, #F16 - 1        
L16: RET 

SET_SPEED: ;����� ��������
SP1:
CJNE SPEED, #1, SP2
MOV 020H, #1H
MOV 021H, #62H
MOV 022H, #79H
RET

SP2:
CJNE SPEED, #2, SP3
MOV 020H, #1H
MOV 021H, #81H
MOV 022H, #0A9H
RET

SP3:
CJNE SPEED, #3, SP4
MOV 020H, #1H
MOV 021H, #0C3H
MOV 022H, #0F0H
RET

SP4:
CJNE SPEED, #4, SP5
MOV 020H, #1H
MOV 021H, #0F4H
MOV 022H, #0ABH
RET

SP5:
CJNE SPEED, #5, SP6
MOV 020H, #2H
MOV 021H, #26H
MOV 022H, #66H
RET

SP6:
CJNE SPEED, #6, SP7
MOV 020H, #2H
MOV 021H, #87H
MOV 022H, #0DDH
RET

SP7:
CJNE SPEED, #7, SP8
MOV 020H, #2H
MOV 021H, #0EAH
MOV 022H, #54H
RET

SP8:
CJNE SPEED, #8, SP9
MOV 020H, #3H
MOV 021H, #86H
MOV 022H, #45H
RET

SP9:
CJNE SPEED, #9, SP10
MOV 020H, #4H
MOV 021H, #0FH
MOV 022H, #0B7H
RET

SP10:
CJNE SPEED, #10, SP11
MOV 020H, #4H
MOV 021H, #71H
MOV 022H, #2EH
RET

SP11:
CJNE SPEED, #11, SP12
MOV 020H, #4H
MOV 021H, #0D2H
MOV 022H, #0A6H
RET

SP12:
CJNE SPEED, #12, SP13
MOV 020H, #5H
MOV 021H, #96H
MOV 022H, #93H
RET

SP13:
CJNE SPEED, #13, SP14
MOV 020H, #6H
MOV 021H, #5AH
MOV 022H, #80H
RET

SP14:
CJNE SPEED, #14, SP15
MOV 020H, #7H
MOV 021H, #1EH
MOV 022H, #6DH
RET

SP15:
CJNE SPEED, #15, SP16
MOV 020H, #7H
MOV 021H, #0E1H
MOV 022H, #5BH
RET

SP16:
CJNE SPEED, #16, R
MOV 020H, #8H
MOV 021H, #0A5H
MOV 022H, #48H
JMP R
R:
MOV SPEED, #0
Ro:
RET     



EFFECT:						;����� �����:
 	DJNZ ZADER20_L,SKIP
 		MOV ZADER20_L, #255
 		DJNZ ZADER20_H,SKIP
 		
 		INC NOMER_EFECTA
 		MOV ZADER20_L,#190
 		MOV ZADER20_H,#2
 		
 		CJNE NOMER_EFECTA, #17, M11   		;���� ����� ������� �� ����� 17 �� �������       
    		MOV NOMER_EFECTA, #16            
		M11:   
		   CJNE NOMER_EFECTA, #16, SKIP     
			MOV COUNT_GREEN, #10
			MOV GR_TWO, #02H
			CLR P3.7							;������ ��������� �������� ���������
			SETB TCON.6							;����������� ������ ������ 	
 	SKIP:
   CALL SET_EFFECT			;����� ������
   DJNZ DELAY, END_CADR		;��������� ������� �������� (��������� �������� � �������, ���� �� ����)
   MOV A, SPEED				;���� ������� �������� ������ ����, ����������� ��� ��������� ��������
   MOV DELAY, A
   MOV A, DPL					
   ADD A, COUNT_CADR 		;���������� � �������� ������ ������� ������� ������, �������� ����� �������� �����
   MOV DPL, A
   MOV A, DPH
   ADDC A,#0
   MOV DPH, A  
   CLR A							;����� ������������
   MOVC  A, @A+DPTR    		;!
   MOV P1, A					;������� ����
   INC COUNT_CADR
   CJNE COUNT_CADR, #9, END_CADR     
   MOV COUNT_CADR, #1
END_CADR:
   RET 
   
   
   
go:                       ; �������� ���������:
	MOV GR_TWO, #02H
   MOV SP, #STACK-1       ; ������������� �����!
   MOV ZADER20_L, #40
   MOV ZADER20_H, #2
   MOV TMOD, #11H         ; �/�0 � �/�1 �������� � ������ 16-������ ��������
   SETB IE.7              ; ����������� ����������
   SETB IE.1              ; ����������� ���������� �� �������� �������
   SETB IE.3              ; ����������� ���������� �� ������� �������
   MOV TH0, #0H         
   MOV TL0, #0H
   SETB TCON.4          
   CLR  TCON.6          
   MOV TH1, #03CH        
   MOV TL1, #0AFH   
   MOV NOMER_EFECTA, #1         ;��������������� ����� ������� - ������
   MOV SPEED, #1         		;��������������� ���������� ��������� ��������
   MOV DELAY, #1         		;������������� ������� �������� 
   MOV COUNT_CADR,  #1   		;������������� ������� ������
   
   

KN1:                     
	JB P3.2, KN2    					;���� �� ������ ������ ������, ������ � ������ ������
	CALL DELAY1
	JNB P3.2, $							;���� �� �������� � 0 �� ������� �� ����
	MOV ZADER20_L, #40
   MOV ZADER20_H, #2
   
	DEC  NOMER_EFECTA                    
	CJNE NOMER_EFECTA, #0, M			;���� ����� ������� �� ����� ���� �� �������             
	MOV NOMER_EFECTA, #1              
	M:  
		CJNE NOMER_EFECTA, #1, KN1			;���� ����� ������� �� ����� 1 �� ������� KN1 � ������ ���������� ��� ������   
		MOV GR_TWO, #02H ;��� �������� ��������         
		MOV COUNT_GREEN, #10  
		CLR P3.7							;������ ��������� �������� ���������
		SETB TCON.6							;����������� ������ ������
         
KN2:
	JB P3.3, KN3						;���� �� ������ ������ ������, ������ � ������ �������
	CALL DELAY1
    JNB P3.3, $ 						;���� �� �������� � 0 �� ������� �� ��������� ����� 
     
   MOV ZADER20_L, #40
   MOV ZADER20_H, #2
      
	INC  NOMER_EFECTA                   
    CJNE NOMER_EFECTA, #17, M1   		;���� ����� ������� �� ����� 17 �� �������       
    MOV NOMER_EFECTA, #16            
	M1:   
		CJNE NOMER_EFECTA, #16, KN2 		;���� ����� ������� �� ����� 6 �� ������� KN2 � ������ ���������� ��� ������    
		MOV GR_TWO, #02H;��� �������� ��������     
		MOV COUNT_GREEN, #10 
		CLR P3.7							;������ ��������� �������� ���������
		SETB TCON.6							;����������� ������ ������
	  
KN3:
	JB P3.4, KN4  						
	CALL DELAY1
    JNB P3.4, $ 
    
   MOV ZADER20_L, #40
   MOV ZADER20_H, #2  
     
	DEC SPEED      
    CJNE SPEED, #0, LL   
    MOV SPEED, #1
LL:  CJNE SPEED, #1, KN3    
		MOV GR_TWO, #0;��� ���������� �������� ��������     
     MOV COUNT_GREEN, #20 
     CLR P3.7; ������ ��������� �������� ���������
     SETB TCON.6; ����������� ������ ������
        

KN4:
   JB P3.5, KN1; ���� �� ������ �������� ������, ������ � ������ ������
   CALL DELAY1
       JNB P3.5, $
       
       MOV ZADER20_L, #40
   		MOV ZADER20_H, #2
                          
       INC SPEED
       CJNE SPEED, #17, LL1    
       MOV SPEED, #16
LL1:   CJNE SPEED, #16, KN4  
		 MOV GR_TWO, #0;��� ���������� �������� ��������       
       MOV COUNT_GREEN, #20 
       CLR P3.7; ������ ��������� �������� ���������
       SETB TCON.6; ����������� ������ ������
       JMP KN1
JMP go      
END













