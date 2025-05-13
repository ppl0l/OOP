using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Ink;

namespace GraphicsApp.Shapes
{
    public class PolylineShape : DrawingShape
    {
        public PointCollection Points { get; } = new PointCollection();

        public override Shape CreateVisual()
        {
            return new System.Windows.Shapes.Polyline
            {
                Points = Points,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
        }

        public override void UpdateBounds(Point start, Point end)
        {
            if (Points.Count == 0) StartPoint = start;
            Points.Add(end);
            EndPoint = end;
        }
    }
    }