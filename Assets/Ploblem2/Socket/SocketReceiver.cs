using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using UnityEngine;

public class SocketReceiver
{
    private readonly SocketManager manager;

    public SocketReceiver(SocketManager manager)
    {
        this.manager = manager;
    }

    public void StartReceiving(NetworkStream stream)
    {
        Thread receiveThread = new Thread(() => ReceiveMessages(stream));
        receiveThread.Start();
    }

    private void ReceiveMessages(NetworkStream stream)
    {
        try
        {
            const int bufferSize = 1024; 
            byte[] buffer = new byte[bufferSize]; 

            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, 4);
                if (bytesRead != 4)
                {
                    Debug.LogError("Failed to read message length.");
                    continue;
                }

                int messageLength = BitConverter.ToInt32(buffer, 0);
                if (messageLength <= 0 || messageLength > bufferSize)
                {
                    Debug.LogError("Invalid message length or length exceeds buffer size.");
                    throw new InvalidOperationException("Invalid message length or length exceeds buffer size.");
                }

                // Read the actual message
                int totalBytesRead = 0;
                while (totalBytesRead < messageLength)
                {
                    bytesRead = stream.Read(buffer, totalBytesRead, messageLength - totalBytesRead);
                    if (bytesRead == 0)
                    {
                        Debug.LogError("Connection closed prematurely.");
                        return;
                    }

                    totalBytesRead += bytesRead;
                }

                // Convert the message bytes to a string
                string responseMessage = Encoding.ASCII.GetString(buffer, 0, messageLength);

                // Process the response message
                SocketResponse response = new SocketResponse(true, responseMessage, null);
                manager.OnMessage(response);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ReceiveMessages exception: " + ex.Message);
        }
    }

}
