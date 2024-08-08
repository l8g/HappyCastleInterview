using System;
using System.Net.Sockets;
using System.Text;

public static class SocketSender
{
    public static void SendMessage(NetworkStream stream, SocketRequest request, SocketManager manager)
    {
        try
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(request.Message);
            byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);
            byte[] data = new byte[lengthBytes.Length + messageBytes.Length];
            Array.Copy(lengthBytes, 0, data, 0, lengthBytes.Length);
            Array.Copy(messageBytes, 0, data, lengthBytes.Length, messageBytes.Length);

            stream.Write(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            SocketError error = new SocketError(request, ex);
            manager.OnError(error);
        }
    }
}
