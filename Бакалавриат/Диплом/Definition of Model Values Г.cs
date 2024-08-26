using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using ExcelDataReader;
using Excel = Microsoft.Office.Interop.Excel;

namespace Definition_of_Model_Values_Г
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)] static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow); // Подключение библиотек VBA для взаимодействия со Spline.
        [DllImport("user32.dll", SetLastError = true)] static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")] static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);


        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;
        const uint WM_LBUTTONDblClk = 0x0203;
        const uint WM_LBUTTONClk = 0x0204;


        private Excel.Application excelapp; // Переменные для взаимодействия с Excel.
        private Excel.Workbooks excelappworkbooks;
        private Excel.Workbook excelappworkbook;
        private Excel.Sheets excelsheets;
        private Excel.Worksheet excelworksheet;
        private Excel.Range excelcells;
        private Excel.Range excelcontainer;
        int l = 0, i, j, n, k, ec;  // Переменные для циклов.
        double a, b, c, d, g, f, x, y, Z, L; // Переменные для МНК.

        private void обработатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = openFileDialog1.FileName;

            Text = FileName;

            Main(FileName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        IntPtr hwndt, hwndb1, hwndb2, hwndb3, hwndm1, hwndm2; // Дескрипторы для взаимодействия со Splyne.
        string aod = "AOD = ", container, Str, StrSCA, StrAOD;

        private string FileName = string.Empty;

        private DataTableCollection TableCollection = null;

        private object lockThread = new object();

        public Form1()
        {
            InitializeComponent();
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = openFileDialog1.ShowDialog();

                if (res == DialogResult.OK)
                {
                    FileName = openFileDialog1.FileName;

                    Text = FileName;

                    OpenExcelFile(FileName);
                }
                else
                {
                    throw new Exception("Файл не выбран.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось открыть файл.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenExcelFile(string path)
        {
            FileStream Stream = File.Open(path, FileMode.Open, FileAccess.Read);

            IExcelDataReader Reader = ExcelReaderFactory.CreateReader(Stream);

            DataSet DB = Reader.AsDataSet(new ExcelDataSetConfiguration() {ConfigureDataTable = (x) => new ExcelDataTableConfiguration() {UseHeaderRow = false}});

            TableCollection = DB.Tables;

            toolStripComboBox1.Items.Clear();

            foreach (DataTable table in TableCollection)
            {
                toolStripComboBox1.Items.Add(table.TableName);
            }

            toolStripComboBox1.SelectedIndex = 1;

            Stream.Close();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable Table = TableCollection[Convert.ToString(toolStripComboBox1.SelectedItem)];

            dataGridView1.DataSource = Table;
        }

        private void Main(string path)
        {
            MessageBox.Show("Обработка файла запущена.", "Обработчик", MessageBoxButtons.OK, MessageBoxIcon.Information);

            string StrF = path, StrG = null, StrAG = null;    // Создаём локальную переменную для пересборки значения.
            string[] SplitF = StrF.Split('\\');  // Разделяем значение по символу.
            int SplitFl = SplitF.Length;
            StrF = null;

            for (i = 0; i < SplitFl - 1; i++)
            {
                StrF += SplitF[i] + '\\';
            }
            StrAG = StrF + "Out_All_Ga_Example.xlsx";
            StrG = StrF + "All_Ga_Example.xlsx";
            StrF = StrF + "All_Ga.xlsx";

            // Инициализация рабочих книг и режимов работы.
            excelapp = new Excel.Application(); // Создаём новую книгу.
            excelapp.Visible = false;   // Выключаем отображение Excel.
            excelapp.SheetsInNewWorkbook = 7;   // Задаём количество листов в новой книге. 
            excelapp.Workbooks.Add(StrAG);    // Копируем формат таблицы в новую книгу.
            excelappworkbooks = excelapp.Workbooks;     // Получаем ссылки на книги.
            excelappworkbook = excelappworkbooks[1];    // Выбираем для работы первую книгу. 
            excelappworkbook.Saved = true;  // Отключаем запрос на сохранение этой книги. 
            excelapp.DefaultSaveFormat = Excel.XlFileFormat.xlExcel12;  // Задаём формат для сохранения.
            excelapp.DisplayAlerts = false;  // Запрос на разрешение перезаписи книги включён. 
            excelapp.Workbooks.Open(path);  // Открываем документ с данными, которые мы копируем.
            bool FileEx = File.Exists(StrF);
            if (FileEx == true)
            {
                excelapp.Workbooks.Add(StrF);
            }
            else
            {
                excelapp.Workbooks.Add(StrG);    // Копируем формат таблицы в новую книгу.
            }

            // Сохраняем значение L.
            excelappworkbook = excelappworkbooks[2];    // Выбираем книгу AOD_03.
            excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
            excelworksheet = (Excel.Worksheet)excelsheets.get_Item(2);  // Выбираем лист.
            excelworksheet.Activate();  // Делаем документ активным.
            excelcells = (Excel.Range)excelworksheet.Cells[2, 2];   // Сохраняем значений из выбранной ячейки.
            string StrL = excelcells.Value2;    // Создаём локальную переменную для пересборки значения.
            string[] SplitL = StrL.Split('.');  // Разделяем значение по символу.
            StrL = SplitL[0] + ',' + SplitL[1]; // Пересобираем строку для конвертирования.
            L = Convert.ToDouble(StrL);

            // Копирование AOD.
            for (k = 0; k < 7; k++)
            {
                excelappworkbook = excelappworkbooks[2];    // Выбираем книгу AOD_03.
                excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k + 2);  // Выбираем лист.
                excelworksheet.Activate();  // Делаем документ активным.
                excelcontainer = (Excel.Range)excelworksheet.Cells[2, 3];   // Сохраняем значений из выбранной ячейки.
                StrAOD = excelcontainer.Value2; // Создаём локальную переменную для пересборки значения.
                string[] SplitAOD = StrAOD.Split('.');  // Разделяем значение по символу.
                StrAOD = SplitAOD[0] + ',' + SplitAOD[1];   // Пересобираем строку для конвертирования.
                container = Convert.ToString(excelcontainer.Value2);
                excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k + 1);  // Выбираем лист.
                excelworksheet.Activate();  // Делаем документ активным.
                excelcells = (Excel.Range)excelworksheet.Cells[1, 2];   // Выбираем ячейку.
                excelcells.Value2 = aod + container; // Заносим в выбранную ячейку сохранённое значение.
            }

            // Копирование SCA. 
            for (k = 0; k < 7; k++) // Проходим все 7 страниц.
            {
                for (i = 4; i <= 40; i++) // Проходим все 36 значений SCA.
                {
                    excelappworkbook = excelappworkbooks[2];    // Выбираем книгу AOD_03.
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k + 2);  // Выбираем лист.
                    excelworksheet.Activate();  // Делаем документ активным.
                    excelcontainer = (Excel.Range)excelworksheet.Cells[i + 9, 3];   // Сохраняем значений из выбранной ячейки.
                    excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k + 1);  // Выбираем лист.
                    excelworksheet.Activate();  // Делаем документ активным.
                    excelcells = (Excel.Range)excelworksheet.Cells[i, 2];   // Выбираем ячейку.
                    excelcells.Value2 = excelcontainer; // Заносим в выбранную ячейку сохранённое значение. 
                }
            }

            // Копирование RWI.
            for (k = 0; k < 7; k++) // Проходим все 7 страниц.
            {
                l = 3; // Стартовое значение столбца для документа Out_All_Ga.
                for (n = 0; n <= 317; n = n + 76)   // Проходим все значения PWI по Г_а(Через одно значение т.к. необходимы только они).
                {
                    for (j = 3; j <= 135; j = j + 18) // Проходим все значения RWI по альбедо(Через одно значение т.к. необходимы только они).
                    {
                        for (i = 4; i <= 40; i++) // Проходим все значения RWI по строке.
                        {
                            excelappworkbook = excelappworkbooks[2];    // Выбираем книгу AOD_03.
                            excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                            excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k + 2);  // Выбираем лист.
                            excelworksheet.Activate();  // Делаем документ активным.
                            excelcontainer = (Excel.Range)excelworksheet.Cells[i + 9 + n, j + 6];   // Сохраняем значение из выбранной ячейки.
                            excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                            excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                            excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k + 1);  // Выбираем лист. 
                            excelworksheet.Activate();  // Делаем документ активным.
                            excelcells = (Excel.Range)excelworksheet.Cells[i, l];   // Выбираем ячейку.
                            excelcells.Value2 = excelcontainer; // Заносим в выбранную ячейку сохранённое значение.
                        }
                        l++; // Выставляем следующий столбец в документе Out_All_Ga.
                    }
                }
            }

            // Продление RWI и SCA методом наименьших квадратов.

            // Продление SCA.
            for (k = 1; k <= 7; k++)    // Проходим все 7 страниц.
            {
                x = 1;
                a = 0; // Обнуляем вспомогательные переменные перед расчётом следующей страницы.
                b = 0;
                c = 0;
                d = 0;
                g = 0;
                f = 0;
                for (i = 34; i <= 40; i++)  // Выборка из послдених 6 значений для расчёта коэффициентов для продления.
                {
                    excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                    excelsheets = excelappworkbook.Worksheets;  // Задаём перменную для взаимодействия со страницами.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем страницу для взаимодействия. 
                    excelworksheet.Activate();  // Делаем документ активным. 
                    excelcells = (Excel.Range)excelworksheet.Cells[i, 2];   // Выбираем ячейку для взаимодействия.
                    y = excelcells.Value2;  // Присваиваем y значение из выбранной ячейки.
                    c = c + (x * y);    // n умноженный на сумму x_i * y_i.
                    d = d + x;  // Сумма x_i
                    g = g + y;  // Сумма y_i
                    f = f + Math.Pow(x, 2); // Сумма x^2
                    x++;
                }
                a = ((7 * c) - (d * g)) / ((7 * f) - Math.Pow(d, 2)); // Расчёт коэффициентов для продления.
                b = (g - (a * d)) / 7;
                do  // Продляем значения SCA до значения в 170 градусов.
                {
                    y = (a * x) + b;    // Вычисляем следующие значения SCA.
                    excelcells = (Excel.Range)excelworksheet.Cells[i, 2];   // Выбираем ячейку для записи.
                    excelcells.Interior.ColorIndex = 15; // Устанавливаем цвет ячейки.
                    excelcells.Interior.PatternColorIndex = Excel.Constants.xlAutomatic;
                    excelcells.Value2 = y;  // Записываем в ячейку полученное значение.
                    x++;
                    i++;
                } while (excelcells.Value2 < 170);
            }

            // Продление RWI.
            for (k = 1; k <= 7; k++) // Проходим все 7 страниц.
            {
                // До куда продлять RWI.
                n = 4;  // Начинаем с ячейки с первым значения SCA.
                do  // Выясняем сколько значений в SCA на заданной странице. Ищем пустую клетку в столбце SCA.
                {
                    excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем лист.
                    excelworksheet.Activate();  // Делаем документ активным.
                    excelcells = (Excel.Range)excelworksheet.Cells[n, 2];   // Выбираем ячейку.
                    n++;
                } while (excelcells.Value2 != null);
                ec = n - 1; // Сохраняем номер ячейки с последним значением 

                for (j = 2; j <= 42; j++) // Проходим все столбцы с RWI.
                {
                    x = 1;
                    a = 0; // Обнуляем вспомогательные переменные перед расчётом.
                    b = 0;
                    c = 0;
                    d = 0;
                    g = 0;
                    f = 0;
                    for (i = 34; i <= 40; i++)  // Выборка из послдених 6 значений для расчёта коэффициентов для продления.
                    {
                        excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                        excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                        excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем лист.
                        excelworksheet.Activate();  // Делаем документ активным.
                        excelcells = (Excel.Range)excelworksheet.Cells[i, j];   // Выбираем ячейку.
                        y = excelcells.Value2;  // Присваиваем y значение из выбранной ячейки.
                        c = c + (x * y);    // n умноженный на сумму x_i * y_i
                        d = d + x;  // Сумма x_i
                        g = g + y;  // Сумма y_i
                        f = f + Math.Pow(x, 2); // Сумма x^2
                        x++;
                    }
                    a = ((7 * c) - (d * g)) / ((7 * f) - Math.Pow(d, 2));   // Расчёт коэффициентов для продления.
                    b = (g - (a * d)) / 7;
                    do  // Продляем до последнего значения SCA.
                    {
                        y = (a * x) + b;    // Вычисляем следующее значение RWI.
                        excelcells = (Excel.Range)excelworksheet.Cells[i, j];   // Выбираем ячейку.
                        excelcells.Value2 = y;  // Записываем в ячейку полученное значение.
                        x++;
                        i++;
                    } while (i < ec);
                }
            }

            for (k = 1; k <= 7; k++)    // Дорисовываем границы ,задаём цвет и создаём ячейки для работы со Spline.
            {
                n = 4;  // Начинаем с ячейки с первым значения SCA.
                do  // Выясняем сколько значений в SCA на заданной странице. Ищем пустую клетку в столбце SCA.
                {
                    excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем лист.
                    excelworksheet.Activate();  // Делаем документ активным.
                    excelcells = (Excel.Range)excelworksheet.Cells[n, 2];   // Выбираем ячейку.
                    n++;
                } while (excelcells.Value2 != null);

                ec = n - 1; // Сохраняем номер ячейки с последним значением

                for (j = 2; j <= 43; j = j + 8)
                {
                    for (i = 41; i < ec; i++)
                    {
                        excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                        excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                        excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем лист.
                        excelworksheet.Activate();  // Делаем документ активным.
                        excelcells = (Excel.Range)excelworksheet.Cells[i, j]; // Выбираем ячейку.
                        excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0; // Устанавливаем чёрный цвет для границ.
                        excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous; // Устанавливаем стиль линий границы.
                        excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium; // Устанавливаем толщину линий границы.
                    }
                }

                ec = n + 1; // Сохраняем номер ячейки с последним значением + 2.

                excelcells = (Excel.Range)excelworksheet.Cells[ec, 2]; // Выбираем ячейку.
                excelcells.Interior.ColorIndex = 15; // Устанавливаем цвет ячейки.
                excelcells.Interior.PatternColorIndex = Excel.Constants.xlAutomatic;
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0; // Устанавливаем чёрный цвет для границ.
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous; // Устанавливаем стиль линий границы.
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium; // Устанавливаем толщину линий границы.
                excelcells.Value2 = "1-й интег.";
                ec++;
                excelcells = (Excel.Range)excelworksheet.Cells[ec, 2]; // Выбираем ячейку.
                excelcells.Interior.ColorIndex = 15; // Устанавливаем цвет ячейки.
                excelcells.Interior.PatternColorIndex = Excel.Constants.xlAutomatic;
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0; // Устанавливаем чёрный цвет для границ.
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous; // Устанавливаем стиль линий границы.
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium; // Устанавливаем толщину линий границы.
                excelcells.Value2 = "2-й интег.";
                ec++;
                excelcells = (Excel.Range)excelworksheet.Cells[ec, 2]; // Выбираем ячейку.
                excelcells.Interior.ColorIndex = 15; // Устанавливаем цвет ячейки.
                excelcells.Interior.PatternColorIndex = Excel.Constants.xlAutomatic;
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0; // Устанавливаем чёрный цвет для границ.
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous; // Устанавливаем стиль линий границы.
                excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium; // Устанавливаем толщину линий границы.
                excelcells.Value2 = "Г";

                for (j = 10; j <= 43; j = j + 8)
                {
                    for (i = n + 1; i <= n + 3; i++)
                    {
                        excelcells = (Excel.Range)excelworksheet.Cells[i, j];
                        excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0; // Устанавливаем чёрный цвет для границ.
                        excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous; // Устанавливаем стиль линий границы.
                        excelcells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium; // Устанавливаем толщину линий границы.
                    }
                }
            }

            Process Spline = Process.Start("Spline.exe"); // Запуск программы Spline. 

            Thread.Sleep(2500); // Задаём задержку, чтобы успеть запустить программу до извлечения дескрипторов.

            hwndt = FindWindow(null, "                                                                       Интерполяторный Интегратор");  // Получаем указатель на основное окно Spline.
            hwndb1 = FindWindowEx(hwndt, (IntPtr)0, null, "Очистить поля"); // Получаем указатель на дочернее окно Spline очитсить поля.
            hwndb2 = FindWindowEx(hwndt, (IntPtr)0, null, "Обработать");    // Получаем указатель на дочернее окно Spline обработать.
            hwndb3 = FindWindowEx(hwndt, (IntPtr)0, null, "Выйти"); // Получаем указатель на дочернее окно Spline Выйти.
            hwndm2 = FindWindowEx(hwndt, (IntPtr)0, "TMemo", "");   // Получаем указатель на дочернее окно для ввода RWI Spline.
            hwndm1 = FindWindowEx(hwndt, hwndm2, "Tmemo", "");  // Получаем указатель на дочернее окно для ввода SCA Spline.

            for (k = 1; k <= 7; k++)    // Взаимодействие со Spline.
            {
                n = 4;  // Начинаем с ячейки с первым значения SCA.
                do  // Выясняем сколько значений в SCA на заданной странице. Ищем пустую клетку в столбце SCA.
                {
                    excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем лист.
                    excelworksheet.Activate();  // Делаем документ активным.
                    excelcells = (Excel.Range)excelworksheet.Cells[n, 2];   // Выбираем ячейку.
                    n++;
                } while (excelcells.Value2 != null);

                ec = n - 1; // Сохраняем номер ячейки с последним значением

                double[] SCA = new double[ec];  // Cоздаём массив для сохранения всех значений SCA.

                for (i = 4; i < ec; i++)    // Заполняем массив SCA значениями.
                {
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем лист.
                    excelworksheet.Activate();  // Делаем документ активным.
                    excelcells = (Excel.Range)excelworksheet.Cells[i, 2];   // Выбираем ячейку.
                    SCA[i - 4] = Convert.ToDouble(excelcells.Value2);   // Заносим в массив значения SCA.
                }

                double[] RWI = new double[ec];  // Создаём масив для сохранения всех значений RWI.

                for (j = 3; j < 43; j++)   // Заполняем массив RWI значениями и обрабатываем с помощью Spline.
                {
                    StrSCA = null;
                    Str = null;
                    for (i = 4; i < ec; i++)
                    {
                        StrSCA = StrSCA + Convert.ToString(SCA[i - 4]) + "\r\n";    // Формируем строку для отправки в Spline.
                        excelcells = (Excel.Range)excelworksheet.Cells[i, j];   // Выбираем ячейку.
                        RWI[i - 4] = Convert.ToDouble(excelcells.Value2);   // Заносим в массив значения RWI.
                        Str = Str + Convert.ToString(RWI[i - 4]) + "\r\n";
                    }
                    Clipboard.SetData(DataFormats.Text, (Object)StrSCA);   // Копируем строку SCA в буфер обмена.
                    PostMessage(hwndm1, WM_LBUTTONDblClk, (IntPtr)1, (IntPtr)0);    // Отправляем строку SCA в Spline.
                    Thread.Sleep(1000); // Задаём задержку 1с.
                    Clipboard.SetData(DataFormats.Text, (Object)Str);   // Копируем строку RWI в буфер обмена.
                    PostMessage(hwndm2, WM_LBUTTONDblClk, (IntPtr)1, (IntPtr)0);    // Отправляем строку RWI в Spline.
                    Thread.Sleep(1000); // Задаём задержку 1с.
                    SendMessage(hwndb2, WM_LBUTTONDOWN, (IntPtr)1, (IntPtr)0);  // Нажатие на кнопку Обработать.
                    SendMessage(hwndb2, WM_LBUTTONUP, (IntPtr)1, (IntPtr)0);    // Отжатие кнопки Обработать.
                    string split = Clipboard.GetText(); // Копируем интегралы из буфера в переменную.
                    string[] Split = split.Split(null); // Разделяем строку по символу переноса строки.
                    a = Convert.ToDouble(Split[0]); // Преобразуем значение 1-го интеграла из строки в дробное.
                    b = Convert.ToDouble(Split[1]); // Преобразуем значение 2-го интеграла из строки в дробное.
                    y = (a / b);    // Считаем Г.
                    excelcells = (Excel.Range)excelworksheet.Cells[ec + 2, j];   // Выбираем ячейку первого интеграла.
                    excelcells.Value2 = a;   // Заносим значение 1-го интеграла в таблицу.
                    excelcells = (Excel.Range)excelworksheet.Cells[ec + 3, j];   // Выбираем ячейку первого интеграла.
                    excelcells.Value2 = b;   // Заносим значение 2-го интеграла в таблицу.
                    excelcells = (Excel.Range)excelworksheet.Cells[ec + 4, j];   // Выбираем ячейку Г.
                    excelcells.Value2 = y;  // Заносим значение Г в таблицу.
                    SendMessage(hwndb1, WM_LBUTTONDOWN, (IntPtr)1, (IntPtr)0);  // Нажатие на кнопку Очистить поля.
                    SendMessage(hwndb1, WM_LBUTTONUP, (IntPtr)1, (IntPtr)0);    // Отжатие кнопки Очистить поля.
                    Thread.Sleep(1000); // Задаём задержку 2с.
                }
            }

            SendMessage(hwndb3, WM_LBUTTONDOWN, (IntPtr)1, (IntPtr)0);  // Нажатие на кнопку Выйти.
            SendMessage(hwndb3, WM_LBUTTONUP, (IntPtr)1, (IntPtr)0);    // Отжатие кнопки Выйти.

            Spline.Close();

            Z = 65.0;
            Convert.ToString(Z);
            for (k = 3; k <= 7; k++) // Заполнение финальной таблицы для расчётов.
            {
                n = 4;  // Начинаем с ячейки с первым значения SCA.
                do  // Выясняем сколько значений в SCA на заданной странице. Ищем пустую клетку в столбце SCA.
                {
                    excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(k);  // Выбираем лист.
                    excelworksheet.Activate();  // Делаем документ активным.
                    excelcells = (Excel.Range)excelworksheet.Cells[n, 2];   // Выбираем ячейку.
                    n++;
                } while (excelcells.Value2 != null);

                ec = n + 3; // Сохраняем номер ячейки со значением Г.

                double[] G = new double[41];    // Создаём массив для сохранения значений Г.

                for (j = 3; j <= 43; j++)   // Сохраняем все значения Г в массив.
                {
                    excelcells = (Excel.Range)excelworksheet.Cells[ec, j];   // Выбираем ячейку.
                    G[j - 3] = Convert.ToDouble(excelcells.Value2);   // Заносим в ячейку значение Г.
                }

                excelappworkbook = excelappworkbooks[3];    // Выбираем книгу All_Ga.
                excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.

                switch (L)  // Выбор листа по длинам волн.
                {
                    case 0.675:
                        excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);  // Выбираем лист.
                        excelworksheet.Activate();  // Делаем документ активным.
                        break;

                    case 0.87:
                        excelworksheet = (Excel.Worksheet)excelsheets.get_Item(2);  // Выбираем лист.
                        excelworksheet.Activate();  // Делаем документ активным.
                        break;

                    case 1.02:
                        excelworksheet = (Excel.Worksheet)excelsheets.get_Item(3);  // Выбираем лист.
                        excelworksheet.Activate();  // Делаем документ активным.
                        break;
                }

                l = 0;
                for (i = 3; i <= 152; i = i + 6)
                {
                    excelcells = (Excel.Range)excelworksheet.Cells[i, 2];   // Выбираем ячейку.
                    if (excelcells.Value2 == Z)
                    {
                        for (n = i; n <= i + 5; n++)
                        {
                            excelcells = (Excel.Range)excelworksheet.Cells[n, 3];   // Выбираем ячейку.
                            if (excelcells.Value2 == Convert.ToDouble(StrAOD))
                            {
                                for (j = 4; j <= 11; j++)
                                {
                                    excelcells = (Excel.Range)excelworksheet.Cells[n, j];   // Выбираем ячейку.
                                    excelcells.Value2 = G[l];
                                    l++;
                                }
                            }
                        }
                    }
                }
                Convert.ToDouble(Z);
                Z = Z + 2.5;
            }


            // Сохранение документа и закрытие.
            excelapp.Visible = true; // Включаем отображение Excel.
            excelappworkbook = excelappworkbooks[3];    // Выбираем книгу Out_All_Ga.
            excelappworkbook.SaveAs(StrF);
            excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
            excelappworkbook.Close(false); // Сохранение таблицы с данными для Spline.
            excelappworkbook = excelappworkbooks[2];    // Выбираем книгу AOD_03.
            excelappworkbook.Close(false);    // Сохраняем книгу OAD_03.
            excelapp.Workbooks.Close(); // Закрытие всех книг.
            excelapp.Quit(); // Закрытие Excel.

            MessageBox.Show("Обработка файла закончена.", "Обработчик", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
