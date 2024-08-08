using UnityEngine;

public class ExampleHttpUsage : MonoBehaviour
{
    void Start()
    {
        HttpRequest request = new HttpRequest(
            url: "https://jsonplaceholder.typicode.com/posts",
            method: "GET",
            maxRetries: 3
        );

        request.OnSuccess = (response) => {
            Debug.Log("Request succeeded: " + response.ResponseData);
        };

        request.OnError = (error) => {
            Debug.LogError("Request failed: " + error.Message);
        };

        NetworkManager.Instance.SendRequest(request);
    }
}
