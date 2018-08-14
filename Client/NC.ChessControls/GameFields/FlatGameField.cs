using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using NC.Shared.Data;

namespace NC.ChessControls.GameFields
{
    /// <summary>
    /// Common flat game field.
    /// </summary>
    public class FlatGameField : StackPanel
    {
        /// <summary>
        /// Dependency property for <see cref="IsReadOnly"/> property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty;

        /// <summary>
        /// Dependency property for <see cref="WhileCellBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty WhileCellBrushProperty;

        /// <summary>
        /// Dependency property for <see cref="BlackCellBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty BlackCellBrushProperty;

        /// <summary>
        /// Dependency property for <see cref="FieldFrame"/> property.
        /// </summary>
        public static readonly DependencyProperty FieldFrameProperty;

        static FlatGameField()
        {
            FieldFrameProperty = DependencyProperty.Register(
                nameof(FieldFrame),
                typeof(VirtualField),
                typeof(FlatGameField),
                new PropertyMetadata(new VirtualField(), (source, args) => ((FlatGameField)source).FieldFrameChanged()));

            IsReadOnlyProperty = DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(FlatGameField),
                new PropertyMetadata(default(bool)));

            WhileCellBrushProperty = DependencyProperty.Register(
                nameof(WhileCellBrush),
                typeof(Brush),
                typeof(FlatGameField),
                new PropertyMetadata(Brushes.WhiteSmoke));

            BlackCellBrushProperty = DependencyProperty.Register(
                nameof(BlackCellBrush),
                typeof(Brush),
                typeof(FlatGameField),
                new PropertyMetadata(Brushes.Black));
        }
        
        private void FieldFrameChanged()
        {
            if (ActualWidth < 1 || ActualHeight < 1)
            {
                return;
            }

            DrawField(FieldFrame ?? new VirtualField());
        }

        private void DrawField(VirtualField field)
        {
            // Clear old field frame
            ClearGrid();

            // Draw square grid.
            DrawGrid(field);

            for (int x = 0; x < field.Width; x++)
            {
                for (int y = 0; y < field.Height; y++)
                {
                    
                }
            }
        }

        private void DrawGrid(VirtualField field)
        {
            var cellX = ActualWidth / field.Width;
            var cellY = ActualHeight / field.Height;
            var lineColor = Brushes.White; 

            // Rows
            for (int i = 0; i <= field.Height; i++)
            {
                var horizontalLine = CreateLine(0, (int)(i * cellX), (int)(field.Width * cellX), (int)(i * cellX));
                horizontalLine.Stroke = Brushes.White;
                CanvasRef.Children.Add(horizontalLine);
            }

            // Columns
            for (int j = 0; j <= field.Width; j++)
            {
                var verticalLine = CreateLine((int)(j * cellY), 0, (int)(j * cellY), (int)(field.Height * cellY));
                verticalLine.Stroke = lineColor;
                CanvasRef.Children.Add(verticalLine);
            }
        }

        private static Line CreateLine(int x1, int y1, int x2, int y2)
        {
            return new Line { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, StrokeThickness = 1 };
        }

        private void ClearGrid()
        {
            CanvasRef.Children.Clear();
        }

        /// <summary>
        /// Constructor for <see cref="FlatGameField"/>.
        /// </summary>
        public FlatGameField()
        {
            CanvasRef = new Canvas();
            Children.Add(CanvasRef);

            MinWidth = 60;
            MinHeight = 60;
        }

        /// <summary>
        /// Game field.
        /// </summary>
        public VirtualField FieldFrame
        {
            get
            {
                return (VirtualField)GetValue(FieldFrameProperty);
            }
            set
            {
                SetValue(FieldFrameProperty, value);
            }
        }

        /// <summary>
        /// White cell color brush.
        /// </summary>
        public Brush WhileCellBrush
        {
            get
            {
                return (Brush)GetValue(WhileCellBrushProperty);
            }
            set
            {
                SetValue(WhileCellBrushProperty, value);
            }
        }

        /// <summary>
        /// Black cell color brush.
        /// </summary>
        public Brush BlackCellBrush
        {
            get
            {
                return (Brush)GetValue(BlackCellBrushProperty);
            }
            set
            {
                SetValue(BlackCellBrushProperty, value);
            }
        }

        /// <summary>
        /// Is read only field.
        /// </summary>
        /// <remarks>Prevent mouse marking.</remarks>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }

            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        private Canvas CanvasRef { get; }
    }
}