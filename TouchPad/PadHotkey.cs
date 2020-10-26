using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WindowsInputLib;
using WindowsInputLib.Native;

namespace KyleOlson.TouchPad
{
    public class PadHotkey : SimpleNotifyClass
    {
        private VirtualKeyCode key;
        private bool alt;
        private bool ctrl;
        private bool shift;
        private bool win;

        public PadHotkey()
        {

        }

        public PadHotkey(PadHotkey old)
        {
            CopyFrom(old);
        }

        public void CopyFrom(PadHotkey old)
        {
            key = old.key;
            alt = old.alt;
            ctrl = old.ctrl;
            shift = old.shift;
            win = old.win;
        }

        public int IntKey
        {
            get { return (int)key; }
            set
            {
                if (value == 0)
                {
                    Key = VirtualKeyCode.D0;
                }
                else
                {
                    Key = (VirtualKeyCode)value;
                }
            }
        }

        [XmlIgnore]
        public VirtualKeyCode Key
        {
            get
            {
                if ((int)key == 0)
                {
                    key = VirtualKeyCode.D0;
                }

                return key;
            }
            set
            {
                if (key != value)
                {
                    key = value;
                    Notify("Key");
                    Notify("IntKey");
                }
            }
        }
        public bool Alt
        {
            get { return alt; }
            set
            {
                if (alt != value)
                {
                    alt = value;
                    Notify("Alt");
                }
            }
        }
        public bool Ctrl
        {
            get { return ctrl; }
            set
            {
                if (ctrl != value)
                {
                    ctrl = value;
                    Notify("Ctrl");
                }
            }
        }
        public bool Shift
        {
            get { return shift; }
            set
            {
                if (shift != value)
                {
                    shift = value;
                    Notify("Shift");
                }
            }
        }
        public bool Win
        {
            get { return win; }
            set
            {
                if (win != value)
                {
                    win = value;
                    Notify("Win");
                }
            }
        }

        [XmlIgnore]
        public ModifierKeys ModKeys
        {
            get
            {
                ModifierKeys k = ModifierKeys.None;

                if (alt)
                {
                    k |= ModifierKeys.Alt;
                }
                if (ctrl)
                {
                    k |= ModifierKeys.Control;
                }
                if (shift)
                {
                    k |= ModifierKeys.Shift;
                }
                if (win)
                {
                    k |= ModifierKeys.Windows;
                }



                return k;

            }
            set
            {
                Alt = (value & ModifierKeys.Alt) == ModifierKeys.Alt;
                Ctrl = (value & ModifierKeys.Control) == ModifierKeys.Control;
                Shift = (value & ModifierKeys.Shift) == ModifierKeys.Shift;
                Win = (value & ModifierKeys.Windows) == ModifierKeys.Windows;
            }
        }

        public override string ToString()
        {

            string mod = "";
            if (alt)
            {
               mod += "Alt+";
            }
            if (ctrl)
            {
                mod += "Ctrl+";
            }
            if (shift)
            {
                mod += "Shift+";
            }
            if (win)
            {
                mod += "Win+";
            }
            return mod + Key.ToString();
        }
    }
}
