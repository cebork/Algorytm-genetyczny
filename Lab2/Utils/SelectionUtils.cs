using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal static class SelectionUtils
    {
        public static void SetUpFitValue(List<Osobnik> osobniks)
        {
            double minValue = osobniks.Min(o => o.Mark);
            foreach (var osobik in osobniks)
            {
                osobik.SetFitValue(minValue);
            }
        }

        internal static void SetUpDistribuator(List<Osobnik> osobniks)
        {
            double sumValue = osobniks.Sum(o => o.FitValue);
            double accumulator = 0;
            for (int i = 0; i < osobniks.Count; i++)
            {
                osobniks[i].SetProbability(sumValue);
                accumulator += osobniks[i].Probability;
                if (i == 0)
                {
                    osobniks[i].Distribuator = osobniks[i].Probability;
                }
                else
                {
                    osobniks[i].Distribuator = accumulator;
                }

            }
        }

        internal static void SetUpNewOsobnikAfterSelection(List<Osobnik> osobniks)
        {
            for (int i = 0; i < osobniks.Count; i++)
            {
                double randomValue = RandomSingleton.Instance.NextDouble();
                osobniks[i].RandomValueToCheck = randomValue;

                if (osobniks[i].RandomValueToCheck <= osobniks[0].Distribuator)
                {
                    osobniks[i].MatrixAfterSelection = osobniks[0].Matrix;
                }
                else
                {
                    for (int j = 1; j < osobniks.Count; j++)
                    {
                        if (osobniks[j - 1].Distribuator < osobniks[i].RandomValueToCheck && osobniks[j].Distribuator >= osobniks[i].RandomValueToCheck)
                        {
                            osobniks[i].MatrixAfterSelection = osobniks[j].Matrix;
                        }
                    }
                }
            }
        }
    }
}
