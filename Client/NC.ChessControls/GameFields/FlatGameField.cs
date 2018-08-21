﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using NC.ChessControls.Data;
using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.GameField;

namespace NC.ChessControls.GameFields
{
    /// <summary>
    /// Common flat game field.
    /// </summary>
    public class FlatGameField : StackPanel
    {
        private readonly static Point DefaultCell = new Point(-1, -1);

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

        /// <summary>
        /// Dependency property for <see cref="IconsProvider"/> property.
        /// </summary>
        public static readonly DependencyProperty IconsProviderProperty;

        /// <summary>
        /// Dependency property for <see cref="MovableCellsBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty MovableCellsBrushProperty;

        private readonly List<Rectangle> _selectedPoints = new List<Rectangle>();

        private Point _selectedCell = DefaultCell;

        private PieceMasterFactory _masterFactory = new PieceMasterFactory(new VirtualField());
        
        static FlatGameField()
        {
            MovableCellsBrushProperty = DependencyProperty.Register(
                nameof(MovableCellsBrush),
                typeof(Brush),
                typeof(FlatGameField),
                new PropertyMetadata(Brushes.LightGreen));

            IconsProviderProperty = DependencyProperty.Register(
                nameof(IconsProvider),
                typeof(IIconsProvider),
                typeof(FlatGameField),
                new PropertyMetadata(new SimpleChessIconsProvider()));

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

        /// <summary>
        /// Constructor for <see cref="FlatGameField"/>.
        /// </summary>
        public FlatGameField()
        {
            CanvasRef = new Canvas();
            Children.Add(CanvasRef);
        }

        /// <summary>
        /// Movable cells brush color.
        /// </summary>
        public Brush MovableCellsBrush
        {
            get
            {
                return (Brush)GetValue(MovableCellsBrushProperty);
            }

            set
            {
                SetValue(MovableCellsBrushProperty, value);
            }
        }

        /// <summary>
        /// Field icons provider.
        /// </summary>
        public IIconsProvider IconsProvider
        {
            get
            {
                return (IIconsProvider)GetValue(IconsProviderProperty);
            }
            set
            {
                SetValue(IconsProviderProperty, value);
            }
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

        private double CellX
        {
            get
            {
                if (FieldFrame == null || FieldFrame.Width == 0)
                {
                    return 0;
                }

                return ActualWidth / (FieldFrame.Width + 1);
            }
        }

        private double CellY
        {
            get
            {
                if (FieldFrame == null || FieldFrame.Height == 0)
                {
                    return 0;
                }

                return ActualHeight / (FieldFrame.Height + 1);
            }
        }

        private Canvas CanvasRef { get; }

        /// <summary>
        /// a,b,c,d... border x offset.
        /// </summary>
        private double NamedX => CellX / 2;

        /// <summary>
        /// 1,2,3,4... border y offset.
        /// </summary>
        private double NamedY => CellY / 2;

        /// <inheritdoc/>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            FieldFrameChanged();
        }

        private Line CreateLine(double x1, double y1, double x2, double y2)
        {
            return new Line
            {
                X1 = x1 + NamedX,
                Y1 = y1 + NamedY,
                X2 = x2 + NamedX,
                Y2 = y2 + NamedY,
                StrokeThickness = 1.5
            };
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
            _masterFactory = new PieceMasterFactory(field);

            // Clear old field frame
            ClearGrid();

            // Draw square grid.
            DrawGrid(field);

            // + 2 for this: a,b,c,d.... & 1,2,3,4...
            for (int x = 0; x < field.Width + 2; x++)
            {
                for (int y = 0; y < field.Height + 2; y++)
                {
                    if (y == 0 || x == 0 || x == field.Width + 1 || y == field.Height + 1)
                    {
                        DrawNamedCell(x, y, field);
                        continue;
                    }

                    var realX = x - 1;
                    var realY = y - 1;

                    // Black and white grid
                    if (realX % 2 == 0)
                    {
                        if (realY % 2 == 0)
                        {
                            DrawRectagle(realX, realY, BlackCellBrush, -1);
                        }
                    }
                    else
                    {
                        if (realY % 2 != 0)
                        {
                            DrawRectagle(realX, realY, BlackCellBrush, - 1);
                        }
                    }

                    // Draw piece icon
                    DrawIcon(realX, realY, field[realX, realY]);
                }
            }
        }

        private void DrawNamedCell(int x, int y, VirtualField field)
        {
            var grid = new Grid();

            bool isCorner = x == 0 && y == 0 || x == field.Width + 1 && y == field.Height + 1 ||
                            x == 0 && y == field.Height + 1 || x == field.Width + 1 && y == 0;

            var width = isCorner ? NamedX : CellX;
            var height = isCorner ? NamedY : CellY;
            var rectangle = new Rectangle { Fill = Brushes.Transparent, Width = width, Height = height };
            grid.Children.Add(rectangle);

            var leftOffset = x == 0 ? 0 : NamedX;
            var topOffset = y == 0 ? 0 : NamedY;

            CanvasRef.Children.Add(grid);
            Canvas.SetLeft(grid, x * CellX - leftOffset);
            Canvas.SetTop(grid, y * CellY - topOffset);
            SetZIndex(grid, -1);

            if (isCorner)
            {
                // No chars in the corners
                return;
            }

            var textBlock = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold
            };

            grid.Children.Add(textBlock);

            if (x == 0 || x == field.Width + 1)
            {
                textBlock.Text = ((char)(field.Width + Convert.ToInt32('1') - y)).ToString();
                rectangle.Width = NamedX;
            }
            else
            {
                textBlock.Text = ((char)(Convert.ToInt32('a') - 1 + x)).ToString();
                rectangle.Height = NamedY;
            }
        }

        private void DrawIcon(int x, int y, ChessPiece chessPiece)
        {
            var image = IconsProvider?.GetIcon(chessPiece);
            if (image != null)
            {
                image.Width = CellX;
                image.Height = CellY;
                CanvasRef.Children.Add(image);
                Canvas.SetLeft(image, x * CellX + NamedX);
                Canvas.SetTop(image, y * CellY + NamedY);
                Panel.SetZIndex(image, 1);
                image.MouseDown += OnIconMouseDown;
                image.Tag = new Point(x, y);
            }
        }

        private void OnIconMouseDown(object sender, MouseButtonEventArgs args)
        {
            var selectedCell = (Point)((Image)sender).Tag;
            if (args.LeftButton == MouseButtonState.Pressed)
            {
                var selectedCellTemp = _selectedCell;
                ClearSelection();

                if (selectedCellTemp != selectedCell)
                {
                    PieceMasterBase master;
                    if (_masterFactory.TryGetMaster((int)selectedCell.X, (int)selectedCell.Y, out master))
                    {
                        SetSelection(selectedCell, master.GetMovements());
                        _selectedCell = selectedCell;
                    }
                }
                else
                {
                    _selectedCell = DefaultCell;
                }
            }
        }

        private void SetSelection(Point selectedCell, IEnumerable<Point> movableCells)
        {
            var markedCells = movableCells.Union(
                new[]
                {
                    selectedCell
                });

            foreach (var markedCell in markedCells)
            {
                var rectangle = DrawRectagle((int)markedCell.X, (int)markedCell.Y, MovableCellsBrush, 0);
                _selectedPoints.Add(rectangle);
            }
        }

        private Rectangle DrawRectagle(int x, int y, Brush color, int zIndex)
        {
            var rectangle = new Rectangle { Fill = color, Width = CellX, Height = CellY };

            CanvasRef.Children.Add(rectangle);
            Canvas.SetLeft(rectangle, x * CellX + NamedX);
            Canvas.SetTop(rectangle, y * CellY + NamedY);
            Panel.SetZIndex(rectangle, zIndex);
            return rectangle;
        }

        private void DrawGrid(VirtualField field)
        {
            var lineColor = Brushes.Black;

            // Rows
            for (int j = 0; j <= field.Height; j++)
            {
                var horizontalLine = CreateLine(0, j * CellY, field.Width * CellX, j * CellY);
                horizontalLine.Stroke = lineColor;
                CanvasRef.Children.Add(horizontalLine);
                SetZIndex(horizontalLine, 1);
            }

            // Columns
            for (int i = 0; i <= field.Width; i++)
            {
                var verticalLine = CreateLine(i * CellX, 0, i * CellX, field.Height * CellY);
                verticalLine.Stroke = lineColor;
                CanvasRef.Children.Add(verticalLine);
                SetZIndex(verticalLine, 1);
            }
        }
        
        private void ClearSelection()
        {
            foreach (var selectedPoint in _selectedPoints)
            {
                CanvasRef.Children.Remove(selectedPoint);
            }

            _selectedPoints.Clear();
            _selectedCell = DefaultCell;
        }

        private void ClearGrid()
        {
            foreach (var image in CanvasRef.Children.OfType<Image>())
            {
                image.MouseDown -= OnIconMouseDown;
            }

            ClearSelection();
            CanvasRef.Children.Clear();
        }
    }
}