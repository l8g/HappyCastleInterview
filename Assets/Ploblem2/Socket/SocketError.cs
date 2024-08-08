using System;

public class SocketError
{
    public SocketRequest Request { get; set; }
    public Exception Exception { get; set; }

    public SocketError(SocketRequest request, Exception exception)
    {
        Request = request;
        Exception = exception;
    }
}
