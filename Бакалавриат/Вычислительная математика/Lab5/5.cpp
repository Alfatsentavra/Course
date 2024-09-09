// 3.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <math.h>
#include <conio.h>
#include <iostream>
#include <stdio.h>
#include <iomanip>

using namespace std;

double fx(double x, double y)
{
	return pow(y, 2.0);
}

int main()
{

	setlocale(LC_ALL, "Russian");
	double a, b, h, k1, k2, k3, k4, y, yg, yp, pg, yt, xi;
	int num, n, i;
	cout << "Введите кол-во точек (n): " << endl;
	cin >> n;
	y = 1;
	do
	{
		cout << "Введите правый конец отрезка (b < 1): ";
		cin >> b;
	} 
	while (b >= 1);
		h = (b / n);
	do
	{
		cout << "Выберите номер метода, которым вы хотите произвести вычисление: " << endl << "Метод предикатор-корректор(1)" << endl << "Усовершенствованный метод Эйлера(2)" << endl << "Метод Рунге - Кутта(3)" << endl;
		cin >> num;
	} while (num < 1 || num > 3);
		yt = 1.0;
	pg = fabs(yt - y);
	a = 0;
	cout << y << "   " << yt << "   " << pg << endl;
	switch (num)
	{
	case 1:
		for (i = 1; i <= n; i++)
		{
			xi = (a + i * h);
			yg = y + h * fx(xi, y);
			y = y + h * ((fx(xi, y) + fx(xi + h, yg)) / 2.0);
			yt = 1.0 / (1.0 - xi);
			pg = fabs(yt - y);
			cout << y << "   ";
			cout << yt << "   ";
			cout << pg << endl;
		}
		cout << "Вычисленный значение: " << y << endl;
		cout << "Точное значение: " << yt << endl;
		cout << "Погрешность вычисления: " << pg << endl;
		break;
	case 2:
		for (i = 1; i <= n; i++)
		{
			xi = (a + i * h);
			yp = y + (h / 2.0) * fx(xi, y);
			y = y + h * fx(xi + (h / 2.0), yp);
			yt = 1.0 / (1.0 - xi);
			pg = fabs(yt - y);
			cout << y << "   ";
			cout << yt << "   ";
			cout << pg << endl;
		}
		cout << "Вычисленный значение: " << y << endl;
		cout << "Точное значение: "<< yt << endl;
		cout << "Погрешность вычисления: " << pg << endl;
		break;
	case 3:
		for (i = 1; i <= n; i++)
		{
			xi = (a + i * h);
			k1 = h * fx(xi, y);
			k2 = h * fx(xi + (h / 2.0), y + (k1 / 2.0));
			k3 = h * fx(xi + (h / 2.0), y + (k2 / 2.0));
			k4 = h * fx(xi + h, y + k3);
			y = y + ((k1 + (2 * k2) + (2 * k3) + k4) / 6.0);
			yt = 1.0 / (1.0 - xi);
			pg = fabs(yt - y);
			cout << y << "   ";
			cout << yt << "   ";
			cout << pg << endl;
		}
		cout << "Вычисленный значение: " << y << endl;
		cout << "Точное значение: " << yt << endl;
		cout << "Погрешность вычисления: " << pg << endl;
		break;
	}
	_getch();
	return 0;
} 


