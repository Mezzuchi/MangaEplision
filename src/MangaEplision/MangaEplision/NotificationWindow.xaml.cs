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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using System.Media;
namespace MangaEplision
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : MangaEplision.Metro.MetroWindow
    {
        private double t;
        private Storyboard aniStry;
        private DoubleAnimation heightAni;
        private Timer tm;
        private bool r = false;
        public NotificationWindow()
        {
            InitializeComponent();
            this.MouseDoubleClick += new MouseButtonEventHandler(NotificationWindow_MouseDoubleClick);
            aniStry = new Storyboard();
            heightAni = new DoubleAnimation();
            tm = new Timer();
            aniStry.Completed += new EventHandler(aniStry_Completed);
            tm.Tick += new EventHandler(tm_Tick);      
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            this.t = System.Windows.SystemParameters.WorkArea.Height - this.Height;
            this.Top = System.Windows.SystemParameters.VirtualScreenHeight;
            this.Left = System.Windows.SystemParameters.VirtualScreenWidth - this.Width;
            heightAni.To = this.t;
            heightAni.From = this.Top;
            heightAni.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            aniStry.Children.Add(heightAni);
            Storyboard.SetTarget(heightAni, this);
            Storyboard.SetTargetProperty(heightAni, new PropertyPath(Window.TopProperty));
        }

        void tm_Tick(object sender, EventArgs e)
        {
            tm.Stop();
            if (!r)
                this.Retract();
            else
                this.Close();
        }

        void aniStry_Completed(object sender, EventArgs e)
        {
            tm.Start();
            this.Topmost = true;
        }

        void NotificationWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tm.Stop();
            this.Retract();
        }
        public void Show(string Message, string Title = "Notification!", int duration = 5000)
        {
            this.Title = Title;
            tm.Interval = duration;
            this.Show();
            aniStry.Begin(this);
        }
        public void Retract()
        {
            aniStry.Children.Remove(heightAni);
            heightAni.To = System.Windows.SystemParameters.VirtualScreenHeight;
            heightAni.From = this.Top;
            heightAni.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            aniStry.Children.Add(heightAni);
            Storyboard.SetTarget(heightAni, this);
            Storyboard.SetTargetProperty(heightAni, new PropertyPath(Window.TopProperty));
            r = true;
            this.Topmost = false;
            tm.Interval = 1000;
            tm.Start();
            aniStry.Begin(this);
        }
    }
}
