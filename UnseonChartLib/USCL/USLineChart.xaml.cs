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
            SectionHeaderPosition = new Point(10, 10);
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
        public void Assembly()
        {
            //add ui_section_header on canvas
            AssemblySingle(ui_canvas, ui_section_header);

            //add ui_line_bottom on canvas
            AssemblySingle(ui_canvas, ui_line_bottom);

            //add ui_title on ui_section_header
            AssemblySingle(ui_section_header, ui_title);

        }

        private void AssemblySingle(Panel parent, FrameworkElement children)
        {
            //add ui_title on ui_section_header
            if (parent != null &&
                children != null &&
                !parent.Children.Contains(children))
            {
                parent.Children.Add(children);
            }
        }

        public string ChartTitle { get; set; }
        public void ContentsUpdate()
        {
            //Changing ChartTitles Text
            if (ui_title!=null&&
                !ui_title.Text.Equals(ChartTitle))
            {
                ui_title.Text = ChartTitle;
            }
        }

        public Point SectionHeaderPosition { get; set; }

        public void PositionUpdate()
        {
            //Update ui_section_header Position
            ui_section_header.Margin = new Thickness(SectionHeaderPosition.X, SectionHeaderPosition.Y, 0, 0);        
        }
    }
}
