using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsApp.Shapes
{
    public class PolylineShape : DrawingShape
    {
        private readonly List<Point> _points = new List<Point>();
        private Point? _previewPoint;

        public override bool IsMultiPoint => true;
        public override bool IsValid => _points.Count > 1;

        public override void AddPoint(Point point)
        {
            _points.Add(point);
        }

        public override void UpdateBounds(Point start, Point end)
        {
            _previewPoint = end;
        }

        public override Shape CreateVisual()
        {
            var polyline = new Polyline
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };

            foreach (var pt in _points)
                polyline.Points.Add(pt);

            if (_previewPoint.HasValue)
                polyline.Points.Add(_previewPoint.Value);

            return polyline;
        }

        public override void Reset()
        {
            _points.Clear();
            _previewPoint = null;
        }
    }
}