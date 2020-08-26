using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace CCServer
{
    public partial class Form1 : Form
    {
        public Socket ServerSocket;
        public bool hasConnection = true;
        public bool sending = false;

        public Form1()
        {
            InitializeComponent();
        }

        public void Sender()
        {
            MakeConnection();
            SendDimensions();
            while (sending)
            {
                try
                {
                    SendImage(GetBytesFromImage(GetDesktop()));
                }
                catch
                {
                    Console.WriteLine("Client Disconnected");
                    break;
                }
            }
            ServerSocket.Close();
            hasConnection = false;
        }

        public void MakeConnection()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9700);
            ServerSocket.Connect(ipEnd);
        }

        public Bitmap GetDesktop()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(screenshot);
            graphic.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);

            return screenshot;
        }

        public byte[] GetBytesFromImage(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;

            byte[] argbValues = new byte[bytes];

            Marshal.Copy(ptr, argbValues, 0, bytes);


            bmp.UnlockBits(bmpData);

            return argbValues;
        }

        public void SendDimensions()
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;

            byte[] dimensionArray = BitConverter.GetBytes(width * height * 4) //depends on argb vs rgb
                                    .Concat(BitConverter.GetBytes(width))
                                    .Concat(BitConverter.GetBytes(height))
                                    .ToArray();

            ServerSocket.Send(dimensionArray);
        }

        public void SendImage(byte[] argbValues)
        {
            int sentPacketCounter;
            int finalPacketSize = argbValues.Length % ServerSocket.SendBufferSize;

            byte[] buffer = new byte[ServerSocket.SendBufferSize];
            byte[] finalPacket = new byte[finalPacketSize];


            sentPacketCounter = 0;

            while (true)
            {
                if (sentPacketCounter + buffer.Length < argbValues.Length)
                {
                    Array.Copy(argbValues, sentPacketCounter, buffer, 0, buffer.Length);
                    sentPacketCounter += ServerSocket.Send(buffer);
                }
                else
                {
                    Array.Copy(argbValues, sentPacketCounter, finalPacket, 0, finalPacket.Length);
                    sentPacketCounter += ServerSocket.Send(finalPacket);
                    break;
                }
            }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            sending = true;
            sendBtn.BackColor = Color.Gray;
            Thread t = new Thread(Sender);
            t.Start();

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            sending = false;
            sendBtn.BackColor = Color.White;
        }
    }
}
