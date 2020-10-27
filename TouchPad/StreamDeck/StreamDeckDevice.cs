using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using StreamDeckSharp;
using OpenMacroBoard.SDK;
using System.Windows.Input;
using System.Windows;

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
            var pt = KeyToPoint(e.Key);
            KeyStateChanged?.Invoke(this, new KeyStateEventArgs() {X =  pt.X, Y = pt.Y , IsPressed = e.IsDown });
        }

        public class KeyStateEventArgs : EventArgs
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsPressed { get; set; }
        }

        public class ConnectionStateEventArgs : EventArgs
        {
            public bool IsConnected { get; set; }
        }

        public int PointToKey(System.Drawing.Point point) => PointToKey(point.X, point.Y);

        public int PointToKey(int x, int y) => x + y * Width;

        System.Drawing.Point KeyToPoint(int key) => new System.Drawing.Point(key % Width, key / Width);

        public void SetKeyBitmap(int x, int y, string path)
        {
            SetKeyBitmap(PointToKey(x, y), path);
        }

        public void SetKeyBitmap(int key, string path)
        {
            byte[] data = null;
            if (path != null)
            {
                data = ImageManager.Instance.GetStreamDeckImage(path, KeyWidth, KeyHeight);
            }


            KeyBitmap kb;
            if (data == null)
            {
                kb = KeyBitmap.Black;
            }
            else
            {
                kb = new KeyBitmap(KeyWidth, KeyHeight, data);
            }

            Board.SetKeyBitmap(key,kb);
            
            

        }
    }
}
