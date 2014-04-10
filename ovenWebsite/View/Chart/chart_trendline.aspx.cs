using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;
using System.Data;

namespace nModBusWeb
{
    public partial class chart_trendline : System.Web.UI.Page
    {
        static private string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
        static private int out_width = 1100, out_hight = 500, in_width = 1000, in_hight = 400;
        //
        // Page Load event handler
        //
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtMachineID.Text = Request.QueryString["machineID"];
                txtStart.Text = Request.QueryString["StartTime"];
                txtEnd.Text = Request.QueryString["EndTime"];
                
                if (string.IsNullOrEmpty(txtMachineID.Text))
                {
                    #region Sample

                    // The data for the line chart
                    double[] data = {50, 55, 47, 34, 42,
                         49, 63, 62, 73, 59,
                         56, 50, 64, 60, 67,
                         67, 58, 59, 73, 77,
                         84, 82, 80, 90};

                    // The labels for the line chart
                    string[] labels = { "2/19 01:00", "2/19 02:00", "2/19 03:00", "2/19 04:00", "2/19 05:00", "2/19 06:00", "2/19 07:00", "2/19 08:00",
                            "2/19 09:00", "2/19 10:00", "2/19 11:00", "2/19 12:00", "2/19 13:00", "2/19 14:00", "2/19 15:00", "2/19 16:00",
                            "2/19 17:00", "2/19 18:00", "2/19 19:00", "2/19 20:00", "2/19 21:00", "2/19 22:00", "2/19 23:00", "2/19 24:00"};

                    // Create a XYChart object of size 500 x 320 pixels, with a pale purpule
                    // (0xffccff) background, a black border, and 1 pixel 3D border effect.
                    //XYChart c = new XYChart(700, 520, 0xffccff, 0x000000, 1);
                    XYChart c = new XYChart(out_width, out_hight, 0xffccff, 0x000000, 1);

                    // Set the plotarea at (55, 45) and of size 420 x 210 pixels, with white
                    // background. Turn on both horizontal and vertical grid lines with light grey
                    // color (0xc0c0c0)
                    //c.setPlotArea(55, 45, 600, 400, 0xffffff, -1, -1, 0xc0c0c0, -1);
                    c.setPlotArea(55, 45, in_width, in_hight, 0xffffff, -1, -1, 0xc0c0c0, -1);

                    // Add a legend box at (55, 25) (top of the chart) with horizontal layout. Use 8
                    // pts Arial font. Set the background and border color to Transparent.
                    c.addLegend(55, 22, false, "", 8).setBackground(Chart.Transparent);

                    // Add a title box to the chart using 13 pts Times Bold Italic font. The text is
                    // white (0xffffff) on a purple (0x800080) background, with a 1 pixel 3D border.
                    c.addTitle("Long Term Server Load (Sample)", "Times New Roman Bold Italic", 13, 0xffffff
                        ).setBackground(0x800080, -1, 1);

                    // Add a title to the y axis
                    c.yAxis().setTitle("MBytes");

                    // Set the labels on the x axis. Rotate the font by 90 degrees.
                    c.xAxis().setLabels(labels).setFontAngle(90);

                    // Add a line layer to the chart
                    LineLayer lineLayer = c.addLineLayer();

                    // Add the data to the line layer using light brown color (0xcc9966) with a 7
                    // pixel square symbol
                    lineLayer.addDataSet(data, 0xcc9966, "Server Utilization").setDataSymbol(
                        Chart.SquareSymbol, 7);

                    // Set the line width to 2 pixels
                    lineLayer.setLineWidth(2);

                    // tool tip for the line layer
                    lineLayer.setHTMLImageMap("", "", "title='{xLabel}: {value} MBytes'");

                    // Add a trend line layer using the same data with a dark green (0x008000) color.
                    // Set the line width to 2 pixels
                    TrendLayer trendLayer = c.addTrendLayer(data, 0x008000, "Trend Line");
                    trendLayer.setLineWidth(2);

                    // tool tip for the trend layer
                    trendLayer.setHTMLImageMap("", "",
                        "title='Change rate: {slope|2} MBytes/per month'");

                    // Output the chart
                    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

                    // include tool tip for the chart
                    WebChartViewer1.ImageMap = c.getHTMLImageMap("");
                    #endregion
                }
                else { btnQuery_Click(null, null); }


                string[] arrKind = new string[] { "Pressure", "Ch1", "Ch2", "Ch3" };
                int kindcount = 1;
                foreach (string k in arrKind)
                {
                    ddlkind.Items.Add(new ListItem(k, kindcount.ToString()));
                    kindcount++;
                }
                ddlkind.Items.Insert(0, new ListItem("please select.."));
            }
        }
        
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (!ddlkind.SelectedValue.Contains("please"))
            {                
                App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);
                string str = string.Format(@"Select     Time,Data,kindName,Measure 
                                             From       oven_assy_log L
                                             Inner join oven_assy_logkind LK on L.oven_assy_logkindid=Lk.oven_assy_logkindid
                                             Where      Machine_ID = '{0}'
                                             And        Time >= to_date('{1}', 'YYYY/MM/DD') And Time <= to_date('{2}', 'YYYY/MM/DD')
                                             And        L.oven_assy_logkindid = '{3}'", txtMachineID.Text.Trim().ToUpper(),txtStart.Text, txtEnd.Text, ddlkind.SelectedValue);
                DataTable dt = new DataTable();
                dt = ado.loadDataTable(str, null, "oven_assy_log");

                if (dt.Rows.Count > 0) { to_Chart(dt); }
                else { ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('no data.');", true); }
            }
            else 
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('please select kind');", true);
            }
        }
        
        public enum kind : int { Pressure = 1, ch1 = 2, ch2 = 3, ch3 = 4 }
        private void to_Chart(DataTable cdt)
        {
            List<double> lstData = new List<double>();
            List<string> lstLable = new List<string>();

            foreach (DataRow dr in cdt.Rows)
            {
                lstData.Add(Convert.ToDouble(dr["data"].ToString()));
                lstLable.Add(Convert.ToDateTime(dr["Time"].ToString()).ToString("MM/dd HH:mm:ss"));
            }
            
            // The data for the line chart
            double[] data = lstData.ToArray();

            // The labels for the line chart
            string[] labels = lstLable.ToArray();
            
            // Create a XYChart object of size 500 x 320 pixels, with a pale purpule
            // (0xffccff) background, a black border, and 1 pixel 3D border effect.
            //XYChart c = new XYChart(700, 520, 0xffccff, 0x000000, 1);
            XYChart c = new XYChart(out_width, out_hight, 0xffccff, 0x000000, 1);

            // Set the plotarea at (55, 45) and of size 420 x 210 pixels, with white
            // background. Turn on both horizontal and vertical grid lines with light grey
            // color (0xc0c0c0)
            //c.setPlotArea(55, 45, 600, 400, 0xffffff, -1, -1, 0xc0c0c0, -1);
            c.setPlotArea(55, 45, in_width, in_hight, 0xffffff, -1, -1, 0xc0c0c0, -1);

            // Add a legend box at (55, 25) (top of the chart) with horizontal layout. Use 8
            // pts Arial font. Set the background and border color to Transparent.
            c.addLegend(55, 22, false, "", 8).setBackground(Chart.Transparent);

            // Add a title box to the chart using 13 pts Times Bold Italic font. The text is
            // white (0xffffff) on a purple (0x800080) background, with a 1 pixel 3D border.
            c.addTitle("Long Term Server Load", "Times New Roman Bold Italic", 13, 0xffffff
                ).setBackground(0x800080, -1, 1);

            // Add a title to the y axis
            //c.yAxis().setTitle("MBytes");
            c.yAxis().setTitle(cdt.Rows[0]["kindname"].ToString() + cdt.Rows[0]["measure"].ToString());

            // Set the labels on the x axis. Rotate the font by 90 degrees.
            c.xAxis().setLabels(labels).setFontAngle(90);

            // Add a line layer to the chart
            LineLayer lineLayer = c.addLineLayer();

            // Add the data to the line layer using light brown color (0xcc9966) with a 7
            // pixel square symbol
            lineLayer.addDataSet(data, 0xcc9966, "Server Utilization").setDataSymbol(
                Chart.SquareSymbol, 7);

            // Set the line width to 2 pixels
            lineLayer.setLineWidth(2);

            // tool tip for the line layer
            lineLayer.setHTMLImageMap("", "", "title='{xLabel}: {value} MBytes'");

            // Add a trend line layer using the same data with a dark green (0x008000) color.
            // Set the line width to 2 pixels
            TrendLayer trendLayer = c.addTrendLayer(data, 0x008000, "Trend Line");
            trendLayer.setLineWidth(2);

            // tool tip for the trend layer
            trendLayer.setHTMLImageMap("", "",
                "title='Change rate: {slope|2} MBytes/per month'");

            // Output the chart
            WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

            // include tool tip for the chart
            WebChartViewer1.ImageMap = c.getHTMLImageMap("");
        }
    }
}