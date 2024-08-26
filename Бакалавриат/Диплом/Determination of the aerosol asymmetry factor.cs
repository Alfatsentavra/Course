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

namespace Determination_of_the_aerosol_asymmetry_factor
{
    public partial class Form1 : Form
    {
        private Excel.Application excelapp; // Переменные для взаимодействия с Excel.
        private Excel.Workbooks excelappworkbooks;
        private Excel.Workbook excelappworkbook;
        private Excel.Sheets excelsheets;
        private Excel.Worksheet excelworksheet;
        private Excel.Range excelcells;

        private string FileName = string.Empty;

        private DataTableCollection TableCollection = null;

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

        private void обработатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = openFileDialog1.FileName;

            Text = FileName;

            Main(FileName);
        }

        private void OpenExcelFile(string path)
        {
            FileStream Stream = File.Open(path, FileMode.Open, FileAccess.Read);

            IExcelDataReader Reader = ExcelReaderFactory.CreateReader(Stream);

            DataSet DB = Reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = (x) => new ExcelDataTableConfiguration() { UseHeaderRow = false } });

            TableCollection = DB.Tables;

            toolStripComboBox1.Items.Clear();

            foreach (DataTable table in TableCollection)
            {
                toolStripComboBox1.Items.Add(table.TableName);
            }

            toolStripComboBox1.SelectedIndex = 0;

            Stream.Close();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable Table = TableCollection[Convert.ToString(toolStripComboBox1.SelectedItem)];

            dataGridView1.DataSource = Table;
        }

        private void Main(string path)
        {
            int i, k, n, l = 2, m, o = 0, p = 0;
            double Z, L, AOD, q, G;
            string Date, Time;
            MessageBox.Show("Обработка файла запущена.", "Обработчик", MessageBoxButtons.OK, MessageBoxIcon.Information);

            excelapp = new Excel.Application();
            excelapp.Visible = false;
            excelapp.DisplayAlerts = false;
            excelapp.DefaultSaveFormat = Excel.XlFileFormat.xlExcel12;
            excelapp.Workbooks.Open(path);
            excelappworkbooks = excelapp.Workbooks;
            excelappworkbook = excelappworkbooks[1];    // Выбираем книгу.
            excelappworkbook.Saved = true;
            excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
            excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);
            excelworksheet.Activate();  // Делаем документ активным.

            string StrG = path;
            string[] SplitG = StrG.Split('\\');
            int SplitGl = SplitG.Length;
            StrG = null;

            for (i = 0; i < SplitGl - 1; i++)
            {
                StrG += SplitG[i] + '\\';
            }
            StrG = StrG + "All_Ga.xlsx";

            excelapp.Workbooks.Open(StrG);

            excelappworkbook = excelappworkbooks[1];
            excelcells = (Excel.Range)excelworksheet.Cells[1, 8];
            excelcells.Value2 = "Га";

            do
            {
                excelcells = (Excel.Range)excelworksheet.Cells[l, 1];
                Date = excelcells.Value2;

                if (excelcells.Value2 == null)
                {
                    break;
                }

                excelcells = (Excel.Range)excelworksheet.Cells[l, 2];
                Time = excelcells.Value2;

                excelcells = (Excel.Range)excelworksheet.Cells[l, 3];
                string StrL = excelcells.Value2;    // Создаём локальную переменную для пересборки значения.
                string[] SplitL = StrL.Split('.');  // Разделяем значение по символу.
                StrL = SplitL[0] + ',' + SplitL[1]; // Пересобираем строку для конвертирования.
                L = Convert.ToDouble(StrL);

                excelcells = (Excel.Range)excelworksheet.Cells[l, 4];
                G = excelcells.Value2;

                excelcells = (Excel.Range)excelworksheet.Cells[l, 5];
                Z = excelcells.Value2;

                excelcells = (Excel.Range)excelworksheet.Cells[l, 6];
                AOD = excelcells.Value2;

                excelcells = (Excel.Range)excelworksheet.Cells[l, 7];
                q = excelcells.Value2;

                excelappworkbook = excelappworkbooks[2];    // Выбираем книгу.
                excelsheets = excelappworkbook.Worksheets;

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

                i = 3;
                do
                {
                    excelcells = (Excel.Range)excelworksheet.Cells[i, 2];   // Выбираем ячейку.
                    i = i + 6;
                } while (excelcells.Value2 <= Z);
                n = i - 6;

                i = 3;
                do
                {
                    excelcells = (Excel.Range)excelworksheet.Cells[i, 3];   // Выбираем ячейку.
                    i++;
                } while (excelcells.Value2 <= AOD);
                k = i - 1;

                i = 4;
                do
                {
                    excelcells = (Excel.Range)excelworksheet.Cells[1, i];   // Выбираем ячейку.
                    i++;
                } while (excelcells.Value2 <= q);
                m = i - 1;

                if (AOD <= 0.05 || AOD >= 0.3)
                {
                    i = 3;
                    do
                    {
                        excelcells = (Excel.Range)excelworksheet.Cells[i, 3];   // Выбираем ячейку.
                        i++;
                    } while (excelcells.Value2 != AOD);
                    k = i - 1;
                }

                if (q <= 0 || q >= 0.7)
                {
                    i = 4;
                    do
                    {
                        excelcells = (Excel.Range)excelworksheet.Cells[1, i];   // Выбираем ячейку.
                        i++;
                    } while (excelcells.Value2 != q);
                    m = i - 1;
                }

                if (Z <= 65 && Z >= 75)
                {
                    i = 3;
                    do
                    {
                        excelcells = (Excel.Range)excelworksheet.Cells[i, 2];   // Выбираем ячейку.
                        i = i + 6;
                    } while (excelcells.Value2 != Z);
                    n = i - 6;
                }

                double[] G3 = new double[5], G6 = new double[5];
                double[] Gm = new double[5];
                for (i = 0; i < 5; i++)
                {
                    double g1, g2, q1, q2, b, a, G1, G2, AOD1, AOD2, G4, G5, Z1, Z2;
                    excelcells = (Excel.Range)excelworksheet.Cells[k - 2, m - 2];   // Выбираем ячейку.
                    g1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[k - 2, m];   // Выбираем ячейку.
                    g2 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m - 2];   // Выбираем ячейку.
                    q1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m];   // Выбираем ячейку.
                    q2 = excelcells.Value2;
                    b = (q1 * g2 - q2 * g1)/(q1 -q2);
                    a = (g1 - b) / q1;
                    G1 = (a * q) + b;

                    excelcells = (Excel.Range)excelworksheet.Cells[k, m];   // Выбираем ячейку.
                    g1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[k, m - 2];   // Выбираем ячейку.
                    g2 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m - 2];   // Выбираем ячейку.
                    q1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m];   // Выбираем ячейку.
                    q2 = excelcells.Value2;
                    b = (q1 * g2 - q2 * g1) / (q1 - q2);
                    a = (g1 - b) / q1;
                    G2 = (a * q) + b;

                    excelcells = (Excel.Range)excelworksheet.Cells[k - 2, 3];   // Выбираем ячейку.
                    AOD1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[k, 3];   // Выбираем ячейку.
                    AOD2 = excelcells.Value2;
                    b = (AOD1 * G2 - AOD2 * G1) / (AOD1 - AOD2);
                    a = (G1 - b) / AOD1;
                    G3[i] = (a * AOD) + b;

                    k = k + 12;

                    excelcells = (Excel.Range)excelworksheet.Cells[k - 2, m - 2];   // Выбираем ячейку.
                    g1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[k - 2, m];   // Выбираем ячейку.
                    g2 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m - 2];   // Выбираем ячейку.
                    q1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m];   // Выбираем ячейку.
                    q2 = excelcells.Value2;
                    b = (q1 * g2 - q2 * g1) / (q1 - q2);
                    a = (g1 - b) / q1;
                    G4 = (a * q) + b;

                    excelcells = (Excel.Range)excelworksheet.Cells[k, m];   // Выбираем ячейку.
                    g1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[k, m - 2];   // Выбираем ячейку.
                    g2 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m - 2];   // Выбираем ячейку.
                    q1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[1, m];   // Выбираем ячейку.
                    q2 = excelcells.Value2;
                    b = (q1 * g2 - q2 * g1) / (q1 - q2);
                    a = (g1 - b) / q1;
                    G5 = (a * q) + b;

                    excelcells = (Excel.Range)excelworksheet.Cells[k - 2, 3];   // Выбираем ячейку.
                    AOD1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[k, 3];   // Выбираем ячейку.
                    AOD2 = excelcells.Value2;
                    b = (AOD1 * G5 - AOD2 * G4) / (AOD1 - AOD2);
                    a = (G1 - b) / AOD1;
                    G6[i] = (a * AOD) + b;

                    excelcells = (Excel.Range)excelworksheet.Cells[n, 2];   // Выбираем ячейку.
                    Z1 = excelcells.Value2;
                    excelcells = (Excel.Range)excelworksheet.Cells[n - 12, 2];   // Выбираем ячейку.
                    Z2 = excelcells.Value2;
                    b = (Z1 * G6[i] - Z2 * G3[i]) / (Z1 - Z2);
                    a = (G3[i] - b) / Z1;
                    Gm[i] = a * Z + b;

                    k = k + 18;
                }

                double Gs, Gb;
                Gs = Gm[0];
                Gb = Gm[4];
                for (i = 0; i < 5; i++)
                {
                    double G1 = 0;
                    if (Gm[i] < G)
                    {
                        G1 = Gm[i];
                    }

                    if (G1 >= Gs)
                    {
                        Gs = G1;
                        o = i;
                    }
                }
                for (i = 4; i >= 0; i--)
                {
                    double G2 = 0;
                    if (Gm[i] > G)
                    {
                        G2 = Gm[i];
                    }

                    if (G2 < Gb)
                    {
                        Gb = G2;
                        p = i + 1;
                    }
                }

            double Ga1 = 0, Ga2 = 0;
                switch (o)
                {
                    case 0:
                        Ga1 = 6;
                        break;
                    case 1:
                        Ga1 = 7.1;
                        break;
                    case 2:
                        Ga1 = 8.65;
                        break;
                    case 3:
                        Ga1 = 10.65;
                        break;
                    case 4:
                        Ga1 = 13.85;
                        break;
                }

                switch (p)
                {
                    case 0:
                        Ga2 = 6;
                        break;
                    case 1:
                        Ga2 = 7.1;
                        break;
                    case 2:
                        Ga2 = 8.65;
                        break;
                    case 3:
                        Ga2 = 10.65;
                        break;
                    case 4:
                        Ga2 = 13.85;
                        break;
                }

                double inf1, inf2, Ga;

                inf1 = (Gs - G) / G;

                inf2 = (Gb - G) / G;

                if (inf1 <= 0.1 || inf2 <= 0.1)
                {
                    double b, a;
                    b = (Ga1 * Gm[p] - Ga2 * Gm[o]) / (Gm[o] - Gm[p]);
                    a = (Gm[o] - b) / Ga1;
                    Ga = a * G + b;
                    excelappworkbook = excelappworkbooks[1];
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);
                    excelcells = (Excel.Range)excelworksheet.Cells[l, 8];   // Выбираем ячейку.
                    excelworksheet.Activate();
                    excelcells.Value2 = Ga;
                }
                else
                {
                    excelappworkbook = excelappworkbooks[1];
                    excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);
                    excelcells = (Excel.Range)excelworksheet.Cells[l, 8];   // Выбираем ячейку.
                    excelworksheet.Activate();
                    excelcells.Value2 = "Плохая точность.";
                    l++;
                    excelappworkbook = excelappworkbooks[1];
                    excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);
                    excelcells = (Excel.Range)excelworksheet.Cells[l, 3];
                    continue;
                }

                l++;
                excelappworkbook = excelappworkbooks[1];
                excelsheets = excelappworkbook.Worksheets;  // Получаем ссылки на листы.
                excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);
                excelcells = (Excel.Range)excelworksheet.Cells[l, 3];
            } while (excelcells.Value2 != null);

            string OutGa = path;
            string[] SplitOutGa = OutGa.Split('\\');
            int SplitOutGal = SplitG.Length;
            OutGa = null;

            for (i = 0; i < SplitOutGal - 1; i++)
            {
                OutGa += SplitOutGa[i] + '\\';
            }
            OutGa = OutGa + "Out_Ga.xlsx";

            // Сохранение документа и закрытие.
            excelapp.Visible = true; // Включаем отображение Excel.
            excelappworkbook = excelappworkbooks[1];    // Выбираем книгу Out_All_Ga.
            excelappworkbook.SaveAs(path);
            excelappworkbook = excelappworkbooks[2];    // Выбираем книгу Out_All_Ga.
            excelappworkbook.Close(false); // Сохранение таблицы с данными для Spline.
            excelapp.Workbooks.Close(); // Закрытие всех книг.
            excelapp.Quit(); // Закрытие Excel.

            MessageBox.Show("Обработка файла закончена.", "Обработчик", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
