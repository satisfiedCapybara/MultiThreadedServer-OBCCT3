using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;


namespace SocketTcpServer
{

  class myThread
  {
    Thread thread;
    public myThread(string name, Socket theListeningSocket)
    {
      thread = new Thread(this.func);
      thread.Name = name;
      thread.Start(theListeningSocket);
    }

    void func(object listeningSocket)
    {

      Socket listenSocket = listeningSocket as Socket;

      StringBuilder builder = new StringBuilder("");
      int bytes = 0;
      int kol_bytes = 0;
      byte[] data = new byte[255];

      Socket handler = listenSocket.Accept();

      while (true)
      {
        builder = new StringBuilder("");

        do
        {
          bytes = handler.Receive(data);  // получаем сообщение
          builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
          kol_bytes += bytes;
        }
        while (handler.Available > 0);

        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
        Console.WriteLine(kol_bytes + "bytes\n");
        // отправляем ответ
        string message = "message was send";

        byte[] aBytes = Encoding.UTF8.GetBytes(message);

        handler.Send(aBytes);

        if (builder.ToString() == "shutdown")
        {
          handler.Shutdown(SocketShutdown.Both);
          handler.Close();
          break;
        }
      }
    }

    class Program
    {
      static int port = 8005; // порт для приема входящих запросов
      static void Main(string[] args)
      {
        String Host = Dns.GetHostName();
        Console.WriteLine("Comp name = " + Host);
        IPAddress[] IPs;
        IPs = Dns.GetHostAddresses(Host);
        foreach (IPAddress ip1 in IPs)
          Console.WriteLine(ip1);


        //получаем адреса для запуска сокета
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

        // создаем сокет сервера
        Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
          // связываем сокет с локальной точкой, по которой будем принимать данные
          listenSocket.Bind(ipPoint);

          // начинаем прослушивание
          listenSocket.Listen(10);

          for (; ; )
          {
            myThread aMyThread = new myThread("sa", listenSocket);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }
  }
}
