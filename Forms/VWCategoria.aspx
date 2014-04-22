<%@ Page Title="Maestro de Categorías" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWCategoria.aspx.vb" Inherits="Forms_frmCodigos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <h1>Maestro de Categorías</h1>
    <table style="width: 40%">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
            </td>
            <td>
                <a href="MCategoria.aspx"><img src="../Icons/new.png" style="width: 24px; height: 24px" /></a></td>
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
                <asp:Button ID="Button1" runat="server" Text="Buscar" PostBackUrl="~/Forms/VWCategoria.aspx" />
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" DataKeyNames="cat_id" DataSourceID="dsCodigos" Width="589px" CellPadding="4" ForeColor="#333333" GridLines="None">
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
        <Columns>
            <asp:BoundField DataField="cat_id" HeaderText="ID" SortExpression="cat_id" InsertVisible="False" ReadOnly="True" Visible="False" />
            <asp:HyperLinkField DataNavigateUrlFields="cat_id" DataNavigateUrlFormatString="MCategoria.aspx?ID={0}" DataTextField="cat_title" HeaderText="Título" SortExpression="cat_title" />
            <asp:BoundField DataField="cat_desc" HeaderText="Descripción" SortExpression="cat_desc" />
            <asp:BoundField DataField="UPDATE_" HeaderText="UPDATE_" SortExpression="UPDATE_" Visible="False" />
            <asp:BoundField DataField="USERID_" HeaderText="USERID_" SortExpression="USERID_" Visible="False" />
            <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Icons/delete.png" ShowDeleteButton="True" />
        </Columns>

        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center"></PagerStyle>
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
    <asp:SqlDataSource ID="dsCodigos" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" 
        DeleteCommand="UPDATE [CATEGORIA] SET [DELETE_] = '*', [UPDATE_] = getdate(), [USERID_] = 'fsanmartin' WHERE [cat_id] = @cat_id" 
        InsertCommand="INSERT INTO [CATEGORIA] ([cat_title], [cat_desc]) VALUES (@cat_title, @cat_desc)" 
        SelectCommand="SELECT [cat_id], [cat_title], [cat_desc], [UPDATE_], [USERID_] FROM [CATEGORIA] WHERE (([DELETE_] &lt;&gt; @DELETE_) AND (([cat_title] LIKE '%' + @cat_title + '%') OR ([cat_desc] LIKE '%' + @cat_desc + '%')))" 
        UpdateCommand="UPDATE [CATEGORIA] SET [cat_title] = @cat_title, [cat_desc] = @cat_desc WHERE [cat_id] = @cat_id">
        <DeleteParameters>
            <asp:Parameter Name="UPDATE_" Type="DateTime" />
            <asp:Parameter Name="USERID_" Type="String" />
            <asp:Parameter Name="cat_id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="cat_title" Type="String" />
            <asp:Parameter Name="cat_desc" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
            <asp:ControlParameter ControlID="txtSearch" DefaultValue="" Name="cat_title" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtSearch" Name="cat_desc" PropertyName="Text" Type="String" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="cat_title" Type="String" />
            <asp:Parameter Name="cat_desc" Type="String" />
            <asp:Parameter Name="cat_id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

