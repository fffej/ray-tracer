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

    public Camera(float vfov, float aspectRatio) {
        var theta = vfov * Math.PI / 180;
        var halfHeight = (float)Math.Tan(theta / 2);
        var halfWidth = aspectRatio * halfHeight;
        origin = new Vector3(0, 0, 0);
        horizontal = new Vector3(2 * halfWidth, 0, 0);
        vertical = new Vector3(0, 2 * halfHeight, 0);
        lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - new Vector3(0, 0, 1);
    }

    public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 vup, double vfov, double aspectRatio) {
        var theta = (float) vfov * Math.PI / 180;
        var h = (float)(2 * Math.Tan(theta / 2));
        var viewportHeight = h;
        var viewportWidth = (float)aspectRatio * viewportHeight;

        var w = Vector3.Normalize(lookFrom - lookAt);
        var u = Vector3.Normalize(Vector3.Cross(vup, w));
        var v = Vector3.Cross(w, u);

        origin  = lookFrom;
        horizontal = u * viewportWidth;
        vertical = v * viewportHeight;
        lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - w;
    }

    public Ray GetRay(float u, float v) {
        return new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
    }
}