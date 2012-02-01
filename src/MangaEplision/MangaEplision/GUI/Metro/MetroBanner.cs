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
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Timers;

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
    ///     <MyNamespace:MetroBanner/>
    ///
    /// </summary>
    public class MetroBanner : Control
    {
        static MetroBanner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroBanner), new FrameworkPropertyMetadata(typeof(MetroBanner)));
        }
        private Timer slideshow = new Timer();
        private Timer slideshow_prog = new Timer();
        private MetroProgressBar progbar = null;
        public MetroBanner()
        {
            this.Loaded += new RoutedEventHandler(MetroBanner_Loaded);
            this.Unloaded += new RoutedEventHandler(MetroBanner_Unloaded);
        }

        void MetroBanner_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= new RoutedEventHandler(MetroBanner_Unloaded);
            this.Loaded -= new RoutedEventHandler(MetroBanner_Loaded);

            slideshow_prog.Stop();
            slideshow_prog.Elapsed -= new ElapsedEventHandler(slideshow_prog_Elapsed);

            slideshow_prog.Dispose();


            slideshow.Stop();
            slideshow.Elapsed -= new ElapsedEventHandler(slideshow_Elapsed);

            slideshow.Dispose();

            MetroCircleButton nextbutton = this.Template.FindName("PART_NextButton", this) as MetroCircleButton;
            nextbutton.Click -= new RoutedEventHandler(NextButtonClick);
        }

        void MetroBanner_Loaded(object sender, RoutedEventArgs e)
        {
            //if (this.IsLoaded) return;

            progbar = this.Template.FindName("PART_SlideProgressBar", this) as MetroProgressBar;

            if (Slides != null && Slides.Count > 0)
                Slide = Slides[CurrentIndex];

            
            slideshow.Elapsed += new ElapsedEventHandler(slideshow_Elapsed);
            slideshow.Interval = 7000; //7 seconds
            slideshow.Start();

            slideshow_prog.Elapsed += new ElapsedEventHandler(slideshow_prog_Elapsed);
            slideshow_prog.Interval = 1000;
            slideshow_prog.Start();

            progbar.Maximum = 7;
            progbar.Value = 7;

            MetroCircleButton nextbutton = this.Template.FindName("PART_NextButton",this) as MetroCircleButton;
            nextbutton.Click += new RoutedEventHandler(NextButtonClick);
        }

        void slideshow_prog_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new EmptyDelegate(() => progbar.Value -= 1));
        }

        void slideshow_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new EmptyDelegate(delegate()
            {
                NextSlide();
                progbar.Maximum = 7;
                progbar.Value = 7;
            }));
        }
        private delegate void EmptyDelegate();
        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            slideshow.Stop();
            slideshow_prog.Stop();

            NextSlide();

            progbar.Maximum = 7;
            progbar.Value = 7;

            slideshow.Start();
            slideshow_prog.Start();
        }
        public void NextSlide()
        {
            if (Slides.Count > (CurrentIndex + 1))
            {
                Slide.FadeOut();

                CurrentIndex += 1;
                //Slide = Slides[CurrentIndex];
                if (Slide.Opacity == 0.0)
                Slide.FadeIn();
            }
            else if (Slides.Count == (CurrentIndex + 1))
            {
                //Slide.RaiseEvent(new RoutedEventArgs(MetroBannerSlide.ClosingEvent));
                Slide.FadeOut();

                CurrentIndex = 0;
                //Slide = Slides[CurrentIndex];
                //Slide.RaiseEvent(new RoutedEventArgs(MetroBannerSlide.OpeningEvent));
                if (Slide.Opacity == 0.0)
                Slide.FadeIn();
            }
        }
        public Collection<MetroBannerSlide> Slides
        {
            get { return (Collection<MetroBannerSlide>)this.GetValue(SlidesProperty); }
            set
            {
                this.SetValue(SlidesProperty, value);
                if (value != null && value.Count > 0)
                {
                    Slide = Slides[0];
                    Slide.FadeIn();
                }

            }
        }
        public static readonly DependencyProperty SlidesProperty = DependencyProperty.Register(
          "Slides", typeof(Collection<MetroBannerSlide>), typeof(MetroBanner), new PropertyMetadata(new Collection<MetroBannerSlide>()));
        public int CurrentIndex
        {
            get { return (int)this.GetValue(CurrentIndexProperty); }
            set
            {
                this.SetValue(CurrentIndexProperty, value);
                if (Slides != null && Slides.Count > 0)
                {
                    Slide = Slides[CurrentIndex];
                    if (Slide.Opacity == 0.0)
                    Slide.FadeIn();
                    //Slide.RaiseEvent(new RoutedEventArgs(MetroBannerSlide.OpeningEvent));
                }
            }
        }
        public static readonly DependencyProperty CurrentIndexProperty = DependencyProperty.Register(
          "CurrentIndex", typeof(int), typeof(MetroBanner), new PropertyMetadata(0));
        public MetroBannerSlide Slide
        {
            get { return (MetroBannerSlide)this.GetValue(SlideProperty); }
            set {
                if (Slide == value) return;
                this.SetValue(SlideProperty, value);
                if (Slide.Opacity == 0.0)
                    Slide.FadeIn();
            }
        }
        public static readonly DependencyProperty SlideProperty = DependencyProperty.Register(
          "Slide", typeof(MetroBannerSlide), typeof(MetroBanner),
          new PropertyMetadata(null));
    }
}
