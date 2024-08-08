using System;

public class SocketRetryHandler
{
    public void Retry(SocketRequest request)
    {
        if (request.RetryCount < request.MaxRetries)
        {
            request.RetryCount++;
            SocketManager.Instance.SendRequest(request);
        }
        else
        {
            request.OnError(new Exception("Max retries reached"));
        }
    }
}
