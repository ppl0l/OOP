namespace GraphicsApp.Shapes
{
    public interface IShapePlugin
    {
        string ShapeName { get; }
        DrawingShape CreateShape();

        DrawingShape CreateShapeWithPar(Dictionary<string, object> parameters);
    }
}