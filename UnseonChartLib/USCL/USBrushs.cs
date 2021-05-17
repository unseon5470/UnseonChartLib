using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace UnseonChartLib.USCL
{
    public class USBrushs
    {
        //Cache of Brushs. same color brush is only one.
        private static Dictionary<String, SolidColorBrush> solidBrushs = new Dictionary<String, SolidColorBrush>();

        //Define Favorit Pasteltone Colors String Array
        private static String[] colorCodes = new String[]{ "#ff6358", "#78d237", "#28b4c8", "#2d73f5", "#aa46be",
        "#1b96c6", "#ef767a", "#456990", "#49dcb1", "#eeb868" };

        //Get PastelSolidBrush is can Access ordered color number.
        //becaus of chart color set not support change.
        //color change function will active enterprise versions.
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
