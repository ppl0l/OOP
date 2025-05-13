using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsApp.Shapes
{
    [Serializable]
    public class LineShape : DrawingShape
    {
        public override Shape CreateVisual()
        {
            return new Line
            {
                X1 = StartPoint.X,
                Y1 = StartPoint.Y,
                X2 = EndPoint.X,
                Y2 = EndPoint.Y,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
        }
    }
}