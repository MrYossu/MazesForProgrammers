using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazes.Models {
  public class Grid {
    public int Rows { get; }
    public int Cols { get; }
    private Cell[,] _cells;
    private readonly Random _r = new Random((int)DateTime.Now.Ticks);
    private readonly string _nl = Environment.NewLine;

    public Grid(int rows, int cols) {
      Rows = rows;
      Cols = cols;
      _cells = BuildCells();
    }

    #region Indexer

    public Cell this[int row, int col] {
      get {
        if (row < 0 || row >= Rows || col < 0 || col >= Cols) {
          return null;
        }
        return _cells[row, col];
      }
    }

    #endregion

    public IEnumerable<Cell> Cells {
      get {
        for (int row = 0; row < Rows; row++) {
          for (int col = 0; col < Cols; col++) {
            yield return _cells[row, col];
          }
        }
      }
    }


    public Cell RandomCell() =>
      this[_r.Next(Rows), _r.Next(Cols)];

    public int Size =>
      Rows * Cols;

    #region Initialise

    private Cell[,] BuildCells() {
      _cells = new Cell[Rows, Cols];
      for (int col = 0; col < Cols; col++) {
        for (int row = 0; row < Rows; row++) {
          _cells[row, col] = new Cell(row, col);
        }
      }
      ConfigureCells();
      return _cells;
    }

    private void ConfigureCells() {
      for (int col = 0; col < Cols; col++) {
        for (int row = 0; row < Rows; row++) {
          _cells[row, col].North = this[row - 1, col];
          _cells[row, col].South = this[row + 1, col];
          _cells[row, col].East = this[row, col + 1];
          _cells[row, col].West = this[row, col - 1];
        }
      }
    }

    #endregion

    public override string ToString() {
      string output = "+" + string.Concat(Enumerable.Repeat("---+", Cols)) + _nl;
      for (int row = 0; row < Rows; row++) {
        string top = "|"; // The cell row itself
        string bottom = "+"; // The divider between this row and the next
        for (int col = 0; col < Cols; col++) {
          Cell cell = this[row, col];
          top += "   " + (cell?.Linked(cell.East) ?? false ? " " : "|");
          bottom += (cell?.Linked(cell.South) ?? false ? "   " : "---") + "+";
        }
        output += top + _nl + bottom + _nl;
      }
      return output;
    }
  }
}