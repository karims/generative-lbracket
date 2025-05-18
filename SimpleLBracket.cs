using System;
using System.Numerics;
using PicoGK;

namespace LBracketDemo
{
  public static class SimpleLBracket
{
    public static void Build()
    {
        // ← your working voxel pipe / bracket code here

        // ── PARAMS ─────────────────────────
        float W = 50f, L = 80f, T = 10f;
        float R = 3f, O = 12f;

        // ── CREATE & UNION BOXES ───────────
        var vox = new Voxels(Utils.mshCreateCube(
            new BBox3(new Vector3(0), new Vector3(W, T, T))));
        vox.BoolAdd(new Voxels(Utils.mshCreateCube(
            new BBox3(new Vector3(0), new Vector3(T, L, T)))));

        // ── DRILL HOLES ────────────────────
        var cuts = new[]
        {
                new Vector3(W - O, T/2, T/2),
                new Vector3(T/2, L - O, T/2)
            };
        foreach (var c in cuts)
        {
            var top = c + Vector3.UnitZ * (T / 2);
            var bot = c - Vector3.UnitZ * (T / 2);
            var lat = new Lattice();
            lat.AddBeam(bot, R, top, R, false);
            vox.BoolSubtract(new Voxels(lat));
        }

        // ── PREVIEW as voxels with flat RGBA color ───────────
        var v = Library.oViewer();

        v.SetGroupMaterial(
            0, "555577", 0.7f, 0.8f
        );
        // pure cyan, no shading
        v.Add(vox, 0);

        // ── EXPORT mesh ─────────────────────
        var m = vox.mshAsMesh();
        m.SaveToStlFile("LBracket.stl");
        Console.WriteLine("Wrote LBracket.stl");

    }
}  
}

