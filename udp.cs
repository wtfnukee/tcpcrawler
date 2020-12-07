// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace tcpcrawler
{
    class Program
    {
        static string remoteAddress; // host to send data
        static int remotePort; // port for sending data
        static int localPort; // local port to listen for incoming connections
 
        static void Main() {
            try {
                Console.Write("Enter the listening port: "); // local port
                localPort = Int32.Parse(Console.ReadLine());
                Console.Write("Enter the remote address to connect: ");
                remoteAddress = Console.ReadLine(); // the address to which we connect
                Console.Write("Enter the port to connect: ");
                remotePort = Int32.Parse(Console.ReadLine()); // the port we are connecting to
                 
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start();
                SendMessage(); 
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
        }
        private static void SendMessage() {
            UdpClient sender = new UdpClient(); // create UdpClient for sending messages
            try {
                while(true) {
                    string message = Console.ReadLine(); // message for sending
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    sender.Send(data, data.Length, remoteAddress, remotePort); // sending
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            finally {
                sender.Close();
            }
        }
 
        private static void ReceiveMessage() {
            UdpClient receiver = new UdpClient(localPort); // UdpClient for receiving data
            IPEndPoint remoteIp = null; // incoming connection address
            try {
                while(true) {
                    byte[] data = receiver.Receive(ref remoteIp); // getting data
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine("Companion: {0}", message);
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            finally {
                receiver.Close();
            }
        }
    }
}