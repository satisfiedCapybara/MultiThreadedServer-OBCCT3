using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace SocketTcpServer
{
  class ClientHandlerThread
  {
    private Thread myThread;
    public ClientHandlerThread(Socket theListenSocket)
    {
      myThread = new Thread(this.Handle);
      myThread.Start(theListenSocket);
    }

    void Handle(object theListenSocket)
    {
      Socket listenSocket = theListenSocket as Socket;

      StringBuilder aBuilder = new StringBuilder("");
      int aBytes = 0;
      int aNumberBytes = 0;
      byte[] aData = new byte[255];

      Socket aSocketHandler = listenSocket.Accept();

      while (true)
      {
        aBuilder = new StringBuilder("");

        do
        {
          aBytes = aSocketHandler.Receive(aData);
          aBuilder.Append(Encoding.Unicode.GetString(aData, 0, aBytes));
          aNumberBytes += aBytes;
        }
        while (aSocketHandler.Available > 0);

        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + aBuilder.ToString());
        Console.WriteLine(aNumberBytes + "bytes\n");

        byte[] anArrayBytes = Encoding.UTF8.GetBytes("Server response");

        aSocketHandler.Send(anArrayBytes);

        if (aBuilder.ToString() == "shutdown")
        {
          aSocketHandler.Shutdown(SocketShutdown.Both);
          aSocketHandler.Close();
          break;
        }
      }
    }
  }
}
