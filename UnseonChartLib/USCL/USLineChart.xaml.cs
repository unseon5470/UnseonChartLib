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
        }

        private void ui_canvas_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += UpdateUI();
        }

        public string ChartTitle { get; set; }

        private TextBlock ui_title = new TextBlock();

        private EventHandler UpdateUI()
        {
            if(!this.ui_canvas.Children.Contains(ui_title))
            {
                this.ui_canvas.Children.Add(ui_title);
            }

            if(!ui_title.Text.Equals(ChartTitle))
            {
                ui_title.Text = ChartTitle;
            }



            return null;
        }
    }
}
