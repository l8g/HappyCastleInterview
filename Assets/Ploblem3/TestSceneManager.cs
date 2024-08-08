using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    private SceneManager sceneManager;
    private Camera mainCamera;

    void Start()
    {
        RectD sceneBounds = new RectD(-1000, -1000, 2000, 2000);
        sceneManager = new SceneManager(sceneBounds);
        mainCamera = Camera.main;

        // 添加一些测试对象
        for (int i = 0; i < 100000; i++)
        {
            double x = Random.Range(-1000f, 1000f);
            double y = Random.Range(-1000f, 1000f);
            sceneManager.AddObject(i, "TestObject", new Vector2d(x, y));
        }
    }

    void Update()
    {
        sceneManager.Update(mainCamera);
    }

    void OnDrawGizmos()
    {
        if (sceneManager != null)
        {
            // 可视化四叉树节点
            DrawQuadTreeNode(sceneManager.rootNode);
        }
    }

    void DrawQuadTreeNode(QuadTreeNode node)
    {
        if (node == null) return;

        Rect rect = node.bounds.ToRect();
        Gizmos.DrawWireCube(new Vector3(rect.x + rect.width / 2, rect.y + rect.height / 2, 0), new Vector3(rect.width, rect.height, 1));

        if (!node.IsLeaf())
        {
            foreach (var child in node.children)
            {
                DrawQuadTreeNode(child);
            }
        }
    }
}
