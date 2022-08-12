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
        
        var a = r.Direction.LengthSquared();
        var halfB = Vector3.Dot(oc, r.Direction);
        var c = oc.LengthSquared() - radius * radius;
        var discriminant = halfB * halfB - a * c;

        if (discriminant < 0) {
            return -1.0f;            
        }
        else {
            return (-halfB - (float)Math.Sqrt(discriminant)) / a;
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

static void HittableWorld() {

    static Vector3 RayColor(Ray r, Hittable world) {
        HitRecord rec = new();
        if (world.Hit(r,0, float.MaxValue, ref rec)) {
            return 0.5f * (rec.Normal + new Vector3(1.0f, 1.0f, 1.0f));
        }

        var unitDirection = Vector3.Normalize(r.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
    }

    // Create an image
    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);

    // Create a world
    var world = new HittableComposite();
    world.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f));
    world.Add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f));

    // Position the camera
    var viewportHeight = 2.0f;
    var viewportWidth = aspectRatio * viewportHeight;
    var focalLength = 1.0f;

    var origin = new Vector3(0f, 0, 0);
    var horizontal = new Vector3(viewportWidth, 0, 0);
    var vertical = new Vector3(0f, viewportHeight, 0);
    var lowerLeftCorner = origin - horizontal/2 - vertical/2 - new Vector3(0, 0, focalLength);    

    // Do the render
    var pixels = CreateBlankImage(imageWidth, imageHeight);

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {
            var u = (float)i / (imageWidth-1);
            var v = (float)j / (imageHeight-1);
            var r = new Ray(origin, lowerLeftCorner + u*horizontal + v*vertical - origin);

            var rayColor = RayColor(r, world);

            pixels[i][j] = ToArray(rayColor);
        }
    }    

    PPM.WritePPM(pixels, "hittable-world.ppm");    
}

static void HittableWorldWithAntiAliasing() {

    static Vector3 RayColor(Ray r, Hittable world) {
        HitRecord rec = new();
        if (world.Hit(r,0, float.MaxValue, ref rec)) {
            return 0.5f * (rec.Normal + new Vector3(1.0f, 1.0f, 1.0f));
        }

        var unitDirection = Vector3.Normalize(r.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
    }

    // Create an image
    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);
    var samplesPerPixel = 100;

    // Create a world
    var world = new HittableComposite();
    world.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f));
    world.Add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f));

    // Position the camera
    Camera camera = new ();
    Random rnd = new ();

    // Do the render
    var pixels = CreateBlankImage(imageWidth, imageHeight);

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {

            var pixelColor = new Vector3(0.0f, 0.0f, 0.0f);
            for (int s=0;s<samplesPerPixel;++s) {
                var u = (float)(i + rnd.NextSingle()) / (imageWidth-1);
                var v = (float)(j  + rnd.NextSingle()) / (imageHeight-1);

                var r = camera.GetRay(u, v);
                pixelColor += RayColor(r, world);

            }
            pixels[i][j] = ToArray(pixelColor);
        }
    }    

    PPM.WritePPM(pixels, "hittable-world-aa.ppm", samplesPerPixel);    
}

static void DiffuseMaterials() {

    static Vector3 RayColor(Ray r, Hittable world, int depth) {

        if (depth <= 0) 
            return new Vector3(0,0,0f);

        HitRecord rec = new();
        // Get rid of the shadow acne problem
        if (world.Hit(r,0.0001f, float.MaxValue, ref rec)) { 
            Vector3 target = rec.P + Vector3Extensions.RandomInHemisphere(rec.Normal);

            var ray = new Ray(rec.P, target - rec.P);

            return 0.5f * RayColor(ray, world, depth-1);
        }

        var unitDirection = Vector3.Normalize(r.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
    }

    // Create an image
    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);
    var samplesPerPixel = 100;
    var maxDepth = 50;

    // Create a world
    var world = new HittableComposite();
    world.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f));
    world.Add(new Sphere(new Vector3(0.0f, -100.5f, -1.0f), 100.0f));

    // Position the camera
    Camera camera = new ();
    Random rnd = new ();

    // Do the render
    var pixels = CreateBlankImage(imageWidth, imageHeight);

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {

            var pixelColor = new Vector3(0.0f, 0.0f, 0.0f);
            for (int s=0;s<samplesPerPixel;++s) {
                var u = (float)(i + rnd.NextSingle()) / (imageWidth-1);
                var v = (float)(j  + rnd.NextSingle()) / (imageHeight-1);

                var r = camera.GetRay(u, v);
                pixelColor += RayColor(r, world, maxDepth);

            }
            pixels[i][j] = ToArray(pixelColor);
        }
    }    

    //PPM.WritePPM(pixels, "diffuse-materials-gamma.ppm", samplesPerPixel);    
    PPM.WritePPM(pixels, "diffuse-materials-gamma-correct.ppm", samplesPerPixel, true);    
}

static void MetalMaterials() {

    static Vector3 RayColor(Ray r, Hittable world, int depth) {

        if (depth <= 0) 
            return new Vector3(0,0,0f);

        HitRecord rec = new();
        // Get rid of the shadow acne problem
        if (world.Hit(r,0.0001f, float.MaxValue, ref rec)) { 

            if (rec.Material.Scatter(r, rec, out var attenuation, out var scattered)) {
                return attenuation * RayColor(scattered, world, depth-1);
            }
            
            return new Vector3(0,0,0f);
        }

        var unitDirection = Vector3.Normalize(r.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
    }

    // Create an image
    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);
    var samplesPerPixel = 100;
    var maxDepth = 50;

    // Create a world
    var world = new HittableComposite();
    
    var materialGround = new Lambertian(new Vector3(0.8f, 0.8f, 0.0f));
    var materialCenter = new Lambertian(new Vector3(0.7f, 0.3f, 0.3f));
    var materialLeft = new Metal(new Vector3(0.8f, 0.8f, 0.8f), 0.3f);
    var materialRight = new Metal(new Vector3(0.8f, 0.6f, 0.2f), 1.0f);

    world.Add(new Sphere(new Vector3(0.0f,-100.5f, -1.0f), 100.0f, materialGround));
    world.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, materialCenter));
    world.Add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, materialLeft));
    world.Add(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, materialRight));    

    // Position the camera
    Camera camera = new ();
    Random rnd = new ();

    // Do the render
    var pixels = CreateBlankImage(imageWidth, imageHeight);

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {

            var pixelColor = new Vector3(0.0f, 0.0f, 0.0f);
            for (int s=0;s<samplesPerPixel;++s) {
                var u = (float)(i + rnd.NextSingle()) / (imageWidth-1);
                var v = (float)(j  + rnd.NextSingle()) / (imageHeight-1);

                var r = camera.GetRay(u, v);
                pixelColor += RayColor(r, world, maxDepth);

            }
            pixels[i][j] = ToArray(pixelColor);
        }
    }    

    //PPM.WritePPM(pixels, "diffuse-materials-gamma.ppm", samplesPerPixel);    
    PPM.WritePPM(pixels, "fuzzed-metal.ppm", samplesPerPixel, true);    
}

static void GlassSphereThatAlwaysRefracts() {

    static Vector3 RayColor(Ray r, Hittable world, int depth) {

        if (depth <= 0) 
            return new Vector3(0,0,0f);

        HitRecord rec = new();
        // Get rid of the shadow acne problem
        if (world.Hit(r,0.0001f, float.MaxValue, ref rec)) { 

            if (rec.Material.Scatter(r, rec, out var attenuation, out var scattered)) {
                return attenuation * RayColor(scattered, world, depth-1);
            }
            
            return new Vector3(0,0,0f);
        }

        var unitDirection = Vector3.Normalize(r.Direction);
        var t = 0.5f * (unitDirection.Y + 1.0f);
        return (1.0f - t) * new Vector3(1.0f, 1.0f, 1.0f) + t * new Vector3(0.5f, 0.7f, 1.0f);
    }

    // Create an image
    var aspectRatio = 16.0f/9.0f;
    var imageWidth = 400;
    var imageHeight = (int)(imageWidth / aspectRatio);
    var samplesPerPixel = 100;
    var maxDepth = 50;

    // Create a world
    var world = new HittableComposite();
    
    var materialGround = new Lambertian(new Vector3(0.8f, 0.8f, 0.0f));
    var materialCenter = new Dielectric(1.5f);
    var materialLeft = new Dielectric(1.5f);
    var materialRight = new Metal(new Vector3(0.8f, 0.6f, 0.2f), 1.0f);

    world.Add(new Sphere(new Vector3(0.0f,-100.5f, -1.0f), 100.0f, materialGround));
    world.Add(new Sphere(new Vector3(0.0f, 0.0f, -1.0f), 0.5f, materialCenter));
    world.Add(new Sphere(new Vector3(-1.0f, 0.0f, -1.0f), 0.5f, materialLeft));
    world.Add(new Sphere(new Vector3(1.0f, 0.0f, -1.0f), 0.5f, materialRight));    

    // Position the camera
    Camera camera = new ();
    Random rnd = new ();

    // Do the render
    var pixels = CreateBlankImage(imageWidth, imageHeight);

    for (int j = imageHeight-1; j >= 0; --j) {
        for (int i = 0; i < imageWidth; ++i) {

            var pixelColor = new Vector3(0.0f, 0.0f, 0.0f);
            for (int s=0;s<samplesPerPixel;++s) {
                var u = (float)(i + rnd.NextSingle()) / (imageWidth-1);
                var v = (float)(j  + rnd.NextSingle()) / (imageHeight-1);

                var r = camera.GetRay(u, v);
                pixelColor += RayColor(r, world, maxDepth);

            }
            pixels[i][j] = ToArray(pixelColor);
        }
    }    

    //PPM.WritePPM(pixels, "diffuse-materials-gamma.ppm", samplesPerPixel);    
    PPM.WritePPM(pixels, "glass-sphere-that-always-refracts.ppm", samplesPerPixel, true);    
}

WriteTestImage();
BlueToWhite();
ASimpleRedSphere();
ColoredSphere();
HittableWorld();
HittableWorldWithAntiAliasing();
DiffuseMaterials();
MetalMaterials();
GlassSphereThatAlwaysRefracts();