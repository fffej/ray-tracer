using System.Numerics;

static void WriteTestImage() {

    const int width = 256;
    const int height = 256;

    var image = CreateBlankImage(width, height);

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

static float[][][] CreateBlankImage(int width, int height) {
    var image = new float[width][][];
    for (int i=0; i<width; i++) {
        image[i] = new float[height][];
        for (int j=0; j<height; j++) {
            image[i][j] = new float[3];
        }
    }
    return image;
}

static float[] ToArray(Vector3 v) {
    return new float[] { v.X, v.Y, v.Z };
}

static void BlueToWhite() {

    static Vector3 RayColor(Ray r) {
        var unitDirection = Vector3.Normalize(r.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
    }

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

    var pixels = CreateBlankImage(imageWidth, imageHeight);

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

static void ASimpleRedSphere() {

    static bool HitSphere(Vector3 center, float radius, Ray r) {
        var oc = r.Origin - center;
        var a = Vector3.Dot(r.Direction, r.Direction);
        var b = 2.0f * Vector3.Dot(oc, r.Direction);
        var c = Vector3.Dot(oc, oc) - radius * radius;
        var discriminant = b * b - 4.0f * a * c;
        return discriminant > 0.0f;
    }

    static Vector3 RayColor(Ray r) {
        if (HitSphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, r)) 
            return new Vector3(1.0f, 0.0f, 0.0f);
        
        var unitDirection = Vector3.Normalize(r.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
        
    }

    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);

    var viewportHeight = 2.0f;
    var viewportWidth = aspectRatio * viewportHeight;
    var focalLength = 1.0f;

    var origin = new Vector3(0f,0f,0f);
    var horizontal = new Vector3(viewportWidth,0f,0f);
    var vertical = new Vector3(0f,viewportHeight,0f);
    var lowerLeftCorner = origin - horizontal/2.0f - vertical/2.0f - new Vector3(0f,0f,focalLength);

    var pixels = CreateBlankImage(imageWidth, imageHeight);

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {
            var u = (float)i / (imageWidth-1);
            var v = (float)j / (imageHeight-1);
            var r = new Ray(origin, lowerLeftCorner + u*horizontal + v*vertical - origin);

            var rayColor = RayColor(r);

            pixels[i][j] = ToArray(rayColor);
        }
    }    

    PPM.WritePPM(pixels, "simple-red-sphere.ppm");
}

static void ColoredSphere() {

    static float HitSphere(Vector3 center, float radius, Ray r) {
        var oc = r.Origin - center;
        var a = Vector3.Dot(r.Direction, r.Direction);
        var b = 2.0f * Vector3.Dot(oc, r.Direction);
        var c = Vector3.Dot(oc, oc) - radius * radius;
        var discriminant = b * b - 4.0f * a * c;

        if (discriminant < 0) {
            return -1.0f;
        } else {
            return (-b - (float)Math.Sqrt(discriminant)) / (2.0f * a);
        }
    }

    static Vector3 RayColor(Ray r) {
        var t =  HitSphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, r);
        if (t > 0.0f) {
            var n = Vector3.Normalize(r.At(t) - new Vector3(0.0f, 0.0f, -1.0f));

            return 0.5f * new Vector3(n.X + 1.0f, n.Y + 1.0f, n.Z + 1.0f);
        }

        var unitDirection = Vector3.Normalize(r.Direction);
        var s = 0.5f * (unitDirection.Y + 1.0f);

        return (1.0f - s) * new Vector3(1.0f, 1.0f, 1.0f) + s * new Vector3(0.5f, 0.7f, 1.0f);
    }

    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);

    var viewportHeight = 2.0f;
    var viewportWidth = aspectRatio * viewportHeight;
    var focalLength = 1.0f;

    var origin = new Vector3(0f,0f,0f);
    var horizontal = new Vector3(viewportWidth,0f,0f);
    var vertical = new Vector3(0f,viewportHeight,0f);
    var lowerLeftCorner = origin - horizontal/2.0f - vertical/2.0f - new Vector3(0f,0f,focalLength);

    var pixels = CreateBlankImage(imageWidth, imageHeight);

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {
            var u = (float)i / (imageWidth-1);
            var v = (float)j / (imageHeight-1);
            var r = new Ray(origin, lowerLeftCorner + u*horizontal + v*vertical - origin);

            var rayColor = RayColor(r);

            pixels[i][j] = ToArray(rayColor);
        }
    }    

    PPM.WritePPM(pixels, "colored-sphere.ppm");
}

WriteTestImage();
BlueToWhite();
ASimpleRedSphere();
ColoredSphere();