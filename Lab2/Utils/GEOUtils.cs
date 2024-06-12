using Lab2.GEO.domain;
using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal static class GEOUtils
    {
        

        public static FinalOsobnik GenerateInitialOsobnik(InitialData initialData)
        {
            string initialChromosome = GeneralUtils.GenerateBinaryString(initialData.ChromosomeLength);
            double xReal = GeneralUtils.IntegerToReal(
                initialData.a, initialData.b, GeneralUtils.BinaryToInteger(initialChromosome), initialData.ChromosomeLength, initialData.PrecisionAsInteger
            );
            return new FinalOsobnik()
            {
                XBin = GeneralUtils.GenerateBinaryString(initialData.ChromosomeLength),
                XReal = xReal,
                Mark = GeneralUtils.SetMark(xReal, initialData.PrecisionAsInteger)
            };
        }

        public static List<GEOSubject> GenerateChromosomeVariants(InitialData initialData, FinalOsobnik osobnik)
        {

            List<GEOSubject> opeartiveGeoVariants = new List<GEOSubject>();


            for (int i = 0; i < initialData.ChromosomeLength; i++)
            {
                char[] chromosomeArray = osobnik.XBin.ToCharArray();
                if (chromosomeArray[i] == '1')
                {
                    chromosomeArray[i] = '0';
                }
                else
                {
                    chromosomeArray[i] = '1';
                }
                string modifiedChromosome = new string(chromosomeArray);

                double xReal = GeneralUtils.IntegerToReal(
                    initialData.a, initialData.b, GeneralUtils.BinaryToInteger(modifiedChromosome), initialData.ChromosomeLength, initialData.PrecisionAsInteger
                );

                GEOSubject modifiedGeoSubject = new GEOSubject()
                {
                    FinalOsobnik = new FinalOsobnik()
                    {
                        XBin = modifiedChromosome,
                        XReal = xReal,
                        Mark = GeneralUtils.SetMark(xReal, initialData.PrecisionAsInteger)
                    }
                };
                opeartiveGeoVariants.Add(modifiedGeoSubject);
            }

            SetupRanking(opeartiveGeoVariants);
            SetupPropability(opeartiveGeoVariants, initialData);
            return opeartiveGeoVariants;
        }

        public static void SetupRanking(List<GEOSubject> gEOSubjects)
        {
            var sortedSubjects = gEOSubjects
             .OrderByDescending(x => x.FinalOsobnik.Mark)
             .ToList();


            for (int i = 0; i < sortedSubjects.Count; i++)
            {
                sortedSubjects[i].RankingPosition = i+1;
            }
        }

        public static void SetupPropability(List<GEOSubject> gEOSubjects, InitialData initialData)
        {
            foreach (GEOSubject subject in gEOSubjects)
            {
                subject.Propabilty = 1 / Math.Pow(subject.RankingPosition, initialData.Teta);
            }
        }

        public static FinalOsobnik Mutate(InitialData initialData, FinalOsobnik finalOsobnik)
        {
            List<GEOSubject> gEOSubjects = GenerateChromosomeVariants(initialData, finalOsobnik);

            //Nie wiem czy przy każdym bicie musi być losowana nowa liczba
            double randomIndicator = RandomSingleton.Instance.NextDouble();

            char[] chromosomeArray = finalOsobnik.XBin.ToCharArray();


            for (int i = 0; i < gEOSubjects.Count; i++)
            {
                if(randomIndicator <= gEOSubjects[i].Propabilty)
                {
                    if (chromosomeArray[i] == '1')
                    {
                        chromosomeArray[i] = '0';
                    }
                    else
                    {
                        chromosomeArray[i] = '1';
                    }
                }
            }

            string modifiedChromosome = new string(chromosomeArray);

            double xReal = GeneralUtils.IntegerToReal(
                initialData.a, initialData.b, GeneralUtils.BinaryToInteger(modifiedChromosome), initialData.ChromosomeLength, initialData.PrecisionAsInteger
            );
            return new FinalOsobnik()
            {
                XBin = GeneralUtils.GenerateBinaryString(initialData.ChromosomeLength),
                XReal = xReal,
                Mark = GeneralUtils.SetMark(xReal, initialData.PrecisionAsInteger)
            };
        }
    }
}
