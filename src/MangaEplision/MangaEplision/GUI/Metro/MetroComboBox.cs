// -----------------------------------------------------------------------
// <copyright file="MetroComboBox.cs" company="">
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
    ///     <MyNamespace:MetroCircleButton/>
    ///
    /// </summary>
    public class MetroComboBox : ComboBox
    {
        static MetroComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(typeof(MetroComboBox)));
        }
        public MetroComboBox()
        {
            throw new NotImplementedException();

            this.Loaded += new RoutedEventHandler(MetroComboBox_Loaded);
            this.Unloaded += new RoutedEventHandler(MetroComboBox_Unloaded);
        }

        void MetroComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            //
        }

        void MetroComboBox_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(MetroComboBox_Loaded);
            this.Unloaded -= new RoutedEventHandler(MetroComboBox_Unloaded);
        }
       
    }
}
