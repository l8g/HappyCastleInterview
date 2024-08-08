using System.Collections.Generic;
using System.Threading;

public class RequestQueue
{
    private Queue<HttpRequest> queue = new Queue<HttpRequest>();
    private readonly object lockObj = new object();

    public void Enqueue(HttpRequest request)
    {
        lock (lockObj)
        {
            queue.Enqueue(request);
            Monitor.Pulse(lockObj);
        }
    }

    public HttpRequest Dequeue()
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
