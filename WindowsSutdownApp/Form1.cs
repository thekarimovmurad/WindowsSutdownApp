using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsSutdownApp
{
    public partial class Form1 : Form
    {
        public delegate void TimerDelegate();
        public Form1()
        {   
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StartTimerAndAction(Shutdown);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            StartTimerAndAction(Restart);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            StartTimerAndAction(Sleep);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            StartTimerAndAction(LockScreen);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Shutdown()
        {
            Process.Start("shutdown", "/s /t 0");
        }
        private void Restart()
        {
            Process.Start("shutdown", "/r /t 0");
        }
        private void LockScreen()
        {
            Process.Start("rundll32.exe", "user32.dll,LockWorkStation");
        }
        private void Sleep()
        {
            SetSuspendState(false, true, true);
        }
        
        public void StartTimerAndAction(TimerDelegate action)
        {
            int time;
            if (string.IsNullOrEmpty(textBox1.Text))
                time = 0;
            else
                time = int.Parse(textBox1.Text);

            Timer timer = new Timer();
            timer.Interval = (time == 0) ? 1 : time* 1000;
            timer.Tick += (sender, e) =>
            {
                action.Invoke();
                timer.Stop();
            };
            timer.Start();
            textBox1.Text = "";
        }

        [System.Runtime.InteropServices.DllImport("powrprof.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        //private void howmanysecond(object sender, EventArgs e)
        //{
        //    textBox1
        //}
    }
}
