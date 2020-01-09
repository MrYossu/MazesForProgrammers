﻿using System;
using System.Collections.Generic;
using Mazes.Models;

namespace Mazes.MazeMakers {
  public static class Sidewinder {
    public static Grid Create(int rows, int cols) {
      Grid grid = new Grid(rows, cols);
      Random r = new Random((int)DateTime.Now.Ticks);
      List<Cell> run = new List<Cell>();
      grid.Cells.ForEach(c => {
        if (c.West == null) {
          run = new List<Cell>();
        }
        run.Add(c);
        bool atEast = c.East == null;
        bool atNorth = c.North == null;
        // If the target is > 50 we favour horizontal runs, if < 50, we favour vertical runs
        bool close = atEast || !atNorth && r.Next(100) > 50;
        if (close) {
          Cell cr = run.Rand();
          if (cr.North != null) {
            cr.Link(cr.North);
          }
          run.Clear();
        } else {
          c.Link(c.East);
        }
      });
      return grid;
    }
  }
}