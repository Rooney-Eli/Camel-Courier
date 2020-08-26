using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace CCClient
{
    class NetHandler
    {
        public Socket clientSocket;
        public byte[] buffer;

        public void GetConnection()
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9700);//IPAddress.any
            listenerSocket.Bind(ipEnd);
            listenerSocket.Listen(0);

            clientSocket = listenerSocket.Accept();
            buffer = new byte[clientSocket.ReceiveBufferSize];
        }

        public void CloseConnection()
        {
            clientSocket.Close();
        }


        public ImageData GetDimensions()
        {
            byte[] dims = new byte[12];

            clientSocket.Receive(dims);

            int imageArgbSize = BitConverter.ToInt32(dims, 0);
            int imageWidth = BitConverter.ToInt32(dims, 4);
            int imageHeight = BitConverter.ToInt32(dims, 8);

            ImageData iData = new ImageData(imageArgbSize, imageWidth, imageHeight);

            return iData;
        }


        public byte[] GetNetworkBytes(ImageData iData)
        {
            byte[] argbValues = new byte[iData.argbLength];


            int recievedPacketCounter = 0;
            int recievedPackets = 0;

            while (clientSocket.Available > 0)
            {
                if (recievedPacketCounter + buffer.Length < argbValues.Length)
                {
                    recievedPackets = clientSocket.Receive(buffer);
                    Array.Copy(buffer, 0, argbValues, recievedPacketCounter, buffer.Length);
                    recievedPacketCounter += recievedPackets;
                }
                else
                {
                    recievedPackets = clientSocket.Receive(buffer);
                    Array.Copy(buffer, 0, argbValues, recievedPacketCounter, argbValues.Length - recievedPacketCounter);
                    recievedPacketCounter += recievedPackets;
                    break;
                }
            }

            return argbValues;
        }
    }
}
