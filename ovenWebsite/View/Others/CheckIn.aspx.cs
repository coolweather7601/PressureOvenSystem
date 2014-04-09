using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//step1. reference nmodbuspc.dll, and using the namespaces
using Modbus.Device;      //for modbus master
using System.IO.Ports;    //for controlling serial ports
using System.Net.Sockets;
using System.Data;

//call console
using System.Diagnostics;
using System.Collections;
using System.Xml;

namespace nModBusWeb
{
    public partial class CheckIn : System.Web.UI.Page
    {
        static private SerialPort serialPort;
        static private ModbusSerialMaster master;
        static private DataTable dtGv;

        static private string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
         

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //push test
                App_Code.func fc = new App_Code.func();
                fc.checkRole(Page.Master);
                 
                txtUser.Text = Request.QueryString["USER"];
                txtMachineID.Text = Request.QueryString["Machine_ID"];

                //callConsole("COM10 OV-135 1 9 1");
                //callWebService("COM10 OV-135 1-0-2-110-7|2-0-2-126-7");
                //intoLog("6420DCC108");                
            }
        }

        /// <summary>
        /// call console auto scan oven param(ComPort/ Machine_ID/ TemperatureLimit/ PressureLimit/ TotalTime )
        /// </summary>
        private void callConsole(string parmes)
        {
            Process w = new Process();
            //指定 調用程序的路徑
            w.StartInfo.FileName = Request.PhysicalApplicationPath + @"ovenWin\ovenWin\bin\debug\ovenWin.exe";
            w.StartInfo.UseShellExecute = false;
            //不顯示執行窗口
            w.StartInfo.CreateNoWindow = false;

            //指定 調用程序的參數
            w.StartInfo.Arguments = parmes;
            w.Start();
        }

        /// <summary>
        /// call webservice auto scan oven param(ComPort/ Machine_ID/ TemperatureLimit/ PressureLimit/ TotalTime )
        /// </summary>
        private void callWebService(string parmes)
        {
            ovenWebservice.Service sfc = new ovenWebservice.Service();
            string ovenWinPath = sfc.callConsole(parmes);
        }

        /// <summary>
        ///get parameters from oven by modbu, return datatable
        /// </summary>
        private DataTable getOvenParameters(string comport)
        {
            DataTable dt = new DataTable();

            try
            {
                // create a new SerialPort object with default settings.
                serialPort = new SerialPort();

                // set the appropriate properties.
                serialPort.PortName = comport;

                serialPort.BaudRate = 9600;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.DataBits = 8;

                serialPort.Open();
                // create Modbus RTU Master by the comport client
                //document->Modbus.Device.Namespace->ModbusSerialMaster Class->CreateRtu Method
                master = ModbusSerialMaster.CreateRtu(serialPort);

                byte slaveID = 1;
                master.Transport.ReadTimeout = 300;
                ushort numOfPoints = 8;

                App_Code.FunctionCode fc = new App_Code.FunctionCode();

                //===============================================================================
                //First Parameters
                //===============================================================================
                ushort[] register_Process_1 = master.ReadHoldingRegisters(slaveID, fc.firstProcess, numOfPoints);
                ushort[] register_Hour_1 = master.ReadHoldingRegisters(slaveID, fc.firstHour, numOfPoints);
                ushort[] register_Min_1 = master.ReadHoldingRegisters(slaveID, fc.firstMin, numOfPoints);
                ushort[] register_Temperature_1 = master.ReadHoldingRegisters(slaveID, fc.firstTemperature, numOfPoints);
                ushort[] register_Pressure_1 = master.ReadHoldingRegisters(slaveID, fc.firstPressure, numOfPoints);

                //===============================================================================
                //Second Parameters
                //===============================================================================
                ushort[] register_Process_2 = master.ReadHoldingRegisters(slaveID, fc.secondProcess, numOfPoints);
                ushort[] register_Hour_2 = master.ReadHoldingRegisters(slaveID, fc.secondHour, numOfPoints);
                ushort[] register_Min_2 = master.ReadHoldingRegisters(slaveID, fc.secondMin, numOfPoints);
                ushort[] register_Temperature_2 = master.ReadHoldingRegisters(slaveID, fc.secondTemperature, numOfPoints);
                ushort[] register_Pressure_2 = master.ReadHoldingRegisters(slaveID, fc.secondPressure, numOfPoints);


                //===============================================================================
                //Create new DataTable
                //===============================================================================
                dt.Columns.Add("Process_1");
                dt.Columns.Add("Hour_1");
                dt.Columns.Add("Min_1");
                dt.Columns.Add("Temperature_1");
                dt.Columns.Add("Pressure_1");
                dt.Columns.Add("Process_2");
                dt.Columns.Add("Hour_2");
                dt.Columns.Add("Min_2");
                dt.Columns.Add("Temperature_2");
                dt.Columns.Add("Pressure_2");
                DataRow dr = dt.NewRow();
                dr[0] = register_Process_1[0].ToString();
                dr[1] = register_Hour_1[0].ToString();
                dr[2] = register_Min_1[0].ToString();
                dr[3] = register_Temperature_1[0].ToString();
                dr[4] = register_Pressure_1[0].ToString();

                dr[5] = register_Process_2[0].ToString();
                dr[6] = register_Hour_2[0].ToString();
                dr[7] = register_Min_2[0].ToString();
                dr[8] = register_Temperature_2[0].ToString();
                dr[9] = register_Pressure_2[0].ToString();

                dt.Rows.Add(dr);
                serialPort.Close();
            }
            catch (Exception ex) { serialPort.Close(); Console.WriteLine(ex.ToString()); }
            return dt;
        }

        /// <summary>
        ///get inofrmation of batch card from intrack
        /// </summary>
        public void intoLog(string batchNo)
        {
            try
            {
                App_Code.func fc = new App_Code.func();
                DataTable mdt = fc.getFromIntrack(batchNo);
                if (mdt == null) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('mdt == null');"), true); }
                string alertStr = string.Format(@"typename = {0}, ED_12NC = {1}, Diffusion = {2}, Package = {3}, Glue = {4}
                                                                  ", mdt.Rows[0]["TypeName"].ToString(), mdt.Rows[0]["ED_12NC"].ToString(),
                                                     mdt.Rows[0]["Diffusion"].ToString(), mdt.Rows[0]["package"].ToString(),
                                                     mdt.Rows[0]["Glue"].ToString());
                Response.Write(alertStr);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('fail');"), true);
            }
        }

        protected void txtMachineID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                App_Code.func func = new App_Code.func();
                DataRow dr = func.getDrOvenLocationFromMsdb(txtMachineID.Text.Trim().ToUpper());
                txtArea.Text = dr.ItemArray.Length > 0 ? dr["Area"].ToString() : null;
                txtOvenID.Text = dr.ItemArray.Length > 0 ? dr["Oven_ID"].ToString() : null;
                txt_TextChanged(null, null);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        protected void txtBC_TextChanged(object sender, EventArgs e)
        {
            App_Code.func func = new App_Code.func();
            txtAdhesive.Text = func.getAdhesiveFromIntrack(txtBC.Text.Trim().ToUpper());
            txt_TextChanged(null, null);
        }        
        protected void txt_TextChanged(object sender, EventArgs e)
        {
            App_Code.func func = new App_Code.func();

            DataTable dt = new DataTable();
            dt = func.getDataFromMsdb(txtArea.Text.Trim().ToUpper(), txtPTN.Text.Trim().ToUpper(), txtAdhesive.Text.Trim().ToUpper());
            GridViewList.DataSource = dt;
            GridViewList.DataBind();
            dtGv = dt;
            showPage();

            //Check isNull
            if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtMachineID.Text) && !string.IsNullOrEmpty(txtBC.Text) &&
               !string.IsNullOrEmpty(txtArea.Text) && !string.IsNullOrEmpty(txtPTN.Text) && !string.IsNullOrEmpty(txtAdhesive.Text) &&
               !string.IsNullOrEmpty(txtOvenID.Text))
            {
                GridViewList.Columns[5].Visible = true;
            }
            else
            {
                GridViewList.Columns[5].Visible = false;
            }
        }
        protected void GridViewList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewList.PageIndex = e.NewPageIndex;
            GridViewList.DataSource = dtGv;
            GridViewList.DataBind();
            showPage();
        }
        protected void GridViewList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);

            if (e.CommandName.Equals("Run"))
            {
                
                    string[] argument = e.CommandArgument.ToString().Split(',');
                    string pk_Area = argument[0];
                    string pk_Adhesive = argument[1];
                    string pk_BakeProgram = argument[2];

                    //============================================================================
                    //Check oven status 
                    //============================================================================
                    string ovenStr = string.Format(@"Select * From Oven_Assy_Status Where PMID = '{0}'", txtMachineID.Text.Trim().ToUpper());
                    DataTable dtStatus = ado.loadDataTable(ovenStr, null, "Oven_Assy_Status");
                    if (dtStatus.Rows.Count > 0) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('this oven is working, please check again!');", true); }
                    else
                    {
                        try
                        {
                            //===============================================================================
                            //取出對應該area、adhesive、bake_Program的Comport
                            //===============================================================================
                            string str = string.Format(@"Select Comport From Oven_Assy_Location Where Machine_ID=:machine_ID");
                            object[] p = new object[] { txtMachineID.Text.Trim().ToUpper() };
                            DataTable dt = ado.loadDataTable(str, p, "Oven_Assy_Location");
                            if (dt.Rows.Count < 1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('can not find this oven 【{0}】');", txtMachineID.Text.Trim().ToUpper()), true); }
                            
                            //===============================================================================
                            //取出 oven_assy_fe_baketime parameters
                            //===============================================================================
                            string str_BakeTime = string.Format(@"Select Process_1, Hour_1, Min_1, Temperature_1, Pressure_1,
                                                             Process_2, Hour_2, Min_2, Temperature_2, Pressure_2,baketime
                                                      From   OVEN_ASSY_FE_BakeTime
                                                      Where  Area like '%{0}%'
                                                             And Adhesive like '%{1}%'
                                                             And Bake_Program like '%{2}%'", pk_Area.ToUpper(), pk_Adhesive.ToUpper(), pk_BakeProgram.ToUpper());
                            DataTable dt_BakeTime = ado.loadDataTable(str_BakeTime, null, "Oven_Assy_Fe_BakeTime");
                            if (dt_BakeTime.Rows.Count < 1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('can not read this PTN 【{0}】parameters');", txtPTN.Text.Trim().ToUpper()), true); }

                            //===============================================================================
                            //取出Parameters from oven 
                            //===============================================================================
                            DataTable dt_ovenParameters = getOvenParameters(dt.Rows[0]["Comport"].ToString());
                            if (dt_ovenParameters.Rows.Count < 1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('can not read this oven 【{0}】parameters, Comport:{1}');", txtMachineID.Text.Trim().ToUpper(), dt.Rows[0]["Comport"].ToString()), true); }

                            //===============================================================================
                            //比對PTN, 如果比對正確, ON機台 & Log(Temperature & Pressure)                
                            //===============================================================================
                            #region check

                            DataRow dr_bakeTime = dt_BakeTime.Rows[0];
                            DataRow dr_ovenParameters = dt_ovenParameters.Rows[0];
                            string[] arrField = new string[]{"Process_1", "Hour_1", "Min_1", "Temperature_1","Pressure_1",
                                                             "Process_2", "Hour_2", "Min_2", "Temperature_2","Pressure_2"};
                            bool check = true;
                            foreach (string field in arrField)
                            {
                                if (dr_bakeTime[field].ToString() != dr_ovenParameters[field].ToString())
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('PTN recipe can not map to the Oven parameters');", true);
                                    check = false;
                                    break;
                                }
                            }
                            if (check)
                            {
                                //============================================================================
                                //Write to Oven_Assy_Status Log
                                //============================================================================

                                App_Code.func fc = new App_Code.func();
                                DataTable mdt = fc.getFromIntrack(txtBC.Text.Trim().ToUpper());
                                if (mdt == null) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('mdt == null');"), true); }

                                string alertStr = string.Format(@"typename = {0}, ED_12NC = {1}, Diffusion = {2}, Package = {3}, Glue = {4}",
                                                                     mdt.Rows[0]["TypeName"].ToString(), mdt.Rows[0]["ED_12NC"].ToString(),
                                                                     mdt.Rows[0]["Diffusion"].ToString(), mdt.Rows[0]["package"].ToString(),
                                                                     mdt.Rows[0]["Glue"].ToString());
                                //Response.Write(alertStr);

                                string insertStr = string.Format(@"Insert into oven_Assy_Status(ID,PMID,AREA,OVEN_ID,PTN,Batch_NO,
                                                                                        Type_Name,NC_Code,Diffusion,Package,
                                                                                        Bake_Time,In_Time,Est_Out_Time,Op_ID,Glue)
                                               Values (:ID,:PMID,:AREA,:OVEN_ID,:PTN,:Batch_NO,
                                                       :Type_Name,:NC_Code,:Diffusion,:Package,
                                                       :Bake_Time,:In_Time,:Est_Out_Time,:Op_ID,:Glue)");
                                object[] para = new object[] { DateTime.Now.ToString("yyyyMMdd_hhmmss"),txtMachineID.Text.Trim().ToUpper(),pk_Area,txtOvenID.Text.Trim().ToUpper(),txtPTN.Text.Trim().ToUpper(),txtBC.Text.Trim().ToUpper(),
                                                       mdt.Rows[0]["TypeName"].ToString().ToUpper(), mdt.Rows[0]["ED_12NC"].ToString(),mdt.Rows[0]["Diffusion"].ToString(),mdt.Rows[0]["package"].ToString(),
                                                       dt_BakeTime.Rows[0]["bakeTime"].ToString(),DateTime.Now,DateTime.Now.AddMinutes(Convert.ToDouble(dt_BakeTime.Rows[0]["bakeTime"].ToString())),txtUser.Text.Trim(),mdt.Rows[0]["Glue"].ToString().ToUpper()};
                                string result = ado.dbNonQuery(insertStr, para).ToString();
                                if (result.Equals("SUCCESS")) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('Success, the oven is working.'); window.location.href='{0}';", System.Web.Configuration.WebConfigurationManager.AppSettings["msWebSite"].ToString()), true); }


                                //============================================================================
                                //Comport/ MachineID/ LimitTemperature/ LimitPressure/ TotalTime(minute)
                                //============================================================================
                                //Process_1, Hour_1, Min_1, Temperature_1, Pressure_1,
                                //Process_2, Hour_2, Min_2, Temperature_2, Pressure_2
                                callWebService(string.Format(@"{0} {1} {2}|{3}", dt.Rows[0]["Comport"].ToString(),
                                                                             txtMachineID.Text.Trim().ToUpper(),
                                                                             string.Format(@"{0}-{1}-{2}-{3}-{4}", dr_bakeTime["Process_1"].ToString(), dr_bakeTime["Hour_1"].ToString(), dr_bakeTime["Min_1"].ToString(), dr_bakeTime["Temperature_1"].ToString(), dr_bakeTime["Pressure_1"].ToString()),
                                                                             string.Format(@"{0}-{1}-{2}-{3}-{4}", dr_bakeTime["Process_2"].ToString(), dr_bakeTime["Hour_2"].ToString(), dr_bakeTime["Min_2"].ToString(), dr_bakeTime["Temperature_2"].ToString(), dr_bakeTime["Pressure_2"].ToString())
                                                                             ));



                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());                            
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('the process fail, please inform the engineer.');"), true);
                        }
                    }
            }            
        }
        protected void GridViewList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
//            if (e.Row.RowType == DataControlRowType.DataRow)
//            {
//                App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);

//                string pk_Area = GridViewList.DataKeys[e.Row.RowIndex].Values[0].ToString();
//                string pk_Adhesive = GridViewList.DataKeys[e.Row.RowIndex].Values[1].ToString();
//                string pk_bakeProgram = GridViewList.DataKeys[e.Row.RowIndex].Values[2].ToString();

//                string str_BakeTime = string.Format(@"Select Process_1, Hour_1, Min_1, Temperature_1, Pressure_1,
//                                                             Process_2, Hour_2, Min_2, Temperature_2, Pressure_2,baketime
//                                                      From   OVEN_ASSY_FE_BakeTime
//                                                      Where  Area like '%{0}%'
//                                                             And Adhesive like '%{1}%'
//                                                             And Bake_Program like '%{2}%'", pk_Area.ToUpper(), pk_Adhesive.ToUpper(), pk_BakeProgram.ToUpper());
//                DataTable dt_BakeTime = ado.loadDataTable(str_BakeTime, null, "Oven_Assy_Fe_BakeTime");

//                ImageButton btn = (ImageButton)e.Row.FindControl("btn2");
//                btn.ImageUrl = "";
//            }
        }
        protected void lbnFirst_Click(object sender, EventArgs e)
        {
            int num = 0;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridViewList_PageIndexChanging(null, ea);
        }
        protected void lbnPrev_Click(object sender, EventArgs e)
        {
            int num = GridViewList.PageIndex - 1;
            
            if (num >= 0)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewList_PageIndexChanging(null, ea);
            }
        }
        protected void lbnNext_Click(object sender, EventArgs e)
        {
            int num = GridViewList.PageIndex + 1;

            if (num < GridViewList.PageCount)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewList_PageIndexChanging(null, ea);
            }
        }
        protected void lbnLast_Click(object sender, EventArgs e)
        {
            int num = GridViewList.PageCount - 1;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridViewList_PageIndexChanging(null, ea);
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string numPage = ((TextBox)GridViewList.BottomPagerRow.FindControl("txtSizePage")).Text.ToString();
                if (!string.IsNullOrEmpty(numPage))
                {
                    GridViewList.PageSize = Convert.ToInt32(numPage);
                }

                TextBox pageNum = ((TextBox)GridViewList.BottomPagerRow.FindControl("inPageNum"));
                string goPage = pageNum.Text.ToString();
                if (!string.IsNullOrEmpty(goPage))
                {
                    int num = Convert.ToInt32(goPage) - 1;
                    if (num >= 0)
                    {
                        GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                        GridViewList_PageIndexChanging(null, ea);
                        ((TextBox)GridViewList.BottomPagerRow.FindControl("inPageNum")).Text = null;
                    }
                }

                GridViewList.DataSource = dtGv;
                GridViewList.DataBind();
                showPage();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void showPage()
        {
            try
            {
                TextBox txtPage = (TextBox)GridViewList.BottomPagerRow.FindControl("txtSizePage");
                Label lblCount = (Label)GridViewList.BottomPagerRow.FindControl("lblTotalCount");
                Label lblPage = (Label)GridViewList.BottomPagerRow.FindControl("lblPage");
                Label lblbTotal = (Label)GridViewList.BottomPagerRow.FindControl("lblTotalPage");

                txtPage.Text = GridViewList.PageSize.ToString();
                lblCount.Text = dtGv.Rows.Count.ToString();
                lblPage.Text = (GridViewList.PageIndex + 1).ToString();
                lblbTotal.Text = GridViewList.PageCount.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        
}
}