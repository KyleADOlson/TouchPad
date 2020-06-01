using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TouchPad
{
    /// <summary>
    /// Interaction logic for TouchWindow.xaml
    /// </summary>
    public partial class TouchWindow : Window
    {

        const string defaultProfile = "DefaultProfile.xml";

        PadLayout layout;
        PadProfile profile;

        int conrow;
        int concol;

        Button currentPreview;

        ActionHandler actionHandler;

        public TouchWindow()
        {


            actionHandler = new ActionHandler(this);

            InitializeComponent();

            LoadProfile();

            LayoutGrid(true);


        }

        void SetWindowContextMenu()
        {
            ContextMenu m = new ContextMenu();
            MenuItem addm = new MenuItem();
            addm.Header = "Add";
            addm.Click += MenuItem_Click;
            m.Items.Add(addm);


            m.Items.Add(new Separator());


            AddLayoutMenuItems(m);

            ContextMenu = m;
        }

        void AddLayoutMenuItems(ContextMenu m)
        {

            MenuItem showm = new MenuItem();
            showm.Header = "Edit Layout";
            showm.Click += Showm_click; ;
            m.Items.Add(showm);

            MenuItem switchm = new MenuItem();
            switchm.Header = "Change Layout";
            m.Opened += (sender, e) =>
            {
                SwitchOpening(switchm);
            };
            m.Items.Add(switchm);

            MenuItem newlayout = new MenuItem();
            newlayout.Header = "New Layout";
            newlayout.Click += Newlayout_click; ;
            m.Items.Add(newlayout);



        }

        private void SwitchOpening(MenuItem switchm)
        {
            switchm.Items.Clear();
            foreach (var l in profile.SortedLayouts)
            {
                MenuItem mi = new MenuItem();
                mi.Header = l.Name;
                mi.Tag = l;
                if (profile.CurrentLayout == l.ID)
                {
                    mi.IsChecked = true;
                }
                mi.Click += LayoutMenuItemClick;
                switchm.Items.Add(mi);
            }
        }

        private void LayoutMenuItemClick(object sender, RoutedEventArgs e)
        {
            var layout = (PadLayout)((MenuItem)sender).Tag;
            if (layout.ID != profile.CurrentLayout)
            {
                ChangeLayout(layout.ID);
            }
        }

        void CreatePadLayout()
        {


            layout = new PadLayout();
            layout.Width = 100;
            layout.Height = 100;
            layout.Rows = 4;
            layout.Columns = 6;

            char c = 'a';

            for ( int i=0; i<3; i++)
            {
                for (int j=0; j<2; j++)
                {
                    ButtonDescription desc = new ButtonDescription();
                    desc.X = i;
                    desc.Y = j;
                    desc.Text = "[ Control + " + c + " ]";
                    desc.Action = new PadAction() { Data = "^" + c, ActionType = PadActionType.SendKey };
                    c++;
                    layout.Buttons.Add(desc);
                }
            }

            layout.Buttons[3].Image = "C:\\Users\\kylea\\Dropbox\\Art\\Stream Deck\\Blue_NoiseANR.png";

            ButtonDescription desc2 = new ButtonDescription();
            desc2.X = 4;
            desc2.Y = 0;
            desc2.Width = 2;
            desc2.Height = 2;
            layout.Buttons.Add(desc2);
        }

        (int, int) GetLoc(double x, double y)
        {
            int col = 0;
            int row = 0;


            col = (int)(x / layout.Width);
            row = (int)(y / layout.Height);

            return (col, row);

        }



        void LayoutGrid(bool save = true)
        {
            ButtonGrid.Children.Clear();
            ButtonGrid.RowDefinitions.Clear();
            ButtonGrid.ColumnDefinitions.Clear();


            ButtonGrid.Background = new SolidColorBrush(layout.Color.ToColor());

            ButtonGrid.RowDefinitions.Count();
            for (int i=0; i<layout.Rows; i++)
            {
                if (layout.Width == null)
                {
                    ButtonGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                }
                else
                {

                    ButtonGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(layout.Height.Value) });
                }
            }
            for (int i = 0; i < layout.Columns; i++)
            {
                if (layout.Width == null)
                {
                    ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                }
                else
                {

                    ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(layout.Width.Value) });
                }
            }

            foreach (ButtonDescription desc in layout.Buttons)
            {
                var b = LayoutButton(desc);

                if (b != null)
                {
                    ButtonGrid.Children.Add(b);
                }
            }

            if (save)
            {
                SaveProfile();
            }

            ButtonGrid.Height = layout.Height.Value * ((double)layout.Rows);
            ButtonGrid.Width = layout.Width.Value * ((double)layout.Columns);

            Title = layout.Name ?? "Touch Window";

            SetWindowContextMenu();
        }


        private Button LayoutButton(ButtonDescription desc)
        {

            Button b = null;

            if (desc.X >= 0 && desc.Y >= 0 && desc.Width > 0 && desc.Height > 0)
            {
                b = new Button();

                b.SetValue(Grid.RowProperty, desc.Y);
                b.SetValue(Grid.ColumnProperty, desc.X);
                b.SetValue(Grid.ColumnSpanProperty, desc.Width);
                b.SetValue(Grid.RowSpanProperty, desc.Height);
                b.Tag = desc;
                b.MouseLeftButtonDown += ButtonMouseLeftDown;
                b.MouseLeftButtonUp += ButtonMouseLeftUp;
                b.Click += ButtonClicked;
                b.FontSize = desc.FontSize;
                b.Foreground = new SolidColorBrush(desc.FontColor.ToColor());
                b.FontFamily = new FontFamily(desc.FontFamily);
                if (desc.Image != null && desc.Image.Length > 0)
                {
                    if (File.Exists(desc.Image))
                    {

                        BitmapImage img = new BitmapImage(new Uri(desc.Image));
                        b.Background = new ImageBrush(img);
                    }
                }
                
                b.Content = desc.Text;
                

                b.DataContext = desc;
                b.ToolTip = desc.ToString();

                ContextMenu m = new ContextMenu();

                MenuItem deletem = new MenuItem();

                var editm = new MenuItem() { Header = "Edit" };
                editm.Click += (sender, e) =>
                {

                    ShowEditDialog(b, (ButtonDescription)b.DataContext);

                };
                m.Items.Add(editm);

                var dupm = new MenuItem() { Header = "Duplicate" };
                dupm.Click += Duplicatem_click;
                m.Items.Add(dupm);

                m.Items.Add(new Separator());
                    

                var delm = new MenuItem() { Header = "Delete" };
                delm.Click += Delm_Click;
                m.Items.Add(delm);

                m.Items.Add(new Separator());


                AddLayoutMenuItems(m);



                b.ContextMenu = m;

                this.ContextMenuOpening += MainWindow_ContextMenuOpening;

                b.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }

            return b;

        }

        

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            ButtonDescription desc = (ButtonDescription)((Button)sender).Tag;

            switch (desc.Action.ActionType)
            {
                case PadActionType.KeyPress:
                case PadActionType.KeySimulator:
                    actionHandler.Click(desc.Action);
                    break;
                default:
                    TakeAction(desc.Action);
                    break;
            }

        }

        private void ButtonMouseLeftDown(object sender, MouseButtonEventArgs e)
        {

            ButtonDescription desc = (ButtonDescription)((Button)sender).Tag;
            switch (desc.Action.ActionType)
            {
                case PadActionType.KeyPress:
                case PadActionType.KeySimulator:
                    actionHandler.Down(desc.Action);
                    break;
            }
        }
        private void ButtonMouseLeftUp(object sender, MouseButtonEventArgs e)
        {

            ButtonDescription desc = (ButtonDescription)((Button)sender).Tag;
            switch (desc.Action.ActionType)
            {
                case PadActionType.KeyPress:
                case PadActionType.KeySimulator:
                    actionHandler.Up(desc.Action);
                    break;
            }
        }

        private void MainWindow_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var r = GetLoc(e.CursorLeft, e.CursorTop);
            conrow = r.Item2;
            concol = r.Item1;
        }


        private void Delm_Click(object sender, RoutedEventArgs e)
        {
            DeleteButton((ButtonDescription)((FrameworkElement)sender).DataContext);
        }
        

        private void Duplicatem_click(object sender, RoutedEventArgs e)
        {


            ShowDuplicateDialog((ButtonDescription)((FrameworkElement)sender).DataContext);
        }

        private void TakeAction(PadAction action)
        {
            switch (action.ActionType)
            {
                case PadActionType.SendKey:
                    SendKey(action);
                    break;
                case PadActionType.ChangeLayout:
                    ChangeLayout(action);
                    break;
                case PadActionType.RunCommand:
                    RunCommand(action);
                    break;
            }
        }

        private void SendKey(PadAction action)
        {
            System.Windows.Forms.SendKeys.SendWait(action.Data);
        }

        private void ChangeLayout(PadAction action)
        {
            ChangeLayout(action.Layout);
        }

        private void ChangeLayout(Guid layoutid)
        {
            if (layoutid != profile.CurrentLayout && layoutid != Guid.Empty)
            {
                PadLayout newlayout = profile[layoutid];
                if (newlayout != null)
                {
                    layout = newlayout;
                    profile.CurrentLayout = layoutid;
                    LayoutGrid();
                }

            }

        }

        private void RunCommand(PadAction action)
        { 
            if (action.Command != null)
            {
                Process.Start(action.Command);
            }
        }

        WinEventDelegate foregroundDelegate;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE,
                GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);

            foregroundDelegate = new WinEventDelegate(ForegroundDelegate);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, foregroundDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_MOUSEACTIVATE)
            {
                handled = true;
                return new IntPtr(MA_NOACTIVATE);
            }
            else
            {
                return IntPtr.Zero;
            }
        }
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int MA_NOACTIVATE = 0x0003;

        public void ForegroundDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Debug.WriteLine(ActiveWindowHelper.Title + " / " + ActiveWindowHelper.ClassName
                + " / " + ActiveWindowHelper.ImageName);

            SetLayoutForWindow();



        }

        private void SetLayoutForWindow()
        {
            foreach (var l in profile.AllLayouts)
            {
                if (l.WindowMatch.Matches)
                {
                    ChangeLayout(l.ID);
                    break;
                }
            }
        }

            private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
 

        }

        private void ShowAddKeyDialog()
        {
            KeyDialog dlg = InitKeyDialog(new ButtonDescription());
           
            dlg.Button.X = concol;
            dlg.Button.Y = conrow;
            if (dlg.ShowDialog() == true)
            {
                AddButton(dlg.Button);
            }
        }

        private KeyDialog InitKeyDialog(ButtonDescription desc)
        {
            KeyDialog dlg = new KeyDialog( layout, profile, desc, PreviewButton);
            dlg.Owner = this;
            return dlg;
        }

        private void ShowEditDialog(Button b, ButtonDescription desc)
        {

            KeyDialog dlg = new KeyDialog(layout, profile, new ButtonDescription(desc), PreviewButton);
            b.Visibility = Visibility.Collapsed;
            if (dlg.ShowDialog() == true)
            {
                desc.CopyFrom(dlg.Button);
                LayoutGrid();
            }
            else
            {
                PreviewButton(null);
                b.Visibility = Visibility.Visible;
            }
        }

        private void ShowDuplicateDialog(ButtonDescription desc)
        {
            

            KeyDialog dlg = InitKeyDialog(new ButtonDescription(desc) { ID = Guid.NewGuid() });
            if (dlg.ShowDialog() == true)
            {
                AddButton(dlg.Button);
            }
        }

        private void AddButton(ButtonDescription button)
        {

            layout.Buttons.Add(button);
            LayoutGrid();
        }


        private void DeleteButton(ButtonDescription desc)
        {
            layout.Buttons.Remove(desc);
            LayoutGrid();
        }

        private void Newlayout_click(object sender, RoutedEventArgs e)
        {
            ShowNewLayoutDialog();
        }


        private void Showm_click(object sender, RoutedEventArgs e)
        {
            ShowEditLayoutDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowAddKeyDialog();
        }



        private void SaveProfile()
        {
            SaveProfile(profile);

        }
        private void SaveProfile(PadProfile saveProfile, string file = defaultProfile)
        {
            XmlLoader<PadProfile>.Save(saveProfile, file, true);
        }

        private void SaveLayout(PadLayout layout, string file)
        {
            XmlLoader<PadLayout>.Save(layout, file, true);
        }


        private PadProfile LoadProfile(string name = defaultProfile, bool display = true)
        {
            PadProfile newProfile = XmlLoader<PadProfile>.Load(name, true);

            if (newProfile == null && !display)
            {
                return null;
            }
            else if (newProfile == null)
            {
                LoadLayout("PadLayout.xml");
                newProfile = new PadProfile() { Name = "Profile" };
                newProfile.AddLayout(layout);
                profile = newProfile;
                return newProfile;
            }
            else if (!display)
            {
                return newProfile;
            }
            else
            {
                
                profile = newProfile;
                layout = profile.Current;
                return newProfile;
            }

        }


        private bool LoadLayout(string name)
        {
            PadLayout newlayout = XmlLoader<PadLayout>.Load(name, true);
            if (newlayout != null)
            {
                layout = newlayout;
                return true;
            }
            else
            {
                return false;
            }

        }

        private LayoutWindow InitLayoutDialog()
        {

            LayoutWindow dlg = new LayoutWindow(profile);
            dlg.Owner = this;
            return dlg;
        }

        private void ShowNewLayoutDialog()
        {
            var dlg = InitLayoutDialog();
            PadLayout newl = new PadLayout();
            newl.Width = layout.Width;
            newl.Height = layout.Height;
            newl.Rows = layout.Rows;
            newl.Columns = layout.Columns;
            newl.Color = layout.Color;
            newl.Name = "New Profile";
            dlg.PadLayout = newl;
            var res = dlg.ShowDialog();
            if (res == true)
            {
                profile.AddLayout(dlg.PadLayout);
                ChangeLayout(dlg.PadLayout.ID);
            }

        }
        private void ShowEditLayoutDialog()
        {
            var dlg = InitLayoutDialog();
            dlg.PadLayout = layout;
            var res = dlg.ShowDialog();
            if (res == true)
            {
                layout.CopyFrom(dlg.PadLayout);
                LayoutGrid();
            }

        }

        private void PreviewButton(ButtonDescription desc)
        {
            if (currentPreview != null)
            {
                ButtonGrid.Children.Remove(currentPreview);
                currentPreview =null;

            }
            if (desc != null)
            {
                currentPreview = LayoutButton(desc);
                if (currentPreview != null)
                {
                    ButtonGrid.Children.Add(currentPreview);
                }
            }
        }
            
    }
}
