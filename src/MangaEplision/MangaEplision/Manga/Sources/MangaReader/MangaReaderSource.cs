using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using MangaEplision.Extensions;
using System.Windows.Threading;
using System.Threading.Tasks;
using MangaEplision.Base;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MangaEplision.Sources.MangaReader
{
    /// <summary>
    /// http://mangareader.net
    /// </summary>
    public class MangaReaderSource : IMangaSource
    {
        #region IMangaSource Members
        public Dictionary<string, string> mangas = null;
        public List<string> manganames = new List<string>();
        public MangaReaderSource(bool dontfetch = false)
        {
            if (dontfetch == false)
                mangas = GetAvailableManga();
            else
                return;
        }
        public MangaReaderSource(Dictionary<string,string> preloadeddict)
        {
            mangas = preloadeddict;
        }
        public Book GetBook(Manga mag, BookEntry chapter, Action<int,int> progressHandler = null)
        {
            Book b = new Book(mag);
            b.Name = chapter.Name;
            b.ReleaseDate = chapter.ReleaseDate;
            b.Pages = new System.Collections.ObjectModel.Collection<object>();
            b.PageOnlineUrls = new System.Collections.ObjectModel.Collection<Uri>();

            string url = chapter.Url; //first page.
            string firstpagehtml = GetHtml(url);


            int maxpages = 0;
            Match pgcount = Regex.Match(firstpagehtml, "<div id=\"selectpage\">.+?</div>", RegexOptions.Singleline | RegexOptions.Compiled);
            string pgcountstr = Regex.Replace(pgcount.Value, "<option.+?>.+?</option>", "");
            pgcountstr = Regex.Replace(pgcountstr, "<.+?>", "");
            pgcountstr = pgcountstr.Replace("of", "").Replace(" ", "").Replace("\n", "");
            maxpages = int.Parse(pgcountstr);


            Match firstimg = Regex.Match(firstpagehtml, "<img id=\"img\".+?>", RegexOptions.Singleline | RegexOptions.Compiled);
            string firstimgurl = Regex.Match(firstimg.Value, "src=\".+?\"", RegexOptions.Singleline | RegexOptions.Compiled).Value;
            firstimgurl = Regex.Replace(firstimgurl, "(src=\"|\")", "");
            System.Windows.Application.Current.Dispatcher.Invoke(new EmptyDelegate(delegate()
            {
                var uri = new Uri(firstimgurl);
                b.PageOnlineUrls.Add(uri);
                /*
                using (var ms = new MemoryStream())
                {
                    

                    if (uri.LocalPath.EndsWith(".png"))
                    {
                        PngBitmapEncoder png = new PngBitmapEncoder();
                        png.Frames.Add(BitmapFrame.Create(uri));
                        png.Save(ms);
                    }
                    else if (uri.LocalPath.EndsWith(".jpg"))
                    {
                        JpegBitmapEncoder jpg = new JpegBitmapEncoder();
                        jpg.Frames.Add(BitmapFrame.Create(uri));
                        jpg.Save(ms);
                    }
                    else if (uri.LocalPath.EndsWith(".bmp"))
                    {
                        BmpBitmapEncoder bmp = new BmpBitmapEncoder();
                        bmp.Frames.Add(BitmapFrame.Create(uri));
                        bmp.Save(ms);
                    }
                    ms.Flush();
                    b.Pages.Add(ms.ToArray());
                } */
            }), null);

            string url_1 = null; 
            string url_2 = null;
            if (url.EndsWith(".html"))
            {
                //old style urls.
                url_1 = url.Substring(0, url.NthIndexOf("-", 2) + 1);
                url_2 = url.Substring(url.NthIndexOf("/", 4));
            }
            else
            {
                url_1 = url + "/";
                url_2 = "";
            }
            for (int i = 2; i < maxpages + 1; i++)
            {
                if (progressHandler != null)
                    progressHandler(i, maxpages);

                string purl = url_1 + i + url_2;
                string html = GetHtml(purl);

                Match img = Regex.Match(html, "<img id=\"img\".+?>", RegexOptions.Singleline |  RegexOptions.Compiled);
                string imgurl = Regex.Match(img.Value, "src=\".+?\"", RegexOptions.Singleline | RegexOptions.Compiled).Value;
                imgurl = Regex.Replace(imgurl, "(src=\"|\")", "");
                System.Windows.Application.Current.Dispatcher.Invoke(new EmptyDelegate(delegate()
                {
                    var uri = new Uri(imgurl);
                    b.PageOnlineUrls.Add(uri);
                    /*
                    using (var ms = new MemoryStream())
                    {
                        

                        if (uri.LocalPath.EndsWith(".png"))
                        {
                            PngBitmapEncoder png = new PngBitmapEncoder();
                            png.Frames.Add(BitmapFrame.Create(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default));
                            png.Save(ms);
                        }
                        else if (uri.LocalPath.EndsWith(".jpg"))
                        {
                            JpegBitmapEncoder jpg = new JpegBitmapEncoder();
                            jpg.Frames.Add(BitmapFrame.Create(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default));
                            jpg.Save(ms);
                        }
                        else if (uri.LocalPath.EndsWith(".bmp"))
                        {
                            BmpBitmapEncoder bmp = new BmpBitmapEncoder();
                            bmp.Frames.Add(BitmapFrame.Create(uri, BitmapCreateOptions.PreservePixelFormat,BitmapCacheOption.Default));
                            bmp.Save(ms);
                        }
                        ms.Flush();
                        b.Pages.Add(ms.ToArray());
                    }
                     * */
                }), null);

                System.Threading.Thread.Sleep(50); //Decreased from 100 to 50, better download time,   // //Prevent simulating a DDOS.
            }

            return b;
        }
        private delegate void EmptyDelegate();
        private string GetHtml(string url)
        {
            HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
            string html = "";
            using (var sr = new StreamReader(hwr.GetResponse().GetResponseStream()))
            {
                html = sr.ReadToEnd();
                sr.Close();
            }
            return html;
        }
        public Dictionary<string, string> GetAvailableManga()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();


            string html = GetHtml("http://www.mangareader.net/alphabetical");
            string important = "";
            important = html.Substring(Regex.Match(html, "<div class=\"content_bloc2\">",RegexOptions.Singleline).Index);
            important = important.Substring(Regex.Match(important, "<div class=\"series_col\">", RegexOptions.Singleline).Index);
            important = important.Substring(0, Regex.Match(important, "<div id=\"adfooter\">", RegexOptions.Singleline).Index);

            foreach (Match m in Regex.Matches(important, "<li>.+?</li>"))
            {
                MatchCollection tagrm = Regex.Matches(m.Value, "<.+?>.+?</.+?>");
                string name = Regex.Replace(tagrm[0].Value, "<.+?>", "");
                string url = "http://www.mangareader.net";
                string urlbit = Regex.Match(tagrm[0].Value, "\".+?\"").Value;
                urlbit = urlbit.Replace("\"", "");
                url += urlbit;
                try
                {
                    dict.Add(name, url);
                    manganames.Add(name);
                }
                catch (Exception) { }
            }

            return dict;
        }

        public Manga GetMangaInfo(string name)
        {
            Manga m = new Manga();
            string url = mangas[name];

            HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
            string html = "";
            using (var sr = new StreamReader(hwr.GetResponse().GetResponseStream()))
            {
                html = sr.ReadToEnd();
                sr.Close();
            }

            string summuaryarea = Regex.Match(html, "<div id=\"readmangasum\">.+?</div>", RegexOptions.Singleline | RegexOptions.Compiled).Value;
            summuaryarea = Regex.Match(summuaryarea, "<p>.+?</p>", RegexOptions.Singleline | RegexOptions.Compiled).Value;
            string sum = Regex.Replace(summuaryarea, "<(/p|p)>", "");

            m.Description = sum;


            string imagearea = Regex.Match(html, "<div id=\"mangaimg\">.+?</div>", RegexOptions.Singleline | RegexOptions.Compiled).Value;
            imagearea = Regex.Match(imagearea, "src=\".+?\"", RegexOptions.Singleline | RegexOptions.Compiled).Value;
            string img = Regex.Replace(imagearea, "(src=\"|\")", "");

            m.IsBookImageCached = false;
            m.BookImageUrl = img;

            m.MangaName = name;


            string chaptersarea = Regex.Match(html, "<div id=\"chapterlist\">.+?</div>.+?</table>", RegexOptions.Singleline | RegexOptions.Compiled).Value;

            foreach (Match chp in Regex.Matches(chaptersarea, "<tr>.+?</tr>", RegexOptions.Singleline | RegexOptions.Compiled))
            {
                MatchCollection split = Regex.Matches(chp.Value, "<td>.+?</td>", RegexOptions.Singleline | RegexOptions.Compiled);
                BookEntry be = new BookEntry(m);
                string datestr = split[1].Value.Replace("</td>", "").Replace("<td>", "");
                DateTime tmp;
                be.ReleaseDate = (DateTime.TryParse(datestr, out tmp)) ? DateTime.Parse(datestr) : DateTime.Now;
                string chpurl = "";
                chpurl = Regex.Match(split[0].Value, "href=\".+?\"", RegexOptions.Singleline | RegexOptions.Compiled).Value;
                chpurl = Regex.Replace(chpurl, "(href=\"|\")", "");
                chpurl = "http://www.mangareader.net" + chpurl;
                be.Url = chpurl;

                string nm = Regex.Replace(split[0].Value.Replace("</td>", "").Replace("<td>", ""), "<.+?>", "");
                //nm = nm.Substring(3);
                be.Name = nm.Replace("\n","");
                be.ID = m.Books.Count + 1;
                m.Books.Add(be);
            }

            string authorarea = Regex.Match(html, "<td class=\"propertytitle\">Author:.+?</tr>", RegexOptions.Singleline | RegexOptions.Compiled).Value;
            MatchCollection authorsplt = Regex.Matches(authorarea, "<td>.+?</td>", RegexOptions.Singleline | RegexOptions.Compiled);
            try
            {
                m.Author = Regex.Replace(authorsplt[0].Value, "<.+?>", "");
            }
            catch (Exception) { m.Author = "Unknown"; }

            m.FetchImage();

            return m;
        }

        #endregion
    }
}
