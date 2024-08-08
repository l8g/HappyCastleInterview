using UnityEngine;
using System.Collections;

public class ExampleSocketUsage : MonoBehaviour
{
    private TcpServer server;

    void Start()
    {
        GameObject serverObject = GameObject.Find("Server");
        if (serverObject != null)
        {
            server = serverObject.GetComponent<TcpServer>();
        }

        StartCoroutine(WaitForServerAndConnect());
    }

    IEnumerator WaitForServerAndConnect()
    {
        while (server == null || !server.IsServerReady)
        {
            Debug.Log("Waiting for server to be ready...");
            yield return new WaitForSeconds(1);
        }

        Debug.Log("Server is ready. Connecting...");

        SocketManager.Instance.Connect("127.0.0.1", 12345);

        SocketRequest request = new SocketRequest("Hello, server!");

        request.OnMessage = (response) => {
            Debug.Log("Received message: " + response.ResponseMessage);
        };

        request.OnError = (error) => {
            Debug.LogError("Error: " + error.Message);
        };

        SocketManager.Instance.SendRequest(request);
    }
}
