using System;
using System.Collections.Generic;

public class HttpRequest
{
    public string Url { get; set; }
    public string Method { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public string Body { get; set; }
    public int RetryCount { get; set; }
    public int MaxRetries { get; set; }
    public Action<HttpResponse> OnSuccess { get; set; }
    public Action<Exception> OnError { get; set; }

    public HttpRequest(string url, string method, Dictionary<string, string> headers = null, string body = null, int maxRetries = 3)
    {
        Url = url;
        Method = method;
        Headers = headers ?? new Dictionary<string, string>();
        Body = body;
        MaxRetries = maxRetries;
        RetryCount = 0;
    }
}
