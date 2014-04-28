<%@ Page Title="Tabla de Códigos Generalizados" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWCodigos.aspx.vb" Inherits="Forms_VWCodigos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .textbox {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <h1>Tabla de Códigos Generalizados</h1>
    <table style="width: 40%">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
            </td>
            <td> 
                <asp:ImageButton ID="btnNew" runat="server" CommandName="btnNew" ImageUrl="~/Icons/new.png" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="Labels" Text="Tabla:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="cboCodigo" runat="server" AutoPostBack="True" CssClass="textbox" DataSourceID="SqlDataSource1" DataTextField="cod_cod" DataValueField="cod_cod" Height="16px" Width="171px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>


    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="cod_id" DataSourceID="SqlDataSource2" Width="525px" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="btnEdit" ImageUrl="~/Icons/edit.png" Text="Botón" CommandArgument='<%# Container.DataItemIndex %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="cod_id" HeaderText="cod_id" InsertVisible="False" ReadOnly="True" SortExpression="cod_id" Visible="False" />
            <asp:TemplateField HeaderText="Valor">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("cod_id", "MCodigos.aspx?ID={0}&Mode=View")%>' Text='<%# Eval("cod_val")%>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" CommandName="Delete" ImageUrl="~/Icons/delete.png" Text="Eliminar" />
                </ItemTemplate>
            </asp:TemplateField>
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
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" DeleteCommand="DELETE FROM [CODIGOS] WHERE [cod_id] = @cod_id" InsertCommand="INSERT INTO [CODIGOS] ([cod_name], [cod_cod], [cod_val], [DELETE_], [INSERT_], [UPDATE_], [USERID_]) VALUES (@cod_name, @cod_cod, @cod_val, @DELETE_, @INSERT_, @UPDATE_, @USERID_)" SelectCommand="SELECT * FROM [CODIGOS] WHERE ([cod_name] = @cod_name)" UpdateCommand="UPDATE [CODIGOS] SET [cod_name] = @cod_name, [cod_cod] = @cod_cod, [cod_val] = @cod_val, [DELETE_] = @DELETE_, [INSERT_] = @INSERT_, [UPDATE_] = @UPDATE_, [USERID_] = @USERID_ WHERE [cod_id] = @cod_id">
        <DeleteParameters>
            <asp:Parameter Name="cod_id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="cod_name" Type="String" />
            <asp:Parameter Name="cod_cod" Type="String" />
            <asp:Parameter Name="cod_val" Type="String" />
            <asp:Parameter Name="DELETE_" Type="String" />
            <asp:Parameter Name="INSERT_" Type="DateTime" />
            <asp:Parameter Name="UPDATE_" Type="DateTime" />
            <asp:Parameter Name="USERID_" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="cboCodigo" Name="cod_name" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="cod_name" Type="String" />
            <asp:Parameter Name="cod_cod" Type="String" />
            <asp:Parameter Name="cod_val" Type="String" />
            <asp:Parameter Name="DELETE_" Type="String" />
            <asp:Parameter Name="INSERT_" Type="DateTime" />
            <asp:Parameter Name="UPDATE_" Type="DateTime" />
            <asp:Parameter Name="USERID_" Type="String" />
            <asp:Parameter Name="cod_id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" SelectCommand="SELECT DISTINCT [cod_cod] FROM [CODIGOS] WHERE (([DELETE_] &lt;&gt; @DELETE_) AND ([cod_name] = @cod_name)) ORDER BY [cod_cod]">
        <SelectParameters>
            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
            <asp:Parameter DefaultValue="__TABLES" Name="cod_name" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>


</asp:Content>

