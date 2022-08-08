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
        // TODO this is wrong
        var p = Random();
        return Vector3.Normalize(p);
    }
}