<%@ Page Title="Maestro de Usuarios" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="ModifyUser.aspx.vb" Inherits="Security_ModifyUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .textbox {}
        .LabelsError {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <script type="text/javascript">
        function MinLength(sender, args) {
            args.IsValid = (args.Value.length >= 6);
        }
    </script>
    <h1>
            <a href="VWUsers.aspx"><img alt="Volver" class="auto-style3" src="../Icons/return_32.png" /></a>&nbsp; Maestro de Usuarios</h1>
    <table style="font-family:Verdana;font-size:100%;width:351px;">
    <tr>
        <td colspan="4" style="color:Red;">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="LabelsError" HeaderText="Debe ingresar datos en los siguientes campos obligatorios:" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="La Contraseña debe tener al menos 6 caracteres." ControlToValidate="Password" CssClass="LabelsError" ClientValidationFunction="MinLength"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="Labels">Nombre de usuario:</asp:Label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="UserName" runat="server" CssClass="textbox" ReadOnly="True"></asp:TextBox>
            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="El nombre de usuario es obligatorio." ToolTip="El nombre de usuario es obligatorio." CssClass="LabelsError">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="Labels">Contraseña:</asp:Label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="La contraseña es obligatoria." ToolTip="La contraseña es obligatoria." CssClass="LabelsError">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword" CssClass="Labels">Confirmar contraseña:</asp:Label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="Confirmar contraseña es obligatorio." ToolTip="Confirmar contraseña es obligatorio." CssClass="LabelsError">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" CssClass="Labels">Correo electrónico:</asp:Label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="Email" runat="server" CssClass="textbox" Width="208px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="El correo electrónico es obligatorio." ToolTip="El correo electrónico es obligatorio." CssClass="LabelsError">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="GrupoLabel" runat="server" AssociatedControlID="cblGrupos" CssClass="Labels">Grupo de acceso:</asp:Label>
        </td>
        <td colspan="2">
            <asp:CheckBoxList ID="cblGrupos" runat="server">
            </asp:CheckBoxList>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="4">
            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="Contraseña y Confirmar contraseña deben coincidir." CssClass="LabelsError"></asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="4" style="color:Red;">
            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td align="center" style="color:Red;">
                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" AlternateText="Grabar" ImageUrl="~/Icons/save.png" ToolTip="Grabar registro" />
        </td>
        <td align="center" colspan="2" style="color:Red;">
                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Icons/edit_32.png" ToolTip="Modificar registro" />
        </td>
        <td align="center" style="color:Red;">
                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Icons/delete_32.png" ToolTip="Eliminar registro" />
        </td>
    </tr>
    </table>
</asp:Content>

