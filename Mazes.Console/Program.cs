using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mazes.Models.MazeMakers;
using Mazes.Models.Models;

namespace Mazes.Console {
  class Program {
    static void Main(string[] args) {
      //DumpGrid(grid);
      //DumpRandomCells(grid);
      //grid.AllCells.ForEach(c => Debug.WriteLine(c));
      //Debug.WriteLine(Wilson.Create(15, 15));
      //TimeAbwMazeCreation();
      //GenerationStatisticsAldousBroder();
      //GenerationStatisticsWilson();
      //Debug.WriteLine(AldousBroderWilson.Create(10,10));
    }

    private static void GenerationStatisticsWilson() {
      Maze maze = Wilson.Create(50, 50);
    }

    private static void GenerationStatisticsAldousBroder() {
      int trials = 10;
      int[] iterations = new int[10];
      for (int iter = 0; iter < 10; iter++) {
        Maze maze = AldousBroder.Create(50, 50);
        for (int i = 0; i < trials; i++) {
          //Debug.WriteLine($"{AldousBroder.Iterations[i]}");
          iterations[i] += AldousBroder.Iterations[i];
        }
      }
      for (int i = 0; i < trials; i++) {
        Debug.WriteLine($"{(i + 1) * 10}, {iterations[i] / trials}");
      }
    }

    /*
     10 trials of 50x50 cells gave (ms):
     BinaryTree         5
     Sidewinder         1
     AldousBroder       51
     Wilson             427
     AldousBroderWilson 396
     */
    private static void TimeMazeCreation() {
      int cells = 50;
      int nTests = 10;
      new List<Func<int, int, Maze>> { AldousBroder.Create, Wilson.Create, AldousBroderWilson.Create }
        .ForEach(a => {
          long totalTime = 0;
          for (int i = 0; i < nTests; i++) {
            Stopwatch sw = Stopwatch.StartNew();
            a(cells, cells);
            totalTime += sw.ElapsedMilliseconds;
          }
          Debug.WriteLine(totalTime / nTests);
        });
    }

    //private static void TimeAbwMazeCreation() {
    //  int cells = 50;
    //  int nTests = 10;
    //  long totalTime = 0;
    //  for (int percentage = 0; percentage <= 100; percentage += 10) {
    //    AldousBroderWilson.AbMaxPercentage = percentage;
    //    Enumerable.Range(0, nTests).ForEach(n => {
    //      Stopwatch sw = Stopwatch.StartNew();
    //      AldousBroderWilson.Create(cells, cells);
    //      totalTime += sw.ElapsedMilliseconds;
    //    });
    //    Debug.WriteLine($"{percentage}\t{totalTime / nTests}");
    //  }
    //}

    private static void DumpRandomCells(Maze maze) {
      for (int i = 0; i < 10; i++) {
        Debug.WriteLine(maze.RandomCell());
      }
    }

    private static void DumpGrid(Maze maze) {
      for (int row = 0; row < maze.Rows; row++) {
        for (int col = 0; col < maze.Cols; col++) {
          Debug.Write(maze[row, col] + " ");
        }
        Debug.WriteLine("");
      }
      int r = 1;
      int c = 1;
      Debug.WriteLine($"({r}, {c}).North: {maze[r, c].North}");
      Debug.WriteLine($"({r}, {c}).South: {maze[r, c].South}");
      Debug.WriteLine($"({r}, {c}).East:  {maze[r, c].East}");
      Debug.WriteLine($"({r}, {c}).West:  {maze[r, c].West}");
    }
  }
}