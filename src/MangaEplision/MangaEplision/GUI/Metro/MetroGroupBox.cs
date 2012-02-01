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
    ///     <MyNamespace:MetroGroupBox/>
    ///
    /// </summary>
    public class MetroGroupBox : ContentControl
    {
        static MetroGroupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroGroupBox), new FrameworkPropertyMetadata(typeof(MetroGroupBox)));
        }
        public int NotificationsCount
        {
            get { return (int)this.GetValue(NotificationsCountProperty); }
            set { this.SetValue(NotificationsCountProperty, value); }
        }
        public static readonly DependencyProperty NotificationsCountProperty = DependencyProperty.Register(
          "NotificationsCount", typeof(int), typeof(MetroGroupBox), new PropertyMetadata(0));
        public string Header
        {
            get { return (string)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
          "Header", typeof(string), typeof(MetroGroupBox), new PropertyMetadata("Header"));
    }
}
