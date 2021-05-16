using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace UnseonChartLib.USCL
{
    public class USBrushs
    {
        private static Dictionary<String, SolidColorBrush> solidBrushs = new Dictionary<String, SolidColorBrush>();

        private static String[] colorCodes = new String[]{ "#ff6358", "#78d237", "#28b4c8", "#2d73f5", "#aa46be",
        "#1b96c6", "#ef767a", "#456990", "#49dcb1", "#eeb868" };

        public static SolidColorBrush GetPastelSolidBrush(int num)
        {
            Color tempColor;
            if(colorCodes.Length> num)
            {
                tempColor = (Color)ColorConverter.ConvertFromString(colorCodes[num]);    
            }
            else
            {
                tempColor = Colors.Black;
            }

            String colorKey = tempColor.ToString();
            if (!solidBrushs.ContainsKey(colorKey))
            {
                solidBrushs.Add(colorKey, new SolidColorBrush(tempColor));    
            }

            return solidBrushs[colorKey];
        }

        public static SolidColorBrush GetSolidBrush(Color color)
        {
            String colorKey = color.ToString();
            if (!solidBrushs.ContainsKey(colorKey))
            {
                solidBrushs.Add(colorKey, new SolidColorBrush(color));
            }
            return solidBrushs[colorKey];
        }

    }
}
