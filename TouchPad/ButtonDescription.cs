using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace KyleOlson.TouchPad
{
    public class ButtonDescription : SimpleNotifyClass
    {

        const double DefaultFontSize = 12.0d;
        const string DefaultFontFamily = "Segoe UI";


        string text; 
        PadAction action = new PadAction();
        int x;
        int y;
        int width = 1;
        int height = 1;
        string image;
        Guid id;
        double fontSize = DefaultFontSize;
        string fontFamily = DefaultFontFamily;
        uint fontColor = 0xFF000000;
        SimpleWeight weight = SimpleWeight.Normal;



        public ButtonDescription()
        {
        }

        public ButtonDescription(ButtonDescription old)
        {
            CopyFrom(old);
        }

        public void CopyFrom(ButtonDescription old)
        {
            text = old.text;
            action = new PadAction(old.action);
            x = old.x;
            y = old.y;
            width = old.width;
            height = old.height;
            image = old.image;
            id = old.ID;
            FontSize = old.FontSize;
            FontColor = old.FontColor;
            FontFamily = old.FontFamily;
            FontWeight = old.FontWeight;
        }


        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    Notify("Text");
                }
            }
        }
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image != value)
                {
                    image = value;
                    Notify("Image");
                }
            }
        }

        public PadAction Action
        {
            get => action;
            set
            {
                if (action != value)
                {
                    action = value;
                    Notify("Action");
                }
            }
        }

        public int X
        {
            get => x;
            set
            {
                if (x != value)
                {
                    x = value;
                    Notify("X");
                }
            }
        }
        public int Y
        {
            get => y;
            set
            {
                if (y != value)
                {
                    y = value;
                    Notify("Y");
                }
            }
        }

        public int Width
        {
            get => width;
            set
            {
                if (width != value)
                {
                    width = value;
                    Notify("Width");
                }
            }
        }

        public int Height
        {
            get => height;
            set
            {
                if (height != value)
                {
                    height = value;
                    Notify("Height");
                }
            }
        }

        public Guid ID
        {
            get
            {
                if (id == Guid.Empty)
                {
                    id = Guid.NewGuid();
                }
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    Notify("ID");
                }
            }
        }

        public double FontSize
        {
            get
            {
                if (fontSize <= 0)
                {
                    fontSize = DefaultFontSize;
                }
                return fontSize;
            }

            set
            {
                if (fontSize != value)
                {
                    fontSize = value;
                    Notify("FontSize");
                }
            }
        }
        public string FontFamily
        {
            get 
            {
                if (fontFamily == null || fontFamily.Length == 0)
                {
                    fontFamily = DefaultFontFamily;
                }
                return fontFamily; 
            }
            set
            {
                if (fontFamily != value)
                {
                    fontFamily = value;
                    Notify("FontFamily");
                }
            }
        }
        public uint FontColor
        {
            get 
            {
                return fontColor; 
            }
            set
            {
                if (fontColor != value)
                {
                    fontColor = value;
                    Notify("FontColor");
                }
            }
        }

        public int WeightInt
        {
            get
            {
                return weight.ToInt();
            }
            set
            {
                FontWeight = (SimpleWeight)value;
            }
        }

        [XmlIgnore]
        public SimpleWeight FontWeight
        {
            get
            {
                return weight;
            }
            set
            {
                if (weight != value)
                {
                    weight = value;
                    Notify("FontWeight");
                    Notify("WeightInt");
                }
            }
        }

        public override string ToString()
        {
            String res = "Pos[" + x + ", " + y + "] Size[" + width + ", " + height + "]";

            if (text != null && text.Length > 0)
            {
                res += " " + text;
            }

            res += " " + action.ActionType.Name();

            switch (Action.ActionType)
            {
                case PadActionType.Delay:
                    res += " " + action.TimeMS;
                    break;
                case PadActionType.ChangeLayout:
                case PadActionType.SendKey:
                case PadActionType.RunCommand:
                    res += " " + action.Data;
                    break;
                case PadActionType.KeyPress:
                case PadActionType.KeySimulator:
                    res += " " + action.Hotkey.ToString();
                    break;
            }


            return res;

        }
    }
}
