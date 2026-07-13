using System;

namespace Lab2.objects
{
    public class DetailedRunObject
    {
        public int ExperimentIndex { get; set; }
        public int Seed { get; set; }
        public decimal N { get; set; }
        public decimal pk { get; set; }
        public decimal pm { get; set; }
        public decimal T { get; set; }
        public decimal Rt { get; set; }
        public decimal Ps { get; set; }
        public decimal Ipk { get; set; }
        public decimal MinMark { get; set; }
        public decimal AvgMark { get; set; }
        public decimal BestMark { get; set; }
        public int BestGeneration { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}

