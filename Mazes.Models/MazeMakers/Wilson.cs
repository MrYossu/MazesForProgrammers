using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mazes.Models.Models;

namespace Mazes.Models.MazeMakers {
  public static class Wilson {
    public static Maze Create(int rows, int cols) {
      Maze maze = new Maze(rows, cols);
      Random r = new Random((int)DateTime.Now.Ticks);
      List<Cell> walk = new List<Cell>();
      // Pick a random cell to mark as visited. This will be the end point of our first walk
      Cell first = maze.Cells.Rand(r);
      bool[,] visited = new bool[rows, cols];
      visited[first.Row, first.Col] = true;
      // Pick a starting cell for our first walk
      Cell current = maze.Cells.Where(c => c.Row != first.Row && c.Col != first.Col).Rand(r);
      walk.Add(maze.Cells.First(c => c.Row == current.Row && c.Col == current.Col));
      while (visited.Cast<bool>().Any(cell => !cell)) {
        Cell next = walk.Last().Neighbours.Rand(r);
        if (visited[next.Row, next.Col]) {
          walk.Add(next);
          // Carve cells along the current walk
          walk.Zip(walk.Skip(1), (thisCell, nextCell) => (thisCell, nextCell)).ForEach(c => {
            c.thisCell.Link(c.nextCell);
            visited[c.thisCell.Row, c.thisCell.Col] = true;
            visited[c.nextCell.Row, c.nextCell.Col] = true;
          });
          walk.Last().Link(next);
          // Start a new walk from a random unvisited cell
          if (maze.Cells.Any(c => !visited[c.Row, c.Col])) {
            walk = new List<Cell> { maze.Cells.Where(c => !visited[c.Row, c.Col]).Rand(r) };
          }
        } else if (walk.Contains(next)) {
          // Mark all cells that we are about to snip as not visited
          walk.Skip(walk.IndexOf(next) + 1).ForEach(c => visited[c.Row, c.Col] = false);
          // Snip off all cells after the last time we visited this cell
          walk = walk.Take(walk.IndexOf(next) + 1).ToList();
        } else {
          walk.Add(next);
        }
      }
      return maze;
    }
  }
}