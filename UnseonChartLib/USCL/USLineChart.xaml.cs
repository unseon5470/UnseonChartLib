using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            dataTable = new DataTable();
        }

        private Object _locker = new Object();

        public bool TryEnterLock()
        {
            return Monitor.TryEnter(_locker);
        }
        public void EnterLock()
        {
            Monitor.Enter(_locker);
        }

        public void ExitLock()
        {
            Monitor.Exit(_locker);
        }

        private void ui_canvas_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += Update; 
        }



        private StackPanel ui_section_header = new StackPanel();
        private Canvas ui_section_body = new Canvas();
        private Canvas ui_section_xLabels = new Canvas();
        private Canvas ui_section_yLabels = new Canvas();

        private StackPanel ui_section_simbols = new StackPanel();
        private TextBlock ui_title = new TextBlock();
        private Line ui_line_bottom = new Line();
        private Line ui_line_right = new Line();

        private Dictionary<String, Polyline> ui_polylines = new Dictionary<String, Polyline>();
        private List<TextBlock> ui_xLabels = new List<TextBlock>();
        private List<TextBlock> ui_yLabels = new List<TextBlock>();
        private Dictionary<String, TextBlock> ui_simbols = new Dictionary<String, TextBlock>();
        private Dictionary<String, List<TextBlock>> ui_xLabel_dic = new Dictionary<string, List<TextBlock>>();
        private Dictionary<String, List<TextBlock>> ui_yLabel_dic = new Dictionary<string, List<TextBlock>>();
        private double m_valuesMax = 0;
        private double m_valuesMin = 0;

        private List<string> tempCheckUseColumnList = new List<string>();
        private List<string> tempRemoveColumnList = new List<string>();


        private void Update(object sender, EventArgs e)
        {
            if(TryEnterLock())
            {
                //Assemble Objects base on Canvas
                Assembly();

                //TODO: It's Graphs Data Drawing (Unique)
                //add chart graph
                if (dataTable != null && dataTable.Columns.Count >= 2 && dataTable.Rows.Count > 0)
                {
                    //chart zoom setup
                    int startRowNum = 0;
                    int endRowNum = dataTable.Rows.Count - 1;
                    int viewRowCount = endRowNum - startRowNum;

                    //calculate chart label position period
                    double xLabelWidthPeriod = 40;
                    double yLabelHeightPeriod = 40;
                    double xLabelCountPeriod = viewRowCount * (xLabelWidthPeriod / ui_section_body.ActualWidth);
                    double yLabelCountPeriod = viewRowCount * (yLabelHeightPeriod / ui_section_body.ActualHeight);
                    double tempCompareValue = 0;

                    tempCheckUseColumnList.Clear();

                    //generate data-draw frameworks elements
                    for (int nCol = 1; nCol < dataTable.Columns.Count; nCol++)
                    {
                        string tempColumnName = dataTable.Columns[nCol].ColumnName;
                        tempCheckUseColumnList.Add(tempColumnName);

                        //gen ui_simbol instance.
                        if (!ui_simbols.ContainsKey(tempColumnName))
                            ui_simbols.Add(tempColumnName, new TextBlock());

                        USCommon.AssemblySingle(ui_section_simbols, ui_simbols[tempColumnName]);

                        for (int nRow = 0; nRow < dataTable.Rows.Count; nRow++)
                        {
                            //Find Max Value
                            tempCompareValue = dataTable.Rows[nRow].Field<double>(tempColumnName);
                            if (m_valuesMax < tempCompareValue)
                                m_valuesMax = tempCompareValue;
                            if (m_valuesMin > tempCompareValue)
                                m_valuesMin = tempCompareValue;
                        }
                    }

                    //Display Y Labels
                    int num = 0;
                    double yLabelMaxHeight = ui_section_yLabels.Margin.Bottom - ui_section_yLabels.Margin.Top;
                    for(double yLabelHeight = 0; yLabelHeight < ui_section_yLabels.ActualHeight; yLabelHeight+=yLabelHeightPeriod)
                    {
                        if (ui_yLabels.Count <= num)
                            ui_yLabels.Add(new TextBlock());
                        ui_yLabels[num].Text = "test";
                        USCommon.AssemblySingle(ui_section_yLabels, ui_yLabels[num]);
                        ui_yLabels[num].Margin = new Thickness(0, yLabelHeight, 0, 0);
                        num++;
                    }


                    //remove unused objects in ui_simbols
                    tempRemoveColumnList.Clear();
                    foreach (string key in ui_simbols.Keys)
                    {
                        if (!tempCheckUseColumnList.Contains(key))
                        {
                            tempRemoveColumnList.Add(key);
                        }
                    }
                    foreach (string key in tempRemoveColumnList)
                    {
                        USCommon.DeAssemblySingle(ui_section_simbols, ui_simbols[key]);
                        ui_simbols[key] = null;
                        ui_simbols.Remove(key);
                    }
                }

                //Update Objects Contents
                ContentsUpdate();
                //Update Objects Positions
                PositionUpdate();

                ExitLock();
            }           
        }



        public void Assembly()
        {
            USCommon.AssemblySingle(ui_canvas, ui_section_header);
            USCommon.AssemblySingle(ui_canvas, ui_section_body);
            USCommon.AssemblySingle(ui_canvas, ui_section_simbols);
            USCommon.AssemblySingle(ui_canvas, ui_section_xLabels);
            USCommon.AssemblySingle(ui_canvas, ui_section_yLabels);
            USCommon.AssemblySingle(ui_canvas, ui_line_bottom);
            USCommon.AssemblySingle(ui_canvas, ui_line_right);
            USCommon.AssemblySingle(ui_section_header, ui_title);
        }

        //Please Use Format : EnterLock(); access _dataTable ExitLock(); 
        public DataTable dataTable { get; set; }
        public string ChartTitle { get; set; }
        public void ContentsUpdate()
        {
            //update title
            USCommon.UpdateText(ui_title, ChartTitle, 16, FontWeight.FromOpenTypeWeight(600), USBrushs.GetSolidBrush(Colors.Black));

            //update graph border lines
            USCommon.UpdateLine(ui_line_bottom,
                new Point(10, ui_canvas.ActualHeight - 45),
                new Point(ui_canvas.ActualWidth - 79, ui_canvas.ActualHeight - 45),
                1,
                USBrushs.GetSolidBrush(Colors.DarkGray),
                true);

            USCommon.UpdateLine(ui_line_right,
                new Point(ui_canvas.ActualWidth - 80, 45),
                new Point(ui_canvas.ActualWidth - 80, ui_canvas.ActualHeight - 45),
                1,
                USBrushs.GetSolidBrush(Colors.DarkGray),
                true);

            //update ui_simbol contents
            int num = 0;
            foreach(string key in ui_simbols.Keys)
            {
               
                USCommon.UpdateText(ui_simbols[key],key,14,FontWeight.FromOpenTypeWeight(300), USBrushs.GetPastelSolidBrush(num));
                num++;
            }
        }

        private static Thickness sectionHeaderPosition = new Thickness(15, 15, 0, 0);
        private static Thickness sectionSimbolsPosition = new Thickness(15, 50, 0, 0);
        private static Thickness sectionBodyPosition = new Thickness();
        private static Thickness sectionXLabelsPosition = new Thickness();
        private static Thickness sectionYLabelsPosition = new Thickness();

        public void PositionUpdate()
        {
            //Update ui_section_header Position
            if (ui_section_header.Margin != sectionHeaderPosition)
                ui_section_header.Margin = sectionHeaderPosition;

            //Update ui_section_simbols Position
            if (ui_section_simbols.Margin != sectionSimbolsPosition)
                ui_section_simbols.Margin = sectionSimbolsPosition;

            //Update ui_section_body Position
            sectionBodyPosition.Left = 10;
            sectionBodyPosition.Top = 45;
            if (ui_section_body.Margin != sectionBodyPosition)
            {
                ui_section_body.Margin = sectionBodyPosition;
                ui_section_body.Width = ui_canvas.ActualWidth - 70 - sectionBodyPosition.Left;
                ui_section_body.Height = ui_canvas.ActualHeight - 60 - sectionBodyPosition.Bottom;
            }

            //Update ui_section_xLabels Positioin
            sectionXLabelsPosition.Left = 10;
            sectionXLabelsPosition.Top = ui_canvas.ActualHeight - 60;
            if (ui_section_xLabels.Margin != sectionXLabelsPosition)
            {
                ui_section_xLabels.Margin = sectionXLabelsPosition;
                ui_section_xLabels.Width = ui_canvas.ActualWidth - 100 - sectionXLabelsPosition.Left;
                ui_section_xLabels.Height = ui_canvas.ActualHeight - 10 - sectionXLabelsPosition.Top;
            }

            //Update ui_section_yLabels Positioin
            sectionYLabelsPosition.Left = ui_canvas.ActualWidth - 70;
            sectionYLabelsPosition.Top = 45;
            if (ui_section_yLabels.Margin != sectionYLabelsPosition)
            {
                ui_section_yLabels.Margin = sectionYLabelsPosition;
                ui_section_yLabels.Width = ui_canvas.ActualWidth - 10 - sectionYLabelsPosition.Left;
                ui_section_yLabels.Height = ui_canvas.ActualHeight - 60 - sectionYLabelsPosition.Top;
            }
        }



    }
}
