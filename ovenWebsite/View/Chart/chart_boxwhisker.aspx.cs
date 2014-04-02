using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;

namespace nModBusWeb
{
    public partial class chart_boxwhisker : System.Web.UI.Page
    {
        static private string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
        //
        // Page Load event handler
        //
        protected void Page_Load(object sender, EventArgs e)
        {
            chartInit(1);
        }

        private void chartInit(int i)
        {
            // Sample data for the Box-Whisker chart. Represents the minimum, 1st quartile,
            // medium, 3rd quartile and maximum values of some quantities
            double[] Q0Data = { 40, 45, 40, 30, 20, 50, 25, 44 };
            if (i % 2 == 0) Q0Data = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };

            double[] Q1Data = { 55, 60, 50, 40, 38, 60, 51, 60 };
            double[] Q2Data = { 62, 70, 60, 50, 48, 70, 62, 70 };
            double[] Q3Data = { 70, 80, 65, 60, 53, 78, 69, 76 };
            double[] Q4Data = { 80, 90, 75, 70, 60, 85, 80, 84 };

            // The labels for the chart
            string[] labels = { "2/19 04:00", "2/19 05:00", "2/19 06:00", "2/19 07:00", "2/19 08:00", "2/19 09:00", "2/19 10:00", "2/19 11:00" };

            // Create a XYChart object of size 550 x 250 pixels
            XYChart c = new XYChart(1000, 1000);

            // Set the plotarea at (50, 25) and of size 450 x 200 pixels. Enable both
            // horizontal and vertical grids by setting their colors to grey (0xc0c0c0)
            c.setPlotArea(50, 25, 600, 400).setGridColor(0xc0c0c0, 0xc0c0c0);

            // Add a title to the chart
            c.addTitle("Computer Vision Test Scores");

            // Set the labels on the x axis and the font to Arial Bold
            c.xAxis().setLabels(labels).setFontStyle("Arial Bold");

            // Set the font for the y axis labels to Arial Bold
            c.yAxis().setLabelStyle("Arial Bold");

            // Add a Box Whisker layer using light blue 0x9999ff as the fill color and blue
            // (0xcc) as the line color. Set the line width to 2 pixels
            c.addBoxWhiskerLayer(Q3Data, Q1Data, Q4Data, Q0Data, Q2Data, 0x9999ff, 0x0000cc
                ).setLineWidth(2);

            // Output the chart
            WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
                "title='{xLabel}: min/med/max = {min}/{med}/{max}\nInter-quartile range: " +
                "{bottom} to {top}'");
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);

        }
    }
}