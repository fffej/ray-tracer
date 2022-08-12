using System.Numerics;

public static class Vector3Extensions {
    private static Random rnd = new ();

    public static Vector3 Random() {
        return new Vector3(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
    }

    public static Vector3 Random(float min, float max) {
        return new Vector3(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
    }

    public static Vector3 RandomUnitSphere() {
        while (true) {
            var p = Vector3Extensions.Random(-1f, 1f);
            if (p.LengthSquared() >= 1f) {
                continue;
            }

            return p;
        }
    }

    public static Vector3 RandomUnitVector() {
        return Vector3.Normalize(RandomUnitSphere());
    }

    public static Vector3 RandomInHemisphere(Vector3 normal) {
        var inUnitSphere = RandomUnitSphere();
        if (Vector3.Dot(inUnitSphere, normal) > 0f) 
            return inUnitSphere;

        return -inUnitSphere;
    }

    public static bool NearZero(Vector3 v) {
        var epsilon = 0.00001f;

        return Math.Abs(v.X) < epsilon && Math.Abs(v.Y) < epsilon && Math.Abs(v.Z) < epsilon;
    }

    public static Vector3 Reflect(Vector3 v, Vector3 n) {
        return v - 2 * Vector3.Dot(v, n) * n;
    }
}