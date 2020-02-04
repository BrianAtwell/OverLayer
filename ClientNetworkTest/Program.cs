/**
 * <summary>
 * This program is an client network to send the OverLayer an updated text parameters.
 * This sends the TextBoxPreprocess Serialized as an XML string.
 * The server then returns a status XML.
 * </summary>
 */

using OverLayerCSharp.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientNetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        // ExecuteClient() Method 
        static void ExecuteClient()
        {

            try
            {

                // Establish the remote endpoint  
                // for the socket. This example  
                // uses port 11111 on the local  
                // computer. 
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 1111);

                // Creation TCP/IP Socket using  
                // Socket Class Costructor 
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // Connect Socket to the remote  
                    // endpoint using method Connect() 
                    sender.Connect(localEndPoint);

                    // We print EndPoint information  
                    // that we are connected 
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Send number of bytes
                    TextBoxPreprocess textBox = new TextBoxPreprocess();
                    textBox.height = "50";
                    textBox.width = "100";
                    textBox.x = "0";
                    textBox.y = "0";
                    textBox.text = "Hello World!";
                    textBox.fontSize = 18;
                    textBox.fontName = "MS Sans Serif";
                    textBox.color = "0xFFFFFF";
                    string xmlString;

                    xmlString = textBox.Serialize();

                    byte[] messageToSend = Encoding.ASCII.GetBytes(xmlString);

                    // Creation of message that 
                    // we will send to Server 
                    byte[] messageSize = BitConverter.GetBytes(messageToSend.Length);

                    Console.WriteLine("Sending TextBoxPreprocess xml: {0}", xmlString);
                    Console.WriteLine("Sending TextBoxPreprocess size: {0}", messageToSend.Length);
                    int byteSent = sender.Send(messageSize);

                    int totalBytesSent = 0;
                    int bytesToSend = messageToSend.Length;

                    while (totalBytesSent < messageToSend.Length)
                    {
                        Console.Write("Sending XML");
                        totalBytesSent += sender.Send(messageToSend, Math.Min(bytesToSend, 1024) , SocketFlags.Partial);
                        bytesToSend = messageToSend.Length - totalBytesSent;
                        Console.WriteLine("Sent, total bytes so far {0} Waiting for {1}", totalBytesSent, messageToSend.Length);
                    }



                    // Data buffer 
                    byte[] messageReceived = new byte[1024];
                    byte[] msgRecvLen = new byte[4];
                    int bytesToReceive;
                    int numByte = 0;
                    string data = "";

                    Console.WriteLine("Receive 4 bytes");
                    int byteRecv = sender.Receive(msgRecvLen);
                    bytesToReceive = BitConverter.ToInt32(msgRecvLen, 0);
                    bytesToSend = bytesToReceive;

                    Console.WriteLine("Received, we should receive {0}", bytesToReceive);

                    while (numByte < bytesToReceive)
                    {
                        Console.Write("Receive Status XML");
                        numByte += sender.Receive(messageReceived, Math.Min(bytesToSend, 1024), SocketFlags.Partial);
                        bytesToSend = bytesToReceive - numByte;

                        data += Encoding.ASCII.GetString(messageReceived,
                                                   0, numByte);
                        Console.WriteLine("Received, total bytes so far {0} Waiting for {1}", numByte, bytesToReceive);
                    }

                    Console.WriteLine(data);

                    // Close Socket using  
                    // the method Close() 
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions 
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
