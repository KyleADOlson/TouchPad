using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KyleOlson.TouchPad
{
    public class MatchDescription : SimpleNotifyClass
    {
        private bool active;
        private string text = "";
        private bool regEx;


        public MatchDescription()
        {

        }

        public MatchDescription(MatchDescription old)
        {
            CopyFrom(old);
        }

        public void CopyFrom(MatchDescription old)
        {
            Active = old.active;
            Text = old.Text;
            RegEx = old.RegEx;
        }

        public bool Active
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    Notify("Active");
                }
            }
        }
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    Notify("Text");
                }
            }
        }
        public bool RegEx
        {
            get { return regEx; }
            set
            {
                if (regEx != value)
                {
                    regEx = value;
                    Notify("RegEx");
                }
            }
        }

        public bool Matches(string val)
        {
            if (!active)
            {
                return true;
            }
            else
            {
                if (val == null)
                {
                    return false;
                }

                if (!RegEx)
                {
                    return val.Contains(text);
                }
                else
                {
                    try
                    {
                        return Regex.Match(val, text).Success;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            
        }

    }
}
