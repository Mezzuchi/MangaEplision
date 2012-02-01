using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Net;

namespace MangaEplision.Extensions
{
    public static class ImageExt
    {
        public static void LoadUrl(this Image img,string url)
        {
            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
            bi.BeginInit();
            bi.StreamSource = HttpWebRequest.Create(url).GetResponse().GetResponseStream();
            bi.EndInit();
            img.Source = bi;
        }
    }
}
