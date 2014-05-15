<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="PressureOvenIn.aspx.cs" Inherits="nModBusWeb.PressureOvenIn" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<script type="text/vbscript" language="vbscript">
sub window_onload()
if form1.IsPlaySound.value = "ok" then
    PlayOKSound()
elseif form1.IsPlaySound.value = "err" then
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
        <div>
            <center>
                <br />
                <asp:Label ID="Label1" runat="server" Text="產品進烤箱 or Plasma" Style="font-size: 16pt;
                    color: #949494"></asp:Label>
                <br />
                <br />
                <font style="font-size: 12pt">請依序輸入: 1. <font color="red">薪號</font> 2. <font color="red">
                    烤箱 or Plasma 編號 (PM Number)</font> 3. <font color="red">PTN (程式)</font> 4. <font
                        color="red">流程卡號 (Batch no.)</font></font>
                <br />
                <br />
                <asp:TextBox ID="txtInData" runat="server" Width="120px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                    ID="btnDataIn" runat="server" Text="Enter" Font-Size="14px" Width="80px" />
                <br />
                <br />
                <table border="1" width="80%">
                    <tr>
                        <td align="center" style="width: 13%; font-size: 12pt">
                            薪 號
                        </td>
                        <td style="width: 12%">
                            <asp:Label ID="lblOPID" runat="server" Text="" Style="width: 16%; font-size: 12pt;
                                color: Fuchsia"></asp:Label>
                        </td>
                        <td align="center" style="width: 13%; font-size: 12pt">
                            區 域
                        </td>
                        <td style="width: 12%">
                            <asp:Label ID="lblArea" runat="server" Text="" Style="width: 16%; font-size: 12pt;
                                color: Fuchsia"></asp:Label>
                        </td>
                        <td align="center" style="width: 13%; font-size: 12pt">
                            Oven ID
                        </td>
                        <td style="width: 12%">
                            <asp:Label ID="lblOvenID" runat="server" Text="" Style="width: 16%; font-size: 12pt;
                                color: Fuchsia"></asp:Label>
                        </td>
                        <td align="center" style="width: 13%; font-size: 12pt">
                            PTN
                        </td>
                        <td style="width: 12%">
                            <asp:Label ID="lblPTN" runat="server" Text="" Style="width: 16%; font-size: 12pt;
                                color: Fuchsia"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Label ID="lblDataInMsg" runat="server" Style="width: 16%; font-family: Arial Unicode MS;
                                font-size: 12pt; color: Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                
                <asp:Label ID="lblPMID" runat="server" Text="" Visible="false"></asp:Label>
            </center>
        </div>
        <asp:TextBox ID="IsPlaySound" runat="server" Style="visibility: hidden"></asp:TextBox>
        <asp:TextBox ID="txtAdhesive" runat="server" Style="visibility: hidden"></asp:TextBox>
        <asp:TextBox ID="txtBC" runat="server" Style="visibility: hidden"></asp:TextBox>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

