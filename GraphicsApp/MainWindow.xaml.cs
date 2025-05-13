using GraphicsApp.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace GraphicsApp
{
    public partial class MainWindow : Window
    {
        private DrawingShape _currentShape;
        private bool _isDrawing;
        private readonly ObservableCollection<DrawingShape> _shapes = new ObservableCollection<DrawingShape>();
        private readonly Stack<List<DrawingShape>> _undoStack = new Stack<List<DrawingShape>>();
        private readonly Stack<List<DrawingShape>> _redoStack = new Stack<List<DrawingShape>>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            shapeComboBox.ItemsSource = ShapeFactory.AvailableShapes;
            shapeComboBox.SelectedIndex = 0;

            strokeColorComboBox.ItemsSource = new[]
            {
                new { Name = "Black", Brush = Brushes.Black },
                new { Name = "Red", Brush = Brushes.Red },
                new { Name = "Green", Brush = Brushes.Green },
                new { Name = "Blue", Brush = Brushes.Blue }
            };
            strokeColorComboBox.DisplayMemberPath = "Name";
            strokeColorComboBox.SelectedValuePath = "Brush";
            strokeColorComboBox.SelectedIndex = 0;

            fillColorComboBox.ItemsSource = new[]
            {
                new { Name = "Transparent", Brush = Brushes.Transparent },
                new { Name = "Red", Brush = Brushes.Red },
                new { Name = "Green", Brush = Brushes.Green },
                new { Name = "Blue", Brush = Brushes.Blue }
            };
            fillColorComboBox.DisplayMemberPath = "Name";
            fillColorComboBox.SelectedValuePath = "Brush";
            fillColorComboBox.SelectedIndex = 0;

            thicknessSlider.Value = 1;
        }

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var point = e.GetPosition(drawingCanvas);
            string shapeType = shapeComboBox.SelectedItem.ToString();

            if (shapeType == "Polygon" || shapeType == "Polyline")
            {
                if (!_isDrawing || !IsCurrentShapeOfType(shapeType))
                {
                    StartNewShape(point);
                }
                else
                {
                    AddPointToCurrentShape(point);
                }
            }
            else
            {
                CompleteCurrentShape();
                StartNewShape(point);
            }

            UpdateCanvas();
        }

        private bool IsCurrentShapeOfType(string shapeType)
        {
            return (shapeType == "Polygon" && _currentShape is PolygonShape) ||
                   (shapeType == "Polyline" && _currentShape is PolylineShape);
        }

        private void StartNewShape(Point startPoint)
        {
            _currentShape = ShapeFactory.Create(shapeComboBox.SelectedItem.ToString());
            if (_currentShape == null) return;

            _currentShape.Stroke = (Brush)strokeColorComboBox.SelectedValue;
            _currentShape.Fill = (Brush)fillColorComboBox.SelectedValue;
            _currentShape.StrokeThickness = thicknessSlider.Value;
            _currentShape.StartPoint = startPoint;
            _currentShape.EndPoint = startPoint;

            _isDrawing = true;
        }

        private void AddPointToCurrentShape(Point point)
        {
            _currentShape.UpdateBounds(_currentShape.StartPoint, point);

            var polygon = _currentShape as PolygonShape;
            var polyline = _currentShape as PolylineShape;

            if ((polygon != null && polygon.Points.Count >= 2) ||
                (polyline != null && polyline.Points.Count >= 2))
            {
                SaveState();
            }
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing || _currentShape == null || e.LeftButton != MouseButtonState.Pressed)
                return;

            _currentShape.UpdateBounds(_currentShape.StartPoint, e.GetPosition(drawingCanvas));
            UpdateCanvas();
        }

        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDrawing || _currentShape == null) return;

            if (!(_currentShape is PolygonShape) && !(_currentShape is PolylineShape))
            {
                _currentShape.UpdateBounds(_currentShape.StartPoint, e.GetPosition(drawingCanvas));
                CompleteCurrentShape();
            }
            UpdateCanvas();
        }

        private void CompleteCurrentShape()
        {
            if (_currentShape == null) return;

            bool isValid = false;
            var polygon = _currentShape as PolygonShape;
            var polyline = _currentShape as PolylineShape;

            if (polygon != null)
            {
                isValid = polygon.Points.Count >= 2;
            }
            else if (polyline != null)
            {
                isValid = polyline.Points.Count >= 2;
            }
            else
            {
                isValid = _currentShape.StartPoint != _currentShape.EndPoint;
            }

            if (isValid)
            {
                SaveState();
                _shapes.Add(_currentShape);
            }

            _isDrawing = false;
            _currentShape = null;
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (_isDrawing && (_currentShape is PolygonShape || _currentShape is PolylineShape))
            {
                CompleteCurrentShape();
            }
            base.OnMouseRightButtonUp(e);
        }

        private void UpdateCanvas()
        {
            drawingCanvas.Children.Clear();
            foreach (var shape in _shapes)
            {
                drawingCanvas.Children.Add(shape.CreateVisual());
            }

            if (_isDrawing && _currentShape != null)
            {
                drawingCanvas.Children.Add(_currentShape.CreateVisual());
            }
        }

        private void SaveState()
        {
            _undoStack.Push(new List<DrawingShape>(_shapes));
            _redoStack.Clear();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e) { }

        private void RedoButton_Click(object sender, RoutedEventArgs e) { }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SaveState();
            _shapes.Clear();
            _isDrawing = false;
            _currentShape = null;
            UpdateCanvas();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) { }
        private void LoadButton_Click(object sender, RoutedEventArgs e) { }
        }
    }