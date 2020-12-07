using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
 
namespace udpcrawler {
    class Program {
        static IPAddress remoteAddress; // host to send data
        const int remotePort = 420; // port for sending data
        const int localPort = 420; // local port to listen for incoming connections
        static string username; 
        static void Main(string[] args) {
            try {
                Console.Write("Enter your name:");
                username = Console.ReadLine();
                remoteAddress = IPAddress.Parse("235.5.5.11");
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                SendMessage();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        private static void SendMessage() {
            UdpClient sender = new UdpClient(); // create UdpClient for sending messages
            IPEndPoint endPoint = new IPEndPoint(remoteAddress, remotePort);
            try {
                while (true) {
                    string message = Console.ReadLine(); // сmessage for sending
                    message = String.Format("{0}: {1}", username, message);
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    sender.Send(data, data.Length, endPoint); // sending
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
            receiver.JoinMulticastGroup(remoteAddress, 20);
            IPEndPoint remoteIp = null;
            string localAddress = LocalIPAddress();
            try {
                while (true){
                    byte[] data = receiver.Receive(ref remoteIp); // getting data
                    if (remoteIp.Address.ToString().Equals(localAddress))
                        continue;
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            finally {
                receiver.Close();
            }
        }
        private static string LocalIPAddress() {
            string localIP = "";
            IPHostEntry  host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}
