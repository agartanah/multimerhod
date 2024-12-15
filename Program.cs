using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace multimerhod {
  internal class Program {
    static void Main(string[] args) {
      decimal M = 1000;
      decimal a = 37;
      decimal b = 1;
      int N = 100;

      decimal[] x = new decimal[N];
      decimal[] u = new decimal[N];
      decimal uSr = 0m;

      decimal zatravkaX = 38;

      x[0] = (a * zatravkaX + b) % M;
      Console.WriteLine($"{1}. X: " + x[0]);
      u[0] = x[0] / M;
      Console.WriteLine($"{1}. U: " + u[0]);

      uSr += u[0];

      for (int index = 1; index < N; ++index) {
        x[index] = (a * x[index - 1] + b) % M;
        Console.WriteLine($"{index + 1}. X: " + x[index]);

        u[index] = x[index] / M;
        Console.WriteLine($"{index + 1}. U: " + u[index]);

        uSr += u[index];
      }

      uSr /= N;
      // Array.Sort(u);
      decimal[] uSort = new decimal[N];
      for (int index = 0; index < N; ++index) {
        uSort[index] = u[index];
      }

      Array.Sort(uSort);

      decimal h = 0.129641m; //(u.Max() - u.Min()) / (decimal)(1 + 3.3221 * Math.Log10(N));
      Console.WriteLine(Math.Log10(N));
      decimal D = 0;
      Console.WriteLine("h = " + h);
      int countInterval = (int)Math.Round(1 / h, 0);
      Console.WriteLine("количество интервалов = " + (1 / h));
      Console.WriteLine("Umax = " + uSort.Max());
      Console.WriteLine("Umin = " + uSort.Min());

      decimal[,] interval = new decimal[countInterval, 3];

      interval[0, 0] = uSort.Min();
      interval[0, 1] = uSort.Min() + h;
      interval[0, 2] = (interval[0, 1] + interval[0, 0]) / 2;

      for (int index = 1; index < countInterval; ++index) {
        interval[index, 0] = interval[index - 1, 1];
        interval[index, 1] += interval[index - 1, 1] + h;
        interval[index, 2] = (interval[index, 0] + interval[index, 1]) / 2;
      }

      for (int index = 0; index < countInterval; ++index) {
          Console.WriteLine($"{index + 1}. " + Math.Round(interval[index, 0], 3, MidpointRounding.AwayFromZero) + " " + Math.Round(interval[index, 1], 3, MidpointRounding.AwayFromZero));
      }

      decimal[] nFact = new decimal[countInterval];

      int indexInterval = 0;
      Console.WriteLine("Частоты:");
      for (int index = 0; index < N; ++index) {
        if (uSort[index] < interval[indexInterval, 1]) {
          ++nFact[indexInterval];
        } else {
          Console.WriteLine($"{ indexInterval + 1 }. { nFact[indexInterval] }");

          ++indexInterval;

          if (indexInterval >= countInterval) {
            break;
          }

          ++nFact[indexInterval];
        }
      }

      Console.WriteLine($"{indexInterval + 1}. { nFact[indexInterval - 1] }");
      Console.WriteLine("Сумма частот: " + nFact.Sum());

      for (int index = 0; index < N; ++index) {
        D += (decimal)Math.Pow((double)(uSort[index] - uSr), 2);
      }
      D /= N;

      decimal sigma = (decimal)Math.Sqrt((double)D);

      decimal aZvezda = uSr - (decimal)Math.Sqrt(3) * sigma;
      decimal bZvezda = uSr + (decimal)Math.Sqrt(3) * sigma;
      Console.WriteLine("a* = " + aZvezda);
      Console.WriteLine("b* = " + bZvezda);

      decimal fx = 1 / (bZvezda - aZvezda);
      Console.WriteLine("f(x) = " +  fx);

      decimal[] nTer = new decimal[countInterval];
      nTer[0] = N * (interval[0, 2] - aZvezda) / (bZvezda - aZvezda);

      for (int index = 1; index < countInterval - 1; ++index) {
        nTer[index] = N * (interval[index, 2] - interval[index - 1, 2]) / (bZvezda - aZvezda);
      }

      nTer[countInterval - 1] = N * (bZvezda - interval[countInterval - 2, 2]) / (bZvezda - aZvezda);

      int indexCh = 1;
      foreach (decimal element in nTer) {
        Console.WriteLine($"{indexCh++}. {element}");
      }

      decimal hi2Nabl = 0;

      for (int index = 0; index < countInterval; ++index) {
        hi2Nabl += (nFact[index] - nTer[index]) * (nFact[index] - nTer[index]) / nTer[index];
      }

      decimal hi2Kr = 11.1m;

      Console.WriteLine();
      Console.WriteLine("X2 набл = " + hi2Nabl);
      Console.WriteLine("X2 rh = " + hi2Kr + "\n");

      if (hi2Nabl < hi2Kr) {
        Console.WriteLine("Нет оснований отвергнуть гипотезу");
      } else {
        Console.WriteLine("Гипотеза отвергается");
      }

      // Б
      Console.WriteLine("\nБ)\n");

      decimal medN;
      int indexMed;

      if (N % 2 == 0) {
        indexMed = (N + 1) / 2;

        medN = uSort[indexMed];
      } else {
        int index1 = N / 2;
        int index2 = N / 2 + 1;

        medN = (uSort[index1] + u[index2]) * 0.5m;
      }

      Console.WriteLine("Mediana: " + medN);

      byte[] serias = new byte[N];

      for (int index = 0; index < N; ++index) {
        if (u[index] >= medN) {
          serias[index] = 1;
        } else {
          serias[index] = 0;
        }
      }

      foreach (decimal n in serias) {
        Console.Write(n + ", ");
      }

      int countSerias = 0;
      byte prevN = serias[0];

      for (int index = 1; index < N; ++index) {
        if (serias[index] != prevN) {
          ++countSerias;
          prevN = serias[index];
        }
      }

      Console.WriteLine("\nCounte Series: " + countSerias);

      int sLeft = 40;
      int sRight = 61;

      if (sLeft < countSerias && sRight > countSerias) {
        Console.WriteLine("Гипотеза принимается");
      } else {
        Console.WriteLine("Гипотеза отвергается");
      }

      // В
      Console.WriteLine("\nВ)\n");

      decimal sumIX = 0;
      decimal sumXN = 0;
      decimal sumX2 = 0;
      decimal sumX = 0;

      for (int index = 0; index < N; ++index) {
        sumIX += (index + 1) * u[index];
      }

      for (int index = 0; index < N; ++index) {
        sumXN += u[index] * (N + 1) / 2;
      }

      for (int index = 0; index < N; ++index) {
        sumX2 += u[index] * u[index];
      }

      for (int index = 0; index < N; ++index) {
        sumX += u[index];
      }

      sumIX = sumIX / N;
      sumXN = sumXN / N;
      sumX2 = sumX2 / N;
      sumX = sumX / N;

      decimal rXI = (sumIX - sumXN) / (decimal)Math.Sqrt((double)((sumX2 - sumX * sumX) * (N * N - 1m) / 12m));
      Console.WriteLine("r(x, i) = " + rXI);

      decimal rMax = 1.96m * (1 - rXI * rXI) / (decimal)Math.Sqrt(N);
      Console.WriteLine("rMax = " + rMax);

      if (Math.Abs(rXI) > rMax) {
        Console.WriteLine("Корреляционная связь присутствует");
      } else {
        Console.WriteLine("Гипотеза о независимости принимается");
      }
    }
  }
}
