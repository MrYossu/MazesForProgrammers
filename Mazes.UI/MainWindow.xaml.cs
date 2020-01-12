using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using Mazes.Models.MazeMakers;
using Mazes.Models.Models;

namespace Mazes.UI {
  public partial class MainWindow {
    private readonly SolidColorBrush _brush = new SolidColorBrush(Colors.Black);
    private double _line = 1;

    public MainWindow() {
      InitializeComponent();
      DrawGrid();
    }

    private void DrawGrid() {
      Width = Canvas.Width + 2 * Canvas.Margin.Left;
      Height = Canvas.Height + 2 * Canvas.Margin.Top;
      int hCells = 10;
      int vCells = 10;
      Maze maze = Sidewinder.Create(vCells, hCells);
      Distances d = maze[0, 0].Distances();
      Debug.WriteLine($"Distances contains {d.Cells.Count()} cell(s)");
      Debug.WriteLine(maze.ToString(c => d[c].ToString("000")));
      //Debug.WriteLine(maze.ToString(c => $"{c.Row},{c.Col}"));
      double hCellSize = Canvas.Width / hCells;
      double vCellSize = Canvas.Height / vCells;
      // (0, 0) is top-left
      for (int row = 0; row < vCells; row++) {
        // (hOffset, vOffset) is the top-left of the current cell
        double vOffset = row * vCellSize;
        for (int col = 0; col < hCells; col++) {
          double hOffset = col * hCellSize;
          Cell thisCell = maze[row, col];
          if (!thisCell.Linked(thisCell.South)) {
            DrawLine(hOffset, vOffset + vCellSize, hOffset + hCellSize, vOffset + vCellSize);
          }
          if (!thisCell.Linked(thisCell.East)) {
            DrawLine(hOffset + hCellSize, vOffset, hOffset + hCellSize, vOffset + vCellSize);
          }
        }
      }
      DrawLine(0, 0, Canvas.Width, 0);
      DrawLine(0, 0, 0, Canvas.Height);
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