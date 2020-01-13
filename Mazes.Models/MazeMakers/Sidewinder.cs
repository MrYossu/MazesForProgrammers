using System;
using System.Collections.Generic;
using Mazes.Models.Models;

namespace Mazes.Models.MazeMakers {
  public static class Sidewinder {
    public static Maze Create(int rows, int cols) {
      Maze maze = new Maze(rows, cols);
      Random r = new Random((int)DateTime.Now.Ticks);
      List<Cell> run = new List<Cell>();
      maze.Cells.ForEach(c => {
        if (c.WesternBoundary) {
          run = new List<Cell>();
        }
        run.Add(c);
        // If the target is > 50 we favour horizontal runs, if < 50, we favour vertical runs
        bool close = c.EasternBoundary || !c.NorthernBoundary && r.Next(100) > 50;
        if (close) {
          Cell cr = run.Rand();
          if (!cr.NorthernBoundary) {
            cr.Link(cr.North);
          }
          run.Clear();
        } else {
          c.Link(c.East);
        }
      });
      return maze;
    }
  }
}