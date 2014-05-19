<%@ Page Title="Ficha para Colección Audiovisual" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="VWAudiovisual.aspx.vb" Inherits="Forms_frmVWAudiovisual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .textbox {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <div id="Formulario">
        <h1>Ficha para Colección Audiovisual</h1>
        <table style="width: 40%">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Ingresar Nuevo:"></asp:Label>
                </td>
                <td>
                    <a href="MAudiovisual.aspx"><img src="../Icons/new.png" style="width: 24px; height: 24px" /></a></td>
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
                    <asp:Button ID="Button1" runat="server" Text="Buscar" PostBackUrl="~/Forms/VWAudiovisual.aspx" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="adv_id" DataSourceID="dsAudiovisual" CellPadding="4" Width="536px" ForeColor="#333333" GridLines="None" PageSize="30">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="adv_id" Visible="False" />
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
                            <asp:TemplateField HeaderText="Título" SortExpression="adv_titulo">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("adv_id", "MAudiovisual.aspx?ID={0}&Mode=View") %>' Text='<%# Eval("adv_titulo") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="adv_numero" HeaderText="Nro. de Inventario" SortExpression="adv_numero" />
                            <asp:BoundField DataField="adv_resp_nombre" HeaderText="Responsable" SortExpression="adv_resp_nombre" />
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
                    <asp:SqlDataSource ID="dsAudiovisual" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" 
                        SelectCommand="SELECT [adv_id], [adv_numero], [adv_titulo], [adv_resp_nombre], [adv_resp_fecha], [DELETE_], [INSERT_], [UPDATE_], [USERID_] FROM [AUDIOVISUAL] WHERE (([DELETE_] &lt;&gt; @DELETE_) AND ([adv_titulo] LIKE '%' + @adv_titulo + '%' OR [adv_contenido] LIKE '%' + @adv_contenido + '%' OR [adv_descripcion_fisica] LIKE '%' + @adv_descripcion_fisica + '%' OR [adv_observaciones] LIKE '%' + @adv_observaciones + '%' OR [adv_palabra_clave] LIKE '%' + @adv_palabra_clave + '%')) ORDER BY [adv_numero]" 
                        DeleteCommand="DELETE FROM [AUDIOVISUAL] WHERE [adv_id] = @adv_id" 
                        InsertCommand="INSERT INTO [AUDIOVISUAL] ([adv_numero], [adv_titulo], [adv_resp_nombre], [adv_resp_fecha], [DELETE_], [INSERT_], [UPDATE_], [USERID_]) VALUES (@adv_numero, @adv_titulo, @adv_resp_nombre, @adv_resp_fecha, @DELETE_, @INSERT_, @UPDATE_, @USERID_)" 
                        UpdateCommand="UPDATE [AUDIOVISUAL] SET [adv_numero] = @adv_numero, [adv_titulo] = @adv_titulo, [adv_resp_nombre] = @adv_resp_nombre, [adv_resp_fecha] = @adv_resp_fecha, [DELETE_] = @DELETE_, [INSERT_] = @INSERT_, [UPDATE_] = @UPDATE_, [USERID_] = @USERID_ WHERE [adv_id] = @adv_id">
                        <DeleteParameters>
                            <asp:Parameter Name="adv_id" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="adv_numero" Type="String" />
                            <asp:Parameter Name="adv_titulo" Type="String" />
                            <asp:Parameter Name="adv_resp_nombre" Type="String" />
                            <asp:Parameter DbType="Date" Name="adv_resp_fecha" />
                            <asp:Parameter Name="DELETE_" Type="String" />
                            <asp:Parameter Name="INSERT_" Type="DateTime" />
                            <asp:Parameter Name="UPDATE_" Type="DateTime" />
                            <asp:Parameter Name="USERID_" Type="String" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="adv_titulo" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="adv_contenido" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="adv_descripcion_fisica" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="adv_observaciones" PropertyName="Text" Type="String" />
                            <asp:ControlParameter ControlID="txtSearch" DefaultValue="%%" Name="adv_palabra_clave" PropertyName="Text" Type="String" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="adv_numero" Type="String" />
                            <asp:Parameter Name="adv_titulo" Type="String" />
                            <asp:Parameter Name="adv_resp_nombre" Type="String" />
                            <asp:Parameter DbType="Date" Name="adv_resp_fecha" />
                            <asp:Parameter Name="DELETE_" Type="String" />
                            <asp:Parameter Name="INSERT_" Type="DateTime" />
                            <asp:Parameter Name="UPDATE_" Type="DateTime" />
                            <asp:Parameter Name="USERID_" Type="String" />
                            <asp:Parameter Name="adv_id" Type="Int32" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

