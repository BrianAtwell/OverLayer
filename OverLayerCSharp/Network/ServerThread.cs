/**
 * <summary>
 * This has the server thread code that handles the clients.
 * Clients sends new text parameters to OverLayWindow.
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */

using OverLayerCSharp.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OverLayerCSharp.Network
{
    public class ServerThread
    {
        public bool IsRunning { get; set; }

        private readonly int _defaultPort = 1111;

        private int _port;

        public int Port { get { return _port; } }

        private IPAddress _ipAddress;

        public IPAddress IPAddress{ get { return _ipAddress; } }

        public delegate void UpdateTextBoxDelegate(TextBoxPreprocess data);

        public delegate void BindAddressPortDelegate(IPAddress ipAddress, int port);

        public UpdateTextBoxDelegate UpdateTextBox;

        public BindAddressPortDelegate BindAddressPort;

        private CancellationTokenSource cancelTokenSource = null;
        private CancellationToken cancelToken;

        public ServerThread(int port)
        {
            _port = port;
        }

        public ServerThread()
        {
            _port = _defaultPort;
            IsRunning = true;
        }

        public void Start()
        {
            MainThread();
        }

        public void Stop()
        {
            if(cancelToken != null)
            {
            }
        }

        public void MainThread()
        {
            // Establish the local endpoint  
            // for the socket. Dns.GetHostName 
            // returns the name of the host  
            // running the application. 
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            _ipAddress = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(_ipAddress, _port);

            if (BindAddressPort != null)
            {
                BindAddressPort(_ipAddress, _port);
            }

            // Creation TCP/IP Socket using  
            // Socket Class Costructor 
            Socket listener = new Socket(_ipAddress.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Using Bind() method we associate a 
                // network address to the Server Socket 
                // All client that will connect to this  
                // Server Socket must know this network 
                // Address 
                listener.Bind(localEndPoint);

                Debug.WriteLine("Bind to IP Address: {0} port {1}", _ipAddress, _port);

                // Using Listen() method we create  
                // the Client list that will want 
                // to connect to Server 
                listener.Listen(10);

                cancelTokenSource = new CancellationTokenSource();
                cancelToken = cancelTokenSource.Token;

                // Start cancelable task.
                Task t = Task.Run(() => {

                    while (IsRunning)
                    {
                        try
                        {
                            bool receivedSuccess = false;

                            Debug.WriteLine("Waiting connection ... ");

                            // Suspend while waiting for 
                            // incoming connection Using  
                            // Accept() method the server  
                            // will accept connection of client 
                            Socket clientSocket = listener.Accept();

                            // Data buffer 
                            byte[] bytes = new Byte[4];
                            string data = null;

                            int bytesToReceive = 0;

                            Debug.WriteLine("Waiting to Receive 4 bytes for the xml string");
                            int numByte = clientSocket.Receive(bytes);

                            bytesToReceive = BitConverter.ToInt32(bytes, 0);
                            Debug.WriteLine("Received xml size {0}", bytesToReceive);

                            byte[] xmlBytesBuffer = new Byte[1024];
                            numByte = 0;
                            int bytesToSend = bytesToReceive;

                            while (IsRunning)
                            {
                                Debug.Write("Receiving bytes of xml");
                                numByte += clientSocket.Receive(xmlBytesBuffer, Math.Min(bytesToSend, 1024), SocketFlags.Partial);
                                bytesToSend = bytesToReceive - numByte;
                                data += Encoding.ASCII.GetString(xmlBytesBuffer,
                                                           0, numByte);
                                Debug.WriteLine(" Received data, received {0} bytes so far waiting for {1} bytes", numByte, bytesToReceive);

                                if (numByte >= bytesToReceive)
                                    break;
                            }

                            Console.WriteLine("Text received -> {0} ", data);
                            string messageString = "";
                            byte[] message;

                            if (data.Length == bytesToReceive)
                            {
                                receivedSuccess = true;
                                messageString = "<xml><status>Ok</status></xml>";
                                message = Encoding.ASCII.GetBytes(messageString);
                            }
                            else
                            {
                                receivedSuccess = false;
                                messageString = "<xml><status>Error</status></xml>";
                                message = Encoding.ASCII.GetBytes(messageString);
                            }

                            // Send a message to Client  
                            // using Send() method 

                            Debug.WriteLine("Status XML {0}", messageString);
                            clientSocket.Send(BitConverter.GetBytes(message.Length));

                            int totalBytesSent = 0;
                            numByte = 0;
                            bytesToSend = message.Length;

                            while (IsRunning)
                            {
                                Debug.Write("Receiving bytes of xml");
                                numByte += clientSocket.Send(message, Math.Min(bytesToSend, 1024), SocketFlags.Partial);
                                bytesToSend = message.Length - numByte;

                                Console.WriteLine("Sent, total bytes so far {0} Waiting for {1}", totalBytesSent, message.Length);

                                if (numByte >= message.Length)
                                    break;
                            }

                            // Close client Socket using the 
                            // Close() method. After closing, 
                            // we can use the closed Socket  
                            // for a new Client Connection 
                            clientSocket.Shutdown(SocketShutdown.Both);
                            clientSocket.Close();

                            TextBoxPreprocess textBoxPre = TextBoxPreprocess.Deserialize(data);

                            if (UpdateTextBox != null)
                            {
                                UpdateTextBox(textBoxPre);
                            }
                        }
                        catch(Exception ex)
                        {
                            Debug.WriteLine("Major exception {0}", ex);
                        }
                    }
                }, cancelToken);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Shutdown()
        {
            IsRunning = false;
            cancelTokenSource.Cancel();
        }
    }
}
