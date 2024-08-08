using UnityEngine;
using System.Collections.Generic;

public class QuadTreeNode
{
    public RectD bounds;
    List<SceneObject> objects;
    public QuadTreeNode[] children;
    const int MAX_OBJECTS = 4;

    public QuadTreeNode(RectD bounds)
    {
        this.bounds = bounds;
        objects = new List<SceneObject>();
        children = new QuadTreeNode[4];
    }

    public void Insert(SceneObject obj)
    {
        if (!bounds.Contains(obj.Position)) return;

        if (objects.Count < MAX_OBJECTS || IsLeaf())
        {
            objects.Add(obj);
        }
        else
        {
            if (IsLeaf()) Split();
            foreach (var child in children)
            {
                child.Insert(obj);
            }
        }
    }

    public List<SceneObject> Retrieve(RectD cameraView)
    {
        List<SceneObject> result = new List<SceneObject>();
        if (!bounds.Overlaps(cameraView)) return result;

        foreach (var obj in objects)
        {
            if (cameraView.Contains(obj.Position))
            {
                result.Add(obj);
            }
        }

        if (!IsLeaf())
        {
            foreach (var child in children)
            {
                result.AddRange(child.Retrieve(cameraView));
            }
        }

        return result;
    }

    private void Split()
    {
        double halfWidth = bounds.width / 2;
        double halfHeight = bounds.height / 2;
        children[0] = new QuadTreeNode(new RectD(bounds.x, bounds.y, halfWidth, halfHeight));
        children[1] = new QuadTreeNode(new RectD(bounds.x + halfWidth, bounds.y, halfWidth, halfHeight));
        children[2] = new QuadTreeNode(new RectD(bounds.x, bounds.y + halfHeight, halfWidth, halfHeight));
        children[3] = new QuadTreeNode(new RectD(bounds.x + halfWidth, bounds.y + halfHeight, halfWidth, halfHeight));
    }

    public bool IsLeaf()
    {
        return children[0] == null;
    }
}
