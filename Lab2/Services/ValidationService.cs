using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Core.Domain;
using Lab2.UI.Domain;
namespace Lab2.Services
{
    public class ValidationService
    {
        public InitialData InitialData { get; set; }

        public PatternChoosingDisplayColumns patternChoosingDisplayColumns { get; set; }
        public ValidationService(InitialData initialData) 
        {
            InitialData = initialData;
            validate();
        }

        public ValidationService(PatternChoosingDisplayColumns patternChoosingDisplay) 
        {
            this.patternChoosingDisplayColumns = patternChoosingDisplay;
            validatePatterCreation();
        }

        public void validate()
        {
            if (
                InitialData.Precision == null ||
                InitialData.MatrixSize == null || 
                InitialData.NumberOfIndividuals == null || 
                InitialData.CrossProbability == null ||
                InitialData.MutationProbability == null ||
                InitialData.NumberOfIterations == null
            ) {
                throw new Exception("Nie wszystkie pola zostały uzupełnione");
            }

            if (InitialData.MatrixSize <= 0)
            {
                throw new Exception("Macierze nie mogą mieć ujemnych wymiarów");
            }

            if (InitialData.CrossProbability > 1 || InitialData.CrossProbability < 0)
            {
                throw new Exception("Niepoprawny przedział dla prawdopodbieństwa krzyżowania");
            }

            if (InitialData.MutationProbability > 1 || InitialData.MutationProbability < 0)
            {
                throw new Exception("Niepoprawny przedział dla prawdopodbieństwa mutacji");
            }

            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED && (InitialData.SupervisedReferenceMatrix == null || InitialData.SupervisedReferenceMatrix.Length == 0))
            {
                throw new Exception("Macierz referencyjna jest niepoprawna");
            }
            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED && (InitialData.UnsupervisedPatternMatrixes == null || InitialData.UnsupervisedPatternMatrixes.Length == 0))
            {
                throw new Exception("Macierze wzorców nie zostały wybrane");
            }
        }

        public void validatePatterCreation()
        {
            if (
                patternChoosingDisplayColumns == null ||
                patternChoosingDisplayColumns.PatternMatrix == null ||
                patternChoosingDisplayColumns.PatternSize == null ||
                patternChoosingDisplayColumns.PatternName == null
            )
            {
                throw new Exception("Nie wszystkie pola zostały uzupełnione");
            }

            if (
                patternChoosingDisplayColumns.PatternName.Length < 5
            )
            {
                throw new Exception("Minimalna długość nazwy wzorca to 5");
            }
        }
    }
}
