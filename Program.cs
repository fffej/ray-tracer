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
            var r = (float)i / (width-1);
            var g = (float)j / (height-1);
            var b = 0.25f;

            image[i][j][0] = r;
            image[i][j][1] = g;
            image[i][j][2] = b;
        }
    }

    PPM.WritePPM(image, "out.ppm");
}

static Vector3 RayColor(Ray r) {
    var unitDirection = Vector3.Normalize(r.Direction);
    var t = 0.5f * (unitDirection.Y + 1.0f);
    return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
}

static float[] ToArray(Vector3 v) {
    return new float[] { v.X, v.Y, v.Z };
}

static void BlueToWhite() {
    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);

    var viewportHeight = 2.0f;
    var viewportWidth = aspectRatio * viewportHeight;
    var focalLength = 1.0f;

    var origin = new Vector3(0f,0f,0f);
    var horizontal = new Vector3(viewportWidth,0f,0f);
    var vertical = new Vector3(0f,viewportHeight,0f);
    var lowerLeftCorner = origin - horizontal/2 - vertical/2 - new Vector3(0f,0f,focalLength);

    var pixels = new float[imageWidth][][];
    for (int i=0; i<imageWidth; i++) {
        pixels[i] = new float[imageHeight][];
        for (int j=0; j<imageHeight; j++) {
            pixels[i][j] = new float[3];
        }
    }    

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {
            var u = (float)i / (imageWidth-1);
            var v = (float)j / (imageHeight-1);
            var r = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
            pixels[i][j] = ToArray(RayColor(r));
        }
    }

    PPM.WritePPM(pixels, "blue-to-white.ppm");
}

WriteTestImage();
BlueToWhite();