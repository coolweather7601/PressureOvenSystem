using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace nModBusWeb.App_Code
{
    public class func
    {
        public static string HrConn = System.Configuration.ConfigurationManager.ConnectionStrings["HR"].ToString();//Hr    
        public static string connMESPROD = System.Configuration.ConfigurationManager.ConnectionStrings["MESPROD"].ToString();
                

        /// <summary>
        /// get Adhesive From Intrack
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns>Adhesive</returns>
        public string getAdhesiveFromIntrack(string batchNo)
        {            
            apkvm1013.Service sfc = new apkvm1013.Service();            
            string Adhesive = sfc.getCompleteLotData(batchNo.ToUpper()).Adhesive.ToString();
            return Adhesive;
        }

        /// <summary>
        /// get OvenArea From Msdb
        /// </summary>
        /// <param name="MachineID"></param>
        /// <returns>DataRow</returns>
        public DataRow getDrOvenLocationFromMsdb(string MachineID)
        {
            string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();//OVEN
            AdoDbConn ado = new AdoDbConn(AdoDbConn.AdoDbType.Oracle, Conn);
            DataTable dt = new DataTable();
            string str = string.Format(@"Select Area,Oven_ID,Machine_ID 
                                         From spare_part.OVEN_ASSY_LOCATION
                                         Where Machine_ID = '{0}'", MachineID);
            dt = ado.loadDataTable(str, null, "spare_part.OVEN_ASSY_LOCATION");

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// get BakeTime From MS database, if null return string.empty
        /// </summary>
        /// <param name="_Area"></param>
        /// <param name="_PTN"></param>
        /// <param name="_Adhesive"></param>
        /// <returns>Table</returns>
        public DataTable getDataFromMsdb(string _Area,string _PTN,string _Adhesive)
        {
            string str = string.Format(@"Select Area,Adhesive,Bake_Program,BakeTime
                                         From spare_part.OVEN_ASSY_FE_BAKETIME
                                         Where Area {0} and Bake_Program {1} and Adhesive {2}",
                                               string.IsNullOrEmpty(_Area) ? " like '%'" : string.Format(@" ='{0}'", _Area),
                                               string.IsNullOrEmpty(_PTN) ? " like '%'" : string.Format(@" ='{0}'", _PTN),
                                               string.IsNullOrEmpty(_Adhesive) ? " like '%'" : string.Format(@" ='{0}'", _Adhesive));
                     
            if (string.IsNullOrEmpty(_Area) && string.IsNullOrEmpty(_PTN) && string.IsNullOrEmpty(_Adhesive))
            {
                str = @"Select Area,Adhesive,Bake_Program,BakeTime
                        From spare_part.OVEN_ASSY_FE_BAKETIME
                        Where Area ='' and Bake_Program ='' and Adhesive =''";
            }

            string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();//OVEN
            AdoDbConn ado = new AdoDbConn(AdoDbConn.AdoDbType.Oracle, Conn);
            DataTable dt = new DataTable();
            dt = ado.loadDataTable(str, null, "spare_part.OVEN_ASSY_FE_BAKETIME");
            return dt;
        }


        //==================================================================================================
        //Common check
        //==================================================================================================
        public object getAssignObj(string typeName, ControlCollection ctrs)
        {
            foreach (object r in ctrs)
            {
                string ss = r.GetType().Name.ToString();
                if (r.GetType().Name.ToString().Equals(typeName))
                    return r;
            }
            object null_obj = new object();
            return null_obj;
        }
        public enum Role : int
        {
            Administrator = 0, Supervisor = 1, User = 2
        }
        public void checkLogin()
        {
            if (HttpContext.Current.Session["account"] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["isDemo"].ToString().Equals("Y"))
                {
                    HttpContext.Current.Session["UsersID"] = "1";
                    HttpContext.Current.Session["account"] = "admin";
                    HttpContext.Current.Session["RoleID"] = "0";
                    HttpContext.Current.Session["Name"] = "系統管理者";
                    HttpContext.Current.Session["Dept_Name"] = "NXP";
                }
                else
                {
                    HttpContext.Current.Response.Redirect("../others/Login.aspx");
                }
            }
        }
        public void checkRole(MasterPage Master)
        {
            ContentPlaceHolder PlaceHolder1 = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            foreach (object obj in PlaceHolder1.Controls)
            {
                switch (obj.GetType().Name.ToString())
                {
                    #region MasterPage_Button
                    case "Button":
                        //==================================================================================
                        //btnRole
                        //==================================================================================
                        if (((Button)obj).ID.Equals("btnRole") && Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator)
                        { ((Button)obj).Visible = true; }
                        break;
                    #endregion
                }
            }


            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder2");
            UpdatePanel panel = (UpdatePanel)PlaceHolder2.FindControl("up");
            Object ctrs = getAssignObj("Control", ((UpdatePanel)panel).Controls);

            if (ctrs.GetType().Name.ToString().Equals("Control"))
            {
                foreach (object ctr in ((Control)ctrs).Controls)
                {
                    switch (ctr.GetType().Name.ToString())
                    {
                        #region GridView
                        case "GridView":
                            string gvID = ((GridView)ctr).ID;

                            //==================================================================================
                            //PTN_Manage
                            //==================================================================================
                            if (gvID.Equals("GridViewAM") &&
                                (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator) || Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Supervisor)
                            {                                
                                ((GridView)ctr).Columns[15].Visible = true;
                                ((GridView)ctr).Columns[16].Visible = true;
                            }

                            //==================================================================================
                            //Oven Manage
                            //==================================================================================
                            if (gvID.Equals("GridViewOV") &&
                                (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator) || Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Supervisor)
                            {
                                ((GridView)ctr).Columns[5].Visible = true;
                                if ((Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator)) ((GridView)ctr).Columns[6].Visible = true;
                            }
                            break;
                        #endregion

                        #region Buttton
                        case "Button":
                            //==================================================================================
                            //btnNew
                            //==================================================================================
                            if (((Button)ctr).ID.Equals("btnNew") &&
                                (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator || Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Supervisor))
                            {
                                ((Button)ctr).Visible = true;
                                break;
                            }
                            break;
                        #endregion
                    }
                }
            }

        }

        //==================================================================================================
        //Get data from Hr
        //==================================================================================================
        public DataTable getEmpDeptDt()
        {
            App_Code.AdoDbConn adoHr = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, HrConn);
            string deptStr = string.Format(@"SELECT distinct DEPT_NAME from EMP_ACCESS_LIST
                                             Where email IS not NULL
                                             order by DEPT_NAME");
            DataTable dtDept = new DataTable();
            dtDept = adoHr.loadDataTable(deptStr, null, "EMP_ACCESS_LIST");
            return dtDept;
        }


        //==================================================================================================
        //Get data from Intrack
        //==================================================================================================
        public DataTable getFromIntrack(string batch)
        {
            try
            {
                App_Code.AdoDbConn Mado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, connMESPROD);
                //string Mstr = string.Format(@"select * from PSK_UPLOADTDFAS where BATCH_NR = '6420DCC108'");
                string Mstr_1 = string.Format(@"Select ED_DESC2,ED_F_12NC,DIFFUSION_LOTID,LOTID2,ED_PACKAGE 
                                              From   PSK_UPLOADTDFAS
                                              Where  BATCH_NR = '{0}'", batch.ToUpper());
                DataTable Mdt_1 = Mado.loadDataTable(Mstr_1, null, "PSK_UPLOADTDFAS");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"alert('typename = {0}');", Mdt.Rows[0]["ED_DESC2"].ToString()), true);


                //batch Substring(0, 7)
                string Mstr_2 = string.Format(@"Select PARAMETERNAME, PARAMETERVALUE 
                                                From   UDI_ORDERPARAMETER 
                                                Where  SHOPORDER = '{0}' and PARAMETERNAME = 'Adhesive description'", batch.Substring(0, 7).ToUpper());
                DataTable Mdt_2 = Mado.loadDataTable(Mstr_2, null, "UDI_ORDERPARAMETER");

            
                //===============================================================================
                //Create new DataTable
                //===============================================================================
                DataTable dt = new DataTable();
                dt.Columns.Add("TypeName");//typename = ED_DESC2
                dt.Columns.Add("ED_12NC");//12nc = ED_F_12NC
                dt.Columns.Add("DIFFUSION");//strDiffusion = DIFFUSION_LOTID + "　　" + LOTID2
                dt.Columns.Add("Package");//strPackage = ED_PACKAGE
                dt.Columns.Add("Glue");//Glue
                DataRow dr = dt.NewRow();
                dr[0] = Mdt_1.Rows[0]["ED_DESC2"].ToString();
                dr[1] = Mdt_1.Rows[0]["ED_F_12NC"].ToString();
                dr[2] = Mdt_1.Rows[0]["DIFFUSION_LOTID"].ToString() + "　　" + Mdt_1.Rows[0]["LOTID2"].ToString();
                dr[3] = Mdt_1.Rows[0]["ED_PACKAGE"].ToString();
                dr[4] = Mdt_2.Rows[0]["PARAMETERVALUE"].ToString();
                dt.Rows.Add(dr);

                return dt;
            }
            catch (Exception e) { return null; }
        }
    }
}