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
            for (int i = 0; i < width; ++i)
                writer.WriteLine($"{pixels[i][j][0]} {pixels[i][j][1]} {pixels[i][j][2]}");
    }
}    