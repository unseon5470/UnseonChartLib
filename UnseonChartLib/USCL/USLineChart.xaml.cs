using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UnseonChartLib.USCL
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class USLineChart : UserControl
    {
        public USLineChart()
        {
            InitializeComponent();
            SectionHeaderPosition = new Point(15, 15);
            ChartOuterLineBrush = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
        }

        private void ui_canvas_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += UpdateUI;
        }

        private void UpdateUI(object sender, EventArgs e)
        {
            //Assemble Objects base on Canvas
            Assembly();

            //Update Objects Contents
            ContentsUpdate();

            //Update Objects Positions
            PositionUpdate();
        }

       
        private StackPanel ui_section_header = new StackPanel();
        private TextBlock ui_title = new TextBlock();
        private Line ui_line_bottom = new Line();
        private Line ui_line_right = new Line();

        public void Assembly()
        {
            //add ui_section_header on canvas
            USCommon.AssemblySingle(ui_canvas, ui_section_header);

            //add ui_line_bottom on canvas
            USCommon.AssemblySingle(ui_canvas, ui_line_bottom);

            //add ui_line_right on canvas
            USCommon.AssemblySingle(ui_canvas, ui_line_right);

            //add ui_title on ui_section_header
            USCommon.AssemblySingle(ui_section_header, ui_title);
        }

        public string ChartTitle { get; set; }
        public Brush ChartOuterLineBrush { get; set; }
        public void ContentsUpdate()
        {
            USCommon.UpdateText(ui_title, ChartTitle, 16, FontWeight.FromOpenTypeWeight(600));

            USCommon.UpdateLine(ui_line_bottom, 
                new Point(10,ui_canvas.ActualHeight-60), 
                new Point(ui_canvas.ActualWidth-100,ui_canvas.ActualHeight-60),
                1,
                ChartOuterLineBrush);

            USCommon.UpdateLine(ui_line_right,
                new Point(ui_canvas.ActualWidth - 100, 60),
                new Point(ui_canvas.ActualWidth - 100, ui_canvas.ActualHeight - 60),
                1,
                ChartOuterLineBrush);


        }

        public Point SectionHeaderPosition { get; set; }

        public void PositionUpdate()
        {
            //Update ui_section_header Position
            ui_section_header.Margin = new Thickness(SectionHeaderPosition.X, SectionHeaderPosition.Y, 0, 0);        
        }

       

    }
}
