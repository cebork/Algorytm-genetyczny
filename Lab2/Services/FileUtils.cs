using Lab2.UI.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Lab2.Services
{
    public static class FileUtils
    {
        private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string FilePath = Path.Combine(DataDirectory, "patterns.json");

        // DTO used for serialization only
        private class SerializablePattern
        {
            public string PatternName { get; set; }
            public int PatternSize { get; set; }
            public bool[][] PatternMatrix { get; set; }
        }

        /// <summary>
        /// Appends a new pattern to the JSON file in the Data folder.
        /// </summary>
        public static void AppendPatternToFile(PatternChoosingDisplayColumns pattern)
        {
            Directory.CreateDirectory(DataDirectory);

            var existing = new List<SerializablePattern>();

            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                existing = JsonSerializer.Deserialize<List<SerializablePattern>>(json) ?? new List<SerializablePattern>();
            }

            var serializable = new SerializablePattern
            {
                PatternName = pattern.PatternName,
                PatternSize = pattern.PatternSize,
                PatternMatrix = ToJaggedArray(pattern.PatternMatrix)
            };

            existing.Add(serializable);

            var newJson = JsonSerializer.Serialize(existing, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, newJson);
        }

        /// <summary>
        /// Loads all patterns from file and converts them to domain objects.
        /// </summary>
        public static List<PatternChoosingDisplayColumns> LoadAllPatterns()
        {
            if (!File.Exists(FilePath)) return new List<PatternChoosingDisplayColumns>();

            var json = File.ReadAllText(FilePath);
            var loadedList = JsonSerializer.Deserialize<List<SerializablePattern>>(json);

            return loadedList?.Select(item => new PatternChoosingDisplayColumns
            {
                PatternName = item.PatternName,
                PatternSize = item.PatternSize,
                PatternMatrix = To2DArray(item.PatternMatrix)
            }).ToList() ?? new List<PatternChoosingDisplayColumns>();
        }

        /// <summary>
        /// Converts a 2D array to a jagged array for serialization.
        /// </summary>
        private static bool[][] ToJaggedArray(bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            var result = new bool[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new bool[cols];
                for (int j = 0; j < cols; j++)
                {
                    result[i][j] = matrix[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a jagged array from file back to a 2D array.
        /// </summary>
        private static bool[,] To2DArray(bool[][] jagged)
        {
            int rows = jagged.Length;
            int cols = jagged[0].Length;
            var result = new bool[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = jagged[i][j];
                }
            }

            return result;
        }
    }
}
