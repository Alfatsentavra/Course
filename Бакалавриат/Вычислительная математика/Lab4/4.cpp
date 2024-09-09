#include <math.h>
#include <conio.h>
#include <iostream>
#include <stdio.h>
#include <iomanip>

using namespace std;

double fx(double x)
{
	return exp(x);
}

int main()
{
	
	setlocale(LC_ALL, "Russian");
	int i, k, n;
	double x[200]; 
	double Ln = 0.0, Pni, a, b, xt, tk, gr, t_Pg, pi = 3.14159265358979323846;
	cout << "������� �������� �������� ����� xt : ";
	cin >> xt;
	cout << "������� �������� ������ ������� a,b : ";
	cin >> a >> b;
	cout << "������� ���������� ����� n : ";
	cin >> n;
	for (i = 0; i <= n; i++)
	{
		tk = cos(((2 * i + 1) * pi) / (2 * (n + 1)));
		x[i] = (((b - a) * tk + (a + b)) / 2);
	}
	t_Pg = 1.0;
	for (i = 0; i <= n; i++)
	{
		Pni = 1.0;
		for (k = 0; k <= n; k++)
		{
			if (k == i)
				continue;
			else
				Pni *= (xt - x[k]) / (x[i] - x[k]);
		}
		Ln += Pni * fx(x[i]);		//������ ���
		t_Pg *= (xt-x[i])/(i+1);
	}
	gr = fabs(fx(xt) - Ln);
	t_Pg = t_Pg * fx(x[n]);
	cout << "���������������� ��������� �������� Ln = " << setprecision(18) << Ln << endl;
	cout << "�������� ������� exp = " << fx(xt) << endl;
	cout << "������������ ����������� = " << gr << endl;
cout << "������������� ����������� = " << t_Pg << endl;
	_getch();
	return 0;
}