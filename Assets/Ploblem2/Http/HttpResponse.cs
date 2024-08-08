using System;

public class HttpResponse
{
    public bool IsSuccess { get; set; }
    public string ResponseData { get; set; }
    public HttpRequest Request { get; set; }
    public Exception Error { get; set; }

    public HttpResponse(bool isSuccess, string responseData, HttpRequest request, Exception error = null)
    {
        IsSuccess = isSuccess;
        ResponseData = responseData;
        Request = request;
        Error = error;
    }
}
