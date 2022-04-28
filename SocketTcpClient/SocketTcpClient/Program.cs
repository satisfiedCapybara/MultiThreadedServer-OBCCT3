using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace SocketTcpClient
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        int aPort = 8005;
        string anAddress = "127.0.0.1";
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(anAddress), aPort);

        Socket aSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        aSocket.Connect(ipPoint);

        string aMessage;
        StringBuilder builder = new StringBuilder("");

        while (true)
        {
          Console.Write("Enter a message:");
          aMessage = Console.ReadLine();
          if (aMessage == "") continue;

          byte[] aData = Encoding.Unicode.GetBytes(aMessage);

          aSocket.Send(aData);
          aData = new byte[256];
          int aBytes = 0;

          builder = new StringBuilder("");

          do
          {
            aBytes = aSocket.Receive(aData, aData.Length, 0);
            builder.Append(Encoding.UTF8.GetString(aData, 0, aBytes));
          }
          while (aSocket.Available > 0);
          Console.WriteLine(builder.ToString());


          if (aMessage == "shutdown")
          {
            aSocket.Shutdown(SocketShutdown.Both);
            aSocket.Close();
            break;
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}
