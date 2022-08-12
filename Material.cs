using System.Numerics;
public interface Material {
    bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered);
}

public class NullMaterial : Material {
    private NullMaterial() {}

    public static readonly NullMaterial Value = new NullMaterial();

    public bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered) {
        attenuation = Vector3.Zero;
        scattered = new Ray(Vector3.Zero, Vector3.Zero);

        return false;
    }
}

public class Lambertian : Material {
    private Vector3 albedo;
    public Lambertian(Vector3 albedo) {
        this.albedo = albedo;
    }
    public bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered) {
        var scatterDirection = rec.Normal + Vector3Extensions.RandomUnitVector();
        if (Vector3Extensions.NearZero(scatterDirection)) {
            scatterDirection = rec.Normal;
        }

        scattered = new Ray(rec.P, scatterDirection);
        attenuation = albedo;

        return true;
    }
}

public class Metal : Material {
    private Vector3 albedo;
    private float fuzz;
    public Metal(Vector3 albedo, float fuzz) {
        this.albedo = albedo;
        this.fuzz = fuzz < 1 ? fuzz : 1;
    }
    public bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered) {
        var reflected = Vector3Extensions.Reflect(Vector3.Normalize(rIn.Direction), rec.Normal);
        scattered = new Ray(rec.P, reflected + fuzz*Vector3Extensions.RandomUnitSphere());
        attenuation = albedo;

        return Vector3.Dot(scattered.Direction, rec.Normal) > 0;
    }
}

public class Dielectric : Material {
    private float refIdx;
    public Dielectric(float refIdx) {
        this.refIdx = refIdx;
    }
    public bool Scatter(Ray rIn, HitRecord rec, out Vector3 attenuation, out Ray scattered) {
        attenuation = new Vector3(1.0f, 1.0f, 1.0f);
        var refRatio = rec.FrontFace ? 1.0f / refIdx : refIdx;

        var unitDirection = Vector3.Normalize(rIn.Direction);
        
        var cosTheta = Math.Min(Vector3.Dot(-unitDirection, rec.Normal), 1.0f);
        var sinTheta = (float)Math.Sqrt(1.0f - cosTheta * cosTheta);

        var cannotRefract = refRatio * sinTheta > 1;

        Vector3 direction = cannotRefract ? 
            Vector3Extensions.Reflect(unitDirection, rec.Normal) : 
            Vector3Extensions.Refract(unitDirection, rec.Normal, refRatio);

        scattered = new Ray(rec.P, direction);

        return true;
    }
}