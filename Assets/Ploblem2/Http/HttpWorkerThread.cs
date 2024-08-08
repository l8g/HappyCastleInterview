using System;
using System.Net.Http;
using System.Threading;

using UnityEngine;

public class HttpWorkerThread
{
    private Thread thread;
    private RequestQueue requestQueue;
    private NetworkManager networkManager;
    private HttpClient httpClient;

    public HttpWorkerThread(RequestQueue queue, NetworkManager manager)
    {
        requestQueue = queue;
        networkManager = manager;
        httpClient = new HttpClient();
        thread = new Thread(Run);
    }

    public void Start()
    {
        thread.Start();
    }

    private void Run()
    {
        while (true)
        {
            HttpRequest request = requestQueue.Dequeue();
            ProcessRequest(request);
        }
    }

    private void ProcessRequest(HttpRequest request)
    {
        try
        {
            HttpResponseMessage httpResponse = null;
            if (request.Method == "GET")
            {
                httpResponse = httpClient.GetAsync(request.Url).Result;
            }
            else if (request.Method == "POST")
            {
                var content = new StringContent(request.Body);
                foreach (var header in request.Headers)
                {
                    content.Headers.Add(header.Key, header.Value);
                }
                httpResponse = httpClient.PostAsync(request.Url, content).Result;
            }

            string responseData = httpResponse.Content.ReadAsStringAsync().Result;
            HttpResponse response = new HttpResponse(true, responseData, request);
            networkManager.OnResponse(response);
        }
        catch (Exception ex)
        {
            HttpError error = new HttpError(request, ex);
            networkManager.OnError(error);
        }
    }
}
