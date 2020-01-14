using System.Collections.Generic;
using System.Linq;

namespace Mazes.Models.Models {
  public class Cell {
    public int Row { get; }
    public int Col { get; }
    public List<Cell> Links { get; }
    public Cell North { get; set; }

    public bool NorthernBoundary =>
      North == null;

    public Cell South { get; set; }

    public bool SouthernBoundary =>
      South == null;

    public Cell East { get; set; }

    public bool EasternBoundary =>
      East == null;

    public Cell West { get; set; }

    public bool WesternBoundary =>
      West == null;

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
        frontier.ForEach(c => c.Links.Where(cl => !distances.Cells.Select(dc => dc.Cell).Contains(cl)).ForEach(link => {
          distances.Add(link, distances[c] + 1);
          newFrontier.Add(link);
        }));
        frontier = newFrontier;
      }
      return distances;
    }
  }
}