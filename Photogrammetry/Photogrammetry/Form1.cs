using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Photogrammetry
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        [System.Runtime.InteropServices.DllImport("user32.dll",
        CharSet = System.Runtime.InteropServices.CharSet.Auto,
        CallingConvention =
            System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags,
        int dx,
        int dy,
        int dwData,
        int dwExtraInfo);
        //Нормированные абсолютные координаты
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        //Нажатие на левую кнопку мыши
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //Поднятие левой кнопки мыши
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        //перемещение указателя мыши
        private const int MOUSEEVENTF_MOVE = 0x0001;
        // координата кнопки X
        int X1 = 0;
        // координата кнопки Y
        int Y1 = 0;
        // объявляем порт
        SerialPort port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);

        // номер фото
        int photo_number = 0;


        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Cursor.Position.X.ToString());
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            textBox1.Text = String.Format("X = {0}, Y = {1}", Cursor.Position.X, Cursor.Position.Y);
            X1 = Cursor.Position.X;
            Y1 = Cursor.Position.Y;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Position = new System.Drawing.Point(X1, Y1);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);


            //int X = 65535;
            //int Y = 1230;
            //Перемещение курсора на указанные координаты
            //mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width, System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height, X, Y);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button3_Click(null, null);
                    }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            label2.Text = port.PortName + " Порт не подключен";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!port.IsOpen)  port.Open();
            label2.Text = port.PortName + " Порт подключен";
            port.BaudRate = 9600;
            port.WriteTimeout = 500;
            port.ReadTimeout = 500;

            string q = this.numericUpDown1.Value.ToString();
            port.Write(q);
            port.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            port.PortName = comboBox1.Text.ToString();
            port.Open();
            label2.Text = port.PortName + " Порт подключен";
            port.BaudRate = 9600;
            port.WriteTimeout = 500;
            port.ReadTimeout = 500;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            photo_number = int.Parse(this.numericUpDown1.Value.ToString());
            timer2.Interval= ((1000*70) / photo_number) + 5000;
            timer3.Interval = int.Parse(this.numericUpDown2.Value.ToString()) * 1000;

            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.timer2.Stop();
            if (photo_number > 0)
            {
                button2_Click(null, null);
                this.label4.Text = "Фотографируем";
                timer3.Start();
            }
            else
            {
                this.label5.Text = "";
                MessageBox.Show("Готово!");
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.timer3.Stop();
            if (photo_number > 0)
            {
                photo_number = photo_number - 1;
                button4_Click(null, null);
                this.label4.Text = "Вращаем стол";
                this.label5.Text = "Осталось "+ photo_number.ToString() + " фотографий";
                timer2.Start();
            }
            else {
                this.label5.Text = "";
                MessageBox.Show("Готово!"); 
            }
        }
    }
}
