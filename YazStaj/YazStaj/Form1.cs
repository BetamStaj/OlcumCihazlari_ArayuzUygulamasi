using System;
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
        public Form1()
        {
            
            InitializeComponent();
            this.BackColor = Color.FromArgb(49, 51, 50); // this should be pink-ish
            label1.ForeColor = Color.FromArgb(230, 219, 87);
            label1.Font = new Font(label1.Font.FontFamily, 13);
            button1.Enabled = false;
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            comboBox1.Items.AddRange(new string[] { "simple", "continuous", "perfect", "perfect continuous" });
            comboBox2.Items.AddRange(new string[] { "DCV", "ACV", "DCI", "ACI", "R" });
            






        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics line = e.Graphics;
            Pen p = new Pen(Color.White, 1);
            line.DrawLine(p, 0, 30, this.Size.Width,30);
           
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != null)
            {
                button1.Enabled = true;

            }
            else
            {
                MessageBox.Show("Some text", "Some title",
    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength!=0)
            {
                textBoxNumber = int.Parse(textBox1.Text);
                timer1.Interval = textBoxNumber;
            }
            else
            {
                MessageBox.Show("Some text", "Some title",
    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            

            //aTimer = new System.Timers.Timer();
           // aTimer.Interval = textBoxNumber;
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            
            
            
           // cevap += "\n"+ measurementType+" "+ textBoxNumber+" "+ a ;
            
            //a++;
            

        }
        private  void plotsomethin()
        {
            //chart1.ChartAreas[0].AxisX.Minimum = mini;
            //chart1.ChartAreas[0].AxisX.Maximum = maks;

            //chart1.ChartAreas[0].AxisY.Minimum = mini;
            //chart1.ChartAreas[0].AxisY.Maximum = maks;
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(mini, maks);
            //chart1.Series[0].Points.AddXY(i, a);
            //mini++;
            //maks++;
            //i++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                timer1.Start();
                randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                chart1.Series[0].Color = randomColor;
            }
            else
            {
                MessageBox.Show("Some text", "Some title",
    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          

            //aTimer.Elapsed += OnTimedEvent;
            //aTimer.Elapsed += plotsomethin;



            // Have the timer fire repeated events (true is the default)
            //aTimer.AutoReset = true;

            // Start the timer
            //aTimer.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //aTimer.Enabled = false;
            timer1.Stop();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cevap += "\n" + measurementType + " " + textBoxNumber + " " + a;

            //chart1.ChartAreas[0].AxisX.Minimum = mini;
            //chart1.ChartAreas[0].AxisX.Maximum = maks;

            //chart1.ChartAreas[0].AxisY.Minimum = mini;
            //chart1.ChartAreas[0].AxisY.Maximum = maks;
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(mini, maks);
            

            chart1.Series[0].Points.AddXY(i, a);
            a++;
            //mini++;
            //maks++;
            i++;
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            measurementType = comboBox2.SelectedItem.ToString();

        }
    }
    }

