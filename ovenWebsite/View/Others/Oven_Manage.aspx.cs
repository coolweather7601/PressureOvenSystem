using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace nModBusWeb
{
    public partial class Oven_Manage : System.Web.UI.Page
    {
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["OVEN"].ToString();
        public nModBusWeb.App_Code.AdoDbConn ado;
        public static DataTable dtGv;
        public static StringBuilder sb = new StringBuilder();

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
            gridviewBind();
        }
        private void gridviewBind()
        {
            ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
            DataTable dt = new DataTable();
            string str = string.Format(@"Select Area,Oven_ID,Machine_ID,IsPressured
                                         From   OVEN_ASSY_Location
                                         Where  Area like '%{0}%'
                                                And Oven_ID like '%{1}%'
                                                And Machine_ID like '%{2}%'", txtArea.Text.Trim().ToUpper(),
                                                                              txtOvenid.Text.Trim().ToUpper(),
                                                                              txtMachineid.Text.Trim().ToUpper());
            dt = ado.loadDataTable(str, null, "OVEN_ASSY_Location");
            GridViewOV.DataSource = dt;
            GridViewOV.DataBind();
            dtGv = dt;
            showPage();
        }

        protected void GridViewOV_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression.ToString();
            string sortDirection = "ASC";

            if (sortExpression == this.GridViewOV.Attributes["SortExpression"])
                sortDirection = (this.GridViewOV.Attributes["SortDirection"].ToString() == sortDirection ? "DESC" : "ASC");

            this.GridViewOV.Attributes["SortExpression"] = sortExpression;
            this.GridViewOV.Attributes["SortDirection"] = sortDirection;

            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dtGv.DefaultView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            GridViewOV.DataSource = dtGv;
            GridViewOV.DataBind();
            showPage();
        }
        protected void GridViewOV_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewOV.EditIndex = e.NewEditIndex;
            GridViewOV.DataSource = dtGv;
            GridViewOV.DataBind();
            showPage();
        }
        protected void GridViewOV_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewOV.EditIndex = -1;
            GridViewOV.DataSource = dtGv;
            GridViewOV.DataBind();
            showPage();
        }
        protected void GridViewOV_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
            RadioButton gv_rdoIsPressured_Y = (RadioButton)GridViewOV.Rows[e.RowIndex].Cells[0].FindControl("rdoIsPressured_Y");
            RadioButton gv_rdoIsPressured_N = (RadioButton)GridViewOV.Rows[e.RowIndex].Cells[0].FindControl("rdoIsPressured_N");

            string pk_Area = GridViewOV.DataKeys[e.RowIndex]["Area"].ToString();
            string pk_OvenID = GridViewOV.DataKeys[e.RowIndex]["OVEN_ID"].ToString();
            string pk_MahcineID = GridViewOV.DataKeys[e.RowIndex]["Machine_ID"].ToString();

            //check
            sb = new StringBuilder();
            bool check = true;

            ////if (string.IsNullOrEmpty(gv_txtArea.Text)) { sb.Append("【Area】"); }
            ////if (string.IsNullOrEmpty(gv_txtAdhesive.Text)) { sb.Append("【Adhesive】"); }
            ////if (string.IsNullOrEmpty(gv_txtBakeProgram.Text)) { sb.Append("【Bake Program】"); }
            ////if (string.IsNullOrEmpty(gv_txtBakeTime.Text)) { sb.Append("【Bake Time】"); }
            ////if (!string.IsNullOrEmpty(sb.ToString())) { check = false; sb.Insert(0, "以下欄位未填\\n\\n"); }

            if (check)
            {
                ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
                string str = string.Format(@"Update Oven_Assy_Location 
                                             Set    isPressured = :isPressured
                                             Where  Area=:Area and Oven_ID =:Oven_ID and Machine_ID=:Machine_ID");

                string rdo = gv_rdoIsPressured_Y.Checked ? "Y" : "N";
                if (gv_rdoIsPressured_Y.Checked == false && gv_rdoIsPressured_N.Checked == false) rdo = null;
                object[] para = new object[] { rdo, pk_Area, pk_OvenID, pk_MahcineID };

                string reStr = (string)ado.dbNonQuery(str, para);
                if (reStr.ToUpper().Contains("SUCCESS"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料修改成功。');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr.Replace("\n", "")), true);

                GridViewOV.EditIndex = -1;
                gridviewBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", sb.ToString()), true);
            }
        }
        protected void GridViewOV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
        protected void GridViewOV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewOV.PageIndex = e.NewPageIndex;
            GridViewOV.DataSource = dtGv;
            GridViewOV.DataBind();
            showPage();
        }
        protected void GridViewOV_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
            //string pk = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "myDelete":
                    string[] argument = e.CommandArgument.ToString().Split(',');
                    string pk_Area = argument[0];
                    string pk_OvenID = argument[1];
                    string pk_MachineID = argument[2];
                                        
                    string sqlStr = string.Format(@"Delete from Oven_Assy_Location
                                                    Where Area='{0}' and Oven_ID='{1}' and Machine_ID='{2}'", pk_Area, pk_OvenID, pk_MachineID);
                    string reStr = (string)ado.dbNonQuery(sqlStr, null);
                    if (reStr.ToUpper().Contains("SUCCESS"))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料刪除成功。');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr.Replace("\n", "")), true);

                    gridviewBind();
                    break;
            }

        }

        #region PagerTemplate
        protected void lbnFirst_Click(object sender, EventArgs e)
        {
            int num = 0;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridViewOV_PageIndexChanging(null, ea);
        }
        protected void lbnPrev_Click(object sender, EventArgs e)
        {
            int num = GridViewOV.PageIndex - 1;

            if (num >= 0)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewOV_PageIndexChanging(null, ea);
            }
        }
        protected void lbnNext_Click(object sender, EventArgs e)
        {
            int num = GridViewOV.PageIndex + 1;

            if (num < GridViewOV.PageCount)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewOV_PageIndexChanging(null, ea);
            }            
        }
        protected void lbnLast_Click(object sender, EventArgs e)
        {
            int num = GridViewOV.PageCount - 1;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridViewOV_PageIndexChanging(null, ea);
        }
        private void showPage()
        {
            try
            {
                TextBox txtPage = (TextBox)GridViewOV.BottomPagerRow.FindControl("txtSizePage");
                Label lblCount = (Label)GridViewOV.BottomPagerRow.FindControl("lblTotalCount");
                Label lblPage = (Label)GridViewOV.BottomPagerRow.FindControl("lblPage");
                Label lblbTotal = (Label)GridViewOV.BottomPagerRow.FindControl("lblTotalPage");

                txtPage.Text = GridViewOV.PageSize.ToString();
                lblCount.Text = dtGv.Rows.Count.ToString();
                lblPage.Text = (GridViewOV.PageIndex + 1).ToString();
                lblbTotal.Text = GridViewOV.PageCount.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        // page change
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string numPage = ((TextBox)GridViewOV.BottomPagerRow.FindControl("txtSizePage")).Text.ToString();
                if (!string.IsNullOrEmpty(numPage))
                {
                    GridViewOV.PageSize = Convert.ToInt32(numPage);
                }

                TextBox pageNum = ((TextBox)GridViewOV.BottomPagerRow.FindControl("inPageNum"));
                string goPage = pageNum.Text.ToString();
                if (!string.IsNullOrEmpty(goPage))
                {
                    int num = Convert.ToInt32(goPage) - 1;
                    if (num >= 0)
                    {
                        GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                        GridViewOV_PageIndexChanging(null, ea);
                        ((TextBox)GridViewOV.BottomPagerRow.FindControl("inPageNum")).Text = null;
                    }
                }

                GridViewOV.DataSource = dtGv;
                GridViewOV.DataBind();
                showPage();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridviewBind();
        }
    }
}