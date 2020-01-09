﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Models;

namespace Mazes.MazeMakers {
  public static class BinaryTree {
    public static Grid Create(int rows, int cols) {
      Grid grid = new Grid(rows, cols);
      Random r = new Random((int)DateTime.Now.Ticks);
      grid.Cells.ForEach(c => {
        List<Cell> neighbours = new List<Cell> { c.North, c.East }.Where(c1 => c1 != null).ToList();
        if (neighbours.Count == 1) {
          c.Link(neighbours.Single());
        } else if (neighbours.Count == 2) {
          // The line below biases the split between vertical and horizontal corridors. If the bias is > 50, we get longer horizontal corridors, if < 50, we get longer vertical corridors
          c.Link(neighbours.Skip(r.Next(100) > 50 ? 0 : 1).First());
        }
      });
      return grid;
    }
  }
}