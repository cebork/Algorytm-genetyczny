namespace Lab2.Core.Domain
{
    public class Individual
    {
        public bool[,] Genotype { get; }

        public decimal Fitness { get; set; }

        public Individual(bool[,] genotype)
        {
            Genotype = genotype ?? throw new ArgumentNullException(nameof(genotype));
        }

        public Individual Clone()
        {
            return new Individual((bool[,])Genotype.Clone())
            {
                Fitness = this.Fitness
            };
        }
    }
}
