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
            <td style="text-align:right;"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
        </tr>
    </table>

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
                <asp:TemplateField HeaderText="IsPressured">
                    <EditItemTemplate>
                        <asp:RadioButton ID="rdoIsPressured_Y" Text="Yes" GroupName="rdo" runat="server" />
                        <asp:RadioButton ID="rdoIsPressured_N" Text="No" GroupName="rdo" runat="server" />
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

    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

