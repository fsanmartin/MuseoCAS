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
    <asp:GridView ID="grdGrupos" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSource1" Width="542px" DataKeyNames="grp_id">
        <Columns>
            <asp:ButtonField ButtonType="Image" ImageUrl="~/Icons/edit.png" Text="Editar" CommandName="btnEdit" />
            <asp:BoundField DataField="grp_id" HeaderText="grp_id" SortExpression="grp_id" InsertVisible="False" ReadOnly="True" Visible="False" />
            <asp:HyperLinkField DataNavigateUrlFields="grp_id" DataNavigateUrlFormatString="MGrupos.aspx?ID={0}&amp;Mode=View" DataTextField="grp_name" HeaderText="Grupo" SortExpression="grp_name" />
            <asp:BoundField DataField="grp_desc" HeaderText="Descripción" SortExpression="grp_desc" />
        </Columns>
        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
        <RowStyle BackColor="White" ForeColor="#003399" />
        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
        <SortedAscendingCellStyle BackColor="#EDF6F6" />
        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
        <SortedDescendingCellStyle BackColor="#D6DFDF" />
        <SortedDescendingHeaderStyle BackColor="#002876" />
                </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" SelectCommand="SELECT [grp_id], [grp_name], [grp_desc] FROM [GRUPOS] WHERE (([DELETE_] &lt;&gt; @DELETE_) AND ([grp_name] LIKE '%' + @grp_name + '%') OR ([grp_desc] LIKE '%' + @grp_desc + '%')) ORDER BY [grp_name]">
        <SelectParameters>
            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="grp_name" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="grp_desc" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

