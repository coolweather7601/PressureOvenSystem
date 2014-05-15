<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="CheckIn.aspx.cs" Inherits="nModBusWeb.CheckIn" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <script type="text/vbscript" language="vbscript">
sub window_onload()
if ctl00$ContentPlaceHolder2$IsPlaySound.value = "ok" then
    PlayOKSound()
elseif ctl00$ContentPlaceHolder2$IsPlaySound.value = "err" then
    PlayErrSound()
end if
end sub

sub PlayOKSound()
strSoundFile = "D:\ok.wav" 
Dim WshShell
Set objShell = CreateObject("Wscript.Shell") 
strCommand = "sndrec32 /play /close " & chr(34) & strSoundFile & chr(34) 
objShell.Run strCommand, 0, True 
end sub

sub PlayErrSound()
strSoundFile = "D:\err.wav" 
Dim WshShell
Set objShell = CreateObject("Wscript.Shell") 
strCommand = "sndrec32 /play /close " & chr(34) & strSoundFile & chr(34) 
objShell.Run strCommand, 0, True 
end sub
</script>

    <asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
    <asp:TextBox ID="IsPlaySound" runat="server" Style="visibility: hidden"></asp:TextBox>
    
        <h2>Check In</h2>
        <div style='font-family: georgia, serif;
                               font-size: 15px;
                               font-weight: bold;'>
    產品進壓力烤箱 or Plasma <br />
    請依序輸入: 1. <font color='red'>薪號</font> 2. <font color='red'>烤箱 or Plasma 編號 (PM Number)</font> 3. <font color='red'>流程卡號 (Batch no.) </font>
    </div>
        <br />
        <div>
            <asp:TextBox ID="txtOvenID" runat="server" AutoPostBack="true"  Style="visibility: hidden"></asp:TextBox>
            <asp:TextBox ID="txtArea" runat="server" ontextchanged="txt_TextChanged" AutoPostBack="true" Style="visibility: hidden"></asp:TextBox>
            <asp:TextBox ID="txtAdhesive" runat="server" ontextchanged="txt_TextChanged" AutoPostBack="true" Style="visibility: hidden"></asp:TextBox>
            <asp:TextBox ID="txtPTN" runat="server" ontextchanged="txt_TextChanged" AutoPostBack="true" Style="visibility: hidden"></asp:TextBox>
            <table> 
                <tr>
                    <td style='font-family: georgia, serif;
                               color: #9E3737;
                               font-size: 15px;
                               font-weight: bold;'>Step1.</td>
                    <td>User：</td>
                    <td><asp:TextBox ID="txtUser" runat="server" AutoPostBack="true" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td style='font-family: georgia, serif;
                               color: #9E3737;
                               font-size: 15px;
                               font-weight: bold;'>Step2.</td>
                    <td>MachineID：</td>
                    <td><asp:TextBox ID="txtMachineID" runat="server" AutoPostBack="true" ontextchanged="txt_TextChanged" ></asp:TextBox></td>                        
                </tr>
                <tr>
                    <td style='font-family: georgia, serif;
                               color: #9E3737;
                               font-size: 15px;
                               font-weight: bold;'>Step3.</td>
                    <td>Batch Card：</td>
                    <td><asp:TextBox ID="txtBC" runat="server" AutoPostBack="true" ontextchanged="txt_TextChanged"></asp:TextBox></td>
                </tr>
            </table>
            <asp:GridView ID="GridViewList" runat="server" BorderWidth="1px" BorderStyle="Solid"
            BorderColor="#999999" BackColor="White" OnPageIndexChanging="GridViewList_PageIndexChanging"
            AllowPaging="True" OnRowCommand="GridViewList_RowCommand"
            GridLines="Vertical" ForeColor="Black" CellPadding="3" AutoGenerateColumns="False"
            DataKeyNames="Area,Adhesive,bake_program">
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
                <asp:TemplateField HeaderText="Run" Visible="false">
                    <ItemTemplate>                        
                        <asp:ImageButton ID="btn2" CommandName="Run" OnClientClick="return confirm('Are you sure of comparing this PTN?');"
                                CommandArgument='<%# Eval("Area") +","+ Eval("Adhesive") +","+ Eval("Bake_Program") %>' ImageUrl="../../images/icon_import.png"
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
            
            <br />

            <asp:GridView ID="GridView_Intrack" runat="server" BorderWidth="1px" BorderStyle="Solid"
            BorderColor="#999999" BackColor="White" OnPageIndexChanging="GridViewList_PageIndexChanging"
            AllowPaging="True" OnRowCommand="GridViewList_RowCommand"
            GridLines="Vertical" ForeColor="Black" CellPadding="3" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="No">
                    <ItemTemplate>
                        <%# (Container.DataItemIndex+1).ToString()%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>                
                <asp:BoundField DataField="Batch" HeaderText="Batch" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="Type" HeaderText="Type" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="Package" HeaderText="Package" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="Adhesive" HeaderText="Adhesive" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="Adhesive2" HeaderText="Adhesive2" ReadOnly="True"></asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC"></FooterStyle>
            <PagerStyle HorizontalAlign="Center" BackColor="#999999" ForeColor="Black"></PagerStyle>
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White"></HeaderStyle>
            <AlternatingRowStyle BackColor="#CCCCCC"></AlternatingRowStyle>
        </asp:GridView>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

