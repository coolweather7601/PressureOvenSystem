using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Xml;

namespace nModBusWeb
{
    /// <summary>
    /// Summary description for PoCeckin
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PoCeckin : System.Web.Services.WebService
    {

        [WebMethod (Description = "Please input the PMID and BatchNo to active the monitor program of pressured-oven.")]
        public XmlDocument PoCeckinService(string PMID, string BatchNo)
        {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);
            App_Code.func func = new App_Code.func();

            string pk_Area="", pk_Adhesive, pk_BakeProgram, comport = "";
            List<string> arrStr = new List<string>();
            DataTable dt_BakeTime = new DataTable();
            XmlDocument xmlDocumentObject = new XmlDocument();

            try
            {
                #region get Area,Adhesive,BakeProgram

                DataRow dr = func.getDrOvenLocationFromMsdb(PMID.ToUpper());
                if (dr != null)
                {
                    pk_Area = dr.ItemArray.Length > 0 ? dr["Area"].ToString() : null;
                }
                pk_Adhesive = func.getAdhesiveFromIntrack(BatchNo.Trim().ToUpper());


                DataTable dtGv = func.getDataFromMsdb(pk_Area, pk_Adhesive.ToUpper());
                if (dtGv.Rows.Count < 1)
                {
                    xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                                <Code>-1</Code>
                                                                <Msg>You need input the right information in this process.</Msg>
                                                              </ResultMessage>"));
                    return xmlDocumentObject;
                }
                else { pk_Area = dtGv.Rows[0]["Area"].ToString(); pk_Adhesive = dtGv.Rows[0]["Adhesive"].ToString(); pk_BakeProgram = dtGv.Rows[0]["Bake_Program"].ToString(); }

                #endregion
                                
                #region call monitor console service & insert log into database
                //============================================================================
                //Check oven status 
                //============================================================================
                string ovenStr = string.Format(@"Select * From Oven_Assy_Status Where PMID = '{0}'", PMID.ToUpper());
                DataTable dtStatus = ado.loadDataTable(ovenStr, null, "Oven_Assy_Status");
                if (dtStatus.Rows.Count > 0)
                {
                    xmlDocumentObject.LoadXml(@"<ResultMessage>  
                                                    <Code>-1</Code>
                                                    <Msg>This oven is working, please check again!</Msg>
                                                </ResultMessage>  ");
                    return xmlDocumentObject;
                }
                else
                {
                    //===============================================================================
                    //取出對應該area、adhesive、bake_Program的Comport
                    //===============================================================================
                    string str = string.Format(@"Select Comport From Oven_Assy_Location Where Machine_ID=:machine_ID");
                    object[] p = new object[] { PMID.ToUpper() };
                    DataTable dt = ado.loadDataTable(str, p, "Oven_Assy_Location");
                    if (dt.Rows.Count < 1)
                    {
                        xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                                    <Code>-1</Code>
                                                                    <Msg>Can not find this oven 【{0}】</Msg>
                                                                  </ResultMessage>  ", PMID.ToUpper()));
                        return xmlDocumentObject; 
                    }
                    else { comport = dt.Rows[0]["comport"].ToString(); }

                    //===============================================================================
                    //取出 oven_assy_fe_baketime parameters
                    //===============================================================================
                    string str_BakeTime = string.Format(@"Select Process_1, Hour_1, Min_1, Temperature_1, Pressure_1,
                                                     Process_2, Hour_2, Min_2, Temperature_2, Pressure_2,baketime
                                              From   OVEN_ASSY_FE_BakeTime
                                              Where  Area like '%{0}%'
                                                     And Adhesive like '%{1}%'
                                                     And Bake_Program like '%{2}%'", pk_Area.ToUpper(), pk_Adhesive.ToUpper(), pk_BakeProgram.ToUpper());
                    dt_BakeTime = ado.loadDataTable(str_BakeTime, null, "Oven_Assy_Fe_BakeTime");
                    if (dt_BakeTime.Rows.Count < 1)
                    {
                        xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                                    <Code>-1</Code>
                                                                    <Msg>Can not read this PTN 【{0}】parameters'.</Msg>
                                                                  </ResultMessage>", pk_BakeProgram.ToUpper()));
                        return xmlDocumentObject;
                    }
                }
#region into log sql_transaction
//                //============================================================================
//                //into log sql_transaction 
//                //============================================================================
//                List<string> lst_BC;
//                foreach (string bc in lst_BC)
//                {
//                    DataTable mdt = func.getFromIntrack(bc.Trim().ToUpper());
//                    if (mdt == null) { ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('mdt == null');"), true); }

//                    string alertStr = string.Format(@"typename = {0}, ED_12NC = {1}, Diffusion = {2}, Package = {3}, Glue = {4}",
//                                                         mdt.Rows[0]["TypeName"].ToString(), mdt.Rows[0]["ED_12NC"].ToString(),
//                                                         mdt.Rows[0]["Diffusion"].ToString(), mdt.Rows[0]["package"].ToString(),
//                                                         mdt.Rows[0]["Glue"].ToString());

//                    string insertStr = string.Format(@"Insert into oven_Assy_Status(ID,PMID,AREA,OVEN_ID,PTN,Batch_NO,
//                                                                                    Type_Name,NC_Code,Diffusion,Package,
//                                                                                    Bake_Time,In_Time,Est_Out_Time,Op_ID,Glue)
//                                           Values ('{0}','{1}','{2}','{3}','{4}','{5}',
//                                                   '{6}','{7}','{8}','{9}',
//                                                   '{10}','{11}','{12}','{13}')", DateTime.Now.ToString("yyyyMMdd_hhmmss"), txtMachineID.Text.Trim().ToUpper(), pk_Area, txtOvenID.Text.Trim().ToUpper(), txtPTN.Text.Trim().ToUpper(), bc.Trim().ToUpper(),
//                                                                                  mdt.Rows[0]["TypeName"].ToString().ToUpper(), mdt.Rows[0]["ED_12NC"].ToString(), mdt.Rows[0]["Diffusion"].ToString(), mdt.Rows[0]["package"].ToString(),
//                                                                                  dt_BakeTime.Rows[0]["bakeTime"].ToString(), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToDouble(dt_BakeTime.Rows[0]["bakeTime"].ToString())), txtUser.Text.Trim(), mdt.Rows[0]["Glue"].ToString().ToUpper());
//                    arrStr.Add(insertStr);
//                }
#endregion
                //===============================================================================
                //比對PTN, 如果比對正確, ON機台 & Log(Temperature & Pressure)                
                //===============================================================================

                //取出Parameters from oven                             
                DataTable dt_ovenParameters = func.getOvenParameters(comport);
                if (dt_ovenParameters.Rows.Count < 1)
                {
                    xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                                <Code>-1</Code>
                                                                <Msg>Can not read this oven 【{0}】parameters, Comport:{1}.</Msg>
                                                              </ResultMessage>", PMID.ToUpper(), comport));
                    return xmlDocumentObject;
                }

                DataRow dr_bakeTime = dt_BakeTime.Rows[0];
                DataRow dr_ovenParameters = dt_ovenParameters.Rows[0];
                string[] arrField = new string[]{"Process_1", "Hour_1", "Min_1", "Temperature_1","Pressure_1",
                                                 "Process_2", "Hour_2", "Min_2", "Temperature_2","Pressure_2"};
                
                foreach (string field in arrField)
                {
                    if (dr_bakeTime[field].ToString() != dr_ovenParameters[field].ToString())
                    {
                        xmlDocumentObject.LoadXml(@"<ResultMessage>  
                                                        <Code>-1</Code>
                                                        <Msg>PTN recipe can not map to the Oven parameters.</Msg>
                                                    </ResultMessage>");
                        return xmlDocumentObject;
                    }
                }
                
                //============================================================================
                //Write to Oven_Assy_Status Log
                //============================================================================
                string reStr = ado.SQL_transaction(arrStr, conn);
                if (reStr.ToUpper().Contains("SUCCESS"))
                {
                    //============================================================================
                    //Comport/ MachineID/ LimitTemperature/ LimitPressure/ TotalTime(minute)
                    //============================================================================
                    //Process_1, Hour_1, Min_1, Temperature_1, Pressure_1,
                    //Process_2, Hour_2, Min_2, Temperature_2, Pressure_2
                    func.callWebService(string.Format(@"{0} {1} {2} {3}|{4} {5}", comport,
                                                                                  PMID.ToUpper(),
                                                                                  dt_BakeTime.Rows[0]["bakeTime"].ToString(),
                                                                                  string.Format(@"{0}-{1}-{2}-{3}-{4}", dr_bakeTime["Process_1"].ToString(), dr_bakeTime["Hour_1"].ToString(), dr_bakeTime["Min_1"].ToString(), dr_bakeTime["Temperature_1"].ToString(), dr_bakeTime["Pressure_1"].ToString()),
                                                                                  string.Format(@"{0}-{1}-{2}-{3}-{4}", dr_bakeTime["Process_2"].ToString(), dr_bakeTime["Hour_2"].ToString(), dr_bakeTime["Min_2"].ToString(), dr_bakeTime["Temperature_2"].ToString(), dr_bakeTime["Pressure_2"].ToString()),
                                                                                  BatchNo.ToUpper()
                                                                                  ));

                    xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                                <Code>0</Code>
                                                                <Msg>Success, the oven is working.</Msg>
                                                              </ResultMessage>"));
                    return xmlDocumentObject;
                }
                else
                {
                    xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                                <Code>-1</Code>
                                                                <Msg>Inserting log and activing monitor application have failed, please check.</Msg>
                                                              </ResultMessage>"));
                    return xmlDocumentObject;
                }                
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                            <Code>-1</Code>
                                                            <Msg>Process Exception!!Please inform the engineer.</Msg>
                                                          </ResultMessage>"));
                return xmlDocumentObject;
            }
        }

        [WebMethod (Description = "Please input the PMID、Area and batchNo to get PTN")]
        public string getPTN(string Area,string PMID, string BatchNo)
        {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);
            App_Code.func func = new App_Code.func();

            DataRow dr = func.getDrOvenLocationFromMsdb(PMID.ToUpper());
            if (dr != null)
            {
                Area = dr.ItemArray.Length > 0 ? dr["Area"].ToString() : null;
            }
            string Adhesive = func.getAdhesiveFromIntrack(BatchNo.Trim().ToUpper());


            DataTable dtGv = func.getDataFromMsdb(Area, Adhesive.ToUpper());
            if (dtGv.Rows.Count < 1) { return ""; }
            return dtGv.Rows[0]["Bake_Program"].ToString();
        }

        [WebMethod(Description = "Please input the PMID to change the mode (auto or none-auto)")]
        public XmlDocument getOvenMode(string PMID)
        {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, conn);
            App_Code.func func = new App_Code.func();

            //===============================================================================
            //取出對應該area、adhesive、bake_Program的Comport
            //===============================================================================
            XmlDocument xmlDocumentObject = new XmlDocument();
            string comport = "";
            string str = string.Format(@"Select Comport From Oven_Assy_Location Where Machine_ID=:machine_ID");
            object[] p = new object[] { PMID.ToUpper() };
            DataTable dt = ado.loadDataTable(str, p, "Oven_Assy_Location");
            if (dt.Rows.Count < 1)
            {
                xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                            <Code>-1</Code>
                                                            <Msg>Can not find this oven 【{0}】</Msg>
                                                          </ResultMessage>  ", PMID.ToUpper()));
                return xmlDocumentObject;
            }
            else { comport = dt.Rows[0]["comport"].ToString(); }



            //===============================================================================
            //return oven Mode
            //===============================================================================
            if (func.changeOvenMode(comport).Contains("error"))
            {
                xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                            <Code>-1</Code>
                                                            <msg>{0}</msg>
                                                          </ResultMessage>", func.changeOvenMode(comport)));
            }
            else
            {
                xmlDocumentObject.LoadXml(string.Format(@"<ResultMessage>  
                                                            <Code>0</Code>
                                                            <mode>{0}</mode>
                                                          </ResultMessage>", func.changeOvenMode(comport)));
            }
            return xmlDocumentObject;

        }

    }

}