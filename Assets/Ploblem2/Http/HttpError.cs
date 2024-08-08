using System;

public class HttpError
{
    public HttpRequest Request { get; set; }
    public Exception Exception { get; set; }

    public HttpError(HttpRequest request, Exception exception)
    {
        Request = request;
        Exception = exception;
    }
}
