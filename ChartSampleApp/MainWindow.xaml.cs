using System;
using System.Threading;
using System.Windows;

namespace ChartSampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread thDataRowAdd = null;

        public MainWindow()
        {
            InitializeComponent();

            //Initializing USLineChart's Instance of UnseonChartLibs 
            ui_usLineChart1.EnterLock();
            ui_usLineChart1.ChartTitle = "Simple Unseon Line Chart Sample";
            ui_usLineChart1.dataTable.Columns.Add("DateTime", typeof(DateTime));
            ui_usLineChart1.dataTable.Columns.Add("Sensor1", typeof(Double));
            ui_usLineChart1.dataTable.Columns.Add("Sensor2", typeof(Double));
            ui_usLineChart1.dataTable.Columns.Add("Sensor3", typeof(Double));
            ui_usLineChart1.ExitLock(); 

            //Start Live Chart Data Auto Generator Thread
            if (thDataRowAdd == null)
                thDataRowAdd = new Thread(new ThreadStart(AddChartDataAuto));
            thDataRowAdd.Start();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //when closing main window, it kill the current process.
            System.Environment.Exit(0);
        }

        private double sensorLatestValue1 = 100;
        private double sensorLatestValue2 = 100;
        private double sensorLatestValue3 = 100;
        private void AddChartDataAuto()
        {
            //Chart Value Random Generation.
            Random random = new Random();
            while (true)
            {
                sensorLatestValue1 += 1 - (random.NextDouble() * 2);
                sensorLatestValue2 += 1 - (random.NextDouble() * 2);
                sensorLatestValue3 += 1 - (random.NextDouble() * 2);

                ui_usLineChart1.EnterLock();
                ui_usLineChart1.dataTable.Rows.Add(DateTime.Now, sensorLatestValue1, sensorLatestValue2, sensorLatestValue3);
                if (ui_usLineChart1.dataTable.Rows.Count > 1000)
                {
                    ui_usLineChart1.dataTable.Rows.RemoveAt(0);
                }
                ui_usLineChart1.ExitLock();

                Thread.Sleep(50);
            }
        }

    }
}
