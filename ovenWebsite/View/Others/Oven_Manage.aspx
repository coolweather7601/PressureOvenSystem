<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="Oven_Manage.aspx.cs" Inherits="nModBusWeb.Oven_Manage" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
    <h2>Oven Manage</h2>
    <table>        
        <tr>
            <td>Area：</td>
            <td><asp:TextBox ID="txtArea" runat="server"></asp:TextBox></td>
            <td>Oven_ID：</td>
            <td><asp:TextBox ID="txtOvenid" runat="server"></asp:TextBox></td>
            <td>Machine_ID：</td>
            <td><asp:TextBox ID="txtMachineid" runat="server"></asp:TextBox></td>
            <td>isPressured：</td>            
            <td><asp:CheckBox ID="chkPressured" runat="server" Checked="true"/></td>
            <td style="text-align:right;"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
        </tr>
    </table>

    <asp:Button ID="btnNew" runat="server"  Text="New OVEN" OnClick="btnNew_Click" Visible="false" />

    <asp:GridView ID="GridViewOV" runat="server" 
            DataKeyNames="Area,OVEN_ID,Machine_ID" AutoGenerateColumns="False"
                      CellPadding="3" ForeColor="Black" GridLines="Vertical" 
            AllowSorting="True" AllowPaging="True"
            OnSorting="GridViewOV_Sorting" 
            OnRowCancelingEdit="GridViewOV_RowCancelingEdit" 
            OnRowEditing="GridViewOV_RowEditing" 
            OnRowUpdating="GridViewOV_RowUpdating" 
            OnRowDataBound="GridViewOV_RowDataBound"  
            OnPageIndexChanging="GridViewOV_PageIndexChanging" BackColor="White" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
            OnRowCommand="GridViewOV_RowCommand" Width="50%">
            <Columns>                
                <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# (Container.DataItemIndex +1).ToString()%>
                        </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Area" SortExpression="Area">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Area") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Oven_ID" SortExpression="Oven_ID">                    
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("OVEN_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="25%" />
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Machine_ID" SortExpression="Machine_ID">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Machine_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="POS_X" SortExpression="POS_X">
                    <ItemTemplate>
                        <asp:Label ID="Label23" runat="server" Text='<%# Bind("POS_X") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="POS_Y" SortExpression="POS_Y">
                    <ItemTemplate>
                        <asp:Label ID="Label24" runat="server" Text='<%# Bind("POS_Y") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="IsPressured">
                    <EditItemTemplate>
                        <asp:CheckBox ID="gv_chkIsPressured" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label04" runat="server" Text='<%# Bind("IsPressured") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit" Visible="false">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lbUpdate" runat="server" CommandName="Update">Submit</asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lbCancelUpdate" runat="server" CommandName="Cancel">Cancel</asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btn1" CommandName="Edit" 
                                ImageUrl="../../images/icon_edit.png" runat="server" Width="18px" ToolTip="Edit"  />                        
                    </ItemTemplate>
                    <HeaderStyle Width="5%" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" Visible="false">
                    <ItemTemplate>                        
                        <asp:ImageButton ID="btn2" CommandName="myDelete" OnClientClick="return confirm('Are you sure of deleteing this tester?');"
                                CommandArgument='<%# Eval("Area") +","+ Eval("Oven_ID") +","+ Eval("Machine_ID") %>' ImageUrl="../../images/icon_delete.png"
                                runat="server" Width="18px" ToolTip="Delete"  />
                    </ItemTemplate>             
                    <HeaderStyle Width="5%" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField> 
            </Columns>
            <PagerTemplate>
                <div style="text-align:center;" id="page">
                共<asp:Label ID="lblTotalCount" runat="server" Text=""></asp:Label>筆 │ 
                <asp:Label ID="lblPage" runat="server" ></asp:Label> / <asp:Label ID="lblTotalPage" runat="server" ></asp:Label>頁 │ 
                <asp:LinkButton ID="lbnFirst" runat="Server" Text="第一頁" onclick="lbnFirst_Click" ></asp:LinkButton> │ 
                <asp:LinkButton ID="lbnPrev" runat="server" Text="上一頁" onclick="lbnPrev_Click" ></asp:LinkButton> │ 
                <asp:LinkButton ID="lbnNext" runat="Server" Text="下一頁" onclick="lbnNext_Click"></asp:LinkButton> │ 
                <asp:LinkButton ID="lbnLast" runat="Server" Text="最後頁" onclick="lbnLast_Click" ></asp:LinkButton> │ 
                到第<asp:TextBox ID="inPageNum" Width="20px" runat="server"></asp:TextBox>頁： 
                每頁<asp:TextBox ID="txtSizePage" Width="25px" runat="server"></asp:TextBox>筆
                <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click"/>
                <br />
                </div>
            </PagerTemplate>
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#CCCCCC" />  
        </asp:GridView>

    <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px" AutoGenerateRows="False"
         CellPadding="4" ForeColor="#333333" GridLines="None" 
         OnItemCommand="DetailsView1_ItemCommand" 
         OnModeChanging="DetailsView1_ModeChanging" 
         OnItemInserting="DetailsView1_ItemInserting" OnDataBound="DetailsView1_DataBound">
                <Fields>                
                    <asp:TemplateField HeaderText="Area">
                        <InsertItemTemplate>
                            <asp:DropDownList ID="dv_ddlArea" runat="server">
                            </asp:DropDownList>
                        </InsertItemTemplate>
                    </asp:TemplateField>     
                    <asp:TemplateField HeaderText="OVEN_ID">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtOvenID" runat="server" Text='<%# Bind("Oven_ID") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label02" runat="server" Text='<%# Bind("Oven_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                   <asp:TemplateField HeaderText="Machine_ID">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtMachineID" runat="server" Text='<%# Bind("Machine_ID") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label03" runat="server" Text='<%# Bind("Machine_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="POS_X">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtPOSX" runat="server" Text='<%# Bind("POS_X") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label04" runat="server" Text='<%# Bind("POS_X") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="POS_Y">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtPOSY" runat="server" Text='<%# Bind("POS_Y") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label05" runat="server" Text='<%# Bind("POS_Y") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="isPressured">
                        <InsertItemTemplate>
                            <table>
                                <tr><td colspan='2'><asp:CheckBox ID="dv_chkIsPressured" runat="server" AutoPostBack="true" oncheckedchanged="dv_chkIsPressured_CheckedChanged" /></td></tr>
                                <tr>
                                    <td><asp:Label ID="dv_lblComport" runat="server" Text="COM" Visible="false"></asp:Label></td>
                                    <td><asp:TextBox ID="dv_txtComport" runat="server" Visible="false"></asp:TextBox></td>
                                </tr>
                            </table>                         
                        </InsertItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action" ShowHeader="False">
                        <InsertItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="Insert"></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel"></asp:LinkButton>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="New"
                                Text="新增"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Fields>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:DetailsView>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

