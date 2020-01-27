using System.Diagnostics;
using Mazes.Models.MazeMakers;
using Mazes.Models.Models;

namespace Mazes.Console {
  class Program {
    static void Main(string[] args) {
      //DumpGrid(grid);
      //DumpRandomCells(grid);
      //grid.AllCells.ForEach(c => Debug.WriteLine(c));
      Debug.WriteLine(Wilson.Create(15, 15));
    }

    private static void DumpRandomCells(Maze maze) {
      for (int i = 0; i < 10; i++) {
        Debug.WriteLine(maze.RandomCell());
      }
    }

    private static void DumpGrid(Maze maze) {
      for (int row = 0; row < maze.Rows; row++) {
        for (int col = 0; col < maze.Cols; col++) {
          Debug.Write(maze[row, col] + " ");
        }
        Debug.WriteLine("");
      }
      int r = 1;
      int c = 1;
      Debug.WriteLine($"({r}, {c}).North: {maze[r, c].North}");
      Debug.WriteLine($"({r}, {c}).South: {maze[r, c].South}");
      Debug.WriteLine($"({r}, {c}).East:  {maze[r, c].East}");
      Debug.WriteLine($"({r}, {c}).West:  {maze[r, c].West}");
    }
  }
}