using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Mazes.Models.Models;
using Mazes.UI.Models;

namespace Mazes.UI {
  public partial class MainWindow {
    public MainWindow() =>
      InitializeComponent();

    private void GenerateBtn_Click(object sender, RoutedEventArgs e) {
      DrawMazeParameters dmp = new DrawMazeParameters {
        CanvasSize = (Convert.ToInt32(HorizontalPixelsTb.Text), Convert.ToInt32(VerticalPixelsTb.Text)),
        MazeSize = (Convert.ToInt32(RowsTb.Text), Convert.ToInt32(ColsTb.Text)),
        MazeAlgorithm = new List<(string, Func<MazeAlgorithms>)> {
          ("Binary tree", () => MazeAlgorithms.BinaryTree),
          ("Sidewinder", () => MazeAlgorithms.SideWinder)
        }.Switch(((ComboBoxItem)AlgorithmCmb.SelectedValue).Content.ToString()),
        DrawWalls = DrawWallsChk.IsChecked ?? false,
        DrawLocations = DrawLocationsChk.IsChecked ?? false,
        ColourCells = ColourCellsChk.IsChecked ?? false,
        DrawLongest = DrawLongestChk.IsChecked ?? false
      };
      MazeWindow mw = new MazeWindow(dmp) {
        Owner = GetWindow(this)
      };
      mw.Show();
    }
  }
}