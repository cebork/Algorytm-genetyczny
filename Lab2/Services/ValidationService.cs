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

        public string ReferenceMatrixName { get; set; }

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

        public ValidationService(string referenceMatrixName)
        {
            this.ReferenceMatrixName = referenceMatrixName;
            validateReferenceMatrixCreation();
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

            if (InitialData.RequestedMatrixSize <= 0)
            {
                throw new Exception("Żądany rozmiar macierzy musi być > 0");
            }

            if (InitialData.UseEvenMatrixPadding)
            {
                int requestedSize = (int)InitialData.RequestedMatrixSize;

                if (InitialData.EmptyRowIndex < 1 || InitialData.EmptyRowIndex > requestedSize)
                {
                    throw new Exception("Pozycja pustego wiersza musi mieścić się w rozmiarze macierzy");
                }

                if (InitialData.EmptyColumnIndex < 1 || InitialData.EmptyColumnIndex > requestedSize)
                {
                    throw new Exception("Pozycja pustej kolumny musi mieścić się w rozmiarze macierzy");
                }

                if (InitialData.MatrixSize != InitialData.RequestedMatrixSize - 1)
                {
                    throw new Exception("Efektywny rozmiar macierzy parzystej musi być o 1 mniejszy od rozmiaru żądanego");
                }
            }

            if (InitialData.CrossProbability > 1 || InitialData.CrossProbability < 0)
            {
                throw new Exception("Niepoprawny przedział dla prawdopodbieństwa krzyżowania");
            }

            if (InitialData.MutationProbability > 1 || InitialData.MutationProbability < 0)
            {
                throw new Exception("Niepoprawny przedział dla prawdopodbieństwa mutacji");
            }

            if (InitialData.UniformBlockMutationProbWhite > 1 || InitialData.UniformBlockMutationProbWhite < 0)
            {
                throw new Exception("Niepoprawny przedział dla prawdopodobieństwa rozbicia białych bloków");
            }

            if (InitialData.UniformBlockMutationProbRed > 1 || InitialData.UniformBlockMutationProbRed < 0)
            {
                throw new Exception("Niepoprawny przedział dla prawdopodobieństwa rozbicia czerwonych bloków");
            }

            if (InitialData.NarrowingMutationMultiplier < 0)
            {
                throw new Exception("Mnożnik mutacji zwężającej musi być >= 0");
            }

            if (InitialData.NarrowingMutationStep <= 0)
            {
                throw new Exception("Krok zejścia mutacji zwężającej musi być > 0");
            }

            if (InitialData.NarrowingMutationExponent < 0)
            {
                throw new Exception("Wykładnik asymptotyczny mutacji zwężającej musi być >= 0");
            }

            if (InitialData.StagnationWindow <= 0)
            {
                throw new Exception("Okno stagnacji musi być > 0");
            }

            if (InitialData.StagnationResetFraction <= 0 || InitialData.StagnationResetFraction > 0.5m)
            {
                throw new Exception("Odsetek wymiany przy stagnacji musi być w zakresie (0,0.5]");
            }

            if (InitialData.StagnationDiversityThreshold < 0 || InitialData.StagnationDiversityThreshold > 1)
            {
                throw new Exception("Próg różnorodności stagnacji musi być w [0,1]");
            }

            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED && (InitialData.SupervisedReferenceMatrix == null || InitialData.SupervisedReferenceMatrix.Length == 0))
            {
                throw new Exception("Macierz referencyjna jest niepoprawna");
            }
            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.SUPERVISED)
            {
                int expectedSize = InitialData.UseEvenMatrixPadding
                    ? (int)InitialData.RequestedMatrixSize
                    : (int)InitialData.MatrixSize;

                if (InitialData.SupervisedReferenceMatrix.GetLength(0) != expectedSize ||
                    InitialData.SupervisedReferenceMatrix.GetLength(1) != expectedSize)
                {
                    throw new Exception("Macierz referencyjna ma niepoprawny rozmiar");
                }
            }
            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED && (InitialData.UnsupervisedPatternMatrixes == null || InitialData.UnsupervisedPatternMatrixes.Length == 0))
            {
                throw new Exception("Macierze wzorców nie zostały wybrane");
            }
            if (InitialData.AlgorithmType == Core.Enums.AlgorithmType.UNSUPERVISED)
            {
                decimal matrixSizeWithDeadZone = InitialData.MatrixSize + 2;

                foreach (var pattern in InitialData.UnsupervisedPatternMatrixes)
                {
                    if (pattern == null)
                    {
                        throw new Exception("Wzorzec nie może być pusty");
                    }

                    int rows = pattern.GetLength(0);
                    int cols = pattern.GetLength(1);

                    if (rows <= 0 || cols <= 0)
                    {
                        throw new Exception("Wzorzec musi mieć rozmiar > 0");
                    }

                    if (rows != cols)
                    {
                        throw new Exception("Wzorzec musi być macierzą kwadratową");
                    }

                    if (rows > matrixSizeWithDeadZone || cols > matrixSizeWithDeadZone)
                    {
                        throw new Exception("Wzorzec nie może być większy niż macierz algorytmu z martwą strefą");
                    }
                }
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

            int rows = patternChoosingDisplayColumns.PatternMatrix.GetLength(0);
            int cols = patternChoosingDisplayColumns.PatternMatrix.GetLength(1);

            if (rows <= 0 || cols <= 0)
            {
                throw new Exception("Wzorzec musi mieć rozmiar > 0");
            }

            if (rows != cols)
            {
                throw new Exception("Wzorzec musi być macierzą kwadratową");
            }
        }

        public void validateReferenceMatrixCreation()
        {
            if (
                ReferenceMatrixName == null 
            )
            {
                throw new Exception("Nie wszystkie pola zostały uzupełnione");
            }

            if (
                ReferenceMatrixName.Length < 5
            )
            {
                throw new Exception("Minimalna długość nazwy wzorca to 5");
            }
        }

    }
}
