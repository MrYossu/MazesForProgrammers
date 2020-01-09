using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mazes.UI {
  public partial class MainWindow {
    private SolidColorBrush _brush = new SolidColorBrush(Colors.Black);
    private double _line = 1;

    public MainWindow() {
      InitializeComponent();
      //Debug.WriteLine($"({Canvas.Width}, {Canvas.Height})");
      DrawGrid();
    }

    private void DrawGrid() {
      Width = Canvas.Width + 2 * Canvas.Margin.Left;
      Height = Canvas.Height + 2 * Canvas.Margin.Top;
      Debug.WriteLine($"Width: {Width}, Height: {Height}");
      int nCells = 25;
      double hCellSize = Canvas.Width / nCells;
      double vCellSize = Canvas.Height / nCells;
      Debug.WriteLine($"Canvas.Width: {Canvas.Width}, Height: {Canvas.Height}");
      Debug.WriteLine($"hCellSize: {hCellSize}, vCellSize: {vCellSize}");
      // (0, 0) is top-left
      for (int row = 0; row < nCells; row++) {
        double vOffset = row * vCellSize;
        //Debug.WriteLine($"row: {row}, vOffset: {vOffset}");
        for (int col = 0; col < nCells; col++) {
          double hOffset = col * hCellSize;
          DrawLine(hOffset, vOffset, hOffset + hCellSize, vOffset);
          DrawLine(hOffset, vOffset, hOffset, vOffset + vCellSize);
        }
      }
      DrawLine(0, Canvas.Height, Canvas.Width, Canvas.Height);
      DrawLine(Canvas.Width, 0, Canvas.Width, Canvas.Height);
    }

    private void DrawLine(double x1, double y1, double x2, double y2) {
      Line line = new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = _brush, StrokeThickness = _line };
      Canvas.Children.Add(line);
    }

    private void DrawEllipse() {
      for (double rads = 0; rads < 2 * Math.PI; rads += Math.PI / 48) {
        Line line = new Line {
          X1 = 400,
          Y1 = 400,
          X2 = 350 * Math.Sin(rads) + 400,
          Y2 = 125 * Math.Cos(rads) + 400,
          Stroke = new SolidColorBrush(Color.FromRgb((byte)(rads * 40), 0, 200)),
          StrokeThickness = 2
        };

        Canvas.Children.Add(line);
      }
    }
  }
}