// See https://aka.ms/new-console-template for more information

// Vec3
using System.Numerics;

static void WriteTestImage() {

    const int width = 256;
    const int height = 256;

    var image = new float[width][][];
    for (int i=0; i<width; i++) {
        image[i] = new float[height][];
        for (int j=0; j<height; j++) {
            image[i][j] = new float[3];
        }
    }

    for (int j = height-1; j >= 0; --j) {
        for (int i = 0; i < width; ++i) {
            var r = (double)i / (width-1);
            var g = (double)j / (height-1);
            var b = 0.25;

            int ir = (int)(255.999 * r);
            int ig = (int)(255.999 * g);
            int ib = (int)(255.999 * b);

            image[i][j][0] = ir;
            image[i][j][1] = ig;
            image[i][j][2] = ib;
        }
    }

    PPM.WritePPM(image, "out.ppm");
}

WriteTestImage();