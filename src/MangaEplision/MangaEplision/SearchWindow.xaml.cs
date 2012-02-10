using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MangaEplision.Base;
using System;

namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : MangaEplision.Metro.MetroWindow
    {
        private delegate void EmptyDelegate();
        public SearchWindow()
        {
            InitializeComponent();
            CatalogListBox.MouseDoubleClick += new MouseButtonEventHandler(CatalogListBox_MouseDoubleClick);
        }

        void CatalogListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (NetworkUtils.IsConnectedToInternet())
            {
                Manga info = null;
                string name = CatalogListBox.SelectedItem.ToString();
                var task = Task.Factory.StartNew(() =>
                {
                    info = Global.GetMangaInfo(name);
                }).ContinueWith((tsk) =>
                {
                    if (tsk.Exception == null)
                        Dispatcher.BeginInvoke(
                            new EmptyDelegate(
                                () =>
                                {
                                    ((MainWindow)Application.Current.MainWindow).InvokeReadManga(info);
                                    this.Close();
                                }));
                    else
                        MessageBox.Show("There was an error grabbing information on that manga!" + Environment.NewLine + "Geeky error details: " + Environment.NewLine + Environment.NewLine + tsk.Exception.ToString() + Environment.NewLine + Environment.NewLine + "NOTE: You can press CTRL + C to copy the contents of this message!");
                    tsk.Dispose();
                });
            }
            else
            {
                MessageBox.Show("You are not connected to the internet! Connect and try again!");
            }
        }

        private void SearchTerm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                Search(SearchTerm.Text);
        }

        private void Search(string p)
        {

            List<string> matches = new List<string>();
            for (int i = 0; i < Global.Mangas.Length + 1; i++)
            {
                if (Global.Mangas[(i < Global.Mangas.Length) ? i : i - 1].ToLower().Contains(SearchTerm.Text.ToLower()))
                    matches.Add(Global.Mangas[(i < Global.Mangas.Length) ? i : i - 1]);
            }
            CatalogListBox.ItemsSource = matches;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Search(SearchTerm.Text);
        }
        
    }
}
