<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="PTN_maintain.aspx.cs" Inherits="nModBusWeb.PTN_maintain" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
    <h2>PTN Maintain</h2>
    <table>        
        <tr>
            <td>Area：</td>
            <td><asp:TextBox ID="txtArea" runat="server"></asp:TextBox></td>
            <td>Adhesive：</td>
            <td><asp:TextBox ID="txtAdhesive" runat="server"></asp:TextBox></td>
            <td>Bake Program：</td>
            <td><asp:TextBox ID="txtBakeProgram" runat="server"></asp:TextBox></td>
            <td style="text-align:right;"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
        </tr>
    </table>
    
    
    <asp:Button ID="btnNew" runat="server"  Text="New PTN" OnClick="btnNew_Click" Visible="false" />
    
    <asp:GridView ID="GridViewAM" runat="server" 
            DataKeyNames="Area,Adhesive,Bake_Program" AutoGenerateColumns="False"
                      CellPadding="3" ForeColor="Black" GridLines="Vertical" 
            AllowSorting="True" AllowPaging="True"
            OnSorting="GridViewAM_Sorting" 
            OnRowCancelingEdit="GridViewAM_RowCancelingEdit" 
            OnRowEditing="GridViewAM_RowEditing" 
            OnRowUpdating="GridViewAM_RowUpdating" 
            OnRowDataBound="GridViewAM_RowDataBound"  
            OnPageIndexChanging="GridViewAM_PageIndexChanging" BackColor="White" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
            OnRowCommand="GridViewAM_RowCommand" Width="80%">
            <Columns>                
                <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# (Container.DataItemIndex +1).ToString()%>
                        </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="5%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Area" SortExpression="Area">
                    <%--<EditItemTemplate>
                        <asp:TextBox id="gv_txtArea" Text='<%# Bind("Area") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>--%>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Area") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Adhesive" SortExpression="Adhesive">
                    <%--<EditItemTemplate>
                        <asp:TextBox id="gv_txtAdhesive" Text='<%# Bind("Adhesive") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>--%>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Adhesive") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="25%" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Bake_Program" SortExpression="Bake_Program">
                    <%--<EditItemTemplate>
                        <asp:TextBox id="gv_txtBakeProgram" Text='<%# Bind("Bake_Program") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>--%>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Bake_Program") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="20%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Process_1" SortExpression="Process_1">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtProcess_1" Text='<%# Bind("Process_1") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label001" runat="server" Text='<%# Bind("Process_1") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_1" SortExpression="Hour_1">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtHour_1" Text='<%# Bind("Hour_1") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label002" runat="server" Text='<%# Bind("Hour_1") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Min_1" SortExpression="Min_1">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtMin_1" Text='<%# Bind("Min_1") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label003" runat="server" Text='<%# Bind("Min_1") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Temperature_1" SortExpression="Temperature_1">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtTemperature_1" Text='<%# Bind("Temperature_1") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label004" runat="server" Text='<%# Bind("Temperature_1") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pressure_1" SortExpression="Pressure_1">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtPressure_1" Text='<%# Bind("Pressure_1") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label005" runat="server" Text='<%# Bind("Pressure_1") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Process_2" SortExpression="Process_2">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtProcess_2" Text='<%# Bind("Process_2") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label006" runat="server" Text='<%# Bind("Process_2") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_2" SortExpression="Hour_2">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtHour_2" Text='<%# Bind("Hour_2") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label007" runat="server" Text='<%# Bind("Hour_2") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Min_2" SortExpression="Min_2">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtMin_2" Text='<%# Bind("Min_2") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label008" runat="server" Text='<%# Bind("Min_2") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Temperature_2" SortExpression="Temperature_2">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtTemperature_2" Text='<%# Bind("Temperature_2") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label009" runat="server" Text='<%# Bind("Temperature_2") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pressure_2" SortExpression="Pressure_2">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtPressure_2" Text='<%# Bind("Pressure_2") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label010" runat="server" Text='<%# Bind("Pressure_2") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="BakeTime" SortExpression="BakeTime">
                    <EditItemTemplate>
                        <asp:TextBox id="gv_txtBakeTime" Text='<%# Bind("BakeTime") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("BakeTime") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit" Visible="false">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lbUpdate" runat="server" CommandName="Update">Submit</asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lbCancelUpdate" runat="server" CommandName="Cancel">Cancel</asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btn1" CommandName="Edit" CommandArgument='<%# Eval("Adhesive") %>'
                                ImageUrl="../../images/icon_edit.png" runat="server" Width="18px" ToolTip="Edit"  />                        
                    </ItemTemplate>
                    <HeaderStyle Width="5%" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" Visible="false">
                    <ItemTemplate>                        
                        <asp:ImageButton ID="btn2" CommandName="myDelete" OnClientClick="return confirm('Are you sure of deleteing this tester?');"
                                CommandArgument='<%# Eval("Area") +","+ Eval("Adhesive") +","+ Eval("Bake_Program") %>' ImageUrl="../../images/icon_delete.png"
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
                            <%--<asp:TextBox ID="dv_txtArea" runat="server" Text='<%# Bind("Area") %>'></asp:TextBox>--%>
                            <asp:DropDownList ID="dv_ddlArea" runat="server">
                            </asp:DropDownList>
                        </InsertItemTemplate>
                    </asp:TemplateField>     
                    <asp:TemplateField HeaderText="Adhesive">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtAdhesive" runat="server" Text='<%# Bind("Adhesive") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label02" runat="server" Text='<%# Bind("Adhesive") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                   <asp:TemplateField HeaderText="Bake_Program">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtBakeProgram" runat="server" Text='<%# Bind("Bake_Program") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label03" runat="server" Text='<%# Bind("Bake_Program") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Process_1">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtProcess_1" runat="server" Text='<%# Bind("Process_1") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label04" runat="server" Text='<%# Bind("Process_1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hour_1">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtHour_1" runat="server" Text='<%# Bind("Hour_1") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label05" runat="server" Text='<%# Bind("Hour_1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Min_1">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtMin_1" runat="server" Text='<%# Bind("Min_1") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label06" runat="server" Text='<%# Bind("Min_1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Temperature_1">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtTemperature_1" runat="server" Text='<%# Bind("Temperature_1") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label07" runat="server" Text='<%# Bind("Temperature_1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pressure_1">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtPressure_1" runat="server" Text='<%# Bind("Pressure_1") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label08" runat="server" Text='<%# Bind("Pressure_1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Process_2">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtProcess_2" runat="server" Text='<%# Bind("Process_2") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label09" runat="server" Text='<%# Bind("Process_2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hour_2">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtHour_2" runat="server" Text='<%# Bind("Hour_2") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("Hour_2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Min_2">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtMin_2" runat="server" Text='<%# Bind("Min_2") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label11" runat="server" Text='<%# Bind("Min_2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Temperature_2">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtTemperature_2" runat="server" Text='<%# Bind("Temperature_2") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label12" runat="server" Text='<%# Bind("Temperature_2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pressure_2">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtPressure_2" runat="server" Text='<%# Bind("Pressure_2") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("Pressure_2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="BakeTime">
                        <InsertItemTemplate>
                            <asp:TextBox ID="dv_txtBakeTime" runat="server" Text='<%# Bind("BakeTime") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label010" runat="server" Text='<%# Bind("BakeTime") %>'></asp:Label>
                        </ItemTemplate>
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

