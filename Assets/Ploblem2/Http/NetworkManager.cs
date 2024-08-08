using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    private RequestQueue requestQueue;
    private HttpWorkerThread workerThread;
    private ResponseHandler responseHandler;
    private RetryHandler retryHandler;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        requestQueue = new RequestQueue();
        responseHandler = new ResponseHandler();
        retryHandler = new RetryHandler();
        workerThread = new HttpWorkerThread(requestQueue, this);
        workerThread.Start();
    }

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                throw new Exception("NetworkManager not initialized");
            }
            return instance;
        }
    }

    public void SendRequest(HttpRequest request)
    {
        requestQueue.Enqueue(request);
    }

    public void OnResponse(HttpResponse response)
    {
        responseHandler.HandleResponse(response);
    }

    public void OnError(HttpError error)
    {
        retryHandler.Retry(error.Request);
    }
}
