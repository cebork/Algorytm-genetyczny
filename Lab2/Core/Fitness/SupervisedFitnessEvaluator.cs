namespace Lab2.Core.Fitness
{
    public class SupervisedFitnessEvaluator : IFitnessEvaluator
    {
        private readonly bool[,] _referenceMatrix;
        private readonly int _precisionDigits;

        public SupervisedFitnessEvaluator(bool[,] referenceMatrix, int precisionDigits)
        {
            _referenceMatrix = referenceMatrix;
            _precisionDigits = precisionDigits;
        }

        public decimal Evaluate(bool[,] genotype)
        {
            int size = genotype.GetLength(0);
            int meter = 0;
            int denominator = 0;

            for (int i = 1; i < size - 1; i++)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    bool g = genotype[i, j];
                    bool r = _referenceMatrix[i - 1, j - 1];

                    if (g == r)
                    {
                        meter++;
                        denominator++;
                    }
                    else
                    {
                        denominator++;
                    }
                }
            }

            if (denominator == 0)
                return 0;

            return Math.Round((decimal)meter / denominator, _precisionDigits);
        }
    }
}
