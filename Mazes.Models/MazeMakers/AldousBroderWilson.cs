using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Models.Models;

namespace Mazes.Models.MazeMakers {
  public class AldousBroderWilson {
    // Combination algorithm that starts off using Aldous-Broder, then switches to Wilson, hopefully gaining the benefits of both

    public static Maze Create(int rows, int cols) {
      int abMaxPercentage = 80;
      int totalCells = rows * cols;
      int tenPercent = totalCells / 10;
      Maze maze = new Maze(rows, cols);
      Random r = maze.R;

      // Start off with Aldous-Broder. Run this until we have carved a specified percentage of cells
      int unvisited = rows * cols - 1;
      Cell current = maze.Cells.Rand(r);
      int currentPercent = tenPercent;
      int n = 0;
      while (unvisited > 0 && currentPercent < abMaxPercentage) {
        if (totalCells - unvisited == currentPercent) {
          currentPercent += tenPercent;
          n++;
        }
        Cell next = current.Neighbours.Rand(r);
        if (next.Links.Count == 0) {
          current.Link(next);
          unvisited--;
        }
        current = next;
      }

      // Now switch to Wilson's
      List<Cell> walk = new List<Cell>();
      Cell first = maze.Cells.Where(c => !c.Links.Any()).Rand(r);
      current = maze.Cells.Where(c => !c.Links.Any()).Rand(r);
      walk.Add(maze.Cells.First(c => c.Row == current.Row && c.Col == current.Col));
      while (maze.Cells.Any(c => !c.Links.Any())) {
        Cell next = walk.Last().Neighbours.Rand(r);
        if (next == first || next.Links.Any()) {
          walk.Add(next);
          walk.Zip(walk.Skip(1), (thisCell, nextCell) => (thisCell, nextCell)).ForEach(c => c.thisCell.Link(c.nextCell));
          if (n == 25 || n % 50 == 0) {
          }
          if (maze.Cells.Any(c => !c.Links.Any())) {
            walk = new List<Cell> { maze.Cells.Where(c => !c.Links.Any()).Rand(r) };
          }
        } else if (walk.Contains(next)) {
          walk = walk.Take(walk.IndexOf(next) + 1).ToList();
        } else {
          walk.Add(next);
        }
      }

      return maze;
    }
  }
}