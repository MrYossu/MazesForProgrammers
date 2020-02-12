using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Models.Models;

namespace Mazes.Models.MazeMakers {
  public class WilsonJb {
    // This is a C# conversion of the author's code, as opposed to my implementation from his description
    // Whilst my implementation was more consistent in execution time, his was about 30% faster on average
    public static Maze Create(int rows, int cols) {
      Maze maze = new Maze(rows, cols);
      Random r = maze.R;
      List<Cell> unvisited = maze.Cells.ToList();
      Cell first = unvisited.Rand(r);
      unvisited.Remove(first);

      while (unvisited.Any()) {
        Cell next = unvisited.Rand(r);
        List<Cell> walk = new List<Cell> { next };

        while (unvisited.Contains(next)) {
          next = next.Neighbours.Rand(r);
          if (walk.IndexOf(next) >= 0) {
            walk = walk.Take(walk.IndexOf(next) + 1).ToList();
          } else {
            walk.Add(next);
          }
        }

        walk.Zip(walk.Skip(1), (thisCell, nextCell) => (thisCell, nextCell)).ForEach(c => c.thisCell.Link(c.nextCell));
        walk.ForEach(c => unvisited.Remove(c));
      }

      return maze;
    }
  }
}