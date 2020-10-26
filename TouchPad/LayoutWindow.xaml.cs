using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KyleOlson.TouchPad
{
    /// <summary>
    /// Interaction logic for LayoutWindow.xaml
    /// </summary>
    public partial class LayoutWindow : Window
    {
        PadProfile profile;
        PadLayout padLayout;
        public LayoutWindow(PadProfile profile)
        {
            this.profile = profile;
            InitializeComponent();
        }

        public PadLayout PadLayout
        {
            get
            {
                return padLayout;
            }
            set
            {
                
                padLayout = new PadLayout(value);
                padLayout.PropertyChanged += PadLayout_PropertyChanged;
                DataContext = padLayout;
                EnableOK();

                EnableMatchBoxes();
            }
        }

        private void EnableOK()
        {
            bool unique = true;
            if (padLayout.Name.Length == 0)
            {
                unique = false;
            }
            else

            {
                foreach (var l in profile.AllLayouts)
                {
                    if (l.ID != padLayout.ID)
                    {
                        if (l.Name == padLayout.Name)
                        {
                            unique = false;
                            break;
                        }
                    }
                }
            }
            OKButton.IsEnabled = unique;
        }

        private void PadLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                EnableOK();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {


            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void WindowMatchActiveCheckbox_Checked(object sender, RoutedEventArgs e)
        {

            EnableMatchBoxes();
        }

        private void TitleActiveCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            EnableMatchBoxes();
        }

        private void ClassActiveCheckbox_Checked(object sender, RoutedEventArgs e)
        {

            EnableMatchBoxes();
        }

        private void ImageNameActiveCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            EnableMatchBoxes();

        }

        private void ImageNameActiveCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableMatchBoxes();
        }

        private void TitleActiveCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableMatchBoxes();
        }

        private void ClassNameActiveCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableMatchBoxes();
        }

        void EnableMatchBoxes()
        {

            bool active = padLayout.WindowMatch.Active;
            foreach (CheckBox x in  new [] { TitleActiveCheckbox, ClassNameActiveCheckbox, ImageNameActiveCheckbox })
            {
                x.IsEnabled = active;
            }

            bool titleActive = active && padLayout.WindowMatch.Title.Active;
            bool classActive = active && padLayout.WindowMatch.ClassName.Active;
            bool imageActive = active && padLayout.WindowMatch.ImageName.Active;

            TitleMatchTextBox.IsEnabled = titleActive;
            TitleMatchRegexCheckbox.IsEnabled = titleActive;
            ClassMatchTextBox.IsEnabled = classActive;
            ClassMatchRegexCheckbox.IsEnabled = classActive;
            ImageMatchTextBox.IsEnabled = imageActive;
            ImageMatchRegexCheckbox.IsEnabled = imageActive;

        }

        private void TitleMatchRegexCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ClassMatchRegexCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ImageMatchRegexCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TitleMatchTextBox_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void ClassMatchTextBox_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void ImageMatchTextBox_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }
    }
}
