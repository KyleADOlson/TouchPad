using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace KyleOlson.TouchPad
{
    public class PadProfile : SimpleNotifyClass
    {
        string name = "Profile";
        Dictionary<Guid, PadLayout> _layouts;
        List<Guid> layoutOrder = new List<Guid>();
        Guid defaultLayout = Guid.Empty;
        Guid currentLayout = Guid.Empty;
        string streamdeckSN = null;
        bool useStreamDeck = false;

        List<LayoutPair> saveLayouts;

        private double x;
        private double y;

        public double X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                    Notify("X");
                }
            }
        }
        public double Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    Notify("Y");
                }
            }
        }

        bool saveLayoutsGet = false;


        public PadProfile()
        {


        }

        public PadProfile(PadProfile old)
        {
            CopyFrom(old);
        }

        public void CopyFrom(PadProfile old)
        {
            _layouts = new Dictionary<Guid, PadLayout>(old.layouts);
            saveLayouts = new List<LayoutPair>(old.saveLayouts);
            layoutOrder = new List<Guid>(old.LayoutOrder);
            saveLayoutsGet = old.saveLayoutsGet;
            name = old.Name;
            defaultLayout = old.defaultLayout;

        }

        public class LayoutPair
        {
            public PadLayout Layout { get; set; }
            public Guid ID { get; set; }

            public LayoutPair()
            {

            }

            public LayoutPair(LayoutPair old)
            {
                CopyFrom(old);
            }

            public void CopyFrom(LayoutPair old)
            {
                this.Layout = old.Layout;
                this.ID = old.ID;
            }
        }

        public List<LayoutPair> SaveLayouts
        {
            get
            {
                CheckLayouts();
                if (saveLayouts == null || saveLayouts.Count != layouts.Count)
                {
                    saveLayouts = new List<LayoutPair>(from x in layouts
                                                       select new LayoutPair() { ID = x.Key, Layout = x.Value });
                }
                saveLayoutsGet = true;
                return saveLayouts;
            }
            set
            {

                CheckLayouts();
                layouts.Clear();
                foreach (var x in value)
                {
                    layouts[x.ID] = x.Layout;
                }

            }
        }

        public int Count
        {
            get
            {
                return layouts.Count;
            }
        }

        private Dictionary<Guid, PadLayout> layouts
        {
            get
            {
                CheckLayouts();
                return _layouts;
            }
        }

        public void RemoveLayout(PadLayout layout)
        {
            if (layouts.ContainsKey(layout.ID))
            {
                layouts.Remove(layout.ID);

                int pos = layoutOrder.IndexOf(layout.ID);
                layoutOrder.Remove(layout.ID);

                if (layouts.Count == 0)
                {
                    currentLayout = Guid.Empty;
                    defaultLayout = Guid.Empty;
                }
                else
                {
                    if (currentLayout == layout.ID)
                    {
                        currentLayout = layoutOrder[Math.Min(layouts.Count - 1, pos)];
                    }
                    if (defaultLayout == layout.ID)
                    {
                        currentLayout = layoutOrder[Math.Min(layouts.Count - 1, pos)];
                    }
                }

            }
        }

        void CheckLayouts()
        {

            if (_layouts == null)
            {
                _layouts = new Dictionary<Guid, PadLayout>();
            }
            else if (saveLayoutsGet)
            {
                _layouts.Clear();
                foreach (var x in saveLayouts)
                {
                    _layouts[x.ID] = x.Layout;
                }
            }
            saveLayoutsGet = false;
        }

        public void AddLayout(PadLayout layout, int? index = null)
        {
            CheckLayouts();

            if (layouts.ContainsKey(layout.ID))
            {
                RemoveLayout(layout);
            }
            layouts[layout.ID] = layout;
            if (index == null)
            {
                layoutOrder.Add(layout.ID);
            }
            else
            {
                layoutOrder.Insert(index.Value, layout.ID);
            }
            FixLayoutPointers();

        }

        public void FixLayoutPointers()
        {
            if (layoutOrder.Count > 0)
            {
                if (defaultLayout == Guid.Empty)
                {
                    defaultLayout = layoutOrder[0];
                }
                if (currentLayout == Guid.Empty)
                {
                    currentLayout = layoutOrder[0];
                }
            }
        }

        [XmlIgnore]
        public PadLayout this[Guid val]
        {
            get
            {
                PadLayout returnval;
                if (!layouts.TryGetValue(val, out returnval))
                {
                    return null;
                }
                return returnval;
            }
        }

        [XmlIgnore]
        public PadLayout this[int val]
        {
            get
            {
                return layouts[layoutOrder[val]];
            }
        }

        [XmlIgnore]
        public IEnumerable<PadLayout> SortedLayouts

        {
            get
            {
                return from g in layoutOrder select layouts[g];
            }
        }

        [XmlIgnore]
        public IEnumerable<PadLayout> AllLayouts

        {
            get
            {
                return from x in layouts select x.Value;
            }
        }

        public List<Guid> LayoutOrder
        {
            get

            {
                return layoutOrder;
            }
            set
            {
                layoutOrder = value;
                Notify("LayoutOrder");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    Notify("Name");
                }
            }
        }

        public bool UseStreamDeck
        {
            get => useStreamDeck;
            set
            {
                if (useStreamDeck != value)
                {
                    useStreamDeck = value;
                    Notify("UseStreamDeck");
                }
            }
        }

        public string StreamdeckSN
        {
            get => streamdeckSN;
            set
            {
                if (streamdeckSN != value)
                {
                    streamdeckSN = value;
                    Notify("StreamdeckSN");
                }
            }
        }


        public Guid DefaultLayout
        {
            get
            {
                if (layouts.Count > 0 && defaultLayout == Guid.Empty)
                {
                    FixLayoutPointers();
                }
                return defaultLayout;
            }
            set
            {
                if (defaultLayout != value)
                {
                    defaultLayout = value;
                    Notify("DefaultLayout");
                }
            }
        }
        public Guid CurrentLayout
        {
            get
            {
                if (layouts.Count > 0 && currentLayout == Guid.Empty)
                {
                    FixLayoutPointers();
                }

                return currentLayout;
            }
            set
            {
                if (currentLayout != value)
                {
                    currentLayout = value;
                    Notify("CurrentLayout");
                }
            }
        }




        [XmlIgnore]
        public PadLayout Current
        {
            get
            {

                return layouts[CurrentLayout];
            }
        }



    }
}
