using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MangaEplision.Base
{
    public interface IMangaSource
    {
        Book GetBook(Manga mag, BookEntry chapter, Action<int,int> progressHandler = null);
        Dictionary<string,string> GetAvailableManga();
        Manga GetMangaInfo(string name);
        Manga GetMangaInfoByUrl(string url);
        BookEntry[] GetNewReleasesOfToday(int amount = 5);
    }
}
