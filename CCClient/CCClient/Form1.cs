using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;

namespace CCClient
{
    public partial class Form1 : Form
    {
        private ImageData iData;
        private NetHandler netHand;
        private bool receiving;

        public Form1()
        {
            InitializeComponent();
        }
        public void Receive()
        {
            netHand = new NetHandler();
            netHand.GetConnection();
            
            iData = netHand.GetDimensions();
            while (receiving)
            {
                iData.argbValues = netHand.GetNetworkBytes(iData);
                Bitmap pbImage = Converter.GetImageFromBytes(iData);
                pictureBox1.Image = pbImage;
                Thread.Sleep(35);
            }
            netHand.CloseConnection();
        }

        private void receiveBtn_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(Receive);
            receiving = true;
            t.Start();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            receiving = false;
        }
    }
}
