using KyleOlson.TouchPad.StreamDeck;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for ProfileSettingsWindow.xaml
    /// </summary>
    public partial class ProfileSettingsWindow : Window
    {
        PadProfile profile;
        public ProfileSettingsWindow(PadProfile profile)
        {
            this.profile = profile;
            InitializeComponent();
            InitSettings();
            UpdateEnabled();
        }

        public void InitSettings()
        {
            UseStreamDeckCheckBox.IsChecked = profile.UseStreamDeck;
            UseStreamDeckCheckBox.Checked += UseStreamDeckCheckBox_Checked;
            UseStreamDeckCheckBox.Unchecked += UseStreamDeckCheckBox_Unchecked;

            SetupStreamDeckCombo();
            StreamDeckComboBox.SelectionChanged += StreamDeckComboBox_SelectionChanged;
        }

        private void StreamDeckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEnabled();
        }

        bool UseStreamDeck
        {
            get
            {
                return UseStreamDeckCheckBox.IsChecked == true;
            }
        }

        String StreamdeckSN
        {
            get
            {
                if (StreamDeckComboBox.SelectedItem is ComboBoxItem cbi)
                { 
                    if (cbi.Tag is StreamDeckDevice sd)
                    {
                        return sd.SerialNumber;
                    }
                    else if (cbi.Tag is string s)
                    {
                        return s;
                    }
                }
                return null;
            }
        }

        void UpdateEnabled()
        {
            StreamDeckComboBox.IsEnabled = UseStreamDeck;

        }

        void UpdateOK()
        {
            bool enabled = true;

            if (UseStreamDeck && StreamdeckSN.IsEmptyOrNull() )
            {
                enabled = false;
            }

            OKButton.IsEnabled = enabled;


        }

        private void UseStreamDeckCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateEnabled();


        }

        private void UseStreamDeckCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateEnabled();
        }

        public void SaveSettings()
        {
            profile.UseStreamDeck = UseStreamDeck;
            profile.StreamdeckSN = StreamdeckSN;
        }

        public void SetupStreamDeckCombo()
        {
            string sdn = profile.StreamdeckSN;
            StreamDeckComboBox.Items.Clear();
            ComboBoxItem match = null;
            foreach (var sd in (from x in StreamDeckConnector.StreamDecks orderby x.Name, x.SerialNumber select x))
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = sd.Name + " / " + sd.SerialNumber;
                cbi.Tag = sd;
                StreamDeckComboBox.Items.Add(cbi);
                if (sd.SerialNumber == sdn)
                {
                    match = cbi;
                }
            }
            if (match == null && sdn != null && sdn.Length > 0)
            {
                match = new ComboBoxItem();

                match.Content = "(Unknown) / " + sdn;
                match.Tag = sdn;
                StreamDeckComboBox.Items.Insert(0, match);

            }
            if (match != null)
            {
                StreamDeckComboBox.SelectedItem = match;
            }
            else if (StreamDeckComboBox.Items.Count > 0)
            {
                StreamDeckComboBox.SelectedIndex = 0;
            }

        }
    }

}
