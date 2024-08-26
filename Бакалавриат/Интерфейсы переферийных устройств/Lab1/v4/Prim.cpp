#include<iostream> //подключаем необходимые библиотеки
#include<conio.h>
#include<stdio.h>
#include<windows.h>
DCB dcb;
HANDLE Com;
BOOL Success;
int init()
{
	Com=CreateFile("COM1",GENERIC_READ| GENERIC_WRITE,0,NULL,OPEN_EXISTING,0,NULL);
	if(Com == INVALID_HANDLE_VALUE)
	{
		printf("Invalid CreateFile()!\n");
		return 1;
	} 
	dcb.BaudRate=CBR_9600; //инициализируем параметры порта
	dcb.ByteSize=8;			
	dcb.Parity = NOPARITY;
	dcb.StopBits=ONESTOPBIT;
	Success=SetCommState(Com,&dcb);
	if(!Success)
	{
		printf("Invalid SetCommState()!\n");
		return 2;
	}
	return 0;
}
int read()
{
	char c;
	DWORD dw;
	BOOL r;
	printf("READ\n");
	do
	{
		r=ReadFile(Com,&c,sizeof(BYTE),&dw,NULL); //чтение из порта
		if(r=0)
		{
			printf("Error ReadFile!\n");
			return 1;
		}
		if(dw!=0)
			printf("%c",c);
	}
	while(c!=27);
	printf("\n");
	return 0;
}
int write()
{
	char c;
	DWORD dw;
	BOOL r;
	printf("WRITE\n");
	do
	{
		c=getch();
		r=WriteFile(Com,&c,sizeof(BYTE),&dw,NULL); //запись в порт
		if(r=0)
		{
			printf("Error WriteFile!\n");
			return 2;
		}
		printf("%c",c);
	}
	while(c!=27);
	printf("\n");
	return 0;
}
int main()
{
	if(init()!=0)
	{
		printf("Error init()!\n");
		getch();
		return 1;
	}
	char q;
	do
	{
		printf("1)READ COM\n");
		printf("2)WRITE COM\n");
		q=getch();
		switch(q)
		{
		case '1':
				read();
		case '2':
				write();
		}
		getch();
	}
	while(q!=27);
	CloseHandle(Com);
	return 0;
}

