using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using StreamDeckSharp;


namespace KyleOlson.TouchPad.StreamDeck
{
    class StreamDeckDevice
    {
        IStreamDeckRefHandle handle;
        IStreamDeckBoard _boardHolder;

        bool connected;

        string _Name;
        string _Path;

        public delegate void KeyStateEvent(object sender, KeyStateEventArgs e);
        public delegate void ConnectionStateEvent(object sender, ConnectionStateEventArgs e);

        public event KeyStateEvent KeyStateChanged;
        public event ConnectionStateEvent ConnectionStateChanged;

        public StreamDeckDevice(IStreamDeckRefHandle handle)

        {
            this.handle = handle;
            this._Name = handle.DeviceName;
            this._Path = handle.DevicePath;

        }



        public string Name
        {
            get

            {
                IHardware foo;
                return _Name;
            }

        }

        public string Path
        {
            get

            {
                return _Path;
            }
        }

        public IUsbHidHardware DeviceInfo
        {
            get

            {
                return this.GetDeviceInfo();
                
            }
        }


        public int Width
        {
            get
            {
                return DeviceInfo?.Keys.KeyCountX ?? 0;
            }
        }

        public int Height
        {
            get
            {
                return DeviceInfo?.Keys.KeyCountY ?? 0;
            }
        }

        public int KeyWidth
        {
            get
            {
                return DeviceInfo?.Keys.KeyWidth ?? 0;
            }
        }

        public int KeyHeight
        {
            get
            {
                return DeviceInfo?.Keys.KeyHeight ?? 0;
            }
        }

        public int XSpacing
        {
            get
            {
                return DeviceInfo?.Keys.KeyDistanceX ?? 0;
            }
        }

        public int YSpacing
        {
            get
            {
                return DeviceInfo?.Keys.KeyDistanceY ?? 0;
            }
        }

        public bool Connected
        {
            get
            {
                if (_boardHolder == null)
                {
                    return false;
                }
                return connected;
            }
        }

        public string FirmwareVersion
        {
            get => Board.GetFirmwareVersion();

        }
        
        public string SerialNumber
        {
            get => Board.GetSerialNumber();

        }

        public string Description
        {

            get => "Name: " + Name + "\r\nPath: " + Path + " \r\nS/N: " + SerialNumber + "\r\nFirmware: " + FirmwareVersion;
        }


        private IStreamDeckBoard Board
        {
            get
            {
                if (_boardHolder == null)
                {
                    _boardHolder = handle.Open();
                    connected = true;
                    _boardHolder.KeyStateChanged += Board_KeyStateChanged;
                    _boardHolder.ConnectionStateChanged += Board_ConnectionStateChanged;

                }
                
                return _boardHolder;
            }

        }

        private void Board_ConnectionStateChanged(object sender, OpenMacroBoard.SDK.ConnectionEventArgs e)
        {
            ConnectionStateChanged?.Invoke(this, new ConnectionStateEventArgs() { IsConnected = e.NewConnectionState });
        }

        private void Board_KeyStateChanged(object sender, OpenMacroBoard.SDK.KeyEventArgs e)
        {
            KeyStateChanged?.Invoke(this, )
        }

        public class KeyStateEventArgs : EventArgs
        {
            public int Key { get; set; }
            public bool IsPressed { get; set; }
        }

        public class ConnectionStateEventArgs : EventArgs
        {
            public bool IsConnected { get; set; }
        }

        public void SetKeyBitmap(int key, String bitmap)
        {


        }
    }
}
