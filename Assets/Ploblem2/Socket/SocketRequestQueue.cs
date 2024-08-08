using System.Collections.Generic;
using System.Threading;

public class SocketRequestQueue
{
    private Queue<SocketRequest> queue = new Queue<SocketRequest>();
    private readonly object lockObj = new object();

    public void Enqueue(SocketRequest request)
    {
        lock (lockObj)
        {
            queue.Enqueue(request);
            Monitor.Pulse(lockObj);
        }
    }

    public SocketRequest Dequeue()
    {
        lock (lockObj)
        {
            while (queue.Count == 0)
            {
                Monitor.Wait(lockObj);
            }
            return queue.Dequeue();
        }
    }
}
