using System.Threading;

public class SocketWorkerThread
{
    private Thread thread;
    private SocketRequestQueue requestQueue;
    private SocketManager socketManager;

    public SocketWorkerThread(SocketRequestQueue queue, SocketManager manager)
    {
        requestQueue = queue;
        thread = new Thread(Run);
        socketManager = manager;
    }

    public void Start()
    {
        thread.Start();
    }

    private void Run()
    {
        while (true)
        {
            SocketRequest request = requestQueue.Dequeue();
            socketManager.SendMessageInternal(request);
        }
    }
}
