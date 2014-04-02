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
                App_Code.func fc = new App_Code.func();
                fc.checkRole(Page.Master);

                txtUser.Text = Request.QueryString["USER"];
                txtMachineID.Text = Request.QueryString["Machine_ID"];

                //callConsole("COM3 OV-135 1 9 1");

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
                ushort[] register_Process_2 = master.ReadHoldingRegisters(slaveID, fc.secondPressure, numOfPoints);
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
            App_Code.func func = new App_Code.func();
            DataRow dr = func.getDrOvenLocationFromMsdb(txtMachineID.Text.Trim());
            txtArea.Text = dr.ItemArray.Length > 0 ? dr["Area"].ToString() : null;
            txtOvenID.Text = dr.ItemArray.Length > 0 ? dr["Oven_ID"].ToString() : null;
            txt_TextChanged(null, null);
        }
        protected void txtBC_TextChanged(object sender, EventArgs e)
        {
            App_Code.func func = new App_Code.func();
            txtAdhesive.Text = func.getAdhesiveFromIntrack(txtBC.Text.Trim());
            txt_TextChanged(null, null);
        }        
        protected void txt_TextChanged(object sender, EventArgs e)
        {
            App_Code.func func = new App_Code.func();

            DataTable dt = new DataTable();
            dt = func.getDataFromMsdb(txtArea.Text.Trim(), txtPTN.Text.Trim(), txtAdhesive.Text.Trim());
            GridViewList.DataSource = dt;
            GridViewList.DataBind();
            dtGv = dt;
            showPage();

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

                //===============================================================================
                //取出對應該area、adhesive、bake_Program的Comport
                //===============================================================================
                string str = string.Format(@"Select Comport From Oven_Assy_Location Where Machine_ID=:machine_ID");
                object[] p = new object[] { Request.QueryString["Machine_ID"].ToUpper() };
                DataTable dt = ado.loadDataTable(str, p, "Oven_Assy_Location");
                if (dt.Rows.Count < 1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('can not find this oven 【{0}】');", Request.QueryString["Machine_ID"].ToUpper()), true); }


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

                //===============================================================================
                //取出Parameters from oven 
                //===============================================================================
                DataTable dt_ovenParameters = getOvenParameters(dt.Rows[0]["Comport"].ToString());


                //===============================================================================
                //比對PTN, 如果比對正確, ON機台 & Log(Temperature & Pressure)                
                //===============================================================================
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
                    #region Into log 
                    //intoLog(txtBC.Text.Trim().ToUpper());
                    try
                    {
                        App_Code.func fc = new App_Code.func();
                        DataTable mdt = fc.getFromIntrack(txtBC.Text.Trim().ToUpper());
                        if (mdt == null) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('mdt == null');"), true); }

                        string alertStr = string.Format(@"typename = {0}, ED_12NC = {1}, Diffusion = {2}, Package = {3}, Glue = {4}
                                                                  ", mdt.Rows[0]["TypeName"].ToString(), mdt.Rows[0]["ED_12NC"].ToString(),
                                                             mdt.Rows[0]["Diffusion"].ToString(), mdt.Rows[0]["package"].ToString(),
                                                             mdt.Rows[0]["Glue"].ToString());
                        //Response.Write(alertStr);

                        string insertStr = string.Format(@"Insert into oven_Assy_Status(ID,PMID,AREA,OVEN_ID,PTN,Batch_NO,
                                                                                Type_Name,NC_Code,Diffusion,Package,
                                                                                Bake_Time,In_Time,Est_Out_Time,Op_ID,Glue)
                                                   Values (:ID,:AREA,:OVEN_ID,:PTN,:Batch_NO,
                                                           :Type_Name,:NC_Code,:Diffusion,:Package,
                                                           :Bake_Time,:In_Time,:Est_Out_Time,:Op_ID,:Glue)");
                        object[] para = new object[] { DateTime.Now.ToString("yyyyMMdd_hhmmss"),pk_Area,txtOvenID.Text.Trim().ToUpper(),txtPTN.Text.Trim().ToUpper(),txtBC.Text.Trim().ToUpper(),
                                                       mdt.Rows[0]["TypeName"].ToString().ToUpper(), mdt.Rows[0]["ED_12NC"].ToString(),mdt.Rows[0]["Diffusion"].ToString(),mdt.Rows[0]["package"].ToString(),
                                                       dt_BakeTime.Rows[0]["bakeTime"].ToString(),DateTime.Now,DateTime.Now.AddMinutes(Convert.ToDouble(dt_BakeTime.Rows[0]["bakeTime"].ToString())),txtUser.Text.Trim(),mdt.Rows[0]["Glue"].ToString().ToUpper()};
                        string result = ado.dbNonQuery(insertStr, para).ToString();
                        if (result.Equals("SUCCESS")) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('insert success');", true); }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('fail');"), true);
                    }
                    #endregion
                    

                    //Comport/ MachineID/ LimitTemperature/ LimitPressure/ TotalTime(minute)
                    callConsole(string.Format(@"{0} {1} {2} {3} {4}", dt.Rows[0]["Comport"].ToString(),
                                                                      txtMachineID.Text.Trim().ToUpper(),
                                                                      dr_bakeTime["Temperature_1"].ToString(),
                                                                      dr_bakeTime["Pressure_1"].ToString(),
                                                                      dr_bakeTime["baketime"].ToString()));
                }
            }            
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

        #region modbus communication
        
        //private void modBusInitial()
        //{
        //    // create a new SerialPort object with default settings.
        //    serialPort = new SerialPort();

        //    // set the appropriate properties.
        //    serialPort.PortName = "COM3";   //for serial server the COM port connected to WISE COM3(RS-232)
        //    //serialPort.PortName = "COM5";   //for RS232 to USB the COM port connected to WISE COM5(RS-232)


        //    serialPort.BaudRate = 9600;
        //    serialPort.Parity = Parity.None;
        //    serialPort.StopBits = StopBits.One;
        //    serialPort.DataBits = 8;

        //    #region ddl data bind
        //    string[] arrFuc = new string[] { "ReadCoilStatus(0x01)", "ReadHoldingRegs(0x03)", "PresetSingleReg(0x06)" };
        //    foreach (string func in arrFuc)
        //    {
        //        ddlFunc.Items.Add(func);
        //    }
        //    ddlFunc.Items.Insert(0, new ListItem("請選擇"));
        //    #endregion

        //}
        //protected void ddlFunc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string[] arrTest = new string[] { };
        //    if (ddlFunc.Text.Equals("PresetSingleReg(0x06)"))
        //    {
        //        txtInput.Enabled = true;
        //        arrTest = new string[] { "選用爐訊號", "啟動按鈕enable並閃爍", "一般模式紅燈OFF", "警報蜂鳴器ON", "強制機台停止訊號" };
        //    }
        //    else if (ddlFunc.Text.Equals("ReadCoilStatus(0x01)"))
        //    {
        //        txtInput.Enabled = false;
        //        arrTest = new string[] { "作業中", "警報", "作業結束" };
        //    }
        //    else if (ddlFunc.Text.Equals("ReadHoldingRegs(0x03)"))
        //    {
        //        txtInput.Enabled = false;
        //        arrTest = new string[] { "最可靠的溫度", "CH1", "CH2", "壓力" };
        //    }

        //    ddlTest.Text = null;
        //    ddlTest.Items.Clear();
        //    foreach (string Test in arrTest) { ddlTest.Items.Add(Test); }
        //    ddlTest.Items.Insert(0, new ListItem("請選擇"));
        //}
        //protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    App_Code.FunctionCode fc = new App_Code.FunctionCode();

        //    //ushort param = 0;
        //    switch (ddlTest.Text)
        //    {
        //        case "作業中": param = fc.Working; break;
        //        case "警報": param = fc.Alarm; break;
        //        case "作業結束": param = fc.Terminate; break;
        //        case "最可靠的溫度": param = fc.Temperature; break;
        //        case "CH1": param = fc.CH1; break;
        //        case "CH2": param = fc.CH2; break;
        //        case "壓力": param = fc.Pressure; break;
        //        case "選用爐訊號": param = fc.Furnace; break;
        //        case "啟動按鈕enable並閃爍": param = fc.OnBtnTwinkle; break;
        //        case "一般模式紅燈OFF": param = fc.RedLightOff; break;
        //        case "警報蜂鳴器ON": param = fc.AlarmON; break;
        //        case "強制機台停止訊號": param = fc.StopMachine; break;
        //    }

        //    lblRequest.Text = param.ToString();
        //}
        //protected void btnOpen_Click(object sender, EventArgs e)
        //{
        //    serialPort.Open();
        //    // create Modbus RTU Master by the comport client
        //    //document->Modbus.Device.Namespace->ModbusSerialMaster Class->CreateRtu Method
        //    master = ModbusSerialMaster.CreateRtu(serialPort);

        //    btnOpen.Enabled = false;
        //    btnClose.Enabled = true;
        //    btnRequest.Enabled = true;
        //}
        //protected void btnClose_Click(object sender, EventArgs e)
        //{
        //    serialPort.Close();
        //    master.Dispose();

        //    btnOpen.Enabled = true;
        //    btnClose.Enabled = false;
        //    btnRequest.Enabled = false;
        //}
        //protected void chk_10_CheckedChanged(object sender, EventArgs e)
        //{
        //    chk_16.Checked = false;
        //    ddlTest.Enabled = false;
        //    if (chk_16.Checked == false && chk_10.Checked == false) { ddlTest.Enabled = true; }
        //}
        //protected void chk_16_CheckedChanged(object sender, EventArgs e)
        //{
        //    chk_10.Checked = false;
        //    ddlTest.Enabled = false;
        //    if (chk_16.Checked == false && chk_10.Checked == false) { ddlTest.Enabled = true; }
        //}
        //protected void btnRequest_Click(object sender, EventArgs e)
        //{
        //    App_Code.FunctionCode fc = new App_Code.FunctionCode();

        //    try
        //    {
        //        byte slaveID = 1;
        //        ushort startAddress = param;
        //        master.Transport.ReadTimeout = 300;
        //        ushort numOfPoints = 8;

        //        if (chk_10.Checked)
        //            startAddress = (ushort)Convert.ToInt32(txt_10.Text.Trim());
        //        else if (chk_16.Checked)
        //            startAddress = (ushort)Convert.ToInt32(txt_16.Text.Trim(), 16);

        //        switch (ddlFunc.Text)
        //        {
        //            #region ReadCoilStatus(0x01)
        //            case "ReadCoilStatus(0x01)":
        //                bool[] status = master.ReadCoils(slaveID, startAddress, numOfPoints);
        //                if (status[0] == true) { ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('ON。');", true); }
        //                else { ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('OFF。');", true); }

        //                for (int index = 0; index < status.Length; index++)
        //                    Console.WriteLine(string.Format("DO[{0}] = {1}", index, status[index]));
        //                break;
        //            #endregion
        //            #region ReadHoldingRegs(0x03)
        //            case "ReadHoldingRegs(0x03)":
        //                ushort[] register = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('十進位：{0}{1}十六進位：{2}。');", register[0].ToString(), "\\n", Convert.ToString(Convert.ToInt32(register[0].ToString()), 16)), true);

        //                float[] floatData2 = new float[register.Length / 2];
        //                //Buffer.BlockCopy(register, 0, floatData2, 0, register.Length * 2);
        //                for (int index = 0; index < floatData2.Length; index++)
        //                    Console.WriteLine(string.Format("AO[{0}] = {1}", index, floatData2[index]));
        //                break;
        //            #endregion
        //            #region PresetSingleReg(0x06)
        //            case "PresetSingleReg(0x06)":
        //                if (string.IsNullOrEmpty(txtInput.Text)) { ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('Please text the input value.。');", true); }
        //                else
        //                {
        //                    master.WriteSingleRegister(slaveID, (ushort)startAddress, (ushort)Convert.ToInt32(txtInput.Text.ToString()));//write
        //                    ushort[] register_read = master.ReadHoldingRegisters(slaveID, (ushort)startAddress, numOfPoints);//read
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('十進位：{0}{1}十六進位：{2}。');", register_read[0].ToString(), "\\n", Convert.ToString(Convert.ToInt32(register_read[0].ToString()), 16)), true);

        //                    //MessageBox.Show(register_read[0].ToString());  
        //                }
        //                break;
        //            #endregion
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        //Connection exception
        //        //No response from server.
        //        //The server maybe close the com port, or response timeout.
        //        if (exception.Source.Equals("System"))
        //            Console.WriteLine(exception.Message);

        //        //The server return error code.
        //        //You can get the function code and exception code.
        //        if (exception.Source.Equals("nModbusPC"))
        //        {
        //            string str = exception.Message;
        //            int FunctionCode;
        //            string ExceptionCode;

        //            str = str.Remove(0, str.IndexOf("\r\n") + 17);
        //            FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n")));
        //            Console.WriteLine("Function Code: " + FunctionCode.ToString("X"));

        //            str = str.Remove(0, str.IndexOf("\r\n") + 17);
        //            ExceptionCode = str.Remove(str.IndexOf("-"));
        //            switch (ExceptionCode.Trim())
        //            {
        //                case "1":
        //                    Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal function!");
        //                    break;
        //                case "2":
        //                    Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data address!");
        //                    break;
        //                case "3":
        //                    Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data value!");
        //                    break;
        //                case "4":
        //                    Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Slave device failure!");
        //                    break;
        //            }

        //            /*
        //               //Modbus exception codes definition
                            
        //               * Code   * Name                                      * Meaning
        //                 01       ILLEGAL FUNCTION                            The function code received in the query is not an allowable action for the server.
                         
        //                 02       ILLEGAL DATA ADDRESS                        The data addrdss received in the query is not an allowable address for the server.
                         
        //                 03       ILLEGAL DATA VALUE                          A value contained in the query data field is not an allowable value for the server.
                           
        //                 04       SLAVE DEVICE FAILURE                        An unrecoverable error occurred while the server attempting to perform the requested action.
                             
        //                 05       ACKNOWLEDGE                                 This response is returned to prevent a timeout error from occurring in the client (or master)
        //                                                                      when the server (or slave) needs a long duration of time to process accepted request.
                          
        //                 06       SLAVE DEVICE BUSY                           The server (or slave) is engaged in processing a long–duration program command , and the
        //                                                                      client (or master) should retransmit the message later when the server (or slave) is free.
                             
        //                 08       MEMORY PARITY ERROR                         The server (or slave) attempted to read record file, but detected a parity error in the memory.
                             
        //                 0A       GATEWAY PATH UNAVAILABLE                    The gateway is misconfigured or overloaded.
                             
        //                 0B       GATEWAY TARGET DEVICE FAILED TO RESPOND     No response was obtained from the target device. Usually means that the device is not present on the network.                         
        //             */
        //        }
        //    }//end catch
        //}
        #endregion
        
}
}