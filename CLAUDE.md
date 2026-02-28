# CLAUDE.md

## Project Overview

Windows Forms desktop application implementing a configurable Genetic Algorithm (GA) framework. Used for pattern optimization and classification experiments, supporting both supervised and unsupervised learning modes. Part of a master's thesis project.

## Tech Stack

- **Language:** C# (.NET 6.0)
- **UI:** Windows Forms (WinExe)
- **Key dependencies:**
  - `MathNet.Numerics 5.0.0` — numerical computations
  - `WinForms.DataVisualization 1.9.2` — charting/plotting

## Build & Run

```bash
# Build
dotnet build Lab2.csproj

# Run
dotnet run --project Lab2.csproj
```

Output binary: `bin/Debug/net6.0-windows/Lab2.exe`

## Architecture

```
Lab2/
├── Core/
│   ├── Algorithm/       # GA engine (GeneticAlgorithm.cs, GeneticAlgorithmBuilder.cs)
│   ├── Domain/          # Individual, population data models
│   ├── Enums/           # Configuration enums
│   ├── Fitness/         # Supervised & unsupervised fitness evaluators
│   ├── Operators/
│   │   ├── Crossover/   # One-point, multi-point crossover
│   │   ├── Mutation/    # BitFlip, BitSwap, Composite, Narrowing, UniformBlock
│   │   └── Selection/   # Roulette, Tournament (hard/soft)
│   ├── Statistics/      # Population metrics collection
│   ├── Termination/     # Stopping conditions
│   └── Validation/      # Input validation
├── Infrastructure/      # File I/O (FileUtils.cs)
├── Services/            # ValidationFacade, app-level services
├── UI/                  # MainWindow.cs and UI domain models
├── Utils/               # Helper utilities
├── Data/                # referenceMatrices.json, patterns.json
├── Results/             # Output files (results_GA.txt, etc.)
├── Program.cs           # Entry point
└── Lab2.csproj
```

## Design Patterns

- **Builder pattern** — `GeneticAlgorithmBuilder` for assembling GA configurations
- **Strategy pattern** — pluggable Selection, Crossover, Mutation, Fitness, and Termination implementations via interfaces
- **Facade** — `ValidationFacade` coordinates input validation

## GA Configuration

The algorithm is configured through the builder with the following interchangeable strategies:

| Component | Options |
|-----------|---------|
| Selection | Roulette, Tournament Hard, Tournament Soft |
| Crossover | One-point, Multi-point |
| Mutation | BitFlip, BitSwap, Composite, Narrowing, UniformBlock |
| Fitness | Supervised (vs. reference matrices), Unsupervised |
| Termination | Configurable stopping conditions |

## Data & Results

- Input patterns: `Data/patterns.json`
- Reference matrices: `Data/referenceMatrices.json`
- Results written to: `Results/results_GA.txt`, `tunning_GA.txt`, `max_f_C_corr.txt`

## Notes

- No automated test suite — testing is manual via the GUI
- Windows-only (Windows Forms target platform)
- Population statistics are tracked per generation for analysis and charting
