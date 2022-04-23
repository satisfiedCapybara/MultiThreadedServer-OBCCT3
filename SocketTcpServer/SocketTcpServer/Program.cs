using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace SocketTcpServer
{
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

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                // готовимся  получать  сообщение
                StringBuilder builder = new StringBuilder("");
                int bytes = 0; // количество полученных байтов за 1 раз
                int kol_bytes = 0;//количество полученных байтов
                byte[] data = new byte[255]; // буфер для получаемых данных

                Socket handler = listenSocket.Accept();

                while (true)
                {
                    if (builder.ToString() == "shutdown")
                    {
                        handler = listenSocket.Accept();  // сокет для связи с     клиентом
                    }

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
                    // закрываем сокет

                    if (builder.ToString() == "shutdown")
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
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
