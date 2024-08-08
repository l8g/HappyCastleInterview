using System;

public class SocketResponse
{
    public bool IsSuccess { get; set; }
    public string ResponseMessage { get; set; }
    public SocketRequest Request { get; set; }
    public Exception Error { get; set; }

    public SocketResponse(bool isSuccess, string responseMessage, SocketRequest request, Exception error = null)
    {
        IsSuccess = isSuccess;
        ResponseMessage = responseMessage;
        Request = request;
        Error = error;
    }
}
