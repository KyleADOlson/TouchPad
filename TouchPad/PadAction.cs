using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WindowsInputLib;
using WindowsInputLib.Native;

namespace KyleOlson.TouchPad
{
    public enum PadActionType
    {
        SendKey = 0,
        ChangeLayout = 1,
        RunCommand = 2,
        KeyPress = 3,
        KeySimulator = 4,
        MultiAction = 5,
        Delay = 6,

    }

    public class PadAction : SimpleNotifyClass
    {
        private PadActionType actionType = PadActionType.KeyPress;
        private string data;
        private string command;
        private Guid layout = Guid.Empty;
        private PadHotkey hotkey;
        private int timeMS;

        private ObservableCollection<PadAction> subactions = new ObservableCollection<PadAction>();



        public PadAction()
        {

        }

        public PadAction(PadAction old)
        {
            CopyFrom(old);
        }

        public void CopyFrom(PadAction old)
        {
            actionType = old.actionType;
            data = old.Data;
            command = old.command;
            layout = old.layout;
            hotkey = new PadHotkey(old.Hotkey);
            timeMS = old.timeMS;
        }
        
        public string Data
        {
            get => data;
            set
            {
                if (data != value)
                {
                    data = value;
                    Notify("Data");
                }
            }
        }


        public string Command
        {
            get => command;
            set
            {
                if (command != value)
                {
                    command = value;
                    Notify("Command");
                }
            }
        }
        public Guid Layout
        {
            get => layout;
            set
            {
                if (layout != value)
                {
                    layout = value;
                    Notify("Layout");
                }
            }
        }

        public PadActionType ActionType
        {
            get => actionType;
            set
            {
                if (actionType != value)
                {
                    actionType = value;
                    Notify("ActionType");
                }
            }
        }

        public PadHotkey Hotkey
        {
            get 
            {
                if (hotkey == null)
                {
                    hotkey = new PadHotkey();
                }
                return hotkey; 
            }
            set
            {
                if (hotkey != value)
                {
                    hotkey = value;
                    Notify("Hotkey");
                }
            }
        }
        public int TimeMS
        {
            get { return timeMS; }
            set
            {
                if (timeMS != value)
                {
                    timeMS = value;
                    Notify("TimeMS");
                }
            }
        }

        public ObservableCollection<PadAction> Subactions
        {
            get
            {
                return subactions;
            }
            set
            {
                if (subactions != value)
                {
                    subactions = value;
                    Notify("Subactions");
                }
            }
        }

    }
}
