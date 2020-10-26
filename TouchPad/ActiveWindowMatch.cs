using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KyleOlson.TouchPad
{
    public class ActiveWindowMatch : SimpleNotifyClass
    {
        private bool active;
        private MatchDescription title = new MatchDescription();
        private MatchDescription className = new MatchDescription();
        private MatchDescription imageName = new MatchDescription();

        public ActiveWindowMatch()
        {

        }

        public ActiveWindowMatch(ActiveWindowMatch old)
        {
            CopyFrom(old);
        }

        public void CopyFrom(ActiveWindowMatch old)
        {
            Active = old.Active;
            title = new MatchDescription(old.Title);
            className = new MatchDescription(old.ClassName);
            imageName = new MatchDescription(old.ImageName);
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
        public MatchDescription Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    Notify("Title");
                }
            }
        }
        public MatchDescription ClassName
        {
            get { return className; }
            set
            {
                if (className != value)
                {
                    className = value;
                    Notify("ClassName");
                }
            }
        }
        public MatchDescription ImageName
        {
            get { return imageName; }
            set
            {
                if (imageName != value)
                {
                    imageName = value;
                    Notify("ImageName");
                }
            }
        }

        [XmlIgnore]
        public bool AnyActive
        {
            get
            {
                return active && (title.Active || className.Active || imageName.Active);
            }
        }

        [XmlIgnore]
        public bool Matches
        {
            get
            {
                if (!AnyActive)
                {
                    return false;
                }
                else
                {
                    return
                        title.Matches(ActiveWindowHelper.Title)
                        &&
                        className.Matches(ActiveWindowHelper.ClassName)
                        &&
                        imageName.Matches(ActiveWindowHelper.ImageName);
                    

                }
            }
        }

    }
}
