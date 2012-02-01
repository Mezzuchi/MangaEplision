using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for ArrowedPopupControl.xaml
    /// </summary>
    public partial class MangaInfoControl : UserControl
    {
        public MangaInfoControl(MangaEplision.Base.Manga info)
        {
            InitializeComponent();
            textBlock1.Text = info.Description;

            label1.Content = info.MangaName;
            label2.Content = "Author(s): " + info.Author;
            label3.Content = "Start Release: " + info.StartRelease.ToString();

            this.Background = new ImageBrush(info.BookImage);
        }
    }
}
