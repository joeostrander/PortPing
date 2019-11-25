using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PortPing
{
    class Program
    {
        static string strHostname = "";
        static int intPort = 0;
        static int intPingCount = 0;
        static int intMaxPingCount = 0;
        static bool boolInfinite = true;


        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                ShowUsage();
                return;
            }


            if (!Int32.TryParse(args[1], out intPort))
            {
                ShowUsage();
                return;
            }


            strHostname = args[0];
            if (string.IsNullOrEmpty(strHostname))
            {
                ShowUsage();
                return;
            }


            if (intPort < 1 || intPort > 65535)
            {
                ShowUsage();
                return;
            }

            if (args.Length == 3)
            {
                if (Int32.TryParse(args[2], out intMaxPingCount))
                {
                    boolInfinite = false;
                }
            }


            while (true)
            {
                CheckPort();
                intPingCount++;
                if (!boolInfinite)
                {
                    if (intPingCount >= intMaxPingCount)
                    {
                        return;
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }

        }

        static void CheckPort()
        {
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(strHostname, intPort);
                if (sock.Connected)
                {
                    Console.WriteLine("{0}\t{1}:{2}\tConnected to port successfully.", DateTime.Now, strHostname, intPort);
                }

                sock.Close();
            }
            catch (SocketException SockEx)
            {
                if (SockEx.ErrorCode == 10061)
                {
                    Console.WriteLine("{0}\t{1}:{2}\tPort unavailable.", DateTime.Now, strHostname, intPort);
                }
                else if (SockEx.ErrorCode == 10060)
                {
                    Console.WriteLine("{0}\t{1}:{2}\tTimed out.", DateTime.Now, strHostname, intPort);
                }
                else
                {
                    Console.WriteLine("{0}\t{1}:{2}\t{3}", DateTime.Now, strHostname, intPort,(SocketError)SockEx.ErrorCode);
                }
            }


    }

        static void ShowUsage()
        {
            Console.WriteLine("Port Ping");
            Console.WriteLine("Joe Ostrander");
            Console.WriteLine("2012.04.16");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Usage:  PortPing.exe <hostname> <port>");
            Console.WriteLine("        PortPing.exe <hostname> <port> <count>");
        }


    
    }
}
