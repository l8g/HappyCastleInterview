using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using UnityEngine;

public class TcpServer : MonoBehaviour
{
    private TcpListener listener;
    private bool isRunning;
    private Thread serverThread;
    public bool IsServerReady { get; private set; }

    public string ipAddress = "127.0.0.1";
    public int port = 12345;

    void Start()
    {
        StartServer(ipAddress, port);
    }

    void OnApplicationQuit()
    {
        StopServer();
    }

    public void StartServer(string ipAddress, int port)
    {
        listener = new TcpListener(IPAddress.Parse(ipAddress), port);
        listener.Start();
        isRunning = true;
        IsServerReady = true;
        Debug.Log("Server started.");

        serverThread = new Thread(AcceptClients);
        serverThread.Start();
    }

    public void StopServer()
    {
        isRunning = false;
        IsServerReady = false;
        listener.Stop();
        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Abort();
        }
        Debug.Log("Server stopped.");
    }

    private void AcceptClients()
    {
        while (isRunning)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                Debug.Log("Client connected.");
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
            catch (Exception ex)
            {
                Debug.LogError("AcceptClients exception: " + ex.Message);
            }
        }
    }

    private void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, 4);
                if (bytesRead != 4)
                {
                    Debug.LogError("Failed to read message length.");
                    continue;
                }

                int messageLength = BitConverter.ToInt32(buffer, 0);
                if (messageLength <= 0 || messageLength > buffer.Length)
                {
                    Debug.LogError("Invalid message length or length exceeds buffer size.");
                    continue;
                }

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

                string message = Encoding.ASCII.GetString(buffer, 0, messageLength);
                Debug.Log("Received message: " + message);

                string responseMessage = "Message received: " + message;
                byte[] responseBytes = Encoding.ASCII.GetBytes(responseMessage);
                byte[] responseLengthBytes = BitConverter.GetBytes(responseBytes.Length);
                byte[] responseData = new byte[responseLengthBytes.Length + responseBytes.Length];
                Array.Copy(responseLengthBytes, 0, responseData, 0, responseLengthBytes.Length);
                Array.Copy(responseBytes, 0, responseData, responseLengthBytes.Length, responseBytes.Length);

                stream.Write(responseData, 0, responseData.Length);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("HandleClient exception: " + ex.Message);
        }
        finally
        {
            client.Close();
            Debug.Log("Client disconnected.");
        }
    }
}
