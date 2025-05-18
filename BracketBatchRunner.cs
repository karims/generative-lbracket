using System;
using System.IO;
using System.Globalization;
using PicoGK;
using System.Numerics;

namespace LBracketDemo
{
    public static class BracketBatchRunner
    {
        public static void Run(string outputFolder = "bracket_outputs", float density = 1.0f)

        {
            Directory.CreateDirectory(outputFolder);
            string csvPath = Path.Combine(outputFolder, "brackets.csv");

            using var writer = new StreamWriter(csvPath);
            writer.WriteLine("flangeX,flangeY,thickness,holeRadius,holeOffset,bboxX,bboxY,bboxZ,volumeEstimate,mass,sliceCount");

            // Parameter sweep ranges
            float[] flangeXOptions = { 50f, 60f, 70f };
            float[] flangeYOptions = { 60f, 80f, 100f };
            float[] holeOffsets = { 8f, 10f, 12f };

            // Shared parameters
            float thickness = 10f;
            float holeRadius = 3f;

            int index = 0;

            foreach (var flangeX in flangeXOptions)
                foreach (var flangeY in flangeYOptions)
                    foreach (var holeOffset in holeOffsets)
                    {
                        // Build bracket
                        var vox = ParametricLBracket.Build(
                            flangeXLength: flangeX,
                            flangeYLength: flangeY,
                            flangeThickness: thickness,
                            holeRadius: holeRadius,
                            holeOffset: holeOffset
                        );

                        // Bounding box & dimensions
                        BBox3 bbox = vox.oCalculateBoundingBox();
                        Vector3 dims = bbox.vecMax - bbox.vecMin;
                        float bboxX = dims.X;
                        float bboxY = dims.Y;
                        float bboxZ = dims.Z;

                        // Volume and mass estimate
                        float volumeEstimate = bboxX * bboxY * bboxZ; // mm³
                        float mass = volumeEstimate * density;

                        // Optional: slice count
                        int sliceCount = vox.nSliceCount();

                        // Write CSV
                        writer.WriteLine(string.Join(",",
                            flangeX.ToString(CultureInfo.InvariantCulture),
                            flangeY.ToString(CultureInfo.InvariantCulture),
                            thickness.ToString(CultureInfo.InvariantCulture),
                            holeRadius.ToString(CultureInfo.InvariantCulture),
                            holeOffset.ToString(CultureInfo.InvariantCulture),
                            bboxX.ToString("F2", CultureInfo.InvariantCulture),
                            bboxY.ToString("F2", CultureInfo.InvariantCulture),
                            bboxZ.ToString("F2", CultureInfo.InvariantCulture),
                            volumeEstimate.ToString("F2", CultureInfo.InvariantCulture),
                            mass.ToString("F2", CultureInfo.InvariantCulture),
                            sliceCount
                        ));

                        Console.WriteLine($"✅ Bracket {index++} | {bboxX:F0}×{bboxY:F0}×{bboxZ:F0} mm | mass: {mass:F1}");
                    }

            Console.WriteLine($"\n📁 CSV written to: {csvPath}");
        }
        
        public static void GenerateFromCSV(string csvPath = "./notebooks/optimized_brackets.csv", string outputFolder = "./notebooks/optimized_outputs")
        {
            Directory.CreateDirectory(outputFolder);

            var lines = File.ReadAllLines(csvPath);
            var headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');

                float flangeX    = float.Parse(parts[headers.ToList().IndexOf("params_flangeX")]);
                float flangeY    = float.Parse(parts[headers.ToList().IndexOf("params_flangeY")]);
                float thickness  = float.Parse(parts[headers.ToList().IndexOf("params_thickness")]);
                float holeRadius = float.Parse(parts[headers.ToList().IndexOf("params_holeRadius")]);
                float holeOffset = float.Parse(parts[headers.ToList().IndexOf("params_holeOffset")]);

                var vox = ParametricLBracket.Build(
                    flangeX, flangeY, thickness, holeRadius, holeOffset
                );

                string filename = $"opt_bracket_{i}.stl";
                vox.mshAsMesh().SaveToStlFile(Path.Combine(outputFolder, filename));
                Console.WriteLine($"✅ Saved: {filename}");
            }

            Console.WriteLine($"\n📁 All optimized brackets saved to: {outputFolder}/");
        }

    }
}
