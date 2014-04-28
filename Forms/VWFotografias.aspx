<%@ Page Title="Ficha para Registro de Fotografías" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWFotografias.aspx.vb" Inherits="Forms_frmVWFotografias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .textbox {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="Formulario">
        <h1>Ficha para Registro de Fotografías</h1>
        <table style="width: 40%">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
                </td>
                <td>
                    <a href="MFotografias.aspx"><img src="../Icons/new.png" style="width: 24px; height: 24px" /></a></td>
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
                    <asp:Button ID="Button1" runat="server" Text="Buscar" PostBackUrl="~/Forms/VWFotografias.aspx" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="fot_id" DataSourceID="dsFotografias" CellPadding="4" Width="536px" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="fot_id" Visible="False" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="btnEdit" ImageUrl="~/Icons/edit.png" Text="Editar" CommandArgument='<%# Container.DataItemIndex %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnImage" runat="server" CausesValidation="false" CommandName="btnImage" ImageUrl="~/Icons/photo.png" Text="Imagen" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Título" SortExpression="fot_titulo">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("fot_id", "MFotografias.aspx?ID={0}&Mode=View")%>' Text='<%# Eval("fot_titulo") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="fot_numero" HeaderText="Número" SortExpression="fot_numero" />
                            <asp:BoundField DataField="fot_resp_nombre" HeaderText="Responsable" SortExpression="fot_resp_nombre" />
                            <asp:BoundField DataField="DELETE_" HeaderText="DELETE_" SortExpression="DELETE_" Visible="False" />
                            <asp:BoundField DataField="INSERT_" HeaderText="INSERT_" SortExpression="INSERT_" Visible="False" />
                            <asp:BoundField DataField="UPDATE_" HeaderText="UPDATE_" SortExpression="UPDATE_" Visible="False" />
                            <asp:BoundField DataField="USERID_" HeaderText="USERID_" SortExpression="USERID_" Visible="False" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" CommandName="Delete" ImageUrl="~/Icons/delete.png" Text="Eliminar" />
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
                    <asp:SqlDataSource ID="dsFotografias" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" 
                        SelectCommand="SELECT [fot_id], [fot_numero], [fot_titulo], [fot_resp_nombre], [fot_resp_fecha], [DELETE_], [INSERT_], [UPDATE_], [USERID_] FROM [FOTOGRAFIA] WHERE (([DELETE_] &lt;&gt; @DELETE_) AND ([fot_titulo] LIKE '%' + @fot_titulo + '%' OR [fot_descripcion] LIKE '%' + @fot_descripcion + '%' OR [fot_observaciones] LIKE '%' + @fot_observaciones + '%')) ORDER BY [fot_titulo]" 
                        DeleteCommand="DELETE FROM [FOTOGRAFIA] WHERE [fot_id] = @fot_id" 
                        InsertCommand="INSERT INTO [FOTOGRAFIA] ([fot_numero], [fot_titulo], [fot_resp_nombre], [fot_resp_fecha], [DELETE_], [INSERT_], [UPDATE_], [USERID_]) VALUES (@fot_numero, @fot_titulo, @fot_resp_nombre, @fot_resp_fecha, @DELETE_, @INSERT_, @UPDATE_, @USERID_)" 
                        UpdateCommand="UPDATE [FOTOGRAFIA] SET [fot_numero] = @fot_numero, [fot_titulo] = @fot_titulo, [fot_resp_nombre] = @fot_resp_nombre, [fot_resp_fecha] = @fot_resp_fecha, [DELETE_] = @DELETE_, [INSERT_] = @INSERT_, [UPDATE_] = @UPDATE_, [USERID_] = @USERID_ WHERE [fot_id] = @fot_id">
                        <DeleteParameters>
                            <asp:Parameter Name="fot_id" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="fot_numero" Type="String" />
                            <asp:Parameter Name="fot_titulo" Type="String" />
                            <asp:Parameter Name="fot_resp_nombre" Type="String" />
                            <asp:Parameter DbType="Date" Name="fot_resp_fecha" />
                            <asp:Parameter Name="DELETE_" Type="String" />
                            <asp:Parameter Name="INSERT_" Type="DateTime" />
                            <asp:Parameter Name="UPDATE_" Type="DateTime" />
                            <asp:Parameter Name="USERID_" Type="String" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="fot_titulo" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="fot_descripcion" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="fot_observaciones" PropertyName="Text" Type="String" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="fot_numero" Type="String" />
                            <asp:Parameter Name="fot_titulo" Type="String" />
                            <asp:Parameter Name="fot_resp_nombre" Type="String" />
                            <asp:Parameter DbType="Date" Name="fot_resp_fecha" />
                            <asp:Parameter Name="DELETE_" Type="String" />
                            <asp:Parameter Name="INSERT_" Type="DateTime" />
                            <asp:Parameter Name="UPDATE_" Type="DateTime" />
                            <asp:Parameter Name="USERID_" Type="String" />
                            <asp:Parameter Name="fot_id" Type="Int32" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

