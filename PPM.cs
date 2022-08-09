using System.IO;

static class PPM {
    public static void WritePPM(float[][][] pixels, string filename, int samplesPerPixel = 1, bool gammaCorrect = false) {
        using var writer = new StreamWriter(filename);
        writer.WriteLine("P3");
        writer.WriteLine($"{pixels.Length} {pixels[0].Length}");
        writer.WriteLine("255");

        int width = pixels.Length;
        int height = pixels[0].Length;

        // It's not good to mutate p in this loop, but it's not a big deal
        // as long as you don't write data out twice!
        void WriteColor(StreamWriter writer, float[] p) {
            var scale = 1.0f / (float)samplesPerPixel;
                
            p[0] *= scale;
            p[1] *= scale;
            p[2] *= scale;

            if (gammaCorrect) {
                p[0] = (float)Math.Sqrt(p[0]);
                p[1] = (float)Math.Sqrt(p[1]);
                p[2] = (float)Math.Sqrt(p[2]);
            }

            var r = (int)(256 * Math.Clamp(p[0], 0, 0.9999f));
            var g = (int)(256 * Math.Clamp(p[1], 0, 0.9999f));
            var b = (int)(256 * Math.Clamp(p[2], 0, 0.9999f));

            writer.WriteLine($"{r} {g} {b}");
        }

        for (int j = height-1; j >= 0; --j)
            for (int i = 0; i < width; ++i) {
                var p = pixels[i][j];
                WriteColor(writer, p);
            }
    }    
}    