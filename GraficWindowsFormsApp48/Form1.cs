using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraficWindowsFormsApp48
{
    public partial class Form1 : Form
    {
        int[] values = { };
        public Form1()
        {
            InitializeComponent();


            // Установим палитру
            chart1.Palette = ChartColorPalette.SeaGreen;

            // Заголовок графика
            //chart1.Titles.Add("");

            Open("values.ddd");

/*            using (FileStream fs = new FileStream("user.ddd", FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fs, numberOfVisitors);
            }
*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chart1.Series.Count > 1)
                return;

            chart1.Series.Add(new Series());

            Dictionary<double, double> serie = new Dictionary<double, double>();
            (double a, double b) =  CalculateMNK();

            serie.Add(300, a * 300 + b);
            serie.Add(700, a * 700 + b);

            chart1.Series[1].Points.DataBindXY(serie.Keys, serie.Values);

            chart1.Series[1].LegendText = "линейная аппроксимация";
            chart1.Series[1].Color = Color.Blue;
            chart1.Series[1].BorderWidth = 2;
            chart1.Series[1].ChartType = SeriesChartType.Line; //тип диаграммы

            serie = new Dictionary<double, double>();
            int N = values.Length;

            for (int i = 0; i < N; i++)
            {
                double x = 20 * i + 300;
                serie.Add(x, values[i] - (a * x + b));
            }
            chart1.Series.Add(new Series());
            chart1.Series[2].Points.DataBindXY(serie.Keys, serie.Values);
            chart1.Series[2].LegendText = "ошибка аппроксимация";
            chart1.Series[2].Color = Color.Green;
            chart1.Series[2].BorderWidth = 2;
            chart1.Series[2].ChartType = SeriesChartType.Column; 
        }

        // линейная аппроксимация мнк
        private (double, double) CalculateMNK()
        {
            double Sx = 0; // сумма температур
            double Sy = 0; // сумма давлений
            double Sxy = 0; // сумма их произведений
            double Sx2 = 0; // сумма квадратов
            int N = values.Length;

            for (int i = 0; i < N; i++)
            {
                double x = 20 * i + 300;
                Sx += x;
                Sy += values[i];
                Sx2 += x * x;
                Sxy += x * values[i];
            }

            // решение системы линейных уравнений

            double D = Sx2 * N - Sx * Sx; // детерминант
            double Da = Sxy * N - Sx * Sy; // детерминант a
            double Db = Sx2 * Sy - Sx * Sxy; // детерминант b
            
            double a = Da / D; // коэффициент a
            double b = Db / D; // коэффициент b
            
            return (a, b);
        }

        // загрузка данных из файла
        internal void Open(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    Dictionary<double, double> serie = new Dictionary<double, double>();
                    
                    // чтение данных
                    using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                    {
                        values = JsonSerializer.Deserialize<int[]>(fs);

                        for (int i = 0; i < values.Length; i++)
                        {
                            serie.Add(300 + (i * 20), values[i]);
                        }
                        chart1.Series[0].Points.DataBindXY(serie.Keys, serie.Values);

                        //привязать точки коллекции к ряду данных номер 0
                        chart1.Series[0].LegendText = "Зависимость давления от температуры"; //настроить легенду
                        chart1.Series[0].Color = Color.Blue;
                        chart1.Series[0].BorderWidth = 1;
                        chart1.Series[0].ChartType = SeriesChartType.Line; //тип диаграммы
                        chart1.Series[0].MarkerStyle = MarkerStyle.Circle; //тип маркеров
                        chart1.Series[0].MarkerSize = 10; //
                        chart1.Series[0].MarkerColor = Color.Red; //цвет точки маркера
                        chart1.Series[0].IsValueShownAsLabel = true;  // при true отображает значение точки в виде метки

                        //chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
                        //chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                        chart1.ChartAreas[0].AxisX.Title = "Температура (F)";
                        chart1.ChartAreas[0].AxisX.Interval = 10;
                        //chart1.ChartAreas[0].AxisX.IntervalOffset = 10;
                        chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
                        chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
                        chart1.ChartAreas[0].AxisY.Title = "Давление (psi)";
                        
                    }
                }
                catch (UnauthorizedAccessException uAEx)
                {
                    MessageBox.Show($"Доступ к файлу закрыт: {uAEx.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Нет файла!");
        }

        private void chart1_Move(object sender, EventArgs e)
        {
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            int Ndx;
            PointF thisPt;
            CalloutAnnotation ta;

            string TheText;

            chart1.Annotations.Clear();

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                Cursor = Cursors.Hand;
                string SName = result.Series.Name;
                if (SName == "Series1")
                {
                    Ndx = 0;
                }
                else
                {
                    Ndx = 1;
                }

                DataPoint p = (DataPoint) result.Object;

                int index = result.PointIndex;
                ta = new CalloutAnnotation();
                label1.Text = $"{p}";

                //chart1.Annotations.Add(ta);
                //chart1.Invalidate();
            }
            else
            {
                Cursor = Cursors.Arrow;
            }


        }
    }
}
