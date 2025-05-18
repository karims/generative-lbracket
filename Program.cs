// // 1) bring in the PicoGK API and Vector3
// using System;
// using System.Numerics;
// using PicoGK;

// namespace CodingForEngineers.Chapter08
// {
//     class Program
//     {
//         static void Main(string[] args)
//         {
//             try
//             {
//                 // 2) launch the live viewer at 0.5 mm voxel resolution,
//                 //    running our Run() method as the task
//                 Library.Go(0.5f, Run);
//             }
//             catch (Exception e)
//             {
//                 // 3) if anything goes wrong, print the exception
//                 Console.WriteLine(e);
//             }
//         }

//         // 4) this method builds two concentric lattice beams (outer & inner),
//         //    voxelizes them, subtracts inner from outer, and shows the result
//         public static void Run()
//         {
//             // // a) outer pipe lattice (radius 10 mm)
//             // var latOutside = new Lattice();
//             // latOutside.AddBeam(
//             //     new Vector3(0, 0, 0),
//             //     new Vector3(50, 0, 0),
//             //     10f,   // start radius
//             //     10f,   // end radius
//             //     false  // flat caps
//             // );

//             // // b) inner cutout lattice (radius 8 mm)
//             // var latInside = new Lattice();
//             // latInside.AddBeam(
//             //     new Vector3(0, 0, 0),
//             //     new Vector3(50, 0, 0),
//             //     8f,
//             //     8f,
//             //     false
//             // );

//             // // c) voxelize both
//             // var voxOutside = new Voxels(latOutside);
//             // var voxInside  = new Voxels(latInside);

//             // // d) subtract inner from outer → hollow pipe
//             // voxOutside.BoolSubtract(voxInside);
//             Lattice lat = new();
			
//             lat.AddBeam(    new Vector3(0,0,0),
//                             new Vector3(50,0,0),
//                             5, 20,
//                             true);

//             Voxels vox = new(lat);
//             var viewer = Library.oViewer();

//             // Set group 0 to bright blue, fully opaque, no gloss
//             viewer.SetGroupMaterial(
//                 /* groupId   */ 0,
//                 /* hexColor  */ "3399FF",
//                 /* opacity   */ 1.0f,
//                 /* glossiness*/ 0.0f
//             );

//             // Now add the pipe into group 0
//             viewer.Add(vox, 0);

//             // e) show it in the PicoGK viewer
//             // Library.oViewer().Add(vox);
//         }
//     }
// }


using System;
using System.Numerics;
using PicoGK;
using LBracketDemo;

class Program
{
    static void Main()
    {
        // Launch PicoGK with 1.0 voxel size and run the build inside the Go callback
        // Library.Go(1.0f, () =>
        // {
        //     SimpleLBracket.Build();
        // });

        // Library.Go(1.0f, () =>
        // {
        //     var vox = ParametricLBracket.Build(
        //         flangeXLength:    60f,
        //         flangeYLength:    90f,
        //         flangeThickness:  10f,
        //         holeRadius:       3f,
        //         holeOffset:       12f,
        //         offsetForCavity:  0f
        //     );

        //     var viewer = Library.oViewer();
        //     viewer.SetGroupMaterial(0, "FF7700", 1.0f, 0.0f);
        //     viewer.Add(vox, 0);

        //     vox.mshAsMesh().SaveToStlFile("ParametricLBracket_Updated.stl");
        // });

        Library.Go(1.0f, () =>
        {
            // BracketBatchRunner.Run("bracket_outputs", density: 1.04f); // e.g., 1.04 for PLA
            // BracketBatchRunner.GenerateFromCSV();
            
                var lines = File.ReadAllLines("notebooks/optimized_brackets.csv");
                var headers = lines[0].Split(',');
                var parts = lines[1].Split(',');
                Console.WriteLine(parts);

                float flangeX    = float.Parse(parts[headers.ToList().IndexOf("params_flangeX")]);
                float flangeY    = float.Parse(parts[headers.ToList().IndexOf("params_flangeY")]);
                float thickness  = float.Parse(parts[headers.ToList().IndexOf("params_thickness")]);
                float holeRadius = float.Parse(parts[headers.ToList().IndexOf("params_holeRadius")]);
                float holeOffset = float.Parse(parts[headers.ToList().IndexOf("params_holeOffset")]);

                var vox = ParametricLBracket.Build(flangeX, flangeY, thickness, holeRadius, holeOffset);
                Library.oViewer().SetGroupMaterial(0, "555577", 1.0f, 0.0f);
                Library.oViewer().Add(vox, 0);

        });


    }
}

