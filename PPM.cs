using System.IO;

static class PPM {
    public static void WritePPM(float[][][] pixels, string filename, int samplesPerPixel = 1) {
        using var writer = new StreamWriter(filename);
        writer.WriteLine("P3");
        writer.WriteLine($"{pixels.Length} {pixels[0].Length}");
        writer.WriteLine("255");

        int width = pixels.Length;
        int height = pixels[0].Length;

        void WriteColor(StreamWriter writer, float[] p) {
            var scale = 1.0f / (float)samplesPerPixel;
            
            var r = (int)(256 * Math.Clamp(p[0] * scale, 0, 0.9999f));
            var g = (int)(256 * Math.Clamp(p[1] * scale, 0, 0.9999f));
            var b = (int)(256 * Math.Clamp(p[2] * scale, 0, 0.9999f));

            writer.WriteLine($"{r} {g} {b}");
        }

        for (int j = height-1; j >= 0; --j)
            for (int i = 0; i < width; ++i) {
                var p = pixels[i][j];
                WriteColor(writer, p);
            }
    }

    
}    