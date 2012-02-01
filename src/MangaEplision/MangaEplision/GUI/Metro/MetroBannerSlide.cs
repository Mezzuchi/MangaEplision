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
using System.Windows.Media.Animation;

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
    ///     <MyNamespace:MetroBannerSlide/>
    ///
    /// </summary>
    public class MetroBannerSlide : Control
    {
        static MetroBannerSlide()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroBannerSlide), new FrameworkPropertyMetadata(typeof(MetroBannerSlide)));
        }
        public MetroBannerSlide()
        {
            this.Loaded += new RoutedEventHandler(MetroBannerSlide_Loaded);
            this.Unloaded += new RoutedEventHandler(MetroBannerSlide_Unloaded);
        }

        void MetroBannerSlide_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(MetroBannerSlide_Loaded);
            this.Unloaded -= new RoutedEventHandler(MetroBannerSlide_Unloaded);
        }

        void MetroBannerSlide_Loaded(object sender, RoutedEventArgs e)
        {
            FadeIn();
        }
        public object SlideOpacity { get { return base.Opacity; } }
        public string Header
        {
            get { return (string)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
          "Header", typeof(string), typeof(MetroBannerSlide), new PropertyMetadata("Header"));
        public object Image
        {
            get { return (object)this.GetValue(ImageProperty); }
            set { this.SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
          "Image", typeof(object), typeof(MetroBannerSlide), new PropertyMetadata(null));
        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(MetroBannerSlide));

        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        public static readonly RoutedEvent OpeningEvent = EventManager.RegisterRoutedEvent("Opening", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(MetroBannerSlide));

        public event RoutedEventHandler Opening
        {
            add { AddHandler(OpeningEvent, value); }
            remove { RemoveHandler(OpeningEvent, value); }
        }
        public void FadeIn()
        {
            /*try
            {
                
                ((Storyboard)this.Template.Resources["FadeInAnimation"]).BeginAnimation(OpacityProperty, null);
            }
            catch (Exception) { }
            finally
            {
                this.Visibility = System.Windows.Visibility.Visible;
                this.Opacity = 1.0;
            }*/
            //
        }
        public void FadeOut()
        {
            /*try
            {
                ((Storyboard)this.Template.Resources["FadeOutAnimation"]).BeginAnimation(OpacityProperty, null);
            }
            catch (Exception) { }
            finally
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
                this.Opacity = 0.0;
            }*/
            //this.Opacity = 0.0;
        }
    }
}
