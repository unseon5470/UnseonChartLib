﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            ui_usLineChart1.EnterLock();
            //Initialize Chart Datas
            ui_usLineChart1.ChartTitle = "Line Chart Sample";
            //Set Chart Column Information
            ui_usLineChart1.dataTable.Columns.Add("DateTime",typeof(DateTime));
            ui_usLineChart1.dataTable.Columns.Add("Level",typeof(Double));
            ui_usLineChart1.ExitLock();

            if (thDataRowAdd == null)
                thDataRowAdd = new Thread(new ThreadStart(AddChartDataAuto));
            thDataRowAdd.Start();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Close Process All.
            Application.Current.Shutdown();
        }

        private double startLevel = 100;
        private void AddChartDataAuto()
        {
            Random random = new Random();
            while(true)
            {
                startLevel += 1-(random.NextDouble()*2);
                ui_usLineChart1.EnterLock();
                ui_usLineChart1.dataTable.Rows.Add(DateTime.Now, startLevel);
                if(ui_usLineChart1.dataTable.Rows.Count>1000)
                {
                    ui_usLineChart1.dataTable.Rows.RemoveAt(0);
                }
                ui_usLineChart1.ExitLock();
                Thread.Sleep(50);
            }
        }

    }
}
