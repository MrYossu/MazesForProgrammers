using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mazes.Models.Models {
  public static class Extensions {
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
      if (items == null) {
        return;
      }
      foreach (T item in items) {
        action(item);
      }
    }

    public static T Rand<T>(this IEnumerable<T> items, Random r = null) =>
      items.Skip((r ?? new Random((int)DateTime.Now.Ticks)).Next(1000) % items.Count()).First();

    public static TReturn Switch<TCase, TReturn>(this IEnumerable<(TCase thisCase, Func<TReturn> f)> cases, TCase theCase, TReturn defaultValue = default) {
      foreach ((TCase thisCase, Func<TReturn> f) c in cases) {
        if (EqualityComparer<TCase>.Default.Equals(c.thisCase, theCase)) {
          return c.f();
        }
      }
      return defaultValue;
    }

    public static string SplitCamelCase(this string str) =>
      Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
  }
}