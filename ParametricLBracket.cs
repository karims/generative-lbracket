using System;
using System.Numerics;
using PicoGK;

namespace LBracketDemo
{
    public static class ParametricLBracket
    {
        /// <summary>
        /// Builds a customizable L-bracket with optional interior offset for lattice fill.
        /// </summary>
        public static Voxels Build(
            float flangeXLength,     // Horizontal arm (X direction)
            float flangeYLength,     // Vertical arm (Y direction)
            float flangeThickness,   // Thickness in Z direction (common to both)
            float holeRadius,        // Radius of bolt holes
            float holeOffset,        // Distance of hole center from arm end
            float offsetForCavity = 0f,   // Optional negative offset for lattice prep
            float holeDepthMargin = 1f    // Extra margin in Z for clean hole cuts
        )
        {
            // ── 1) Create horizontal flange ───────────────
            var horizBox = new BBox3(
                new Vector3(0, 0, 0),
                new Vector3(flangeXLength, flangeThickness, flangeThickness)
            );
            var horizVox = new Voxels(Utils.mshCreateCube(horizBox));

            // ── 2) Create vertical flange (shifted up in Y) ─
            var vertBox = new BBox3(
                new Vector3(0, flangeThickness, 0),
                new Vector3(flangeThickness, flangeYLength + flangeThickness, flangeThickness)
            );
            var vertVox = new Voxels(Utils.mshCreateCube(vertBox));

            // ── 3) Union flanges into one bracket ─────────
            horizVox.BoolAdd(vertVox);

            // ── 4) Subtract two bolt holes ───────────────
            var centers = new[]
            {
                // in horizontal arm (near far X end)
                new Vector3(flangeXLength - holeOffset, flangeThickness / 2, flangeThickness / 2),

                // in vertical arm (near far Y end)
                new Vector3(flangeThickness / 2, flangeYLength - holeOffset, flangeThickness / 2)
            };

            foreach (var center in centers)
            {
                // var top    = center + Vector3.UnitZ * (flangeThickness + holeDepthMargin);
                // var bottom = center - Vector3.UnitZ * holeDepthMargin;

                // BETTER: symmetric, see-through hole
                var top    = center + Vector3.UnitZ * (flangeThickness / 2 + holeDepthMargin);
                var bottom = center - Vector3.UnitZ * (flangeThickness / 2 + holeDepthMargin);


                var lat = new Lattice();
                lat.AddBeam(bottom, holeRadius, top, holeRadius, false);

                horizVox.BoolSubtract(new Voxels(lat));
            }

            // ── 5) Optional: carve cavity for lattice ─────
            if (offsetForCavity > 0f)
            {
                var cavity = new Voxels(horizVox);
                cavity.Offset(-offsetForCavity);
                horizVox.BoolAdd(cavity); // placeholder – replace with lattice later
            }

            return horizVox;
        }
    }
}
