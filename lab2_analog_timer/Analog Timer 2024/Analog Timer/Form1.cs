using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnalogTimer
{
    public partial class Form1 : Form
    {
        private AnalogClockTimer hourClock;
        private AnalogClockTimer minuteClock;
        private AnalogClockTimer secondClock;
        private Button startButton;

        public Form1()
        {
            InitializeComponent();

            hourClock = new AnalogClockTimer(40, 40, 100, 100, 0, 24 * 60 * 60 );
            minuteClock = new AnalogClockTimer(160, 40, 100, 100, 0, 60 * 60);
            secondClock = new AnalogClockTimer(280, 40, 100, 100, 0, 60 );

            startButton = new Button();
            startButton.Text = "Старт";
            startButton.Location = new Point(400, 90);
            startButton.Click += StartButton_Click;

            Controls.Add(hourClock);
            Controls.Add(minuteClock);
            Controls.Add(secondClock);
            Controls.Add(startButton);
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            hourClock.Start();
            minuteClock.Start();
            secondClock.Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }


    public class AnalogClockTimer : Control
    {
        private int minValue;
        private int maxValue;
        private int value;
        private bool isDragging;
        private Point mouseStart;

        public AnalogClockTimer(int x, int y, int width, int height, int minValue, int maxValue)
        {
            Location = new Point(x, y);
            Size = new Size(width, height);
            this.minValue = minValue;
            this.maxValue = maxValue;
            value = minValue;

            MouseDown += AnalogClockTimer_MouseDown;
            MouseMove += AnalogClockTimer_MouseMove;
            MouseUp += AnalogClockTimer_MouseUp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.FillEllipse(Brushes.White, ClientRectangle);
            g.DrawEllipse(Pens.Black, ClientRectangle);

            double angle = (value - minValue) / (double)(maxValue - minValue) * 360 * Math.PI / 180;
            int x = ClientSize.Width / 2 + (int)(ClientSize.Width / 2 * Math.Sin(angle));
            int y = ClientSize.Height / 2 - (int)(ClientSize.Height / 2 * Math.Cos(angle));

            g.DrawLine(Pens.Black, ClientSize.Width / 2, ClientSize.Height / 2, x, y);
        }

        private void AnalogClockTimer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ClientRectangle.Contains(e.Location))
            {
                isDragging = true;
                mouseStart = e.Location;
            }
        }

        private void AnalogClockTimer_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                double angle = Math.Atan2(e.Y - ClientSize.Height / 2, e.X - ClientSize.Width / 2);
                value = (int)Math.Round(angle * 180 / Math.PI * (maxValue - minValue) / 360 + minValue);
                if (value < minValue) value = minValue;
                if (value > maxValue) value = maxValue;
                Invalidate();
            }
        }

        private void AnalogClockTimer_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        public void Start()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            value++;
            if (value > maxValue)
                value = minValue;
            Invalidate();
        }
    }
}
