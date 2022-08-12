using System.Numerics;

// Refactor the crap out of this code.
public record HitRecord(Vector3 p = new Vector3(), Vector3 normal = new Vector3(), float t = 0, bool frontFace = false) {
    public Vector3 P { get; set; }
    public Vector3 Normal { get; set; }
    public float T { get; set; }
    public bool FrontFace { get; set; }

    public Material Material { get; set; }

    public void SetFaceNormal(Ray r, Vector3 outwardNormal) {
        this.Normal = outwardNormal;
        this.FrontFace = Vector3.Dot(r.Direction, outwardNormal) < 0;
        if (!FrontFace) {
            this.Normal = -this.Normal;
        }
    }
}

public interface Hittable {
    public bool Hit(Ray r, float tMin, float tMax, out HitRecord rec);
}

public class HittableComposite : Hittable {

    private IList<Hittable> objects = new List<Hittable>();

    public void Add(Hittable h) {
        objects.Add(h);
    }

    public bool Hit(Ray r, float tMin, float tMax, out HitRecord rec) {
        var hitAnything = false;
        var closestSoFar = tMax;

        rec = new HitRecord();

// TODO: This is really a fold
        foreach(var o in objects)  {
            if (o.Hit(r, tMin, closestSoFar, out var tempRec)) {
                hitAnything = true;
                closestSoFar = tempRec.T;
                rec = tempRec;
            }
        }

        return hitAnything;
    }
}

public class Sphere : Hittable {

    private float radius;

    private Vector3 center;

    private Material material;

    public Sphere(Vector3 center, float radius) : this(center, radius, NullMaterial.Value){
    }

    public Sphere(Vector3 center, float radius, Material material) {
        this.center = center;
        this.radius = radius;
        this.material = material;
    }

     public bool Hit(Ray r, float tMin, float tMax, out HitRecord rec) {
        var oc = r.Origin - center;
        var a = r.Direction.LengthSquared();
        var halfB = Vector3.Dot(oc, r.Direction);
        var c = oc.LengthSquared() - radius * radius;
        
        var discriminant = halfB * halfB - a * c;
        if (discriminant < 0) {
            rec = new HitRecord();
            return false;
        }

        // Find root in acceptable range
        var sqrtD = (float)Math.Sqrt(discriminant);
        var root = (-halfB - sqrtD) / a;
        if (root < tMin || tMax < root) {
            root = (-halfB + sqrtD) / a;
            if (root < tMin || tMax < root) {
                rec = new HitRecord();
                return false;
            }
        }

        rec = new HitRecord();
        rec.T = root;
        rec.P = r.At(rec.T);
        var outwardNormal = (rec.P - center) / radius;
        rec.SetFaceNormal(r, outwardNormal);
        rec.Material = material;

        return true;
     }
}