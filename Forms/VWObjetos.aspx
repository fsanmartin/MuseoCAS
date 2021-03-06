﻿<%@ Page Title="Ficha para Registro de Objetos" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWObjetos.aspx.vb" Inherits="Forms_frmVWObjetos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .textbox {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="Formulario">
        <h1>Ficha para Registro de Objetos</h1>
        <table style="width: 40%">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
                </td>
                <td>
                    <a href="MObjetos.aspx"><img src="../Icons/new.png" style="width: 24px; height: 24px" /></a></td>
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
                    <asp:Button ID="Button1" runat="server" Text="Buscar" PostBackUrl="~/Forms/VWObjetos.aspx" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="obj_id" DataSourceID="dsObjetos" CellPadding="4" Width="536px" ForeColor="#333333" GridLines="None" PageSize="30">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="obj_id" Visible="False" />
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
                            <asp:TemplateField HeaderText="N° Inventario" SortExpression="obj_numero">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("obj_id", "MObjetos.aspx?ID={0}&Mode=View")%>' Text='<%# Eval("obj_numero")%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Título" SortExpression="obj_titulo">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("obj_titulo")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="obj_resp_nombre" HeaderText="Responsable" SortExpression="obj_resp_nombre" />
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
                    <asp:SqlDataSource ID="dsObjetos" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" 
                        SelectCommand="SELECT [obj_id], [obj_numero], [obj_titulo], [obj_resp_nombre], [obj_resp_fecha], [DELETE_], [INSERT_], [UPDATE_], [USERID_] FROM [OBJETOS] WHERE (([DELETE_] &lt;&gt; @DELETE_) AND ([obj_titulo] LIKE '%' + @obj_titulo + '%' OR [obj_descripcion_fisica] LIKE '%' + @obj_descripcion_fisica + '%' OR [obj_observaciones] LIKE '%' + @obj_observaciones + '%' OR [obj_palabra_clave] LIKE '%' + @obj_palabra_clave + '%')) ORDER BY [obj_numero]" 
                        DeleteCommand="DELETE FROM [OBJETOS] WHERE [obj_id] = @obj_id" 
                        InsertCommand="INSERT INTO [OBJETOS] ([obj_numero], [obj_titulo], [obj_resp_nombre], [obj_resp_fecha], [DELETE_], [INSERT_], [UPDATE_], [USERID_]) VALUES (@obj_numero, @obj_titulo, @obj_resp_nombre, @obj_resp_fecha, @DELETE_, @INSERT_, @UPDATE_, @USERID_)" 
                        UpdateCommand="UPDATE [OBJETOS] SET [obj_numero] = @obj_numero, [obj_titulo] = @obj_titulo, [obj_resp_nombre] = @obj_resp_nombre, [obj_resp_fecha] = @obj_resp_fecha, [DELETE_] = @DELETE_, [INSERT_] = @INSERT_, [UPDATE_] = @UPDATE_, [USERID_] = @USERID_ WHERE [obj_id] = @obj_id">
                        <DeleteParameters>
                            <asp:Parameter Name="obj_id" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="obj_numero" Type="String" />
                            <asp:Parameter Name="obj_titulo" Type="String" />
                            <asp:Parameter Name="obj_resp_nombre" Type="String" />
                            <asp:Parameter DbType="Date" Name="obj_resp_fecha" />
                            <asp:Parameter Name="DELETE_" Type="String" />
                            <asp:Parameter Name="INSERT_" Type="DateTime" />
                            <asp:Parameter Name="UPDATE_" Type="DateTime" />
                            <asp:Parameter Name="USERID_" Type="String" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="obj_titulo" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="obj_descripcion_fisica" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="obj_observaciones" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="obj_palabra_clave" PropertyName="Text" Type="String" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="obj_numero" Type="String" />
                            <asp:Parameter Name="obj_titulo" Type="String" />
                            <asp:Parameter Name="obj_resp_nombre" Type="String" />
                            <asp:Parameter DbType="Date" Name="obj_resp_fecha" />
                            <asp:Parameter Name="DELETE_" Type="String" />
                            <asp:Parameter Name="INSERT_" Type="DateTime" />
                            <asp:Parameter Name="UPDATE_" Type="DateTime" />
                            <asp:Parameter Name="USERID_" Type="String" />
                            <asp:Parameter Name="obj_id" Type="Int32" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

