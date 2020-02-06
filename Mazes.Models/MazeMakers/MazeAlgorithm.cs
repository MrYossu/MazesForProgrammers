using System;
using System.Collections.Generic;
using Mazes.Models.Models;

namespace Mazes.Models.MazeMakers {
  public class MazeAlgorithm {
    public string Name { get; }
    public Func<int, int, Maze> Create { get; }

    public MazeAlgorithm(string name, Func<int, int, Maze> create) {
      Name = name;
      Create = create;
    }

    public static List<MazeAlgorithm> Algorithms =>
      new List<MazeAlgorithm> {
        new MazeAlgorithm("Binary Tree", BinaryTree.Create),
        new MazeAlgorithm("Sidewinder", Sidewinder.Create),
        new MazeAlgorithm("Aldous-Broder", AldousBroder.Create),
        new MazeAlgorithm("Aldous-Broder - No links", AldousBroderNoLinks.Create),
        new MazeAlgorithm("Wilson", Wilson.Create),
        new MazeAlgorithm("Aldous-Broder/Wilson", AldousBroderWilson.Create)
      };
  }
}