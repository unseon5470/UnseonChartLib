using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UnseonChartLib.USCL
{
    public class USCommon
    {
        public static void UpdateText(TextBlock textblock, string text, double fontSize, FontWeight fontWeight)
        {
            if (text == null || textblock == null)
                return;

            //Changing Text
            if (!textblock.Text.Equals(text))
            {
                textblock.Text = text;
            }

            //Changing FontSize
            if(textblock.FontSize!=fontSize)
            {
                textblock.FontSize = fontSize;
            }

            //Changing FontWeight
            if (textblock.FontWeight != fontWeight)
            {
                textblock.FontWeight = fontWeight;
            }
        }

        public static void UpdateLine(Line line, Point startPoint, Point endPoint, double thickness, Brush brush)
        {
            if (line == null)
                return;

            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;

            line.Y2 = endPoint.Y;
            line.X2 = endPoint.X;

            line.StrokeThickness = thickness;
            line.Stroke = brush;

        }

        public static void AssemblySingle(Panel parent, FrameworkElement children)
        {
            //add ui_title on ui_section_header
            if (parent != null &&
                children != null &&
                !parent.Children.Contains(children))
            {
                parent.Children.Add(children);
            }
        }
    }
}
