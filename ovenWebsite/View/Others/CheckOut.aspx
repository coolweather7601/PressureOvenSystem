<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="CheckOut.aspx.cs" Inherits="nModBusWeb.CheckOut" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<h2>Check Out</h2>
<table> 
    <tr>
        <td style='font-family: georgia, serif;
                    color: #9E3737;
                    font-size: 15px;
                    font-weight: bold;'>Step1.</td>
        <td>User：</td>
        <td colspan='5'><asp:TextBox ID="txtUser" runat="server" AutoPostBack="false" 
                ontextchanged="txtUser_TextChanged" ></asp:TextBox></td>
    </tr>
    <tr>
        <td style='font-family: georgia, serif;
                    color: #9E3737;
                    font-size: 15px;
                    font-weight: bold;'>Step2.</td>
        <td>MachineID：</td>
        <td colspan='5'><asp:TextBox ID="txtMachineID" runat="server" AutoPostBack="true" ontextchanged="txtMachineID_TextChanged" ></asp:TextBox></td>
    </tr>
</table>
</asp:Content>

