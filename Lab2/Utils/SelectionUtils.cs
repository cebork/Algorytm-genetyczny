using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Domain;

namespace Lab2.Utils
{
    internal static class SelectionUtils
    {
        public static void SetUpFitValue(List<Individual> individuals)
        {
            double minValue = individuals.Min(o => o.Mark);
            Parallel.ForEach(individuals, individual => {
                individual.SetFitValue(minValue);
            });

        }

        internal static void SetUpDistribuator(List<Individual> individuals)
        {
            double sumValue = individuals.Sum(o => o.FitValue);
            double accumulator = 0;
            for (int i = 0; i < individuals.Count; i++)
            {
                individuals[i].SetProbability(sumValue);
                accumulator += individuals[i].Probability;
                if (i == 0)
                {
                    individuals[i].Distribuator = individuals[i].Probability;
                }
                else
                {
                    individuals[i].Distribuator = accumulator;
                }

            }
        }

        internal static void SetUpNewOsobnikAfterSelection(List<Individual> individuals)
        {
            var distribList = individuals.Select(i => i.Distribuator).ToList();

            Parallel.For(0, individuals.Count, i =>
            {
                double randomValue = (double)RandomSingleton.Instance.NextDouble();
                individuals[i].RandomValueToCheck = randomValue;

                int selectedIndex = BinarySearchDistrib(distribList, randomValue);
                individuals[i].MatrixAfterSelection = individuals[selectedIndex].IndividualMatrix;
            });
        }

        private static int BinarySearchDistrib(List<double> distribList, double target)
        {
            int left = 0;
            int right = distribList.Count - 1;

            while (left < right)
            {
                int mid = (left + right) / 2;
                if (target <= distribList[mid])
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return left;
        }



        //internal static void SetUpNewOsobnikAfterSelection(List<Individual> individuals)
        //{
        //    for (int i = 0; i < individuals.Count; i++)
        //    {
        //        decimal randomValue = (decimal)RandomSingleton.Instance.NextDouble();
        //        individuals[i].RandomValueToCheck = randomValue;

        //        if (individuals[i].RandomValueToCheck <= individuals[0].Distribuator)
        //        {
        //            individuals[i].MatrixAfterSelection = individuals[0].IndividualMatrix;
        //        }
        //        else
        //        {
        //            for (int j = 1; j < individuals.Count; j++)
        //            {
        //                if (individuals[j - 1].Distribuator < individuals[i].RandomValueToCheck && individuals[j].Distribuator >= individuals[i].RandomValueToCheck)
        //                {
        //                    individuals[i].MatrixAfterSelection = individuals[j].IndividualMatrix;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
