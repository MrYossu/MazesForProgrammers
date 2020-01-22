using System;
using System.Collections.Generic;
using System.Linq;

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

    public static T Rand<T>(this IEnumerable<T> items) =>
      items.Skip(new Random((int)DateTime.Now.Ticks).Next(1000) % items.Count()).First();

    public static TReturn Switch<TCase, TReturn>(this IEnumerable<(TCase thisCase, Func<TReturn> f)> cases, TCase theCase, TReturn defaultValue = default) {
      foreach ((TCase thisCase, Func<TReturn> f) c in cases) {
        if (EqualityComparer<TCase>.Default.Equals(c.thisCase, theCase)) {
          return c.f();
        }
      }
      return defaultValue;
    }
  }
}