<%@ Page Title="Grupos de Acceso" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWGrupos.aspx.vb" Inherits="Forms_MGrupos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <h1>Grupos de Acceso</h1>
    <table style="width: 50%">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
            </td>
            <td>
                <a href="MGrupos.aspx"><img src="../Icons/new.png" style="width: 24px; height: 24px" /></a></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="Labels" Text="Buscar:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="textbox" Width="206px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Buscar" PostBackUrl="~/Forms/frmVWAudiovisual.aspx" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="grdGrupos" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource1" Width="542px" DataKeyNames="grp_id" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:ButtonField ButtonType="Image" ImageUrl="~/Icons/edit.png" Text="Editar" CommandName="btnEdit" />
            <asp:BoundField DataField="grp_id" HeaderText="grp_id" SortExpression="grp_id" InsertVisible="False" ReadOnly="True" Visible="False" />
            <asp:HyperLinkField DataNavigateUrlFields="grp_id" DataNavigateUrlFormatString="MGrupos.aspx?ID={0}&amp;Mode=View" DataTextField="grp_name" HeaderText="Grupo" SortExpression="grp_name" />
            <asp:BoundField DataField="grp_desc" HeaderText="Descripción" SortExpression="grp_desc" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" SelectCommand="SELECT [grp_id], [grp_name], [grp_desc] FROM [GRUPOS] WHERE (([DELETE_] &lt;&gt; @DELETE_) AND ([grp_name] LIKE '%' + @grp_name + '%') OR ([grp_desc] LIKE '%' + @grp_desc + '%')) ORDER BY [grp_name]">
        <SelectParameters>
            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="grp_name" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="grp_desc" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

