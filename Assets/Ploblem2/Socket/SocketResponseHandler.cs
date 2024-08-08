public class SocketResponseHandler
{
    public void HandleMessage(SocketResponse response)
    {
        if (response.IsSuccess)
        {
            response.Request?.OnMessage(response);
        }
        else
        {
            response.Request?.OnError(response.Error);
        }
    }
}
