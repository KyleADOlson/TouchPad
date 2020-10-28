using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace KyleOlson.TouchPad
{
    public class PadLayout : SimpleNotifyClass
    {
        private string name;
        private int rows;
        private int columns;
        private double? width;
        private double? height;
        private ObservableCollection<ButtonDescription> buttons = new ObservableCollection<ButtonDescription>();
        private uint color = 0xff000000;

        private Guid id = Guid.Empty;
        private bool reverse;



        private ActiveWindowMatch windowMatch = new ActiveWindowMatch();



        public PadLayout()
        {

        }

        public PadLayout(PadLayout old)
        {
            CopyFrom(old);
        }

        public void CopyFrom(PadLayout old)
        {
            Rows = old.rows;
            Columns = old.Columns;
            Width = old.Width;
            Height = old.Height;
            Name = old.name;
            buttons.Clear();
            ID = old.id;
            foreach (var b in old.Buttons)
            {
                buttons.Add(new ButtonDescription(b));
            }
            Color = old.Color;
            WindowMatch = new ActiveWindowMatch(old.WindowMatch);
        }

        public PadLayout Duplicate(PadProfile profile = null)
        {
            PadLayout copy = new PadLayout();
            if (profile == null)
            {
                copy.Name = copy.Name + " Copy";
            }
            else
            {


            }
            copy.ID = Guid.NewGuid();
            return copy;
        }

        public ObservableCollection<ButtonDescription> Buttons
        {
            get { return buttons; }
            set
            {
                if (buttons != value)
                {
                    buttons = value;
                    Notify("Buttons");
                }
            }
        }


        public uint Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    Notify("Color");
                }
            }
        }

        public int Rows
        {
            get { return rows; }
            set
            {
                if (rows != value)
                {
                    rows = value;
                    Notify("Rows");
                }
            }
        }
        public int Columns
        {
            get { return columns; }
            set
            {
                if (columns != value)
                {
                    columns = value;
                    Notify("Columns");
                }
            }
        }
        public double? Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    Notify("Width");
                }
            }
        }
        public double? Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    Notify("Height");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    Notify("Name");
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
                if(id != value)
                {
                    id = value;
                    Notify("ID");
                }

            }
        }

        public ActiveWindowMatch WindowMatch
        {
            get
            {
                return windowMatch;
            }
            set
            {
                if (windowMatch != value)
                {
                    windowMatch = value;
                    Notify("WindowMatch");
                }
            }
        }


        public bool Reverse
        {
            get { return reverse; }
            set
            {
                if (reverse != value)
                {
                    reverse = value;
                    Notify("Reverse");
                }
            }
        }

        public ButtonDescription ButtonAt(int x, int y)
        {
            return buttons.FirstOrDefault(b => b.IsPointInside(x, y));

        }


    }
}
