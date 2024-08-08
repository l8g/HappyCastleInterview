using System;
using System.Net.Sockets;
using System.Threading;

using UnityEngine;

public class SocketManager
{
    private static SocketManager instance;
    private TcpClient client;
    private NetworkStream stream;
    private SocketRequestQueue requestQueue;
    private SocketWorkerThread workerThread;
    private SocketResponseHandler responseHandler;
    private SocketRetryHandler retryHandler;
    private SocketReceiver receiver;

    private SocketManager()
    {
        requestQueue = new SocketRequestQueue();
        workerThread = new SocketWorkerThread(requestQueue, this);
        responseHandler = new SocketResponseHandler();
        retryHandler = new SocketRetryHandler();
        receiver = new SocketReceiver(this);
        workerThread.Start();
    }

    public static SocketManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SocketManager();
            }
            return instance;
        }
    }

    public void Connect(string host, int port)
    {
        client = new TcpClient(host, port);
        stream = client.GetStream();
        receiver.StartReceiving(stream);
    }

    public void SendRequest(SocketRequest request)
    {
        requestQueue.Enqueue(request);
    }

    public void OnMessage(SocketResponse response)
    {
        responseHandler.HandleMessage(response);
    }

    public void OnError(SocketError error)
    {
        retryHandler.Retry(error.Request);
    }

    internal void SendMessageInternal(SocketRequest request)
    {
        SocketSender.SendMessage(stream, request, this);
    }
}
