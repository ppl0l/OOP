using System;
using System.Collections.Generic;

namespace GraphicsApp.Shapes
{
    public static class ShapeFactory
    {
        private static readonly Dictionary<string, Func<DrawingShape>> _shapeCreators = new Dictionary<string, Func<DrawingShape>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Line"] = () => new LineShape(),
            ["Rectangle"] = () => new RectangleShape(),
            ["Ellipse"] = () => new EllipseShape(),
            ["Polyline"] = () => new PolylineShape(),
            ["Polygon"] = () => new PolygonShape()
        };

        public static DrawingShape Create(string shapeName)
        {
            return _shapeCreators.TryGetValue(shapeName, out var creator) ? creator() : null;
        }

        public static IEnumerable<string> AvailableShapes => _shapeCreators.Keys;
    }
}