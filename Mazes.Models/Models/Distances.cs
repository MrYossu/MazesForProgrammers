using System.Collections.Generic;
using System.Linq;

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

    public IEnumerable<CellDistance> Cells =>
      _distances.Keys.Select(k => new CellDistance(k, _distances[k]));

    public void Add(Cell c, int d) =>
      _distances.Add(c, d);

    public int this[Cell c] =>
      _distances[c];
  }
}