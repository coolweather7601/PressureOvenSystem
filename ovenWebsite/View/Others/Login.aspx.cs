using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace nModBusWeb
{
    public partial class Login : System.Web.UI.Page
    {
        public static string HrConn = System.Configuration.ConfigurationManager.ConnectionStrings["HR"].ToString();
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["account"] != null)
                {
                    Response.Redirect(string.Format(@"PTN_Maintain.aspx"));
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            #region OVEN
            App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
            DataTable dt = new DataTable();
            string sqlstr = @"SELECT OVEN_ASSY_UsersID,OVEN_ASSY_RoleID,account,password,name,dept,emp_no,mail 
                              FROM OVEN_ASSY_Users 
                              WHERE account=:account AND password=:password
                              Order by OVEN_ASSY_usersid desc";
            object[] para = new object[] { txtAcc.Text.Trim(), txtPw.Text.Trim() };

            dt = ado.loadDataTable(sqlstr, para, "OVEN_ASSY_Users");
            if (dt.Rows.Count > 0)
            {
                Session["UsersID"] = dt.Rows[0]["OVEN_ASSY_UsersID"].ToString();
                Session["account"] = dt.Rows[0]["account"].ToString();
                Session["RoleID"] = dt.Rows[0]["OVEN_ASSY_RoleID"].ToString();
                Session["Name"] = dt.Rows[0]["name"].ToString();
                Session["Dept_Name"] = dt.Rows[0]["Dept"].ToString();
                Response.Redirect(string.Format(@"PTN_Maintain.aspx"));
                return;
            }
            #endregion

            #region EMP
            App_Code.AdoDbConn adoHr = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, HrConn);
            DataTable dtEmp = new DataTable();
            string strEmp = string.Format(@"SELECT ltrim(EMP_NO,'0') as EMP_NO,Emp_Name,Dept_NAME,passwd,email
                                            FROM   EMP_ACCESS_LIST 
                                            WHERE  Emp_No like '%{0}%' AND Passwd='{1}'", txtAcc.Text.Trim(), txtPw.Text.Trim());
            dtEmp = adoHr.loadDataTable(strEmp, null, "EMP_ACCESS_LIST");
            if (dtEmp.Rows.Count > 0)
            {
                Session["account"] = dtEmp.Rows[0]["Emp_No"].ToString();
                Session["Name"] = dtEmp.Rows[0]["Emp_Name"].ToString();
                Session["Dept_Name"] = dtEmp.Rows[0]["Dept_Name"].ToString();
                Session["RoleID"] = "2";//general user

                string insertStr = @"INSERT INTO OVEN_ASSY_Users(OVEN_ASSY_UsersID,OVEN_ASSY_RoleID,account,password,name,dept,emp_no,mail )
                                     VALUES (OVEN_ASSY_Users_sequence.nextval,'2',:account,:password,:name,:dept,:emp_no,:mail)";
                object[] param = new object[] { dtEmp.Rows[0]["Emp_No"].ToString(),dtEmp.Rows[0]["passwd"].ToString(),
                                                dtEmp.Rows[0]["Emp_Name"].ToString(),dtEmp.Rows[0]["Dept_Name"].ToString(),
                                                dtEmp.Rows[0]["Emp_No"].ToString(),dtEmp.Rows[0]["email"].ToString()};
                ado.dbNonQuery(insertStr, param);
                Response.Redirect(string.Format(@"PTN_Maintain.aspx"));
                return;
            }
            #endregion
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('account/password 輸入錯誤，請重新檢查');", true);
            txtAcc.Text = null;
            txtPw.Text = null;
            SetFocus(txtAcc);
        }
    }
}