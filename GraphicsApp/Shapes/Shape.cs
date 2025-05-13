using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsApp.Shapes
{
    public abstract class DrawingShape
    {
        public Brush Stroke { get; set; } = Brushes.Black;
        public Brush Fill { get; set; } = Brushes.Transparent;
        public double StrokeThickness { get; set; } = 1.0;
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public abstract Shape CreateVisual();
        public virtual void UpdateBounds(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public virtual void Reset()
        {
            StartPoint = new Point(0, 0);
            EndPoint = new Point(0, 0);
        }
    }
}