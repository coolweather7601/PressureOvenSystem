using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;
using System.Data;

namespace nModBusWeb
{
    public partial class chart_multiaxes : System.Web.UI.Page
    {
        static private string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
        //query
        static private string mahcineID, StartTime, EndTime;

        //
        // Page Load event handler
        //
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mahcineID = Request.QueryString["machineID"];
                StartTime = Request.QueryString["StartTime"];
                EndTime = Request.QueryString["EndTime"];

                if (string.IsNullOrEmpty(mahcineID))
                {
                    #region  Sample

                    // Data for the chart
                    double[] data0 = {1700, 3900, 2900, 3800, 4100, 4600, 2900, 4100, 4400, 5700,
        5900, 5200, 3700, 3400, 5100, 5600, 5600, 6000, 7000, 7600, 6300, 6700, 7500,
        6400, 8800};
                    double[] data1 = {500, 550, 670, 990, 820, 730, 800, 720, 730, 790, 860, 800,
        840, 680, 740, 890, 680, 790, 730, 770, 840, 820, 800, 840, 670};
                    double[] data2 = {46, 68, 35, 33, 38, 20, 12, 18, 15, 23, 30, 24, 28, 15, 21, 26,
        46, 42, 38, 25, 23, 32, 24, 20, 25};
                    double[] data3 = {0.84, 0.82, 0.82, 0.38, 0.25, 0.52, 0.54, 0.52, 0.38, 0.51,
        0.46, 0.29, 0.5, 0.55, 0.47, 0.34, 0.52, 0.33, 0.21, 0.3, 0.25, 0.15, 0.18,
        0.22, 0.3};

                    // Labels for the chart
                    string[] labels = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11",
        "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24"}
                        ;

                    // Create a XYChart object of size 600 x 360 pixels. Use a vertical gradient
                    // color from sky blue (aaccff) to white (ffffff) as background. Set border to
                    // grey (888888). Use rounded corners. Enable soft drop shadow.
                    XYChart c = new XYChart(600, 360);
                    c.setBackground(c.linearGradientColor(0, 0, 0, c.getHeight(), 0xaaccff, 0xffffff
                        ), 0x888888);
                    c.setRoundedFrame();
                    c.setDropShadow();

                    // Add a title box to the chart using 15 pts Arial Bold Italic font. Set top
                    // margin to 16 pixels.
                    ChartDirector.TextBox title = c.addTitle("Multiple Axes Demonstration",
                        "Arial Bold Italic", 15);
                    title.setMargin2(0, 0, 16, 0);

                    // Set the plotarea at (100, 80) and of size 400 x 230 pixels, with white
                    // (ffffff) background. Use grey #(aaaaa) dotted lines for both horizontal and
                    // vertical grid lines.
                    c.setPlotArea(100, 80, 400, 230, 0xffffff, -1, -1, c.dashLineColor(0xaaaaaa,
                        Chart.DotLine), -1);

                    // Add a legend box with the bottom center anchored at (300, 80) (top center of
                    // the plot area). Use horizontal layout, and 8 points Arial Bold font. Set
                    // background and border to transparent.
                    LegendBox legendBox = c.addLegend(300, 80, false, "Arial Bold", 8);
                    legendBox.setAlignment(Chart.BottomCenter);
                    legendBox.setBackground(Chart.Transparent, Chart.Transparent);

                    // Set the labels on the x axis.
                    c.xAxis().setLabels(labels);

                    // Display 1 out of 3 labels on the x-axis.
                    c.xAxis().setLabelStep(3);

                    // Add a title to the x-axis
                    c.xAxis().setTitle("Hour of Day");

                    // Add a title on top of the primary (left) y axis.
                    c.yAxis().setTitle("Power\n(Watt)").setAlignment(Chart.TopLeft2);
                    // Set the axis, label and title colors for the primary y axis to red (c00000) to
                    // match the first data set
                    c.yAxis().setColors(0xcc0000, 0xcc0000, 0xcc0000);

                    // Add a title on top of the secondary (right) y axis.
                    c.yAxis2().setTitle("Load\n(Mbps)").setAlignment(Chart.TopRight2);
                    // Set the axis, label and title colors for the secondary y axis to green
                    // (00800000) to match the second data set
                    c.yAxis2().setColors(0x008000, 0x008000, 0x008000);

                    // Add the third y-axis at 50 pixels to the left of the plot area
                    Axis leftAxis = c.addAxis(Chart.Left, 50);
                    // Add a title on top of the third y axis.
                    leftAxis.setTitle("Temp\n(C)").setAlignment(Chart.TopLeft2);
                    // Set the axis, label and title colors for the third y axis to blue (0000cc) to
                    // match the third data set
                    leftAxis.setColors(0x0000cc, 0x0000cc, 0x0000cc);

                    // Add the fouth y-axis at 50 pixels to the right of the plot area
                    Axis rightAxis = c.addAxis(Chart.Right, 50);
                    // Add a title on top of the fourth y axis.
                    rightAxis.setTitle("Error\n(%)").setAlignment(Chart.TopRight2);
                    // Set the axis, label and title colors for the fourth y axis to purple (880088)
                    // to match the fourth data set
                    rightAxis.setColors(0x880088, 0x880088, 0x880088);

                    // Add a line layer to for the first data set using red (c00000) color, with a
                    // line width of 2 pixels
                    LineLayer layer0 = c.addLineLayer(data0, 0xcc0000, "Power");
                    layer0.setLineWidth(2);

                    // Add a line layer to for the second data set using green (00c0000) color, with
                    // a line width of 2 pixels. Bind the layer to the secondary y-axis.
                    LineLayer layer1 = c.addLineLayer(data1, 0x008000, "Load");
                    layer1.setLineWidth(2);
                    layer1.setUseYAxis2();

                    // Add a line layer to for the third data set using blue (0000cc) color, with a
                    // line width of 2 pixels. Bind the layer to the third y-axis.
                    LineLayer layer2 = c.addLineLayer(data2, 0x0000cc, "Temperature");
                    layer2.setLineWidth(2);
                    layer2.setUseYAxis(leftAxis);

                    // Add a line layer to for the fourth data set using purple (880088) color, with
                    // a line width of 2 pixels. Bind the layer to the fourth y-axis.
                    LineLayer layer3 = c.addLineLayer(data3, 0x880088, "Error Rate");
                    layer3.setLineWidth(2);
                    layer3.setUseYAxis(rightAxis);

                    // Output the chart
                    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

                    // Include tool tip for the chart
                    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
                        "title='{dataSetName} at hour {xLabel} = {value}'");
                    #endregion
                }
                else { btnQuery_Click(null, null); }
            }
        }

        public enum kind : int { Pressure = 1, ch1 = 2, ch2 = 3, ch3 = 4 }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mahcineID)) { mahcineID = txtMachineID.Text.Trim().ToUpper(); }
            if (string.IsNullOrEmpty(StartTime)) { StartTime = txtStart.Text; }
            if (string.IsNullOrEmpty(EndTime)) { EndTime = txtEnd.Text; }

            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);
            string str = string.Format(@"Select     Time,Data,kindName,Measure 
                                            From       oven_assy_log L
                                            Inner join oven_assy_logkind LK on L.oven_assy_logkindid=Lk.oven_assy_logkindid
                                            Where      Machine_ID = '{0}'
                                            And        Time >= to_date('{1}', 'YYYY/MM/DD') And Time <= to_date('{2}', 'YYYY/MM/DD')", mahcineID, StartTime, EndTime);

            DataTable dt_Pressure = new DataTable();
            dt_Pressure = ado.loadDataTable(str + string.Format(@" And L.oven_assy_logkindid = '{0}'", (int)kind.Pressure), null, "oven_assy_log");
            
            DataTable dt_Ch1 = new DataTable();
            dt_Ch1 = ado.loadDataTable(str + string.Format(@" And L.oven_assy_logkindid = '{0}'", (int)kind.ch1), null, "oven_assy_log");
            

            DataTable dt_Ch2 = new DataTable();
            dt_Ch2 = ado.loadDataTable(str + string.Format(@" And L.oven_assy_logkindid = '{0}'", (int)kind.ch2), null, "oven_assy_log");
            

            DataTable dt_Ch3 = new DataTable();
            dt_Ch3 = ado.loadDataTable(str + string.Format(@" And L.oven_assy_logkindid = '{0}'", (int)kind.ch3), null, "oven_assy_log");


            if (dt_Pressure.Rows.Count > 0) { to_Chart(dt_Pressure, dt_Ch1, dt_Ch2, dt_Ch3); }
            else { ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('no data.');", true); }
        }

        private void to_Chart(DataTable dtPre,DataTable dtCh1,DataTable dtCh2,DataTable dtCh3)
        {
            List<string> lstLable = new List<string>();
            List<double> lstData_Pressure = new List<double>();
            foreach (DataRow dr in dtPre.Rows)
            {
                lstData_Pressure.Add(Convert.ToDouble(dr["data"].ToString()));
                lstLable.Add(Convert.ToDateTime(dr["Time"].ToString()).ToString("MM/dd HH:mm:ss"));
            }

            List<double> lstData_Ch1 = new List<double>();
            foreach (DataRow dr in dtCh1.Rows) { lstData_Ch1.Add(Convert.ToDouble(dr["data"].ToString())); }

            List<double> lstData_Ch2 = new List<double>();
            foreach (DataRow dr in dtCh2.Rows) { lstData_Ch2.Add(Convert.ToDouble(dr["data"].ToString())); }

            List<double> lstData_Ch3 = new List<double>();
            foreach (DataRow dr in dtCh3.Rows) { lstData_Ch3.Add(Convert.ToDouble(dr["data"].ToString())); }

            // Data for the chart
            double[] data0 = lstData_Pressure.ToArray();
            double[] data1 = lstData_Ch1.ToArray();
            double[] data2 = lstData_Ch2.ToArray();
            double[] data3 = lstData_Ch3.ToArray();

            // Labels for the chart
            string[] labels = lstLable.ToArray();

            // Create a XYChart object of size 600 x 360 pixels. Use a vertical gradient
            // color from sky blue (aaccff) to white (ffffff) as background. Set border to
            // grey (888888). Use rounded corners. Enable soft drop shadow.
            XYChart c = new XYChart(800, 600);
            c.setBackground(c.linearGradientColor(0, 0, 0, c.getHeight(), 0xaaccff, 0xffffff
                ), 0x888888);
            c.setRoundedFrame();
            c.setDropShadow();

            // Add a title box to the chart using 15 pts Arial Bold Italic font. Set top
            // margin to 16 pixels.
            ChartDirector.TextBox title = c.addTitle("Multiple Axes Demonstration",
                "Arial Bold Italic", 15);
            title.setMargin2(0, 0, 16, 0);

            // Set the plotarea at (100, 80) and of size 400 x 230 pixels, with white
            // (ffffff) background. Use grey #(aaaaa) dotted lines for both horizontal and
            // vertical grid lines.
            c.setPlotArea(100, 80, 600, 400, 0xffffff, -1, -1, c.dashLineColor(0xaaaaaa,
                Chart.DotLine), -1);

            // Add a legend box with the bottom center anchored at (300, 80) (top center of
            // the plot area). Use horizontal layout, and 8 points Arial Bold font. Set
            // background and border to transparent.
            LegendBox legendBox = c.addLegend(300, 80, false, "Arial Bold", 8);
            legendBox.setAlignment(Chart.BottomCenter);
            legendBox.setBackground(Chart.Transparent, Chart.Transparent);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            // Display 1 out of 3 labels on the x-axis.
            c.xAxis().setLabelStep(3);

            // Add a title to the x-axis
            c.xAxis().setTitle("Hour of Day");

            // Add a title on top of the primary (left) y axis.
            //c.yAxis().setTitle("Power\n(Watt)").setAlignment(Chart.TopLeft2);
            c.yAxis().setTitle(dtPre.Rows[0]["measure"].ToString()).setAlignment(Chart.TopLeft2);
            // Set the axis, label and title colors for the primary y axis to red (c00000) to
            // match the first data set
            c.yAxis().setColors(0xcc0000, 0xcc0000, 0xcc0000);

            // Add a title on top of the secondary (right) y axis.
            //c.yAxis2().setTitle("Load\n(Mbps)").setAlignment(Chart.TopRight2);
            c.yAxis2().setTitle(dtCh1.Rows[0]["measure"].ToString()).setAlignment(Chart.TopRight2);
            // Set the axis, label and title colors for the secondary y axis to green
            // (00800000) to match the second data set
            c.yAxis2().setColors(0x008000, 0x008000, 0x008000);

            // Add the third y-axis at 50 pixels to the left of the plot area
            Axis leftAxis = c.addAxis(Chart.Left, 50);
            // Add a title on top of the third y axis.
            //leftAxis.setTitle("Temp\n(C)").setAlignment(Chart.TopLeft2);
            leftAxis.setTitle( dtCh2.Rows[0]["measure"].ToString()).setAlignment(Chart.TopLeft2);

            // Set the axis, label and title colors for the third y axis to blue (0000cc) to
            // match the third data set
            leftAxis.setColors(0x0000cc, 0x0000cc, 0x0000cc);

            // Add the fouth y-axis at 50 pixels to the right of the plot area
            Axis rightAxis = c.addAxis(Chart.Right, 50);
            // Add a title on top of the fourth y axis.
            //rightAxis.setTitle("Error\n(%)").setAlignment(Chart.TopRight2);
            rightAxis.setTitle( dtCh3.Rows[0]["measure"].ToString()).setAlignment(Chart.TopRight2);
            // Set the axis, label and title colors for the fourth y axis to purple (880088)
            // to match the fourth data set
            rightAxis.setColors(0x880088, 0x880088, 0x880088);

            // Add a line layer to for the first data set using red (c00000) color, with a
            // line width of 2 pixels
            LineLayer layer0 = c.addLineLayer(data0, 0xcc0000, "Pressure");
            layer0.setLineWidth(2);

            // Add a line layer to for the second data set using green (00c0000) color, with
            // a line width of 2 pixels. Bind the layer to the secondary y-axis.
            LineLayer layer1 = c.addLineLayer(data1, 0x008000, "Ch1");
            layer1.setLineWidth(2);
            layer1.setUseYAxis2();

            // Add a line layer to for the third data set using blue (0000cc) color, with a
            // line width of 2 pixels. Bind the layer to the third y-axis.
            LineLayer layer2 = c.addLineLayer(data2, 0x0000cc, "Ch2");
            layer2.setLineWidth(2);
            layer2.setUseYAxis(leftAxis);

            // Add a line layer to for the fourth data set using purple (880088) color, with
            // a line width of 2 pixels. Bind the layer to the fourth y-axis.
            LineLayer layer3 = c.addLineLayer(data3, 0x880088, "Ch3");
            layer3.setLineWidth(2);
            layer3.setUseYAxis(rightAxis);

            // Output the chart
            WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
                "title='{dataSetName} at hour {xLabel} = {value}'");
        }
    }
}