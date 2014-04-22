<%@ Page Title="Maestro de Códigos" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="_MCodigos.aspx.vb" Inherits="Forms_MCodigos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style2 {
            width: 40%;
        }
        .auto-style3 {
            width: 36px;
            height: 36px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <h1>Tabla de Códigos Generalizados</h1>
    <table class="auto-style2">
        <tr>
            <td colspan="3" style="text-align: right">
                <a href="VWCodigos.aspx"><img alt="Volver" class="auto-style3" src="../Icons/return_32.png" /></a>
            </td>
        </tr>
    </table>
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="cod_id" DataSourceID="SqlDataSource1" DefaultMode="Insert" Height="50px" Width="212px">
            <Fields>
                <asp:BoundField DataField="cod_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="cod_id" />
                <asp:TemplateField HeaderText="Tabla:" SortExpression="cod_name">
                    <EditItemTemplate>
<%--                        <asp:TextBox ID="TextBox1" CssClass="textbox" runat="server" Text='<%# Bind("cod_name") %>'></asp:TextBox>--%>
                        <asp:DropDownList ID="DropDownList1" CssClass="textbox" runat="server" DataSourceID="dsTabla" DataTextField="cod_name" DataValueField="cod_name" SelectedValue='<%# Bind("cod_name") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
<%--                        <asp:TextBox ID="TextBox1" CssClass="textbox" runat="server" Text='<%# Bind("cod_name") %>'></asp:TextBox>--%>
                        <asp:DropDownList ID="DropDownList1" CssClass="textbox" runat="server" DataSourceID="dsTabla" DataTextField="cod_name" DataValueField="cod_name" SelectedValue='<%# Bind("cod_name") %>' AutoPostBack="True">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" CssClass="Labels" runat="server" Text='<%# Bind("cod_name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Código:" SortExpression="cod_cod" InsertVisible="False" Visible="False">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" CssClass="textbox" runat="server" Text='<%# Bind("cod_cod") %>' OnLoad="TextBox2_Load1"></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox2" CssClass="textbox" runat="server" Text='<%# Bind("cod_cod") %>'></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" CssClass="Labels" runat="server" Text='<%# Bind("cod_cod") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor:" SortExpression="cod_val">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" CssClass="textbox" runat="server" Text='<%# Bind("cod_val") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox3" CssClass="textbox" runat="server" Text='<%# Bind("cod_val") %>'></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" CssClass="Labels" runat="server" Text='<%# Bind("cod_val") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DELETE_" HeaderText="DELETE_" SortExpression="DELETE_" Visible="False" />
                <asp:TemplateField HeaderText="Creado:" InsertVisible="False" SortExpression="INSERT_">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("INSERT_") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("INSERT_") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actualizado:" SortExpression="UPDATE_" Visible="False" InsertVisible="False">
                    <EditItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("UPDATE_") %>'></asp:Label>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("UPDATE_") %>'></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("UPDATE_") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Usuario:" InsertVisible="False" SortExpression="USERID_">
                    <EditItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("USERID_") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("USERID_") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ButtonType="Image" CancelImageUrl="~/Icons/cancel.png" DeleteImageUrl="~/Icons/delete_32.png" EditImageUrl="~/Icons/save.png" InsertImageUrl="~/Icons/save.png" ShowEditButton="True" UpdateImageUrl="~/Icons/save.png" ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" 
            DeleteCommand="DELETE FROM [CODIGOS] WHERE [cod_id] = @cod_id" 
            InsertCommand="INSERT INTO [CODIGOS] ([cod_name], [cod_cod], [cod_val], [DELETE_], [INSERT_], [UPDATE_], [USERID_]) VALUES (@cod_name, @cod_cod, @cod_val, @DELETE_, @INSERT_, @UPDATE_, @USERID_)" 
            SelectCommand="SELECT * FROM [CODIGOS] WHERE ([cod_id] = @cod_id)" 
            UpdateCommand="UPDATE [CODIGOS] SET [cod_name] = @cod_name, [cod_cod] = @cod_cod, [cod_val] = @cod_val, [DELETE_] = @DELETE_, [UPDATE_] = @UPDATE_, [USERID_] = @USERID_ WHERE [cod_id] = @cod_id">
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
                <asp:QueryStringParameter Name="cod_id" QueryStringField="ID" Type="Int32" />
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
        <asp:SqlDataSource ID="dsTabla" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" SelectCommand="SELECT DISTINCT [cod_name] FROM [CODIGOS] WHERE ([DELETE_] &lt;&gt; @DELETE_) ORDER BY [cod_name]">
            <SelectParameters>
                <asp:Parameter DefaultValue="*" Name="DELETE_" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
</asp:Content>

