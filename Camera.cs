using System.Numerics;

public class Camera {
    private Vector3 origin;
    private Vector3 lowerLeftCorner;
    private Vector3 horizontal; 
    private Vector3 vertical;

    public Camera() {
        var aspect = 16.0f / 9.0f;
        var viewportHeight = 2.0f;
        var viewportWidth = aspect * viewportHeight;
        var focalLength = 1.0f;

        origin = new Vector3(0, 0, 0);
        horizontal = new Vector3(viewportWidth, 0, 0);
        vertical = new Vector3(0, viewportHeight, 0);
        lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vector3(0, 0, focalLength);
    }

    public Ray GetRay(float u, float v) {
        return new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
    }
}