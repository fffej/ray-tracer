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
}