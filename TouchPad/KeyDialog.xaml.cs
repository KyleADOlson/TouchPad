using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsInputLib;
using WindowsInputLib.Native;
using Xceed.Wpf.Toolkit;

namespace KyleOlson.TouchPad
{
    /// <summary>
    /// Interaction logic for KeyDialog.xaml
    /// </summary>
    public partial class KeyDialog : Window
    {
        private ButtonDescription button;

        public delegate void ButtonPreviewFunction(ButtonDescription desc);

        ButtonPreviewFunction preview;

        PadLayout layout;
        PadProfile profile;


        Dictionary<SimpleWeight, ComboBoxItem> fontWeightItems;
        

        public KeyDialog(PadLayout layout, PadProfile profile, ButtonDescription button, ButtonPreviewFunction preview)
        {
            this.layout = layout;
            this.profile = profile;
            this.preview = preview;

            InitializeComponent();


            SetupFontWeightBox();
            SetupFontFamilyBox();

            Button = button;

            SetupVKeys();

            ActionTypeCombo.SelectedIndex = (int)Button.Action.ActionType;
            UpdateForActionType();
        }

        void SetupVKeys()
        {
            var keys = KeyHelper.GetAllKeys();

            foreach (var key in keys)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = key.KeyText();
                item.Tag = key;
                VKeyComboBox.Items.Add(item);
            }
        }


        public ButtonDescription Button
        {
            get
            {
                return button;
            }
            set
            {
                if (button != value)
                {
                    if (button != null)
                    {
                        button.PropertyChanged -= Button_PropertyChanged;
                    }
                    button = value;
                    DataContext = button;
                    if (button != null)
                    {
                        button.PropertyChanged += Button_PropertyChanged;
                    }
                }
                ShowPreview();

                UpdateSampleButton();
                UpdateFontWeightBox();
                UpdateFontFamilyBox();
                UpdateFontColorButton();

            }

        }

        protected override void OnClosed(EventArgs e)
        {
            if (button != null)
            {
                button.PropertyChanged -= Button_PropertyChanged;
            }
            base.OnClosed(e);
        }

        private void Button_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ShowPreview();
            

            if (e.PropertyName == "Image" || e.PropertyName == "Text" 
                || e.PropertyName == "FontSize" || e.PropertyName == "FontWeight" 
                || e.PropertyName == "FontFamily" || e.PropertyName == "FontColor")
            {
                UpdateSampleButton();
            }
            else if (e.PropertyName == "X")
            {
                if (button.X < 0)
                {
                    button.X = 0;
                }
            }
            else if (e.PropertyName == "Y")
            { 
                if (button.Y < 0)
                {
                    button.Y = 0;
                }
            }
            if (e.PropertyName == "Width")
            {

                if (button.Width < 1)
                {
                    button.Width = 1;
                }
            }
            else if (e.PropertyName == "Height")
            {
                if (button.Height < 1)
                {
                    button.Height = 0;
                }
            } 
        }

        private void ShowPreview()
        {
            Preview?.Invoke(button);
        }

        public ButtonPreviewFunction Preview
        {
            get
            {
                return preview;
            }
            set
            {
                if (preview != value)
                {
                    preview = value;
                }
            }

        }




        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            ShowImageDialog();
        }

        void ShowImageDialog()
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = button.Image;
            
            if (dlg.ShowDialog(this) == true)
            {
                button.Image = dlg.FileName;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys.send?view=netcore-3.1");
        }

        Brush backBrush = null;

        private void UpdateSampleButton()
        {
            SampleGrid.Background = new SolidColorBrush(layout.Color.ToColor());

            if (backBrush == null)
            {
                backBrush = SampleButton.Background;
            }
            SampleButton.Background = backBrush;


            ButtonStyler.Style(SampleButton, button, true);
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (button.Y > 0)
            {
                button.Y--;
            }
        }

        private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (button.X > 0)
            {
                button.X--;
            }
        }

        private void MoveRightButton_Click(object sender, RoutedEventArgs e)
        {
            Button.X++;
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            Button.Y++;
        }

        private void ActionTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ActionTypeCombo.SelectedIndex;
            if (index != (int)Button.Action.ActionType)
            {

                Button.Action.ActionType = (PadActionType)index;
                UpdateForActionType();
            }
        }

        void UpdateForActionType()
        {
            KeysTextBox.Visibility = Visibility.Collapsed;
            CommandTextBox.Visibility = Visibility.Collapsed;
            LayoutComboBox.Visibility = Visibility.Collapsed;
            VKeyComboBox.Visibility = Visibility.Collapsed;
            ControlKeyPanel.Visibility = Visibility.Collapsed;
            PressButton.Visibility = Visibility.Collapsed;

            switch (Button.Action.ActionType)
            {
                case PadActionType.SendKey:
                    ActionTextLabel.Text = "Keys";
                    KeysTextBox.Visibility = Visibility.Visible;
                    break;
                case PadActionType.RunCommand:
                    ActionTextLabel.Text = "Command";
                    CommandTextBox.Visibility = Visibility.Visible;
                    break;
                case PadActionType.ChangeLayout:
                    ActionTextLabel.Text = "Layout";
                    LayoutComboBox.Visibility = Visibility.Visible;

                    SetupLayoutComboBox();


                    break;
                case PadActionType.KeyPress:
                    ActionTextLabel.Text = "Key";
                    VKeyComboBox.Visibility = Visibility.Visible;
                    ControlKeyPanel.Visibility = Visibility.Visible;
                    PressButton.Visibility = Visibility.Visible;
                    SetVKey();
                    break;
                case PadActionType.KeySimulator:
                    ActionTextLabel.Text = "Key";
                    VKeyComboBox.Visibility = Visibility.Visible;
                    ControlKeyPanel.Visibility = Visibility.Visible;
                    PressButton.Visibility = Visibility.Visible;
                    SetVKey();

                    break;


            }

            
        }

        void SetupLayoutComboBox()
        {
            LayoutComboBox.Items.Clear();
            ComboBoxItem noneitem = new ComboBoxItem();
            noneitem.Content = new Italic(new Run("<None>"));
            noneitem.Tag = Guid.Empty;
            noneitem.DataContext = Guid.Empty;
            LayoutComboBox.Items.Add(noneitem);

            ComboBoxItem selectItem = noneitem;

            foreach (PadLayout l in profile.AllLayouts)
            {
                if (l.ID != layout.ID)
                {
                    ComboBoxItem layoutitem = new ComboBoxItem();
                    layoutitem.Tag = l.ID;
                    layoutitem.DataContext = l.ID;
                    layoutitem.Content = l.Name;
                    if (l.ID == Button.Action.Layout)
                    {
                        selectItem = layoutitem;
                    }
                    LayoutComboBox.Items.Add(layoutitem);
                }
            }

            LayoutComboBox.SelectedItem = selectItem;

        }

        void SetVKey()
        {
            VirtualKeyCode key = button.Action.Hotkey.Key;
            foreach (ComboBoxItem item in VKeyComboBox.Items)
            {
                if (key == (VirtualKeyCode)item.Tag)
                {
                    VKeyComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void LayoutComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbi = (ComboBoxItem)LayoutComboBox.SelectedItem;
            Guid id = (Guid)cbi.DataContext;
            Button.Action.Layout = id;
        }

        private void VKeyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button.Action.Hotkey.Key = (VirtualKeyCode)((FrameworkElement)VKeyComboBox.SelectedItem).Tag;
        }


        private void PressButton_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
        private void PressButton_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            var sim = new InputSimulator();
            var state = sim.InputDeviceState;

            VirtualKeyCode k = VirtualKeyCode.A;
            bool found = false;
            ModifierKeys mod = ModifierKeys.None;
           
            foreach (var vk in KeyHelper.GetAllKeys())
            {
                if (state.IsKeyDown(vk))
                {
                    switch (vk)
                    {
                        case VirtualKeyCode.Alt:
                        case VirtualKeyCode.LAlt:
                        case VirtualKeyCode.RAlt:
                            mod |= ModifierKeys.Alt;
                            break;
                        case VirtualKeyCode.Control:
                        case VirtualKeyCode.LControl:
                        case VirtualKeyCode.RControl:
                            mod |= ModifierKeys.Control;
                            break;
                        case VirtualKeyCode.Shift:
                        case VirtualKeyCode.LShift:
                        case VirtualKeyCode.RShift:
                            mod |= ModifierKeys.Shift;
                            break;
                        case VirtualKeyCode.LWin:
                        case VirtualKeyCode.RWin:
                            mod |= ModifierKeys.Windows;
                            break;
                        default:
                            k = vk;
                            found = true;
                            break;
                    }
                }
            }

            if (found)
            {
                Button.Action.Hotkey.Key = k;
                Button.Action.Hotkey.ModKeys = mod;
                SetVKey();
            }
            
        }

        private void SampleButton_Click(object sender, RoutedEventArgs e)
        {
            ShowImageDialog();
        }

        private void FontSizeSpinner_Spin(object sender, Xceed.Wpf.Toolkit.SpinEventArgs e)
        {
            double fs = button.FontSize;
            double diff = e.Direction == SpinDirection.Increase ? 1.0d : -1.0d;
            fs += diff;
            if (fs < 1.0d)
            {
                fs = 1.0d;
            }
            button.FontSize = fs;
        }


        private void SetupFontWeightBox()
        {
            fontWeightItems = new Dictionary<SimpleWeight, ComboBoxItem>();

            var weights = Utils.SimpleWeights;

            foreach (var s in weights)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = Utils.WeightNames[(int)s];
                cbi.Tag = s;
                FontWeightBox.Items.Add(cbi);
                fontWeightItems[s] = cbi;
            }

        }

        ComboBoxItem segoeDefault;

        private void SetupFontFamilyBox()
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                ComboBoxItem item = new ComboBoxItem();
                item.FontFamily = fontFamily;
                item.Content = fontFamily.Source;
                item.Tag = fontFamily.Source;
                if (fontFamily.Source == "Segoe UI")
                {
                    segoeDefault = item;
                }
                FontFamilyBox.Items.Add(item);
            }


        }

        private void UpdateFontFamilyBox()
        {
            ComboBoxItem selectedItem = segoeDefault;

            foreach (ComboBoxItem item in FontFamilyBox.Items)
            {
                if (((string)item.Tag) == button.FontFamily)
                {
                    selectedItem = item;
                    
                }

            }

            FontFamilyBox.SelectedItem = selectedItem;
        }


        private void UpdateFontWeightBox()
        {
            if (fontWeightItems != null && button != null)
            {
                FontWeightBox.SelectedItem = fontWeightItems[button.FontWeight];
            }
        }

        private void FontWeightBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)FontWeightBox.SelectedItem;
            if (cbi != null)
            {
                button.FontWeight = (SimpleWeight)cbi.Tag;
            }
        }

        private void FontFamilyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)FontFamilyBox.SelectedItem;
            if (cbi != null)
            {
                button.FontFamily = ((string)cbi.Tag);
            }
        }

        private void UpdateFontColorButton()
        {
            FontColorButton.SelectedColor = button.FontColor.ToColor();
        }

        private void FontColorButton_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            button.FontColor = FontColorButton.SelectedColor.Value.ToUInt32();
            
        }

    }
}
