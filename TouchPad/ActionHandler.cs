using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsInputLib;
using WindowsInputLib.Native;

namespace KyleOlson.TouchPad
{
    public class ActionHandler
    {
        TouchWindow parent;
        InputSimulator sim;
        public ActionHandler(TouchWindow parent)
        {
            this.parent = parent;
            sim = new InputSimulator();
        }

        public void Down(PadAction action)
        {
            switch (action.ActionType)
            {
                case PadActionType.KeySimulator:
                    sim.Keyboard.KeyDown(action.Hotkey.Key, action.Hotkey.ModKeys);
                    break;
            }
        }

        public void Click(PadAction action)
        {
            switch (action.ActionType)
            {
                case PadActionType.KeyPress:
                    sim.Keyboard.KeyPress(action.Hotkey.Key, action.Hotkey.ModKeys);
                    break;
            }
        }

        public void Up(PadAction action)
        {
            switch (action.ActionType)
            {
                case PadActionType.KeySimulator:
                    sim.Keyboard.KeyUp(action.Hotkey.Key, action.Hotkey.ModKeys);
                    break;
            }
        }

    }
}
