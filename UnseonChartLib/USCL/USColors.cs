using System;
using System.Drawing;

namespace UnseonChartLib.USCL
{
    public class USColors
    {
        private static String[] colorCodes = new String[]{ "#ff6358", "#ffd246", "#78d237", "#28b4c8", "#2d73f5", "#aa46be",
        "#1b96c6", "#ef767a", "#456990", "#49dcb1", "#eeb868" };

        public static Color GetColor(int i)
        {
            if (colorCodes.Length > i)
                return Color.FromName(colorCodes[i]);
            else
                return Color.DarkGray;
        }

    }
}
