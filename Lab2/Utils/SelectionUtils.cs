//using Lab2.objects;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Lab2.Core.Domain;

//namespace Lab2.Utils
//{
//    internal static class SelectionUtils
//    {
//        public static void SetUpFitValue(List<Individual> individuals)
//        {
//            decimal minValue = individuals.Min(o => o.Mark);
//            for (int i = 0; i < individuals.Count; i++)
//            {
//                individuals[i].SetFitValue(minValue);
//            }

//        }

//        internal static void SetUpDistribuator(List<Individual> individuals)
//        {
//            decimal sumValue = individuals.Sum(o => o.FitValue);
//            decimal accumulator = 0;
//            for (int i = 0; i < individuals.Count; i++)
//            {
//                individuals[i].SetProbability(sumValue);
//                accumulator += individuals[i].Probability;
//                if (i == 0)
//                {
//                    individuals[i].Distribuator = individuals[i].Probability;
//                }
//                else
//                {
//                    individuals[i].Distribuator = accumulator;
//                }

//            }
//        }

//        internal static void SetUpNewOsobnikAfterSelection(List<Individual> individuals)
//        {
//            var distribList = individuals.Select(i => i.Distribuator).ToList();

//            for (int i = 0; i < individuals.Count; i++)
//            {
//                decimal randomValue = (decimal)RandomSingleton.Instance.NextDouble();
//                individuals[i].RandomValueToCheck = randomValue;

//                int selectedIndex = BinarySearchDistrib(distribList, randomValue);
//                individuals[i].MatrixAfterSelection = individuals[selectedIndex].IndividualMatrix;
//            }
//        }


//        internal static void SetUpNewOsobnikAfterSelectionTournamentHard(List<Individual> individuals, decimal tournamentSize)
//        {
//            var random = RandomSingleton.Instance;


//            for (int i = 0; i < individuals.Count; i++)
//            {
//                decimal randomValue = (decimal)random.NextDouble();
//                individuals[i].RandomValueToCheck = randomValue;

//                var competitors = new List<Individual>();
//                while (competitors.Count < tournamentSize)
//                {
//                    int randomIndex = random.Next(individuals.Count);
//                    if (randomIndex != i)
//                    {
//                        competitors.Add(individuals[randomIndex]);
//                    }
//                }

//                var winner = competitors.OrderByDescending(ind => ind.Mark).First();

//                individuals[i].MatrixAfterSelection = winner.IndividualMatrix;
//            }
//        }

//        internal static void SetUpNewOsobnikAfterSelectionTournamentSoft(List<Individual> individuals, decimal tournamentSize, decimal tournamentThreshold)
//        {
//            var random = RandomSingleton.Instance;

//            for (int i = 0; i < individuals.Count; i++)
//            {
//                var competitors = new List<Individual>();
//                while (competitors.Count < tournamentSize)
//                {
//                    int randomIndex = random.Next(individuals.Count);
//                    if (randomIndex != i && !competitors.Contains(individuals[randomIndex]))
//                    {
//                        competitors.Add(individuals[randomIndex]);
//                    }
//                }

//                var remaining = competitors.OrderByDescending(ind => ind.Mark).ToList();
//                Individual selected = null;

//                while (remaining.Count > 1)
//                {
//                    var best = remaining[0];
//                    decimal randomValue = (decimal)random.NextDouble();

//                    if (randomValue >= tournamentThreshold)
//                    {
//                        selected = best;
//                        break;
//                    }
//                    else
//                    {
//                        remaining.RemoveAt(0);
//                    }
//                }

//                if (selected == null)
//                {
//                    selected = remaining[0];
//                }

//                individuals[i].RandomValueToCheck = -1;
//                individuals[i].MatrixAfterSelection = selected.IndividualMatrix;
//            }
//        }






//        private static int BinarySearchDistrib(List<decimal> distribList, decimal target)
//        {
//            int left = 0;
//            int right = distribList.Count - 1;

//            while (left < right)
//            {
//                int mid = (left + right) / 2;
//                if (target <= distribList[mid])
//                {
//                    right = mid;
//                }
//                else
//                {
//                    left = mid + 1;
//                }
//            }

//            return left;
//        }



//        //internal static void SetUpNewOsobnikAfterSelection(List<Individual> individuals)
//        //{
//        //    for (int i = 0; i < individuals.Count; i++)
//        //    {
//        //        decimal randomValue = (decimal)RandomSingleton.Instance.NextDouble();
//        //        individuals[i].RandomValueToCheck = randomValue;

//        //        if (individuals[i].RandomValueToCheck <= individuals[0].Distribuator)
//        //        {
//        //            individuals[i].MatrixAfterSelection = individuals[0].IndividualMatrix;
//        //        }
//        //        else
//        //        {
//        //            for (int j = 1; j < individuals.Count; j++)
//        //            {
//        //                if (individuals[j - 1].Distribuator < individuals[i].RandomValueToCheck && individuals[j].Distribuator >= individuals[i].RandomValueToCheck)
//        //                {
//        //                    individuals[i].MatrixAfterSelection = individuals[j].IndividualMatrix;
//        //                }
//        //            }
//        //        }
//        //    }
//        //}
//    }
//}
