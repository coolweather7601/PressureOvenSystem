﻿<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="CheckIn.aspx.cs" Inherits="nModBusWeb.CheckIn" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
    <asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
    <h2>Check In</h2>
        <div>
            <table> 
                <tr>
                    <td style='font-family: georgia, serif;
                               color: #9E3737;
                               font-size: 15px;
                               font-weight: bold;'>Step1.</td>
                    <td>User：</td>
                    <td colspan='5'><asp:TextBox ID="txtUser" runat="server" AutoPostBack="false" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td style='font-family: georgia, serif;
                               color: #9E3737;
                               font-size: 15px;
                               font-weight: bold;'>Step2.</td>
                    <td>MachineID：</td>
                    <td colspan='5'><asp:TextBox ID="txtMachineID" runat="server" AutoPostBack="true" ontextchanged="txtMachineID_TextChanged" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td style='font-family: georgia, serif;
                               color: #9E3737;
                               font-size: 15px;
                               font-weight: bold;'>Step3.</td>
                    <td>Batch Card：</td>
                    <td colspan='5'><asp:TextBox ID="txtBC" runat="server" AutoPostBack="true" ontextchanged="txtBC_TextChanged"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style='font-family: georgia, serif;
                               color: #9E3737;
                               font-size: 15px;
                               font-weight: bold;'>Step4.</td>
                    <td>Oven Area：</td>
                    <td><asp:TextBox ID="txtArea" runat="server" ontextchanged="txt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                    <td>PTN：</td>
                    <td><asp:TextBox ID="txtPTN" runat="server" ontextchanged="txt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                    <td>Adhesive：<asp:TextBox ID="txtAdhesive" runat="server" ontextchanged="txt_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                    <td>Oven_ID：<asp:TextBox ID="txtOvenID" runat="server" AutoPostBack="true" ></asp:TextBox></td>
                </tr>
            </table>
            <asp:GridView ID="GridViewList" runat="server" BorderWidth="1px" BorderStyle="Solid"
            BorderColor="#999999" BackColor="White" OnPageIndexChanging="GridViewList_PageIndexChanging"
            AllowPaging="True" OnRowCommand="GridViewList_RowCommand"
            GridLines="Vertical" ForeColor="Black" CellPadding="3" AutoGenerateColumns="False"
            DataKeyNames="Adhesive">
            <Columns>
                <asp:TemplateField HeaderText="No">
                    <ItemTemplate>
                        <%# (Container.DataItemIndex+1).ToString()%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>                
                <asp:BoundField DataField="Area" HeaderText="Area" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="Adhesive" HeaderText="Adhesive" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="Bake_Program" HeaderText="Bake_Program" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="BakeTime" HeaderText="BakeTime" ReadOnly="True"></asp:BoundField>
                <asp:TemplateField HeaderText="Run" Visible="true">
                    <ItemTemplate>                        
                        <asp:ImageButton ID="btn2" CommandName="Run" OnClientClick="return confirm('Are you sure of comparing this PTN?');"
                                CommandArgument='<%# Eval("Area") +","+ Eval("Adhesive") +","+ Eval("Bake_Program") %>' ImageUrl="../../images/ball_red.gif"
                                runat="server" Width="18px" ToolTip="Run"  />
                    </ItemTemplate>             
                    <HeaderStyle Width="5%" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC"></FooterStyle>
            <PagerTemplate>
                <div style="text-align: center;" id="page">
                    共<asp:Label ID="lblTotalCount" runat="server" Text=""></asp:Label>筆 │
                    <asp:Label ID="lblPage" runat="server"></asp:Label>
                    /
                    <asp:Label ID="lblTotalPage" runat="server"></asp:Label>頁 │
                    <asp:LinkButton ID="lbnFirst" runat="Server" Text="第一頁" OnClick="lbnFirst_Click"></asp:LinkButton>
                    │
                    <asp:LinkButton ID="lbnPrev" runat="server" Text="上一頁" OnClick="lbnPrev_Click"></asp:LinkButton>
                    │
                    <asp:LinkButton ID="lbnNext" runat="Server" Text="下一頁" OnClick="lbnNext_Click"></asp:LinkButton>
                    │
                    <asp:LinkButton ID="lbnLast" runat="Server" Text="最後頁" OnClick="lbnLast_Click"></asp:LinkButton>
                    │ 到第 <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="inPageNum" Width="20px" runat="server"></asp:TextBox>
                       頁： 每頁 <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtSizePage" Width="25px" runat="server"></asp:TextBox>筆
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                    <br />
                </div>
            </PagerTemplate>
            <PagerStyle HorizontalAlign="Center" BackColor="#999999" ForeColor="Black"></PagerStyle>
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White"></HeaderStyle>
            <AlternatingRowStyle BackColor="#CCCCCC"></AlternatingRowStyle>
        </asp:GridView>
        </div>


        <%--<div style='border-top: 1px solid #96d1f8;
                    background: #1f5d87;
                    padding: 7.5px 15px;
                    -webkit-border-radius: 8px;
                    -moz-border-radius: 8px;
                    -webkit-box-shadow: rgba(0,0,0,1) 0 1px 0;
                    -moz-box-shadow: rgba(0,0,0,1) 0 1px 0;
                    color: white;'>
            <table> 
                <tr>
                    <td><asp:Label ID="lblTimer" runat="server" Text="ComPort"></asp:Label></td>
                    <td><asp:Button ID="btnOpen" runat="server" Text="Open" onclick="btnOpen_Click" Enabled="true" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" onclick="btnClose_Click" Enabled="false" />
                    </td>                    
                </tr>
                <tr>
                    <td>Function Cade.</td>
                    <td><asp:DropDownList ID="ddlFunc" runat="server" AutoPostBack="true" onselectedindexchanged="ddlFunc_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Test Category.</td>
                    <td><asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" onselectedindexchanged="ddlTest_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="2">
                        Write Value.
                        <asp:TextBox ID="txtInput" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Start Address.</td>
                    <td><asp:Label ID="lblRequest" runat="server" Text="Label"></asp:Label></td>
                </tr>                    
                <tr>
                    <td>
                        <asp:CheckBox ID="chk_10" runat="server" oncheckedchanged="chk_10_CheckedChanged" />十進位.
                    </td>
                    <td>
                        <asp:TextBox ID="txt_10" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chk_16" runat="server" oncheckedchanged="chk_16_CheckedChanged" />十六進位.
                    </td>
                    <td>
                        <asp:TextBox ID="txt_16" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnRequest" runat="server" Text="Request" onclick="btnRequest_Click" />
                    </td>
                </tr>
                
            </table>
        </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

