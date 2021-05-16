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
            //It's good!
            textblock.Text = text;

            //Changing FontSize
            if (textblock.FontSize != fontSize)
                textblock.FontSize = fontSize;

            //Changing FontWeight
            if (textblock.FontWeight != fontWeight)
                textblock.FontWeight = fontWeight;
        }

        public static void UpdateLine(Line line, Point startPoint, Point endPoint, double thickness, Brush brush, bool PixelMode)
        {
            if (line == null)
                return;

            //line set startPoint
            if (line.X1 != startPoint.X)
                line.X1 = startPoint.X;
            if (line.Y1 != startPoint.Y)
                line.Y1 = startPoint.Y;

            //line set endPoint
            if (line.X2 != endPoint.X)
                line.X2 = endPoint.X;
            if (line.Y2 != endPoint.Y)
                line.Y2 = endPoint.Y;

            if (line.SnapsToDevicePixels != PixelMode)
            {
                line.SnapsToDevicePixels = PixelMode;
                line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            }


            //line set thickness
            if (line.StrokeThickness != thickness)
            {
                line.StrokeThickness = thickness;
            }


            //line set stroke brush
            if (line.Stroke != brush)
                line.Stroke = brush;
        }

        public static void AssemblySingle(Panel parent, FrameworkElement children)
        {
            if (parent == null || children == null)
                return;

            //aseembly frameworkElements
            if (!parent.Children.Contains(children))
                parent.Children.Add(children);
        }
    }
}
