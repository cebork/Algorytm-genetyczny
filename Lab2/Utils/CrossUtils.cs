using Lab2.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal static class CrossUtils
    {
        public static void SetCutPoint(List<Osobnik> osobniks)
        {
            int newCutPoint = RandomSingleton.Instance.Next(1, osobniks[0].getL() - 2);
            int iter = 0;
            foreach (var osobnik in osobniks)
            {
                if (osobnik.xBinParents != "-")
                {
                    if (iter == 2)
                    {
                        newCutPoint = RandomSingleton.Instance.Next(1, osobniks[0].getL() - 2);
                        iter = 0;
                    }
                    osobnik.CutPoint = newCutPoint;
                    iter++;
                }
                else
                {
                    osobnik.CutPoint = -1;
                }
            }
        }

        public static void CrossOsobniks(List<Osobnik> osobniks)
        {
            Osobnik fParent = null;
            Osobnik sParent = null;
            foreach (var osobnik in osobniks)
            {
                if (osobnik.xBinParents != "-")
                {
                    if (fParent != null)
                    {
                        sParent = osobnik;
                    } else
                    {
                        fParent = osobnik;
                    } 
                    if (fParent != null && sParent != null)
                    {
                        if (fParent.CutPoint >= 0 && fParent.CutPoint <= fParent.xBinAfterSelection.Length && sParent.CutPoint >= 0 && sParent.CutPoint <= sParent.xBinAfterSelection.Length)
                        {
                            string fParentPart1 = fParent.xBinAfterSelection.Substring(0, fParent.CutPoint);
                            string fParentPart2 = fParent.CutPoint < sParent.xBinAfterSelection.Length ? sParent.xBinAfterSelection.Substring(sParent.CutPoint) : string.Empty;

                            string sParentPart1 = sParent.xBinAfterSelection.Substring(0, sParent.CutPoint);
                            string sParentPart2 = sParent.CutPoint < fParent.xBinAfterSelection.Length ? fParent.xBinAfterSelection.Substring(fParent.CutPoint) : string.Empty;

                            fParent.xBinChild = fParentPart1 + fParentPart2;
                            sParent.xBinChild = sParentPart1 + sParentPart2;
                        }
                        fParent = null;
                        sParent = null;
                    }
                }
                else
                {
                    osobnik.xBinChild = "-";
                }
            }
            if (fParent != null)
            {
                fParent.xBinChild = "-";
            }
            
        }


        public static void CreatePopulationAfterCrossing(List<Osobnik> osobniks)
        {
            foreach (var osobik in osobniks)
            {
                if (osobik.xBinChild != "-")
                {
                    osobik.xBinAfterCross = osobik.xBinChild;
                }
                else
                {
                    osobik.xBinAfterCross = osobik.xBinAfterSelection;
                }
            }
        }
    }
}

