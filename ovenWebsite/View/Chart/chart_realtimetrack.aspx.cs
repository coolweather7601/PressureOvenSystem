using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;
using System.Data;

namespace nModBusWeb
{
    public partial class chart_realtimetrack : System.Web.UI.Page
    {
        static private string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
        static private int out_width = 1100, out_hight = 500, in_width = 900, in_hight = 350;

        //
        // Page Load event handler
        //
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // This script handles both the full page request, as well as the subsequent partial updates
            // (AJAX chart updates). We need to determine the type of request first before we processing it.
            //
            if (WebChartViewer.IsPartialUpdateRequest(Page))
            {
                // Is a partial update request.

                // The .NET platform will not restore the states of the controls before or during Page_Load,
                // so we need to restore the state ourselves
                WebChartViewer1.LoadViewerState();

                // Draw the chart in partial update mode
                drawChart(WebChartViewer1);

                // Output the chart immediately and then terminate the page life cycle (PartialUpdateChart
                // will cause Page_Load to terminate immediately without running the following code).
                WebChartViewer1.PartialUpdateChart();
            }

            //
            // If the code reaches here, it is a full page request.
            //

            drawChart(WebChartViewer1);
        }

        //
        // Draw the chart
        //
        private void drawChart(WebChartViewer viewer)
        {
            //
            // Data to draw the chart. In this demo, the data buffer will be filled by a random data
            // generator. In real life, the data is probably stored in a buffer (eg. a database table, a
            // text file, or some global memory) and updated by other means.
            //

            // We use a data buffer to emulate the last 240 samples.
            int sampleSize = 240;
            double[] dataSeries1 = new double[sampleSize];
            double[] dataSeries2 = new double[sampleSize];
            double[] dataSeries3 = new double[sampleSize];
            DateTime[] timeStamps = new DateTime[sampleSize];

            // Our pseudo random number generator
            DateTime firstDate = DateTime.Now.AddSeconds(-timeStamps.Length);
            for (int i = 0; i < timeStamps.Length; ++i)
            {
                timeStamps[i] = firstDate.AddSeconds(i);
                double p = timeStamps[i].Ticks / 10000000;
                dataSeries1[i] = Math.Cos(p * 2.1) * 10 + 1 / (Math.Cos(p) * Math.Cos(p) + 0.01) + 20;
                dataSeries2[i] = 100 * Math.Sin(p / 27.7) * Math.Sin(p / 10.1) + 150;
                dataSeries3[i] = 100 * Math.Cos(p / 6.7) * Math.Cos(p / 11.9) + 150;
            }

            // Create an XYChart object 600 x 270 pixels in size, with light grey (f4f4f4) background, black
            // (000000) border, 1 pixel raised effect, and with a rounded frame.
            XYChart c = new XYChart(out_width, out_hight, 0xf4f4f4, 0x000000, 0);
            c.setRoundedFrame();

            // Set the plotarea at (55, 57) and of size 520 x 185 pixels. Use white (ffffff) background.
            // Enable both horizontal and vertical grids by setting their colors to grey (cccccc). Set
            // clipping mode to clip the data lines to the plot area.
            c.setPlotArea(55, 57, in_width, in_hight, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);
            c.setClipping();

            // Add a title to the chart using 15 pts Times New Roman Bold Italic font, with a light grey
            // (dddddd) background, black (000000) border, and a glass like raised effect.
            c.addTitle("Field Intensity at Observation Satellite", "Times New Roman Bold Italic", 15
                ).setBackground(0xdddddd, 0x000000, Chart.glassEffect());

            // Configure the y-axis with a 10pts Arial Bold axis title
            c.yAxis().setTitle("Intensity (V/m)", "Arial Bold", 10);

            // Configure the x-axis to auto-scale with at least 75 pixels between major tick and 15 pixels
            // between minor ticks. This shows more minor grid lines on the chart.
            c.xAxis().setTickDensity(75, 15);

            // Set the axes width to 2 pixels
            c.xAxis().setWidth(2);
            c.yAxis().setWidth(2);

            // Set the x-axis label format
            c.xAxis().setLabelFormat("{value|hh:nn:ss}");

            // Create a line layer to plot the lines
            LineLayer layer = c.addLineLayer2();

            // The x-coordinates are the timeStamps.
            layer.setXData(timeStamps);

            // The 3 data series are used to draw 3 lines. Here we put the latest data values as part of the
            // data set name, so you can see them updated in the legend box.
            layer.addDataSet(dataSeries1, 0xff0000, "Alpha");
            layer.addDataSet(dataSeries2, 0x00cc00, "Beta");
            layer.addDataSet(dataSeries3, 0x0000ff, "Gamma");

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);

            // Output Javascript chart model to the browser to suppport tracking cursor
            WebChartViewer1.ChartModel = c.getJsChartModel();
        }

    }
}