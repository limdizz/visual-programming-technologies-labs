using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Stripes
{
    public partial class Form1 : Form
    {
        private List<Tuple<Rectangle, Color>> stripes = new List<Tuple<Rectangle, Color>>();
        private int stripesCountToWin = 5; 
        private int maxStripesCount = 2 * 5;
        private int clickedCounter = 0;

        public Form1()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;
            this.MouseClick += Form1_MouseClick;
            this.Size = new Size(1280, 720);

            Timer timer = new Timer
            {
                Interval = 1000 
            };

            timer.Tick += (sender, e) =>
            {
                if (stripes.Count < maxStripesCount)
                {
                    Random random = new Random();
                    int width = 150;
                    int height = 20;
                    int x = random.Next(0, this.Width - width);
                    int y = random.Next(0, this.Height - height);
                    Rectangle newStripe = new Rectangle(x, y, width, height);
                    Color fillColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

                    if (random.Next(2) == 0) 
                    {
                        newStripe = new Rectangle(x, y, width, height);
                    }
                    else
                    {
                        newStripe = new Rectangle(x, y, height, width);
                    }

                    stripes.Add(new Tuple<Rectangle, Color>(newStripe, fillColor));

                    this.Invalidate();
                }
                else
                {
                    timer.Stop();
                    MessageBox.Show("Game over! You lost.");
                }
            };
            timer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Tuple<Rectangle, Color> stripe in stripes)
            {
                e.Graphics.FillRectangle(new SolidBrush(stripe.Item2), stripe.Item1);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = stripes.Count - 1; i >= 0; i--)
            {
                if (stripes[i].Item1.Contains(e.Location))
                {
                    bool canRemove = true;
                    foreach (Tuple<Rectangle, Color> otherStripe in stripes)
                    {
                        if (stripes[i].Item1 != otherStripe.Item1 && stripes[i].Item1.IntersectsWith(otherStripe.Item1))
                        {
                            canRemove = false;
                            break;
                        }
                    }
                    if (canRemove)
                    {
                        Timer t = new Timer
                        {
                            Interval = 1000
                        };
                        stripes.RemoveAt(i);
                        clickedCounter++;
                        this.Invalidate();
                        if (clickedCounter == stripesCountToWin)
                        {
                            t.Tick += delegate { Close(); t.Stop(); };
                            t.Start();
                            MessageBox.Show("Congratulations! You won!");
                        
                        } else if (stripes.Count == 0)
                        {
                            t.Tick += delegate { Close(); t.Stop(); };
                            t.Start();
                            MessageBox.Show("Congratulations! You clicked all lines and won!");
                        }
                    }
                }
            }
        }
    }
}
