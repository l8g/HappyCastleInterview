using System;

public class RetryHandler
{
    public void Retry(HttpRequest request)
    {
        if (request.RetryCount < request.MaxRetries)
        {
            request.RetryCount++;
            NetworkManager.Instance.SendRequest(request);
        }
        else
        {
            request.OnError?.Invoke(new Exception("Max retries reached"));
        }
    }
}
