using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Barricades
{
    public partial class MainWindow : Window
    {
        private List<Line> lines = new List<Line>();
        private Point? startCoords = null;
        private Point? endCoords = null;

        public MainWindow()
        {
            InitializeComponent();
            DrawGrid();
        }

        private void DrawGrid()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = 50;
                    rect.Height = 50;
                    rect.Stroke = Brushes.LightBlue;
                    Canvas.SetLeft(rect, i * 50);
                    Canvas.SetTop(rect, j * 50);
                    canvas.Children.Add(rect);
                }
            }
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (startCoords == null)
            {
                startCoords = e.GetPosition(canvas);
            }
            else if (endCoords == null)
            {
                endCoords = e.GetPosition(canvas);
                DrawLine();
            }
        }

        private void DrawLine()
        {
            double cellSize = 50;
            double maxLineLength = cellSize * 2.05;
            double lineLength = Math.Sqrt(Math.Pow(endCoords.Value.X - startCoords.Value.X, 2) + Math.Pow(endCoords.Value.Y - startCoords.Value.Y, 2));

            if ((lineLength > maxLineLength) || ((lineLength - cellSize) < 5))
            {
                startCoords = null;
                endCoords = null;
                return;
            }

            if (DoesLineIntersectExistingLines(startCoords.Value, endCoords.Value))
            {
                startCoords = null;
                endCoords = null;
                return;
            }

            Line line = new Line();

            if (canvas.Children.Count % 2 == 0)
            {
                line.Stroke = Brushes.DarkViolet;
            }
            else
            {
                line.Stroke = Brushes.Black;
            }

            line.StrokeThickness = 2;
            line.X1 = startCoords.Value.X;
            line.Y1 = startCoords.Value.Y;
            line.X2 = endCoords.Value.X;
            line.Y2 = endCoords.Value.Y;

            canvas.Children.Add(line);
            lines.Add(line);

            startCoords = null;
            endCoords = null;
        }

        private bool DoesLineIntersectExistingLines(Point p1, Point p2)
        {
            foreach (Line existingLine in lines)
            {
                Point line1Start = new Point(existingLine.X1, existingLine.Y1);
                Point line1End = new Point(existingLine.X2, existingLine.Y2);

                if (DoLinesIntersect(p1, p2, line1Start, line1End))
                {
                    return true;
                }
            }
            return false;
        }

        private bool DoLinesIntersect(Point line1Start, Point line1End, Point line2Start, Point line2End)
        {
            double a1 = line1End.Y - line1Start.Y;
            double b1 = line1Start.X - line1End.X;
            double c1 = a1 * line1Start.X + b1 * line1Start.Y;

            double a2 = line2End.Y - line2Start.Y;
            double b2 = line2Start.X - line2End.X;
            double c2 = a2 * line2Start.X + b2 * line2Start.Y;

            double determinant = a1 * b2 - a2 * b1;

            if (determinant == 0)
            {
                return false;
            }

            double x = (b2 * c1 - b1 * c2) / determinant;
            double y = (a1 * c2 - a2 * c1) / determinant;

            return IsPointOnSegment(x, y, line1Start, line1End) &&
                   IsPointOnSegment(x, y, line2Start, line2End);
        }

        private bool IsPointOnSegment(double x, double y, Point segmentStart, Point segmentEnd)
        {
            const double epsilon = 0.001;
            double minX = Math.Min(segmentStart.X, segmentEnd.X) - epsilon;
            double maxX = Math.Max(segmentStart.X, segmentEnd.X) + epsilon;
            double minY = Math.Min(segmentStart.Y, segmentEnd.Y) - epsilon;
            double maxY = Math.Max(segmentStart.Y, segmentEnd.Y) + epsilon;

            return (x >= minX && x <= maxX) && (y >= minY && y <= maxY);
        }
    }
}
