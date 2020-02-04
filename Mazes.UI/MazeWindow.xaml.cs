using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mazes.Models.MazeMakers;
using Mazes.Models.Models;
using Mazes.UI.Models;

namespace Mazes.UI {
  public partial class MazeWindow {
    private readonly SolidColorBrush _wallsBrush = new SolidColorBrush(Colors.Black);
    private readonly SolidColorBrush _pathBrush = new SolidColorBrush(Colors.Crimson);
    private double _line = 2;
    public static RoutedCommand PrintCommand = new RoutedCommand();
    public static RoutedCommand RefreshCommand = new RoutedCommand();
    public static RoutedCommand CopyCommand = new RoutedCommand();
    private DrawMazeParameters _dmp;

    public MazeWindow(DrawMazeParameters dmp) {
      InitializeComponent();
      PrintCommand.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
      RefreshCommand.InputGestures.Add(new KeyGesture(Key.F5));
      CopyCommand.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Control));
      _dmp = dmp;
      DrawMaze(_dmp);
    }

    private void DrawMaze(DrawMazeParameters dmp) {
      SetUpCanvas(dmp.CanvasSize.HorizontalPixels, dmp.CanvasSize.VerticalPixels);
      List<(MazeAlgorithms, Func<Maze>)> cases = new List<(MazeAlgorithms, Func<Maze>)> {
        (MazeAlgorithms.BinaryTree, () => BinaryTree.Create(dmp.MazeSize.Rows, dmp.MazeSize.Cols)),
        (MazeAlgorithms.SideWinder, () => Sidewinder.Create(dmp.MazeSize.Rows, dmp.MazeSize.Cols)),
        (MazeAlgorithms.AldousBroder, () => AldousBroder.Create(dmp.MazeSize.Rows, dmp.MazeSize.Cols)),
        (MazeAlgorithms.Wilson, () => Wilson.Create(dmp.MazeSize.Rows, dmp.MazeSize.Cols)),
        (MazeAlgorithms.AldousBroderWilson, () => AldousBroderWilson.Create(dmp.MazeSize.Rows, dmp.MazeSize.Cols))
      };
      Maze maze = cases.Switch(dmp.MazeAlgorithm);
      Distances d = null;
      if (dmp.ColourCells) {
        d = maze[dmp.PathStartRow, dmp.PathStartCol].Distances();
      }
      double hCellSize = MazeCanvas.Width / maze.Cols;
      double vCellSize = MazeCanvas.Height / maze.Rows;
      // (0, 0) is top-left
      for (int row = 0; row < maze.Rows; row++) {
        // (hOffset, vOffset) is the top-left of the current cell
        double vOffset = row * vCellSize;
        for (int col = 0; col < maze.Cols; col++) {
          double hOffset = col * hCellSize;
          Cell thisCell = maze[row, col];
          if (dmp.ColourCells) {
            if (dmp.ColourCells) {
              ColourCell(d.Max.Distance, d, thisCell, hCellSize, vCellSize);
            }
          }
          if (dmp.DrawWalls) {
            if (!thisCell.Linked(thisCell.South)) {
              DrawLine(hOffset, vOffset + vCellSize, hOffset + hCellSize, vOffset + vCellSize);
            }
            if (!thisCell.Linked(thisCell.East)) {
              DrawLine(hOffset + hCellSize, vOffset, hOffset + hCellSize, vOffset + vCellSize);
            }
          }
        }
        if (dmp.DrawWalls) {
          DrawLine(0, 0, MazeCanvas.Width, 0);
          DrawLine(0, 0, 0, MazeCanvas.Height);
        }
      }
      if (dmp.DrawDistances) {
        DrawDistances(d, hCellSize, vCellSize);
      }
      if (dmp.DrawLocations) {
        DrawLocations(maze, hCellSize, vCellSize);
      }
      if (dmp.DrawLongest) {
        DrawPath(maze.LongestPath, hCellSize, vCellSize);
      }
      if (dmp.DrawStartStop) {
        // TODO AYS - Draw start and stop icons at the two ends of the longest path
        double iconSize = Math.Min(hCellSize - 4, vCellSize - 4);
        List<Cell> longestPath = maze.LongestPath;
        Cell start = longestPath.First();
        double hOffset = start.Col * hCellSize + (hCellSize - iconSize) / 2;
        double vOffset = start.Row * vCellSize - (vCellSize - iconSize) / 2 + 4;
        Image icon = new Image {
          Width = iconSize,
          Height = iconSize,
          Margin = new Thickness(hOffset, vOffset, iconSize, iconSize),
          Source = new BitmapImage(new Uri("pack://application:,,,/Mazes.UI;component/Images/startbw.png"))
        };
        MazeCanvas.Children.Add(icon);
        Cell stop = longestPath.Last();
        hOffset = stop.Col * hCellSize + (hCellSize - iconSize) / 2;
        vOffset = stop.Row * vCellSize - (vCellSize - iconSize) / 2 + 4;
        icon = new Image {
          Width = iconSize,
          Height = iconSize,
          Margin = new Thickness(hOffset, vOffset, iconSize, iconSize),
          Source = new BitmapImage(new Uri("pack://application:,,,/Mazes.UI;component/Images/stop.png"))
        };
        MazeCanvas.Children.Add(icon);
      }
    }

    private void SetUpCanvas(int hPixels, int vPixels) {
      MazeCanvas.Width = hPixels;
      MazeCanvas.Height = vPixels;
      Width = MazeCanvas.Width + 2 * MazeCanvas.Margin.Left;
      Height = MazeCanvas.Height + 2 * MazeCanvas.Margin.Top;
      MazeCanvas.Children.Clear();
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

    private void DrawLocations(Maze d, double hCellSize, double vCellSize) =>
      d.Cells.ForEach(c => {
        TextBlock tb = new TextBlock {
          Text = c.ToString(),
          Margin = new Thickness(c.Col * hCellSize + 10, c.Row * vCellSize + 10, 0, 0)
        };
        MazeCanvas.Children.Add(tb);
      });

    private void DrawPath(List<Cell> path, double hCellSize, double vCellSize) {
      Cell start = path.First();
      double prevX = start.Col * vCellSize + vCellSize / 2;
      double prevY = start.Row * hCellSize + hCellSize / 2;
      path.Skip(1).ForEach(c => {
        double thisX = c.Col * vCellSize + vCellSize / 2;
        double thisY = c.Row * hCellSize + hCellSize / 2;
        DrawLine(prevX, prevY, thisX, thisY, _pathBrush);
        prevX = thisX;
        prevY = thisY;
      });
    }

    private void DrawLine(double x1, double y1, double x2, double y2, SolidColorBrush brush = null) {
      Line line = new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = brush ?? _wallsBrush, StrokeThickness = _line };
      MazeCanvas.Children.Add(line);
    }

    #region Commands

    private void PrintCommandExecute(object sender, ExecutedRoutedEventArgs e) {
      PrintDialog prnt = new PrintDialog();
      if (prnt.ShowDialog() == true) {
        Size pageSize = new Size(prnt.PrintableAreaWidth, prnt.PrintableAreaHeight);
        MazeCanvas.Measure(pageSize);
        MazeCanvas.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
        prnt.PrintVisual(MazeCanvas, "Maze");
      }
    }

    private void RefreshCommandExecute(object sender, ExecutedRoutedEventArgs e) =>
      DrawMaze(_dmp);

    private void CopyCommandExecute(object sender, ExecutedRoutedEventArgs e) {
      Transform transform = MazeCanvas.LayoutTransform;
      MazeCanvas.LayoutTransform = null;
      Size size = new Size(MazeCanvas.Width, MazeCanvas.Height);
      MazeCanvas.Measure(size);
      MazeCanvas.Arrange(new Rect(size));
      RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
      renderBitmap.Render(MazeCanvas);
      MazeCanvas.LayoutTransform = transform;
      Clipboard.SetImage(renderBitmap);
    }

    #endregion
  }
}