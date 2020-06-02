using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TouchPad
{
    public enum SimpleWeight
    {
        Thin = 0,
        ExtraLight,
        UltraLight,
        Light,
        Normal,
        Regular,
        Medium,
        DemiBold,
        SemiBold,
        Bold,
        ExtraBold,
        UltraBold,
        Black,
        Heavy,
        ExtraBlack,
        UltraBlack,
    }

    public static class Utils
    {
        public static string[] WeightNames = new[]
        {
            "Thin",
            "ExtraLight",
            "UltraLight",
            "Light",
            "Normal",
            "Regular",
            "Medium",
            "DemiBold",
            "SemiBold",
            "Bold",
            "ExtraBold",
            "UltraBold",
            "Black",
            "Heavy",
            "ExtraBlack",
            "UltraBlack",
        };

        public const int DefaultWeight = 4;


        public static Color ToColor(this uint source)
        {

            byte[] bytes = BitConverter.GetBytes(source);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }

        public static int ToInt(this SimpleWeight simple)
        {
            return (int)simple;
        }

        public static int ReverseOn(this int val, int size)
        {
            int dist = size - 1;
            return dist - val;

        }


        public static uint ToUInt32(this Color color)
        {
            return BitConverter.ToUInt32(new byte[] { color.B, color.G, color.R, color.A }, 0);
        }


        public static SimpleWeight[] SimpleWeights
        {
            get
            {
                return (SimpleWeight[]) Enum.GetValues(typeof(SimpleWeight));
            }
        }


        public static FontWeight FontWeight(this SimpleWeight simple)
        {
            switch (simple)
            {
                case SimpleWeight.Thin:
                    return FontWeights.Thin;
                case SimpleWeight.ExtraLight:
                    return FontWeights.ExtraLight;
                case SimpleWeight.UltraLight:
                    return FontWeights.UltraLight;
                case SimpleWeight.Light:
                    return FontWeights.Light;
                case SimpleWeight.Normal:
                    return FontWeights.Normal;
                case SimpleWeight.Regular:
                    return FontWeights.Regular;
                case SimpleWeight.Medium:
                    return FontWeights.Medium;
                case SimpleWeight.DemiBold:
                    return FontWeights.DemiBold;
                case SimpleWeight.SemiBold:
                    return FontWeights.SemiBold;
                case SimpleWeight.Bold:
                    return FontWeights.Bold;
                case SimpleWeight.ExtraBold:
                    return FontWeights.ExtraBold;
                case SimpleWeight.UltraBold:
                    return FontWeights.UltraBold;
                case SimpleWeight.Black:
                    return FontWeights.Black;
                case SimpleWeight.Heavy:
                    return FontWeights.Heavy;
                case SimpleWeight.ExtraBlack:
                    return FontWeights.ExtraBlack;
                case SimpleWeight.UltraBlack:
                    return FontWeights.UltraBlack;
            }
            return FontWeights.Normal;
        }



    }
}
