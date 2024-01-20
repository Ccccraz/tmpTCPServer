using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TcpCor : MonoBehaviour
{
    [Header("Server settings")]
    public string hostname;
    public int port;
    
    private TcpListener _server;
    private NetworkStream _stream;
    
    void Start()
    {
        StartCoroutine(SetServer(hostname, port));
        StartCoroutine(ReceiveData());
    }

    /// <summary>
    /// Create server and client
    /// </summary>
    /// <param name="host"> target host ip address </param>
    /// <param name="port"> target host port </param>
    /// <returns></returns>
    private IEnumerator SetServer(string host, int port)
    {
        _server = new TcpListener(IPAddress.Parse(host), port);
        _server.Start();
        print("Server started");

        var handler = _server.AcceptTcpClientAsync();

        print("Connecting...");
        while (!handler.IsCompleted)
        {
            yield return null;
        }
        print("Connected");
    
        _stream = handler.Result.GetStream();
    }

    /// <summary>
    /// Create a coroutine to receive data
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReceiveData()
    {
        // Waiting for stream creation
        while (_stream == null)
        {
            yield return new WaitForSeconds(1);
            print("Waiting...");
        }
        
        while (_stream != null)
        {
            var buf = new byte[1024];
            //Async read data
            var readHandler = _stream.ReadAsync(buf, 0, buf.Length);
            
            //Task not completed handed back to main thread
            while (!readHandler.IsCompleted)
            {
                yield return null;
            }

            //Task completed print data to console
            print(Encoding.UTF8.GetString(buf));
        }
    }
}
