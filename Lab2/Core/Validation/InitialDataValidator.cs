using Lab2.Core.Domain;
using Lab2.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Core.Validation
{
    public sealed class InitialDataValidator : IValidator<InitialData>
    {
        public ValidationResult Validate(InitialData data)
        {
            var result = new ValidationResult();

            if (data.MatrixSize <= 0)
                result.Errors.Add("Rozmiar macierzy musi być > 0");

            if (data.NumberOfIndividuals <= 0)
                result.Errors.Add("Liczba osobników musi być > 0");

            if (data.NumberOfIterations <= 0)
                result.Errors.Add("Liczba iteracji musi być > 0");

            if (data.CrossProbability is < 0 or > 1)
                result.Errors.Add("Prawdopodobieństwo krzyżowania musi być w [0,1]");

            if (data.MutationProbability is < 0 or > 1)
                result.Errors.Add("Prawdopodobieństwo mutacji musi być w [0,1]");

            if (data.UniformBlockMutationProbWhite is < 0 or > 1)
                result.Errors.Add("Prawdopodobieństwo rozbicia białych bloków musi być w [0,1]");

            if (data.UniformBlockMutationProbRed is < 0 or > 1)
                result.Errors.Add("Prawdopodobieństwo rozbicia czerwonych bloków musi być w [0,1]");

            if (data.NarrowingMutationMultiplier < 0)
                result.Errors.Add("Mnożnik mutacji zwężającej musi być >= 0");

            if (data.NarrowingMutationStep <= 0)
                result.Errors.Add("Krok zejścia mutacji zwężającej musi być > 0");

            if (data.ProbGen1 is < 0 or > 1)
                result.Errors.Add("ProbGen1 musi być w [0,1]");

            if (data.AlgorithmType == AlgorithmType.SUPERVISED &&
                (data.SupervisedReferenceMatrix == null || data.SupervisedReferenceMatrix.Length == 0))
            {
                result.Errors.Add("Brak macierzy referencyjnej");
            }

            if (data.AlgorithmType == AlgorithmType.UNSUPERVISED &&
                (data.UnsupervisedPatternMatrixes == null || data.UnsupervisedPatternMatrixes.Length == 0))
            {
                result.Errors.Add("Nie wybrano macierzy wzorców");
            }
            else if (data.AlgorithmType == AlgorithmType.UNSUPERVISED)
            {
                int matrixSizeWithDeadZone = (int)data.MatrixSize + 2;

                foreach (var pattern in data.UnsupervisedPatternMatrixes)
                {
                    if (pattern == null)
                    {
                        result.Errors.Add("Wzorzec nie może być pusty");
                        continue;
                    }

                    int rows = pattern.GetLength(0);
                    int cols = pattern.GetLength(1);

                    if (rows <= 0 || cols <= 0)
                        result.Errors.Add("Wzorzec musi mieć rozmiar > 0");

                    if (rows != cols)
                        result.Errors.Add("Wzorzec musi być macierzą kwadratową");

                    if (rows > matrixSizeWithDeadZone || cols > matrixSizeWithDeadZone)
                        result.Errors.Add("Wzorzec nie może być większy niż macierz algorytmu z martwą strefą");
                }
            }

            return result;
        }
    }
}
