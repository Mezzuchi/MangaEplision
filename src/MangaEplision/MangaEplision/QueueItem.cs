﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MangaEplision.Base;
using System.Windows;
using System.Windows.Threading;

namespace MangaEplision
{
    [Serializable]
    class QueueItem
    {
       public BookEntry Book { get; set; }
       public string Name { get; set; }
       public QueueStatus Status { get; set; }
       public DateTime Date { get; set; }
       public Manga Manga { get; set; }
       public QueueItem(BookEntry Book, Manga manga)
       {
           this.Book = Book;
           this.Manga = manga;
           this.Name = this.Book.Name;
           this.Status = QueueStatus.Queued;
           this.Date = DateTime.Now;
       }


       public bool Downloading { get; set; }
    }
    enum QueueStatus
    {
        Queued,
        Downloading,
        Completed,
        Failure,
    }
}
