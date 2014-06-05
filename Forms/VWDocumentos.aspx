<%@ Page Title="Ficha para Registro de Documentos" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWDocumentos.aspx.vb" Inherits="Forms_frmVWAudiovisual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .textbox {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="Formulario">
        <h1>Ficha para Registro de Documentos</h1>
        <table style="width: 40%">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
                </td>
                <td>
                    <a href="MDocumentos.aspx"><img src="../Icons/new.png" style="width: 24px; height: 24px" /></a></td>
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
                    <asp:Button ID="Button1" runat="server" Text="Buscar" PostBackUrl="~/Forms/VWDocumentos.aspx" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="doc_id" DataSourceID="dsDocumentos" CellPadding="4" Width="536px" ForeColor="#333333" GridLines="None" PageSize="30">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="btnEdit" ImageUrl="~/Icons/edit.png" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnImage" runat="server" CausesValidation="false" CommandName="btnImage" ImageUrl="~/Icons/photo.png" Text="Imagen" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="doc_id" HeaderText="doc_id" InsertVisible="False" ReadOnly="True" SortExpression="doc_id" Visible="False" />
                            <asp:TemplateField HeaderText="N° Inventario" SortExpression="doc_numero">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("doc_id", "MDocumentos.aspx?ID={0}&Mode=View")%>' Text='<%# Eval("doc_numero")%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Título" SortExpression="doc_titulo">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("doc_titulo")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="doc_resp_nombre" HeaderText="Responsable" SortExpression="doc_resp_nombre" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" CommandName="btnDelete" ImageUrl="~/Icons/delete.png" CommandArgument='<%# Container.DataItemIndex%>' />
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
                    <asp:SqlDataSource ID="dsDocumentos" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" 
                        SelectCommand="SELECT * FROM [DOCUMENTOS] WHERE (([DELETE_] <> @DELETE_) AND ([doc_titulo] LIKE '%' + @doc_titulo + '%' OR [doc_descripcion_fisica] LIKE '%' + @doc_descripcion_fisica + '%' OR [doc_observaciones] LIKE '%' + @doc_observaciones + '%' OR [doc_palabra_clave] LIKE '%' + @doc_palabra_clave + '%')) ORDER BY [doc_numero]">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="doc_titulo" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="doc_descripcion_fisica" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="doc_observaciones" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%" Name="doc_palabra_clave" PropertyName="Text" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

