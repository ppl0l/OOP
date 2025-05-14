using System;
using System.Collections.Generic;

namespace GraphicsApp.UndoRedo
{
    public class Undo_redo
    {
        private readonly Stack<Action> _undoStack = new Stack<Action>();
        private readonly Stack<Action> _redoStack = new Stack<Action>();

        public void AddAction(Action doAction, Action undoAction) { }
        public void Undo() { }

        public void Redo() { }

        public void Clear() { }
    }
}