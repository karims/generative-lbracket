# Generative L-Bracket Design with PicoGK + ML

This project explores **parametric design and generative optimization** of mechanical L-brackets using [Leap71's PicoGK](https://github.com/leap71/PicoGK) geometry kernel and machine learning.

## 🚀 Features

- 📐 **Parametric 3D modeling** of L-brackets using C# + PicoGK
- ⚙️ Configurable parameters:
  - `flangeX`, `flangeY` — arm lengths
  - `thickness` — bracket depth
  - `holeRadius`, `holeOffset` — bolt hole properties
- 🧱 **CSV-based dataset generation** with:
  - Bounding box dimensions
  - Estimated volume and mass
  - Optional STL export
- 🧠 **ML model training** in Python:
  - Regression (Random Forest, MLP) to predict bracket mass
  - Visual analysis and prediction evaluation
- 🧬 **Generative design** using Optuna:
  - Minimizes predicted mass
  - Outputs top-performing bracket designs

## 📦 Dependencies

### C#
- [.NET 8.0 SDK](https://dotnet.microsoft.com/)
- [PicoGK](https://github.com/leap71/PicoGK) (included as submodule or library)

### Python
- `pandas`, `scikit-learn`, `optuna`, `matplotlib`, `seaborn`
```bash
pip install pandas scikit-learn optuna matplotlib seaborn
````

## 🛠 How to Run

1. Generate dataset:

   ```csharp
   Library.Go(1.0f, () => BracketBatchRunner.Run());
   ```

2. Train ML model + visualize:

   ```bash
   cd notebooks/
   jupyter notebook bracket_analysis.ipynb
   ```

3. Run generative design:

   ```bash
   python optimize_bracket.py
   ```

4. Preview best bracket in viewer:

   ```csharp
   Library.Go(1.0f, () => BracketBatchRunner.GenerateFromCSV());
   ```

## 📈 Future Ideas

* Add structural simulation (FEA integration)
* Explore lattice infills and weight optimization
* Multi-objective optimization (e.g., strength vs. mass)
* Voxel-based generative modeling with neural networks

