# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Windows Forms desktop application implementing a configurable Genetic Algorithm (GA) framework. Used for pattern optimization and classification experiments, supporting both supervised and unsupervised learning modes. Part of a master's thesis project.

## Tech Stack

- **Language:** C# (.NET 6.0)
- **UI:** Windows Forms (WinExe), single project (`Lab2/Lab2.csproj`), no separate class library
- **Key dependencies:**
  - `MathNet.Numerics 5.0.0` — numerical computations
  - `WinForms.DataVisualization 1.9.2` — charting/plotting

## Build & Run

```bash
# Build
dotnet build Lab2/Lab2.csproj

# Run
dotnet run --project Lab2/Lab2.csproj
```

Assembly name is `praca-magisterska` (differs from the `Lab2` project/folder name — see `Lab2.csproj`). Output binary: `Lab2/bin/Debug/net6.0-windows/praca-magisterska.exe`.

There is no automated test suite and no test project in `Lab2.sln` — verify changes by running the app and exercising the GUI manually.

## Architecture

```
Lab2/
├── Core/
│   ├── Algorithm/       # GA engine: GeneticAlgorithm.cs (run loop), GeneticAlgorithmBuilder.cs, Population.cs
│   ├── Domain/          # Individual, InitialData, InitialGenotypeFactory, MatrixPaddingMapper
│   ├── Enums/           # AlgorithmType, AlgorithmOption, CrossType, MutationType, SelectionType
│   ├── Fitness/         # IFitnessEvaluator: Supervised (vs. reference matrices) & Unsupervised
│   ├── Operators/
│   │   ├── Crossover/   # One-point, multi-point
│   │   ├── Mutation/    # BitFlip, BitSwap, Composite, Narrowing, UniformBlock
│   │   └── Selection/   # Roulette, Ranking, Tournament (hard/soft)
│   ├── Random/          # IRandomProvider / SeededRandomProvider — inject for reproducible runs
│   ├── Statistics/      # PopulationStatisticsCollector — per-generation metrics
│   ├── Termination/     # ITerminationCondition (e.g. MaxIterationCondition)
│   └── Validation/      # IValidator, InitialDataValidator, ValidationResult
├── Infrastructure/
│   └── FileUtils.cs     # Active file I/O for patterns/reference matrices/results (see note below)
├── Services/            # ValidationFacade, ValidationService, StartService
├── UI/                  # MainWindow + modal windows (WinForms designer partials), UI/Domain display models
├── Utils/               # CrossUtils, SelectionUtils
├── objects/             # Supporting DTOs: DetailedRunObject, DetailedTestObject, TestObject, SumUp, RandomSingleton
└── Program.cs           # Entry point (STAThread, runs MainWindow)
```

`Data/` (patterns.json, referenceMatrixes.json) and `Results/` (results_GA.txt, tunning_GA.txt, max_f_C_corr.txt, etc.) are not part of the source tree — `Infrastructure/FileUtils.cs` creates/reads them at runtime relative to `AppDomain.CurrentDomain.BaseDirectory` (i.e. next to the built exe), and they are git-ignored.

**Note:** `Services/FileUtils.cs` is entirely commented-out dead code from a prior refactor — the live implementation is `Infrastructure/FileUtils.cs`. Don't edit the `Services` copy expecting it to run.

## Design Patterns

- **Builder** — `GeneticAlgorithmBuilder` assembles a `GeneticAlgorithm`; required components (population, fitness, selection, crossover, mutation, termination, crossover random provider) are validated in `Build()` and throw `InvalidOperationException` if missing.
- **Strategy** — pluggable Selection, Crossover, Mutation, Fitness, and Termination implementations via interfaces (`ISelectionStrategy`, `ICrossoverOperator`, `IMutationOperator`, `IFitnessEvaluator`, `ITerminationCondition`).
- **Facade** — `ValidationFacade` coordinates input validation.

## GA Engine Notes

- Genotypes are `bool[,]` matrices; `Individual` wraps a genotype with a `decimal Fitness`.
- `GeneticAlgorithm.Run` evaluates fitness in parallel (`Parallel.ForEach`, one thread per core), tracks `BestSolution`/`PerfectSolution` (fitness ≥ 1) per generation, and can early-stop via `stopAtFirstCorrect`.
- Optional elitism clones the top `_eliteCount` individuals into the next generation unchanged.
- Optional **stagnation diversification** (`ConfigureStagnationReset`/`WithStagnationReset`): if best fitness hasn't improved for `stagnationWindow` generations and population diversity drops below a threshold, the weakest tail of the population is replaced with mutated clones of strong individuals. Disabled unless explicitly configured via the builder.
- `StoreHistory` (opt-in) snapshots the full population every generation into `History` — expensive, only enable when needed (e.g. for the history view window).

| Component | Options |
|-----------|---------|
| Selection | Roulette, Ranking, Tournament Hard, Tournament Soft |
| Crossover | One-point, Multi-point |
| Mutation | BitFlip, BitSwap, Composite, Narrowing, UniformBlock |
| Fitness | Supervised (vs. reference matrices), Unsupervised |
| Termination | Configurable stopping conditions (e.g. max iterations) |
