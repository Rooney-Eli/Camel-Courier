using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CCClient
{
    class Converter
    {
        public static Bitmap GetImageFromBytes(ImageData iData)
        {
            Bitmap bmp = new Bitmap(iData.width, iData.height);

            Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);

            BitmapData bmpData2 = bmp.LockBits(rect2, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData2.Scan0;

            Marshal.Copy(iData.argbValues, 0, ptr, iData.argbLength);

            bmp.UnlockBits(bmpData2);

            return bmp;
        }
    }
}
