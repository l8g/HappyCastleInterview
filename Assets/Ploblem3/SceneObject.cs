using UnityEngine;

public class SceneObject
{
    public int Id { get; set; }
    public string ResourceName { get; set; }
    public GameObject GameObject { get; set; }
    public Vector2d Position { get; set; }

    public SceneObject(int id, string resourceName, Vector2d position)
    {
        Id = id;
        ResourceName = resourceName;
        Position = position;
        GameObject = null;
    }
}


public struct RectD {
    public double x;
    public double y;
    public double width;
    public double height;

    public RectD(double x, double y, double width, double height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public bool Contains(Vector2d point) {
        return point.X >= x && point.X <= x + width && point.Y >= y && point.Y <= y + height;
    }

    public bool Overlaps(RectD other) {
        return !(other.x > x + width || other.x + other.width < x || other.y > y + height || other.y + other.height < y);
    }

    public Rect ToRect() {
        return new Rect((float)x, (float)y, (float)width, (float)height);
    }
}

public struct Vector2d {
    public double X { get; set; }
    public double Y { get; set; }

    public Vector2d(double x, double y) {
        X = x;
        Y = y;
    }

    public static implicit operator Vector2(Vector2d v) {
        return new Vector2((float)v.X, (float)v.Y);
    }

    public static implicit operator Vector2d(Vector2 v) {
        return new Vector2d(v.x, v.y);
    }
}
