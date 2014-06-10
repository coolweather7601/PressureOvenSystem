<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="chart_linecompare.aspx.cs" Inherits="nModBusWeb.chart_linecompare" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="ibtnOutput" />
    </Triggers>

    <ContentTemplate>        
         <div style="font-size:18pt; font-family:verdana; font-weight:bold">
            Curing Record
        </div>

        <table>
            <tr>
                <td>Machine ID:</td>
                <td><asp:TextBox ID="txtMachineID" runat="server"></asp:TextBox></td>  
                <td>Batch No.:</td>
                <td><asp:TextBox ID="txtBatch" runat="server"></asp:TextBox></td>             
                <td>Start Time:</td>
                <td>
                    <asp:TextBox ID="txtStart" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1"
                        runat="server" Format="yyyy/MM/dd" TargetControlID="txtStart">
                    </cc1:CalendarExtender>
                </td>                
                <td>End Time:</td>
                <td>
                    <asp:TextBox ID="txtEnd" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2"
                        runat="server" Format="yyyy/MM/dd" TargetControlID="txtEnd">
                    </cc1:CalendarExtender>
                </td>
                <td><asp:Button ID="btnQuery" Text="Query" runat="server" onclick="btnQuery_Click"/></td>   
                <td><asp:ImageButton ID="ibtnOutput" ImageUrl="~/Images/excel.jpg" runat="server" onclick="ibtnOutput_Click" /></td>             
            </tr>
            
        </table>
        
        <hr style="border:solid 1px #000080" />
        <h1> <%= Press_result %> </h1>
        <h1> <%= Temp_result%> </h1>

        <chart:WebChartViewer id="WebChartViewer1" runat="server" />
        <br /><br /><br /><br />
        <chart:WebChartViewer id="WebChartViewer2" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>