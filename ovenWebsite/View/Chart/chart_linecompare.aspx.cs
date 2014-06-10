using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;
using System.Data;
//NPOI
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace nModBusWeb
{
    public partial class chart_linecompare : System.Web.UI.Page
    {
        static private string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
        static private int out_width = 1100, out_hight = 535, in_width = 1000, in_hight = 400;
        static private DataTable pdt;
        static double control_Limit_Press = 0.5, control_Limit_Temp = 5;

        public string Press_result = "", Temp_result = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtMachineID.Text = Request.QueryString["Machine_ID"];
                txtBatch.Text = Request.QueryString["Batch_NO"];
                txtStart.Text = Request.QueryString["In_Time"];
                txtEnd.Text = Request.QueryString["Out_time"];

                if (!string.IsNullOrEmpty(txtMachineID.Text))
                {
                    btnQuery_Click(null, null);
                }
                else if (Request.QueryString["demo"] == "Y" || Request.QueryString["demo"] == "y")
                {
                    #region sample code

                    // Create a XYChart object of size 600 x 300 pixels, with a light grey (cccccc)
                    // background, black border, and 1 pixel 3D border effect.
                    XYChart c = new XYChart(out_width, out_hight, 0xcccccc, 0x000000, 1);

                    //Set default directory for loading images from current script directory
                    c.setSearchPath(Server.MapPath("."));

                    // Set the plotarea at (55, 58) and of size 520 x 195 pixels, with white
                    // background. Turn on both horizontal and vertical grid lines with light grey
                    // color (cccccc)
                    c.setPlotArea(55, 58, in_width, in_hight, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

                    // Add a legend box at (55, 32) (top of the chart) with horizontal layout. Use 9
                    // pts Arial Bold font. Set the background and border color to Transparent.
                    c.addLegend(55, 32, false, "Arial Bold", 9).setBackground(Chart.Transparent);

                    // Add a title box to the chart using 15 pts Times Bold Italic font. The title is
                    // in CDML and includes embedded images for highlight. The text is white (ffffff)
                    // on a black background, with a 1 pixel 3D border.
                    c.addTitle(
                        "<*block,valign=absmiddle*><*img=star.png*><*img=star.png*> Performance " +
                        "Enhancer <*img=star.png*><*img=star.png*><*/*>",
                        "Times New Roman Bold Italic", 15, 0xffffff).setBackground(0x000000, -1, 1);

                    // Add a title to the y axis
                    c.yAxis().setTitle("Temperature");

                    // Add a title to the x axis using CMDL
                    c.xAxis().setTitle(
                        "<*block,valign=absmiddle*><*img=clock.png*>  Elapsed Time<*/*>");

                    // Set the axes width to 2 pixels
                    c.xAxis().setWidth(2);
                    c.yAxis().setWidth(2);


                    // The data for the spline curve
                    double[] curveY = {50, 44, 54, 48, 58, 50, 90, 85, 104, 82, 96, 90, 74, 52, 35,
        58, 46, 54, 48, 52, 50};
                    double[] curveX = {0, 1.8, 1.2, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7,
        7.5, 8, 8.5, 9, 9.5, 10};

                    // Add a purple (800080) spline layer to the chart with a line width of 2 pixels
                    SplineLayer splineLayer = c.addSplineLayer(curveY, 0x800080,
                        "Temperature");
                    splineLayer.setXData(curveX);
                    splineLayer.setLineWidth(2);


                    // The data for the upper and lower bounding lines
                    double[] upperY = { 60, 60, 60, 60, 60, 60 };
                    double[] lowerY = { 40, 40, 40, 40, 40, 40 };
                    double[] zoneX = { 0, 2.5, 3.5, 5.5, 6.5, 10 };


                    // Add a line layer to the chart with two dark green (338033) data sets, and a
                    // line width of 2 pixels
                    LineLayer lineLayer = c.addLineLayer2();
                    lineLayer.addDataSet(upperY, 0x338033, "Control Limit H");
                    lineLayer.addDataSet(lowerY, 0x338033, "Control Limit L");
                    lineLayer.setXData(zoneX);
                    lineLayer.setLineWidth(2);

                    // Color the zone between the upper zone line and lower zone line as
                    // semi-transparent light green (8099ff99)
                    c.addInterLineLayer(lineLayer.getLine(0), lineLayer.getLine(1),
                        unchecked((int)0x8099ff99), unchecked((int)0x8099ff99));

                    // If the spline line gets above the upper zone line, color to area between the
                    // lines red (ff0000)
                    c.addInterLineLayer(splineLayer.getLine(0), lineLayer.getLine(0), 0xff0000,
                        Chart.Transparent);

                    // If the spline line gets below the lower zone line, color to area between the
                    // lines blue (0000ff)
                    c.addInterLineLayer(splineLayer.getLine(0), lineLayer.getLine(1),
                        Chart.Transparent, 0x0000ff);

                    // Add a custom CDML text at the bottom right of the plot area as the logo
                    c.addText(out_width - 50, out_hight - 320,
                        "<*block,valign=absmiddle*><*img=small_molecule.png*> <*block*>" +
                        "<*font=Times New Roman Bold Italic,size=10,color=804040*>Control " +
                        "Limit H<*/*>").setAlignment(Chart.BottomRight);

                    c.addText(out_width - 50, out_hight - 170,
                        "<*block,valign=absmiddle*><*img=small_molecule.png*> <*block*>" +
                        "<*font=Times New Roman Bold Italic,size=10,color=804040*>Control " +
                        "Limit L<*/*>").setAlignment(Chart.BottomRight);

                    // Output the chart
                    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

                    // Include tool tip for the chart
                    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
                        "title='Temperature at hour {x}: {value} C'");
                    #endregion
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e) 
        {
            if (string.IsNullOrEmpty(txtMachineID.Text)) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Please input machineID!');", true); return; }
            //===================================================================================
            //fuki curring parameters
            //txtBatch.Text = "942640A106";
            //txtMachineID.Text = "PO-002";
            //txtStart.Text = "2014/05/19 13:45:00";
            //txtEnd.Text = "2014/05/19 15:33:59";
            //===================================================================================
            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);
            string str = string.Format(@"Select     Time,Data,kindName,Measure,target,l.oven_assy_logkindid
                                             From       oven_assy_log L
                                             Inner join oven_assy_logkind LK on L.oven_assy_logkindid=Lk.oven_assy_logkindid
                                             Where      Machine_ID like '{0}%'
                                             And        Time >= to_date('{1}', 'YYYY/MM/DD hh24:mi:ss') And Time <= to_date('{2}', 'YYYY/MM/DD hh24:mi:ss')
                                             And        Batch_NO like '{3}%'
                                             Order by   Time", string.IsNullOrEmpty(txtMachineID.Text) ? "" : txtMachineID.Text.Trim().ToUpper(),
                                                               string.IsNullOrEmpty(txtStart.Text)?"2014/01/01 00:00:00":(txtStart.Text.Contains(":") ? txtStart.Text.Trim() : txtStart.Text.Trim() + " 00:00:00"),
                                                               string.IsNullOrEmpty(txtEnd.Text)?"2014/12/31 23:59:59":(txtEnd.Text.Contains(":") ? txtEnd.Text.Trim() : txtEnd.Text.Trim() + " 23:59:59"),
                                                               string.IsNullOrEmpty(txtBatch.Text) ?"" : txtBatch.Text.Trim().ToUpper());

            DataTable dt = new DataTable();
            dt = ado.loadDataTable(str, null, "oven_assy_log");
            pdt = new DataTable();
            pdt = dt;

            if (dt.Rows.Count > 0) { to_Chart(dt); }
            else { ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('no data.');", true); }
            

            //===================================================================================
            //Result(Normal or Abnormal)
            //===================================================================================
            string reStr_press = str.Replace("Order by   Time", "") + string.Format(@" And l.oven_assy_logkindid=1 And l.isOverSpec='Y' Order by Time");
            DataTable reDt_press = ado.loadDataTable(reStr_press, null, "oven_assy_log");
            if (reDt_press.Rows.Count > 0) { Press_result = "Presssure curve：<Font COLOR=#FF0000>Abnormal.</Font>"; }
            else { Press_result = "Presssure curve：<FONT COLOR=#00FF00>Normal.</font>"; }

            string reStr_ch3 = str.Replace("Order by   Time", "") + string.Format(@" And l.oven_assy_logkindid=4 And l.isOverSpec='Y' Order by Time");
            DataTable reDt_ch3 = ado.loadDataTable(reStr_ch3, null, "oven_assy_log");
            if (reDt_ch3.Rows.Count > 0) { Temp_result = "Temperature curve：<Font COLOR=#FF0000>Abnormal.</Font>"; }
            else { Temp_result = "Temperature curve：<FONT COLOR=#00FF00>Normal.</font>"; }

        }

        public enum kind : int { Pressure = 1, ch1 = 2, ch2 = 3, ch3 = 4 }
        private void to_Chart(DataTable cdt)
        {
            List<DateTime> lstData_Press_x = new List<DateTime>();
            List<double> lstData_Press_y = new List<double>();            
            List<double> lstUpper_Press_y = new List<double>();
            List<double> lstLower_Press_y = new List<double>();

            List<DateTime> lstData_Temp3_x = new List<DateTime>();
            List<double> lstData_Temp3_y = new List<double>();
            List<double> lstUpper_Temp3_y = new List<double>();
            List<double> lstLower_Temp3_y = new List<double>();

            foreach (DataRow dr in cdt.Rows)
            {
                if (dr["oven_assy_logkindid"].ToString().Equals("1")) //1=> pressure ; 4=>temperature(Ch3)
                {
                    lstData_Press_x.Add(Convert.ToDateTime(dr["Time"].ToString()));
                    lstData_Press_y.Add(Convert.ToDouble(dr["data"].ToString()));
                    lstUpper_Press_y.Add(Convert.ToDouble(dr["target"].ToString()) + control_Limit_Press);
                    lstLower_Press_y.Add(Convert.ToDouble(dr["target"].ToString()) - control_Limit_Press);
                }
                else if (dr["oven_assy_logkindid"].ToString().Equals("4"))
                {
                    lstData_Temp3_x.Add(Convert.ToDateTime(dr["Time"].ToString()));
                    lstData_Temp3_y.Add(Convert.ToDouble(dr["data"].ToString()));
                    lstUpper_Temp3_y.Add(Convert.ToDouble(dr["target"].ToString()) + control_Limit_Temp);
                    lstLower_Temp3_y.Add(Convert.ToDouble(dr["target"].ToString()) - control_Limit_Temp);
                }                
            }


            #region chart setup Pressure

            // Create a XYChart object of size 600 x 300 pixels, with a light grey (cccccc)
            // background, black border, and 1 pixel 3D border effect.
            XYChart c = new XYChart(out_width, out_hight, 0xcccccc, 0x000000, 1);

            //Set default directory for loading images from current script directory
            c.setSearchPath(Server.MapPath("."));

            // Set the plotarea at (55, 58) and of size 520 x 195 pixels, with white
            // background. Turn on both horizontal and vertical grid lines with light grey
            // color (cccccc)
            c.setPlotArea(55, 58, in_width, in_hight, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

            // Add a legend box at (55, 32) (top of the chart) with horizontal layout. Use 9
            // pts Arial Bold font. Set the background and border color to Transparent.
            c.addLegend(55, 32, false, "Arial Bold", 9).setBackground(Chart.Transparent);

            // Add a title box to the chart using 15 pts Times Bold Italic font. The title is
            // in CDML and includes embedded images for highlight. The text is white (ffffff)
            // on a black background, with a 1 pixel 3D border.
            c.addTitle(
                "<*block,valign=absmiddle*><*img=star.png*><*img=star.png*> Pressure Monitor <*img=star.png*><*img=star.png*><*/*>",
                "Times New Roman Bold Italic", 15, 0xffffff).setBackground(0x000000, -1, 1);

            // Add a title to the y axis
            c.yAxis().setTitle("Pressure(kg/cm2)");

            // Add a title to the x axis using CMDL
            c.xAxis().setTitle(
                "<*block,valign=absmiddle*><*img=clock.png*>  Elapsed Time<*/*>");

            // Set the axes width to 2 pixels
            c.xAxis().setWidth(2);
            c.yAxis().setWidth(2);


            // The data for the spline curve
            DateTime[] curveX = lstData_Press_x.ToArray();
            double[] curveY = lstData_Press_y.ToArray();

            // Add a purple (800080) spline layer to the chart with a line width of 2 pixels
            SplineLayer splineLayer = c.addSplineLayer(curveY, 0x800080,
                "Pressure(kg/cm2)");
            splineLayer.setXData(curveX);
            splineLayer.setLineWidth(2);


            // The data for the upper and lower bounding lines
            double[] upperY = lstUpper_Press_y.ToArray();
            double[] lowerY = lstLower_Press_y.ToArray();



            // Add a line layer to the chart with two dark green (338033) data sets, and a
            // line width of 2 pixels
            LineLayer lineLayer = c.addLineLayer2();
            lineLayer.addDataSet(upperY, 0x338033, "Control Limit H");
            lineLayer.addDataSet(lowerY, 0x338033, "Control Limit L");
            //lineLayer.setXData(zoneX);
            lineLayer.setXData(curveX); //test
            lineLayer.setLineWidth(2);

            // Color the zone between the upper zone line and lower zone line as
            // semi-transparent light green (8099ff99)
            c.addInterLineLayer(lineLayer.getLine(0), lineLayer.getLine(1),
                unchecked((int)0x8099ff99), unchecked((int)0x8099ff99));

            // If the spline line gets above the upper zone line, color to area between the
            // lines red (ff0000)
            c.addInterLineLayer(splineLayer.getLine(0), lineLayer.getLine(0), 0xff0000,
                Chart.Transparent);

            // If the spline line gets below the lower zone line, color to area between the
            // lines blue (0000ff)
            c.addInterLineLayer(splineLayer.getLine(0), lineLayer.getLine(1),
                Chart.Transparent, 0xFFFFFF);

            // Add a custom CDML text at the bottom right of the plot area as the logo
            c.addText(in_width,110,
                "<*block,valign=absmiddle*><*img=small_molecule.png*> <*block*>" +
                "<*font=Times New Roman Bold Italic,size=10,color=804040*>Control " +
                "Limit H<*/*>").setAlignment(Chart.BottomRight);
            
            c.addText(in_width, 210,
                "<*block,valign=absmiddle*><*img=small_molecule.png*> <*block*>" +
                "<*font=Times New Roman Bold Italic,size=10,color=804040*>Control " +
                "Limit L<*/*>").setAlignment(Chart.BottomRight);

            // Output the chart
            WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
                "title='Pressure at hour {x}: {value} C'");
            #endregion

            #region chart setup Temperature(CH3)

            // Create a XYChart object of size 600 x 300 pixels, with a light grey (cccccc)
            // background, black border, and 1 pixel 3D border effect.
            XYChart ct3 = new XYChart(out_width, out_hight, 0xcccccc, 0x000000, 1);

            //Set default directory for loading images from current script directory
            ct3.setSearchPath(Server.MapPath("."));

            // Set the plotarea at (55, 58) and of size 520 x 195 pixels, with white
            // background. Turn on both horizontal and vertical grid lines with light grey
            // color (cccccc)
            ct3.setPlotArea(55, 58, in_width, in_hight, 0xffffff, -1, -1, 0xcccccc, 0xcccccc);

            // Add a legend box at (55, 32) (top of the chart) with horizontal layout. Use 9
            // pts Arial Bold font. Set the background and border color to Transparent.
            ct3.addLegend(55, 32, false, "Arial Bold", 9).setBackground(Chart.Transparent);

            // Add a title box to the chart using 15 pts Times Bold Italic font. The title is
            // in CDML and includes embedded images for highlight. The text is white (ffffff)
            // on a black background, with a 1 pixel 3D border.
            ct3.addTitle(
                "<*block,valign=absmiddle*><*img=star.png*><*img=star.png*> Temperature(Ch3) Monitor <*img=star.png*><*img=star.png*><*/*>",
                "Times New Roman Bold Italic", 15, 0xffffff).setBackground(0x000000, -1, 1);

            // Add a title to the y axis
            ct3.yAxis().setTitle("Temperature(°C)");

            // Add a title to the x axis using CMDL
            ct3.xAxis().setTitle(
                "<*block,valign=absmiddle*><*img=clock.png*>  Elapsed Time<*/*>");

            // Set the axes width to 2 pixels
            ct3.xAxis().setWidth(2);
            ct3.yAxis().setWidth(2);


            // The data for the spline curve
            DateTime[] curveX_t3 = lstData_Temp3_x.ToArray();
            double[] curveY_t3 = lstData_Temp3_y.ToArray();

            // Add a purple (800080) spline layer to the chart with a line width of 2 pixels
            SplineLayer splineLayer_t3 = ct3.addSplineLayer(curveY_t3, 0x800080,
                "Temperature(°C)");
            splineLayer_t3.setXData(curveX_t3);
            splineLayer_t3.setLineWidth(2);


            // The data for the upper and lower bounding lines
            double[] upperY_t3 = lstUpper_Temp3_y.ToArray();
            double[] lowerY_t3 = lstLower_Temp3_y.ToArray();



            // Add a line layer to the chart with two dark green (338033) data sets, and a
            // line width of 2 pixels
            LineLayer lineLayer_t3 = ct3.addLineLayer2();
            lineLayer_t3.addDataSet(upperY_t3, 0x338033, "Control Limit H");
            lineLayer_t3.addDataSet(lowerY_t3, 0x338033, "Control Limit L");
            //lineLayer.setXData(zoneX);
            lineLayer_t3.setXData(curveX_t3); //test
            lineLayer_t3.setLineWidth(2);

            // Color the zone between the upper zone line and lower zone line as
            // semi-transparent light green (8099ff99)
            ct3.addInterLineLayer(lineLayer_t3.getLine(0), lineLayer_t3.getLine(1),
                unchecked((int)0x8099ff99), unchecked((int)0x8099ff99));

            // If the spline line gets above the upper zone line, color to area between the
            // lines red (ff0000)
            ct3.addInterLineLayer(splineLayer_t3.getLine(0), lineLayer_t3.getLine(0), 0xff0000,
                Chart.Transparent);

            // If the spline line gets below the lower zone line, color to area between the
            // lines blue (0000ff)
            ct3.addInterLineLayer(splineLayer_t3.getLine(0), lineLayer_t3.getLine(1),
                Chart.Transparent, 0xFFFFFF);

            // Add a custom CDML text at the bottom right of the plot area as the logo
            ct3.addText(in_width, 110,
                "<*block,valign=absmiddle*><*img=small_molecule.png*> <*block*>" +
                "<*font=Times New Roman Bold Italic,size=10,color=804040*>Control " +
                "Limit H<*/*>").setAlignment(Chart.BottomRight);

            ct3.addText(in_width, 180,
                "<*block,valign=absmiddle*><*img=small_molecule.png*> <*block*>" +
                "<*font=Times New Roman Bold Italic,size=10,color=804040*>Control " +
                "Limit L<*/*>").setAlignment(Chart.BottomRight);

            // Output the chart
            WebChartViewer2.Image = ct3.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            WebChartViewer2.ImageMap = ct3.getHTMLImageMap("", "",
                "title='Temperature at hour {x}: {value} C'");
            #endregion
        }


        private string outputToxls(DataTable input_dt,string title)
        {
            HSSFWorkbook hssfWorkBook_1 = new HSSFWorkbook();
            HSSFSheet sheet = (NPOI.HSSF.UserModel.HSSFSheet)hssfWorkBook_1.CreateSheet(title + "_output");
            IRow row; ICell cell;
            bool isOnly = false;
            int Row_Count = 0;
            int Cell_Count = 0;
                        
            row = sheet.CreateRow(Row_Count);
            string[] xls_title = new string[] { "Time", "Data", "kindName", "Measure", "target", "oven_assy_logkindid" };
            foreach (string _title in xls_title)
            {
                cell = row.CreateCell(Cell_Count);
                cell.SetCellValue(_title.ToString());
                Cell_Count++;
            }
            if (isOnly == false)
            {
                sheet.SetAutoFilter(CellRangeAddress.ValueOf(string.Format(@"A{0}:F{0}", Row_Count + 1)));//Fliter
                sheet.CreateFreezePane(Cell_Count, Row_Count + 1);//Freeze
                isOnly = true;
            }
            Row_Count += 1;

            foreach (DataRow dr in input_dt.Rows)
            {
                Cell_Count = 0;
                row = sheet.CreateRow(Row_Count);
                foreach (object item in dr.ItemArray)
                {
                    if (!string.IsNullOrEmpty(item.ToString()))
                    {
                        cell = row.CreateCell(Cell_Count);
                        cell.SetCellValue(item.ToString());
                        Cell_Count++;
                    }
                }
                Row_Count++;    
            }

            
            //Save File (\download\..)
            string path = Request.PhysicalApplicationPath + "download\\" + string.Format(@"{0}_{1}_Output.xls", title, DateTime.Now.ToString("D"));
            FileStream fs = new FileStream(path, FileMode.Create);            
            hssfWorkBook_1.Write(fs);
            
            fs.Close();
            fs.Dispose();
            return path;
        }

        protected void ibtnOutput_Click(object sender, ImageClickEventArgs e)
        {
            btnQuery_Click(null, null);
            string path = outputToxls(pdt, txtMachineID.Text.Trim().ToUpper());

            if (File.Exists(path))
            {
                try
                {
                    System.Net.WebClient wc = new System.Net.WebClient(); //呼叫 webclient 方式做檔案下載
                    byte[] xfile = null;
                    xfile = wc.DownloadData(path);
                    string xfileName = System.IO.Path.GetFileName(path);
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(xfileName));
                    HttpContext.Current.Response.ContentType = "application/octet-stream"; //二進位方式
                    //// 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                    HttpContext.Current.Response.BinaryWrite(xfile); //內容轉出作檔案下載
                    HttpContext.Current.Response.End();
                }
                catch (Exception ex)
                { Console.WriteLine(ex.ToString()); }

            }
        }
}
}