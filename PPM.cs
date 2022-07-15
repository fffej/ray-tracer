using System.IO;

static class PPM {
    public static void WritePPM(float[][][] pixels, string filename) {
        using var writer = new StreamWriter(filename);
        writer.WriteLine("P3");
        writer.WriteLine($"{pixels[0].Length} {pixels.Length}");
        writer.WriteLine("255");

        int width = pixels.Length;
        int height = pixels[0].Length;

        for (int j = height-1; j >= 0; --j)
            for (int i = 0; i < width; ++i) {

                var p = pixels[i][j];

                int ir = (int)(255.999 * p[0]);
                int ig = (int)(255.999 * p[1]);
                int ib = (int)(255.999 * p[2]);

                writer.WriteLine($"{ir} {ig} {ib}");
            }
    }
}    