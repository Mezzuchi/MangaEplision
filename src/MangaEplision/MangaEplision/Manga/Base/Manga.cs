using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Net;

namespace MangaEplision.Base
{
    [Serializable]
    public class Manga
    {
        public Manga()
        {
            Books = new Collection<BookEntry>();
        }
        public string BookImageUrl { get; set; }
        [NonSerialized]
        private ImageSource _imgcache = null;
        [NonSerialized]
        public object BookImageFld = null;
        public object BookImage
        {
            get
            {
                return BookImageFld;
            }
        }
        internal void FetchImage()
        {
            if (BookImageUrl != null)
            {
                if (_imgcache == null)
                {
                    try
                    {
                        System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = HttpWebRequest.Create(BookImageUrl).GetResponse().GetResponseStream();
                        bi.EndInit();
                        bi.Freeze();
                        _imgcache = bi;
                        BookImageFld = bi;
                    }
                    catch (Exception)
                    {
                        BookImageFld = null;
                    }
                }
                else
                    BookImageFld = _imgcache;
            }
        }
        public Collection<BookEntry> Books { get; set; }
        public string MangaName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int StartRelease { get; set; }

        public bool IsBookImageCached { get; set; }
    }
    [Serializable]
    public class Book
    {
        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }
        public Book(Manga m)
        {
            ParentManga = m;   
        }
        public Manga ParentManga { get; set; }
        public Collection<object> Pages { get; set; }
        public Collection<Uri> PageOnlineUrls { get; set; }
        public Collection<string> PageLocalUrls { get; set; }
        public string Filename { get; set; }
    }
    [Serializable]
    public class BookEntry
    {
        public string Url { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }
        public BookEntry(Manga m)
        {
            ParentManga = m;   
        }
        public int ID { get; set; }
        public Manga ParentManga { get; set; }
    }
}
