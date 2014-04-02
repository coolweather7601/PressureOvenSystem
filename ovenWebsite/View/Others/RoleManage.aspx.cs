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
using System.Collections.Generic;

namespace nModBusWeb
{
    public partial class RoleManage : System.Web.UI.Page
    {
        public static string HrConn = System.Configuration.ConfigurationManager.ConnectionStrings["HR"].ToString();
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
        public App_Code.AdoDbConn adoHr;
        public App_Code.AdoDbConn ado;
        public static DataTable dtGv;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                App_Code.func fc = new App_Code.func();
                fc.checkLogin();
                fc.checkRole(Page.Master);
                initial();                
            }
        }
        private void initial()
        {
            ddlBind();
            gridviewBind();
        }
        private void ddlBind()
        {
            try
            {
                App_Code.func fc = new App_Code.func();
                DataTable dt = new DataTable();
                dt = fc.getEmpDeptDt();

                ddlDept.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    ddlDept.Items.Add(new ListItem(dr["DEPT_NAME"].ToString()));
                }
                ddlDept.Items.Insert(0, new ListItem("All"));
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
        private void gridviewBind()
        {
            ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
            DataTable dt = new DataTable();
            //prod db (PARAMDB_KHPLXSVC1) ,UsersID(49,65,69) 匯入時重複, 排除管理
            string str = string.Format(
                         @" Select Us.Oven_Assy_UsersID,R.Oven_Assy_RoleID,R.DESCRIBES as Role_desc, Us.account,Us.password,Us.Name,Us.Dept,US.EMP_NO,Us.Mail
                            From  Oven_Assy_Users Us
                            Inner join Oven_Assy_Role R on Us.Oven_Assy_RoleID= R.Oven_Assy_RoleID
                            WHERE Us.dept like '%{0}%'
                            And Us.Name like '%{1}%'
                            And Us.Account like '%{2}%'
                            Order by Us.dept", ddlDept.SelectedIndex.Equals(0) ? "%" : ddlDept.SelectedValue,
                                               txtName.Text.Trim(),
                                               txtAccount.Text.Trim());
            dt = ado.loadDataTable(str, null, "Oven_Assy_Users");
            GridView1.DataSource = dt;
            GridView1.DataBind();
            dtGv = dt;
            showPage();
        }

        //==================================================================================================
        //events
        //==================================================================================================
        protected void btnImport_Click(object sender, EventArgs e)
        {
            App_Code.func fc = new App_Code.func();

            adoHr = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, HrConn);
            ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);

            #region 取出相關dept
           
            DataTable hr_dtDep = fc.getEmpDeptDt();
            string hr_sql = string.Format(@"Select ltrim(EMP_NO,'0') as EMP_NO,Emp_Name,Dept_NAME,passwd,email 
                                            From   EMP_ACCESS_LIST");

            foreach (DataRow hr_drDept in hr_dtDep.Rows)
	        {
                if (hr_sql.Contains("Where"))
                    hr_sql += string.Format(@" OR dept_name='{0}'", hr_drDept["dept_name"].ToString());
                else
                    hr_sql += string.Format(@" Where dept_name='{0}'", hr_drDept["dept_name"].ToString());
	        }
            DataTable hr_dt = adoHr.loadDataTable(hr_sql, null, "EMP_ACCESS_LIST");
            #endregion

            List<string> arrStr = new List<string>();
            foreach (DataRow hr_dr in hr_dt.Rows)
            {
                string sql = string.Format(@"Select * 
                                             From   Oven_Assy_Users
                                             Where  account='{0}'", hr_dr["EMP_NO"].ToString());
                DataTable dt = ado.loadDataTable(sql, null, "Oven_Assy_Users");
                if (dt.Rows.Count < 1) 
                {
                    string insertStr = string.Format(@"Insert Into Oven_Assy_Users(Oven_Assy_UsersID,Oven_Assy_RoleID,account,password,name,dept,Emp_No,Mail)
                                                       VALUES(Oven_Assy_Users_sequence.nextval,'2','{0}','{1}','{2}','{3}','{4}','{5}')",
                                                              hr_dr["EMP_NO"].ToString(), hr_dr["passwd"].ToString(),
                                                              hr_dr["emp_name"].ToString(), hr_dr["dept_name"].ToString(),
                                                              hr_dr["EMP_NO"].ToString(), hr_dr["Email"].ToString());
                    arrStr.Add(insertStr);
                }
            }

            string reStr = ado.SQL_transaction(arrStr, Conn);
            if (reStr.ToUpper().Contains("SUCCESS"))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('匯入成功。');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr), true);
            
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridviewBind();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression.ToString();
            string sortDirection = "ASC";

            if (sortExpression == this.GridView1.Attributes["SortExpression"])
                sortDirection = (this.GridView1.Attributes["SortDirection"].ToString() == sortDirection ? "DESC" : "ASC");

            this.GridView1.Attributes["SortExpression"] = sortExpression;
            this.GridView1.Attributes["SortDirection"] = sortDirection;

            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dtGv.DefaultView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            GridView1.DataSource = dtGv;
            GridView1.DataBind();
            showPage();
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource = dtGv;
            GridView1.DataBind();
            showPage();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = dtGv;
            GridView1.DataBind();
            showPage();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string dk = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string describe = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("ddlDescribe")).SelectedValue;

            ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
            string str = string.Format(@"Update Oven_Assy_Users Set Oven_Assy_RoleID='{0}' where Oven_Assy_UsersID='{1}'", describe, dk);
            //ado.dbNonQuery(str, null);
            string reStr = (string)ado.dbNonQuery(str, null);
            if (reStr.ToUpper().Contains("SUCCESS"))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料修改成功。');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr), true); 

            GridView1.EditIndex = -1;

            gridviewBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                    && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
                string str = "select Oven_Assy_RoleID, Describes from Oven_Assy_Role";
                DataTable dt = ado.loadDataTable(str, null, "Oven_Assy_Role");

                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlDescribe");

                ddl.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    ddl.Items.Add(new ListItem(dr["Describes"].ToString(), dr["Oven_Assy_RoleID"].ToString()));
                }

                string dk = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
                str = string.Format(@"select Oven_Assy_RoleID from Oven_Assy_Users WHERE Oven_Assy_UsersID='{0}'", dk);
                dt = ado.loadDataTable(str, null, "Oven_Assy_Users");
                ddl.SelectedValue = dt.Rows[0]["Oven_Assy_RoleID"].ToString();
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = dtGv;
            GridView1.DataBind();
            showPage();
        }

        #region PagerTemplate
        protected void lbnFirst_Click(object sender, EventArgs e)
        {
            int num = 0;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridView1_PageIndexChanging(null, ea);
        }
        protected void lbnPrev_Click(object sender, EventArgs e)
        {
            int num = GridView1.PageIndex - 1;

            if (num >= 0)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridView1_PageIndexChanging(null, ea);
            }
        }
        protected void lbnNext_Click(object sender, EventArgs e)
        {
            int num = GridView1.PageIndex + 1;

            if (num < GridView1.PageCount)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridView1_PageIndexChanging(null, ea);
            }
        }
        protected void lbnLast_Click(object sender, EventArgs e)
        {
            int num = GridView1.PageCount - 1;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridView1_PageIndexChanging(null, ea);
        }
        private void showPage()
        {
            try
            {
                TextBox txtPage = (TextBox)GridView1.BottomPagerRow.FindControl("txtSizePage");
                Label lblCount = (Label)GridView1.BottomPagerRow.FindControl("lblTotalCount");
                Label lblPage = (Label)GridView1.BottomPagerRow.FindControl("lblPage");
                Label lblbTotal = (Label)GridView1.BottomPagerRow.FindControl("lblTotalPage");

                txtPage.Text = GridView1.PageSize.ToString();
                lblCount.Text = dtGv.Rows.Count.ToString();
                lblPage.Text = (GridView1.PageIndex + 1).ToString();
                lblbTotal.Text = GridView1.PageCount.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        // page change
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string numPage = ((TextBox)GridView1.BottomPagerRow.FindControl("txtSizePage")).Text.ToString();
                if (!string.IsNullOrEmpty(numPage))
                {
                    GridView1.PageSize = Convert.ToInt32(numPage);
                }

                TextBox pageNum = ((TextBox)GridView1.BottomPagerRow.FindControl("inPageNum"));
                string goPage = pageNum.Text.ToString();
                if (!string.IsNullOrEmpty(goPage))
                {
                    int num = Convert.ToInt32(goPage) - 1;
                    if (num >= 0)
                    {
                        GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                        GridView1_PageIndexChanging(null, ea);
                        ((TextBox)GridView1.BottomPagerRow.FindControl("inPageNum")).Text = null;
                    }
                }

                GridView1.DataSource = dtGv;
                GridView1.DataBind();
                showPage();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion
    }
}