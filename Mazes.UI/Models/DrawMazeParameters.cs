using Mazes.Models.MazeMakers;

namespace Mazes.UI.Models {
  public class DrawMazeParameters {
    public (int HorizontalPixels, int VerticalPixels) CanvasSize { get; set; }
    public (int Rows, int Cols) MazeSize { get; set; }
    public MazeAlgorithm MazeAlgorithm { get; set; }
    public bool DrawWalls { get; set; } = true;
    public bool DrawLocations { get; set; }
    public int PathStartRow { get; set; }
    public int PathStartCol { get; set; }
    public bool ColourCells { get; set; }
    public bool DrawDistances { get; set; }
    public bool DrawLongest { get; set; }
    public bool DrawStartStop { get; set; }
  }
}