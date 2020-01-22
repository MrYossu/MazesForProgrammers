﻿namespace Mazes.UI.Models {
  public class DrawMazeParameters {
    public (int HorizontalPixels, int VerticalPixels) CanvasSize { get; set; }
    public (int Rows, int Cols) MazeSize { get; set; }
    public MazeAlgorithms MazeAlgorithm { get; set; }
    public bool DrawWalls { get; set; } = true;
    public bool DrawLocations { get; set; }
    public bool DrawLongest { get; set; }
    public bool ColourCells { get; set; }
  }
}