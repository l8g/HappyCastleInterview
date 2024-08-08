using System;

public class SocketRequest
{
    public string Message { get; set; }
    public int RetryCount { get; set; }
    public int MaxRetries { get; set; }
    public Action<SocketResponse> OnMessage { get; set; }
    public Action<Exception> OnError { get; set; }

    public SocketRequest(string message, int maxRetries = 3)
    {
        Message = message;
        MaxRetries = maxRetries;
        RetryCount = 0;
    }
}
