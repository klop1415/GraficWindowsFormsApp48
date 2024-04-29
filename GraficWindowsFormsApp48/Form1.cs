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
        public Form1()
        {
            InitializeComponent();

            string[] daysOfWeek = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
            int[] numberOfVisitors = { 1200, 1450, 1504, 1790, 2450, 1900, 3050 };

            // Установим палитру
            chart1.Palette = ChartColorPalette.SeaGreen;

            // Заголовок графика
            chart1.Titles.Add("Посетители");

            // Добавляем последовательность
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                Series series = chart1.Series.Add(daysOfWeek[i]);

                // Добавляем точку
                series.Points.Add(numberOfVisitors[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        // загрузка данных из файла
        internal static int[] Open(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    using (FileStream openStream = File.OpenRead(file))
                    {
                        var list =
                            JsonSerializer.Deserialize<int[]>(openStream);
                        return list;
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
            return null;
        }
    }
}
