using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamDeckSharp;

namespace KyleOlson.TouchPad.StreamDeck
{
    static class StreamDeckConnector
    {


        private static Dictionary<string, IUsbHidHardware> deviceInfo;

        static StreamDeckConnector()
       {
            

            deviceInfo = new Dictionary<string, IUsbHidHardware>();
            foreach (var d in new IUsbHidHardware[] { Hardware.StreamDeck, Hardware.StreamDeckMini, 
                Hardware.StreamDeckRev2, Hardware.StreamDeckXL})
            {
                deviceInfo[d.DeviceName] = d;
            }

        }

        public static  IEnumerable<StreamDeckDevice> StreamDecks
        {
            get
            {
                foreach (var sd in StreamDeckSharp.StreamDeck.EnumerateDevices())
                {
                    yield return new StreamDeckDevice(sd);

                }
            }
        }

        public static StreamDeckDevice GetStreamDeck(String serialNumber)
        {
            foreach (StreamDeckDevice d in StreamDecks)
            {
                if (d.SerialNumber == serialNumber)
                {
                    return d;
                }
            }
            return null;

        }
        
        public static IUsbHidHardware GetDeviceInfo(string name)
        {
            IUsbHidHardware hw;
            deviceInfo.TryGetValue(name, out hw);
            return hw;

        }

        public static IUsbHidHardware GetDeviceInfo(this StreamDeckDevice dev)
        {
            return GetDeviceInfo(dev.Name);

        }

    }
}
