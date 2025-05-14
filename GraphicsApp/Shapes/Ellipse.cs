using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsApp.Shapes
{
    public class EllipseShape : DrawingShape
    {
        public override Shape CreateVisual()
        {
            return new Ellipse
            {
                Width = Math.Abs(EndPoint.X - StartPoint.X),
                Height = Math.Abs(EndPoint.Y - StartPoint.Y),
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness,
                Margin = new Thickness(
                    Math.Min(StartPoint.X, EndPoint.X),
                    Math.Min(StartPoint.Y, EndPoint.Y),
                    0, 0)
            };
        }

        public override void UpdateBounds(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
        }
    }
}