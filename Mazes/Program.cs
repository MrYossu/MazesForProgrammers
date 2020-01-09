using System.Diagnostics;
using Mazes.MazeMakers;
using Mazes.Models;

namespace Mazes {
  class Program {
    static void Main(string[] args) {
      //DumpGrid(grid);
      //DumpRandomCells(grid);
      //grid.AllCells.ForEach(c => Debug.WriteLine(c));
      Debug.WriteLine(Sidewinder.Create(10, 10));
    }

    private static void DumpRandomCells(Grid grid) {
      for (int i = 0; i < 10; i++) {
        Debug.WriteLine(grid.RandomCell());
      }
    }

    private static void DumpGrid(Grid grid) {
      for (int row = 0; row < grid.Rows; row++) {
        for (int col = 0; col < grid.Cols; col++) {
          Debug.Write(grid[row, col] + " ");
        }
        Debug.WriteLine("");
      }
      int r = 1;
      int c = 1;
      Debug.WriteLine($"({r}, {c}).North: {grid[r, c].North}");
      Debug.WriteLine($"({r}, {c}).South: {grid[r, c].South}");
      Debug.WriteLine($"({r}, {c}).East:  {grid[r, c].East}");
      Debug.WriteLine($"({r}, {c}).West:  {grid[r, c].West}");
    }
  }
}