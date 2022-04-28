using System;
using System.Net;
using System.Net.Sockets;


namespace SocketTcpServer
{
  class Program
  {
    static void Main(string[] args)
    {
      int aPort = 8005;
      String aHost = Dns.GetHostName();
      Console.WriteLine("Host: " + aHost);

      IPAddress[] IPs;
      IPs = Dns.GetHostAddresses(aHost);
      foreach (IPAddress ip1 in IPs)
      {
        Console.WriteLine(ip1);
      }

      IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), aPort);

      Socket aListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      try
      {
        aListenSocket.Bind(ipPoint);
        aListenSocket.Listen(10);

        for (; ; )
        {
          ClientHandlerThread aMyThread = new ClientHandlerThread(aListenSocket);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}
