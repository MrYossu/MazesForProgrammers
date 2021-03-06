﻿using System;
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
      //Debug.WriteLine(WilsonJb.Create(5, 5));
      //TimeOneAlgorithm();
      //Debug.WriteLine(AldousBroderWilson.Create(10,10));
      //TimeAbwMazeCreation();
    }

    private static void TimeOneAlgorithm() {
      long ms = 0;
      Enumerable.Range(0, 10).ForEach(n => {
        Stopwatch sw = Stopwatch.StartNew();
        Maze maze = Wilson.Create(100, 100);
        sw.Stop();
        Debug.WriteLine(sw.ElapsedMilliseconds);
        ms += sw.ElapsedMilliseconds;
      });
      Debug.WriteLine("---");
      Debug.WriteLine(ms / 10);
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

    private static void TimeAbwMazeCreation() {
      int cells = 50;
      int nTests = 10;
      long totalTime = 0;
      Enumerable.Range(0, nTests).ForEach(n => {
        Stopwatch sw = Stopwatch.StartNew();
        AldousBroderWilson.Create(cells, cells);
        totalTime += sw.ElapsedMilliseconds;
      });
      Debug.WriteLine(totalTime / nTests);
    }

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