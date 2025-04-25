using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Stripes
{
    public partial class MainWindow : Window
    {
        private List<Tuple<Rect, SolidColorBrush>> stripes = new List<Tuple<Rect, SolidColorBrush>>();
        private int stripesCountToWin = 5;
        private int maxStripesCount = 2 * 5; 
        private int clickedCounter = 0;
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) 
            };
            timer.Tick += (sender, e) =>
            {
                if (stripes.Count < maxStripesCount)
                {
                    Random random = new Random();
                    int width = random.Next(50, 150);
                    int height = random.Next(50, 150);
                    double x = random.Next(0, (int)canvas.ActualWidth + width);
                    double y = random.Next(0, (int)canvas.ActualHeight + height);

                    Rect newStripe;
                    SolidColorBrush fillColor = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));

                    if (random.Next(2) == 0) 
                    {
                        newStripe = new Rect(x, y, width, height);
                    }
                    else
                    {
                        newStripe = new Rect(x, y, height, width); 
                    }

                    stripes.Add(new Tuple<Rect, SolidColorBrush>(newStripe, fillColor));

                    DrawStripe(newStripe, fillColor);
                }
                else
                {
                    timer.Stop();
                    MessageBox.Show("Game over! You lose.");
                }
            };
            timer.Start();
        }

        private void DrawStripe(Rect rect, SolidColorBrush color)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = rect.Width,
                Height = rect.Height,
                Fill = color
            };
            Canvas.SetLeft(rectangle, rect.Left);
            Canvas.SetTop(rectangle, rect.Top);
            canvas.Children.Add(rectangle);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = stripes.Count - 1; i >= 0; i--)
            {
                if (stripes[i].Item1.Contains(e.GetPosition(canvas)))
                {
                    bool canRemove = true;
                    foreach (Tuple<Rect, SolidColorBrush> otherStripe in stripes)
                    {
                        if (stripes[i].Item1 != otherStripe.Item1 && stripes[i].Item1.IntersectsWith(otherStripe.Item1))
                        {
                            canRemove = false;
                            break;
                        }
                    }
                    if (canRemove)
                    {
                        DispatcherTimer t = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromSeconds(1) 
                        };

                        stripes.RemoveAt(i);
                        clickedCounter++;
                        canvas.Children.Clear();

                        foreach (Tuple<Rect, SolidColorBrush> stripe in stripes)
                        {
                            DrawStripe(stripe.Item1, stripe.Item2);
                        }

                        if (stripes.Count == 0) {
                            t.Tick += delegate { Close(); t.Stop(); };
                            t.Start();
                            MessageBox.Show("Congratulations! You win!");
                        } 

                        else if (clickedCounter == stripesCountToWin) {
                            t.Tick += delegate { Close(); t.Stop(); };
                            t.Start();
                            MessageBox.Show("Congratulations! You clicked required amount of lines!");
                        }
                    }
                }
            }
        }
    }
}