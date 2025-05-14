using GraphicsApp.Shapes;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicsApp
{
    public partial class MainWindow : Window
    {
        private DrawingShape _currentShape;
        private bool _isDrawing;
        private readonly ObservableCollection<DrawingShape> _shapes = new ObservableCollection<DrawingShape>();

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

            if (!_isDrawing)
            {
                StartNewShape(point);
            }
            else if (_currentShape?.IsMultiPoint == true)
            {
                _currentShape.AddPoint(point);
            }

            UpdateCanvas();
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

        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDrawing || _currentShape == null) return;

            if (!_currentShape.IsMultiPoint)
            {
                _currentShape.UpdateBounds(_currentShape.StartPoint, e.GetPosition(drawingCanvas));
                CompleteCurrentShape();
                UpdateCanvas();
            }
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing || _currentShape == null || e.LeftButton != MouseButtonState.Pressed)
                return;

            _currentShape.UpdateBounds(_currentShape.StartPoint, e.GetPosition(drawingCanvas));
            UpdateCanvas();
        }

        private void CompleteCurrentShape()
        {
            if (_currentShape == null) return;

            if (_currentShape.IsValid)
            {
                _shapes.Add(_currentShape);
            }

            _isDrawing = false;
            _currentShape = null;
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (_isDrawing && _currentShape?.IsMultiPoint == true)
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

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _shapes.Clear();
            _isDrawing = false;
            _currentShape = null;
            UpdateCanvas();
        }
    }
}