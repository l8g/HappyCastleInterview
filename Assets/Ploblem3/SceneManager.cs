using UnityEngine;
using System.Collections.Generic;

public class SceneManager
{
    public QuadTreeNode rootNode;
    ObjectPool objectPool;
    Dictionary<int, SceneObject> allObjects;

    public SceneManager(RectD sceneBounds)
    {
        rootNode = new QuadTreeNode(sceneBounds);
        objectPool = new ObjectPool();
        allObjects = new Dictionary<int, SceneObject>();
    }

    public void AddObject(int id, string resourceName, Vector2d position)
    {
        var obj = new SceneObject(id, resourceName, position);
        allObjects.Add(id, obj);
        rootNode.Insert(obj);
    }

    public void Update(Camera camera)
    {
        RectD cameraView = GetCameraViewBounds(camera);
        List<SceneObject> visibleObjects = rootNode.Retrieve(cameraView);

        foreach (var obj in visibleObjects)
        {
            if (obj.GameObject == null)
            {
                obj.GameObject = objectPool.Get(obj.ResourceName);
            }
            obj.GameObject.SetActive(true);
            obj.GameObject.transform.position = (Vector2)obj.Position;
            // Add any additional update logic for the GameObject here
        }

        foreach (var obj in allObjects.Values)
        {
            if (obj.GameObject != null && !cameraView.Contains(obj.Position))
            {
                objectPool.Release(obj.GameObject, obj.ResourceName);
                obj.GameObject = null;
            }
        }
    }

    private RectD GetCameraViewBounds(Camera camera)
    {
        Vector3 camPos = camera.transform.position;
        double camWidth = camera.orthographicSize * camera.aspect * 2;
        double camHeight = camera.orthographicSize * 2;
        return new RectD(camPos.x - camWidth / 2, camPos.y - camHeight / 2, camWidth, camHeight);
    }
}
