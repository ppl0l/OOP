using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace GraphicsApp.Shapes
{
    public class PolygonShape : DrawingShape
    {
        public PointCollection Points { get; } = new PointCollection();

        public override Shape CreateVisual()
        {
            return new System.Windows.Shapes.Polygon
            {
                Points = Points,
                Stroke = Stroke,
                Fill = Fill,
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