using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MangaEplision.Base
{
    public interface IMangaSource
    {
        Book GetBook(Manga mag, BookEntry chapter);
        Dictionary<string,string> GetAvailableManga();
        Manga GetMangaInfo(string name);
    }
}
