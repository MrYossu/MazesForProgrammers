using System;
using System.Windows;
using Mazes.Models.MazeMakers;
using Mazes.UI.Models;

namespace Mazes.UI {
  public partial class MainWindow {
    public MainWindow() {
      InitializeComponent();
      AlgorithmCmb.ItemsSource = MazeAlgorithm.Algorithms;
      AlgorithmCmb.SelectedIndex = 3;
    }

    private void GenerateBtn_Click(object sender, RoutedEventArgs e) {
      DrawMazeParameters dmp = new DrawMazeParameters {
        CanvasSize = (Convert.ToInt32(HorizontalPixelsTb.Text), Convert.ToInt32(VerticalPixelsTb.Text)),
        MazeSize = (Convert.ToInt32(RowsTb.Text), Convert.ToInt32(ColsTb.Text)),
        MazeAlgorithm = (MazeAlgorithm)AlgorithmCmb.SelectedValue,
        Braid = BraidChk.IsChecked ?? false,
        BraidProbability = Convert.ToDouble(BraidProbabilityTb.Text),
        DrawWalls = DrawWallsChk.IsChecked ?? false,
        DrawLocations = DrawLocationsChk.IsChecked ?? false,
        ColourCells = ColourDistancesFromRb.IsChecked ?? false,
        DrawDistances = ShowDistancesChk.IsChecked ?? false,
        PathStartRow = Convert.ToInt32(PathFromX.Text),
        PathStartCol = Convert.ToInt32(PathFromY.Text),
        DrawLongest = DrawLongestRb.IsChecked ?? false,
        DrawStartStop = DrawStartStopRb.IsChecked ?? false
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

    private void BraidChk_Checked(object sender, RoutedEventArgs e) {
      if (BraidProbabilityTb != null && BraidChk != null) {
        BraidProbabilityTb.IsEnabled = BraidChk.IsChecked ?? false;
      }
    }
  }
}