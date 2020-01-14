﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Mazes.Models.MazeMakers;
using Mazes.Models.Models;

namespace Mazes.UI {
  public partial class MainWindow {
    private readonly SolidColorBrush _brush = new SolidColorBrush(Colors.Black);
    private double _line = 1;
    public static RoutedCommand PrintCommand = new RoutedCommand();

    public MainWindow() {
      InitializeComponent();
      MazeCanvas.Width = 400;
      MazeCanvas.Height = 400;
      PrintCommand.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
      DrawMaze();
    }

    private void DrawMaze() {
      Width = MazeCanvas.Width + 2 * MazeCanvas.Margin.Left;
      Height = MazeCanvas.Height + 2 * MazeCanvas.Margin.Top;
      int hCells = 10;
      int vCells = 10;
      Maze maze = Sidewinder.Create(vCells, hCells);
      Distances d = maze[0, 0].Distances();
      CellDistance max = d.Max;
      Debug.WriteLine($"Maximum distance: {max.Distance} in cell ({max.Cell.Row}, {max.Cell.Col})");
      //Debug.WriteLine(maze.ToString(c => d[c].ToString("000")));
      //Debug.WriteLine(maze.ToString(c => $"{c.Row},{c.Col}"));
      double hCellSize = MazeCanvas.Width / hCells;
      double vCellSize = MazeCanvas.Height / vCells;
      // (0, 0) is top-left
      for (int row = 0; row < vCells; row++) {
        // (hOffset, vOffset) is the top-left of the current cell
        double vOffset = row * vCellSize;
        for (int col = 0; col < hCells; col++) {
          double hOffset = col * hCellSize;
          Cell thisCell = maze[row, col];
          ColourCell(d.Max.Distance, d, thisCell, hCellSize, vCellSize);
          if (!thisCell.Linked(thisCell.South)) {
            DrawLine(hOffset, vOffset + vCellSize, hOffset + hCellSize, vOffset + vCellSize);
          }
          if (!thisCell.Linked(thisCell.East)) {
            DrawLine(hOffset + hCellSize, vOffset, hOffset + hCellSize, vOffset + vCellSize);
          }
        }
      }
      DrawLine(0, 0, MazeCanvas.Width, 0);
      DrawLine(0, 0, 0, MazeCanvas.Height);
      DrawDistances(d, hCellSize, vCellSize);
    }

    private void ColourCell(int maxDist, Distances d, Cell thisCell, double hCellSize, double vCellSize) {
      float intensity = (float)(maxDist - d[thisCell]) / maxDist;
      byte dark = (byte)(255 * intensity);
      byte bright = (byte)(127 * intensity + 128);
      Rectangle rect = new Rectangle {
        Fill = new SolidColorBrush(Color.FromRgb(dark, bright, dark)),
        Width = hCellSize,
        Height = vCellSize,
        Margin = new Thickness(thisCell.Col * hCellSize, thisCell.Row * vCellSize, hCellSize, vCellSize)
      };
      MazeCanvas.Children.Add(rect);
    }

    private void DrawDistances(Distances d, double hCellSize, double vCellSize) =>
      d.Cells.ForEach(cellDistance => {
        TextBlock tb = new TextBlock {
          Text = cellDistance.Distance.ToString(),
          Margin = new Thickness(cellDistance.Cell.Col * hCellSize + 10, cellDistance.Cell.Row * vCellSize + 10, 0, 0)
        };
        MazeCanvas.Children.Add(tb);
      });

    private void DrawLine(double x1, double y1, double x2, double y2) {
      Line line = new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = _brush, StrokeThickness = _line };
      MazeCanvas.Children.Add(line);
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

        MazeCanvas.Children.Add(line);
      }
    }

    private void PrintCommandExecute(object sender, ExecutedRoutedEventArgs e) {
      PrintDialog prnt = new PrintDialog();
      if (prnt.ShowDialog() == true) {
        Size pageSize = new Size(prnt.PrintableAreaWidth, prnt.PrintableAreaHeight);
        MazeCanvas.Measure(pageSize);
        MazeCanvas.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
        prnt.PrintVisual(MazeCanvas, "Maze");
      }
    }
  }
}