using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rfid.Helpers
{
    public static class ImageLoaderHelper
    {
        public static BitmapImage GetImageFromFolder(string filePath)
        {
            var ms = new MemoryStream();
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ms.SetLength(stream.Length);
            stream.Read(ms.GetBuffer(), 0, (int)stream.Length);
            ms.Flush();
            stream.Close();
            var src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = ms;
            src.EndInit();
            return src;
        }
    }
}

