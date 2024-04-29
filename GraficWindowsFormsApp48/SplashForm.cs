using System.Reflection.Emit;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GraficWindowsFormsApp48
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
            timer1.Interval = 400; // 0.4 секунд
            timer1.Enabled = true;
        }
        
        int ticks = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            label1.Text = $"{10 - ticks}";
            Opacity = ticks * 10;

            if (ticks == 11)
            {
                timer1.Enabled = false;
                //Thread.Sleep(1200);
                //Close();
                DialogResult = DialogResult.OK;
            }
            ticks++;
        }
    }
}
