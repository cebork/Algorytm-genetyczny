using Lab2.Core.Domain;
using System.Collections.Generic;

namespace Lab2.Core.Operators.Selection
{
    public interface IGenerationAwareSelectionStrategy : ISelectionStrategy
    {
        void SetGenerationContext(int currentGeneration, int maxGenerations);
    }
}
