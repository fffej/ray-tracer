using System.Numerics;

public record Ray(Vector3 origin, Vector3 direction) {
    public Vector3 At(float t) => origin + t * direction;
    public Vector3 Direction => direction;
    public Vector3 Origin => origin;
}