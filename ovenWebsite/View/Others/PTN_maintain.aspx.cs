using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace nModBusWeb
{
    public partial class PTN_maintain : System.Web.UI.Page
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
            string str = string.Format(@"Select Area,Adhesive,Bake_Program,
                                                Process_1, Hour_1, Min_1, Temperature_1, Pressure_1,
                                                Process_2, Hour_2, Min_2, Temperature_2, Pressure_2,BakeTime,isPressured
                                         From   OVEN_ASSY_FE_BakeTime
                                         Where  Area like '%{0}%'
                                                And Adhesive like '%{1}%'
                                                And Bake_Program like '%{2}%'
                                                And isPressured = '{3}'", txtArea.Text.Trim().ToUpper(),
                                                                          txtAdhesive.Text.Trim().ToUpper(),
                                                                          txtBakeProgram.Text.Trim().ToUpper(),
                                                                          (chkPressured.Checked) ? 'Y' : 'N');
            dt = ado.loadDataTable(str, null, "OVEN_ASSY_FE_BakeTime");
            GridViewAM.DataSource = dt;
            GridViewAM.DataBind();
            dtGv = dt;
            showPage();

            //isPressured attributes
            GridViewAM.Columns[4].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[5].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[6].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[7].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[8].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[9].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[10].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[11].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[12].Visible = chkPressured.Checked ? true : false;
            GridViewAM.Columns[13].Visible = chkPressured.Checked ? true : false;
        }

        protected void GridViewAM_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression.ToString();
            string sortDirection = "ASC";

            if (sortExpression == this.GridViewAM.Attributes["SortExpression"])
                sortDirection = (this.GridViewAM.Attributes["SortDirection"].ToString() == sortDirection ? "DESC" : "ASC");

            this.GridViewAM.Attributes["SortExpression"] = sortExpression;
            this.GridViewAM.Attributes["SortDirection"] = sortDirection;

            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dtGv.DefaultView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewAM.EditIndex = e.NewEditIndex;
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAM.EditIndex = -1;
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox gv_txtProcess_1 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtProcess_1");
            TextBox gv_txtHour_1 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtHour_1");
            TextBox gv_txtMin_1 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtMin_1");
            TextBox gv_txtTemperature_1 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtTemperature_1");
            TextBox gv_txtPressure_1 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtPressure_1");
            TextBox gv_txtProcess_2 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtProcess_2");
            TextBox gv_txtHour_2 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtHour_2");
            TextBox gv_txtMin_2 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtMin_2");
            TextBox gv_txtTemperature_2 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtTemperature_2");
            TextBox gv_txtPressure_2 = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtPressure_2");
            TextBox gv_txtBakeTime = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtBakeTime");
            CheckBox gv_chkIsPressured = (CheckBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_chkIsPressured");

            string pk_Area = GridViewAM.DataKeys[e.RowIndex]["Area"].ToString();
            string pk_Adhesive = GridViewAM.DataKeys[e.RowIndex]["Adhesive"].ToString();
            string pk_Bake_Program = GridViewAM.DataKeys[e.RowIndex]["Bake_Program"].ToString();

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
                string str = string.Format(@"Update Oven_Assy_Fe_BakeTime 
                                             Set    Process_1     = :Process_1,
                                                    Hour_1        = :Hour_1,
                                                    Min_1         = :Min_1,
                                                    Temperature_1 = :Temperature_1,
                                                    Pressure_1    = :Pressure_1,
                                                    Process_2     = :Process_2,
                                                    Hour_2        = :Hour_2,
                                                    Min_2         = :Min_2,
                                                    Temperature_2 = :Temperature_2,
                                                    Pressure_2    = :pressure_2,
                                                    BakeTime      = :BakeTime,
                                                    isPressured   = :isPressured
                                             Where  Area=:Area and Adhesive =:Adhesive and Bake_Program=:Bake_Program");

                object[] para = new object[] { gv_txtProcess_1.Text.Trim(), gv_txtHour_1.Text.Trim(), gv_txtMin_1.Text.Trim(), gv_txtTemperature_1.Text.Trim(), gv_txtPressure_1.Text.Trim(),
                                               gv_txtProcess_2.Text.Trim(), gv_txtHour_2.Text.Trim(), gv_txtMin_2.Text.Trim(), gv_txtTemperature_2.Text.Trim(), gv_txtPressure_2.Text.Trim(),
                                               gv_txtBakeTime.Text.Trim(),  gv_chkIsPressured.Checked?"Y":"N",
                                               pk_Area, pk_Adhesive, pk_Bake_Program };

                string reStr = (string)ado.dbNonQuery(str, para);
                if (reStr.ToUpper().Contains("SUCCESS"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料修改成功。');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr.Replace("\n", "")), true);

                GridViewAM.EditIndex = -1;
                gridviewBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", sb.ToString()), true);
            }
        }
        protected void GridViewAM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit )
            {
                CheckBox gv_chkIsPressured = (CheckBox)e.Row.FindControl("gv_chkIsPressured");
                ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
                
                string pk_Area = GridViewAM.DataKeys[e.Row.RowIndex]["Area"].ToString();
                string pk_Adhesive = GridViewAM.DataKeys[e.Row.RowIndex]["Adhesive"].ToString();
                string pk_Bake_Program = GridViewAM.DataKeys[e.Row.RowIndex]["Bake_Program"].ToString();

                string str = string.Format(@"Select isPressured From Oven_Assy_Fe_BakeTime Where Area='{0}' and Adhesive='{1}' and Bake_Program='{2}'", pk_Area, pk_Adhesive, pk_Bake_Program);

                DataTable dt = ado.loadDataTable(str, null, "Oven_Assy_Fe_BakeTime");

                gv_chkIsPressured.Checked = dt.Rows[0]["isPressured"].ToString().Equals("Y") ? true : false;
            }
        }
        protected void GridViewAM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAM.PageIndex = e.NewPageIndex;
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
            //string pk = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "myDelete":
                    string[] argument = e.CommandArgument.ToString().Split(',');
                    string pk_Area = argument[0];
                    string pk_Adhesive = argument[1];
                    string pk_BakeProgram = argument[2];

                    string sqlStr = string.Format(@"Delete from Oven_Assy_Fe_Baketime
                                                    Where Area='{0}' and Adhesive='{1}' and Bake_Program='{2}'", pk_Area, pk_Adhesive, pk_BakeProgram);
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
            GridViewAM_PageIndexChanging(null, ea);
        }
        protected void lbnPrev_Click(object sender, EventArgs e)
        {
            int num = GridViewAM.PageIndex - 1;

            if (num >= 0)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewAM_PageIndexChanging(null, ea);
            }
        }
        protected void lbnNext_Click(object sender, EventArgs e)
        {
            int num = GridViewAM.PageIndex + 1;

            if (num < GridViewAM.PageCount)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewAM_PageIndexChanging(null, ea);
            }
        }
        protected void lbnLast_Click(object sender, EventArgs e)
        {
            int num = GridViewAM.PageCount - 1;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridViewAM_PageIndexChanging(null, ea);
        }
        private void showPage()
        {
            try
            {
                TextBox txtPage = (TextBox)GridViewAM.BottomPagerRow.FindControl("txtSizePage");
                Label lblCount = (Label)GridViewAM.BottomPagerRow.FindControl("lblTotalCount");
                Label lblPage = (Label)GridViewAM.BottomPagerRow.FindControl("lblPage");
                Label lblbTotal = (Label)GridViewAM.BottomPagerRow.FindControl("lblTotalPage");

                txtPage.Text = GridViewAM.PageSize.ToString();
                lblCount.Text = dtGv.Rows.Count.ToString();
                lblPage.Text = (GridViewAM.PageIndex + 1).ToString();
                lblbTotal.Text = GridViewAM.PageCount.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        // page change
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string numPage = ((TextBox)GridViewAM.BottomPagerRow.FindControl("txtSizePage")).Text.ToString();
                if (!string.IsNullOrEmpty(numPage))
                {
                    GridViewAM.PageSize = Convert.ToInt32(numPage);
                }

                TextBox pageNum = ((TextBox)GridViewAM.BottomPagerRow.FindControl("inPageNum"));
                string goPage = pageNum.Text.ToString();
                if (!string.IsNullOrEmpty(goPage))
                {
                    int num = Convert.ToInt32(goPage) - 1;
                    if (num >= 0)
                    {
                        GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                        GridViewAM_PageIndexChanging(null, ea);
                        ((TextBox)GridViewAM.BottomPagerRow.FindControl("inPageNum")).Text = null;
                    }
                }

                GridViewAM.DataSource = dtGv;
                GridViewAM.DataBind();
                showPage();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridviewBind();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            GridViewAM.DataSource = null;
            GridViewAM.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Insert);
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
            //TextBox dv_txtArea = ((TextBox)DetailsView1.FindControl("dv_txtArea"));
            DropDownList dv_ddlArea = ((DropDownList)DetailsView1.FindControl("dv_ddlArea"));
            TextBox dv_txtAdhesive = ((TextBox)DetailsView1.FindControl("dv_txtAdhesive"));
            TextBox dv_txtBakeProgram = ((TextBox)DetailsView1.FindControl("dv_txtBakeProgram"));
            TextBox dv_txtProcess_1 = ((TextBox)DetailsView1.FindControl("dv_txtProcess_1"));
            TextBox dv_txtHour_1 = ((TextBox)DetailsView1.FindControl("dv_txtHour_1"));
            TextBox dv_txtMin_1 = ((TextBox)DetailsView1.FindControl("dv_txtMin_1"));
            TextBox dv_txtTemperature_1 = ((TextBox)DetailsView1.FindControl("dv_txtTemperature_1"));
            TextBox dv_txtPressure_1 = ((TextBox)DetailsView1.FindControl("dv_txtPressure_1"));
            TextBox dv_txtProcess_2 = ((TextBox)DetailsView1.FindControl("dv_txtProcess_2"));
            TextBox dv_txtHour_2 = ((TextBox)DetailsView1.FindControl("dv_txtHour_2"));
            TextBox dv_txtMin_2 = ((TextBox)DetailsView1.FindControl("dv_txtMin_2"));
            TextBox dv_txtTemperature_2 = ((TextBox)DetailsView1.FindControl("dv_txtTemperature_2"));
            TextBox dv_txtPressure_2 = ((TextBox)DetailsView1.FindControl("dv_txtPressure_2"));
            TextBox dv_txtBakeTime = ((TextBox)DetailsView1.FindControl("dv_txtBakeTime"));
            CheckBox dv_chkIsPressured = ((CheckBox)DetailsView1.FindControl("dv_chkIsPressured"));

            //Check
            sb = new StringBuilder();
            bool check = true;

            // (string.IsNullOrEmpty(dv_txtArea.Text)) { sb.Append("【Area】"); }
            if (dv_ddlArea.Text.Equals("0")) { sb.Append("【Area】"); }
            if (string.IsNullOrEmpty(dv_txtAdhesive.Text)) { sb.Append("【Adhesive】"); }
            if (string.IsNullOrEmpty(dv_txtBakeProgram.Text)) { sb.Append("【Bake Program】"); }
            if (string.IsNullOrEmpty(dv_txtBakeTime.Text)) { sb.Append("【BakeTime】"); }
            if (!string.IsNullOrEmpty(sb.ToString())) { check = false; sb.Insert(0, "以下欄位未填\\n\\n"); }

            if (check == true)
            {
                string TestStr = string.Format(@"Select * 
                                                 From Oven_Assy_Fe_BakeTime 
                                                 Where Area='{0}' and Adhesive='{1}' and Bake_Program='{2}'", dv_ddlArea.Text.Trim(), dv_txtAdhesive.Text.Trim().ToUpper(), dv_txtBakeProgram.Text.Trim().ToUpper());
                DataTable tester_dt = ado.loadDataTable(TestStr, null, "Oven_Assy_FE_BakeTime");
                if (tester_dt.Rows.Count > 0) { check = false; sb.Insert(0, "PTN重複定義，請檢查\\n\\n"); }                
            }

            if (check)
            {
                string sqlStr = string.Format(@"Insert into Oven_Assy_Fe_BakeTime (Area, Adhesive, Bake_Program,
                                                                                   Process_1, Hour_1, Min_1, Temperature_1, Pressure_1,
                                                                                   Process_2, Hour_2, Min_2, Temperature_2, Pressure_2,
                                                                                   BakeTime,isPressured)
                                                Values (:Area, :Adhesive, :Bake_Program,
                                                        :Process_1, :Hour_1, :Min_1, :Temperature_1, :Pressure_1,
                                                        :Process_2, :Hour_2, :Min_2, :Temperature_2, :Pressure_2,
                                                        :BakeTime,:isPressured)");
                object[] para = new object[] { dv_ddlArea.Text.Trim().ToUpper(), dv_txtAdhesive.Text.Trim().ToUpper(), dv_txtBakeProgram.Text.Trim().ToUpper(),
                                               dv_txtProcess_1.Text.Trim(), dv_txtHour_1.Text.Trim(), dv_txtMin_1.Text.Trim(), dv_txtTemperature_1.Text.Trim(), dv_txtPressure_1.Text.Trim(),
                                               dv_txtProcess_2.Text.Trim(), dv_txtHour_2.Text.Trim(), dv_txtMin_2.Text.Trim(), dv_txtTemperature_2.Text.Trim(), dv_txtPressure_2.Text.Trim(),
                                               dv_txtBakeTime.Text.Trim().ToUpper(),dv_chkIsPressured.Checked?"Y":"N" };
                
                string reStr = (string)ado.dbNonQuery(sqlStr, para);
                if (reStr.ToUpper().Contains("SUCCESS"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料新增成功。');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr.Replace("\n", "")), true);

                DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
                DetailsView1.DataSource = null;
                DetailsView1.DataBind();

                gridviewBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", sb.ToString()), true);
            }
        }
        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
            {
                DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
                DetailsView1.DataSource = null;
                DetailsView1.DataBind();

                gridviewBind();
            }
        }
        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {
            if (DetailsView1.CurrentMode == DetailsViewMode.Insert)
            {                
                ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle,Conn);
                string str = "select distinct area from oven_assy_location";
                DataTable dt = ado.loadDataTable(str, null, "oven_assy_location");

                DropDownList dv_ddlArea = (DropDownList)DetailsView1.FindControl("dv_ddlArea");
                dv_ddlArea.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    dv_ddlArea.Items.Add(new ListItem(dr["area"].ToString()));
                }
                dv_ddlArea.Items.Insert(0, new ListItem("請選擇", "0"));
            }
        }
        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

        }
        
}
}