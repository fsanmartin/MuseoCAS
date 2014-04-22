<%@ Page Title="Maestro de Categorías" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="MCategoria.aspx.vb" Inherits="Forms_frmMNTCodigos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <h1>Maestro de Categorías</h1>
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="cat_id" DataSourceID="dsCodigos" Height="50px" Width="125px" DefaultMode="Insert">
        <Fields>
            <asp:BoundField DataField="cat_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="cat_id" />
            <asp:TemplateField HeaderText="Título" SortExpression="cat_title">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" CssClass="textbox" runat="server" Text='<%# Bind("cat_title")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TextBox1" runat="server" CssClass="LabelsAlert" ErrorMessage="RequiredFieldValidator">(*) Debe ingresar Título</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox2" CssClass="Labels" runat="server" Text='<%# Bind("cat_title")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TextBox2" runat="server" CssClass="LabelsAlert" ErrorMessage="RequiredFieldValidator">(*) Debe ingresar Título</asp:RequiredFieldValidator>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("cat_title") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripción" SortExpression="cat_desc">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox3" CssClass="textbox" TextMode="MultiLine" Width="500" Height="100" runat="server" Text='<%# Bind("cat_desc")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="TextBox3" runat="server" CssClass="LabelsAlert" ErrorMessage="RequiredFieldValidator">(*) Debe ingresar Descripción</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="TextBox4" CssClass="textbox" TextMode="MultiLine" Width="500" Height="100" runat="server" Text='<%# Bind("cat_desc")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="TextBox4" runat="server" CssClass="LabelsAlert" ErrorMessage="RequiredFieldValidator">(*) Debe ingresar Descripción</asp:RequiredFieldValidator>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("cat_desc") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="INSERT_" HeaderText="Creación" SortExpression="INSERT_" ReadOnly="True" InsertVisible="False" />
            <asp:BoundField DataField="DELETE_" HeaderText="DELETE_" SortExpression="DELETE_" Visible="False" />
            <asp:BoundField DataField="UPDATE_" HeaderText="Modificación" SortExpression="UPDATE_" InsertVisible="False" ReadOnly="True" />
            <asp:BoundField DataField="USERID_" HeaderText="USERID_" SortExpression="USERID_" Visible="False" />
            <asp:CommandField ButtonType="Image" CancelImageUrl="~/Icons/cancel.png" DeleteImageUrl="~/Icons/delete.png" EditImageUrl="~/Icons/edit.png" InsertImageUrl="~/Icons/save.png" NewImageUrl="~/Icons/save.png" ShowDeleteButton="True" ShowEditButton="True" ShowInsertButton="True" UpdateImageUrl="~/Icons/edit.png" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="dsCodigos" runat="server" ConnectionString="<%$ ConnectionStrings:ColegioCN %>" DeleteCommand="UPDATE [CATEGORIA] SET [DELETE_] = '*', [UPDATE_] = @UPDATE_, [USERID_] = @USERID_ WHERE [cat_id] = @cat_id" InsertCommand="INSERT INTO [CATEGORIA] ([cat_title], [cat_desc], [INSERT_], [DELETE_], [UPDATE_], [USERID_]) VALUES (@cat_title, @cat_desc, @INSERT_, @DELETE_, @UPDATE_, @USERID_)" SelectCommand="SELECT [cat_id], [cat_title], [cat_desc], [INSERT_], [DELETE_], [UPDATE_], [USERID_] FROM [CATEGORIA] WHERE ([cat_id] = @cat_id)" UpdateCommand="UPDATE [CATEGORIA] SET [cat_title] = @cat_title, [cat_desc] = @cat_desc, [INSERT_] = @INSERT_, [DELETE_] = @DELETE_, [UPDATE_] = @UPDATE_, [USERID_] = @USERID_ WHERE [cat_id] = @cat_id">
        <DeleteParameters>
            <asp:Parameter Name="cat_id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="cat_title" Type="String" />
            <asp:Parameter Name="cat_desc" Type="String" />
            <asp:Parameter Name="INSERT_" Type="DateTime" />
            <asp:Parameter Name="DELETE_" Type="String" />
            <asp:Parameter Name="UPDATE_" Type="DateTime" />
            <asp:Parameter Name="USERID_" Type="String" />
        </InsertParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="cat_id" QueryStringField="ID" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="cat_title" Type="String" />
            <asp:Parameter Name="cat_desc" Type="String" />
            <asp:Parameter Name="INSERT_" Type="DateTime" />
            <asp:Parameter Name="DELETE_" Type="String" />
            <asp:Parameter Name="UPDATE_" Type="DateTime" />
            <asp:Parameter Name="USERID_" Type="String" />
            <asp:Parameter Name="cat_id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

