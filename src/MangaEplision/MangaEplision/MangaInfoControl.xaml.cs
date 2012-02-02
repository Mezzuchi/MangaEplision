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
using System.Windows.Media.Effects;

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

            this.DataContext = info;            
        }
    }
}
