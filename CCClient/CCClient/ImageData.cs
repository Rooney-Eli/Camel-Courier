using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCClient
{
    class ImageData
    {
        public byte[] argbValues { get; set; }
        public int argbLength { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public ImageData(int argbLength, int width, int height)
        {
            this.argbLength = argbLength;
            this.width = width;
            this.height = height;

        }
    }
}
