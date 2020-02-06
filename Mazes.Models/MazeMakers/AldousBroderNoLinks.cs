using System;
using System.Linq;
using Mazes.Models.Models;

namespace Mazes.Models.MazeMakers {
  public static class AldousBroderNoLinks {
    // This is a modification of the standard Aldous-Broder algorithm that will not pick a visited neighbour if there is a non-visited one available
    public static Maze Create(int rows, int cols) {
      Maze maze = new Maze(rows, cols);
      Random r = new Random((int)DateTime.Now.Ticks);
      int unvisited = rows * cols - 1;
      Cell current = maze.Cells.Rand(r);
      while (unvisited > 0) {
        Cell next = current.Neighbours.Any(c => r.Next(1000) < 750 && c.Links.Count == 0)
          ? current.Neighbours.Where(c => c.Links.Count == 0).Rand(r)
          : current.Neighbours.Rand(r);
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