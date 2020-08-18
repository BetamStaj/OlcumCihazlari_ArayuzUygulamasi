﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using Cursor = System.Windows.Forms.Cursor;

namespace YazStaj
{
    public partial class Form1 : Form
    {
        private const string V = "Measurement_type;time_interval;get_value";
        private bool movement = false;
        private Point start = new Point(0, 0);
        private static System.Timers.Timer aTimer;
        private static int textBoxNumber;
        private static string cevap = V;
        private static string measurementType;
        private static int a = 0;
        private static long maks = 30, mini = 5, i=0;
        private Random rnd = new Random();
        private static Color randomColor;
        private static string measType;
        private static bool isTimeSetted = false;
        private static bool isMeasurementTypeDefined = false;
        private static int timevalue = 0;
        private static bool screenClean = true;
        private static bool connected = false;
        private SerialPort ComPort;
        public Form1()
        {
            
            InitializeComponent();
            //labelAppName.Font = new Font(labelAppName.Font.FontFamily, 13);
            buttonSaveData.Enabled = false;
            buttonDisconnect.Enabled = false;
            string[] ports = SerialPort.GetPortNames();
            comboBoxDevice.Items.AddRange(ports);
           
            comboBoxMeasurementType.Items.AddRange(new string[] { "DCV", "ACV", "DCI", "ACI", "R" });
            comboBoxDevice.BackColor = Color.FromArgb(41, 53, 65);
            comboBoxMeasurementType.BackColor = Color.FromArgb(41, 53, 65);
            chart1.ChartAreas[0].BackColor = Color.FromArgb(41, 53, 65);
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.LineColor = Color.FromArgb(226, 227, 229);
            chart1.ChartAreas[0].AxisY.LineColor = Color.FromArgb(226, 227, 229);
            chart1.ChartAreas[0].AxisX.InterlacedColor = Color.FromArgb(226, 227, 229);
            
            for (int i = 0; i < 4; i++)
            {
                chart1.Series["D1"].Points.AddXY(rnd.Next(100), rnd.Next(100));
                chart1.Series["D2"].Points.AddXY(rnd.Next(100), rnd.Next(100));
                chart1.Series["D3"].Points.AddXY(rnd.Next(100), rnd.Next(100));
                chart1.Series["D4"].Points.AddXY(rnd.Next(100), rnd.Next(100));
                chart1.Series["D5"].Points.AddXY(rnd.Next(100), rnd.Next(100));
                chart1.Series["D6"].Points.AddXY(rnd.Next(100), rnd.Next(100));
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics line = e.Graphics;
            Pen p = new Pen(Color.FromArgb(151, 207, 235), 1);
            line.DrawLine(p, 0, 50, this.Size.Width,50);
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Size.Height / 15)
            {
                movement = true;
                start = new Point(e.X, e.Y);

            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (movement)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.start.X, p.Y - this.start.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            movement = false;
        }

        

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Size.Height / 15)
            {
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;

                
            }
            
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            


        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File | *.txt";
            if ( sfd.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(sfd.FileName, FileMode.Create)) 
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(cevap);
                }
            }
            
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if(comboBoxDevice.SelectedItem != null)
            {

                ComPort = new SerialPort(comboBoxDevice.Text, 9600, Parity.None, 8, StopBits.One);

                try
                {
                    buttonSaveData.Enabled = true;
                    buttonDisconnect.Enabled = true;
                    connected = true;
                    //data gelirse eğer SerialPortDataReceived fonksiyonu çalışacak
                    ComPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived); ;

                    //portu açıyorum
                    ComPort.Open();

                    //diğer butonlarımı aktif hale getiriyorum
                   

                }//hatalarimi kontrol ediyorum
                catch (UnauthorizedAccessException) {    }
                catch (System.IO.IOException) {    }
                catch (ArgumentException) {  }

                //kullaniciyi hataya karşı bilgilendiriyorum
               
                


                MessageBox.Show("Connected Successfully!", "Connected",
    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("A port should be chosen!", "Connect Warning",
    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connected = false;
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;

            if (serialPort.IsOpen)
            {

                //gelen data:
                //1#2#3#4#5#6#7#8#9#10#11#12#13#14#15#16#17#18*
                //bu şekilde


                var data = "";
                int asciCode = 0;

                //cihazdan gelen datayi aliyorum
                do
                {

                    asciCode = serialPort.ReadByte();
                    data += (char)asciCode;


                } while ((char)asciCode != '.');

                Console.WriteLine(data);
                Console.ReadLine();
            }
        }

        private void buttonSetInterval_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxSetTimeInterval.TextLength != 0)
                {
                    isTimeSetted = true;
                    textBoxNumber = int.Parse(textBoxSetTimeInterval.Text);
                    timer1.Interval = textBoxNumber;
                    MessageBox.Show("Time interval is setted to " + textBoxNumber, "Time Interval Setted",
       MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    isTimeSetted = false;
                    MessageBox.Show("No Interval has been written yet!", "Set Interval Warning",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("A number must be entered.", "Type Error",
       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
        }

        
        

        private void buttonStart_Click(object sender, EventArgs e)
        {

            Console.WriteLine("Kapi 1");
            ComPort.Write("S");
            if (connected) {
               
            if (isMeasurementTypeDefined && isTimeSetted)
            {
                Console.WriteLine("DATA GELDİ?");
                ComPort.Write("S");

                if (screenClean)
                {
                    chart1.Series.Clear();
                    screenClean = false;
                }
               
                measType = comboBoxMeasurementType.SelectedItem.ToString() + 1;
               
                try
                {
                    chart1.Series.Add(measType);
                    chart1.Series[measType].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;


                }
                catch
                {

                    while (chart1.Series.IsUniqueName(measType) == false)
                    {

                        char lastElement = measType.Last();
                        measType = measType.Remove(measType.Length - 1, 1);
                        double newLastElement = Char.GetNumericValue(lastElement) + 1;
                        measType = measType + newLastElement;






                    }
                    chart1.Series.Add(measType);
                    chart1.Series[measType].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;


                }
               
                    timer1.Start();

                    //   serialPort1.PortName = comboBoxDevice.SelectedItem.ToString();
                    //  serialPort1.Open();
                    //  serialPort1.ReadTimeout = timer1.Interval;



                    



                    //randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    //chart1.Series[measType].Color = randomColor;

                   

                }
            else
            {
                    
                MessageBox.Show("Time inverval or measurement type is empty!", "Starting Failed",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }
            else
            {
                connected = false;
                MessageBox.Show("Not yet connected!", "Starting Failed",
MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            comboBoxDevice.SelectedIndex = -1;
            connected = false;
            MessageBox.Show("Disconnected Successfully!", "Disconnected",
  MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            comboBoxMeasurementType.SelectedIndex = -1;
            textBoxSetTimeInterval.Clear();
            isMeasurementTypeDefined = false;
            isTimeSetted = false;
            timevalue = 0;

        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxSetTimeInterval_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBoxSetTimeInterval.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBoxSetTimeInterval.Text = textBoxSetTimeInterval.Text.Remove(textBoxSetTimeInterval.Text.Length - 1);
            }
        }

        private void buttonMinimize_MouseHover(object sender, EventArgs e)
        {
            buttonMinimize.Image = Properties.Resources.rsz_minimizeback;
        }

        private void buttonExit_MouseLeave(object sender, EventArgs e)
        {
            buttonMinimize.Image = Properties.Resources.rsz_minimizebackmat;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timevalue += textBoxNumber;
                if (connected)
            {
                //string deger = serialPort1.ReadLine();

                cevap += "\n" + measurementType + ";" + timevalue + ";" + a;


                chart1.Series[measType].Points.AddXY(i, a);
                a++;
                //chart1.Series[measType].Points.AddXY(i, deger);
                i++;

            }




        }

        private void buttonDefine_Click(object sender, EventArgs e)
        {
            if (comboBoxMeasurementType.SelectedItem != null)
            {
                isMeasurementTypeDefined = true;
                measurementType = comboBoxMeasurementType.SelectedItem.ToString();
                MessageBox.Show("The measurement type was defined as "+ measurementType, "Measurement Type Warning",
  MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                isMeasurementTypeDefined = false;
                MessageBox.Show("Measurement type should be chosen.", "Measurement Type Warning",
   MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
    }

