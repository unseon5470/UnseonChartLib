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
        private List<Line> ui_xLines = new List<Line>();
        private List<Line> ui_yLines = new List<Line>();
        private Dictionary<String, TextBlock> ui_simbols = new Dictionary<String, TextBlock>();
        private Dictionary<String, List<TextBlock>> ui_xLabel_dic = new Dictionary<string, List<TextBlock>>();
        private Dictionary<String, List<TextBlock>> ui_yLabel_dic = new Dictionary<string, List<TextBlock>>();
        private double m_valuesMax = 0;
        private double m_valuesMin = 0;
        private double m_valuesWidth = 0;
        private List<string> tempCheckUseColumnList = new List<string>();
        private List<string> tempRemoveColumnList = new List<string>();

        private DoubleCollection dashType01 = new DoubleCollection(new double[] { 0.4, 1 });


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
                    double xLabelWidthPeriod = 100;
                    double yLabelHeightPeriod = 40;
                    //i don't like format change. but.... div need format change to integer
                    int xLabelCountPeriod = (int)(viewRowCount * (xLabelWidthPeriod / ui_section_body.ActualWidth));
                    int yLabelCountPeriod = (int)(viewRowCount * (yLabelHeightPeriod / ui_section_body.ActualHeight));
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
                    m_valuesWidth = m_valuesMax + 10 - m_valuesMin;

                    //Display Y Labels & Y Lines
                    int num = 0;
                    double yLabelMaxHeight = ui_section_yLabels.Margin.Bottom - ui_section_yLabels.Margin.Top;
                    for(double yLabelHeight = 0; yLabelHeight < ui_section_yLabels.ActualHeight; yLabelHeight+=yLabelHeightPeriod)
                    {
                        if (ui_yLabels.Count <= num)
                            ui_yLabels.Add(new TextBlock());

                        if (ui_yLines.Count <= num)
                            ui_yLines.Add(new Line());

                        double tempLabelValue = (m_valuesWidth * yLabelHeight) / ui_section_body.ActualHeight;

                        ui_yLabels[num].Text = tempLabelValue.ToString("F4");
                        USCommon.AssemblySingle(ui_section_yLabels, ui_yLabels[num]);

                        USCommon.AssemblySingle(ui_section_body, ui_yLines[num]);
                        ui_yLabels[num].Margin = new Thickness(0, ui_section_yLabels.ActualHeight - yLabelHeight-7, 0, 0);
                        ui_yLines[num].X1 = 0;
                        ui_yLines[num].X2 = ui_section_body.ActualWidth;
                        ui_yLines[num].Y1 = ui_section_yLabels.ActualHeight - yLabelHeight;
                        ui_yLines[num].Y2 = ui_section_yLabels.ActualHeight - yLabelHeight;
                        ui_yLines[num].Stroke = USBrushs.GetSolidBrush(Color.FromArgb(100, 40, 40, 40));
                        ui_yLines[num].StrokeDashArray = dashType01;
                        ui_yLines[num].StrokeThickness = 1;
                        if (ui_yLines[num].SnapsToDevicePixels != true)
                        {
                            ui_yLines[num].SnapsToDevicePixels = true;
                            ui_yLines[num].SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                        }

                        num++;
                    }
                    while(ui_yLabels.Count>num)
                    {
                        USCommon.DeAssemblySingle(ui_section_yLabels, ui_yLabels[num - 1]);
                        ui_yLabels[num - 1] = null;
                        ui_yLabels.RemoveAt(num - 1);

                        USCommon.DeAssemblySingle(ui_section_body, ui_yLines[num - 1]);
                        ui_yLines[num - 1] = null;
                        ui_yLines.RemoveAt(num - 1);
                    }

                    //Display X Labels & X Lines
                    num = 0;
                    double xLabelMaxWidth = ui_section_xLabels.Margin.Right - ui_section_xLabels.Margin.Left;
                    int xLabelCount = 0;
                    for (double xLabelWidth = 0; xLabelWidth < ui_section_xLabels.ActualWidth; xLabelWidth += xLabelWidthPeriod)
                    {
                        if (ui_xLabels.Count <= num)
                            ui_xLabels.Add(new TextBlock());

                        if (ui_xLines.Count <= num)
                            ui_xLines.Add(new Line());

                        ui_xLabels[num].Text = dataTable.Rows[xLabelCount].Field<DateTime>(dataTable.Columns[0]).ToString("hh:mm:ss.fff");
                        USCommon.AssemblySingle(ui_section_xLabels, ui_xLabels[num]);
                        
                        USCommon.AssemblySingle(ui_section_body, ui_xLines[num]);
                        ui_xLabels[num].Margin = new Thickness(xLabelWidth-35, 0, 0, 0);
                        ui_xLines[num].X1 = xLabelWidth;
                        ui_xLines[num].X2 = xLabelWidth;
                        ui_xLines[num].Y1 = 0;
                        ui_xLines[num].Y2 = ui_section_body.ActualHeight;
                        ui_xLines[num].Stroke = USBrushs.GetSolidBrush(Color.FromArgb(100, 40, 40, 40));
                        ui_xLines[num].StrokeDashArray = dashType01;
                        ui_xLines[num].StrokeThickness = 1;
                        if (ui_xLines[num].SnapsToDevicePixels != true)
                        {
                            ui_xLines[num].SnapsToDevicePixels = true;
                            ui_xLines[num].SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                        }

                        if(num==0)
                        {
                            ui_xLabels[num].Visibility = Visibility.Hidden;
                            ui_xLines[num].Visibility = Visibility.Hidden;
                        }


                        num++;
                        xLabelCount += xLabelCountPeriod;
                    }
                    while (ui_xLabels.Count > num)
                    {
                        USCommon.DeAssemblySingle(ui_section_xLabels, ui_xLabels[num - 1]);
                        ui_xLabels[num - 1] = null;
                        ui_xLabels.RemoveAt(num - 1);

                        USCommon.DeAssemblySingle(ui_section_body, ui_xLines[num - 1]);
                        ui_xLines[num - 1] = null;
                        ui_xLines.RemoveAt(num - 1);
                    }

                    //Display Graph Object


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
            }
            ui_section_body.Width = Math.Abs(ui_canvas.ActualWidth - 80 - sectionBodyPosition.Left);
            ui_section_body.Height = Math.Abs(ui_canvas.ActualHeight - 45 - sectionBodyPosition.Top);

            //Update ui_section_xLabels Positioin 
            sectionXLabelsPosition.Left = 10;
            sectionXLabelsPosition.Top = ui_canvas.ActualHeight - 35;
            if (ui_section_xLabels.Margin != sectionXLabelsPosition) 
            {
                ui_section_xLabels.Margin = sectionXLabelsPosition;
            }
            ui_section_xLabels.Width = Math.Abs(ui_canvas.ActualWidth - 100 - sectionXLabelsPosition.Left);
            ui_section_xLabels.Height = Math.Abs(ui_canvas.ActualHeight - 5 - sectionXLabelsPosition.Top);

            //Update ui_section_yLabels Positioin
            sectionYLabelsPosition.Left = ui_canvas.ActualWidth - 70;
            sectionYLabelsPosition.Top = 45;
            if (ui_section_yLabels.Margin != sectionYLabelsPosition)
            {
                ui_section_yLabels.Margin = sectionYLabelsPosition;            
            }
            //Width&Height value not enable put nagative value.
            ui_section_yLabels.Width = Math.Abs(ui_canvas.ActualWidth - 10 - sectionYLabelsPosition.Left);
            ui_section_yLabels.Height = Math.Abs(ui_canvas.ActualHeight - 60 - sectionYLabelsPosition.Top);
        }
    }
}
