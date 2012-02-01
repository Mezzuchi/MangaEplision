// -----------------------------------------------------------------------
// <copyright file="MetroTabControl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MangaEplision.Metro
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MetroTabControl: TabControl
    {
        static MetroTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroTabControl), new FrameworkPropertyMetadata(typeof(MetroTabControl)));
        }
        public MetroTabControl(): base()
        {

        }
    }

    public class MetroTabItem : TabItem
    {
        static MetroTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroTabItem), new FrameworkPropertyMetadata(typeof(MetroTabItem)));
        }
        public MetroTabItem(): base()
        {
            this.Loaded += MetroTabItem_Loaded;
            this.Unloaded += new RoutedEventHandler(MetroTabItem_Unloaded);
        }

        void MetroTabItem_Unloaded(object sender, RoutedEventArgs e)
        {
            CloseLabel.MouseDoubleClick -= CloseLabel_MouseDoubleClick;
            this.Loaded -= MetroTabItem_Loaded;
        }

        private Label CloseLabel = null;

        void MetroTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            CloseLabel = (Label)this.Template.FindName("PART_CloseLabel", this);
            CloseLabel.MouseDoubleClick += CloseLabel_MouseDoubleClick;
        }

        void CloseLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var earg = new MetroTabItemCloseEventArgs();
            if (MetroTabItemClosing != null)
            {
                MetroTabItemClosing(this, earg);
            }

            if (!earg.Cancel)
            {
                if (MetroTabItemClose != null)
                {
                    MetroTabItemClose(this, (MetroTabItemCloseEventArgs)MetroTabItemCloseEventArgs.Empty);
                }

                MetroTabControl mtc = (MetroTabControl)base.Parent;
                mtc.Items.Remove(this);
            }
        }

        ~MetroTabItem()
        {

        }

        public bool IsClosable
        {
            get
            {
                return (bool)this.GetValue(IsClosableProperty);
            }
            set
            {
                this.SetValue(IsClosableProperty, value);
            }
        }
        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register("IsClosable", typeof(bool), typeof(MetroTabItem), new PropertyMetadata(false));
        public delegate void MetroTabItemCloseHandler(object sender, MetroTabItemCloseEventArgs e);
        public event MetroTabItemCloseHandler MetroTabItemClosing;
        public event MetroTabItemCloseHandler MetroTabItemClose;
    }

    public class MetroTabItemCloseEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}
