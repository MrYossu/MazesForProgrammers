using System.Collections.Generic;

namespace Mazes.Models.Models {
  public class Distances {
    public Cell Root { get; }
    private Dictionary<Cell, int> _distances;

    public Distances(Cell root) {
      Root = root;
      _distances = new Dictionary<Cell, int> {
        { root, 0 }
      };
    }

    public IEnumerable<Cell> Cells =>
      _distances.Keys;

    public void Add(Cell c, int d) =>
      _distances.Add(c, d);

    public int this[Cell c] =>
      _distances[c];
  }
}