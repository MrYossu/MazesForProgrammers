using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Models.Models {
  public class Distances {
    public Cell Root { get; }
    private readonly Dictionary<Cell, int> _distances;

    public Distances(Cell root) {
      Root = root;
      _distances = new Dictionary<Cell, int> {
        { root, 0 }
      };
    }

    public IEnumerable<CellDistance> Cells =>
      _distances.Keys.Select(k => new CellDistance(k, _distances[k]));

    public void Add(Cell c, int d) =>
      _distances.Add(c, d);

    public int this[Cell c] =>
      _distances[c];

    public CellDistance Max => 
      new CellDistance(_distances.Keys.Last(), _distances[_distances.Keys.Last()]);

    public List<Cell> PathFrom(int row, int col) {
      CellDistance start = Cells.FirstOrDefault(c => c.Cell.Row == row && c.Cell.Col == col);
      if (start == null) {
        throw new ArgumentException($"No such cell at ({row}, {col})");
      }
      List<Cell> path = new List<Cell> { start.Cell };
      CellDistance current = start;
      while (current.Cell != Root) {
        CellDistance next = Cells.First(c => current.Cell.Links.Contains(c.Cell) && c.Distance == current.Distance - 1);
        current = next;
        path.Add(current.Cell);
      }
      return path;
    }
  }
}