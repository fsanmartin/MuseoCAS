<%@ Page Title="Usuarios del Sistema" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWUsers.aspx.vb" Inherits="Security_VWUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <h1>Usuarios del sistema</h1>
    <table style="width: 40%">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
            </td>
            <td><a href="AddUser.aspx"><img src="../Icons/new.png" style="width: 24px; height: 24px" /></a></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="Labels" Text="Buscar:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Buscar" PostBackUrl="~/Security/VWUsers.aspx" />
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="UserId" DataSourceID="dsUsers" ForeColor="#333333" GridLines="None" Width="202px">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:ButtonField ButtonType="Image" CommandName="btnEdit" ImageUrl="~/Icons/edit.png" Text="Editar" />
            <asp:HyperLinkField DataNavigateUrlFields="UserID" DataNavigateUrlFormatString="ModifyUser.aspx?ID={0}&amp;Mode=VIEW" HeaderText="Nombre de Usuario" SortExpression="UserName" DataTextField="UserName" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
<asp:SqlDataSource ID="dsUsers" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" SelectCommand="SELECT [UserName], [UserId] FROM [Users] WHERE ([UserName] LIKE '%' + @UserName + '%') ORDER BY [UserName]">
    <SelectParameters>
        <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="UserName" PropertyName="Text" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
</asp:Content>

