using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mazes.Models.Models;
using Mazes.UI.Models;

namespace Mazes.UI {
  public partial class MainWindow {
    public MainWindow() {
      InitializeComponent();
      AlgorithmCmb.ItemsSource = Enum.GetValues(typeof(MazeAlgorithms)).Cast<MazeAlgorithms>();
      AlgorithmCmb.SelectedValue = MazeAlgorithms.Wilson;
    }

    private void GenerateBtn_Click(object sender, RoutedEventArgs e) {
      DrawMazeParameters dmp = new DrawMazeParameters {
        CanvasSize = (Convert.ToInt32(HorizontalPixelsTb.Text), Convert.ToInt32(VerticalPixelsTb.Text)),
        MazeSize = (Convert.ToInt32(RowsTb.Text), Convert.ToInt32(ColsTb.Text)),
        MazeAlgorithm = (MazeAlgorithms)AlgorithmCmb.SelectedValue,
        DrawWalls = DrawWallsChk.IsChecked ?? false,
        DrawLocations = DrawLocationsChk.IsChecked ?? false,
        ColourCells = ColourDistancesFromRb.IsChecked ?? false,
        DrawDistances = ShowDistancesChk.IsChecked ?? false,
        PathStartRow = Convert.ToInt32(PathFromX.Text),
        PathStartCol = Convert.ToInt32(PathFromY.Text),
        DrawLongest = DrawLongestRb.IsChecked ?? false
      };
      MazeWindow mw = new MazeWindow(dmp) {
        Owner = GetWindow(this)
      };
      mw.Show();
    }

    private void PathGb_Checked(object sender, RoutedEventArgs e) {
      if (PathFromSp != null && ColourDistancesFromRb != null) {
        PathFromSp.IsEnabled = ColourDistancesFromRb.IsChecked ?? false;
      }
    }
  }
}