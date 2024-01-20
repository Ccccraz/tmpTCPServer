using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TcpASync : MonoBehaviour
{
    [Header("Server settings")]
    public string hostname;
    public int port;
    
    private TcpListener _server;
    private NetworkStream _stream;
    
    void Start()
    {
        SetServerAsync(hostname, port);
        ReceiveDataAsync();
    }

    /// <summary>
    /// Async create server and stream
    /// </summary>
    /// <param name="host"> target host ip address </param>
    /// <param name="port"> target host port </param>
    private async void SetServerAsync(string host, int port)
    {
        _server = new TcpListener(IPAddress.Parse(host), port);
        _server.Start();
        print("Server started");

        print("Connecting...");
        var handler = await _server.AcceptTcpClientAsync();
        print("Connected");

        _stream = handler.GetStream();
    }

    /// <summary>
    /// Async receive data from stream
    /// </summary>
    private async void ReceiveDataAsync()
    {
        // Waiting for stream creation
        while (_stream == null)
        {
           await Task.Delay(1000);
           print("Waiting...");
        }
        
        while (_stream != null)
        {
            var buf = new byte[1024];
            
            // waiting for data
            var result = await _stream.ReadAsync(buf, 0, buf.Length);
            print("received");
            if (result > 0)
            {
                print(Encoding.UTF8.GetString(buf)); 
            }
        }
    }
}
