namespace Lab2.Core.Operators.Selection
{
    public sealed class ThresholdSelectionStage
    {
        public ThresholdSelectionStage(ISelectionStrategy selection, decimal threshold)
        {
            Selection = selection;
            Threshold = threshold;
        }

        public ISelectionStrategy Selection { get; }
        public decimal Threshold { get; }
    }
}
