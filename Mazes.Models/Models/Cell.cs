﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mazes.Models.Models {
  public class Cell {
    public int Row { get; }
    public int Col { get; }
    public List<Cell> Links { get; }
    public Cell North { get; set; }
    public Cell South { get; set; }
    public Cell East { get; set; }
    public Cell West { get; set; }

    public Cell(int row, int col) {
      Row = row;
      Col = col;
      Links = new List<Cell>();
    }

    public override string ToString() =>
      $"Cell: ({Row}, {Col})";

    public Cell Link(Cell cell, bool bidirectional = true) {
      Links.Add(cell);
      if (bidirectional) {
        cell.Link(this, !bidirectional);
      }
      return this;
    }

    public Cell Unlink(Cell cell, bool bidirectional = true) {
      Links.Remove(cell);
      if (bidirectional) {
        cell.Unlink(this, !bidirectional);
      }
      return this;
    }

    public bool Linked(Cell cell) =>
      Links.Contains(cell);

    public IEnumerable<Cell> Neighbours =>
      new List<Cell> { North, South, East, West }.Where(c => c != null);

    public Distances Distances() {
      Distances distances = new Distances(this);
      List<Cell> frontier = new List<Cell> { this };
      while (frontier.Any()) {
        List<Cell> newFrontier = new List<Cell>();
        //Debug.WriteLine($"frontier contains {frontier.Count} cell(s)");
        frontier.ForEach(c => c.Links.Where(cl => !distances.Cells.Contains(cl)).ForEach(link => {
          distances.Add(link, distances[c] + 1);
          newFrontier.Add(link);
        }));
        frontier = newFrontier;
        //Debug.WriteLine($"  newFrontier contains {newFrontier.Count} cell(s)");
      }
      return distances;
    }
  }
}