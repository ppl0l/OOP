using System;
using GraphicsApp.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GraphicsApp.UndoRedo
{
    public class ShapeHistory
    {
        private readonly Stack<List<DrawingShape>> _undoStack = new Stack<List<DrawingShape>>();
        private readonly Stack<List<DrawingShape>> _redoStack = new Stack<List<DrawingShape>>();

        public void SaveState(List<DrawingShape> shapes)
        {
            _undoStack.Push(new List<DrawingShape>(shapes));
            _redoStack.Clear();
        }

        public bool Undo(ObservableCollection<DrawingShape> shapes)
        {
            if (_undoStack.Count > 0)
            {
                _redoStack.Push(new List<DrawingShape>(shapes));
                shapes.Clear();
                foreach (var shape in _undoStack.Pop())
                {
                    shapes.Add(shape);
                }
                return true;
            }
            return false;
        }

        public bool Redo(ObservableCollection<DrawingShape> shapes)
        {
            if (_redoStack.Count > 0)
            {
                _undoStack.Push(new List<DrawingShape>(shapes));
                shapes.Clear();
                foreach (var shape in _redoStack.Pop())
                {
                    shapes.Add(shape);
                }
                return true;
            }
            return false;
        }

        public void ClearRedoStack()
        {
            _redoStack.Clear();
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }

}
