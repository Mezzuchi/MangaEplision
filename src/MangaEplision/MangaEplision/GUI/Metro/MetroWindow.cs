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

namespace MangaEplision.Metro
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AppStoreConcept"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AppStoreConcept;assembly=AppStoreConcept"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MetroWindow/>
    ///
    /// </summary>
    public class MetroWindow : Window
    {
        static MetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
        }
        public MetroWindow()
        {
            this.Loaded += new RoutedEventHandler(MetroWindow_Loaded);
            this.Unloaded += new RoutedEventHandler(MetroWindow_Unloaded);
        }

        void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private Rectangle title = null;
        private Grid closebtn = null;
        private Grid maxbtn = null;
        private Grid minbtn = null;
        void MetroWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(MetroWindow_Loaded);
            this.Unloaded -= new RoutedEventHandler(MetroWindow_Unloaded);
            title.MouseLeftButtonDown -= new MouseButtonEventHandler(title_MouseLeftButtonDown);

            closebtn.MouseLeftButtonUp -= new MouseButtonEventHandler(closebtn_MouseLeftButtonUp);
            maxbtn.MouseLeftButtonUp -= new MouseButtonEventHandler(maxbtn_MouseLeftButtonUp);
            minbtn.MouseLeftButtonUp -= new MouseButtonEventHandler(minbtn_MouseLeftButtonUp);
        }

        void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            title = this.Template.FindName("PART_Titlebar", this) as Rectangle;
            title.MouseLeftButtonDown += new MouseButtonEventHandler(title_MouseLeftButtonDown);

            closebtn = this.Template.FindName("PART_CloseBtn", this) as Grid;
            maxbtn = this.Template.FindName("PART_MaxBtn", this) as Grid;
            minbtn = this.Template.FindName("PART_MinBtn", this) as Grid;

            closebtn.MouseLeftButtonUp += new MouseButtonEventHandler(closebtn_MouseLeftButtonUp);
            maxbtn.MouseLeftButtonUp += new MouseButtonEventHandler(maxbtn_MouseLeftButtonUp);
            minbtn.MouseLeftButtonUp += new MouseButtonEventHandler(minbtn_MouseLeftButtonUp);
        }

        void minbtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            base.WindowState = System.Windows.WindowState.Minimized;
        }

        void maxbtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch(base.WindowState)
            {
                case System.Windows.WindowState.Maximized:
                    base.WindowState = System.Windows.WindowState.Normal;
                    break;
                case System.Windows.WindowState.Normal:
                    base.WindowState = System.Windows.WindowState.Maximized;
                    break;
            }
        }

        void closebtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            base.Close();
        }
    }
}
