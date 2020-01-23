using System;
using Mazes.Models.Models;

namespace Mazes.Models.MazeMakers {
  public static class AldousBroder {
    public static Maze Create(int rows, int cols) {
      Maze maze = new Maze(rows, cols);
      Random r = new Random((int)DateTime.Now.Ticks);
      int unvisited = rows * cols - 1;
      Cell current = maze.Cells.Rand();
      while (unvisited > 0) {
        Cell next = current.Neighbours.Rand();
        if (next.Links.Count == 0) {
          current.Link(next);
          unvisited--;
        }
        current = next;
      }
      return maze;
    }
  }
}